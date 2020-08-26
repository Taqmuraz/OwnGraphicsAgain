using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public class WindowComponent<TControl, TArg> : WindowComponentBase<TControl> where TControl : Control, IConstructorOwner<TArg>, new()
	{
		private readonly TArg arg;
		public WindowComponent(Form form, TArg arg) : base (form)
		{
			this.arg = arg;
			Initalize();
		}

		protected override TControl CreateControl()
		{
			var control = new TControl();
			control.CallConstructor(arg);
			return control;
		}
	}
}
