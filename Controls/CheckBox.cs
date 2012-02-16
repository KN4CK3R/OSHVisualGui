using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHGuiBuilder.Controls
{
    class CheckBox : BaseControl
    {
        #region Properties
        private Label label;

        private bool checked_;
        public bool Checked { get { return checked_; } set { checked_ = value; } }
        public override Size Size { get { return base.Size; } set { if (!autoSize) { base.Size = value; label.Size = value; } } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (autoSize) { size = new Size(label.Size.Width + 20, label.Size.Height + 2); } } }
        #endregion

        public CheckBox()
        {
            label = new Label();
            label.Location = new Point(20, 2);

            BackColor = Color.FromArgb(unchecked((int)0xFF222222));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

            AutoSize = true;

            CalculateAbsoluteLocation();
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            label.Parent = this;
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, new Size(17, 17)));
            if (checked_)
            {
                graphics.FillRectangle(foreBrush, absoluteLocation.X + 4, absoluteLocation.Y + 4, 9, 9);
            }
            label.Render(graphics);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 4);
                }
            }
        }

        public override string ToString()
        {
            return name + " - CheckBox";
        }
    }
}
