using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EnginePart
{
	public static class Mathf
	{
		public const float PI = (float)Math.PI;
		public const float Deg2Rad = PI / 180f;
		public const float Rad2Deg = 180f / PI;

		public static float Sin (this float a)
		{
			return (float)Math.Sin (a * Deg2Rad);
		}
		public static float Cos (this float a)
		{
			return (float)Math.Cos (a * Deg2Rad);
		}
		public static float Sqrt (this float a)
		{
			return (float)Math.Sqrt (a);
		}
		public static float ASin (this float a)
		{
			return (float)Math.Asin (a) * Rad2Deg;
		}
		public static float ACos (this float a)
		{
			return (float)Math.Acos (a) * Rad2Deg;
		}
		public static float Sign (this float a)
		{
			return Math.Sign (a);
		}
		public static float Lerp(this float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		public static float Tan(this float v)
		{
			return Sin(v) / Cos(v);
		}

		public static int Round(this float i)
		{
			return (int)Math.Round(i);
		}

		public static Vector3 Barycentric(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, Vector2 p)
		{
			Vector3 u = Vector3.Cross(new Vector3(v2.x - v0.x, v1.x - v0.x, v0.x - p.x), new Vector3(v2.y - v0.y, v1.y - v0.y, v0.y - p.y));
			if (Abs(u.z) < 1) return new Vector3(-1, 1, 1);
			return new Vector3(1f - (u.x + u.y) / u.z, u.y / u.z, u.x / u.z);
		}

		public static void Swap<T>(this object obj, ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}

		public static float Min (float a, float b)
		{
			if (a > b) return b;
			return a;
		}
		public static float Min(float a, float b, float c)
		{
			if (a <= b && a <= c) return a;
			if (b <= c && b <= a) return b;
			return c;
		}
		public static float Max(float a, float b)
		{
			if (a < b) return b;
			return a;
		}
		public static float Max(float a, float b, float c)
		{
			if (a >= b && a >= c) return a;
			if (b >= c && b >= a) return b;
			return c;
		}
		public static float Abs (this float a)
		{
			if (a < 0) return -a;
			return a;
		}
		public static Vector3 ToVector (this Color32 color)
		{
			return new Vector3(color.r, color.g, color.b);
		}
		public static Color32 ToColor(this Vector3 vector)
		{
			return new Color32(vector.x, vector.y, vector.z, 255f);
		}
		public static float Determinant (float a1, float b1, float a2, float b2)
		{
			return a1 * b2 - a2 * b1;
		}
		public static float Determinant (Vector2 axis_a, Vector2 axis_b)
		{
			return Determinant(axis_a.x, axis_b.x, axis_a.y, axis_b.y);
		}
		public static int Clamp (this int a, int min, int max)
		{
			if (a > max) a = max;
			if (a < min) a = min;
			return a;
		}
		public static float Clamp(this float a, float min, float max)
		{
			if (a > max) a = max;
			if (a < min) a = min;
			return a;
		}
	}
}

