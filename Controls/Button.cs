using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OSHGuiBuilder.Controls
{
    class Button : BaseControl
    {
        #region Properties
        private Label label;

        public override Point Location { get { return base.Location; } set { base.Location = value; label.Location = new Point(location.X + 6, location.Y + 5); } }
        public override Color ForeColor { get { return label.ForeColor; } set { label.ForeColor = value; } }
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (autoSize) { Size = new Size(label.Size.Width + 12, label.Size.Height + 10); } } }
        #endregion

        public Button()
        {
            label = new Label();

            Size = new Size(92, 24);

            BackColor = Color.FromArgb(unchecked((int)0xFF4E4E4E));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            label.Parent = this;
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, new Rectangle(location, size));
            label.Render(graphics);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, location.X - 2, location.Y - 2, size.Width + 3, size.Height + 3);
                }
            }
        }

        public override string ToString()
        {
            return name + " - Button";
        }
    }
}
