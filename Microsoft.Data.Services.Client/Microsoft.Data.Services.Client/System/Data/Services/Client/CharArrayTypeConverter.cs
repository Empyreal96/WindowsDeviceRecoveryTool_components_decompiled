using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200009E RID: 158
	internal sealed class CharArrayTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600055C RID: 1372 RVA: 0x00014E90 File Offset: 0x00013090
		internal override object Parse(string text)
		{
			return text.ToCharArray();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00014E98 File Offset: 0x00013098
		internal override string ToString(object instance)
		{
			return new string((char[])instance);
		}
	}
}
