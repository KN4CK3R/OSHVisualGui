using System.Drawing;
using System.Drawing.Drawing2D;

namespace OSHVisualGui.GuiControls
{
	public class RadioButton : CheckBox
	{
		#region Properties

		internal override string DefaultName => "radioButton";

		public override bool Checked
		{
			get => _checked;
			set
			{
				if (_checked != value)
				{
					if (Parent != null)
					{
						foreach (var control in (Parent as ContainerControl).Controls)
						{
							if (control is RadioButton)
							{
								(control as RadioButton)._checked = false;
							}
						}
					}
					_checked = value;
				}
			}
		}
		
		#endregion

		public RadioButton()
		{
			Type = ControlType.RadioButton;

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF222222));
		}

		public override void Render(Graphics graphics)
		{
			graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, new Size(17, 17)));
			var rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, 15, 15);
			var temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);
			rect = new Rectangle(AbsoluteLocation.X + 2, AbsoluteLocation.Y + 2, 13, 13);
			temp = new LinearGradientBrush(rect, BackColor, BackColor.Add(Color.FromArgb(0, 55, 55, 55)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);

			if (_checked)
			{
				rect = new Rectangle(AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5, 7, 7);
				temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
				graphics.FillEllipse(temp, rect);
			}
			label.Render(graphics);
		}

		public override Control Copy()
		{
			var copy = new RadioButton();
			CopyTo(copy);
			return copy;
		}

		public override string ToString()
		{
			return Name + " - RadioButton";
		}
	}
}
