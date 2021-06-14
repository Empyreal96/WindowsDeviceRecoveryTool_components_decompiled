using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x0200008F RID: 143
	internal sealed class BooleanTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x00014C4A File Offset: 0x00012E4A
		internal override object Parse(string text)
		{
			return XmlConvert.ToBoolean(text);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00014C57 File Offset: 0x00012E57
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((bool)instance);
		}
	}
}
