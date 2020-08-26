using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public class WindowComponent<TControl, TArg0, TArg1> : WindowComponentBase<TControl> where TControl : Control, IConstructorOwner<TArg0, TArg1>, new()
	{
		private readonly TArg0 arg0;
		private readonly TArg1 arg1;
		public WindowComponent(Form form, TArg0 arg0, TArg1 arg1) : base (form)
		{
			this.arg0 = arg0;
			this.arg1 = arg1;

			Initalize();
		}

		protected override TControl CreateControl()
		{
			var control = new TControl();
			control.CallConstructor(arg0, arg1);
			return control;
		}
	}
}
