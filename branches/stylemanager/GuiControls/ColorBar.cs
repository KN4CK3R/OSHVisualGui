using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	class ColorBar : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "colorBar";
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
				UpdateBars();
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
				if (value.Height != 45)
				{
					value.Height = 45;
				}
				base.Size = value;
				UpdateBars();
			}
		}
		private Bitmap[] colorBar;

		[Category("Events")]
		public ColorChangedEvent ColorChangedEvent
		{
			get;
			set;
		}
		#endregion

		public ColorBar()
		{
			Type = ControlType.ColorBar;

			colorBar = new Bitmap[3];

			Size = DefaultSize = new Size(150, 45);

			Color = Color.Black;

			ForeColor = DefaultForeColor = Color.White;
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

		private void UpdateBars()
		{
			int width = Size.Width - 2;
			float multi = 255.0f / width;
			for (int i = 0; i < 3; ++i)
			{
				Bitmap bar = new Bitmap(Size.Width, 10);
				for (int x = 0; x < width; ++x)
				{
					Color tempColor;
					switch (i)
					{
						case 0:
							tempColor = Color.FromArgb((int)(x * multi), color.G, color.B);
							break;
						case 1:
							tempColor = Color.FromArgb(color.R, (int)(x * multi), color.B);
							break;
						default:
							tempColor = Color.FromArgb(color.R, color.G, (int)(x * multi));
							break;
					}
					for (int j = 1; j < 9; ++j)
					{
						bar.SetPixel(x + 1, j, tempColor);
					}
				}
				colorBar[i] = bar;
			}
		}

		public override void Render(Graphics graphics)
		{
			for (int i = 0; i < 3; ++i)
			{
				graphics.DrawImage(colorBar[i], AbsoluteLocation.X, AbsoluteLocation.Y + i * 15);
			}
		}

		public override Control Copy()
		{
			ColorBar copy = new ColorBar();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			ColorBar colorBar = copy as ColorBar;
			colorBar.color = color;
		}

		public override string ToString()
		{
			return Name + " - ColorBar";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("color"))
				Color = Color.FromXMLString(element.Attribute("color").Value.Trim());
		}
	}
}
