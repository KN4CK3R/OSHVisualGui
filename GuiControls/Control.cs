using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
	public enum ControlType
	{
		Button = 1,
		CheckBox,
		ColorBar,
		ColorPicker,
		ComboBox,
		Form,
		GroupBox,
		HotkeyControl,
		Label,
		LinkLabel,
		ListBox,
		Panel,
		PictureBox,
		ProgressBar,
		RadioButton,
		ScrollBar,
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

		public Point Location
		{
			get;
			set;
		}
		public MouseStates Buttons
		{
			get;
			set;
		}

		public Mouse(Point location, MouseStates buttons)
		{
			Location = location;
			Buttons = buttons;
		}
	}

	public abstract class Control
	{
		internal virtual string DefaultName => "form";
		internal ControlType Type;

		public string Name { get; set; }

		private bool enabled;
		public virtual bool Enabled
		{
			get => enabled;
			set => enabled = value;
		}
		internal bool DesignerHidden { get; set; }
		private bool visible;
		public virtual bool Visible
		{
			get => visible;
			set => visible = value;
		}
		private Point absoluteLocation;
		internal Point AbsoluteLocation => absoluteLocation;
		private Point location;
		protected Point DefaultLocation;
		public virtual Point Location
		{
			get => location;
			set
			{
				location = value;
				CalculateAbsoluteLocation();
			}
		}
		private Size size;
		protected Size DefaultSize;
		internal Size MinimumSize;
		public virtual Size Size
		{
			get => size;
			set => size = value.LimitMin(MinimumSize.Width, MinimumSize.Height);
		}

		private AnchorStyles anchor;
		[Editor(typeof(System.Windows.Forms.Design.AnchorEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public virtual AnchorStyles Anchor
		{
			get => anchor;
			set => anchor = value;
		}
		private bool autoSize;
		protected bool DefaultAutoSize;
		public virtual bool AutoSize
		{
			get => autoSize;
			set => autoSize = value;
		}
		private Font font;
		protected Font DefaultFont;
		public virtual Font Font
		{
			get => font ?? (Parent != null ? Parent.Font : DefaultFont);
			set => font = value;
		}
		protected Brush foreBrush;
		private Color foreColor;
		protected Color DefaultForeColor;
		public virtual Color ForeColor
		{
			get => foreColor;
			set
			{
				foreColor = value;
				foreBrush = new SolidBrush(foreColor);
			}
		}
		protected Brush backBrush;
		private Color backColor;
		protected Color DefaultBackColor;
		public virtual Color BackColor
		{
			get => backColor;
			set
			{
				backColor = value;
				backBrush = new SolidBrush(backColor);
			}
		}
		internal int _zOrder;
		internal virtual int zOrder
		{
			get => _zOrder;
			set
			{
				_zOrder = value;
				RealParent.Sort();
			}
		}

		private Control parent;
		internal Control Parent
		{
			get => parent;
			set
			{
				parent = value;
				CalculateAbsoluteLocation();
			}
		}
		internal ContainerControl RealParent
		{
			get
			{
				if (Parent == null)
				{
					return this as ContainerControl;
				}

				var parent = Parent;
				while (parent.IsSubControl && parent != this)
				{
					parent = parent.Parent;
				}
				return parent as ContainerControl;
			}
		}

		public bool IsHighlighted;

		protected bool hasCaptured;
		protected bool isInside;
		protected bool isFocusable;
		protected bool isFocused;
		protected bool isClicked;
		public bool IsSubControl;

		public delegate void MouseEventHandler(Control sender, Mouse e);
		public event MouseEventHandler MouseDown;
		public event MouseEventHandler MouseUp;
		public event MouseEventHandler MouseMove;

		public EventHandler MouseEnter;
		public EventHandler MouseLeave;
		public EventHandler GotFocus;
		public EventHandler LostFocus;

		[Category("Events")]
		public LocationChangedEvent LocationChangedEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public SizeChangedEvent SizeChangedEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public KeyDownEvent KeyDownEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public KeyPressEvent KeyPressEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public KeyUpEvent KeyUpEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public ClickEvent ClickEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseClickEvent MouseClickEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseDownEvent MouseDownEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseUpEvent MouseUpEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseMoveEvent MouseMoveEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseScrollEvent MouseScrollEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseEnterEvent MouseEnterEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseLeaveEvent MouseLeaveEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public MouseCaptureChangedEvent MouseCaptureChangedEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public FocusGotEvent FocusGotEvent
		{
			get;
			set;
		}
		[Category("Events")]
		public FocusLostEvent FocusLostEvent
		{
			get;
			set;
		}

		public static Control MouseOverControl;
		public static Control FocusedControl;
		public static Control MouseCaptureControl;

		protected Control()
		{
			enabled = true;
			visible = true;
			DesignerHidden = false;

			autoSize = DefaultAutoSize = false;

			location = DefaultLocation = new Point(6, 6);
			MinimumSize = new Size(5, 5);

			anchor = AnchorStyles.Top | AnchorStyles.Left;

			isFocusable = true;
			isFocused = false;
			IsHighlighted = false;
			IsSubControl = false;

			_zOrder = 0;

			DefaultFont = new Font("Arial", 11, GraphicsUnit.Pixel);

			DefaultForeColor = Color.Empty;
			DefaultBackColor = Color.Empty;

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

		public virtual IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			yield return new KeyValuePair<string, ChangedProperty>("name", new ChangedProperty(Name));
			if (!Enabled)
				yield return new KeyValuePair<string, ChangedProperty>("enabled", new ChangedProperty(Enabled));
			if (!Visible)
				yield return new KeyValuePair<string, ChangedProperty>("visible", new ChangedProperty(Visible));
			if (Location != DefaultLocation)
				yield return new KeyValuePair<string, ChangedProperty>("location", new ChangedProperty(Location));
			if (Size != DefaultSize && AutoSize == false)
				yield return new KeyValuePair<string, ChangedProperty>("size", new ChangedProperty(Size));
			if (Anchor != (AnchorStyles.Top | AnchorStyles.Left))
				yield return new KeyValuePair<string, ChangedProperty>("anchor", new ChangedProperty(Anchor));
			if (AutoSize != DefaultAutoSize)
				yield return new KeyValuePair<string, ChangedProperty>("autosize", new ChangedProperty(AutoSize));
			if (!this.Font.Equals(DefaultFont))
				yield return new KeyValuePair<string, ChangedProperty>("font", new ChangedProperty(Font));
			if (ForeColor != DefaultForeColor)
				yield return new KeyValuePair<string, ChangedProperty>("forecolor", new ChangedProperty(ForeColor));
			if (BackColor != DefaultBackColor)
				yield return new KeyValuePair<string, ChangedProperty>("backcolor", new ChangedProperty(BackColor));
		}

		public virtual IEnumerable<Event> GetUsedEvents()
		{
			foreach (var property in GetType().GetProperties())
			{
				if (property.Name.Contains("Event"))
				{
					var controlEvent = property.GetValue(this, null) as Event;
					if (!controlEvent.IsEmpty)
					{
						yield return controlEvent;
					}
				}
			}
		}

		public virtual bool Intersect(Point location)
		{
			return location.X >= absoluteLocation.X && location.X <= absoluteLocation.X + size.Width && location.Y >= absoluteLocation.Y && location.Y <= absoluteLocation.Y + size.Height;
		}

		public virtual void CalculateAbsoluteLocation()
		{
			absoluteLocation = parent?.absoluteLocation.Add(location) ?? location;
		}

		public abstract void Render(Graphics graphics);

		public abstract Control Copy();
		protected virtual void CopyTo(Control copy)
		{
			copy.Name = Name;
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
			return Name;
		}

		public XElement SerializeToXml()
		{
			var control = new XElement(DefaultName);
			WriteToXmlElement(control);
			return control;
		}

		protected virtual void WriteToXmlElement(XElement element)
		{
			foreach (var property in GetChangedProperties())
			{
				if (property.Value.UseForXml)
				{
					element.Add(new XAttribute(property.Key, property.Value.Value.ToXMLString()));
				}
			}

			foreach (var controlEvent in GetUsedEvents())
			{
				element.Add(new XAttribute(controlEvent.GetType().Name, controlEvent.Code.ToBase64String()));
			}
		}

		public virtual void ReadPropertiesFromXml(XElement element)
		{
			if (element.HasAttribute("name"))
				Name = Name.FromXMLString(element.Attribute("name").Value.Trim());
			if (element.HasAttribute("enabled"))
				Enabled = Enabled.FromXMLString(element.Attribute("enabled").Value.Trim());
			if (element.HasAttribute("visible"))
				Visible = Visible.FromXMLString(element.Attribute("visible").Value.Trim());
			if (element.HasAttribute("location"))
				Location = Location.FromXMLString(element.Attribute("location").Value.Trim());
			if (element.HasAttribute("size"))
				Size = Size.FromXMLString(element.Attribute("size").Value.Trim());
			if (element.HasAttribute("anchor"))
				Anchor = Anchor.FromXMLString(element.Attribute("anchor").Value.Trim());
			if (element.HasAttribute("autosize"))
				AutoSize = AutoSize.FromXMLString(element.Attribute("autosize").Value.Trim());
			if (element.HasAttribute("font"))
				Font = Font.FromXMLString(element.Attribute("font").Value.Trim());
			if (element.HasAttribute("forecolor"))
				ForeColor = ForeColor.FromXMLString(element.Attribute("forecolor").Value.Trim());
			if (element.HasAttribute("backcolor"))
				BackColor = BackColor.FromXMLString(element.Attribute("backcolor").Value.Trim());

			foreach (var property in GetType().GetProperties())
			{
				if (property.Name.Contains("Event"))
				{
					if (element.Attribute(property.Name) != null)
					{
						var controlEvent = property.GetValue(this, null) as Event;
						controlEvent.Code = element.Attribute(property.Name).Value.Trim().FromBase64String();
					}
				}
			}
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

		public static Size MeasureText(string text, Font font)
		{
			return TextRenderer.MeasureText(MainForm.Renderer, text, font, new Size(1000, 1000), TextFormatFlags.NoPadding);
		}

		#region EventHandling

		protected virtual void OnGotFocus(Control control)
		{
			if (isFocusable)
			{
				if (FocusedControl != this)
				{
					FocusedControl?.OnLostFocus(this);
					FocusedControl = this;
					isFocused = true;

					GotFocus?.Invoke(this, null);
				}
			}
		}

		protected virtual void OnLostFocus(Control control)
		{
			if (isFocusable)
			{
				isFocused = false;
				isClicked = false;

				FocusedControl = null;

				LostFocus?.Invoke(this, null);
			}
		}

		protected virtual void OnClick(Mouse mouse)
		{

		}

		protected virtual void OnMouseDown(Mouse mouse)
		{
			isClicked = true;

			OnGotMouseCapture();

			MouseDown?.Invoke(this, mouse);
		}

		protected virtual void OnMouseUp(Mouse mouse)
		{
			isClicked = false;

			OnLostMouseCapture();

			MouseUp?.Invoke(this, mouse);
		}

		protected virtual void OnMouseMove(Mouse mouse)
		{
			MouseMove?.Invoke(this, mouse);
		}

		protected virtual void OnGotMouseCapture()
		{
			MouseCaptureControl?.OnLostMouseCapture();
			MouseCaptureControl = this;

			hasCaptured = true;
			//isClicked = false;
		}

		protected virtual void OnLostMouseCapture()
		{
			hasCaptured = false;

			MouseCaptureControl = null;
		}

		protected virtual void OnMouseEnter()
		{
			isInside = true;

			MouseOverControl?.OnMouseLeave();

			MouseEnter?.Invoke(this, null);
		}

		protected virtual void OnMouseLeave()
		{
			isInside = false;

			MouseOverControl = null;

			MouseLeave?.Invoke(this, null);
		}

		public bool ProcessMouseMessage(Mouse mouse)
		{
			switch (mouse.Buttons)
			{
				case Mouse.MouseStates.LeftDown:
					if (Intersect(mouse.Location))
					{
						OnMouseDown(mouse);

						if (!isFocused && !IsSubControl)
						{
							OnGotFocus(this);
						}

						return true;
					}
					break;
				case Mouse.MouseStates.LeftUp:
					if (hasCaptured || Intersect(mouse.Location))
					{
						if (isClicked)
						{
							OnClick(mouse);
						}

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
