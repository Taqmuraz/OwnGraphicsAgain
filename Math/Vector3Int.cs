namespace EnginePart
{
	public struct Vector3Int
	{
		public int x, y, z;

		public Vector3Int (int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector3Int operator +(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Vector3Int operator -(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3Int operator *(Vector3Int a, int b)
		{
			return new Vector3Int(a.x * b, a.y * b, a.z * b);
		}
		public static Vector3Int operator /(Vector3Int a, int b)
		{
			return new Vector3Int(a.x / b, a.y / b, a.z / b);
		}
		public static Vector3Int operator -(Vector3Int v)
		{
			return new Vector3Int(-v.x, -v.y, -v.z);
		}
		public static explicit operator Vector3Int (Vector3 vector)
		{
			return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
		}
		public static implicit operator Vector3(Vector3Int v)
		{
			return new Vector3(v.x, v.y, v.z);
		}
	}
}