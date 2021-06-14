using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A5 RID: 165
	public sealed class RetryInfo
	{
		// Token: 0x06001050 RID: 4176 RVA: 0x0003E2B8 File Offset: 0x0003C4B8
		public RetryInfo()
		{
			this.TargetLocation = StorageLocation.Primary;
			this.UpdatedLocationMode = LocationMode.PrimaryOnly;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003E2E2 File Offset: 0x0003C4E2
		public RetryInfo(RetryContext retryContext)
		{
			CommonUtility.AssertNotNull("retryContext", retryContext);
			this.TargetLocation = retryContext.NextLocation;
			this.UpdatedLocationMode = retryContext.LocationMode;
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0003E321 File Offset: 0x0003C521
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x0003E329 File Offset: 0x0003C529
		public StorageLocation TargetLocation { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0003E332 File Offset: 0x0003C532
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x0003E33A File Offset: 0x0003C53A
		public LocationMode UpdatedLocationMode { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0003E343 File Offset: 0x0003C543
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x0003E34B File Offset: 0x0003C54B
		public TimeSpan RetryInterval
		{
			get
			{
				return this.interval;
			}
			set
			{
				this.interval = CommonUtility.MaxTimeSpan(value, TimeSpan.Zero);
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0003E360 File Offset: 0x0003C560
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "({0},{1})", new object[]
			{
				this.TargetLocation,
				this.RetryInterval
			});
		}

		// Token: 0x040003D9 RID: 985
		private TimeSpan interval = TimeSpan.FromSeconds(3.0);
	}
}
