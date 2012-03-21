﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class TrackBar : ScalableControl
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

        [Category("Events")]
        public ValueChangedEvent ValueChangedEvent { get; set; }
        #endregion

        public TrackBar()
        {
            Type = ControlType.TrackBar;

            minimum = 1;
            maximum = 10;
            tickFrequency = 1;

            Size = new Size(110, 18);

            BackColor = Color.Empty;
            ForeColor = Color.FromArgb(unchecked((int)0xFFA6A4A1));

            ValueChangedEvent = new ValueChangedEvent(this);
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

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
            element.Add(new XAttribute("tickFrequency", tickFrequency.ToString()));
            element.Add(new XAttribute("minimum", minimum.ToString()));
            element.Add(new XAttribute("maximum", maximum.ToString()));
            element.Add(new XAttribute("value", value.ToString()));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("tickFrequency") != null)
                TickFrequency = int.Parse(element.Attribute("tickFrequency").Value.Trim());
            else
                throw new Exception("Missing attribute 'tickFrequency': " + element.Name);
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
