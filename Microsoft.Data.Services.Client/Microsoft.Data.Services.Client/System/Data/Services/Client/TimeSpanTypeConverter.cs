using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A4 RID: 164
	internal sealed class TimeSpanTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x00014F49 File Offset: 0x00013149
		internal override object Parse(string text)
		{
			return XmlConvert.ToTimeSpan(text);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00014F56 File Offset: 0x00013156
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((TimeSpan)instance);
		}
	}
}
