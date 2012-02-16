using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSHGuiBuilder.Controls
{
    class ContainerControl : BaseControl
    {
        private List<BaseControl> controls;
        private List<BaseControl> internalControls;

        public IEnumerable<BaseControl> PostOrderVisible
        {
            get { return PostOrderVisibleVisit(this); }
        }

        public ContainerControl()
        {
            controls = new List<BaseControl>();
            internalControls = new List<BaseControl>();
        }

        public virtual void AddControl(BaseControl control)
        {
            if (control == null)
            {
                return;
            }

            if (control is Form)
            {
                return;
            }

            AddSubControl(control);

            controls.Add(control);
        }

        protected void AddSubControl(BaseControl control)
        {
            if (controls.Contains(control))
            {
                return;
            }

            control.Parent = this;

            internalControls.Add(control);
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

        private IEnumerable<BaseControl> PostOrderVisibleVisit(ContainerControl container)
        {
            foreach (BaseControl control in container.internalControls)
            {
                if (control.Visible)
                {
                    if (control is ContainerControl)
                    {
                        foreach (BaseControl child in PostOrderVisibleVisit(control as ContainerControl))
                        {
                            if (child.isSubControl)
                            {
                                continue;
                            }
                            yield return child;
                        }
                    }
                    if (control.isSubControl)
                    {
                        continue;
                    }
                    yield return control;
                }
                else continue;
            }
        }
    }
}
