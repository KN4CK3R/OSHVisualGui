using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
	public class HotkeyControl : ScalableControl
	{
		private readonly TextBox textBox;

		#region Properties
		internal override string DefaultName => "hotkeyControl";

		public override Size Size
		{
			get => base.Size;
			set
			{
				base.Size = value;
				textBox.Size = Size;
			}
		}
		public override Font Font
		{
			get => base.Font;
			set
			{
				base.Font = value;
				textBox.Font = Font;
			}
		}
		public override Color ForeColor
		{
			get => base.ForeColor;
			set
			{
				base.ForeColor = value;
				textBox.ForeColor = ForeColor;
			}
		}
		public override Color BackColor
		{
			get => base.BackColor;
			set
			{
				base.BackColor = value;
				textBox.BackColor = BackColor;
			}
		}

		private Keys hotkey;
		public Keys Hotkey
		{
			get => hotkey;
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

			textBox = new TextBox
			{
				Location = new Point(0, 0),
				Parent = this
			};

			Size = DefaultSize = new Size(100, 24);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF242321));

			Hotkey = Keys.None;

			HotkeyChangedEvent = new HotkeyChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if ((hotkey & Keys.Modifiers) != Keys.None)
			{
				yield return new KeyValuePair<string, ChangedProperty>("modifier", new ChangedProperty(hotkey & Keys.Modifiers));
			}
			if ((hotkey & Keys.KeyCode) != Keys.None)
			{
				yield return new KeyValuePair<string, ChangedProperty>("hotkey", new ChangedProperty(hotkey & Keys.KeyCode));
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
			var copy = new HotkeyControl();
			CopyTo(copy);
			return copy;
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
