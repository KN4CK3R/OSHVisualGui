namespace OSHVisualGui
{
	public struct ChangedProperty
	{
		public object Value { get; }
		public bool UseForCPP { get; }
		public bool UseForXML { get; }

		public ChangedProperty(object value)
			: this(value, true, true)
		{

		}

		public ChangedProperty(object value, bool useForCPP, bool useForXML)
		{
			Value = value;
			UseForCPP = useForCPP;
			UseForXML = useForXML;
		}
	}
}
