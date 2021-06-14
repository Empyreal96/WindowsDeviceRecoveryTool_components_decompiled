using System;
using System.Net;

namespace Microsoft.WindowsAzure.Storage.Auth.Protocol
{
	// Token: 0x0200000A RID: 10
	public interface IAuthenticationHandler
	{
		// Token: 0x06000116 RID: 278
		void SignRequest(HttpWebRequest request, OperationContext operationContext);
	}
}
