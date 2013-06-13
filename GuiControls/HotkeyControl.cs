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
    class HotkeyControl : TextBox
    {
        #region Properties
        internal override string DefaultName { get { return "hotkeyControl"; } }
        public override string Text { get { return base.Text; } set {  } }

		//private Keys modifier;
		//public Keys Modifier { get { return modifier; } set { modifier = value; HotkeyToText(); } }
		private Keys hotkey;
		public Keys Hotkey { get { return hotkey; } set { hotkey = value; HotkeyToText(); } }

		[Category("Events")]
		public HotkeyChangedEvent HotkeyChangedEvent { get; set; }
        #endregion

		public HotkeyControl()
        {
            Type = ControlType.HotkeyControl;

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
				base.Text = "None";
			}
			else if (modifier == Keys.None)
			{
				base.Text = key.ToString();
			}
			else if (hotkey != Keys.None)
			{
				base.Text = ModifierToText() + " + " + key.ToString();
			}
			else
			{
				base.Text = ModifierToText();
			}
		}

        public override string ToString()
        {
            return Name + " - HotkeyControl";
        }
    }
}
