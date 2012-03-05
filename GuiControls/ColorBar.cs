using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    class ColorBar : Control
    {
        #region Properties
        internal override string DefaultName { get { return "colorBar"; } }
        private Color color;
        public virtual Color Color { get { return color; } set { color = value; UpdateBars(); } }
        public override Size Size { get { return base.Size; } set { if (value.Height != 45) { value.Height = 45; } base.Size = value; UpdateBars(); } }
        private Bitmap[] colorBar;
        #endregion

        public ColorBar()
        {
            colorBar = new Bitmap[3];

            Size = new Size(150, 45);

            Color = Color.Black;

            BackColor = Color.Empty;
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        private void UpdateBars()
        {
            int width = size.Width - 2;
            float multi = 255.0f / width;
            for (int i = 0; i < 3; ++i)
            {
                Bitmap bar = new Bitmap(size.Width, 10);
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
                graphics.DrawImage(colorBar[i], absoluteLocation.X, absoluteLocation.Y + i * 15);
		    }

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
            return name + " - ColorBar";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::ColorBar();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(linePrefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(linePrefix + name + "->SetVisible(false);");
            }
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (size != new Size(150, 45))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (color != Color.Black)
            {
                code.AppendLine(linePrefix + name + "->SetColor(OSHGui::Drawing::Color(" + color.A + ", " + color.R + ", " + color.G + ", " + color.B + "));");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(System.Xml.XmlDocument document, System.Xml.XmlElement element)
        {
            base.WriteToXmlElement(document, element);
        }

        public override Control XmlElementToControl(System.Xml.XmlElement element)
        {
            throw new NotImplementedException();
        }
    }
}
