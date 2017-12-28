
namespace OSHVisualGui.Toolbox
{
	public class ToolboxItem : ToolboxItemBase
	{
		public ToolboxItem(string caption, int iconIndex, object data)
		{
			this.Caption = caption;
			IconIndex = iconIndex;
			Data = data;
		}

		public int IconIndex { get; set; }

		public object Data { get; }
	}
}
