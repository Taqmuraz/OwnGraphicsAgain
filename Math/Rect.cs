namespace EnginePart
{
	public struct Rect
	{
		public Vector2 min;
		public Vector2 max;

		public Rect(float x, float y, float width, float height)
		{
			min = new Vector2(x, y);
			max = new Vector2(x + width, y + height);
		}

		public Rect(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		public Vector2 size => max - min;
		public Vector2 center => (min + max) * 0.5f;

		
		public static Rect TriangleRect(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
		{
			return new Rect(new Vector2(Mathf.Min(v0.x, v1.x, v2.x), Mathf.Min(v0.y, v1.y, v2.y)), new Vector2(Mathf.Max(v0.x, v1.x, v2.x), Mathf.Max(v0.y, v1.y, v2.y)));
		}
		
		public bool IntersectsWith(Rect rect)
		{
			return (min.x < rect.max.x && max.x > rect.min.x) && (min.y < rect.max.y && max.y > rect.min.y);
		}
	}
}