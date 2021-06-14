using System;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A3 RID: 163
	public sealed class NoRetry : IRetryPolicy
	{
		// Token: 0x06001044 RID: 4164 RVA: 0x0003E1F6 File Offset: 0x0003C3F6
		public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext)
		{
			retryInterval = TimeSpan.Zero;
			return false;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003E205 File Offset: 0x0003C405
		public IRetryPolicy CreateInstance()
		{
			return new NoRetry();
		}
	}
}
