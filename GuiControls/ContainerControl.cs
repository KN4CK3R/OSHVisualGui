using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    public abstract class ContainerControl : Control
    {
        protected List<Control> controls;
        internal virtual List<Control> Controls { get { return controls; } }
        protected List<Control> internalControls;
        internal virtual Point ContainerLocation { get { return Location; } }
        internal virtual Point ContainerAbsoluteLocation { get { return absoluteLocation; } }
        internal virtual Size ContainerSize { get { return Size; } }

        public ContainerControl()
        {
            controls = new List<Control>();
            internalControls = new List<Control>();
        }

        public virtual void AddControl(Control control)
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

            Sort();
        }

        protected void AddSubControl(Control control)
        {
            if (controls.Contains(control))
            {
                return;
            }

            control.Parent = this;
            control._zOrder = zOrder + 1;

            internalControls.Add(control);
        }

        public virtual void RemoveControl(Control control)
        {
            internalControls.Remove(control);
            controls.Remove(control);

            control.Parent = null;
        }

        public void Sort()
        {
            internalControls.Sort(DepthSort);
            controls.Sort(DepthSort);
        }

        int DepthSort(Control c1, Control c2)
        {
            return -(c1.CompareTo(c2));
        }

        public void SendToFront(Control control)
        {
            if (!internalControls.Contains(control))
            {
                return;
            }

            int zOrder = 1;
            foreach (Control c in internalControls)
            {
                if (c != control)
                {
                    c.zOrder = zOrder++;
                }
            }
            control.zOrder = zOrder;
            Sort();
        }

        public void SendToBack(Control control)
        {
            if (!internalControls.Contains(control))
            {
                return;
            }

            int zOrder = 1;
            control.zOrder = zOrder++;
            foreach (Control c in internalControls)
            {
                if (c != control)
                {
                    c.zOrder = zOrder++;
                }
            }
            Sort();
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            foreach (Control control in internalControls)
            {
                control.CalculateAbsoluteLocation();
            }
        }

        public override void Render(System.Drawing.Graphics graphics)
        {
            foreach (Control control in controls.FastReverse())
            {
                control.Render(graphics);
            }
        }

        public virtual IEnumerable<Control> PostOrderVisit()
        {
            foreach (Control control in internalControls)
            {
                if (control is ContainerControl)
                {
                    foreach (Control child in (control as ContainerControl).PostOrderVisit())
                    {
                        yield return child;
                    }
                }
                if (!control.isSubControl)
                {
                    yield return control;
                }
            }
        }

        public virtual IEnumerable<Control> PreOrderVisit()
        {
            foreach (Control control in internalControls)
            {
                if (!control.isSubControl)
                {
                    yield return control;
                }
                
                if (control is ContainerControl)
                {
                    foreach (Control child in (control as ContainerControl).PreOrderVisit())
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
