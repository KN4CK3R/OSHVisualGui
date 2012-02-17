using System;
using System.Collections.Generic;
using System.Text;

namespace OSHGuiBuilder.Toolbox
{
    public class ToolboxItem : ToolboxItemBase
    {

        private int iconIndex;
        private object data;

        public ToolboxItem(string caption, int iconIndex, object data)
        {
            this.caption = caption;
            this.iconIndex = iconIndex;
            this.data = data;
        }

        public int IconIndex
        {
            get
            {
                return iconIndex;
            }
            set
            {
                iconIndex = value;
            }
        }

        public object Data
        {
            get
            {
                return data;
            }
        }

    }
}
