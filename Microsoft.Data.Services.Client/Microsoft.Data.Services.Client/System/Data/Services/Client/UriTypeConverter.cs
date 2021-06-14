using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000A0 RID: 160
	internal sealed class UriTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x00014ECA File Offset: 0x000130CA
		internal override object Parse(string text)
		{
			return UriUtil.CreateUri(text, UriKind.RelativeOrAbsolute);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00014ED3 File Offset: 0x000130D3
		internal override string ToString(object instance)
		{
			return UriUtil.UriToString((Uri)instance);
		}
	}
}
