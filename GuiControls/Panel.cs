using System.Drawing;
using System.Drawing.Drawing2D;

namespace OSHVisualGui.GuiControls
{
	public class Panel : ContainerControl
	{
		#region Properties

		internal override string DefaultName => "panel";

		#endregion

		public Panel()
		{
			Type = ControlType.Panel;

			Size = DefaultSize = new Size(200, 200);

			ForeColor = DefaultForeColor = Color.Empty;
			BackColor = DefaultBackColor = Color.Empty;
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				var rect = new Rectangle(AbsoluteLocation, Size);
				var linearBrush = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 90, 90, 90)), LinearGradientMode.Vertical);
				graphics.FillRectangle(linearBrush, rect);
			}

			graphics.DrawString(Name, Font, new SolidBrush(Color.Black), AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5);

			base.Render(graphics);

			if (IsHighlighted)
			{
				using (var pen = new Pen(Color.Orange, 1))
				{
					graphics.DrawRectangle(pen, AbsoluteLocation.X - 3, AbsoluteLocation.Y - 2, Size.Width + 5, Size.Height + 4);
				}

				IsHighlighted = false;
			}
		}

		public override Control Copy()
		{
			var copy = new Panel();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var panel = copy as Panel;
			foreach (var control in PreOrderVisit())
			{
				panel.AddControl(control.Copy());
			}
		}

		public override string ToString()
		{
			return Name + " - Panel";
		}
	}
}
