using EnginePart;

namespace OwnGraphicsAgain
{
	public abstract class Material
	{
		private static Vector3 m_lightDirection = new Vector3(0.25f, -1f, 0.25f).normalized;
		public static Vector3 lightDirection
		{
			
			get => m_lightDirection;
			
			set => m_lightDirection = value.normalized;
		}
		
		public abstract Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData);

		
		public abstract void VertexShader(IVertexShaderData shaderData);

		
		protected float DotForBarycentric(ref Vector3 barycentric, IFragmentShaderData shaderData)
		{
			return (1f + Vector3.Dot(shaderData.GetNormal(ref barycentric).normalized, -lightDirection)) * 0.5f;
		}
	}
}
