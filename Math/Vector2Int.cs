namespace EnginePart
{
	public struct Vector2Int
	{
		public int x, y;

		public Vector2Int (int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2Int(Vector2 vector)
		{
			return new Vector2Int((int)vector.x, (int)vector.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2Int(Vector3 vector)
		{
			return new Vector2Int((int)vector.x, (int)vector.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2Int(Vector4 vector)
		{
			return new Vector2Int((int)vector.x, (int)vector.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator +(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x + b.x, a.y + b.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x - b.x, a.y - b.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, int b)
		{
			return new Vector2Int(a.x * b, a.y * b);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator *(Vector2Int a, float b)
		{
			return new Vector2Int((int)(a.x * b), (int)(a.y * b));
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator /(Vector2Int a, int b)
		{
			return new Vector2Int(a.x / b, a.y / b);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static Vector2Int operator -(Vector2Int v)
		{
			return new Vector2Int(-v.x, -v.y);
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static void ClampInclusive(ref Vector2Int v, Vector2Int range)
		{
			if (v.x >= range.x) v.x = range.x - 1;
			else if (v.x < 0) v.x = 0;
			if (v.y >= range.y) v.y = range.y - 1;
			else if (v.y < 0) v.y = 0;
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return $"x : {x}\ny : {y}";
		}
	}
}