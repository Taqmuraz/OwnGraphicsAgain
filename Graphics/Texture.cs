using EnginePart;
using System;
using System.Drawing;

namespace OwnGraphicsAgain
{
	public class Texture
	{
		private int[] buffer;
		private int width, height;

		public Vector2 tiling { get; set; } = Vector2.one;

		public Texture(string fileName)
		{
			Bitmap bitmap = new Bitmap(fileName);
			width = bitmap.Width;
			height = bitmap.Height;
			buffer = new int[width * height];

			for (int x = 0; x < bitmap.Width; x++)
			{
				for (int y = 0; y < bitmap.Height; y++)
				{
					buffer[x + y * width] = bitmap.GetPixel(x, y).ToArgb();
				}
			}
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public Color GetColor(Vector2 uv)
		{
			uv = new Vector2(uv.x * tiling.x, uv.y * tiling.y);
			uv.y = 1f - uv.y;

			int x = (int)(uv.x * width);
			int y = (int)(uv.y * height);

			x %= width;
			y %= height;

			if (x < 0) x = width + x;
			if (y < 0) y = height + y;

			//return new Color32(255f * uv.x, 255f * uv.y, 0f, 255f);

			return Color.FromArgb(buffer[x + y * width]);
		}
	}
}
