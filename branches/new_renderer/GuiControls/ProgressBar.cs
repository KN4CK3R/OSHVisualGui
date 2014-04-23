using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	class ProgressBar : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "progressBar";
			}
		}
		private int minimum;
		public int Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				if (value < maximum)
				{
					minimum = value;
				}
				this.value = this.value < minimum ? minimum : this.value;
			}
		}
		private int maximum;
		public int Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				if (value > minimum)
				{
					maximum = value;
				}
				this.value = this.value > maximum ? maximum : this.value;
			}
		}
		private int value;
		public int Value
		{
			get
			{
				return value;
			}
			set
			{
				if (value >= minimum && value <= maximum)
				{
					this.value = value;
				}
			}
		}
		private Brush barBrush;
		private Color barColor;
		public virtual Color BarColor
		{
			get
			{
				return barColor;
			}
			set
			{
				barColor = value;
				barBrush = new SolidBrush(barColor);
			}
		}
		#endregion

		public ProgressBar()
		{
			Type = ControlType.ProgressBar;

			Size = new Size(110, 24);

			minimum = 0;
			maximum = 100;
			value = 0;

			BackColor = Color.Empty;
			ForeColor = Color.FromArgb(unchecked((int)0xFF5A5857));
			BarColor = Color.FromArgb(unchecked((int)0xFF67AFF5));
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
				graphics.FillRectangle(backBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y, Size.Width - 2, Size.Height);
				graphics.FillRectangle(backBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 1, Size.Width, Size.Height - 2);
			}

			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y, Size.Width - 2, 1);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 1, Size.Width - 2, 1);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 1, 1, Size.Height - 2);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 1, 1, Size.Height - 2);

			for (int i = (int)(value / ((maximum - minimum) / ((Size.Width - 8) / 12.0f)) - 1); i >= 0; --i)
			{
				graphics.FillRectangle(barBrush, AbsoluteLocation.X + 4 + i * 12, AbsoluteLocation.Y + 4, 8, Size.Height - 8);
			}
		}

		public override Control Copy()
		{
			ProgressBar copy = new ProgressBar();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			ProgressBar progressBar = copy as ProgressBar;
			progressBar.minimum = minimum;
			progressBar.maximum = maximum;
			progressBar.value = value;
			progressBar.BarColor = barColor;
		}

		public override string ToString()
		{
			return Name + " - ProgressBar";
		}

		protected override void WriteToXmlElement(XElement element)
		{
			base.WriteToXmlElement(element);
			element.Add(new XAttribute("minimum", minimum.ToString()));
			element.Add(new XAttribute("maximum", maximum.ToString()));
			element.Add(new XAttribute("value", value.ToString()));
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.Attribute("minimum") != null)
				Minimum = int.Parse(element.Attribute("minimum").Value.Trim());
			else
				throw new Exception("Missing attribute 'minimum': " + element.Name);
			if (element.Attribute("maximum") != null)
				Maximum = int.Parse(element.Attribute("maximum").Value.Trim());
			else
				throw new Exception("Missing attribute 'maximum': " + element.Name);
			if (element.Attribute("value") != null)
				Value = int.Parse(element.Attribute("value").Value.Trim());
			else
				throw new Exception("Missing attribute 'value': " + element.Name);
		}
	}
}
