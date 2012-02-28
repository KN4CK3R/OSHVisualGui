using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
    public class ComboBox : Button
    {
        #region Properties
        internal override string DefaultName { get { return "comboBox"; } }
        private string[] items;
        public string[] Items { get { return items; } set { items = value; } }
        #endregion

        public ComboBox()
        {
            Size = new Size(160, 24);
        }

        public override void Render(Graphics graphics)
        {
            base.Render(graphics);

            int arrowLeft = absoluteLocation.X + size.Width - 9;
            int arrowTop = absoluteLocation.Y + size.Height - 11;
            for (int i = 0; i < 4; ++i)
            {
                graphics.FillRectangle(foreBrush, arrowLeft - i, arrowTop - i, 1 + i * 2, 1);
            }
        }

        public override Control Copy()
        {
            ComboBox copy = new ComboBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            ComboBox comboBox = copy as ComboBox;
            string[] itemsCopy = new string[items.Length];
            for (int i = 0; i < items.Length; ++i)
            {
                itemsCopy[i] = items[i];
            }
            comboBox.items = itemsCopy;
        }

        public override string ToString()
        {
            return name + " - ComboBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::ComboBox();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(linePrefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(linePrefix + name + "->SetVisible(false);");
            }
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (autoSize)
            {
                code.AppendLine(linePrefix + name + "->SetAutoSize(true);");
            }
            else if (size != new Size(160, 24))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF4E4E4E)))
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
