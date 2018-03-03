using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class ColorPicker : ScalableControl
	{
		#region Properties

		internal override string DefaultName => "colorPicker";

		private Bitmap gradient;

		private Color color;
		public virtual Color Color
		{
			get => color;
			set
			{
				color = value;
				UpdateGradient();
			}
		}

		public override Size Size
		{
			get => base.Size;
			set
			{
				base.Size = value;
				UpdateGradient();
			}
		}

		[Category("Events")]
		public ColorChangedEvent ColorChangedEvent { get; set; }

		#endregion

		public ColorPicker()
		{
			Type = ControlType.ColorPicker;

			Size = DefaultSize = new Size(100, 150);

			Color = Color.Black;

			ForeColor = DefaultForeColor = Color.Empty;
			BackColor = DefaultBackColor = Color.Empty;

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
				return Color.FromArgb(
					(int)(brightness * 255),
					(int)(t * 255),
					(int)(p * 255)
				);
			}
			if (h < 2)
			{
				return Color.FromArgb(
					(int)(q * 255),
					(int)(brightness * 255),
					(int)(p * 255)
				);
			}
			if (h < 3)
			{
				return Color.FromArgb(
					(int)(p * 255),
					(int)(brightness * 255),
					(int)(t * 255)
				);
			}
			if (h < 4)
			{
				return Color.FromArgb(
					(int)(p * 255),
					(int)(q * 255),
					(int)(brightness * 255)
				);
			}
			if (h < 5)
			{
				return Color.FromArgb(
					(int)(t * 255),
					(int)(p * 255),
					(int)(brightness * 255)
				);
			}
				return Color.FromArgb(
					(int)(brightness * 255),
					(int)(p * 255),
					(int)(q * 255)
				);
		}

		private void UpdateGradient()
		{
			gradient = new Bitmap(Size.Width, Size.Height);

			for (var y = 0; y < Size.Height; ++y)
			{
				for (var x = 0; x < Size.Width; ++x)
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
			var copy = new ColorPicker();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var colorPicker = copy as ColorPicker;
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
