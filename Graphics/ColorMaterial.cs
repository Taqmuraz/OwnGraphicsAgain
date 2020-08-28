using EnginePart;
using System.Drawing;
using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public class UnlitColorMaterial : Material
	{
		public Color32 color { get; set; }

		public UnlitColorMaterial()
		{
			color = Color.White;
		}

		public UnlitColorMaterial(Color32 color)
		{
			this.color = color;
		}

		public override Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData)
		{
			zBuffer = shaderData.GetVertex(ref barycentric).z;
			return color;
		}

		public override void VertexShader(IVertexShaderData shaderData)
		{

		}
	}
	public sealed class ColorMaterial : UnlitColorMaterial
	{
		public ColorMaterial(Color32 color) : base(color)
		{
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
