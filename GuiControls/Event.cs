using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [Editor(typeof(EventEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Event
    {
        private Control control;
        [Browsable(false)]
        public Control Control { get { return control; } }
        private string code;
        [Browsable(false)]
        public string Code { get { return code; } set { code = value; } }
        protected string stub;
        [Browsable(false)]
        public string Stub { get { return stub; } }
        [Browsable(false)]
        public bool IsEmpty { get { return string.IsNullOrEmpty(code); } }

        protected Event(Control control, string stub)
        {
            if (string.IsNullOrEmpty(stub))
            {
                throw new ArgumentNullException("stub");
            }

            code = string.Empty;
            this.control = control;
            this.stub = stub;
        }

        public override string ToString()
        {
            return code;
        }
    }

    public class ClickEvent : Event
    {
        public ClickEvent(Control control)
            : base(control, "_Clicked(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class CheckedChangedEvent : Event
    {
        public CheckedChangedEvent(Control control)
            : base(control, "_CheckedChanged(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class LocationChangedEvent : Event
    {
        public LocationChangedEvent(Control control)
            : base(control, "_LocationChanged(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class SizeChangedEvent : Event
    {
        public SizeChangedEvent(Control control)
            : base(control, "_SizeChanged(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class KeyDownEvent : Event
    {
        public KeyDownEvent(Control control)
            : base(control, "_KeyDown(Control *sender, KeyEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class KeyPressEvent : Event
    {
        public KeyPressEvent(Control control)
            : base(control, "_KeyPress(Control *sender, KeyPressEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class KeyUpEvent : Event
    {
        public KeyUpEvent(Control control)
            : base(control, "_KeyUp(Control *sender, KeyEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseClickEvent : Event
    {
        public MouseClickEvent(Control control)
            : base(control, "_MouseClick(Control *sender, MouseEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseDownEvent : Event
    {
        public MouseDownEvent(Control control)
            : base(control, "_MouseDown(Control *sender, MouseEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseUpEvent : Event
    {
        public MouseUpEvent(Control control)
            : base(control, "_MouseUp(Control *sender, MouseEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseMoveEvent : Event
    {
        public MouseMoveEvent(Control control)
            : base(control, "_MouseMove(Control *sender, MouseEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseScrollEvent : Event
    {
        public MouseScrollEvent(Control control)
            : base(control, "_MouseScroll(Control *sender, MouseEventArgs &e)\n{\n\t\n}")
        {

        }
    }

    public class MouseEnterEvent : Event
    {
        public MouseEnterEvent(Control control)
            : base(control, "_MouseEnter(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class MouseLeaveEvent : Event
    {
        public MouseLeaveEvent(Control control)
            : base(control, "_MouseLeave(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class MouseCaptureChangedEvent : Event
    {
        public MouseCaptureChangedEvent(Control control)
            : base(control, "_MouseCaptureChanged(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class FocusGotEvent : Event
    {
        public FocusGotEvent(Control control)
            : base(control, "_FocusGot(Control *sender)\n{\n\t\n}")
        {

        }
    }

    public class FocusLostEvent : Event
    {
        public FocusLostEvent(Control control)
            : base(control, "_FocusLost(Control *sender, Control *newFocusedControl)\n{\n\t\n}")
        {

        }
    }
}
