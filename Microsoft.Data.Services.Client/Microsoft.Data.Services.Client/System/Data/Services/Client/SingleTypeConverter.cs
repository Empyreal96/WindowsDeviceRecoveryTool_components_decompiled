using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x0200009A RID: 154
	internal sealed class SingleTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000550 RID: 1360 RVA: 0x00014E17 File Offset: 0x00013017
		internal override object Parse(string text)
		{
			return XmlConvert.ToSingle(text);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00014E24 File Offset: 0x00013024
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((float)instance);
		}
	}
}
