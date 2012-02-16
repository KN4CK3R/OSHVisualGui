using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSHGuiBuilder.Controls
{
    class Label : BaseControl
    {
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }

        public Label()
        {
            autoSize = true;

            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawString(text, font, foreBrush, new RectangleF(absoluteLocation, size));

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 1, absoluteLocation.Y - 1, size.Width + 1, size.Height + 1);
                }
            }
        }

        public override string ToString()
        {
            return name + " - Label";
        }

        public override string ToCPlusPlusString()
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(name + " = new Label();");
            code.AppendLine(name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(name + "->SetLocation(Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (!autoSize)
            {
                code.AppendLine(name + "->SetSize(Drawing::Size(" + size.Width + ", " + size.Height + "));");
                code.AppendLine(name + "->SetAutoSize(false);");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(name + "->SetBackColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(name + "->SetForeColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(name + "->SetText(\"" + text.Replace("\"", "\\\"") + "\");");
            code.AppendLine(parent.Name + "->AddControl(" + name + ");");
            return code.ToString();
        }
    }
}
