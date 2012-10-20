﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class LinkLabel : Label
    {
        #region Properties
        internal override string DefaultName { get { return "linkLabel"; } }
        private Font underlinedFont;

        public override Font Font { get { return base.Font; } set { base.Font = value; underlinedFont = new Font(value, FontStyle.Underline); } }
        #endregion

        public LinkLabel()
        {
            Type = ControlType.LinkLabel;

            underlinedFont = new Font(Font, FontStyle.Underline);
        }

        public override void Render(Graphics graphics)
        {
			if (BackColor.A != 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}
			graphics.DrawString(text, underlinedFont, foreBrush, new RectangleF(AbsoluteLocation, Size));
        }

        public override Control Copy()
        {
            LinkLabel copy = new LinkLabel();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            LinkLabel linkLabel = copy as LinkLabel;
            linkLabel.underlinedFont = new Font(underlinedFont, FontStyle.Underline);
        }

        public override string ToString()
        {
            return Name + " - LinkLabel";
        }
    }
}
