namespace EnginePart
{
	public struct Color32
	{
		public byte r, g, b, a;

		public Color32(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		public Color32(float r, float g, float b, float a)
		{
			this.r = (byte)r;
			this.g = (byte)g;
			this.b = (byte)b;
			this.a = (byte)a;
		}

		public static readonly Color32 red = new Color32 (255, 0, 0, 255);
		public static readonly Color32 green = new Color32 (0, 255, 0, 255);
		public static readonly Color32 blue = new Color32 (0, 0, 255, 255);
		public static readonly Color32 white = new Color32 (255, 255, 255, 255);
		public static readonly Color32 black = new Color32 (0, 0, 0, 255);

		
		public static Color32 operator * (Color32 a, float b)
		{
			return new Color32(a.r * b, a.g * b, a.b * b, a.a);
		}
		
		public static Color32 operator *(Color32 a, Color32 b)
		{
			return new Color32(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
		}
		
		public static Color32 operator +(Color32 a, Color32 b)
		{
			return new Color32(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}
		
		public static implicit operator System.Drawing.Color (Color32 color)
		{
			return System.Drawing.Color.FromArgb (color.a, color.r, color.g, color.b);
		}
		
		public static implicit operator Color32(System.Drawing.Color color)
		{
			return new Color32(color.R, color.G, color.B, color.A);
		}
	}
}