using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	public class GroupBox : ContainerControl
	{
		#region Properties

		private readonly Label label;
		private readonly Panel panel;

		internal override string DefaultName => "groupBox";
		protected string DefaultText;
		public string Text
		{
			get => label.Text;
			set => label.Text = value ?? string.Empty;
		}
		internal override List<Control> Controls => panel.Controls;

		public override Size Size
		{
			get => base.Size;
			set
			{
				base.Size = value.LimitMin(label.Size.Width + 10, 17);
				panel.Size = base.Size.Add(new Size(-3 * 2, -3 * 2 - 10));
			}
		}
		internal override Point ContainerLocation => base.ContainerLocation.Add(panel.Location);

		internal override Point ContainerAbsoluteLocation => panel.ContainerAbsoluteLocation;

		internal override Size ContainerSize => panel.ContainerSize;

		public override Color ForeColor
		{
			get => base.ForeColor;
			set
			{
				base.ForeColor = value;
				label.ForeColor = value;
			}
		}
		#endregion

		public GroupBox()
		{
			label = new Label
			{
				Location = new Point(5, -1),
				IsSubControl = true
			};
			AddSubControl(label);

			panel = new Panel
			{
				Location = new Point(3, 10),
				IsSubControl = true
			};
			AddSubControl(panel);

			Type = ControlType.GroupBox;

			Size = DefaultSize = new Size(200, 200);

			Text = DefaultText = string.Empty;

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.Empty;
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (Text != DefaultText)
			{
				yield return new KeyValuePair<string, ChangedProperty>("text", new ChangedProperty(Text));
			}
		}

		public override void AddControl(Control control)
		{
			panel.AddControl(control);
		}

		public override void RemoveControl(Control control)
		{
			panel.RemoveControl(control);
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}
			label.Render(graphics);

			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + 5, 3, 1);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + label.Size.Width + 5, AbsoluteLocation.Y + 5, Size.Width - label.Size.Width - 6, 1);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X, AbsoluteLocation.Y + 6, 1, Size.Height - 7);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + Size.Width - 1, AbsoluteLocation.Y + 6, 1, Size.Height - 7);
			graphics.FillRectangle(foreBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + Size.Height - 1, Size.Width - 2, 1);

			panel.Render(graphics);

			if (IsHighlighted)
			{
				using (var pen = new Pen(Color.Orange, 1))
				{
					graphics.DrawRectangle(pen, AbsoluteLocation.X - 3, AbsoluteLocation.Y - 2, Size.Width + 5, Size.Height + 4);
				}

				IsHighlighted = false;
			}
		}

		public override Control Copy()
		{
			var copy = new GroupBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var groupBox = copy as GroupBox;
			groupBox.Text = Text;

			foreach (var control in panel.Controls)
			{
				groupBox.AddControl(control.Copy());
			}
		}

		public override string ToString()
		{
			return Name + " - GroupBox";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("text"))
				Text = Text.FromXMLString(element.Attribute("text").Value.Trim());
		}
	}
}
