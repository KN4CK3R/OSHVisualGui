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

        public override string ToCPlusPlusString()
        {
            return "";

            StringBuilder code = new StringBuilder();
            code.AppendLine(name + " = new Button();");
            code.AppendLine(name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(name + "->SetLocation(Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (autoSize)
            {
                code.AppendLine(name + "->SetAutoSize(true);");
            }
            else
            {
                code.AppendLine(name + "->SetSize(Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF4E4E4E)))
            {
                code.AppendLine(name + "->SetBackColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(name + "->SetForeColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(parent.Name + "->AddControl(" + name + ");");
            return code.ToString();
        }
    }
}
