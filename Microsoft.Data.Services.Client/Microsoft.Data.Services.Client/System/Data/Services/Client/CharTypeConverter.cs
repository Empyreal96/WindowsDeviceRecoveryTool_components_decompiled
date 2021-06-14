using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x0200009D RID: 157
	internal sealed class CharTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x00014E6E File Offset: 0x0001306E
		internal override object Parse(string text)
		{
			return XmlConvert.ToChar(text);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00014E7B File Offset: 0x0001307B
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((char)instance);
		}
	}
}
