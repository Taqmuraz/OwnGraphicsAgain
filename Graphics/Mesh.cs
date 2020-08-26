using EnginePart;

namespace OwnGraphicsAgain
{

	public class Mesh
	{
		public Material material { get; set; }

		public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] uv, VertexIndex[] indices)
		{
			this.vertices = vertices;
			this.normals = normals;
			this.uv = uv;
			this.indices = indices;
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
