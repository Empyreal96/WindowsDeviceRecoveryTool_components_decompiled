using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000245 RID: 581
	internal static class UriUtilsCommon
	{
		// Token: 0x06001298 RID: 4760 RVA: 0x00045C06 File Offset: 0x00043E06
		internal static string UriToString(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
			{
				return uri.OriginalString;
			}
			return uri.AbsoluteUri;
		}
	}
}
