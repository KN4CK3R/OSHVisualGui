using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class TabPage : Panel
	{
		#region Properties

		internal override string DefaultName => "tabPage";

		internal TabControl.TabControlButton Button;

		private readonly Panel containerPanel = new Panel
		{
			Location = new Point(2, 2),
			IsSubControl = true
		};

		protected string text;
		protected string DefaultText;
		public string Text
		{
			get => text;
			set
			{
				text = value ?? string.Empty;
				Button?.CalculateSize();
			}
		}
		internal override List<Control> Controls => containerPanel.Controls;

		[Browsable(false)]
		public override bool AutoSize
		{
			get => base.AutoSize;
			set => base.AutoSize = value;
		}
		[Browsable(false)]
		public override bool Enabled
		{
			get => base.Enabled;
			set => base.Enabled = value;
		}
		[Browsable(false)]
		public override bool Visible
		{
			get => base.Visible;
			set => base.Visible = value;
		}
		[Browsable(false)]
		public override Point Location
		{
			get => base.Location;
			set => base.Location = value;
		}
		[Browsable(false)]
		public override Font Font
		{
			get => base.Font;
			set => base.Font = value;
		}
		[Browsable(false)]
		public override Size Size
		{
			get => base.Size;
			set
			{
				base.Size = value;

				containerPanel.Size = value.Substract(new Size(4, 4));
			}
		}

		#endregion

		public TabPage()
		{
			Type = ControlType.TabPage;

			AddSubControl(containerPanel);

			Mode = DragMode.None;

			Text = DefaultText = string.Empty;

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF474747));

			//isSubControl = true;
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			yield return new KeyValuePair<string, ChangedProperty>("name", new ChangedProperty(Name));
			if (ForeColor != DefaultForeColor)
				yield return new KeyValuePair<string, ChangedProperty>("forecolor", new ChangedProperty(ForeColor));
			if (BackColor != DefaultBackColor)
				yield return new KeyValuePair<string, ChangedProperty>("backcolor", new ChangedProperty(BackColor));
			if (Text != DefaultText)
				yield return new KeyValuePair<string, ChangedProperty>("text", new ChangedProperty(Text));
		}

		public override void AddControl(Control control)
		{
			containerPanel.AddControl(control);
		}

		public override void RemoveControl(Control control)
		{
			containerPanel.RemoveControl(control);
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				Brush brush = new SolidBrush(BackColor.Add(Color.FromArgb(0, 32, 32, 32)));
				graphics.FillRectangle(brush, AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
				var rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height - 2);
				brush = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
				graphics.FillRectangle(brush, rect);
			}

			containerPanel.Render(graphics);
		}

		public override Control Copy()
		{
			var copy = new TabPage();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var tabPage = copy as TabPage;
			tabPage.Text = text;
		}

		public override string ToString()
		{
			return Name + " - TabPage";
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.HasAttribute("text"))
				Text = Text.FromXMLString(element.Attribute("text").Value.Trim());
		}
	}
}
