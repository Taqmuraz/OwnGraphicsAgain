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

		public override Color32 GetColor(Vector2 uv)
		{
			if (texture == null) return Color.White;
			else return texture.GetColor(uv);
		}
	}
}
