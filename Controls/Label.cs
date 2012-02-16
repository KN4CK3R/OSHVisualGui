using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSHGuiBuilder.Controls
{
    class Label : BaseControl
    {
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; if (autoSize) { size = TextRenderer.MeasureText(text, font); } } }

        public Label()
        {
            autoSize = true;

            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawString(text, font, foreBrush, new RectangleF(location, size));

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, location.X - 1, location.Y - 1, size.Width + 1, size.Height + 1);
                }
            }
        }

        public override string ToString()
        {
            return name + " - Label";
        }
    }
}
