using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    class TrackBar : BaseControl
    {
        #region Properties
        private int minimum;
        public int Minimum { get { return minimum; } set { if (value < maximum) { minimum = value; } } }
        private int maximum;
        public int Maximum { get { return maximum; } set { if (value > minimum) { maximum = value; } } }
        private int value;
        public int Value { get { return value; } set { if (value >= minimum && value < maximum) { this.value = value; } } }
        private int tickFrequency;
        public int TickFrequency { get { return tickFrequency; } set { if (value >= 1) { this.tickFrequency = value; } } }
        #endregion

        public TrackBar()
        {
            minimum = 1;
            maximum = 10;
            tickFrequency = 1;

            Size = new Size(110, 18);

            BackColor = Color.Empty;
            ForeColor = Color.FromArgb(unchecked((int)0xFFA6A4A1));
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, size));
            }

            int tickCount = 1 + (maximum - minimum) / tickFrequency;
            int pixelsPerTick = (size.Width - 8) / ((maximum - minimum) / tickFrequency);
            for (int i = 0; i < tickCount; ++i)
            {
                int x = absoluteLocation.X + 4 + i * pixelsPerTick;
                int y = absoluteLocation.Y + 7;
                graphics.FillRectangle(foreBrush, x, y, 1, 5);
            }

            int tick = value / tickFrequency;
            graphics.FillRectangle(foreBrush, absoluteLocation.X + tick * pixelsPerTick, absoluteLocation.Y + 1, 8, 16);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 1, absoluteLocation.Y - 1, size.Width + 1, size.Height + 1);
                }
            }
        }

        public override BaseControl Copy()
        {
            TrackBar copy = new TrackBar();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(BaseControl copy)
        {
            base.CopyTo(copy);

            TrackBar trackBar = copy as TrackBar;
            trackBar.minimum = minimum;
            trackBar.maximum = maximum;
            trackBar.tickFrequency = tickFrequency;
        }

        public override string ToString()
        {
            return name + " - TrackBar";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::TrackBar();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (size != new Size(110, 18))
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
            if (tickFrequency != 1)
            {
                code.AppendLine(linePrefix + name + "->SetTickFrequency(" + tickFrequency + "));");
            }
            return code.ToString();
        }
    }
}
