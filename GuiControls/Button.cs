using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace OSHVisualGui.GuiControls
{
    public class Button : Control
    {
        #region Properties
        internal override string DefaultName { get { return "button"; } }
        private Label label;

        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
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
            Brush tempBrush = new SolidBrush(backColor.Add(Color.FromArgb(0, 10, 10, 10)));
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y, size.Width - 2, size.Height - 1);
            graphics.FillRectangle(tempBrush, absoluteLocation.X, absoluteLocation.Y + 1, size.Width, size.Height - 3);
            tempBrush = new SolidBrush(backColor.Substract(Color.FromArgb(0, 50, 50, 50)));
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y + size.Height - 2, size.Width - 2, 2);
            graphics.FillRectangle(tempBrush, absoluteLocation.X + size.Width - 1, absoluteLocation.Y + 1, 1, size.Height - 2);
            Rectangle rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 2, size.Width - 2, size.Height - 4);
            LinearGradientBrush temp = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);
            rect = new Rectangle(absoluteLocation.X + 2, absoluteLocation.Y + 1, size.Width - 4, size.Height - 2);
            temp = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);
            
            label.Render(graphics);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 3);
                }
            }
        }

        public override Control Copy()
        {
            Button copy = new Button();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            Button button = copy as Button;
            button.Text = Text;
        }

        public override string ToString()
        {
            return name + " - Button";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::Button();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(linePrefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(linePrefix + name + "->SetVisible(false);");
            }
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (autoSize)
            {
                code.AppendLine(linePrefix + name + "->SetAutoSize(true);");
            }
            else if (size != new Size(92, 24))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF4E4E4E)))
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + Text.Replace("\"", "\\\"") + "\"));");
            return code.ToString();
        }

        protected override void WriteToXmlElement(XmlDocument document, XmlElement element)
        {
            base.WriteToXmlElement(document, element);
            element.Attributes.Append(document.CreateValueAttribute("text", Text));
        }

        public override Control XmlElementToControl(XmlElement element)
        {
            Button button = new Button();
            ReadFromXml(element, button);
            return button;
        }
        protected override void ReadFromXml(XmlElement element, Control control)
        {
            base.ReadFromXml(element, control);

            Button button = control as Button;
            if (element.Attributes["text"] != null)
                button.Text = element.Attributes["text"].Value.Trim();
            else
                throw new XmlException("Missing attribute 'text': " + element.Name);
        }
    }
}
