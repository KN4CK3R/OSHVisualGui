using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TabPage : Panel
    {
        #region Properties
        protected string text;
        internal TabControl.TabControlButton button;
        private Panel containerPanel = new Panel();

        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }

        [Browsable(false)]
        public override bool AutoSize { get { return base.AutoSize; } set { base.AutoSize = value; } }
        [Browsable(false)]
        public override bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
        [Browsable(false)]
        public override bool Visible { get { return base.Visible; } set { base.Visible = value; } }
        [Browsable(false)]
        public override Point Location { get { return base.Location; } set { base.Location = value; } }
        [Browsable(false)]
        public override Font Font { get { return base.Font; } set { base.Font = value; } }
        [Browsable(false)]
        public override Size Size { get { return base.Size; } set { base.Size = value; containerPanel.Size = value.Substract(new Size(4, 4)); } }

        #endregion

        public TabPage()
        {
            button = null;

		    containerPanel.Location = new Point(2, 2);
            containerPanel.isSubControl = true;
		    AddSubControl(containerPanel);

            BackColor = Color.FromArgb(unchecked((int)0xFF474747));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

            isSubControl = true;
        }

        public override void AddControl(Control control)
        {
            containerPanel.AddControl(control);
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                Brush brush = new SolidBrush(backColor.Add(Color.FromArgb(0, 32, 32, 32)));
                graphics.FillRectangle(brush, absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
                Rectangle rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);
                brush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 20, 20, 20)), LinearGradientMode.Vertical);
                graphics.FillRectangle(brush, rect);
            }

            containerPanel.Render(graphics);
        }

        public override string ToString()
        {
            return name + " - TabPage";
        }

        public override Control Copy()
        {
            throw new NotImplementedException();
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            throw new NotImplementedException();
        }
    }
}
