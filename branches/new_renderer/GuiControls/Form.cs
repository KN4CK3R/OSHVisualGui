using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	public class Form : ContainerControl
	{
		#region Properties
		private Panel panel;
		private string text;
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value == null ? string.Empty : value;
			}
		}
		public override Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
			}
		}
		internal override List<Control> Controls
		{
			get
			{
				return panel.Controls;
			}
		}
		internal override Point ContainerLocation
		{
			get
			{
				return base.ContainerLocation.Add(panel.Location);
			}
		}
		internal override Point ContainerAbsoluteLocation
		{
			get
			{
				return panel.ContainerAbsoluteLocation;
			}
		}
		internal override Size ContainerSize
		{
			get
			{
				return panel.ContainerSize;
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
				Size tempSize = value;
				if (tempSize.Width < 80 || tempSize.Height < 50)
				{
					tempSize = new Size(Math.Max(80, tempSize.Width), Math.Max(50, tempSize.Height));
				}
				base.Size = tempSize;
				panel.Size = new Size(value.Width - 2 * 6, value.Height - 17 - 2 * 6);
			}
		}

		[Category("Events")]
		public ConstructorEvent ConstructorEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public FormClosingEvent FormClosingEvent
		{
			get;
			set;
		}
		#endregion

		public Form(Point location)
			: this()
		{
			base.Location = location;
		}

		public Form()
		{
			Type = ControlType.Form;

			Parent = this;

			Mode = DragMode.GrowOnly;

			panel = new Panel();
			panel.Location = new Point(6, 6 + 17);
			panel.isSubControl = true;
			AddSubControl(panel);

			DefaultSize = Size = new Size(300, 300);

			DefaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF7C7B79));
			DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

			FormClosingEvent = new FormClosingEvent(this);
			ConstructorEvent = new ConstructorEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			yield return new KeyValuePair<string, object>("SetText", Text);
		}

		public override void AddControl(Control control)
		{
			panel.AddControl(control);
		}

		public override void RemoveControl(Control control)
		{
			panel.RemoveControl(control);
		}

		public override void Render(Graphics graphics)
		{
			Rectangle rect = new Rectangle(AbsoluteLocation, Size);
			LinearGradientBrush linearBrush = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 100, 100, 100)), LinearGradientMode.Vertical);

			graphics.FillRectangle(linearBrush, rect);
			graphics.DrawString(text, Font, foreBrush, new Point(AbsoluteLocation.X, AbsoluteLocation.Y + 2));
			graphics.FillRectangle(new SolidBrush(BackColor.Substract(Color.FromArgb(0, 50, 50, 50))), AbsoluteLocation.X + 5, AbsoluteLocation.Y + 17 + 2, Size.Width - 10, 1);

			Point crossLocation = new Point(AbsoluteLocation.X + Size.Width - 16, AbsoluteLocation.Y + 6);
			for (int i = 0; i < 4; ++i)
			{
				graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + i, 3, 1);
				graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + i, 3, 1);
				graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + 7 - i, 3, 1);
				graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + 7 - i, 3, 1);
			}

			panel.Render(graphics);

			if (isHighlighted)
			{
				using (Pen pen = new Pen(Color.Orange, 1))
				{
					graphics.DrawRectangle(pen, AbsoluteLocation.X - 2, AbsoluteLocation.Y - 2, Size.Width + 3, Size.Height + 3);
				}

				isHighlighted = false;
			}
		}

		public override Control Copy()
		{
			throw new NotImplementedException();
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);
		}

		public override string ToString()
		{
			return Name + " - Form";
		}

		protected override void WriteToXmlElement(XElement element)
		{
			base.WriteToXmlElement(element);

			element.Add(new XAttribute("text", Text));
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.Attribute("text") != null)
				Text = element.Attribute("text").Value;
			else
				throw new Exception("Missing attribute 'text': " + element.Name);
		}
	}
}
