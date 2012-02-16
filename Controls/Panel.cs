using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHGuiBuilder.Controls
{
    class Panel : ContainerControl
    {
        public Panel()
        {
            Size = new Size(200, 200);

            ForeColor = Color.Empty;
            BackColor = Color.Empty;
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                Rectangle rect = new Rectangle(absoluteLocation, size);
                LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 90, 90, 90)), LinearGradientMode.Vertical);
                graphics.FillRectangle(linearBrush, rect);
            }
            
            base.Render(graphics);
        }
    }
}
