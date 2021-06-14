using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A6 RID: 166
	internal sealed class UInt32TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00014F8D File Offset: 0x0001318D
		internal override object Parse(string text)
		{
			return XmlConvert.ToUInt32(text);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00014F9A File Offset: 0x0001319A
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((uint)instance);
		}
	}
}
