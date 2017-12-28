namespace OSHVisualGui
{
	public struct ChangedProperty
	{
		public object Value { get; }
		public bool UseForCpp { get; }
		public bool UseForXml { get; }

		public ChangedProperty(object value)
			: this(value, true, true)
		{

		}

		public ChangedProperty(object value, bool useForCpp, bool useForXml)
		{
			Value = value;
			UseForCpp = useForCpp;
			UseForXml = useForXml;
		}
	}
}
