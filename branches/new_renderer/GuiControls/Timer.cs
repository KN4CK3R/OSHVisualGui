using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	class Timer : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "timer";
			}
		}
		private long interval;
		private long DefaultInterval;
		public long Interval
		{
			get
			{
				return interval;
			}
			set
			{
				if (value >= 1)
				{
					interval = value;
				}
			}
		}

		[Browsable(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}
		[Browsable(false)]
		public override Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
			}
		}
		[Browsable(false)]
		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}
		[Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		[Category("Events")]
		public TickEvent TickEvent
		{
			get;
			set;
		}

		[Category("Events"), Browsable(false)]
		public new LocationChangedEvent LocationChangedEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new SizeChangedEvent SizeChangedEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new KeyDownEvent KeyDownEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new KeyPressEvent KeyPressEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new KeyUpEvent KeyUpEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new ClickEvent ClickEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseClickEvent MouseClickEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseDownEvent MouseDownEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseUpEvent MouseUpEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseMoveEvent MouseMoveEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseScrollEvent MouseScrollEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseEnterEvent MouseEnterEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseLeaveEvent MouseLeaveEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new MouseCaptureChangedEvent MouseCaptureChangedEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new FocusGotEvent FocusGotEvent
		{
			get;
			set;
		}
		[Category("Events"), Browsable(false)]
		public new FocusLostEvent FocusLostEvent
		{
			get;
			set;
		}
		#endregion

		public Timer()
		{
			Type = ControlType.Timer;

			Enabled = false;
			Size = new Size(16, 16);

			DefaultInterval = interval = 100;

			Mode = DragMode.None;

			TickEvent = new TickEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
		{
			yield return new KeyValuePair<string, object>("name", Name);
			if (Enabled)
				yield return new KeyValuePair<string, object>("enabled", Enabled);
			if (interval != DefaultInterval)
				yield return new KeyValuePair<string, object>("interval", Interval);
		}

		public override IEnumerable<Event> GetUsedEvents()
		{
			if (!TickEvent.IsEmpty)
				yield return TickEvent;
		}

		public override void Render(Graphics graphics)
		{
			graphics.DrawImage(Properties.Resources.control_timer, AbsoluteLocation.X, AbsoluteLocation.Y, 16, 16);
		}

		public override Control Copy()
		{
			Timer copy = new Timer();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			Timer timer = copy as Timer;
			timer.interval = interval;
		}

		public override string ToString()
		{
			return Name + " - Timer";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			if (element.HasAttribute("name"))
				Name = Name.FromXMLString(element.Attribute("name").Value.Trim());
			if (element.HasAttribute("enabled"))
				Enabled = Enabled.FromXMLString(element.Attribute("enabled").Value.Trim());
			if (element.HasAttribute("interval"))
				Interval = Interval.FromXMLString(element.Attribute("interval").Value.Trim());
		}
	}
}
