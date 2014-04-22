using System;
using System.Collections.Generic;
using System.Text;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
    public class ControlManager
    {
        private static ControlManager instance = new ControlManager();
        private List<Control> controls;
        public List<Control> Controls { get { return controls; } }

        private ControlManager()
        {
            controls = new List<Control>();
        }

        public static ControlManager Instance()
        {
            return instance;
        }

        public void Clear()
        {
            controls.Clear();
        }

        public void RegisterControl(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (controls.Contains(control))
            {
                throw new ArgumentException("Control already exists.");
            }

            foreach (var c in controls)
            {
                if (control.Name == c.Name)
                {
                    throw new Exception("A control with name '" + control.Name + "' already exists.");
                }
            }

            controls.Add(control);

            controls.Sort(delegate(Control c1, Control c2)
            {
                return c1.Name.CompareTo(c2.Name);
            });
        }

        public void UnregisterControl(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (controls.Contains(control))
            {
                controls.Remove(control);
            }
            else
            {
                throw new ArgumentException("Control does not exist.");
            }
        }

        public Control FindByName(string name, Control skip)
        {
            foreach (Control control in controls)
            {
                if (skip != control && control.Name == name)
                {
                    return control;
                }
            }
            return null;
        }

        public int GetControlCount(Type controlType)
        {
            int count = 1;

            foreach (Control control in controls)
            {
                if (control.GetType() == controlType)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
