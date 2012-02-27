﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    class Timer : Control
    {
        #region Properties
        internal override string DefaultName { get { return "timer"; } }
        private long interval;
        public long Interval { get { return interval; } set { if (value >= 1) { interval = value; } } }
        private Image icon;
        #endregion

        public Timer(Image icon)
        {
            this.icon = icon;

            Size = new Size(16, 16);

            interval = 100;
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawImage(icon, absoluteLocation);
   
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
            Timer copy = new Timer(icon);
            CopyTo(copy);
            return copy;
        }

        protected override void CopyTo(Control copy)
        {
            base.CopyTo(copy);

            Timer timer = copy as Timer;
            timer.interval = interval;
        }

        public override string ToString()
        {
            return name + " - Timer";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::Timer();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (interval != 100)
            {
                code.AppendLine(linePrefix + name + "->SetInterval(" + interval + ");");
            }
            return code.ToString();
        }
    }
}
