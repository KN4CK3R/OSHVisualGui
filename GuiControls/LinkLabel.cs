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

            underlinedFont = new Font(font, FontStyle.Underline);
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawString(text, underlinedFont, foreBrush, new RectangleF(absoluteLocation, size));
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
            return name + " - LinkLabel";
        }

        public override string ToCPlusPlusString(string prefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(prefix + name + " = new OSHGui::LinkLabel();");
            code.AppendLine(prefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(prefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(prefix + name + "->SetVisible(false);");
            }
            if (location != new Point(6, 6))
            {
                code.AppendLine(prefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (!autoSize)
            {
                code.AppendLine(prefix + name + "->SetAutoSize(false);");
                code.AppendLine(prefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(prefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(prefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(prefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + text.Replace("\"", "\\\"") + "\"));");
            return code.ToString();
        }
    }
}
