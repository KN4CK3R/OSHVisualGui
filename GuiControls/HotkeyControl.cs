using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class HotkeyControl : TextBox
    {
        #region Properties
        internal override string DefaultName { get { return "hotkeyControl"; } }
        public override string Text { get { return base.Text; } set {  } }
        #endregion

		public HotkeyControl()
        {
            Type = ControlType.HotkeyControl;

			base.Text = "Control + A";
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

        public override string ToString()
        {
            return Name + " - HotkeyControl";
        }
    }
}
