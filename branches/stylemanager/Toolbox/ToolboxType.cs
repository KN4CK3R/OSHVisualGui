using System;

namespace OSHVisualGui.Toolbox
{
	public class ToolboxType
	{
		public Type Type { get; private set; }

		public ToolboxType(Type type)
		{
			Type = type;
		}
	}
}
