using EnginePart;

namespace OwnGraphicsAgain
{
	public abstract class Material
	{
		private static Vector3 m_lightDirection = new Vector3(0.25f, -1f, 0.25f).normalized;
		public static Vector3 lightDirection
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			get => m_lightDirection;
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			set => m_lightDirection = value.normalized;
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public abstract Color32 FragmentShader(Vector3 barycentric, out float zBuffer, IFragmentShaderData shaderData);

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public abstract void VertexShader(IVertexShaderData shaderData);

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		protected float DotForBarycentric(ref Vector3 barycentric, IFragmentShaderData shaderData)
		{
			return (1f + Vector3.Dot(shaderData.GetNormal(ref barycentric).normalized, -lightDirection)) * 0.5f;
		}
	}
}
