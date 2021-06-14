using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200009B RID: 155
	internal sealed class StringTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x00014E39 File Offset: 0x00013039
		internal override object Parse(string text)
		{
			return text;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00014E3C File Offset: 0x0001303C
		internal override string ToString(object instance)
		{
			return (string)instance;
		}
	}
}
