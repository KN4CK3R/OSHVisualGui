using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSHVisualGui
{
	public struct ChangedProperty
	{
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

		public object Value;
		public bool UseForCPP;
		public bool UseForXML;
	}
}
