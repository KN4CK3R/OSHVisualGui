using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Linq;

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

        public XElement SerializeToXml()
        {
            XElement control = new XElement(DefaultName);
            WriteToXmlElement(control);
            return control;
        }

        protected virtual void WriteToXmlElement(XElement element)
        {
            element.Add(new XAttribute("name", name));
            element.Add(new XAttribute("enabled", enabled.ToString().ToLower()));
            element.Add(new XAttribute("visible", visible.ToString().ToLower()));
            element.Add(new XAttribute("location", location.X + "," + location.Y));
            element.Add(new XAttribute("size", size.Width + "," + size.Height));
            element.Add(new XAttribute("autoSize", autoSize.ToString().ToLower()));
            element.Add(new XAttribute("font", font.Name + "," + font.Size + "," + font.Bold.ToString().ToLower() + "," + font.Italic.ToString().ToLower() + "," + font.Underline.ToString().ToLower()));
            element.Add(new XAttribute("foreColor", foreColor.ToArgb().ToString("X")));
            element.Add(new XAttribute("backColor", backColor.ToArgb().ToString("X")));
        }

        public virtual void ReadPropertiesFromXml(XElement element)
        {
            if (element.Attribute("name") != null)
                Name = element.Attribute("name").Value.Trim();
            else
                throw new Exception("Missing attribute 'name': " + element.Name);
            if (element.Attribute("enabled") != null)
                Enabled = element.Attribute("enabled").Value.Trim().ToLower() == "true";
            else
                throw new Exception("Missing attribute 'enabled': " + element.Name);
            if (element.Attribute("visible") != null)
                Visible = bool.Parse(element.Attribute("visible").Value.Trim());
            else
                throw new Exception("Missing attribute 'visible': " + element.Name);
            if (element.Attribute("location") != null)
                Location = location.Parse(element.Attribute("location").Value.Trim());
            else
                throw new Exception("Missing attribute 'location': " + element.Name);
            if (element.Attribute("size") != null)
                Size = size.Parse(element.Attribute("size").Value.Trim());
            else
                throw new Exception("Missing attribute 'size': " + element.Name);
            if (element.Attribute("autoSize") != null)
                AutoSize = bool.Parse(element.Attribute("autoSize").Value.Trim());
            else
                throw new Exception("Missing attribute 'autoSize': " + element.Name);
            if (element.Attribute("font") != null)
                Font = font.Parse(element.Attribute("font").Value.Trim());
            else
                throw new Exception("Missing attribute 'font': " + element.Name);
            if (element.Attribute("foreColor") != null)
                ForeColor = foreColor.Parse(element.Attribute("foreColor").Value.Trim());
            else
                throw new Exception("Missing attribute 'foreColor': " + element.Name);
            if (element.Attribute("backColor") != null)
                BackColor = backColor.Parse(element.Attribute("backColor").Value.Trim());
            else
                throw new Exception("Missing attribute 'backColor': " + element.Name);
        }

        internal virtual void RegisterInternalControls()
        {

        }

        internal virtual void UnregisterInternalControls()
        {

        }
    }
}
