using System;
using System.Collections.Generic;
using System.Text;

namespace OSHVisualGui.Toolbox
{
    public class ToolboxItemBase
    {

        protected int top;
        protected bool mouseOver;
        protected string caption;
        protected bool selected;

        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }

        public bool MouseOver
        {
            get
            {
                return mouseOver;
            }
            set
            {
                mouseOver = value;
            }
        }

        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }
    }
}
