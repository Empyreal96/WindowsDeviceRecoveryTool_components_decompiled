using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A0 RID: 160
	public sealed class ExponentialRetry : IExtendedRetryPolicy, IRetryPolicy
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x0003DD27 File Offset: 0x0003BF27
		public ExponentialRetry() : this(ExponentialRetry.DefaultClientBackoff, 3)
		{
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003DD35 File Offset: 0x0003BF35
		public ExponentialRetry(TimeSpan deltaBackoff, int maxAttempts)
		{
			this.deltaBackoff = deltaBackoff;
			this.maximumAttempts = maxAttempts;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003DD64 File Offset: 0x0003BF64
		public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("lastException", lastException);
			retryInterval = TimeSpan.Zero;
			if ((statusCode >= 300 && statusCode < 500 && statusCode != 408) || statusCode == 501 || statusCode == 505 || lastException.Message == "Blob type of the blob reference doesn't match blob type of the blob.")
			{
				return false;
			}
			if (currentRetryCount < this.maximumAttempts)
			{
				Random random = new Random();
				double num = (Math.Pow(2.0, (double)currentRetryCount) - 1.0) * (double)random.Next((int)(this.deltaBackoff.TotalMilliseconds * 0.8), (int)(this.deltaBackoff.TotalMilliseconds * 1.2));
				retryInterval = ((num < 0.0) ? ExponentialRetry.MaxBackoff : TimeSpan.FromMilliseconds(Math.Min(ExponentialRetry.MaxBackoff.TotalMilliseconds, ExponentialRetry.MinBackoff.TotalMilliseconds + num)));
				return true;
			}
			return false;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003DE68 File Offset: 0x0003C068
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

		// Token: 0x0600103B RID: 4155 RVA: 0x0003DF96 File Offset: 0x0003C196
		public IRetryPolicy CreateInstance()
		{
			return new ExponentialRetry(this.deltaBackoff, this.maximumAttempts);
		}

		// Token: 0x040003C2 RID: 962
		private const int DefaultClientRetryCount = 3;

		// Token: 0x040003C3 RID: 963
		private static readonly TimeSpan DefaultClientBackoff = TimeSpan.FromSeconds(4.0);

		// Token: 0x040003C4 RID: 964
		private static readonly TimeSpan MaxBackoff = TimeSpan.FromSeconds(120.0);

		// Token: 0x040003C5 RID: 965
		private static readonly TimeSpan MinBackoff = TimeSpan.FromSeconds(3.0);

		// Token: 0x040003C6 RID: 966
		private TimeSpan deltaBackoff;

		// Token: 0x040003C7 RID: 967
		private int maximumAttempts;

		// Token: 0x040003C8 RID: 968
		private DateTimeOffset? lastPrimaryAttempt = null;

		// Token: 0x040003C9 RID: 969
		private DateTimeOffset? lastSecondaryAttempt = null;
	}
}
