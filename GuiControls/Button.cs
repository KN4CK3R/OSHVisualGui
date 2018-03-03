using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class Button : ScalableControl
	{
		#region Properties

		internal override string DefaultName => "button";

		private readonly Label label = new Label
		{
			Location = new Point(6, 5)
		};

		public override Color ForeColor { get => base.ForeColor; set { base.ForeColor = value; label.ForeColor = value; } }

		protected string DefaultText;
		public string Text { get => label.Text; set { label.Text = value ?? string.Empty; if (AutoSize) { base.Size = new Size(label.Size.Width + 12, label.Size.Height + 10); } CalculateLabelLocation(); } }

		public override Size Size { get => base.Size; set { base.Size = value; CalculateLabelLocation(); } }
		
		#endregion

		public Button()
		{
			Type = ControlType.Button;

			DefaultText = string.Empty;

			DefaultSize = new Size(92, 24);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));

			Size = DefaultSize;
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (Text != DefaultText)
			{
				yield return new KeyValuePair<string, ChangedProperty>("text", new ChangedProperty(Text));
			}
		}

		private void CalculateLabelLocation()
		{
			label.Location = new Point(Size.Width / 2 - label.Size.Width / 2, Size.Height / 2 - label.Size.Height / 2);
		}

		public override void CalculateAbsoluteLocation()
		{
			base.CalculateAbsoluteLocation();

			label.Parent = this;
		}

		public override void Render(Graphics graphics)
		{
			var tempBrush = new SolidBrush(BackColor.Add(Color.FromArgb(0, 10, 10, 10)));
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y, Size.Width - 2, Size.Height - 1);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 1, Size.Width, Size.Height - 3);
			tempBrush = new SolidBrush(BackColor.Substract(Color.FromArgb(0, 50, 50, 50)));
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 2, Size.Width - 2, 2);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 1, 1, Size.Height - 2);
			var rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 2, Size.Width - 2, Size.Height - 4);
			var temp = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);
			rect = new Rectangle(AbsoluteLocation.X + 2, AbsoluteLocation.Y + 1, Size.Width - 4, Size.Height - 2);
			temp = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);

			label.Render(graphics);
		}

		public override Control Copy()
		{
			var copy = new Button();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var button = copy as Button;
			button.Text = Text;
		}

		public override string ToString()
		{
			return Name + " - Button";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("text"))
				Text = Text.FromXMLString(element.Attribute("text").Value.Trim());
		}
	}
}
