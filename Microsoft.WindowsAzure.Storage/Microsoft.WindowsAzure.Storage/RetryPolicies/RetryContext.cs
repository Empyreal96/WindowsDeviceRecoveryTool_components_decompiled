using System;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A4 RID: 164
	public sealed class RetryContext
	{
		// Token: 0x06001046 RID: 4166 RVA: 0x0003E20C File Offset: 0x0003C40C
		internal RetryContext(int currentRetryCount, RequestResult lastRequestResult, StorageLocation nextLocation, LocationMode locationMode)
		{
			this.CurrentRetryCount = currentRetryCount;
			this.LastRequestResult = lastRequestResult;
			this.NextLocation = nextLocation;
			this.LocationMode = locationMode;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0003E231 File Offset: 0x0003C431
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x0003E239 File Offset: 0x0003C439
		public StorageLocation NextLocation { get; private set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0003E242 File Offset: 0x0003C442
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x0003E24A File Offset: 0x0003C44A
		public LocationMode LocationMode { get; private set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0003E253 File Offset: 0x0003C453
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x0003E25B File Offset: 0x0003C45B
		public int CurrentRetryCount { get; private set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0003E264 File Offset: 0x0003C464
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x0003E26C File Offset: 0x0003C46C
		public RequestResult LastRequestResult { get; private set; }

		// Token: 0x0600104F RID: 4175 RVA: 0x0003E278 File Offset: 0x0003C478
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "({0},{1})", new object[]
			{
				this.CurrentRetryCount,
				this.LocationMode
			});
		}
	}
}
