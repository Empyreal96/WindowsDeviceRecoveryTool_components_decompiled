using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A7 RID: 167
	internal sealed class UInt64TypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x00014FAF File Offset: 0x000131AF
		internal override object Parse(string text)
		{
			return XmlConvert.ToUInt64(text);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00014FBC File Offset: 0x000131BC
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((ulong)instance);
		}
	}
}
