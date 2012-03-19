using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class Button : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "button"; } }
        private Label label;

        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
        protected string defaultText;
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (autoSize) { Size = new Size(label.Size.Width + 12, label.Size.Height + 10); } CalculateLabelLocation(); } }
        public override Size Size { get { return base.Size; } set { base.Size = value; CalculateLabelLocation(); } }
        #endregion

        public Button()
        {
            Type = ControlType.Button;

            label = new Label();
            label.Location = new Point(6, 5);

            defaultText = string.Empty;

            defaultSize = Size = new Size(92, 24);

            defaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));
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

        private void CalculateLabelLocation()
        {
            label.Location = new Point(Size.Width / 2 - label.Size.Width / 2, Size.Height / 2 - label.Size.Height / 2);
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

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
            element.Add(new XAttribute("text", Text));
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
