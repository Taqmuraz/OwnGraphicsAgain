using EnginePart;
using System.Drawing;

namespace OwnGraphicsAgain
{
	public sealed class TextureMaterial : Material
	{
		public TextureMaterial(Texture texture)
		{
			this.texture = texture;
		}

		public Texture texture { get; private set; }

		public override Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData)
		{
			zBuffer = shaderData.GetVertex(ref barycentric).z;

			//return new Color32(255f * barycentric.x, 255f * barycentric.y, 255f * barycentric.z, 255f);

			if (texture == null) return Color.White;
			else
			{
				Color32 color = texture.GetColor(shaderData.GetUV(ref barycentric));
				color *= DotForBarycentric(ref barycentric, shaderData);
				return color;
			}
		}

		public override void VertexShader(IVertexShaderData shaderData)
		{

		}
	}
}
