using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
    public class Panel : ContainerControl
    {
        #region Properties
        internal override string DefaultName { get { return "panel"; } }
        #endregion

        public Panel()
        {
            Type = ControlType.Panel;

            DefaultSize = Size = new Size(200, 200);

            DefaultBackColor = BackColor = Color.Empty;
            DefaultForeColor = ForeColor = Color.Empty;
        }

        public override void Render(Graphics graphics)
        {
            if (BackColor.A > 0)
            {
                Rectangle rect = new Rectangle(AbsoluteLocation, Size);
                LinearGradientBrush linearBrush = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 90, 90, 90)), LinearGradientMode.Vertical);
                graphics.FillRectangle(linearBrush, rect);
            }

            graphics.DrawString(Name, Font, new SolidBrush(Color.Black), AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5);
            
            base.Render(graphics);

            if (isHighlighted)
            {
                using (Pen pen = new Pen(Color.Orange, 1))
                {
                    graphics.DrawRectangle(pen, AbsoluteLocation.X - 3, AbsoluteLocation.Y - 2, Size.Width + 5, Size.Height + 4);
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
            return Name + " - Panel";
        }
    }
}
