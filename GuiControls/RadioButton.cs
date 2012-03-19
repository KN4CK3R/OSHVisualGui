﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
    class RadioButton : CheckBox
    {
        #region Properties
        internal override string DefaultName { get { return "radioButton"; } }
        public override bool Checked { get { return base._checked; }
            set
            {
                if (_checked != value)
                {
                    if (parent != null)
                    {
                        foreach (Control control in (parent as ContainerControl).Controls)
                        {
                            if (control is RadioButton)
                            {
                                (control as RadioButton)._checked = false;
                            }
                        }
                    }
                    _checked = value;
                }
            }
        }
        #endregion

        public RadioButton()
        {
            Type = ControlType.RadioButton;
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
                rect = new Rectangle(absoluteLocation.X + 5, absoluteLocation.Y + 5, 7, 7);
                temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
                graphics.FillEllipse(temp, rect);
            }
            label.Render(graphics);
        }

        public override string ToString()
        {
            return name + " - RadioButton";
        }

        public override string ToCPlusPlusString(string prefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(prefix + name + " = new OSHGui::RadioButton();");
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
            if (!autoSize)
            {
                code.AppendLine(prefix + name + "->SetAutoSize(false);");
                code.AppendLine(prefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF222222)))
            {
                code.AppendLine(prefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(prefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }
            code.AppendLine(prefix + name + "->SetText(\"" + Text.Replace("\"", "\\\"") + "\");");
            if (_checked)
            {
                code.AppendLine(prefix + name + "->SetChecked(true);");
            }
            return code.ToString();
        }
    }
}
