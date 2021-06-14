using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000127 RID: 295
	internal static class UriUtil
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x00028065 File Offset: 0x00026265
		internal static string UriToString(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
			{
				return uri.OriginalString;
			}
			return uri.AbsoluteUri;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002807C File Offset: 0x0002627C
		internal static Uri CreateUri(string value, UriKind kind)
		{
			if (value != null)
			{
				return new Uri(value, kind);
			}
			return null;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002808C File Offset: 0x0002628C
		internal static Uri CreateUri(Uri baseUri, Uri requestUri)
		{
			Util.CheckArgumentNull<Uri>(requestUri, "requestUri");
			if (!baseUri.IsAbsoluteUri)
			{
				return UriUtil.CreateUri(UriUtil.UriToString(baseUri) + '/' + UriUtil.UriToString(requestUri), UriKind.Relative);
			}
			if (requestUri.IsAbsoluteUri)
			{
				return requestUri;
			}
			return UriUtil.AppendBaseUriAndRelativeUri(baseUri, requestUri);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000280E0 File Offset: 0x000262E0
		private static Uri AppendBaseUriAndRelativeUri(Uri baseUri, Uri relativeUri)
		{
			string text = UriUtil.UriToString(baseUri);
			string text2 = UriUtil.UriToString(relativeUri);
			if (text.EndsWith("/", StringComparison.Ordinal))
			{
				if (text2.StartsWith("/", StringComparison.Ordinal))
				{
					relativeUri = new Uri(baseUri, UriUtil.CreateUri(text2.TrimStart(UriUtil.ForwardSlash), UriKind.Relative));
				}
				else
				{
					relativeUri = new Uri(baseUri, relativeUri);
				}
			}
			else
			{
				relativeUri = UriUtil.CreateUri(text + "/" + text2.TrimStart(UriUtil.ForwardSlash), UriKind.Absolute);
			}
			return relativeUri;
		}

		// Token: 0x040005A6 RID: 1446
		internal static readonly char[] ForwardSlash = new char[]
		{
			'/'
		};
	}
}
