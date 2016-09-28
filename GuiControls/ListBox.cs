using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	class ListBox : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "listBox";
			}
		}
		private string[] items;
		public string[] Items
		{
			get
			{
				return items;
			}
			set
			{
				items = value;
			}
		}
		private bool autoScrollEnabled;
		public bool AutoScrollEnabled
		{
			get
			{
				return autoScrollEnabled;
			}
			set
			{
				autoScrollEnabled = value;
			}
		}

		[Category("Events")]
		public SelectedIndexChangedEvent SelectedIndexChangedEvent
		{
			get;
			set;
		}
		#endregion

		public ListBox()
		{
			Type = ControlType.ListBox;

			Size = DefaultSize = new Size(120, 95);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF171614));

			SelectedIndexChangedEvent = new SelectedIndexChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (autoScrollEnabled)
			{
				yield return new KeyValuePair<string, ChangedProperty>("autoscrollenabled", new ChangedProperty(autoScrollEnabled));
			}
			if (Items != null && Items.Length > 0)
			{
				foreach (var item in Items)
				{
					yield return new KeyValuePair<string, ChangedProperty>("item", new ChangedProperty(item, true, false));
				}
			}
		}

		public override void Render(Graphics graphics)
		{
			graphics.FillRectangle(backBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height - 2);
			Brush tempBrush = new SolidBrush(BackColor.Add(Color.FromArgb(0, 54, 53, 52)));
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y, Size.Width - 2, 1);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 1, 1, Size.Height - 2);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 1, 1, Size.Height - 2);
			graphics.FillRectangle(tempBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 1, Size.Width - 2, 1);

			if (Items != null && Items.Length > 0)
			{
				int y = 5;
				for (int i = 0; i < Items.Length; ++i)
				{
					Size stringSize = MeasureText(Items[i], Font);
					if (y + stringSize.Height >= Size.Height)
					{
						break;
					}
					graphics.DrawString(Items[i], Font, foreBrush, AbsoluteLocation.X + 5, AbsoluteLocation.Y + y);
					y += stringSize.Height;
				}
			}
			else
			{
				graphics.DrawString(Name, Font, foreBrush, AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5);
			}
		}

		public override Control Copy()
		{
			ListBox copy = new ListBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			ListBox listBox = copy as ListBox;
			listBox.autoScrollEnabled = autoScrollEnabled;
			if (items != null)
			{
				listBox.items = (string[])items.Clone();
			}
		}

		public override string ToString()
		{
			return Name + " - ListBox";
		}

		protected override void WriteToXmlElement(XElement element)
		{
			base.WriteToXmlElement(element);

			if (Items != null)
			{
				foreach (string item in Items)
				{
					element.Add(new XElement("item", item));
				}
			}
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("autoScrollEnabled"))
				AutoScrollEnabled = AutoScrollEnabled.FromXMLString(element.Attribute("autoScrollEnabled").Value.Trim());

			List<string> itemList = new List<string>();
			foreach (XElement itemElement in element.Nodes())
			{
				itemList.Add(itemElement.Value);
			}
			if (itemList.Count > 0)
			{
				Items = itemList.ToArray();
			}
		}
	}
}
