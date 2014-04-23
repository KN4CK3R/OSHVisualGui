using System;

namespace OSHVisualGui.Toolbox
{
	[Serializable]
	public class ToolboxType
	{
		private Type type;

		public ToolboxType(Type type)
		{
			this.type = type;
		}

		public Type Type
		{
			get
			{
				return type;
			}
		}
	}
}
