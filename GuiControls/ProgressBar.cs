using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class ProgressBar : Control
    {
        #region Properties
        internal override string DefaultName { get { return "progressBar"; } }
        private int minimum;
        public int Minimum { get { return minimum; } set { if (value < maximum) { minimum = value; } this.value = this.value < minimum ? minimum : this.value; } }
        private int maximum;
        public int Maximum { get { return maximum; } set { if (value > minimum) { maximum = value; } this.value = this.value > maximum ? maximum : this.value; } }
        private int value;
        public int Value { get { return value; } set { if (value >= minimum && value <= maximum) { this.value = value; } } }
        private Brush barBrush;
        private Color barColor;
        public virtual Color BarColor { get { return barColor; } set { barColor = value; barBrush = new SolidBrush(barColor); } }
        #endregion

        public ProgressBar()
        {
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
            if (backColor.A > 0)
            {
                graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, size));
                graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y, size.Width - 2, size.Height);
                graphics.FillRectangle(backBrush, absoluteLocation.X, absoluteLocation.Y + 1, size.Width, size.Height - 2);
            }

            graphics.FillRectangle(foreBrush, absoluteLocation.X + 1, absoluteLocation.Y, size.Width - 2, 1);
            graphics.FillRectangle(foreBrush, absoluteLocation.X + 1, absoluteLocation.Y + size.Height - 1, size.Width - 2, 1);
            graphics.FillRectangle(foreBrush, absoluteLocation.X, absoluteLocation.Y + 1, 1, size.Height - 2);
            graphics.FillRectangle(foreBrush, absoluteLocation.X + size.Width - 1, absoluteLocation.Y + 1, 1, size.Height - 2);

            for (int i = (int)(value / ((maximum - minimum) / ((size.Width - 8) / 12.0f)) - 1); i >= 0; --i)
            {
                graphics.FillRectangle(barBrush, absoluteLocation.X + 4 + i * 12, absoluteLocation.Y + 4, 8, size.Height - 8);
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
            return name + " - ProgressBar";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::ProgressBar();");
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
            if (size != new Size(110, 24))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFF5A5857)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (barColor != Color.FromArgb(unchecked((int)0xFF67AFF5)))
            {
                code.AppendLine(linePrefix + name + "->SetBarColor(OSHGui::Drawing::Color(" + barColor.A + ", " + barColor.R + ", " + barColor.G + ", " + barColor.B + "));");
            }
            if (minimum != 0)
            {
                code.AppendLine(linePrefix + name + "->SetMinimum(" + minimum + "));");
            }
            if (maximum != 100)
            {
                code.AppendLine(linePrefix + name + "->SetMaximum(" + maximum + "));");
            }
            if (value != 0)
            {
                code.AppendLine(linePrefix + name + "->SetValue(" + value + "));");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
        }
    }
}
