using EnginePart;
using System.Drawing;

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

		public override Color32 GetColor(Vector2 uv)
		{
			return color;
		}
	}
}
