using EnginePart;
using System.Drawing;
using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public sealed class ColorMaterial : Material
	{
		public Color32 color { get; set; }

		public ColorMaterial()
		{
			color = Color.White;
		}

		public ColorMaterial(Color32 color)
		{
			this.color = color;
		}

		public override Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData)
		{
			zBuffer = shaderData.GetVertex(ref barycentric).z;
			return color * DotForBarycentric(ref barycentric, shaderData);
		}

		public override void VertexShader(IVertexShaderData shaderData)
		{
		}
	}
}
