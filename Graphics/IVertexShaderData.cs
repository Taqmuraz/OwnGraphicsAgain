using EnginePart;

namespace OwnGraphicsAgain
{
	public interface IVertexShaderData
	{
		Vector3 normal { get; set; }
		Vector3 vertex { get; set; }
		Vector2 uv { get; set; }
	}
}
