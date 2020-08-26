using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public sealed class WindowComponent<TControl> : WindowComponentBase<TControl> where TControl : Control, new()
	{
		public WindowComponent(Form form) : base(form)
		{
			Initalize();
		}

		protected override TControl CreateControl()
		{
			return new TControl();
		}
	}
}
