using System;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000015 RID: 21
	public interface IPropertyValueFormatter
	{
		// Token: 0x06000092 RID: 146
		object ReadFrom(byte[] buffer, int index, int count, PropertyType propertyType);

		// Token: 0x06000093 RID: 147
		bool TryReadFrom(byte[] buffer, int index, int count, PropertyType propertyType, out object value);
	}
}
