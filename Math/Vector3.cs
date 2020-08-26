using System;
using System.Drawing;

namespace EnginePart
{

	public struct Vector3
	{
		public float x, y, z;

		public Vector3 (float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public static float Dot (Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}
		public static Vector3 Cross(Vector3 a, Vector3 b)
		{
			Vector3 mul = Vector3.right * a.y * b.z + Vector3.forward * a.x * b.y + Vector3.up * a.z * b.x
			- Vector3.up * a.x * b.z - Vector3.right * a.z * b.y - Vector3.forward * a.y * b.x;

			return mul;
		}
		public override string ToString ()
		{
			return string.Format ("{0} {1} {2}", x.ToString (), y.ToString (), z.ToString ());
		}

		public static Vector3 operator + (Vector3 a, Vector3 b)
		{
			return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Vector3 operator - (Vector3 a, Vector3 b)
		{
			return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3 operator * (Vector3 a, float b)
		{
			return new Vector3 (a.x * b, a.y * b, a.z * b);
		}
		public static Vector3 operator *(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		public static Vector3 operator / (Vector3 a, float b)
		{
			return new Vector3 (a.x / b, a.y / b, a.z / b);
		}

		public static explicit operator Point (Vector3 v)
		{
			return new Point ((int)v.x, (int)v.y);
		}
		public static implicit operator PointF (Vector3 v)
		{
			return new PointF (v.x, v.y);
		}
		public static explicit operator Vector3 (Point point) 
		{
			return new Vector3 (point.X, point.Y, 0f);
		}
		public static explicit operator Vector3 (PointF point) 
		{
			return new Vector3 (point.X, point.Y, 0f);
		}
		public static Vector3 operator - (Vector3 v)
		{
			return new Vector3 (-v.x, -v.y, -v.z);
		}
		public static implicit operator Vector4(Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, 0f);
		}
		public static explicit operator Vector3(Vector4 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		public float length
		{
			get
			{
				return (x * x + y * y + z * z).Sqrt();
			}
		}

		public Vector3 normalized
		{
			get
			{
				if (length == 0)
				{
					return Vector3.zero;
				}
				return this / length;
			}
		}

		public static float Angle (Vector3 a, Vector3 b)
		{
			return Dot(a, b).ACos();
		}

		public static Vector3 Lerp (Vector3 a, Vector3 b, float t)
		{
			return a + (b - a) * t;
		}

		public static readonly Vector3 right = new Vector3 (1, 0, 0);
		public static readonly Vector3 zero = new Vector3 (0, 0, 0);
		public static readonly Vector3 one = new Vector3 (1, 1, 1);
		public static readonly Vector3 left = new Vector3 (-1, 0, 0);
		public static readonly Vector3 up = new Vector3 (0, 1, 0);
		public static readonly Vector3 down = new Vector3 (0, -1, 0);
		public static readonly Vector3 forward = new Vector3 (0, 0, 1);
		public static readonly Vector3 back = new Vector3 (0, 0, -1);
	}
}