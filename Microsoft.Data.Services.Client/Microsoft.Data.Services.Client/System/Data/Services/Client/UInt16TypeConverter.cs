using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A5 RID: 165
	internal sealed class UInt16TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00014F6B File Offset: 0x0001316B
		internal override object Parse(string text)
		{
			return XmlConvert.ToUInt16(text);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00014F78 File Offset: 0x00013178
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((ushort)instance);
		}
	}
}
