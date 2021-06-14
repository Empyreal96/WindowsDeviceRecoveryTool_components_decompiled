using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000095 RID: 149
	internal sealed class DoubleTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x00014D72 File Offset: 0x00012F72
		internal override object Parse(string text)
		{
			return XmlConvert.ToDouble(text);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00014D7F File Offset: 0x00012F7F
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((double)instance);
		}
	}
}
