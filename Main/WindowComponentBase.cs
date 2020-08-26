using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnGraphicsAgain
{
	public abstract class WindowComponentBase<TControl> where TControl : Control, new()
	{
		public TControl control { get; private set; }
		private readonly Form form;

		public WindowComponentBase(Form form)
		{
			this.form = form;
		}

		protected void Initalize()
		{
			control = CreateControl();
			control.Parent = form;
		}

		public static implicit operator TControl(WindowComponentBase<TControl> component)
		{
			return component.control;
		}

		protected abstract TControl CreateControl();
	}
}
