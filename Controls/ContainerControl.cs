using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSHGuiBuilder.Controls
{
    class ContainerControl : BaseControl
    {
        private List<BaseControl> controls;

        public ContainerControl()
        {
            controls = new List<BaseControl>();
        }

        public void AddControl(BaseControl control)
        {
            if (control == null)
            {
                return;
            }

            if (control is Form)
            {
                return;
            }

            if (controls.Contains(control))
            {
                return;
            }

            control.Parent = this;

            controls.Add(control);
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            foreach (BaseControl control in controls)
            {
                control.CalculateAbsoluteLocation();
            }
        }

        public override void Render(System.Drawing.Graphics graphics)
        {
            foreach (BaseControl control in controls)
            {
                control.Render(graphics);
            }
        }
    }
}
