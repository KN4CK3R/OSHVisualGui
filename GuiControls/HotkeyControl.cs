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

		private Keys modifier;
		public Keys Modifier { get { return modifier; } set { modifier = value; HotkeyToText(); } }
		private Keys hotkey;
		public Keys Hotkey { get { return hotkey; } set { hotkey = value; HotkeyToText(); } }

		[Category("Events")]
		public HotkeyChangedEvent HotkeyChangedEvent { get; set; }
        #endregion

		public HotkeyControl()
        {
            Type = ControlType.HotkeyControl;

			Modifier = Keys.Control;
			Hotkey = Keys.F12;

			HotkeyChangedEvent = new HotkeyChangedEvent(this);
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
			List<string> modifiers = new List<string>();
			if ((modifier & Keys.Control) != Keys.None) modifiers.Add("Control");
			if ((modifier & Keys.Menu) != Keys.None) modifiers.Add("Alt");
			if ((modifier & Keys.Shift) != Keys.None) modifiers.Add("Shift");

			return string.Join(" + ", modifiers.ToArray());
		}

		private void HotkeyToText()
		{
			if (modifier == Keys.None && hotkey == Keys.None)
			{
				base.Text = "None";
			}
			else if (modifier == Keys.None)
			{
				base.Text = hotkey.ToString();
			}
			else if (hotkey != Keys.None)
			{
				base.Text = ModifierToText() + " + " + hotkey.ToString();
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
