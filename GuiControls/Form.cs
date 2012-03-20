﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class Form : ContainerControl
    {
        private Panel panel;
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }
        public override Point Location { get { return base.Location; } set { } }
        internal override List<Control> Controls { get { return panel.Controls; } }
        internal override Point ContainerLocation { get { return base.ContainerLocation.Add(panel.Location); } }
        internal override Point ContainerAbsoluteLocation { get { return panel.ContainerAbsoluteLocation; } }
        internal override Size ContainerSize { get { return panel.ContainerSize; } }
        public override Size Size { get { return base.Size; } set { if (value.Width > 80 && value.Height > 50) { base.Size = value; panel.Size = new Size(value.Width - 2 * 6, value.Height - 17 - 2 * 6); } } }

        public Form()
        {
            Type = ControlType.Form;

            Parent = this;

            Mode = DragMode.GrowOnly;

            panel = new Panel();
            panel.Location = new Point(6, 6 + 17);
            panel.isSubControl = true;
            AddSubControl(panel);

            defaultSize = Size = new Size(300, 300);

            defaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF7C7B79));
            defaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(Control control)
        {
            panel.AddControl(control);
        }

        public override void RemoveControl(Control control)
        {
            panel.RemoveControl(control);
        }

        public override void Render(Graphics graphics)
        {
            Rectangle rect = new Rectangle(absoluteLocation, size);
            LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 100, 100, 100)), LinearGradientMode.Vertical);

            graphics.FillRectangle(linearBrush, rect);
            graphics.DrawString(text, font, foreBrush, new PointF(absoluteLocation.X + 4, absoluteLocation.Y + 2));
            graphics.FillRectangle(new SolidBrush(backColor.Substract(Color.FromArgb(0, 50, 50, 50))), absoluteLocation.X + 5, absoluteLocation.Y + 17 + 2, size.Width - 10, 1);

            Point crossLocation = new Point(absoluteLocation.X + size.Width - 17, absoluteLocation.Y + 5); 
            for (int i = 0; i < 4; ++i)
            {
                graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + 7 - i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + 7 - i, 3, 1);
            }

            panel.Render(graphics);

            if (isHighlighted)
            {
                using (Pen pen = new Pen(Color.Orange, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 3);
                }

                isHighlighted = false;
            }
        }

        public override Control Copy()
        {
            throw new NotImplementedException();
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);
        }

        public override string ToString()
        {
            return name + " - Form";
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);

            element.Add(new XAttribute("text", Text));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("text") != null)
                Text = element.Attribute("text").Value.Trim();
            else
                throw new Exception("Missing attribute 'text': " + element.Name);
        }
    }
}
