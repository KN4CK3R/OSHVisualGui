using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHGuiBuilder.GuiControls
{
    class GroupBox : Panel
    {
        private Label label = new Label();
        private Panel panel = new Panel();

        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; } }
        public override List<BaseControl> Controls { get { return panel.Controls; } }
        public override Size Size { get { return base.Size; } set { base.Size = value; panel.Size = value.Add(new Size(-3 * 2, -3 * 2 - 10)); } }
        public override Point GetContainerLocation() { return base.GetContainerLocation().Add(panel.Location); }
        public override Point GetContainerAbsoluteLocation() { return panel.GetContainerAbsoluteLocation(); }

        public GroupBox()
        {
            label.Location = new Point(5, -1);
            label.isSubControl = true;
            AddSubControl(label);

            panel.Location = new Point(3, 10);
            panel.isSubControl = true;
            AddSubControl(panel);

            BackColor = Color.Empty;
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(BaseControl control)
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

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
                }
            }
        }

        public override BaseControl Copy()
        {
            GroupBox copy = new GroupBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(BaseControl copy)
        {
            base.CopyTo(copy);

            GroupBox groupBox = copy as GroupBox;
            groupBox.Text = Text;
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
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }

            if (panel.Controls.Count > 0)
            {
                code.AppendLine("");
                foreach (BaseControl control in panel.Controls)
                {
                    code.Append(control.ToCPlusPlusString(linePrefix));
                    code.AppendLine(linePrefix + name + "->AddControl(" + control.Name + ");\r\n");
                }
            }

            return code.ToString();
        }
    }
}
