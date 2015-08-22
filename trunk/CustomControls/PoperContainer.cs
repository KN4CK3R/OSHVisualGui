using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public partial class PoperContainer : ToolStripDropDown
	{
		private Control popedControl;
		private ToolStripControlHost host;
		private bool fade = true;

		public PoperContainer(Control popedControl)
		{
			InitializeComponent();

			if (popedControl == null)
			{
				throw new ArgumentNullException();
			}
			
			this.popedControl = popedControl;

			fade = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;

			host = new ToolStripControlHost(popedControl);
			host.AutoSize = false;

			Padding = Margin = host.Padding = host.Margin = Padding.Empty;

			popedControl.Location = Point.Empty;

			Items.Add(host);

			popedControl.Disposed += delegate(object sender, EventArgs e)
			{
				popedControl = null;
				Dispose(true);
			};
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}

			return base.ProcessDialogKey(keyData);
		}

		public void Show(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException();
			}

			Show(control, control.ClientRectangle);
		}

		public void Show(Form f, Point p)
		{
			Show(f, new Rectangle(p, new Size(0, 0)));
		}

		private void Show(Control control, Rectangle area)
		{
			if (control == null)
			{
				throw new ArgumentNullException();
			}

			var location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));

			var screen = Screen.FromControl(control).WorkingArea;

			if (location.X + Size.Width > (screen.Left + screen.Width))
				location.X = (screen.Left + screen.Width) - Size.Width;

			if (location.Y + Size.Height > (screen.Top + screen.Height))
				location.Y -= Size.Height + area.Height;

			location = control.PointToClient(location);

			Show(control, location, ToolStripDropDownDirection.BelowRight);
		}

		private const int frames = 5;
		private const int totalduration = 100;
		private const int frameduration = totalduration / frames;

		protected override void SetVisibleCore(bool visible)
		{
			double opacity = Opacity;
			if (visible && fade)
				Opacity = 0;

			base.SetVisibleCore(visible);

			if (!visible || !fade)
				return;

			for (int i = 1; i <= frames; i++)
			{
				if (i > 1)
				{
					System.Threading.Thread.Sleep(frameduration);
				}
				Opacity = opacity * i / frames;
			}
			Opacity = opacity;
		}

		protected override void OnOpening(CancelEventArgs e)
		{
			if (popedControl.IsDisposed || popedControl.Disposing)
			{
				e.Cancel = true;
				return;
			}
			base.OnOpening(e);
		}

		protected override void OnOpened(EventArgs e)
		{
			popedControl.Focus();

			base.OnOpened(e);
		}
	}
}
