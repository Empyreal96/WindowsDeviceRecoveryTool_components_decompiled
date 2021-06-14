using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x0200005C RID: 92
	public sealed class SharedKeyTableCanonicalizer : ICanonicalizer
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x000315BE File Offset: 0x0002F7BE
		public static SharedKeyTableCanonicalizer Instance
		{
			get
			{
				return SharedKeyTableCanonicalizer.instance;
			}
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000315C5 File Offset: 0x0002F7C5
		private SharedKeyTableCanonicalizer()
		{
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x000315CD File Offset: 0x0002F7CD
		public string AuthorizationScheme
		{
			get
			{
				return "SharedKey";
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x000315D4 File Offset: 0x0002F7D4
		public string CanonicalizeHttpRequest(HttpWebRequest request, string accountName)
		{
			CommonUtility.AssertNotNull("request", request);
			CanonicalizedString canonicalizedString = new CanonicalizedString(request.Method, 200);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.ContentMd5]);
			canonicalizedString.AppendCanonicalizedElement(request.ContentType);
			AuthenticationUtility.AppendCanonicalizedDateHeader(canonicalizedString, request, true);
			string canonicalizedResourceString = AuthenticationUtility.GetCanonicalizedResourceString(request.RequestUri, accountName, true);
			canonicalizedString.AppendCanonicalizedElement(canonicalizedResourceString);
			return canonicalizedString.ToString();
		}

		// Token: 0x040001B5 RID: 437
		private const string SharedKeyAuthorizationScheme = "SharedKey";

		// Token: 0x040001B6 RID: 438
		private const int ExpectedCanonicalizedStringLength = 200;

		// Token: 0x040001B7 RID: 439
		private static SharedKeyTableCanonicalizer instance = new SharedKeyTableCanonicalizer();
	}
}
