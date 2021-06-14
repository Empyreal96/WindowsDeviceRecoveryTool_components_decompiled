using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x02000059 RID: 89
	public sealed class SharedKeyCanonicalizer : ICanonicalizer
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x000313A4 File Offset: 0x0002F5A4
		public static SharedKeyCanonicalizer Instance
		{
			get
			{
				return SharedKeyCanonicalizer.instance;
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000313AB File Offset: 0x0002F5AB
		private SharedKeyCanonicalizer()
		{
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x000313B3 File Offset: 0x0002F5B3
		public string AuthorizationScheme
		{
			get
			{
				return "SharedKey";
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000313BC File Offset: 0x0002F5BC
		public string CanonicalizeHttpRequest(HttpWebRequest request, string accountName)
		{
			CommonUtility.AssertNotNull("request", request);
			CanonicalizedString canonicalizedString = new CanonicalizedString(request.Method);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.ContentEncoding]);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.ContentLanguage]);
			AuthenticationUtility.AppendCanonicalizedContentLengthHeader(canonicalizedString, request);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.ContentMd5]);
			canonicalizedString.AppendCanonicalizedElement(request.ContentType);
			AuthenticationUtility.AppendCanonicalizedDateHeader(canonicalizedString, request, false);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.IfModifiedSince]);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.IfMatch]);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.IfNoneMatch]);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.IfUnmodifiedSince]);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.Range]);
			AuthenticationUtility.AppendCanonicalizedCustomHeaders(canonicalizedString, request);
			string canonicalizedResourceString = AuthenticationUtility.GetCanonicalizedResourceString(request.RequestUri, accountName, false);
			canonicalizedString.AppendCanonicalizedElement(canonicalizedResourceString);
			return canonicalizedString.ToString();
		}

		// Token: 0x040001AD RID: 429
		private const string SharedKeyAuthorizationScheme = "SharedKey";

		// Token: 0x040001AE RID: 430
		private static SharedKeyCanonicalizer instance = new SharedKeyCanonicalizer();
	}
}
