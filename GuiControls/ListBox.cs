using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
    class ListBox : Control
    {
        #region Properties
        internal override string DefaultName { get { return "listBox"; } }
        private string[] items;
        public string[] Items { get { return items; } set { items = value; } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }
        #endregion

        public ListBox()
        {
            Size = new Size(120, 95);

            BackColor = Color.FromArgb(unchecked((int)0xFF171614));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);
            Brush tempBrush = new SolidBrush(backColor.Add(Color.FromArgb(0, 54, 53, 52)));
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y, size.Width - 2, 1);
            graphics.FillRectangle(tempBrush, absoluteLocation.X, absoluteLocation.Y + 1, 1, size.Height - 2);
            graphics.FillRectangle(tempBrush, absoluteLocation.X + size.Width - 1, absoluteLocation.Y + 1, 1, size.Height - 2);
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y + size.Height - 1, size.Width - 2, 1);

            graphics.DrawString(name, font, foreBrush, absoluteLocation.X + 5, absoluteLocation.Y + 5);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 3);
                }
            }
        }

        public override Control Copy()
        {
            ListBox copy = new ListBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            ListBox listBox = copy as ListBox;
            string[] itemsCopy = new string[items.Length];
            for (int i = 0; i < items.Length; ++i)
            {
                itemsCopy[i] = items[i];
            }
            listBox.items = itemsCopy;
        }

        public override string ToString()
        {
            return name + " - ListBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::ListBox();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (Size != new Size(120, 95))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF171614)))
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (Items != null)
            {
                foreach (string item in Items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        code.AppendLine(linePrefix + name + "->AddItem(OSHGui::Misc::AnsiString(\"" + item.Replace("\"", "\\\"") + "\"));");
                    }
                }
            }
            return code.ToString();
        }
    }
}
