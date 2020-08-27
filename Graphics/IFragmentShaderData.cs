using EnginePart;

namespace OwnGraphicsAgain
{
	public interface IFragmentShaderData
	{
		Vector3 GetVertex (ref Vector3 barycentric);
		Vector2 GetUV (ref Vector3 barycentric);
		Vector3 GetNormal (ref Vector3 barycentric);
	}
}
