using System;
using System.Collections.Generic;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000012 RID: 18
	public interface IDevicePropertySet
	{
		// Token: 0x0600008E RID: 142
		IEnumerable<PropertyKey> EnumeratePropertyKeys();

		// Token: 0x0600008F RID: 143
		object ReadProperty(PropertyKey key, IPropertyValueFormatter formatter);

		// Token: 0x06000090 RID: 144
		bool TryReadProperty(PropertyKey key, IPropertyValueFormatter formatter, out object value);
	}
}
