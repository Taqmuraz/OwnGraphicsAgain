using EnginePart;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OwnGraphicsAgain
{

	public class Mesh
	{
		public Material material { get; set; }
		public Bounds localBounds { get; private set; }

		public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] uv, VertexIndex[] indices)
		{
			this.vertices = vertices;
			this.normals = normals;
			this.uv = uv;
			this.indices = indices;

			float minX = vertices.OrderBy(v => v.x).First().x;
			float minY = vertices.OrderBy(v => v.y).First().y;
			float minZ = vertices.OrderBy(v => v.z).First().z;

			float maxX = vertices.OrderByDescending(v => v.x).First().x;
			float maxY = vertices.OrderByDescending(v => v.y).First().y;
			float maxZ = vertices.OrderByDescending(v => v.z).First().z;

			localBounds = Bounds.MinMax(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
		}

		public struct VertexIndex
		{
			public int vertex;
			public int normal;
			public int uv;
		}

		public Vector3[] vertices { get; private set; }
		public Vector3[] normals { get; private set; }
		public Vector2[] uv { get; private set; }
		public VertexIndex[] indices { get; private set; }
	}
}
