using System;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000077 RID: 119
	public interface IRequestOptions
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000E85 RID: 3717
		// (set) Token: 0x06000E86 RID: 3718
		IRetryPolicy RetryPolicy { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000E87 RID: 3719
		// (set) Token: 0x06000E88 RID: 3720
		LocationMode? LocationMode { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000E89 RID: 3721
		// (set) Token: 0x06000E8A RID: 3722
		TimeSpan? ServerTimeout { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000E8B RID: 3723
		// (set) Token: 0x06000E8C RID: 3724
		TimeSpan? MaximumExecutionTime { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000E8D RID: 3725
		// (set) Token: 0x06000E8E RID: 3726
		bool? RequireEncryption { get; set; }
	}
}
