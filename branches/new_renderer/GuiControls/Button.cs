using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	public class Button : ScalableControl
	{
		#region Properties
		internal override string DefaultName { get { return "button"; } }
		private Label label;

		public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
		protected string DefaultText;
		public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (AutoSize) { base.Size = new Size(label.Size.Width + 12, label.Size.Height + 10); } CalculateLabelLocation(); } }
		public override Size Size { get { return base.Size; } set { base.Size = value; CalculateLabelLocation(); } }
		#endregion

		public Button()
		{
			Initialize();

			Size = DefaultSize;

			BackColor = DefaultBackColor;
			ForeColor = DefaultForeColor;
		}

		protected Button(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Initialize();

			Text = info.GetString("text");
		}

		private void Initialize()
		{
			Type = ControlType.Button;

			label = new Label();
			label.Location = new Point(6, 5);

			DefaultText = string.Empty;

			DefaultSize = new Size(92, 24);

			DefaultBackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));
			DefaultForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("text", Text);
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
			Brush tempBrush = new SolidBrush(BackColor.Add(Color.FromArgb(0, 10, 10, 10)));
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y, Size.Width - 2, Size.Height - 1);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 1, Size.Width, Size.Height - 3);
			tempBrush = new SolidBrush(BackColor.Substract(Color.FromArgb(0, 50, 50, 50)));
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 2, Size.Width - 2, 2);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 1, 1, Size.Height - 2);
			Rectangle rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 2, Size.Width - 2, Size.Height - 4);
			LinearGradientBrush temp = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);
			rect = new Rectangle(AbsoluteLocation.X + 2, AbsoluteLocation.Y + 1, Size.Width - 4, Size.Height - 2);
			temp = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
			graphics.FillRectangle(temp, rect);

			label.Render(graphics);
		}

		public override Control Copy()
		{
			Button copy = new Button();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			Button button = copy as Button;
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
