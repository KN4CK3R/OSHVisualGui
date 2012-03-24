﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    [Serializable]
    public abstract class ContainerControl : ScalableControl
    {
        protected List<Control> controls;
        internal virtual List<Control> Controls { get { return controls; } }
        protected List<Control> internalControls;
        internal virtual Point ContainerLocation { get { return Location; } }
        internal virtual Point ContainerAbsoluteLocation { get { return AbsoluteLocation; } }
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
            control._zOrder = internalControls.Count + 1;

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
            return c1.zOrder.CompareTo(c2.zOrder);
        }

        public void SendToFront(Control control)
        {
            if (!internalControls.Contains(control))
            {
                return;
            }

            int zOrder = control._zOrder;
            foreach (Control c in internalControls)
            {
                if (c != control && c._zOrder > zOrder)
                {
                    --c._zOrder;
                }
            }
            control._zOrder = internalControls.Count;
            Sort();
        }

        public void SendToBack(Control control)
        {
            if (!internalControls.Contains(control))
            {
                return;
            }

            int zOrder = control._zOrder;
            control._zOrder = 1;
            foreach (Control c in internalControls)
            {
                if (c != control && c._zOrder < zOrder)
                {
                    ++c._zOrder;
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
            foreach (Control control in controls)
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

        internal override void RegisterInternalControls()
        {
            foreach (var control in Controls)
            {
                ControlManager.Instance().RegisterControl(control);
                control.RegisterInternalControls();
            }
        }

        internal override void UnregisterInternalControls()
        {
            foreach (var control in Controls)
            {
                ControlManager.Instance().UnregisterControl(control);
                control.UnregisterInternalControls();
            }
        }
    }
}
