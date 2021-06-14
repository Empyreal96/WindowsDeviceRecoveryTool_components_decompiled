using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000093 RID: 147
	internal sealed class DateTimeTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600053B RID: 1339 RVA: 0x00014D2E File Offset: 0x00012F2E
		internal override object Parse(string text)
		{
			return PlatformHelper.ConvertStringToDateTime(text);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00014D3B File Offset: 0x00012F3B
		internal override string ToString(object instance)
		{
			return PlatformHelper.ConvertDateTimeToString((DateTime)instance);
		}
	}
}
