using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class Panel : ContainerControl
    {
        #region Properties
        internal override string DefaultName { get { return "panel"; } }
        #endregion

        public Panel()
        {
            Type = ControlType.Panel;

            defaultSize = Size = new Size(200, 200);

            defaultBackColor = BackColor = Color.Empty;
            defaultForeColor = ForeColor = Color.Empty;
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                Rectangle rect = new Rectangle(absoluteLocation, size);
                LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 90, 90, 90)), LinearGradientMode.Vertical);
                graphics.FillRectangle(linearBrush, rect);
            }

            graphics.DrawString(name, font, new SolidBrush(Color.Black), absoluteLocation.X + 5, absoluteLocation.Y + 5);
            
            base.Render(graphics);

            if (isHighlighted)
            {
                using (Pen pen = new Pen(Color.Orange, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
                }

                isHighlighted = false;
            }
        }

        public override Control Copy()
        {
            Panel copy = new Panel();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            Panel panel = copy as Panel;
            foreach (Control control in PreOrderVisit())
            {
                panel.AddControl(control.Copy());
            }
        }

        public override string ToString()
        {
            return name + " - Panel";
        }
    }
}
