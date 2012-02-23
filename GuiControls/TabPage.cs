using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TabPage : Panel
    {
        #region Properties
        protected string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }

        /*protected bool Enabled { get { return enabled; } set { enabled = value; } }
        protected bool Visible { get { return visible; } set { visible = value; } }
        protected override Point Location { get { return location; } set { location = value; CalculateAbsoluteLocation(); } }
        protected override Size Size { get { return size; } set { size = value; } }
        protected override bool AutoSize { get { return autoSize; } set { autoSize = value; } }
        protected override Font Font { get { return font; } set { font = value; } }
        protected override Color ForeColor { get { return foreColor; } set { foreColor = value; foreBrush = new SolidBrush(foreColor); } }
        protected override Color BackColor { get { return backColor; } set { backColor = value; backBrush = new SolidBrush(backColor); } }*/
        #endregion

        public TabPage()
        {

            BackColor = Color.FromArgb(unchecked((int)0xFF474747));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override string ToString()
        {
            return name + " - TabPage";
        }
    }
}
