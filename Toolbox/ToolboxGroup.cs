using System.Collections.Generic;

namespace OSHVisualGui.Toolbox
{
	public class ToolboxGroup : ToolboxItemBase
	{
		private readonly List<ToolboxItem> items;

		public ToolboxGroup(string caption)
		{
			items = new List<ToolboxItem>();
			this.Caption = caption;
			Expanded = false;
		}

		public List<ToolboxItem> Items => items;

		public int ItemHeight => 19 * items.Count;

		public bool Expanded { get; set; }
	}
}
