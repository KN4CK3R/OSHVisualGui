using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class GroupBox : ContainerControl
    {
        #region Properties
        private Label label = new Label();
        private Panel panel = new Panel();

        internal override string DefaultName { get { return "groupBox"; } }
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; } }
        internal override List<Control> Controls { get { return panel.Controls; } }
        public override Size Size { get { return base.Size; } set { base.Size = value; panel.Size = value.Add(new Size(-3 * 2, -3 * 2 - 10)); } }
        internal override Point ContainerLocation { get { return base.ContainerLocation.Add(panel.Location); } }
        internal override Point ContainerAbsoluteLocation { get { return panel.ContainerAbsoluteLocation; } }
        internal override Size ContainerSize { get { return panel.ContainerSize; } }
        #endregion

        public GroupBox()
        {
            Size = new Size(200, 200);

            label.Location = new Point(5, -1);
            label.isSubControl = true;
            AddSubControl(label);

            panel.Location = new Point(3, 10);
            panel.isSubControl = true;
            AddSubControl(panel);

            BackColor = Color.Empty;
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(Control control)
        {
            panel.AddControl(control);
        }

        public override void Render(Graphics graphics)
        {
            if (backColor.A > 0)
            {
                graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, size));
            }
            label.Render(graphics);

            graphics.FillRectangle(foreBrush, absoluteLocation.X + 1, absoluteLocation.Y + 5, 3, 1);
            graphics.FillRectangle(foreBrush, absoluteLocation.X + label.Size.Width + 5, absoluteLocation.Y + 5, size.Width - label.Size.Width - 6, 1);
            graphics.FillRectangle(foreBrush, absoluteLocation.X, absoluteLocation.Y + 6, 1, size.Height - 7);
            graphics.FillRectangle(foreBrush, absoluteLocation.X + size.Width - 1, absoluteLocation.Y + 6, 1, size.Height - 7);
            graphics.FillRectangle(foreBrush, absoluteLocation.X + 1, absoluteLocation.Y + size.Height - 1, size.Width - 2, 1);

            panel.Render(graphics);

            if (isHighlighted)
            {
                using (Pen pen = new Pen(Color.Orange, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
                }

                isHighlighted = false;
            }
        }

        public override Control Copy()
        {
            GroupBox copy = new GroupBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            GroupBox groupBox = copy as GroupBox;
            groupBox.Text = Text;

            foreach (Control control in panel.Controls)
            {
                groupBox.AddControl(control.Copy());
            }
        }

        public override string ToString()
        {
            return name + " - GroupBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::GroupBox();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(linePrefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(linePrefix + name + "->SetVisible(false);");
            }
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
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + Text.Replace("\"", "\\\"") + "\"));");

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

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);
            element.Add(new XAttribute("text", Text));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("text") != null)
                Text = element.Attribute("text").Value.Trim();
            else
                throw new Exception("Missing attribute 'text': " + element.Name);
        }
    }
}
