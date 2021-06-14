using System;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x0200009F RID: 159
	public interface IExtendedRetryPolicy : IRetryPolicy
	{
		// Token: 0x06001036 RID: 4150
		RetryInfo Evaluate(RetryContext retryContext, OperationContext operationContext);
	}
}
