using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Auth;

namespace Microsoft.WindowsAzure.Storage.Auth.Protocol
{
	// Token: 0x0200000D RID: 13
	[Obsolete("Use SharedKeyAuthenticationHandler")]
	public sealed class SharedKeyLiteAuthenticationHandler : IAuthenticationHandler
	{
		// Token: 0x0600011B RID: 283 RVA: 0x000059D5 File Offset: 0x00003BD5
		public SharedKeyLiteAuthenticationHandler(ICanonicalizer canonicalizer, StorageCredentials credentials, string resourceAccountName)
		{
			this.authenticationHandler = new SharedKeyAuthenticationHandler(canonicalizer, credentials, resourceAccountName);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000059EB File Offset: 0x00003BEB
		public void SignRequest(HttpWebRequest request, OperationContext operationContext)
		{
			this.authenticationHandler.SignRequest(request, operationContext);
		}

		// Token: 0x04000074 RID: 116
		private readonly SharedKeyAuthenticationHandler authenticationHandler;
	}
}
