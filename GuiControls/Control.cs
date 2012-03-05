using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

namespace OSHVisualGui.GuiControls
{
    public enum ControlType
    {
        Button,
        CheckBox,
        ColorBar,
        ColorPicker,
        ComboBox,
        GroupBox,
        Label,
        LinkLabel,
        ListBox,
        Panel,
        PictureBox,
        ProgressBar,
        RadioButton,
        TabControl,
        TabPage,
        TextBox,
        Timer,
        TrackBar
    }

    public abstract class Control
    {
        internal virtual string DefaultName { get { return "form"; } }
        protected string name;
        public string Name { get { return name; } set { name = value; } }
        protected bool enabled;
        public virtual bool Enabled { get { return enabled; } set { enabled = value; } }
        protected bool visible;
        public virtual bool Visible { get { return visible; } set { visible = value; } }
        protected Point absoluteLocation;
        internal Point AbsoluteLocation { get { return absoluteLocation; } }
        protected Point location;
        public virtual Point Location { get { return location; } set { location = value; CalculateAbsoluteLocation(); } }
        protected Size size;
        public virtual Size Size { get { return size; } set { size = value; } }
        protected bool autoSize;
        public virtual bool AutoSize { get { return autoSize; } set { autoSize = value; } }
        protected Font font;
        public virtual Font Font { get { return font; } set { font = value; } }
        protected Brush foreBrush;
        protected Color foreColor;
        public virtual Color ForeColor { get { return foreColor; } set { foreColor = value; foreBrush = new SolidBrush(foreColor); } }
        protected Brush backBrush;
        protected Color backColor;
        public virtual Color BackColor { get { return backColor; } set { backColor = value; backBrush = new SolidBrush(backColor); } }
        internal int _zOrder;
        internal virtual int zOrder { get { return _zOrder; } set { _zOrder = value; RealParent.Sort(); } }

        protected Control parent;
        internal Control Parent { get { return parent; } set { parent = value; CalculateAbsoluteLocation(); } }
        internal ContainerControl RealParent
        {
            get
            {
                Control parent = Parent;
                while (parent.isSubControl && parent != this)
                {
                    parent = parent.Parent;
                }
                return parent as ContainerControl;
            }
        }

        public bool isFocused;
        public bool isHighlighted;
        public bool isSubControl;

        public Control()
        {
            enabled = true;
            visible = true;

            location = new Point(6, 6);

            isFocused = false;
            isHighlighted = false;
            isSubControl = false;

            _zOrder = 0;

            font = new Font("Arial", 8);
        }

        public virtual bool Intersect(Point location)
        {
            return ((location.X >= absoluteLocation.X && location.X <= absoluteLocation.X + size.Width)
                && (location.Y >= absoluteLocation.Y && location.Y <= absoluteLocation.Y + size.Height));
        }

        public virtual void CalculateAbsoluteLocation()
        {
            if (parent != null && parent != this)
            {
                absoluteLocation = parent.absoluteLocation.Add(location);
            }
            if (parent == this)
            {
                absoluteLocation = location;
            }
        }

        public abstract void Render(Graphics graphics);

        public abstract Control Copy();
        protected virtual void CopyTo(Control copy)
        {
            copy.name = DefaultName + ControlManager.Instance().GetControlCount(copy.GetType());
            copy.enabled = enabled;
            copy.visible = visible;
            copy.location = location;
            copy.size = size;
            copy.autoSize = autoSize;
            copy.font = font;
            copy.foreBrush = foreBrush;
            copy.foreColor = foreColor;
            copy.backBrush = backBrush;
            copy.backColor = backColor;
        }

        public override string ToString()
        {
            return name;
        }

        public abstract string ToCPlusPlusString(string linePrefix);
        public void AddToXmlElement(XmlDocument document, XmlElement element)
        {
            XmlElement control = document.CreateElement(DefaultName);
            WriteToXmlElement(document, control);
            element.AppendChild(control);
        }
        protected virtual void WriteToXmlElement(XmlDocument document, XmlElement element)
        {
            element.Attributes.Append(document.CreateValueAttribute("name", name));
            element.Attributes.Append(document.CreateValueAttribute("enabled", enabled.ToString().ToLower()));
            element.Attributes.Append(document.CreateValueAttribute("visible", visible.ToString().ToLower()));
            element.Attributes.Append(document.CreateValueAttribute("location", location.X + "," + location.Y));
            element.Attributes.Append(document.CreateValueAttribute("size", size.Width + "," + size.Height));
            element.Attributes.Append(document.CreateValueAttribute("autoSize", autoSize.ToString().ToLower()));
            element.Attributes.Append(document.CreateValueAttribute("font", font.Name + "," + font.Size + "," + font.Bold.ToString().ToLower() + "," + font.Italic.ToString().ToLower() + "," + font.Underline.ToString().ToLower()));
            element.Attributes.Append(document.CreateValueAttribute("foreColor", foreColor.ToArgb().ToString("X")));
            element.Attributes.Append(document.CreateValueAttribute("backColor", backColor.ToArgb().ToString("X")));
        }

        public abstract Control XmlElementToControl(XmlElement element);
        protected virtual void ReadFromXml(XmlElement element, Control control)
        {
            XmlAttributeCollection att = element.Attributes;
            if (att["name"] != null)
                control.Name = att["name"].Value.Trim();
            else
                throw new XmlException("Missing attribute 'name': " + element.Name);
            if (att["enabled"] != null)
                control.Enabled = att["enabled"].Value.Trim().ToLower() == "true";
            else
                throw new XmlException("Missing attribute 'enabled': " + element.Name);
            if (att["visible"] != null)
                control.Visible = bool.Parse(att["visible"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'visible': " + element.Name);
            if (att["location"] != null)
                control.Location = control.location.Parse(att["location"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'location': " + element.Name);
            if (att["size"] != null)
                control.Size = control.size.Parse(att["size"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'size': " + element.Name);
            if (att["autoSize"] != null)
                control.AutoSize = bool.Parse(att["autoSize"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'autoSize': " + element.Name);
            if (att["font"] != null)
                control.Font = control.font.Parse(att["font"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'font': " + element.Name);
            if (att["foreColor"] != null)
                control.ForeColor = control.foreColor.Parse(att["foreColor"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'foreColor': " + element.Name);
            if (att["backColor"] != null)
                control.BackColor = control.backColor.Parse(att["backColor"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'backColor': " + element.Name);
        }

        internal virtual void OnControlAdded()
        {
            ControlManager.Instance().AddControl(this);
        }
    }
}
