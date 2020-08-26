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

		private struct BakedTriangleData
		{
			Matrix4x4 normals;
			Matrix4x4 vertices;
			Matrix4x4 uvs;

			public BakedTriangleData(VertexData v0, VertexData v1, VertexData v2)
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
			}

			public Vector3 GetVertex(Vector3 barycentric)
			{
				return vertices.MultiplyVector(barycentric);
			}

			public Vector2 GetUV(Vector3 barycentric)
			{
				return (Vector2)uvs.MultiplyVector(barycentric);
			}

			public Vector3 GetNormal(Vector3 barycentric)
			{
				return normals.MultiplyVector(barycentric);
			}
		}

		private struct VertexData
		{
			public Vector3 vertex;
			public Vector3 normal;
			public Vector2 uv;

			public VertexData(Vector3 vertex, Vector3 normal, Vector2 uv)
			{
				this.vertex = vertex;
				this.normal = normal;
				this.uv = uv;
			}
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

		public void WritePixel(int x, int y, float depth, Color color)
		{
			y = (height - 1) - y;

			int ptr = y * width + x;
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

		private void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2, BakedTriangleData data, Material material)
		{
			Vector3 cross = Vector3.Cross(v0 - v2, v1 - v2);
			float dot = Vector3.Dot(cross.normalized, Vector3.forward);
			if (dot < 0) return;

			Vector2Int min = new Vector2Int((int)Mathf.Min(v0.x, v1.x, v2.x), (int)Mathf.Min(v0.y, v1.y, v2.y));
			Vector2Int max = new Vector2Int((int)Mathf.Max(v0.x, v1.x, v2.x), (int)Mathf.Max(v0.y, v1.y, v2.y));
			Vector2Int.ClampInclusive(ref min, imageSizeInt);
			Vector2Int.ClampInclusive(ref max, imageSizeInt);

			for (int x = min.x; x <= max.x; x++)
			{
				for (int y = min.y; y <= max.y; y++)
				{
					Vector3 barycentricScreen = Mathf.Barycentric((Vector2)v0, (Vector2)v1, (Vector2)v2, new Vector2(x, y));
					if (barycentricScreen.x < 0 || barycentricScreen.y < 0 || barycentricScreen.z < 0) continue;


					dot = (Vector3.Dot(data.GetNormal(barycentricScreen), Vector3.forward) + 1f) * 0.5f;

					Color color;

					if (material != null)
					{
						color = material.GetColor(data.GetUV(barycentricScreen));
						color = new Color32(dot * color.R, dot * color.G, dot * color.B, color.A);
					}
					else color = Color.Magenta;

					WritePixel(x, y, data.GetVertex(barycentricScreen).z, color);
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

				if (x < width && y < height && x >= 0 && y >= 0) pixelsBuffer[y * width + x] = color.ToArgb();
			}
		}

		private VertexData[] vertexDataNonAlloc = new VertexData[3];
		public void DrawMesh (Mesh mesh, Matrix4x4 matrix)
		{
			for (int i = 0; i < mesh.indices.Length; i+=3)
			{
				for (int index = 0; index < 3; index++)
				{
					Mesh.VertexIndex vertexIndex = mesh.indices[i + index];

					VertexData data = new VertexData();
					data.vertex = matrix.MultiplyPoint(mesh.vertices[vertexIndex.vertex]);
					data.normal = matrix.MultiplyVector(mesh.normals[vertexIndex.normal]).normalized;
					data.uv = mesh.uv[vertexIndex.uv];

					vertexDataNonAlloc[index] = data;
				}

				BakedTriangleData bakedData = new BakedTriangleData(vertexDataNonAlloc[0], vertexDataNonAlloc[1], vertexDataNonAlloc[2]);

				DrawTriangle(vertexDataNonAlloc[0].vertex, vertexDataNonAlloc[1].vertex, vertexDataNonAlloc[2].vertex, bakedData, mesh.material);
			}
		}
	}
}
