using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHGuiBuilder.GuiControls
{
    public class Panel : ContainerControl
    {
        public Panel()
        {
            Size = new Size(200, 200);

            BackColor = Color.Empty;
            ForeColor = Color.Empty;
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

        public override BaseControl Copy()
        {
            Panel copy = new Panel();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(BaseControl copy)
        {
            base.CopyTo(copy);

            Panel panel = copy as Panel;
            foreach (BaseControl control in PreOrderVisit())
            {
                panel.AddControl(control.Copy());
            }
        }

        public override string ToString()
        {
            return name + " - Panel";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::Panel();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (size != new Size(200, 200))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (foreColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            return code.ToString();
        }
    }
}
