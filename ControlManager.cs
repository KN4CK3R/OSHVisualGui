using System;
using System.Collections.Generic;
using System.Text;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
    public class ControlManager
    {
        private static ControlManager instance = new ControlManager();
        private Form form;
        public Form Form { set { form = value; } }
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

        public bool AddControl(Control control)
        {
            if (control == null || controls.Contains(control))
            {
                return false;
            }

            controls.Add(control);

            return true;
        }

        public bool RemoveControl(Control control)
        {
            if (control == null)
            {
                return false;
            }

            if (controls.Contains(control))
            {
                controls.Remove(control);

                return true;
            }

            return false;
        }

        public IEnumerable<Control> FindByName(string name)
        {
            foreach (Control control in controls)
            {
                if (control.Name == name)
                {
                    yield return control;
                }
            }
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
