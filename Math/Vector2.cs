using System;
using System.Drawing;

namespace EnginePart
{

	public struct Vector2
	{
		public float x, y;

		public Vector2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString ()
		{
			return $"( {x}, {y} )";
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3 (Vector2 v)
		{
			return new Vector3 (v.x, v.y, 0f);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2 (Vector3 v)
		{
			return new Vector2 (v.x, v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Point (Vector2 v)
		{
			return new Point ((int)v.x, (int)v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static implicit operator PointF (Vector2 v)
		{
			return new PointF (v.x, v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2 (Point v)
		{
			return new Vector2 (v.X, v.Y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2 (PointF v)
		{
			return new Vector2 (v.X, v.Y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator + (Vector2 a, Vector2 b)
		{
			return new Vector2 (a.x + b.x, a.y + b.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator - (Vector2 a, Vector2 b)
		{
			return new Vector2 (a.x - b.x, a.y - b.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator * (Vector2 a, float b)
		{
			return new Vector2 (a.x * b, a.y * b);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator / (Vector2 a, float b)
		{
			return new Vector2 (a.x / b, a.y / b);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator - (Vector2 v)
		{
			return new Vector2 (-v.x, -v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2Int v)
		{
			return new Vector2(v.x, v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref Vector2 v, Vector2 range)
		{
			if (v.x > range.x) v.x = range.x;
			if (v.y > range.y) v.y = range.y;
			if (v.x < 0) v.x = 0;
			if (v.y < 0) v.y = 0;
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static float Dot (Vector2 a, Vector2 b)
		{
			return a.x * b.x + a.y * b.y;
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static float Angle (Vector2 a, Vector2 b)
		{
			a = a.normalized;
			b = b.normalized;
			return Dot(a, b).ACos();
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			return a + (b - a) * t;
		}
		public float length
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			get
			{
				return (x * x + y * y).Sqrt();
			}
		}
		public Vector2 normalized
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			get
			{
				if (length == 0)
				{
					return Vector2.zero;
				}
				return this / length;
			}
		}

		public static readonly Vector2 right = new Vector2 (1, 0);
		public static readonly Vector2 zero = new Vector2 (0, 0);
		public static readonly Vector2 one = new Vector2 (1, 1);
		public static readonly Vector2 left = new Vector2 (-1, 0);
		public static readonly Vector2 up = new Vector2 (0, 1);
		public static readonly Vector2 down = new Vector2 (0, -1);
	}
}