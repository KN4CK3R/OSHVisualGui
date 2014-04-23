using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	class HotkeyControl : ScalableControl
	{
		private TextBox textBox;

		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "hotkeyControl";
			}
		}

		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
				textBox.Size = Size;
			}
		}
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				textBox.Font = Font;
			}
		}
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				textBox.ForeColor = ForeColor;
			}
		}
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				textBox.BackColor = BackColor;
			}
		}

		private Keys hotkey;
		public Keys Hotkey
		{
			get
			{
				return hotkey;
			}
			set
			{
				hotkey = value;
				HotkeyToText();
			}
		}

		[Category("Events")]
		public HotkeyChangedEvent HotkeyChangedEvent
		{
			get;
			set;
		}
		#endregion

		public HotkeyControl()
		{
			Type = ControlType.HotkeyControl;

			textBox = new TextBox();
			textBox.Location = new Point(0, 0);
			textBox.Parent = this;

			DefaultSize = Size = new Size(100, 24);

			DefaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF242321));
			DefaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

			Hotkey = Keys.None;

			HotkeyChangedEvent = new HotkeyChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				if (pair.Key != "SetText")
				{
					yield return pair;
				}
			}
			if ((hotkey & Keys.Modifiers) != Keys.None)
			{
				yield return new KeyValuePair<string, object>("SetModifier", hotkey & Keys.Modifiers);
			}
			if ((hotkey & Keys.KeyCode) != Keys.None)
			{
				yield return new KeyValuePair<string, object>("SetHotkey", hotkey & Keys.KeyCode);
			}
		}

		public override void CalculateAbsoluteLocation()
		{
			base.CalculateAbsoluteLocation();

			textBox.CalculateAbsoluteLocation();
		}

		public override void Render(Graphics graphics)
		{
			textBox.Render(graphics);
		}

		public override Control Copy()
		{
			HotkeyControl copy = new HotkeyControl();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);
		}

		private string ModifierToText()
		{
			return (hotkey & Keys.Modifiers).ToString().Replace(", ", " + ");
		}

		private void HotkeyToText()
		{
			var modifier = hotkey & Keys.Modifiers;
			var key = hotkey & Keys.KeyCode;
			if (modifier == Keys.None && key == Keys.None)
			{
				textBox.Text = "None";
			}
			else if (modifier == Keys.None)
			{
				textBox.Text = key.ToString();
			}
			else if (hotkey != Keys.None)
			{
				textBox.Text = ModifierToText() + " + " + key.ToString();
			}
			else
			{
				textBox.Text = ModifierToText();
			}
		}

		public override string ToString()
		{
			return Name + " - HotkeyControl";
		}
	}
}
