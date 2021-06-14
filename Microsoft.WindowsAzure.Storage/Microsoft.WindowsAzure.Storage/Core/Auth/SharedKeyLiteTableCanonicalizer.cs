using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x0200005B RID: 91
	public sealed class SharedKeyLiteTableCanonicalizer : ICanonicalizer
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00031556 File Offset: 0x0002F756
		public static SharedKeyLiteTableCanonicalizer Instance
		{
			get
			{
				return SharedKeyLiteTableCanonicalizer.instance;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0003155D File Offset: 0x0002F75D
		private SharedKeyLiteTableCanonicalizer()
		{
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00031565 File Offset: 0x0002F765
		public string AuthorizationScheme
		{
			get
			{
				return "SharedKeyLite";
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003156C File Offset: 0x0002F76C
		public string CanonicalizeHttpRequest(HttpWebRequest request, string accountName)
		{
			CommonUtility.AssertNotNull("request", request);
			string preferredDateHeaderValue = AuthenticationUtility.GetPreferredDateHeaderValue(request);
			CanonicalizedString canonicalizedString = new CanonicalizedString(preferredDateHeaderValue, 150);
			string canonicalizedResourceString = AuthenticationUtility.GetCanonicalizedResourceString(request.RequestUri, accountName, true);
			canonicalizedString.AppendCanonicalizedElement(canonicalizedResourceString);
			return canonicalizedString.ToString();
		}

		// Token: 0x040001B2 RID: 434
		private const string SharedKeyLiteAuthorizationScheme = "SharedKeyLite";

		// Token: 0x040001B3 RID: 435
		private const int ExpectedCanonicalizedStringLength = 150;

		// Token: 0x040001B4 RID: 436
		private static SharedKeyLiteTableCanonicalizer instance = new SharedKeyLiteTableCanonicalizer();
	}
}
