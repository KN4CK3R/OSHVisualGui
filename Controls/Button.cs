using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OSHGuiBuilder.Controls
{
    class Button : BaseControl
    {
        #region Properties
        private Label label;

        public override Color ForeColor { get { return label.ForeColor; } set { label.ForeColor = value; } }
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (autoSize) { Size = new Size(label.Size.Width + 12, label.Size.Height + 10); } } }
        #endregion

        public Button()
        {
            label = new Label();
            label.Location = new Point(6, 5);

            Size = new Size(92, 24);

            BackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            label.Parent = this;
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, size));
            label.Render(graphics);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 3);
                }
            }
        }

        public override string ToString()
        {
            return name + " - Button";
        }

        public override string ToCPlusPlusString()
        {
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
            code.AppendLine(name + "->SetText(\"" + Text.Replace("\"", "\\\"") + "\");");
            code.AppendLine(parent.Name + "->AddControl(" + name + ");");
            return code.ToString();
        }
    }
}
