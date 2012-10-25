using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [Editor(typeof(EventEditor), typeof(UITypeEditor))]
    [ImmutableObject(true)]
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
        public string Stub { get { return "void " + Control.Name + stub + "(" + string.Join(", ", Parameter) + ")\n{\n\t\n}"; } }
        [Browsable(false)]
        public string Signature { get { return Code.Substring(5, Code.IndexOf('(') - 5); } }
        [Browsable(false)]
        public bool IsEmpty { get { return string.IsNullOrEmpty(code); } }
        private string[] parameter;
        [Browsable(false)]
        public string[] Parameter { get { return parameter; } }

        protected Event(Control control, string stub, string[] parameter)
        {
            if (string.IsNullOrEmpty(stub))
            {
                throw new ArgumentNullException("stub");
            }

            code = string.Empty;
            this.control = control;
            this.stub = stub;
            this.parameter = parameter;
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(code) ? "void " + Control.Name + stub + "(" + string.Join(", ", Parameter) + ")" : string.Empty;
        }
    }

    public class ClickEvent : Event
    {
        public ClickEvent(Control control)
            : base(control, "_Clicked", new string[] { "Control *sender" })
        {

        }
    }

    public class LocationChangedEvent : Event
    {
        public LocationChangedEvent(Control control)
            : base(control, "_LocationChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class SizeChangedEvent : Event
    {
        public SizeChangedEvent(Control control)
            : base(control, "_SizeChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class KeyDownEvent : Event
    {
        public KeyDownEvent(Control control)
            : base(control, "_KeyDown", new string[] { "Control *sender", "KeyEventArgs &e" })
        {

        }
    }

    public class KeyPressEvent : Event
    {
        public KeyPressEvent(Control control)
            : base(control, "_KeyPress", new string[] { "Control *sender", "KeyPressEventArgs &e" })
        {

        }
    }

    public class KeyUpEvent : Event
    {
        public KeyUpEvent(Control control)
            : base(control, "_KeyUp", new string[] { "Control *sender", "KeyEventArgs &e" })
        {

        }
    }

    public class MouseClickEvent : Event
    {
        public MouseClickEvent(Control control)
            : base(control, "_MouseClick", new string[] { "Control *sender", "MouseEventArgs &e" })
        {

        }
    }

    public class MouseDownEvent : Event
    {
        public MouseDownEvent(Control control)
            : base(control, "_MouseDown", new string[] { "Control *sender", "MouseEventArgs &e" })
        {

        }
    }

    public class MouseUpEvent : Event
    {
        public MouseUpEvent(Control control)
            : base(control, "_MouseUp", new string[] { "Control *sender", "MouseEventArgs &e" })
        {

        }
    }

    public class MouseMoveEvent : Event
    {
        public MouseMoveEvent(Control control)
            : base(control, "_MouseMove", new string[] { "Control *sender", "MouseEventArgs &e" })
        {

        }
    }

    public class MouseScrollEvent : Event
    {
        public MouseScrollEvent(Control control)
            : base(control, "_MouseScroll", new string[] { "Control *sender", "MouseEventArgs &e" })
        {

        }
    }

    public class MouseEnterEvent : Event
    {
        public MouseEnterEvent(Control control)
            : base(control, "_MouseEnter", new string[] { "Control *sender" })
        {

        }
    }

    public class MouseLeaveEvent : Event
    {
        public MouseLeaveEvent(Control control)
            : base(control, "_MouseLeave", new string[] { "Control *sender" })
        {

        }
    }

    public class MouseCaptureChangedEvent : Event
    {
        public MouseCaptureChangedEvent(Control control)
            : base(control, "_MouseCaptureChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class FocusGotEvent : Event
    {
        public FocusGotEvent(Control control)
            : base(control, "_FocusGot", new string[] { "Control *sender" })
        {

        }
    }

    public class FocusLostEvent : Event
    {
        public FocusLostEvent(Control control)
            : base(control, "_FocusLost", new string[] { "Control *sender", "Control *newFocusedControl" })
        {

        }
    }

    public class FormClosingEvent : Event
    {
        public FormClosingEvent(Control control)
            : base(control, "_FormClosing", new string[] { "Control *sender", "bool &canClose" })
        {

        }
    }

    public class SelectedIndexChangedEvent : Event
    {
        public SelectedIndexChangedEvent(Control control)
            : base(control, "_SelectedIndexChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class ColorChangedEvent : Event
    {
        public ColorChangedEvent(Control control)
            : base(control, "_ColorChanged", new string[] { "Control *sender", "Drawing::Color &color" })
        {

        }
    }

    public class CheckedChangedEvent : Event
    {
        public CheckedChangedEvent(Control control)
            : base(control, "_CheckedChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class TextChangedEvent : Event
    {
        public TextChangedEvent(Control control)
            : base(control, "_TextChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class ValueChangedEvent : Event
    {
        public ValueChangedEvent(Control control)
            : base(control, "_ValueChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class TickEvent : Event
    {
        public TickEvent(Control control)
            : base(control, "_TickChanged", new string[] { "Control *sender" })
        {

        }
    }

    public class ScrollEvent : Event
    {
        public ScrollEvent(Control control)
            : base(control, "_Scroll", new string[] { "Control *sender", "ScrollEventArgs &e" })
        {

        }
    }
}
