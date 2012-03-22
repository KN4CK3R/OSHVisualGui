using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class CheckBox : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "checkBox"; } }

        protected Label label;

        protected bool _checked;
        protected bool DefaultChecked;
        public virtual bool Checked { get { return _checked; } set { _checked = value; } }
        public override Size Size { get { return base.Size; } set { Size tempSize = value.LimitMin(20, 14); if (!AutoSize) { base.Size = tempSize; label.Size = tempSize; } } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
        protected string DefaultText;
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (AutoSize) { base.Size = new Size(label.Size.Width + 20, label.Size.Height + 2); } } }

        [Category("Events")]
        public CheckedChangedEvent CheckedChangedEvent { get; set; }
        #endregion

        public CheckBox()
        {
            Type = ControlType.CheckBox;

            label = new Label();
            label.Location = new Point(20, 2);

            DefaultChecked = false;
            DefaultText = string.Empty;

            DefaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF222222));
            DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

            DefaultAutoSize = AutoSize = true;

            CalculateAbsoluteLocation();

            CheckedChangedEvent = new CheckedChangedEvent(this);
        }

        public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            foreach (var pair in base.GetChangedProperties())
            {
                yield return pair;
            }
            if (Checked != DefaultChecked)
            {
                yield return new KeyValuePair<string, object>("SetChecked", Checked);
            }
            if (Text != DefaultText)
            {
                yield return new KeyValuePair<string, object>("SetText", Text);
            }
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            label.Parent = this;
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, new Size(17, 17)));
            Rectangle rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, 15, 15);
            LinearGradientBrush temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);
            rect = new Rectangle(AbsoluteLocation.X + 2, AbsoluteLocation.Y + 2, 13, 13);
            temp = new LinearGradientBrush(rect, BackColor, BackColor.Add(Color.FromArgb(0, 55, 55, 55)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);

            if (_checked)
            {
                graphics.FillRectangle(new SolidBrush(Color.White), AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5, 7, 7);
                rect = new Rectangle(AbsoluteLocation.X + 6, AbsoluteLocation.Y + 6, 5, 5);
                temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
                graphics.FillRectangle(temp, rect);
            }
            label.Render(graphics);
        }

        public override Control Copy()
        {
            CheckBox copy = new CheckBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            CheckBox checkBox = copy as CheckBox;
            checkBox._checked = _checked;
            checkBox.Text = Text;
        }

        public override string ToString()
        {
            return Name + " - CheckBox";
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
            element.Add(new XAttribute("text", Text));
            element.Add(new XAttribute("checked", Checked.ToString().ToLower()));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("text") != null)
                Text = element.Attribute("text").Value.Trim();
            else
                throw new Exception("Missing attribute 'text': " + element.Name);
            if (element.Attribute("text") != null)
                Checked = bool.Parse(element.Attribute("checked").Value.Trim());
            else
                throw new Exception("Missing attribute 'checked': " + element.Name);
        }
    }
}
