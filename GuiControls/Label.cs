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
        protected string defaultText;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        public override Size Size { get { return base.Size; } set { if (!autoSize) { base.Size = value; } } }
        public override Font Font { get { return base.Font; } set { base.Font = value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        public override bool AutoSize { get { return base.AutoSize; } set { base.AutoSize = value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }
        #endregion

        public Label()
        {
            Type = ControlType.Label;

            defaultText = text = string.Empty;

            defaultAutoSize = autoSize = true;

            defaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            foreach (var pair in base.GetChangedProperties())
            {
                yield return pair;
            }
            if (Text != defaultText)
            {
                yield return new KeyValuePair<string, object>("SetText", Text);
            }
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
