using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public static partial class Extensions
	{
		public static TControl AddControl<TControl>(this Form form) where TControl : Control, new()
		{
			return new WindowComponent<TControl>(form);
		}
		public static TControl AddControl<TControl, TArg>(this Form form, TArg arg) where TControl : Control, IConstructorOwner<TArg>, new()
		{
			return new WindowComponent<TControl, TArg>(form, arg);
		}
		public static TControl AddControl<TControl, TArg0, TArg1>(this Form form, TArg0 arg0, TArg1 arg1) where TControl : Control, IConstructorOwner<TArg0, TArg1>, new()
		{
			return new WindowComponent<TControl, TArg0, TArg1>(form, arg0, arg1);
		}
		public static TControl AddControl<TControl, TArg0, TArg1, TArg2>(this Form form, TArg0 arg0, TArg1 arg1, TArg2 arg2) where TControl : Control, IConstructorOwner<TArg0, TArg1, TArg2>, new()
		{
			return new WindowComponent<TControl, TArg0, TArg1, TArg2>(form, arg0, arg1, arg2);
		}

		public static void Swap<T>(this object obj, ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}
	}
}
