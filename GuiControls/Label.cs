using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class Label : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "label"; } }
        protected string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        public override Size Size { get { return base.Size; } set { if (!autoSize) { base.Size = value; } } }
        public override Font Font { get { return base.Font; } set { base.Font = value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        public override bool AutoSize { get { return base.AutoSize; } set { base.AutoSize = value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        #endregion

        public Label()
        {
            autoSize = true;

            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawString(text, font, foreBrush, new RectangleF(absoluteLocation, size));
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
            if (font.Bold || font.Italic || font.Size != 8 || font.Name != "Arial")
            {
                code.AppendLine(linePrefix + name + "->Font(Application::GetRenderer()->CreateNewFont(\"" + font.Name + "\", " + font.Size + ", " + font.Bold.ToString().ToLower() + ", " + font.Italic.ToString().ToLower() + "));");
            }
            code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + text.Replace("\"", "\\\"") + "\"));");
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
