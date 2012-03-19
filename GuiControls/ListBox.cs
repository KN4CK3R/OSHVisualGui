using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class ListBox : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "listBox"; } }
        private string[] items;
        public string[] Items { get { return items; } set { items = value; } }
        #endregion

        public ListBox()
        {
            Type = ControlType.ListBox;

            defaultSize = Size = new Size(120, 95);

            defaultBackColor = BackColor = Color.FromArgb(unchecked((int)0xFF171614));
            defaultForeColor = ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
        {
            foreach (var pair in base.GetChangedProperties())
            {
                yield return pair;
            }
            if (Items != null && Items.Length > 0)
            {
                foreach (var item in Items)
                {
                    yield return new KeyValuePair<string, object>("AddItem", item);
                }
            }
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);
            Brush tempBrush = new SolidBrush(backColor.Add(Color.FromArgb(0, 54, 53, 52)));
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y, size.Width - 2, 1);
            graphics.FillRectangle(tempBrush, absoluteLocation.X, absoluteLocation.Y + 1, 1, size.Height - 2);
            graphics.FillRectangle(tempBrush, absoluteLocation.X + size.Width - 1, absoluteLocation.Y + 1, 1, size.Height - 2);
            graphics.FillRectangle(tempBrush, absoluteLocation.X + 1, absoluteLocation.Y + size.Height - 1, size.Width - 2, 1);

            if (Items != null && Items.Length > 0)
            {
                int y = 5;
                for (int i = 0; i < Items.Length; ++i)
                {
                    Size stringSize = TextRenderer.MeasureText(Items[i], Font);
                    if (y + stringSize.Height >= Size.Height)
                    {
                        break;
                    }
                    graphics.DrawString(Items[i], font, foreBrush, absoluteLocation.X + 5, absoluteLocation.Y + y);
                    y += stringSize.Height;
                }
            }
            else
            {
                graphics.DrawString(name, font, foreBrush, absoluteLocation.X + 5, absoluteLocation.Y + 5);
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

        public override string ToCPlusPlusString(string prefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(prefix + name + " = new OSHGui::ListBox();");
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
            if (Size != new Size(120, 95))
            {
                code.AppendLine(prefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF171614)))
            {
                code.AppendLine(prefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(prefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (Items != null)
            {
                foreach (string item in Items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        code.AppendLine(prefix + name + "->AddItem(OSHGui::Misc::AnsiString(\"" + item.Replace("\"", "\\\"") + "\"));");
                    }
                }
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);

            foreach (string item in Items)
            {
                element.Add(new XElement("item", item));
            }
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            List<string> itemList = new List<string>();
            foreach (XElement itemElement in element.Nodes())
            {
                itemList.Add(itemElement.Value);
            }
            if (itemList.Count > 0)
            {
                Items = itemList.ToArray();
            }
        }
    }
}
