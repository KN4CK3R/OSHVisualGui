using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    public class PictureBox : Control
    {
        #region Properties
        internal override string DefaultName { get { return "pictureBox"; } }
        #endregion

        public PictureBox()
        {
            Size = new Size(100, 100);

            BackColor = Color.Empty;
            ForeColor = Color.Empty;
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, size));
            }

            using (Pen pen = new Pen(Color.Yellow, 2))
            {
                graphics.DrawRectangle(pen, absoluteLocation.X + 2, absoluteLocation.Y + 2, size.Width - 4, size.Height - 4);
            }

            graphics.DrawString(name, font, new SolidBrush(Color.Black), absoluteLocation.X + 4, absoluteLocation.Y + 4);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 1, absoluteLocation.Y - 1, size.Width + 1, size.Height + 1);
                }
            }
        }

        public override Control Copy()
        {
            PictureBox copy = new PictureBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);
        }

        public override string ToString()
        {
            return name + " - PictureBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::PictureBox();");
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
            if (size != new Size(100, 100))
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
            return code.ToString();
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
        }
    }
}
