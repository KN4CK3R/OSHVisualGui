using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.Serialization;
using System.Reflection;

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
		TrackBar,
		HotkeyControl
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
			this.Location = location;
			this.Buttons = buttons;
		}
	}

	public abstract class Control : ISerializable
	{
		internal virtual string DefaultName
		{
			get
			{
				return "form";
			}
		}
		internal ControlType Type;

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		private bool enabled;
		public virtual bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}
		private bool visible;
		public virtual bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
			}
		}
		private Point absoluteLocation;
		internal Point AbsoluteLocation
		{
			get
			{
				return absoluteLocation;
			}
		}
		private Point location;
		protected Point DefaultLocation;
		public virtual Point Location
		{
			get
			{
				return location;
			}
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
			get
			{
				return size;
			}
			set
			{
				size = value.LimitMin(MinimumSize.Width, MinimumSize.Height);
			}
		}
		internal System.Windows.Forms.AnchorStyles anchor;
		[Editor(typeof(System.Windows.Forms.Design.AnchorEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public virtual System.Windows.Forms.AnchorStyles Anchor
		{
			get
			{
				return anchor;
			}
			set
			{
				anchor = value;
			}
		}
		private bool autoSize;
		protected bool DefaultAutoSize;
		public virtual bool AutoSize
		{
			get
			{
				return autoSize;
			}
			set
			{
				autoSize = value;
			}
		}
		private Font font;
		protected Font DefaultFont;
		public virtual Font Font
		{
			get
			{
				return font != null ? font : Parent != null ? Parent.Font : DefaultFont;
			}
			set
			{
				font = value;
			}
		}
		protected Brush foreBrush;
		private Color foreColor;
		protected Color DefaultForeColor;
		public virtual Color ForeColor
		{
			get
			{
				return foreColor;
			}
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
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
				backBrush = new SolidBrush(backColor);
			}
		}
		internal int _zOrder;
		internal virtual int zOrder
		{
			get
			{
				return _zOrder;
			}
			set
			{
				_zOrder = value;
				RealParent.Sort();
			}
		}

		private Control parent;
		internal Control Parent
		{
			get
			{
				return parent;
			}
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
		protected bool isFocusable;
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

		public Control()
		{
			Initialize();

			enabled = true;
			visible = true;

			autoSize = DefaultAutoSize;

			location = DefaultLocation;
		}

		protected Control(SerializationInfo info, StreamingContext context)
		{
			Initialize();

			enabled = info.GetBoolean("enabled");
			visible = info.GetBoolean("visible");

			autoSize = info.GetBoolean("autoSize");
			size = (Size)info.GetValue("size", typeof(Size));

			Anchor = (AnchorStyles)info.GetValue("anchor", typeof(AnchorStyles));

			Font = info.GetValue("font", typeof(Font)) as Font;
			ForeColor = (Color)info.GetValue("foreColor", typeof(Color));
			BackColor = (Color)info.GetValue("backColor", typeof(Color));

			foreach (var it in info)
			{
				if (it.Name.Contains("Event"))
				{
					var controlEvent = GetType().GetProperty(it.Name).GetValue(this, null) as Event;
					if (controlEvent != null)
					{
						controlEvent.Code = it.Value.ToString();
					}
				}
			}
		}

		private void Initialize()
		{
			DefaultAutoSize = false;

			DefaultLocation = new Point(6, 6);

			Anchor = AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;

			MinimumSize = new Size(5, 5);

			isFocusable = true;
			isFocused = false;
			isHighlighted = false;
			isSubControl = false;

			_zOrder = 0;

			DefaultFont = new Font("Arial", 11, GraphicsUnit.Pixel);

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

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("name", name);
			info.AddValue("enabled", enabled);
			info.AddValue("visible", visible);
			info.AddValue("location", location);
			info.AddValue("size", size);
			info.AddValue("anchor", anchor);
			info.AddValue("autoSize", autoSize);
			info.AddValue("font", font);
			info.AddValue("foreColor", foreColor);
			info.AddValue("backColor", backColor);
			foreach (var controlEvent in GetUsedEvents())
			{
				info.AddValue(controlEvent.GetType().Name, controlEvent.Code);
			}
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
			if (parent != null)
			{
				absoluteLocation = parent.absoluteLocation.Add(location);
			}
			else
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

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			ForeColor = foreColor;
			BackColor = backColor;
		}

		public XElement SerializeToXml()
		{
			XElement control = new XElement(DefaultName);
			WriteToXmlElement(control);
			return control;
		}

		protected virtual void WriteToXmlElement(XElement element)
		{
			foreach (var property in GetChangedProperties())
			{
				if (property.Value.UseForXML)
				{
					element.Add(new XAttribute(property.Key, property.Value.ToXMLString()));
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
						Event controlEvent = property.GetValue(this, null) as Event;
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

		#region EventHandling

		protected virtual void OnGotFocus(Control control)
		{
			if (isFocusable)
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
		}

		protected virtual void OnLostFocus(Control control)
		{
			if (isFocusable)
			{
				isFocused = false;
				isClicked = false;

				FocusedControl = null;

				if (LostFocus != null)
				{
					LostFocus(this, null);
				}
			}
		}

		protected virtual void OnClick(Mouse mouse)
		{

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
