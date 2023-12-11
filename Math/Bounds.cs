using EnginePart;

namespace OwnGraphicsAgain
{
	public struct Bounds
	{
		public Vector3 center;
		public Vector3 size;
		public Vector3 min { get; private set; }
		public Vector3 max { get; private set; }

		public Bounds(Vector3 center, Vector3 size)
		{
			this.center = center;
			this.size = size;
			size *= 0.5f;
			min = center - size;
			max = center + size;
		}
		
		public static Bounds MinMax(Vector3 min, Vector3 max)
		{
			if (min.x > max.x) Mathf.Swap(ref min.x, ref max.x);
			if (min.y > max.y) Mathf.Swap(ref min.y, ref max.y);
			if (min.z > max.z) Mathf.Swap(ref min.z, ref max.z);

			Vector3 delta = max - min;
			Vector3 center = (max + min) * 0.5f;
			return new Bounds(center, delta);
		}
		
		public override string ToString()
		{
			return $"Min : {min} max : {max}";
		}
		
		public static Bounds TriangleBounds(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2)
		{
			return MinMax(new Vector3(Mathf.Min(v0.x, v1.x, v2.x), Mathf.Min(v0.y, v1.y, v2.y), Mathf.Min(v0.z, v1.z, v2.z)), new Vector3(Mathf.Max(v0.x, v1.x, v2.x), Mathf.Max(v0.y, v1.y, v2.y), Mathf.Max(v0.z, v1.z, v2.z)));
		}
		
		public bool Contains(Vector3 point)
		{
			return point.x < max.x && point.y < max.y && point.z < max.z && point.x > min.x && point.y > min.y && point.z > min.z;
		}
		
		public bool IntersectsWith(Bounds box)
		{
			return (min.x < box.max.x && max.x > box.min.x) && (min.y < box.max.y && max.y > box.min.y) && ((min.z < box.max.z && max.z > box.min.z));
		}
	}
}
