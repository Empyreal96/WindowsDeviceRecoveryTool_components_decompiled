using System;
using System.Net;

namespace Microsoft.WindowsAzure.Storage.Core.Auth
{
	// Token: 0x02000058 RID: 88
	public interface ICanonicalizer
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000D58 RID: 3416
		string AuthorizationScheme { get; }

		// Token: 0x06000D59 RID: 3417
		string CanonicalizeHttpRequest(HttpWebRequest request, string accountName);
	}
}
