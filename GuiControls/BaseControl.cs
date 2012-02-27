using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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

    public abstract class Control : IComparable<Control>
    {
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
        protected int _zOrder;
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
            copy.name = name + "_copy";
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

        public int CompareTo(Control control)
        {
            return _zOrder.CompareTo(control._zOrder);
        }
    }
}
