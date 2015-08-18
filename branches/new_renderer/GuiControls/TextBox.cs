﻿using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	class TextBox : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "textBox";
			}
		}
		private string text;
		protected string DefaultText;
		public virtual string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		[Category("Events")]
		public TextChangedEvent TextChangedEvent
		{
			get;
			set;
		}
		#endregion

		public TextBox()
		{
			Type = ControlType.TextBox;

			DefaultText = text = string.Empty;

			DefaultSize = Size = new Size(100, 24);

			DefaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF242321));
			DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

			TextChangedEvent = new TextChangedEvent(this);
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
			Brush tempBrush = new SolidBrush(BackColor.Add(Color.FromArgb(0, 20, 20, 20)));
			graphics.FillRectangle(tempBrush, new Rectangle(AbsoluteLocation, Size));
			graphics.FillRectangle(backBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height - 2);

			graphics.DrawString(text, Font, foreBrush, new RectangleF(AbsoluteLocation.X + 5, AbsoluteLocation.Y + 6, Size.Width - 10, Size.Height - 12));
		}

		public override Control Copy()
		{
			TextBox copy = new TextBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			TextBox textBox = copy as TextBox;
			textBox.text = text;
		}

		public override string ToString()
		{
			return Name + " - TextBox";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("text"))
				Text = Text.FromXMLString(element.Attribute("text").Value.Trim());
		}
	}
}
