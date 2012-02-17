using System;
using System.Collections.Generic;
using System.Text;

namespace OSHGuiBuilder.Toolbox
{
    public class ToolboxGroup : ToolboxItemBase
    {

        private List<ToolboxItem> items;
        private bool expanded;

        public ToolboxGroup(string caption)
        {
            items = new List<ToolboxItem>();
            this.caption = caption;
            expanded = false;
        }

        public List<ToolboxItem> Items
        {
            get
            {
                return items;
            }
        }

        public int ItemHeight
        {
            get
            {
                return 19 * items.Count;
            }
        }

        public bool Expanded
        {
            get
            {
                return expanded;
            }
            set
            {
                expanded = value;
            }
        }
    }
}
