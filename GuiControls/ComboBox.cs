using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class ComboBox : Button
	{
		#region Properties

		internal override string DefaultName => "comboBox";

		public string[] Items { get; set; }

		[Category("Events")]
		public SelectedIndexChangedEvent SelectedIndexChangedEvent { get; set; }

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

			var arrowLeft = AbsoluteLocation.X + Size.Width - 9;
			var arrowTop = AbsoluteLocation.Y + Size.Height / 2 + 1;
			for (var i = 0; i < 4; ++i)
			{
				graphics.FillRectangle(foreBrush, arrowLeft - i, arrowTop - i, 1 + i * 2, 1);
			}
		}

		public override Control Copy()
		{
			var copy = new ComboBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var comboBox = copy as ComboBox;
			var itemsCopy = new string[Items.Length];
			for (var i = 0; i < Items.Length; ++i)
			{
				itemsCopy[i] = Items[i];
			}
			comboBox.Items = itemsCopy;
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
				foreach (var item in Items)
				{
					element.Add(new XElement("item", item));
				}
			}
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			var itemList = new List<string>();
			foreach (var itemElement in element.Nodes().OfType<XElement>())
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
