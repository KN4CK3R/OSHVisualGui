using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class TextBox : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "textBox"; } }
        private string text;
        protected string defaultText;
        public string Text { get { return text; } set { text = value; } }
        #endregion

        public TextBox()
        {
            Type = ControlType.TextBox;

            defaultText = text = string.Empty;

            defaultSize = Size = new Size(100, 24);

            defaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF242321));
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
            Brush tempBrush = new SolidBrush(backColor.Add(Color.FromArgb(0, 20, 20, 20)));
		    graphics.FillRectangle(tempBrush, new Rectangle(absoluteLocation, size));
            graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);
		
		    graphics.DrawString(text, font, foreBrush, new RectangleF(absoluteLocation.X + 5, absoluteLocation.Y + 6, size.Width - 10, size.Height - 12));
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
