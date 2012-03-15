using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class TextBox : Control
    {
        #region Properties
        internal override string DefaultName { get { return "textBox"; } }
        private string text = string.Empty;
        public string Text { get { return text; } set { text = value; } }
        #endregion

        public TextBox()
        {
            Size = new Size(100, 24);

            BackColor = Color.FromArgb(unchecked((int)0xFF242321));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void Render(Graphics graphics)
        {
            Brush tempBrush = new SolidBrush(backColor.Add(Color.FromArgb(0, 20, 20, 20)));
		    graphics.FillRectangle(tempBrush, new Rectangle(absoluteLocation, size));
            graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);
		
		    graphics.DrawString(text, font, foreBrush, new RectangleF(absoluteLocation.X + 5, absoluteLocation.Y + 6, size.Width - 10, size.Height - 12));

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
            TextBox copy = new TextBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            TextBox textBox = copy as TextBox;
            textBox.text = text;
        }

        public override string ToString()
        {
            return name + " - TextBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::TextBox();");
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
            if (size != new Size(100, 24))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF242321)))
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (!string.IsNullOrEmpty(text))
            {
                code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + text.Replace("\"", "\\\"") + "\"));");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
            element.Add(new XAttribute("text", text));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("text") != null)
                Text = element.Attribute("text").Value.Trim();
            else
                throw new Exception("Missing attribute 'text': " + element.Name);
        }
    }
}
