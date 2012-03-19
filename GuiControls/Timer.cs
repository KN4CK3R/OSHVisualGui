﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class Timer : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "timer"; } }
        private long interval;
        public long Interval { get { return interval; } set { if (value >= 1) { interval = value; } } }
        private Image icon;
        #endregion

        public Timer()
        {
            Type = ControlType.Timer;

            this.icon = Properties.Resources.control_timer;

            Enabled = false;
            Size = new Size(16, 16);

            interval = 100;

            Mode = DragMode.None;
        }

        public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            yield return new KeyValuePair<string, object>("SetName", Name);
            yield return new KeyValuePair<string, object>("SetEnabled", Enabled);
            yield return new KeyValuePair<string, object>("SetInterval", Interval);
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawImage(icon, absoluteLocation.X, absoluteLocation.Y, 16, 16);
        }

        public override Control Copy()
        {
            Timer copy = new Timer();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            Timer timer = copy as Timer;
            timer.interval = interval;
        }

        public override string ToString()
        {
            return name + " - Timer";
        }

        protected override void WriteToXmlElement(XElement element)
        {
            element.Add(new XAttribute("name", Name));
            element.Add(new XAttribute("location", location.X + "," + location.Y));
            element.Add(new XAttribute("enabled", Enabled.ToString().ToLower()));
            element.Add(new XAttribute("interval", Interval.ToString()));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            if (element.Attribute("name") != null)
                Name = element.Attribute("name").Value.Trim();
            else
                throw new Exception("Missing attribute 'name': " + element.Name);
            if (element.Attribute("location") != null)
                Location = location.Parse(element.Attribute("location").Value.Trim());
            if (element.Attribute("enabled") != null)
                Enabled = bool.Parse(element.Attribute("enabled").Value.Trim());
            else
                throw new Exception("Missing attribute 'enabled': " + element.Name);
            if (element.Attribute("interval") != null)
                Interval = long.Parse(element.Attribute("interval").Value.Trim());
            else
                throw new Exception("Missing attribute 'interval': " + element.Name);
        }
    }
}
