using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
    public class Label : Control
    {
        #region Properties
        internal override string DefaultName { get { return "label"; } }
        protected string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        #endregion

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

        public override Control Copy()
        {
            Label copy = new Label();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);
            
            Label label = copy as Label;
            label.text = text;
        }

        public override string ToString()
        {
            return name + " - Label";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::Label();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (!autoSize)
            {
                code.AppendLine(linePrefix + name + "->SetAutoSize(false);");
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + text.Replace("\"", "\\\"") + "\"));");
            return code.ToString();
        }
    }
}
