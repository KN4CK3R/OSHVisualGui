﻿using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	class TrackBar : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "trackBar";
			}
		}
		private int minimum;
		public int Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				if (value < maximum)
				{
					minimum = value;
				}
			}
		}
		private int maximum;
		public int Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				if (value > minimum)
				{
					maximum = value;
				}
			}
		}
		private int value;
		public int Value
		{
			get
			{
				return value;
			}
			set
			{
				if (value >= minimum && value <= maximum)
				{
					this.value = value;
				}
			}
		}
		private int tickFrequency;
		public int TickFrequency
		{
			get
			{
				return tickFrequency;
			}
			set
			{
				if (value >= 1)
				{
					this.tickFrequency = value;
				}
			}
		}

		[Category("Events")]
		public ValueChangedEvent ValueChangedEvent
		{
			get;
			set;
		}
		#endregion

		public TrackBar()
		{
			Type = ControlType.TrackBar;

			minimum = 1;
			maximum = 10;
			tickFrequency = 1;

			Size = DefaultSize = new Size(110, 18);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.Empty;

			ValueChangedEvent = new ValueChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (Minimum != 1)
			{
				yield return new KeyValuePair<string, ChangedProperty>("minimum", new ChangedProperty(Minimum));
			}
			if (Maximum != 10)
			{
				yield return new KeyValuePair<string, ChangedProperty>("maximum", new ChangedProperty(Maximum));
			}
			if (TickFrequency != 1)
			{
				yield return new KeyValuePair<string, ChangedProperty>("tickfrequency", new ChangedProperty(TickFrequency));
			}
			if (Value != 0)
			{
				yield return new KeyValuePair<string, ChangedProperty>("value", new ChangedProperty(Value));
			}
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}

			int tickCount = 1 + (maximum - minimum) / tickFrequency;
			float pixelsPerTick = (Size.Width - 8) / ((maximum - minimum) / (float)tickFrequency);
			for (int i = 0; i < tickCount; ++i)
			{
				int x = (int)(AbsoluteLocation.X + 4 + i * pixelsPerTick);
				int y = AbsoluteLocation.Y + 7;
				graphics.FillRectangle(foreBrush, x, y, 1, 5);
			}

			int tick = value / tickFrequency;
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + tick * pixelsPerTick, AbsoluteLocation.Y + 1, 8, 16);
		}

		public override Control Copy()
		{
			TrackBar copy = new TrackBar();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			TrackBar trackBar = copy as TrackBar;
			trackBar.minimum = minimum;
			trackBar.maximum = maximum;
			trackBar.tickFrequency = tickFrequency;
		}

		public override string ToString()
		{
			return Name + " - TrackBar";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("tickFrequency"))
				TickFrequency = TickFrequency.FromXMLString(element.Attribute("tickFrequency").Value.Trim());
			if (element.HasAttribute("minimum"))
				Minimum = Minimum.FromXMLString(element.Attribute("minimum").Value.Trim());
			if (element.HasAttribute("maximum"))
				Maximum = Maximum.FromXMLString(element.Attribute("maximum").Value.Trim());
		}
	}
}
