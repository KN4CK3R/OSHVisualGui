using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace OSHVisualGui.GuiControls
{
	[Editor(typeof(EventEditor), typeof(UITypeEditor))]
	[ImmutableObject(true)]
	[Serializable]
	public class Event
	{
		[Browsable(false)]
		public Control Control { get; }

		private string code;
		[Browsable(false)]
		public string Code
		{
			get => code;
			set => code = value;
		}
		protected string stub;
		[Browsable(false)]
		public string Stub => "void " + Control.Name + stub + "(" + string.Join(", ", Parameter) + ")\n{\n\t\n}";

		[Browsable(false)]
		public string Signature => Code.Substring(5, Code.IndexOf('(') - 5);

		[Browsable(false)]
		public bool IsEmpty => string.IsNullOrEmpty(code);

		[Browsable(false)]
		public string[] Parameter { get; }

		protected Event(Control control, string stub, string[] parameter)
		{
			if (stub == null)
			{
				throw new ArgumentNullException(nameof(stub));
			}

			code = string.Empty;
			Control = control;
			this.stub = stub;
			Parameter = parameter;
		}

		public override string ToString()
		{
			return !string.IsNullOrEmpty(code) ? "void " + Control.Name + stub + "(" + string.Join(", ", Parameter) + ")" : string.Empty;
		}
	}

	[Serializable]
	public class ConstructorEvent : Event
	{
		public ConstructorEvent(Control control)
			: base(control, "", new[] { "" })
		{

		}
	}

	[Serializable]
	public class ClickEvent : Event
	{
		public ClickEvent(Control control)
			: base(control, "_Clicked", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class LocationChangedEvent : Event
	{
		public LocationChangedEvent(Control control)
			: base(control, "_LocationChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class SizeChangedEvent : Event
	{
		public SizeChangedEvent(Control control)
			: base(control, "_SizeChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class KeyDownEvent : Event
	{
		public KeyDownEvent(Control control)
			: base(control, "_KeyDown", new[] { "Control *sender", "KeyEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class KeyPressEvent : Event
	{
		public KeyPressEvent(Control control)
			: base(control, "_KeyPress", new[] { "Control *sender", "KeyPressEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class KeyUpEvent : Event
	{
		public KeyUpEvent(Control control)
			: base(control, "_KeyUp", new[] { "Control *sender", "KeyEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseClickEvent : Event
	{
		public MouseClickEvent(Control control)
			: base(control, "_MouseClick", new[] { "Control *sender", "MouseEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseDownEvent : Event
	{
		public MouseDownEvent(Control control)
			: base(control, "_MouseDown", new[] { "Control *sender", "MouseEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseUpEvent : Event
	{
		public MouseUpEvent(Control control)
			: base(control, "_MouseUp", new[] { "Control *sender", "MouseEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseMoveEvent : Event
	{
		public MouseMoveEvent(Control control)
			: base(control, "_MouseMove", new[] { "Control *sender", "MouseEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseScrollEvent : Event
	{
		public MouseScrollEvent(Control control)
			: base(control, "_MouseScroll", new[] { "Control *sender", "MouseEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class MouseEnterEvent : Event
	{
		public MouseEnterEvent(Control control)
			: base(control, "_MouseEnter", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class MouseLeaveEvent : Event
	{
		public MouseLeaveEvent(Control control)
			: base(control, "_MouseLeave", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class MouseCaptureChangedEvent : Event
	{
		public MouseCaptureChangedEvent(Control control)
			: base(control, "_MouseCaptureChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class FocusGotEvent : Event
	{
		public FocusGotEvent(Control control)
			: base(control, "_FocusGot", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class FocusLostEvent : Event
	{
		public FocusLostEvent(Control control)
			: base(control, "_FocusLost", new[] { "Control *sender", "Control *newFocusedControl" })
		{

		}
	}

	[Serializable]
	public class FormClosingEvent : Event
	{
		public FormClosingEvent(Control control)
			: base(control, "_FormClosing", new[] { "Control *sender", "bool &canClose" })
		{

		}
	}

	[Serializable]
	public class SelectedIndexChangedEvent : Event
	{
		public SelectedIndexChangedEvent(Control control)
			: base(control, "_SelectedIndexChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class ColorChangedEvent : Event
	{
		public ColorChangedEvent(Control control)
			: base(control, "_ColorChanged", new[] { "Control *sender", "const Drawing::Color &color" })
		{

		}
	}

	[Serializable]
	public class CheckedChangedEvent : Event
	{
		public CheckedChangedEvent(Control control)
			: base(control, "_CheckedChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class TextChangedEvent : Event
	{
		public TextChangedEvent(Control control)
			: base(control, "_TextChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class ValueChangedEvent : Event
	{
		public ValueChangedEvent(Control control)
			: base(control, "_ValueChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class TickEvent : Event
	{
		public TickEvent(Control control)
			: base(control, "_TickChanged", new[] { "Control *sender" })
		{

		}
	}

	[Serializable]
	public class ScrollEvent : Event
	{
		public ScrollEvent(Control control)
			: base(control, "_Scroll", new[] { "Control *sender", "ScrollEventArgs &e" })
		{

		}
	}

	[Serializable]
	public class HotkeyChangedEvent : Event
	{
		public HotkeyChangedEvent(Control control)
			: base(control, "_HotkeyChanged", new[] { "Control *sender" })
		{

		}
	}
}
