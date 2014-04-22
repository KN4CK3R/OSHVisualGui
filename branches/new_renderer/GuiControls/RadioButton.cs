using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
    class RadioButton : CheckBox
    {
        #region Properties
        internal override string DefaultName { get { return "radioButton"; } }
        public override bool Checked { get { return base._checked; }
            set
            {
                if (_checked != value)
                {
                    if (Parent != null)
                    {
                        foreach (Control control in (Parent as ContainerControl).Controls)
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
            graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, new Size(17, 17)));
            Rectangle rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, 15, 15);
            LinearGradientBrush temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);
            rect = new Rectangle(AbsoluteLocation.X + 2, AbsoluteLocation.Y + 2, 13, 13);
            temp = new LinearGradientBrush(rect, BackColor, BackColor.Add(Color.FromArgb(0, 55, 55, 55)), LinearGradientMode.Vertical);
            graphics.FillRectangle(temp, rect);

            if (_checked)
            {
                rect = new Rectangle(AbsoluteLocation.X + 5, AbsoluteLocation.Y + 5, 7, 7);
                temp = new LinearGradientBrush(rect, Color.White, Color.White.Substract(Color.FromArgb(0, 137, 137, 137)), LinearGradientMode.Vertical);
                graphics.FillEllipse(temp, rect);
            }
            label.Render(graphics);
        }

		public override Control Copy()
		{
			RadioButton copy = new RadioButton();
			CopyTo(copy);
			return copy;
		}

        public override string ToString()
        {
            return Name + " - RadioButton";
        }
    }
}
