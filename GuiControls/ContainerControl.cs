using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    public abstract class ContainerControl : BaseControl
    {
        protected List<BaseControl> controls;
        public virtual List<BaseControl> GetControls() { return controls; }
        protected List<BaseControl> internalControls;
        public virtual Point GetContainerLocation() { return Location; }
        public virtual Point GetContainerAbsoluteLocation() { return absoluteLocation; }
        public virtual Size GetContainerSize() { return Size; }

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

            control.SetParent(this);

            internalControls.Add(control);
        }

        public virtual void RemoveControl(BaseControl control)
        {
            internalControls.Remove(control);
            controls.Remove(control);

            control.SetParent(null);
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            foreach (BaseControl control in internalControls)
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

        public IEnumerable<BaseControl> PostOrderVisit()
        {
            foreach (BaseControl control in internalControls)
            {
                if (control is ContainerControl)
                {
                    foreach (BaseControl child in (control as ContainerControl).PostOrderVisit())
                    {
                        if (!child.isSubControl)
                        {
                            yield return child;
                        }
                    }
                }
                if (!control.isSubControl)
                {
                    yield return control;
                }
            }
        }

        public IEnumerable<BaseControl> PreOrderVisit()
        {
            foreach (BaseControl control in internalControls)
            {
                if (!control.isSubControl)
                {
                    yield return control;
                }
                
                if (control is ContainerControl)
                {
                    foreach (BaseControl child in (control as ContainerControl).PreOrderVisit())
                    {
                        if (!child.isSubControl)
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        protected IEnumerable<string> GetControlNames()
        {
            foreach (BaseControl control in controls)
            {
                yield return control.Name;
                if (control is ContainerControl)
                {
                    foreach (string name in (control as ContainerControl).GetControlNames())
                    {
                        yield return name;
                    }
                }
            }
        }
    }
}
