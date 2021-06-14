using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A3 RID: 163
	internal sealed class DateTimeOffsetTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00014F27 File Offset: 0x00013127
		internal override object Parse(string text)
		{
			return PlatformHelper.ConvertStringToDateTimeOffset(text);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00014F34 File Offset: 0x00013134
		internal override string ToString(object instance)
		{
			return XmlConvert.ToString((DateTimeOffset)instance);
		}
	}
}
