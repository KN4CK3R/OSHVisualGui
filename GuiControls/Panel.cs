using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml;

namespace OSHVisualGui.GuiControls
{
    public class Panel : ContainerControl
    {
        #region Properties
        internal override string DefaultName { get { return "panel"; } }
        #endregion

        public Panel()
        {
            Size = new Size(200, 200);

            BackColor = Color.Empty;
            ForeColor = Color.Empty;
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                Rectangle rect = new Rectangle(absoluteLocation, size);
                LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 90, 90, 90)), LinearGradientMode.Vertical);
                graphics.FillRectangle(linearBrush, rect);
            }

            graphics.DrawString(name, font, new SolidBrush(Color.Black), absoluteLocation.X + 5, absoluteLocation.Y + 5);
            
            base.Render(graphics);

            if (isFocused || isHighlighted)
            {
                using (Pen pen = new Pen(isHighlighted ? Color.Orange : Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
                }

                isHighlighted = false;
            }
        }

        public override Control Copy()
        {
            Panel copy = new Panel();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            Panel panel = copy as Panel;
            foreach (Control control in PreOrderVisit())
            {
                panel.AddControl(control.Copy());
            }
        }

        public override string ToString()
        {
            return name + " - Panel";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::Panel();");
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
            if (size != new Size(200, 200))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.Empty)
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }

            if (Controls.Count > 0)
            {
                code.AppendLine("");
                foreach (Control control in Controls.FastReverse())
                {
                    code.Append(control.ToCPlusPlusString(linePrefix));
                    code.AppendLine(linePrefix + name + "->AddControl(" + control.Name + ");\r\n");
                }
            }

            return code.ToString();
        }

        protected override void WriteToXmlElement(XmlDocument document, XmlElement element)
        {
            base.WriteToXmlElement(document, element);
            foreach (Control control in Controls.FastReverse())
            {
                control.AddToXmlElement(document, element);
            }
        }

        public override Control XmlElementToControl(XmlElement element)
        {
            throw new NotImplementedException();
        }
    }
}
