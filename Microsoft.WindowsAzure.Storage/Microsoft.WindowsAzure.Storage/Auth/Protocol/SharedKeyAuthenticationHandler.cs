using System;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Auth.Protocol
{
	// Token: 0x0200000C RID: 12
	public sealed class SharedKeyAuthenticationHandler : IAuthenticationHandler
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00005898 File Offset: 0x00003A98
		public SharedKeyAuthenticationHandler(ICanonicalizer canonicalizer, StorageCredentials credentials, string resourceAccountName)
		{
			this.canonicalizer = canonicalizer;
			this.credentials = credentials;
			this.accountName = resourceAccountName;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000058B8 File Offset: 0x00003AB8
		public void SignRequest(HttpWebRequest request, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("request", request);
			if (!request.Headers.AllKeys.Contains("x-ms-date", StringComparer.Ordinal))
			{
				string value = HttpWebUtility.ConvertDateTimeToHttpString(DateTime.UtcNow);
				request.Headers.Add("x-ms-date", value);
			}
			if (this.credentials.IsSharedKey)
			{
				StorageAccountKey key = this.credentials.Key;
				if (!string.IsNullOrEmpty(key.KeyName))
				{
					request.Headers.Add("x-ms-key-name", key.KeyName);
				}
				string text = this.canonicalizer.CanonicalizeHttpRequest(request, this.accountName);
				Logger.LogVerbose(operationContext, "StringToSign = {0}.", new object[]
				{
					text
				});
				string text2 = CryptoUtility.ComputeHmac256(key.KeyValue, text);
				request.Headers.Add("Authorization", string.Format(CultureInfo.InvariantCulture, "{0} {1}:{2}", new object[]
				{
					this.canonicalizer.AuthorizationScheme,
					this.credentials.AccountName,
					text2
				}));
			}
		}

		// Token: 0x04000071 RID: 113
		private readonly ICanonicalizer canonicalizer;

		// Token: 0x04000072 RID: 114
		private readonly StorageCredentials credentials;

		// Token: 0x04000073 RID: 115
		private readonly string accountName;
	}
}
