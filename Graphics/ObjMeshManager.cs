using System;
using EnginePart;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace OwnGraphicsAgain
{
	public sealed class ObjMeshManager
	{
		public static Mesh LoadMesh(string path)
		{
			List<Vector3> rawVertices = new List<Vector3>();
			List<Vector3> rawNormals = new List<Vector3>();
			List<Vector2> rawUV = new List<Vector2>();
			List<Mesh.VertexIndex> indices = new List<Mesh.VertexIndex>();

			if (File.Exists(path))
			{
				var file = new StreamReader(path);

				string[] lines = file.ReadToEnd().Split('\n');

				for (int l = 0; l < lines.Length; l++)
				{
					string[] words = lines[l].Split(' ');

					var culture = CultureInfo.InvariantCulture;

					if (words[0].Equals("v"))
					{
						float x = float.Parse(words[1], culture);
						float y = float.Parse(words[2], culture);
						float z = float.Parse(words[3], culture);
						rawVertices.Add(new Vector3(x, y, z));
					}
					else if (words[0].Equals("vn"))
					{
						float x = float.Parse(words[1], culture);
						float y = float.Parse(words[2], culture);
						float z = float.Parse(words[3], culture);
						rawNormals.Add(new Vector3(x, y, z));
					}
					else if (words[0].Equals("vt"))
					{
						float x = float.Parse(words[1], culture);
						float y = float.Parse(words[2], culture);
						rawUV.Add(new Vector2(x, y));
					}
					else if (words[0].Equals("f"))
					{
						for (int f = 1; f <= 3; f++)
						{
							Mesh.VertexIndex index = new Mesh.VertexIndex();

							string[] vertexDesc = words[f].Split('/');

							index.vertex = int.Parse(vertexDesc[0]) - 1;
							index.uv = int.Parse(vertexDesc[1]) - 1;
							index.normal = int.Parse(vertexDesc[2]) - 1;

							indices.Add(index);
						}
					}
				}

				return new Mesh(rawVertices.ToArray(), rawNormals.ToArray(), rawUV.ToArray(), indices.ToArray());
			}
			else throw new Exception("Can't find model file");
		}
	}
}
