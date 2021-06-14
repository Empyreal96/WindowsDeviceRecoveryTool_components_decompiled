using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A1 RID: 161
	public sealed class LinearRetry : IExtendedRetryPolicy, IRetryPolicy
	{
		// Token: 0x0600103D RID: 4157 RVA: 0x0003DFE4 File Offset: 0x0003C1E4
		public LinearRetry() : this(LinearRetry.DefaultClientBackoff, 3)
		{
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0003DFF2 File Offset: 0x0003C1F2
		public LinearRetry(TimeSpan deltaBackoff, int maxAttempts)
		{
			this.deltaBackoff = deltaBackoff;
			this.maximumAttempts = maxAttempts;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0003E020 File Offset: 0x0003C220
		public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("lastException", lastException);
			retryInterval = TimeSpan.Zero;
			if ((statusCode >= 300 && statusCode < 500 && statusCode != 408) || statusCode == 501 || statusCode == 505 || lastException.Message == "Blob type of the blob reference doesn't match blob type of the blob.")
			{
				return false;
			}
			retryInterval = this.deltaBackoff;
			return currentRetryCount < this.maximumAttempts;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0003E098 File Offset: 0x0003C298
		public RetryInfo Evaluate(RetryContext retryContext, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("retryContext", retryContext);
			if (retryContext.LastRequestResult.TargetLocation == StorageLocation.Primary)
			{
				this.lastPrimaryAttempt = new DateTimeOffset?(retryContext.LastRequestResult.EndTime);
			}
			else
			{
				this.lastSecondaryAttempt = new DateTimeOffset?(retryContext.LastRequestResult.EndTime);
			}
			bool flag = retryContext.LastRequestResult.TargetLocation == StorageLocation.Secondary && retryContext.LastRequestResult.HttpStatusCode == 404;
			TimeSpan t;
			if (this.ShouldRetry(retryContext.CurrentRetryCount, flag ? 500 : retryContext.LastRequestResult.HttpStatusCode, retryContext.LastRequestResult.Exception, out t, operationContext))
			{
				RetryInfo retryInfo = new RetryInfo(retryContext);
				if (flag && retryContext.LocationMode != LocationMode.SecondaryOnly)
				{
					retryInfo.UpdatedLocationMode = LocationMode.PrimaryOnly;
					retryInfo.TargetLocation = StorageLocation.Primary;
				}
				DateTimeOffset? dateTimeOffset = (retryInfo.TargetLocation == StorageLocation.Primary) ? this.lastPrimaryAttempt : this.lastSecondaryAttempt;
				if (dateTimeOffset != null)
				{
					TimeSpan t2 = CommonUtility.MaxTimeSpan(DateTimeOffset.Now - dateTimeOffset.Value, TimeSpan.Zero);
					retryInfo.RetryInterval = t - t2;
				}
				else
				{
					retryInfo.RetryInterval = TimeSpan.Zero;
				}
				return retryInfo;
			}
			return null;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0003E1C6 File Offset: 0x0003C3C6
		public IRetryPolicy CreateInstance()
		{
			return new LinearRetry(this.deltaBackoff, this.maximumAttempts);
		}

		// Token: 0x040003CA RID: 970
		private const int DefaultClientRetryCount = 3;

		// Token: 0x040003CB RID: 971
		private static readonly TimeSpan DefaultClientBackoff = TimeSpan.FromSeconds(30.0);

		// Token: 0x040003CC RID: 972
		private TimeSpan deltaBackoff;

		// Token: 0x040003CD RID: 973
		private int maximumAttempts;

		// Token: 0x040003CE RID: 974
		private DateTimeOffset? lastPrimaryAttempt = null;

		// Token: 0x040003CF RID: 975
		private DateTimeOffset? lastSecondaryAttempt = null;
	}
}
