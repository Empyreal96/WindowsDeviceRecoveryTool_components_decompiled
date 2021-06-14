using System;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x0200009E RID: 158
	public interface IRetryPolicy
	{
		// Token: 0x06001034 RID: 4148
		IRetryPolicy CreateInstance();

		// Token: 0x06001035 RID: 4149
		bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext);
	}
}
