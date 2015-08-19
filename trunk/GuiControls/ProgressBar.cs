using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
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
		protected Color DefaultBarColor;
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

			Size = DefaultSize = new Size(110, 24);

			minimum = 0;
			maximum = 100;
			value = 0;

			ForeColor = DefaultForeColor = Color.FromArgb(unchecked((int)0xFF5A5857));
			BackColor = DefaultBackColor = Color.Empty;

			BarColor = DefaultBarColor = Color.FromArgb(unchecked((int)0xFF67AFF5));
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (Minimum != 0)
			{
				yield return new KeyValuePair<string, ChangedProperty>("minimum", new ChangedProperty(Minimum));
			}
			if (Maximum != 100)
			{
				yield return new KeyValuePair<string, ChangedProperty>("maximum", new ChangedProperty(Maximum));
			}
			if (Value != 0)
			{
				yield return new KeyValuePair<string, ChangedProperty>("value", new ChangedProperty(Value));
			}
			if (BarColor != DefaultBarColor)
			{
				yield return new KeyValuePair<string, ChangedProperty>("barColor", new ChangedProperty(BarColor));
			}
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

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("minimum"))
				Minimum = Minimum.FromXMLString(element.Attribute("minimum").Value.Trim());
			if (element.HasAttribute("maximum"))
				Maximum = Maximum.FromXMLString(element.Attribute("maximum").Value.Trim());
			if (element.HasAttribute("value"))
				Value = Value.FromXMLString(element.Attribute("value").Value.Trim());
		}
	}
}
