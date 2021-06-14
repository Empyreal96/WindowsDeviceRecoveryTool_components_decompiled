using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x0200009C RID: 156
	internal sealed class SByteTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x00014E4C File Offset: 0x0001304C
		internal override object Parse(string text)
		{
			return XmlConvert.ToSByte(text);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00014E59 File Offset: 0x00013059
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((sbyte)instance);
		}
	}
}
