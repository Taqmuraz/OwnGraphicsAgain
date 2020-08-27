using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System;
using EnginePart;
using System.IO;

namespace OwnGraphicsAgain
{

	public sealed class DrawPanel : PictureBox, IConstructorOwner<int, int>
	{
		private Bitmap bitmap;
		private int[] pixelsBuffer;
		private int[] pixelsBufferClear;
		private float[] depthBuffer;
		private float[] depthBufferClear;
		private GCHandle bufferHandle;
		private int width, height;

		int LARGE_TRIANGLE_SQUARE;
		int FAST_DRAW_PIXEL_SIZE = 3;

		private class BakedTriangleData : IFragmentShaderData
		{
			Matrix4x4 normals;
			Matrix4x4 vertices;
			Matrix4x4 uvs;
			public VertexData rawV0;
			public VertexData rawV1;
			public VertexData rawV2;

			public BakedTriangleData()
			{
			}

			public void SetData (VertexData v0, VertexData v1, VertexData v2)
			{
				Matrix4x4 matrix = new Matrix4x4();

				matrix.column_0 = v0.vertex;
				matrix.column_1 = v1.vertex;
				matrix.column_2 = v2.vertex;
				vertices = matrix;

				matrix.column_0 = (Vector3)v0.uv;
				matrix.column_1 = (Vector3)v1.uv;
				matrix.column_2 = (Vector3)v2.uv;
				uvs = matrix;

				matrix.column_0 = v0.normal;
				matrix.column_1 = v1.normal;
				matrix.column_2 = v2.normal;
				normals = matrix;

				rawV0 = v0;
				rawV1 = v1;
				rawV2 = v2;
			}

			public Vector3 GetVertex(ref Vector3 barycentric)
			{
				return vertices.MultiplyVector(barycentric);
			}

			public Vector2 GetUV(ref Vector3 barycentric)
			{
				return (Vector2)uvs.MultiplyVector(barycentric);
			}

			public Vector3 GetNormal(ref Vector3 barycentric)
			{
				return normals.MultiplyVector(barycentric);
			}
		}

		private class VertexData : IVertexShaderData
		{
			public Vector3 vertex;
			public Vector3 normal;
			public Vector2 uv;

			public VertexData()
			{
			}

			public void SetData(Vector3 vertex, Vector3 normal, Vector2 uv)
			{
				this.vertex = vertex;
				this.normal = normal;
				this.uv = uv;
			}

			Vector3 IVertexShaderData.normal { get => normal; set => normal = value; }
			Vector3 IVertexShaderData.vertex { get => vertex; set => vertex = value; }
			Vector2 IVertexShaderData.uv { get => uv; set => uv = value; }
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				bitmap.Dispose();
				bufferHandle.Free();
			}
		}

		public Vector2 imageSize => new Vector2(width, height);
		public Vector2Int imageSizeInt => new Vector2Int(width, height);

		private void FillZone(int startX, int startY, int sizeX, int sizeY, float depth, Color color)
		{
			for (int x = 0; x < sizeX; x++)
				for (int y = 0; y < sizeY; y++)
					WritePixel(startX + x, startY + y, depth, color);
		}

		public void WritePixel(int x, int y, float depth, Color color)
		{
			y = (height - 1) - y;

			int ptr = y * width + x;
			if (ptr < 0 || ptr >= pixelsBuffer.Length) return;
			if (depth <= depthBuffer[ptr]) return;

			pixelsBuffer[ptr] = color.ToArgb();
			depthBuffer[ptr] = depth;
		}

		public Color ReadPixel(int x, int y)
		{
			int ptr = y * width + x;
			return Color.FromArgb(pixelsBuffer[ptr]);
		}

