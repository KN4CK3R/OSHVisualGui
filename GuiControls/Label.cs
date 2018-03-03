using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class Label : ScalableControl
	{
		#region Properties

		internal override string DefaultName => "label";

		protected string text;
		protected string DefaultText;
		public string Text
		{
			get => text;
			set
			{
				text = value ?? string.Empty;
				if (AutoSize)
				{
					base.Size = MeasureText(text, Font);
				}
			}
		}

		public override Size Size
		{
			get => base.Size;
			set
			{
				if (!AutoSize)
				{
					base.Size = value;
				}
			}
		}

		public override Font Font
		{
			get => base.Font;
			set
			{
				base.Font = value;
				if (AutoSize)
				{
					base.Size = MeasureText(text, Font);
				}
			}
		}

		public override bool AutoSize
		{
			get => base.AutoSize;
			set
			{
				base.AutoSize = value;
				if (AutoSize)
				{
					base.Size = MeasureText(text, Font);
				}
			}
		}
		
		#endregion

		public Label()
		{
			Type = ControlType.Label;

			text = DefaultText = string.Empty;

			AutoSize = DefaultAutoSize = true;
			MinimumSize = new Size(0, 0);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.Empty;
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

		public override void Render(Graphics graphics)
		{
			if (BackColor.A != 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}
			TextRenderer.DrawText(graphics, Text, Font, new Rectangle(AbsoluteLocation, Size), ForeColor, TextFormatFlags.NoPadding);
		}

		public override Control Copy()
		{
			var copy = new Label();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var label = copy as Label;
			label.text = text;
		}

		public override string ToString()
		{
			return Name + " - Label";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("text"))
				Text = Text.FromXMLString(element.Attribute("text").Value.Trim());
		}
	}
}
