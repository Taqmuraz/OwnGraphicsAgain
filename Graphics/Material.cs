using EnginePart;

namespace OwnGraphicsAgain
{
	public abstract class Material
	{
		public abstract Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData);
		public abstract void VertexShader(IVertexShaderData shaderData);

		protected float DotForBarycentric(ref Vector3 barycentric, IFragmentShaderData shaderData)
		{
			return Vector3.Dot(shaderData.GetNormal(ref barycentric).normalized, Vector3.forward).Clamp(0f, 1f);
		}
	}
}
