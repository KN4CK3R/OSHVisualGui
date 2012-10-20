using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class GroupBox : ContainerControl
    {
        #region Properties
        private Label label = new Label();
        private Panel panel = new Panel();

        internal override string DefaultName { get { return "groupBox"; } }
        protected string DefaultText;
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; } }
        internal override List<Control> Controls { get { return panel.Controls; } }
        public override Size Size { get { return base.Size; } set { base.Size = value.LimitMin(label.Size.Width + 10, 17); panel.Size = base.Size.Add(new Size(-3 * 2, -3 * 2 - 10)); } }
        internal override Point ContainerLocation { get { return base.ContainerLocation.Add(panel.Location); } }
        internal override Point ContainerAbsoluteLocation { get { return panel.ContainerAbsoluteLocation; } }
        internal override Size ContainerSize { get { return panel.ContainerSize; } }
		public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
        #endregion

        public GroupBox()
        {
            Type = ControlType.GroupBox;

            DefaultText = string.Empty;

            DefaultSize = Size = new Size(200, 200);

            label.Location = new Point(5, -1);
            label.isSubControl = true;
            AddSubControl(label);

            panel.Location = new Point(3, 10);
            panel.isSubControl = true;
            AddSubControl(panel);

            DefaultBackColor = BackColor = Color.Empty;
            DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            foreach (var pair in base.GetChangedProperties())
            {
                yield return pair;
            }
            if (Text != DefaultText)
            {
                yield return new KeyValuePair<string, object>("SetText", Text);
            }
        }

        public override void AddControl(Control control)
        {
            panel.AddControl(control);
        }

        public override void RemoveControl(Control control)
        {
            panel.RemoveControl(control);
        }

        public override void Render(Graphics graphics)
        {
            if (BackColor.A > 0)
            {
                graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
            }
            label.Render(graphics);

            graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + 5, 3, 1);
            graphics.FillRectangle(foreBrush, AbsoluteLocation.X + label.Size.Width + 5, AbsoluteLocation.Y + 5, Size.Width - label.Size.Width - 6, 1);
            graphics.FillRectangle(foreBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 6, 1, Size.Height - 7);
            graphics.FillRectangle(foreBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 6, 1, Size.Height - 7);
            graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 1, Size.Width - 2, 1);

            panel.Render(graphics);

            if (isHighlighted)
            {
                using (Pen pen = new Pen(Color.Orange, 1))
                {
                    graphics.DrawRectangle(pen, AbsoluteLocation.X - 3, AbsoluteLocation.Y - 2, Size.Width + 5, Size.Height + 4);
                }

                isHighlighted = false;
            }
        }

        public override Control Copy()
        {
            GroupBox copy = new GroupBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            GroupBox groupBox = copy as GroupBox;
            groupBox.Text = Text;

            foreach (Control control in panel.Controls)
            {
                groupBox.AddControl(control.Copy());
            }
        }

        public override string ToString()
        {
            return Name + " - GroupBox";
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
                Text = element.Attribute("text").Value;
            else
                throw new Exception("Missing attribute 'text': " + element.Name);
        }
    }
}
