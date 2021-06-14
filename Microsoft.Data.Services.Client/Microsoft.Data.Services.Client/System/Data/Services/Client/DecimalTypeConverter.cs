using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000094 RID: 148
	internal sealed class DecimalTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600053E RID: 1342 RVA: 0x00014D50 File Offset: 0x00012F50
		internal override object Parse(string text)
		{
			return XmlConvert.ToDecimal(text);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00014D5D File Offset: 0x00012F5D
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((decimal)instance);
		}
	}
}
