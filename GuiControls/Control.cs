using System;
using System.ComponentModel;
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
        Form,
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

    public class Mouse
    {
        public enum MouseStates
        {
            LeftDown,
            LeftUp,
            Move
        }

        public Point Location { get; set; }
        public MouseStates Buttons { get; set; }

        public Mouse(Point location, MouseStates buttons)
        {
            this.Location = location;
            this.Buttons = buttons;
        }
    }

    public abstract class Control
    {
        internal virtual string DefaultName { get { return "form"; } }
        internal ControlType Type;

        protected string name;
        public string Name { get { return name; } set { name = value; } }
        protected bool enabled;
        public virtual bool Enabled { get { return enabled; } set { enabled = value; } }
        protected bool visible;
        public virtual bool Visible { get { return visible; } set { visible = value; } }
        protected Point absoluteLocation;
        internal Point AbsoluteLocation { get { return absoluteLocation; } }
        protected Point location;
        protected Point defaultLocation;
        public virtual Point Location { get { return location; } set { location = value; CalculateAbsoluteLocation(); } }
        protected Size size;
        protected Size defaultSize;
        public virtual Size Size { get { return size; } set { size = value.LimitMin(5, 5); } }
        protected bool autoSize;
        protected bool defaultAutoSize;
        public virtual bool AutoSize { get { return autoSize; } set { autoSize = value; } }
        protected Font font;
        protected Font defaultFont;
        public virtual Font Font { get { return font; } set { font = value; } }
        protected Brush foreBrush;
        protected Color foreColor;
        protected Color defaultForeColor;
        public virtual Color ForeColor { get { return foreColor; } set { foreColor = value; foreBrush = new SolidBrush(foreColor); } }
        protected Brush backBrush;
        protected Color backColor;
        protected Color defaultBackColor;
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

        public bool isHighlighted;

        protected bool hasCaptured;
        protected bool isInside;
        protected bool isFocused;
        protected bool isClicked;
        public bool isSubControl;

        public delegate void MouseEventHandler(Control sender, Mouse mouse);
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseMove;

        public EventHandler MouseEnter;
        public EventHandler MouseLeave;
        public EventHandler GotFocus;
        public EventHandler LostFocus;

        [Category("Events")]
        public LocationChangedEvent LocationChangedEvent { get; set; }
        [Category("Events")]
        public SizeChangedEvent SizeChangedEvent { get; set; }
        [Category("Events")]
        public KeyDownEvent KeyDownEvent { get; set; }
        [Category("Events")]
        public KeyPressEvent KeyPressEvent { get; set; }
        [Category("Events")]
        public KeyUpEvent KeyUpEvent { get; set; }
        [Category("Events")]
        public ClickEvent ClickEvent { get; set; }
        [Category("Events")]
        public MouseClickEvent MouseClickEvent { get; set; }
        [Category("Events")]
        public MouseDownEvent MouseDownEvent { get; set; }
        [Category("Events")]
        public MouseUpEvent MouseUpEvent { get; set; }
        [Category("Events")]
        public MouseMoveEvent MouseMoveEvent { get; set; }
        [Category("Events")]
        public MouseScrollEvent MouseScrollEvent { get; set; }
        [Category("Events")]
        public MouseEnterEvent MouseEnterEvent { get; set; }
        [Category("Events")]
        public MouseLeaveEvent MouseLeaveEvent { get; set; }
        [Category("Events")]
        public MouseCaptureChangedEvent MouseCaptureChangedEvent { get; set; }
        [Category("Events")]
        public FocusGotEvent FocusGotEvent { get; set; }
        [Category("Events")]
        public FocusLostEvent FocusLostEvent { get; set; }

        public static Control MouseOverControl;
        public static Control FocusedControl;
        public static Control MouseCaptureControl;

        public Control()
        {
            enabled = true;
            visible = true;
            defaultAutoSize = false;

            defaultLocation = location = new Point(6, 6);

            isFocused = false;
            isHighlighted = false;
            isSubControl = false;

            _zOrder = 0;

            defaultFont = font = new Font("Arial", 8);

            LocationChangedEvent = new LocationChangedEvent(this);
            SizeChangedEvent = new SizeChangedEvent(this);
            KeyDownEvent = new KeyDownEvent(this);
            KeyPressEvent = new KeyPressEvent(this);
            KeyUpEvent = new KeyUpEvent(this);
            ClickEvent = new ClickEvent(this);
            MouseClickEvent = new MouseClickEvent(this);
            MouseDownEvent = new MouseDownEvent(this);
            MouseUpEvent = new MouseUpEvent(this);
            MouseMoveEvent = new MouseMoveEvent(this);
            MouseScrollEvent = new MouseScrollEvent(this);
            MouseEnterEvent = new MouseEnterEvent(this);
            MouseLeaveEvent = new MouseLeaveEvent(this);
            MouseCaptureChangedEvent = new MouseCaptureChangedEvent(this);
            FocusGotEvent = new FocusGotEvent(this);
            FocusLostEvent = new FocusLostEvent(this);
        }

        public virtual IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            yield return new KeyValuePair<string, object>("SetName", Name);
            if (!Enabled)
                yield return new KeyValuePair<string, object>("SetEnabled", Enabled);
            if (!Visible)
                yield return new KeyValuePair<string, object>("SetVisible", Visible);
            if (Location != defaultLocation)
                yield return new KeyValuePair<string, object>("SetLocation", Location);
            if (Size != defaultSize)
                yield return new KeyValuePair<string, object>("SetSize", Size);
            if (AutoSize != defaultAutoSize)
                yield return new KeyValuePair<string, object>("SetAutoSize", AutoSize);
            if (!this.Font.Equals(defaultFont))
                yield return new KeyValuePair<string, object>("SetFont", Font);
            if (ForeColor != defaultForeColor)
                yield return new KeyValuePair<string, object>("SetForeColor", ForeColor);
            if (BackColor != defaultBackColor)
                yield return new KeyValuePair<string, object>("SetBackColor", BackColor);
        }

        public virtual IEnumerable<Event> GetUsedEvents()
        {
            foreach (var property in GetType().GetProperties())
            {
                if (property.Name.Contains("Event"))
                {
                    Event controlEvent = property.GetValue(this, null) as Event;
                    if (!controlEvent.IsEmpty)
                    {
                        yield return controlEvent;
                    }
                }
            }
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

        public abstract string ToCPlusPlusString(string prefix);

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
            foreach (var controlEvent in GetUsedEvents())
            {
                element.Add(new XAttribute(controlEvent.GetType().Name, controlEvent.Code.ToBase64String()));
            }
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

        public void Focus()
        {
            OnGotFocus(this);
        }

        #region EventHandling

        protected virtual void OnGotFocus(Control control)
        {
            if (FocusedControl != this)
            {
                if (FocusedControl != null)
                {
                    FocusedControl.OnLostFocus(this);
                }
                FocusedControl = this;
                isFocused = true;

                if (GotFocus != null)
                {
                    GotFocus(this, null);
                }
            }
        }

        protected virtual void OnLostFocus(Control control)
        {
            isFocused = false;
            isClicked = false;

            FocusedControl = null;

            if (LostFocus != null)
            {
                LostFocus(this, null);
            }
        }

        protected virtual void OnMouseDown(Mouse mouse)
        {
            isClicked = true;

            OnGotMouseCapture();

            if (MouseDown != null)
            {
                MouseDown(this, mouse);
            }
        }

        protected virtual void OnMouseUp(Mouse mouse)
        {
            isClicked = false;

            OnLostMouseCapture();

            if (MouseUp != null)
            {
                MouseUp(this, mouse);
            }
        }

        protected virtual void OnMouseMove(Mouse mouse)
        {
            if (MouseMove != null)
            {
                MouseMove(this, mouse);
            }
        }

        protected virtual void OnGotMouseCapture()
        {
            if (MouseCaptureControl != null)
            {
                MouseCaptureControl.OnLostMouseCapture();
            }
            MouseCaptureControl = this;

            hasCaptured = true;
            isClicked = false;
        }

        protected virtual void OnLostMouseCapture()
        {
            hasCaptured = false;

            MouseCaptureControl = null;
        }

        protected virtual void OnMouseEnter()
        {
            isInside = true;

            if (MouseOverControl != null)
            {
                MouseOverControl.OnMouseLeave();
            }

            if (MouseEnter != null)
            {
                MouseEnter(this, null);
            }
        }

        protected virtual void OnMouseLeave()
        {
            isInside = false;

            MouseOverControl = null;

            if (MouseLeave != null)
            {
                MouseLeave(this, null);
            }
        }

        public bool ProcessMouseMessage(Mouse mouse)
        {
            if (!Enabled)
            {
                return false;
            }

            switch (mouse.Buttons)
            {
                case Mouse.MouseStates.LeftDown:
                    if (Intersect(mouse.Location))
                    {
                        OnMouseDown(mouse);

                        if (!isFocused && !isSubControl)
                        {
                            OnGotFocus(this);
                        }

                        return true;
                    }
                    break;
                case Mouse.MouseStates.LeftUp:
                    if (hasCaptured || Intersect(mouse.Location))
                    {
                        OnMouseUp(mouse);

                        return true;
                    }
                    break;
                case Mouse.MouseStates.Move:
                    if (hasCaptured || Intersect(mouse.Location))
                    {
                        if (!isInside)
                        {
                            OnMouseEnter();
                        }

                        OnMouseMove(mouse);

                        return true;
                    }
                    break;
            }

            return false;
        }

        #endregion
    }
}
