using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000096 RID: 150
	internal sealed class GuidTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x00014D94 File Offset: 0x00012F94
		internal override object Parse(string text)
		{
			return new Guid(text);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00014DA1 File Offset: 0x00012FA1
		internal override string ToString(object instance)
		{
			return instance.ToString();
		}
	}
}
