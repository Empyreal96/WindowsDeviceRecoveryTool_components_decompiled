using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x0200005A RID: 90
	public sealed class SharedKeyLiteCanonicalizer : ICanonicalizer
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x000314C1 File Offset: 0x0002F6C1
		public static SharedKeyLiteCanonicalizer Instance
		{
			get
			{
				return SharedKeyLiteCanonicalizer.instance;
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000314C8 File Offset: 0x0002F6C8
		private SharedKeyLiteCanonicalizer()
		{
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x000314D0 File Offset: 0x0002F6D0
		public string AuthorizationScheme
		{
			get
			{
				return "SharedKeyLite";
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000314D8 File Offset: 0x0002F6D8
		public string CanonicalizeHttpRequest(HttpWebRequest request, string accountName)
		{
			CommonUtility.AssertNotNull("request", request);
			CanonicalizedString canonicalizedString = new CanonicalizedString(request.Method, 250);
			canonicalizedString.AppendCanonicalizedElement(request.Headers[HttpRequestHeader.ContentMd5]);
			canonicalizedString.AppendCanonicalizedElement(request.ContentType);
			AuthenticationUtility.AppendCanonicalizedDateHeader(canonicalizedString, request, false);
			AuthenticationUtility.AppendCanonicalizedCustomHeaders(canonicalizedString, request);
			string canonicalizedResourceString = AuthenticationUtility.GetCanonicalizedResourceString(request.RequestUri, accountName, true);
			canonicalizedString.AppendCanonicalizedElement(canonicalizedResourceString);
			return canonicalizedString.ToString();
		}

		// Token: 0x040001AF RID: 431
		private const string SharedKeyLiteAuthorizationScheme = "SharedKeyLite";

		// Token: 0x040001B0 RID: 432
		private const int ExpectedCanonicalizedStringLength = 250;

		// Token: 0x040001B1 RID: 433
		private static SharedKeyLiteCanonicalizer instance = new SharedKeyLiteCanonicalizer();
	}
}
