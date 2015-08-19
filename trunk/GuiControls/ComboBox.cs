using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class ComboBox : Button
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "comboBox";
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

		[Category("Events")]
		public SelectedIndexChangedEvent SelectedIndexChangedEvent
		{
			get;
			set;
		}
		#endregion

		public ComboBox()
		{
			Type = ControlType.ComboBox;

			DefaultSize = Size = new Size(160, 24);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));

			SelectedIndexChangedEvent = new SelectedIndexChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
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
			base.Render(graphics);

			int arrowLeft = AbsoluteLocation.X + Size.Width - 9;
			int arrowTop = AbsoluteLocation.Y + Size.Height / 2 + 1;
			for (int i = 0; i < 4; ++i)
			{
				graphics.FillRectangle(foreBrush, arrowLeft - i, arrowTop - i, 1 + i * 2, 1);
			}
		}

		public override Control Copy()
		{
			ComboBox copy = new ComboBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			ComboBox comboBox = copy as ComboBox;
			string[] itemsCopy = new string[items.Length];
			for (int i = 0; i < items.Length; ++i)
			{
				itemsCopy[i] = items[i];
			}
			comboBox.items = itemsCopy;
		}

		public override string ToString()
		{
			return Name + " - ComboBox";
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
