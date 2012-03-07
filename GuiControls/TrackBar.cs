using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace OSHVisualGui.GuiControls
{
    class TrackBar : Control
    {
        #region Properties
        internal override string DefaultName { get { return "trackBar"; } }
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

        public override Control Copy()
        {
            TrackBar copy = new TrackBar();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
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
            if (size != new Size(110, 18))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFA6A4A1)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (tickFrequency != 1)
            {
                code.AppendLine(linePrefix + name + "->SetTickFrequency(" + tickFrequency + "));");
            }
            if (minimum != 1)
            {
                code.AppendLine(linePrefix + name + "->SetMinimum(" + minimum + "));");
            }
            if (maximum != 10)
            {
                code.AppendLine(linePrefix + name + "->SetMaximum(" + maximum + "));");
            }
            if (value != 0)
            {
                code.AppendLine(linePrefix + name + "->SetValue(" + value + "));");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XmlDocument document, XmlElement element)
        {
            base.WriteToXmlElement(document, element);
            element.Attributes.Append(document.CreateValueAttribute("tickFrequency", tickFrequency.ToString()));
            element.Attributes.Append(document.CreateValueAttribute("minimum", minimum.ToString()));
            element.Attributes.Append(document.CreateValueAttribute("maximum", maximum.ToString()));
            element.Attributes.Append(document.CreateValueAttribute("value", value.ToString()));
        }

        public override void ReadPropertiesFromXml(XmlElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attributes["tickFrequency"] != null)
                TickFrequency = int.Parse(element.Attributes["textickFrequencyt"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'tickFrequency': " + element.Name);
            if (element.Attributes["minimum"] != null)
                Minimum = int.Parse(element.Attributes["minimum"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'minimum': " + element.Name);
            if (element.Attributes["maximum"] != null)
                Maximum = int.Parse(element.Attributes["maximum"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'maximum': " + element.Name);
            if (element.Attributes["value"] != null)
                Value = int.Parse(element.Attributes["value"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'value': " + element.Name);
        }
    }
}
