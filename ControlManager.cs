using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSHVisualGui
{
    public class ControlManager
    {
        private static ControlManager instance = new ControlManager();
        private GuiControls.Form form;
        public GuiControls.Form Form { set { form = value; } }

        public static ControlManager Instance()
        {
            return instance;
        }

        public int GetControlCount(Type controlType)
        {
            int count = 1;

            foreach (GuiControls.Control control in form.PostOrderVisit())
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