		public void CallConstructor(int width, int height)
		{
			this.width = width;
			this.height = height;

			LARGE_TRIANGLE_SQUARE = width * height >> 2;

			SetBounds(0, 0, width, height);

			pixelsBuffer = new int[width * height];
			depthBuffer = new float[width * height];
			pixelsBufferClear = new int[width * height];
			depthBufferClear = new float[width * height];

			for (int i = 0; i < width * height; i++)
			{
				depthBufferClear[i] = float.NegativeInfinity;
				pixelsBufferClear[i] = Color.LightSkyBlue.ToArgb();
			}

			bufferHandle = GCHandle.Alloc(pixelsBuffer, GCHandleType.Pinned);
			bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, bufferHandle.AddrOfPinnedObject());
			Image = bitmap;
		}

		public void Clear()
		{
			Array.Copy(depthBufferClear, depthBuffer, width * height);
			Array.Copy(pixelsBufferClear, pixelsBuffer, width * height);
			//Array.Clear(depthBuffer, 0, width * height);
			//Array.Clear(pixelsBuffer, 0, width * height);
		}

		private void DrawTriangle_Fast_But_Low_Quality(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, BakedTriangleData data, Material material)
		{
			Vector3 cross = Vector3.Cross(v0 - v2, v1 - v2);
			float dot = Vector3.Dot(cross.normalized, Vector3.forward);
			if (dot < 0) return;

			Vector3 b0 = Vector3.right;
			Vector3 b1 = Vector3.up;
			Vector3 b2 = Vector3.forward;

			if (v2.y > v1.y)
			{
				this.Swap(ref v2, ref v1);
				this.Swap(ref b2, ref b1);
			}
			if (v2.y > v0.y)
			{
				this.Swap(ref v2, ref v0);
				this.Swap(ref b2, ref b0);
			}
			if (v1.y > v0.y)
			{
				this.Swap(ref v1, ref v0);
				this.Swap(ref b1, ref b0);
			}

			DrawTrianglePart(ref v0, ref v1, ref v2, ref b0, ref b1, ref b2, ref data, material, true);
			DrawTrianglePart(ref v0, ref v1, ref v2, ref b0, ref b1, ref b2, ref data, material, false);
		}

		private void DrawTriangle_Slow_But_HighQuality(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, BakedTriangleData data, Material material)
		{
			Vector2Int min = new Vector2Int((int)Mathf.Min(v0.x, v1.x, v2.x), (int)Mathf.Min(v0.y, v1.y, v2.y));
			Vector2Int max = new Vector2Int((int)Mathf.Max(v0.x, v1.x, v2.x), (int)Mathf.Max(v0.y, v1.y, v2.y));

			Vector3 cross = Vector3.Cross(v0 - v2, v1 - v2);
			float dot = Vector3.Dot(cross.normalized, Vector3.forward);
			if (dot < 0) return;
			Vector2Int.ClampInclusive(ref min, imageSizeInt);
			Vector2Int.ClampInclusive(ref max, imageSizeInt);

			for (int x = min.x; x <= max.x; x++)
			{
				for (int y = min.y; y <= max.y; y++)
				{
					Vector3 barycentricScreen = Mathf.Barycentric(ref v0, ref v1, ref v2, new Vector2(x, y));
					if (barycentricScreen.x < 0 || barycentricScreen.y < 0 || barycentricScreen.z < 0) continue;

					Color color;
					float zBuffer = 0f;
					if (material == null) color = Color.Magenta;
					else color = material.FragmentShader(barycentricScreen, out zBuffer, data);

					WritePixel(x, y, zBuffer, color);
				}
			}
		}

		private void DrawTrianglePart(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, ref Vector3 b0, ref Vector3 b1, ref Vector3 b2, ref BakedTriangleData data, Material material, bool buttom)
		{
			Vector3Int vi0 = (Vector3Int)v0;
			Vector3Int vi1 = (Vector3Int)v1;
			Vector3Int vi2 = (Vector3Int)v2;

			if (vi0.y == vi1.y && vi0.y == vi2.y) return;
			if (vi0.x == vi1.x && vi0.x == vi2.x) return;

			Vector3Int delta01 = (vi0 - vi1);
			Vector3Int delta02 = (vi0 - vi2);
			Vector3Int delta12 = (vi1 - vi2);

			if (delta01.y < 0 || delta02.y < 0 || delta12.y < 0) throw new Exception();

			if (buttom)
			{
				if (delta12.y == 0) return;
			}
			else if (delta01.y == 0) return;

			Vector3Int halfDir = buttom ? delta12 : delta01;
			Vector3Int startPos = buttom ? vi2 : vi1;

			Vector3 bHalf = buttom ? b2 : b1;
			Vector3 bEnd = buttom ? b1 : b0;

			int end = buttom ? vi1.y : vi0.y;

			for (int y = startPos.y; y <= end; y+=FAST_DRAW_PIXEL_SIZE)
			{
				if (y < 0 || y >= height) continue;

				float progress_half = (y - startPos.y) / (float)(halfDir.y);
				float progress_full = (y - vi2.y) / (float)delta02.y;

				int start_x = startPos.x + (int)(halfDir.x * progress_half);
				int end_x = vi2.x + (int)(delta02.x * progress_full);

				Vector3 barycentric_full = Vector3.Lerp(b2, b0, progress_full);
				Vector3 barycentric_half = Vector3.Lerp(bHalf, bEnd, progress_half);

				if (start_x > end_x)
				{
					this.Swap(ref start_x, ref end_x);
					this.Swap(ref barycentric_full, ref barycentric_half);
				}

				for (int x = start_x; x <= end_x; x+=FAST_DRAW_PIXEL_SIZE)
				{
					if (x < 0 || x >= width) continue;

					Vector3 barycentric_screen = Vector3.Lerp(barycentric_half, barycentric_full, (x - start_x) / (float)(end_x - start_x));

					Color color;
					float zBuffer = 0f;
					if (material == null) color = Color.Magenta;
					else  color = material.FragmentShader(barycentric_screen, out zBuffer, data);

					FillZone(x, y, FAST_DRAW_PIXEL_SIZE, FAST_DRAW_PIXEL_SIZE, zBuffer, color);
				}
			}
		}

		public void DrawLine(Vector2Int start, Vector2Int end, Color color)
		{
			int a = end.y - start.y;
			int b = start.x - end.x;
			int signA = a < 0 ? -1 : 1;
			int signB = b < 0 ? -1 : 1;

			int f = 0;
			int sign = Mathf.Abs(a) > Mathf.Abs(b) ? 1 : -1;

			int x = start.x;
			int y = start.y;

			while (x != end.x || y != end.y)
			{
				if (sign < 0)
				{
					f += a * signA;
					if (Mathf.Abs(f) > Mathf.Abs(f - Mathf.Abs(b)))
					{
						f -= b * signB;
						y += signA;
					}
					x -= signB;
				}
				else
				{
					f += b * signB;
					if (Mathf.Abs(f) > Mathf.Abs(f - Mathf.Abs(a)))
					{
						f -= a * signA;
						x -= signB;
					}
					y += signA;
				}

				if (x < width && y < height && x >= 0 && y >= 0) WritePixel(x, y, float.PositiveInfinity, color);
			}
		}

		private VertexData[] vertexDataNonAlloc = new VertexData[3]
		{
			new VertexData(), new VertexData(), new VertexData()
		};
		BakedTriangleData bakedData = new BakedTriangleData();

		private bool IsInsideScreen(Vector3 point)
		{
			return point.x < width && point.y < height && point.x >= 0 && point.y >= 0;
		}

		public void DrawMesh (Mesh mesh, Matrix4x4 matrix)
		{
			bool fast;
			Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

			for (int i = 0; i < mesh.vertices.Length; i++)
			{
				Vector3 vertex = matrix.MultiplyPoint(mesh.vertices[i]);
				if (vertex.x > max.x) max.x = vertex.x;
				if (vertex.y > max.y) max.y = vertex.y;
				if (vertex.x < min.x) min.x = vertex.x;
				if (vertex.y < min.y) min.y = vertex.y;
			}

			Vector2 screen_size = max - min;

			fast = screen_size.x * screen_size.y > LARGE_TRIANGLE_SQUARE;

			for (int i = 0; i < mesh.indices.Length; i+=3)
			{
				for (int index = 0; index < 3; index++)
				{
					Mesh.VertexIndex vertexIndex = mesh.indices[i + index];

					VertexData data = vertexDataNonAlloc[index];
					data.vertex = matrix.MultiplyPoint(mesh.vertices[vertexIndex.vertex]);
					data.normal = matrix.MultiplyVector(mesh.normals[vertexIndex.normal]).normalized;
					data.uv = mesh.uv[vertexIndex.uv];

					mesh.material.VertexShader(data);
				}

				if (IsInsideScreen(vertexDataNonAlloc[0].vertex) || IsInsideScreen(vertexDataNonAlloc[1].vertex) || IsInsideScreen(vertexDataNonAlloc[2].vertex))
				{
					bakedData.SetData(vertexDataNonAlloc[0], vertexDataNonAlloc[1], vertexDataNonAlloc[2]);
					if (fast) DrawTriangle_Fast_But_Low_Quality (ref vertexDataNonAlloc[0].vertex, ref vertexDataNonAlloc[1].vertex, ref vertexDataNonAlloc[2].vertex, bakedData, mesh.material);
					else DrawTriangle_Slow_But_HighQuality (ref vertexDataNonAlloc[0].vertex, ref vertexDataNonAlloc[1].vertex, ref vertexDataNonAlloc[2].vertex, bakedData, mesh.material);
				}
			}
		}
	}
}
