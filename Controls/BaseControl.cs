using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OSHGuiBuilder.Controls
{
    public abstract class BaseControl
    {
        protected string name;
        public string Name { get { return name; } set { name = value; } }
        protected bool enabled;
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        protected bool visible;
        public bool Visible { get { return visible; } set { visible = value; } }
        protected Point absoluteLocation;
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

        protected BaseControl parent;
        public BaseControl Parent { get { return parent; } set { parent = value; CalculateAbsoluteLocation(); } }

        public bool isFocused;
        public bool isSubControl;

        public BaseControl()
        {
            enabled = true;
            visible = true;

            location = new Point(6, 6);

            isFocused = false;
            isSubControl = false;

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

        public override string ToString()
        {
            return name;
        }
    }
}
