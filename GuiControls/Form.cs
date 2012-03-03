using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [Serializable]
    public class Form : ContainerControl
    {
        private Panel panel;
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }
        internal override List<Control> Controls { get { return panel.Controls; } }
        internal override Point ContainerLocation { get { return base.ContainerLocation.Add(panel.Location); } }
        internal override Point ContainerAbsoluteLocation { get { return panel.ContainerAbsoluteLocation; } }
        internal override Size ContainerSize { get { return panel.ContainerSize; } }
        public override Size Size { get { return base.Size; } set { base.Size = value; panel.Size = new Size(value.Width - 2 * 6, value.Height - 17 - 2 * 6); } }

        public Form()
        {
            Parent = this;

            panel = new Panel();
            panel.Location = new Point(6, 6 + 17);
            panel.isSubControl = true;
            AddSubControl(panel);

            Size = new Size(300, 300);

            BackColor = Color.FromArgb(unchecked((int)0xFF7C7B79));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(Control control)
        {
            panel.AddControl(control);
        }

        public override void RemoveControl(Control control)
        {
            panel.RemoveControl(control);
        }

        public override void Render(Graphics graphics)
        {
            Rectangle rect = new Rectangle(absoluteLocation, size);
            LinearGradientBrush linearBrush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 100, 100, 100)), LinearGradientMode.Vertical);

            graphics.FillRectangle(linearBrush, rect);
            graphics.DrawString(text, font, foreBrush, new PointF(absoluteLocation.X + 4, absoluteLocation.Y + 2));
            graphics.FillRectangle(new SolidBrush(backColor.Substract(Color.FromArgb(0, 50, 50, 50))), absoluteLocation.X + 5, absoluteLocation.Y + 17 + 2, size.Width - 10, 1);

            Point crossLocation = new Point(absoluteLocation.X + size.Width - 17, absoluteLocation.Y + 5); 
            for (int i = 0; i < 4; ++i)
            {
                graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + i, crossLocation.Y + 7 - i, 3, 1);
                graphics.FillRectangle(foreBrush, crossLocation.X + 6 - i, crossLocation.Y + 7 - i, 3, 1);
            }

            panel.Render(graphics);

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
            throw new NotImplementedException();
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);
        }

        public override string ToString()
        {
            return name + " - Form";
        }

        public string[] GenerateCode()
        {
            string[] generatedCode = new string[2];

            StringBuilder code = new StringBuilder();
            code.AppendLine("#ifndef OSHGUI_" + name.ToUpper() + "_HPP");
            code.AppendLine("#define OSHGUI_" + name.ToUpper() + "_HPP\r\n");
            code.AppendLine("#include <OSHGui.hpp>\r\n");
            code.AppendLine("class " + name + " : OSHGui::Form");
            code.AppendLine("{");
            code.AppendLine("public:");
            code.AppendLine("\t" + name + "();");
            code.AppendLine("\r\nprivate:");
            code.AppendLine("\tvoid InitializeComponent()");
            code.AppendLine("\t{");
            code.AppendLine("\t\tSetName(\"" + name + "\");");
            if (size != new Size(300, 300))
            {
                code.AppendLine("\t\tSetSize(Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine("\t\tSetForeColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF7C7B79)))
            {
                code.AppendLine("\t\tSetBackColor(Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine("\t\tSetText(\"" + text.Replace("\"", "\\\"") + "\");");
            if (Controls.Count > 0)
            {
                code.AppendLine(string.Empty);
                foreach (Control control in Controls.FastReverse())
                {
                    if (control != this)
                    {
                        code.Append(control.ToCPlusPlusString("\t\t"));
                        code.AppendLine("\t\tAddControl(" + control.Name + ");\r\n");
                    }
                }
                code.Length -= 2;
                code.AppendLine("\t}\r\n");
                foreach (Control control in PreOrderVisit())
                {
                    code.AppendLine("\tOSHGui::" + control.GetType().Name + " *" + control.Name + ";");
                }
            }
            else
            {
                code.AppendLine("\t}");
            }
            code.AppendLine("};\r\n");
            code.AppendLine("#endif");
            generatedCode[0] = code.ToString();

            code.Length = 0;
            code.AppendLine("#include \"" + name + ".hpp\"\r\n");
            code.AppendLine(name + "::" + name + "()");
            code.AppendLine("{");
            code.AppendLine("\tInitializeComponent();");
            code.AppendLine("}");
            code.AppendLine("//---------------------------------------------------------------------------");
            generatedCode[1] = code.ToString();

            return generatedCode;
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            throw new Exception("Call GenerateCode");
        }
    }
}
