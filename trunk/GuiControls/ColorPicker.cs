using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	class ColorPicker : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "colorPicker";
			}
		}
		private Color color;
		public virtual Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				UpdateGradient();
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
				base.Size = value;
				UpdateGradient();
			}
		}
		private Bitmap gradient;

		[Category("Events")]
		public ColorChangedEvent ColorChangedEvent
		{
			get;
			set;
		}
		#endregion

		public ColorPicker()
		{
			Type = ControlType.ColorPicker;

			Size = new Size(100, 150);

			BackColor = Color.Empty;
			ForeColor = Color.Empty;

			Color = Color.White;

			ColorChangedEvent = new ColorChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (Color != Color.Black)
			{
				yield return new KeyValuePair<string, ChangedProperty>("color", new ChangedProperty(Color));
			}
		}

		private Color GetColorAtPoint(int x, int y)
		{
			Color tmpColor;

			double hue = (1.0 / Size.Width) * x;
			hue = hue - (int)hue;
			double saturation, brightness;
			if (y <= Size.Height / 2.0)
			{
				saturation = y / (Size.Height / 2.0);
				brightness = 1;
			}
			else
			{
				saturation = (Size.Height / 2.0) / y;
				brightness = ((Size.Height / 2.0) - y + (Size.Height / 2.0)) / y;
			}

			double h = hue == 1.0 ? 0 : hue * 6.0;
			double f = h - (int)h;
			double p = brightness * (1.0 - saturation);
			double q = brightness * (1.0 - saturation * f);
			double t = brightness * (1.0 - (saturation * (1.0 - f)));
			if (h < 1)
			{
				tmpColor = Color.FromArgb(
								(int)(brightness * 255),
								(int)(t * 255),
								(int)(p * 255)
								);
			}
			else if (h < 2)
			{
				tmpColor = Color.FromArgb(
								(int)(q * 255),
								(int)(brightness * 255),
								(int)(p * 255)
								);
			}
			else if (h < 3)
			{
				tmpColor = Color.FromArgb(
								(int)(p * 255),
								(int)(brightness * 255),
								(int)(t * 255)
								);
			}
			else if (h < 4)
			{
				tmpColor = Color.FromArgb(
								(int)(p * 255),
								(int)(q * 255),
								(int)(brightness * 255)
								);
			}
			else if (h < 5)
			{
				tmpColor = Color.FromArgb(
								(int)(t * 255),
								(int)(p * 255),
								(int)(brightness * 255)
								);
			}
			else
			{
				tmpColor = Color.FromArgb(
								(int)(brightness * 255),
								(int)(p * 255),
								(int)(q * 255)
								);
			}

			return tmpColor;
		}

		private void UpdateGradient()
		{
			gradient = new Bitmap(Size.Width, Size.Height);

			for (int y = 0; y < Size.Height; ++y)
			{
				for (int x = 0; x < Size.Width; ++x)
				{
					gradient.SetPixel(x, y, GetColorAtPoint(x, y));
				}
			}
		}

		public override void Render(Graphics graphics)
		{
			graphics.DrawImage(gradient, AbsoluteLocation);
		}

		public override Control Copy()
		{
			ColorPicker copy = new ColorPicker();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			ColorPicker colorPicker = copy as ColorPicker;
			colorPicker.color = color;
		}

		public override string ToString()
		{
			return Name + " - ColorPicker";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("color"))
				Color = Color.FromXMLString(element.Attribute("color").Value.Trim());
		}
	}
}
