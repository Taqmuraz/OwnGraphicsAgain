namespace EnginePart
{
	public struct Quaternion
	{
		public float x, y, z, w;

		public Quaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Vector3 vector
		{
			get
			{
				return new Vector3(x, y, z);
			}
		}

		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
			Vector3 v_1 = a.vector;
			Vector3 v_2 = b.vector;

			Vector3 mul = Vector3.Cross(v_1, v_2);
			mul += v_1 * b.w + v_2 * a.w;

			return new Quaternion(mul.x, mul.y, mul.z, Vector3.Dot(v_1, v_2) * a.w * b.w);
		}
		public static Quaternion operator *(Quaternion a, float b)
		{
			return new Quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
		}
		public static Quaternion operator /(Quaternion a, float b)
		{
			return new Quaternion(a.x / b, a.y / b, a.z / b, a.w / b);
		}
		public static Quaternion operator +(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
		}
		public static Quaternion operator -(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
		}
		public static Quaternion FromMatrix4x4(Matrix4x4 matrix)
		{
			Vector3 right = (Vector3)matrix.column_0;
			Vector3 up = (Vector3)matrix.column_1;
			Vector3 forward = (Vector3)matrix.column_2;

			Quaternion ret = new Quaternion();

			ret.w = (1.0f + right.x + up.y + forward.z).Sqrt() * 0.5f;
			float w4_recip = 1.0f / (4.0f * ret.w);
			ret.x = (forward.y - up.z) * w4_recip;
			ret.y = (right.z - forward.x) * w4_recip;
			ret.z = (up.x - right.y) * w4_recip;

			return ret;
		}

		public static Quaternion AngleAxis(Vector3 axis, float angle)
		{
			axis = axis.normalized;
			axis *= angle.Sin() * 0.5f;
			return new Quaternion(axis.x, axis.y, axis.z, angle.Cos() * 0.5f);
		}

		public static float Norm(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}
		public static float Module(Quaternion q)
		{
			return Norm(q).Sqrt();
		}
		public static Quaternion Conjugate(Quaternion q)
		{
			return new Quaternion(-q.x, -q.y, -q.z, q.w);
		}
		public static Quaternion Inverse(Quaternion q)
		{
			return Conjugate(q) / Norm(q);
		}
	}
}