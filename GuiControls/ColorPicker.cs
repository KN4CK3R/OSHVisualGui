using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    class ColorPicker : Control
    {
        #region Properties
        internal override string DefaultName { get { return "colorPicker"; } }
        private Color color;
        public virtual Color Color { get { return color; } set { color = value; UpdateGradient(); } }
        public override Size Size { get { return base.Size; } set { base.Size = value; UpdateGradient(); } }
        private Bitmap gradient;
        #endregion

        public ColorPicker()
        {
            Size = new Size(100, 150);

            BackColor = Color.Empty;
            ForeColor = Color.Empty;

            Color = Color.White;
        }

        private Color GetColorAtPoint(int x, int y)
	    {
		    Color tmpColor;
		
		    double hue = (1.0 / size.Width) * x;
		    hue = hue - (int)hue;
		    double saturation, brightness;
		    if (y <= size.Height / 2.0)
		    {
			    saturation = y / (size.Height / 2.0);
			    brightness = 1;
		    }
		    else
		    {
			    saturation = (size.Height / 2.0) / y;
			    brightness = ((size.Height / 2.0) - y + (size.Height / 2.0)) / y;
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
            gradient = new Bitmap(size.Width, size.Height);

            for (int y = 0; y < size.Height; ++y)
            {
                for (int x = 0; x < size.Width; ++x)
                {
                    gradient.SetPixel(x, y, GetColorAtPoint(x, y));
                }
            }
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawImage(gradient, absoluteLocation);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 1, absoluteLocation.Y - 1, size.Width + 1, size.Height + 1);
                }
            }
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
            return name + " - ColorPicker";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::ColorPicker();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (size != new Size(100, 150))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (color != Color.Black)
            {
                code.AppendLine(linePrefix + name + "->SetColor(OSHGui::Drawing::Color(" + color.A + ", " + color.R + ", " + color.G + ", " + color.B + "));");
            }
            return code.ToString();
        }
    }
}
