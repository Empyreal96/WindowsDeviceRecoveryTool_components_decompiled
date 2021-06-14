using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000091 RID: 145
	internal sealed class ByteArrayTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000532 RID: 1330 RVA: 0x00014C8E File Offset: 0x00012E8E
		internal override object Parse(string text)
		{
			return Convert.FromBase64String(text);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00014C96 File Offset: 0x00012E96
		internal override string ToString(object instance)
		{
			return Convert.ToBase64String((byte[])instance);
		}
	}
}
