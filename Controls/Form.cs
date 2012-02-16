using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHGuiBuilder.Controls
{
    class Form : ContainerControl
    {
        private Panel panel;
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }

        public Form()
        {
            Parent = this;

            Size = new Size(300, 300);

            panel = new Panel();
            panel.Location = new Point(6, 6 + 17);
            panel.isSubControl = true;
            AddSubControl(panel);

            BackColor = Color.FromArgb(unchecked((int)0xFF7C7B79));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(BaseControl control)
        {
            panel.AddControl(control);
        }

        public override void Render(System.Drawing.Graphics graphics)
        {
            Rectangle rect = new Rectangle(absoluteLocation, size);
            LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 100, 100, 100)), LinearGradientMode.Vertical);

            graphics.FillRectangle(linearBrush, rect);
            graphics.DrawString(text, font, foreBrush, new PointF(absoluteLocation.X + 4, absoluteLocation.Y + 2));
            graphics.FillRectangle(new SolidBrush(backColor.Substract(Color.FromArgb(0, 50, 50, 50))), absoluteLocation.X + 5, absoluteLocation.Y + 17 + 2, size.Width - 10, 1);

            panel.Render(graphics);
        }

        public override string ToString()
        {
            return name + " - Form";
        }
    }
}
