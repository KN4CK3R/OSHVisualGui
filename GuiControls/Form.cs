using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHGuiBuilder.GuiControls
{
    public class Form : ContainerControl
    {
        private Panel panel;
        private string text;
        public string Text { get { return text; } set { text = value == null ? string.Empty : value; } }
        public override List<BaseControl> GetControls() { return panel.GetControls(); }
        public override Point GetContainerLocation() { return base.GetContainerLocation().Add(panel.Location); }
        public override Point GetContainerAbsoluteLocation() { return panel.GetContainerAbsoluteLocation(); }

        public Form()
        {
            SetParent(this);

            Size = new Size(300, 300);

            panel = new Panel();
            panel.Location = new Point(6, 6 + 17);
            panel.isSubControl = true;
            AddSubControl(panel);

            BackColor = Color.FromArgb(unchecked((int)0xFF7C7B79));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(BaseControl control)
        {
            panel.AddControl(control);
        }

        public override void RemoveControl(BaseControl control)
        {
            panel.RemoveControl(control);
        }

        public override void Render(System.Drawing.Graphics graphics)
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
        }

        public override BaseControl Copy()
        {
            throw new NotImplementedException();
        }

        protected override void CopyTo(BaseControl copy)
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
            code.AppendLine("\t" + name + "()");
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
            code.AppendLine("\t}\r\n");

            code.AppendLine("private:");
            code.AppendLine("\tvoid InitializeComponent()");
            code.AppendLine("\t{");
            foreach (BaseControl control in GetControls())
            {
                code.Append(control.ToCPlusPlusString("\t\t"));
                code.AppendLine("\t\tAddControl(" + control.Name + ");\r\n");
            }
            code.AppendLine("\t}\r\n");
            foreach (BaseControl control in PreOrderVisit())
            {
                code.AppendLine("\tOSHGui::" + control.GetType().Name + " " + control.Name + ";");
            }
            
            code.AppendLine("};\r\n");
            code.AppendLine("#endif");
            generatedCode[0] = code.ToString();

            code.Length = 0;
            code.AppendLine("#include \"" + name + ".hpp\"\r\n");
            code.AppendLine("");

            return generatedCode;
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            throw new Exception("Call GenerateCode");
        }
    }
}
