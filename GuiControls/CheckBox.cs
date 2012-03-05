﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml;

namespace OSHVisualGui.GuiControls
{
    public class CheckBox : Control
    {
        #region Properties
        internal override string DefaultName { get { return "checkBox"; } }

        protected Label label;

        protected bool _checked;
        public virtual bool Checked { get { return _checked; } set { _checked = value; } }
        public override Size Size { get { return base.Size; } set { if (!autoSize) { base.Size = value; label.Size = value; } } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; label.ForeColor = value; } }
        public string Text { get { return label.Text; } set { label.Text = value == null ? string.Empty : value; if (autoSize) { size = new Size(label.Size.Width + 20, label.Size.Height + 2); } } }
        #endregion

        public CheckBox()
        {
            label = new Label();
            label.Location = new Point(20, 2);

            BackColor = Color.FromArgb(unchecked((int)0xFF222222));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

            AutoSize = true;

            CalculateAbsoluteLocation();
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            label.Parent = this;
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(backBrush, new Rectangle(absoluteLocation, new Size(17, 17)));
            Rectangle rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 1, 15, 15);
            LinearGradientBrush temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);
            rect = new Rectangle(absoluteLocation.X + 2, absoluteLocation.Y + 2, 13, 13);
            temp = new LinearGradientBrush(rect, backColor, backColor.Add(Color.FromArgb(0, 55, 55, 55)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);

            if (_checked)
            {
                graphics.FillRectangle(new SolidBrush(Color.White), absoluteLocation.X + 5, absoluteLocation.Y + 5, 7, 7);
                rect = new Rectangle(absoluteLocation.X + 6, absoluteLocation.Y + 6, 5, 5);
                temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
                graphics.FillRectangle(temp, rect);
            }
            label.Render(graphics);

            if (isFocused)
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 2, absoluteLocation.Y - 2, size.Width + 3, size.Height + 4);
                }
            }
        }

        public override Control Copy()
        {
            CheckBox copy = new CheckBox();
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            CheckBox checkBox = copy as CheckBox;
            checkBox._checked = _checked;
            checkBox.Text = Text;
        }

        public override string ToString()
        {
            return name + " - CheckBox";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::CheckBox();");
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
            if (!autoSize)
            {
                code.AppendLine(linePrefix + name + "->SetAutoSize(false);");
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF222222)))
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(linePrefix + name + "->SetText(OSHGui::Misc::AnsiString(\"" + Text.Replace("\"", "\\\"") + "\"));");
            if (_checked)
            {
                code.AppendLine(linePrefix + name + "->SetChecked(true);");
            }
            return code.ToString();
        }

        protected override void WriteToXmlElement(XmlDocument document, XmlElement element)
        {
            base.WriteToXmlElement(document, element);
            element.Attributes.Append(document.CreateValueAttribute("text", Text));
            element.Attributes.Append(document.CreateValueAttribute("checked", Checked.ToString().ToLower()));
        }

        public override Control XmlElementToControl(XmlElement element)
        {
            CheckBox checkBox = new CheckBox();
            ReadFromXml(element, checkBox);
            return checkBox;
        }
        protected override void ReadFromXml(XmlElement element, Control control)
        {
            base.ReadFromXml(element, control);

            CheckBox checkBox = control as CheckBox;
            if (element.Attributes["text"] != null)
                checkBox.Text = element.Attributes["text"].Value.Trim();
            else
                throw new XmlException("Missing attribute 'text': " + element.Name);
            if (element.Attributes["text"] != null)
                checkBox.Checked = bool.Parse(element.Attributes["checked"].Value.Trim());
            else
                throw new XmlException("Missing attribute 'checked': " + element.Name);
        }
    }
}
