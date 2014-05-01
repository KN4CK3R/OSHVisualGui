using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	public class Label : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "label";
			}
		}
		protected string text;
		protected string DefaultText;
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value == null ? string.Empty : value;
				if (AutoSize)
				{
					base.Size = TextRenderer.MeasureText(text, Font, new Size(1000, 1000), TextFormatFlags.NoPadding);
				}
			}
		}
		public override Size Size
		{
			get
			{
				return base.Size;
			}
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
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				if (AutoSize)
				{
					base.Size = TextRenderer.MeasureText(text, Font, new Size(1000, 1000), TextFormatFlags.NoPadding);
				}
			}
		}
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (AutoSize)
				{
					base.Size = TextRenderer.MeasureText(text, Font, new Size(1000, 1000), TextFormatFlags.NoPadding);
				}
			}
		}
		#endregion

		public Label()
		{
			Type = ControlType.Label;

			DefaultText = text = string.Empty;

			DefaultAutoSize = AutoSize = true;
			MinimumSize = new Size(0, 0);

			DefaultBackColor = BackColor = Color.Empty;
			DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
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
			TextRenderer.DrawText(graphics, text, Font, new Rectangle(AbsoluteLocation, Size), ForeColor);
		}

		public override Control Copy()
		{
			Label copy = new Label();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			Label label = copy as Label;
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
