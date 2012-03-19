using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Drawing.Design;

namespace OSHVisualGui.GuiControls
{
    public class PictureBox : ScalableControl
    {
        #region Properties
        internal override string DefaultName { get { return "pictureBox"; } }
        private string path;
        [Editor(typeof(FilenameEditor), typeof(UITypeEditor)), FileDialogFilter("Image files (*.jpg, *.bmp, *.gif, *.png)|*.jpg;*.bmp;*.gif;*.png|All files (*.*)|*.*")]
        public string Path { 
            get { return path; }
            set
            {
                Image tempImage = Image.FromFile(value);
                path = value;
                image = tempImage;
            }
        }
        private Image image;
        #endregion

        public PictureBox()
        {
            Type = ControlType.PictureBox;

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

            if (image != null)
            {
                graphics.DrawImage(image, absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
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

        public override string ToCPlusPlusString(string prefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(prefix + name + " = new OSHGui::PictureBox();");
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
            if (size != new Size(100, 100))
            {
                code.AppendLine(prefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.Empty)
            {
                code.AppendLine(prefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.Empty)
            {
                code.AppendLine(prefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            if (!string.IsNullOrEmpty(path))
            {
                code.AppendLine(prefix + name + "->SetImage(Application::GetRenderer()->CreateNewTexture(\"" + path.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"));");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XElement element)
        {
            base.WriteToXmlElement(element);

            element.Add(new XAttribute("image", path));
        }

        public override void ReadPropertiesFromXml(XElement element)
        {
            base.ReadPropertiesFromXml(element);

            if (element.Attribute("image") != null)
                Path = element.Attribute("image").Value.Trim();
            else
                throw new Exception("Missing attribute 'image': " + element.Name);
        }
    }
}
