using System;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000DA RID: 218
	public sealed class FileRequestOptions : IRequestOptions
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x00041E40 File Offset: 0x00040040
		public FileRequestOptions()
		{
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00041E48 File Offset: 0x00040048
		internal FileRequestOptions(FileRequestOptions other) : this()
		{
			if (other != null)
			{
				this.RetryPolicy = other.RetryPolicy;
				this.RequireEncryption = other.RequireEncryption;
				this.LocationMode = other.LocationMode;
				this.ServerTimeout = other.ServerTimeout;
				this.MaximumExecutionTime = other.MaximumExecutionTime;
				this.OperationExpiryTime = other.OperationExpiryTime;
				this.UseTransactionalMD5 = other.UseTransactionalMD5;
				this.StoreFileContentMD5 = other.StoreFileContentMD5;
				this.DisableContentMD5Validation = other.DisableContentMD5Validation;
				this.ParallelOperationThreadCount = other.ParallelOperationThreadCount;
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00041ED8 File Offset: 0x000400D8
		internal static FileRequestOptions ApplyDefaults(FileRequestOptions options, CloudFileClient serviceClient, bool applyExpiry = true)
		{
			FileRequestOptions fileRequestOptions = new FileRequestOptions(options);
			fileRequestOptions.RetryPolicy = (fileRequestOptions.RetryPolicy ?? serviceClient.DefaultRequestOptions.RetryPolicy);
			FileRequestOptions fileRequestOptions2 = fileRequestOptions;
			LocationMode? locationMode = fileRequestOptions.LocationMode;
			fileRequestOptions2.LocationMode = new LocationMode?(((locationMode != null) ? new LocationMode?(locationMode.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.LocationMode) ?? Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			FileRequestOptions fileRequestOptions3 = fileRequestOptions;
			bool? requireEncryption = fileRequestOptions.RequireEncryption;
			fileRequestOptions3.RequireEncryption = ((requireEncryption != null) ? new bool?(requireEncryption.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.RequireEncryption);
			FileRequestOptions fileRequestOptions4 = fileRequestOptions;
			TimeSpan? serverTimeout = fileRequestOptions.ServerTimeout;
			fileRequestOptions4.ServerTimeout = ((serverTimeout != null) ? new TimeSpan?(serverTimeout.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ServerTimeout);
			FileRequestOptions fileRequestOptions5 = fileRequestOptions;
			TimeSpan? timeSpan = fileRequestOptions.MaximumExecutionTime;
			fileRequestOptions5.MaximumExecutionTime = ((timeSpan != null) ? new TimeSpan?(timeSpan.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.MaximumExecutionTime);
			FileRequestOptions fileRequestOptions6 = fileRequestOptions;
			int? num = fileRequestOptions.ParallelOperationThreadCount;
			fileRequestOptions6.ParallelOperationThreadCount = new int?(((num != null) ? new int?(num.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ParallelOperationThreadCount) ?? 1);
			if (applyExpiry && fileRequestOptions.OperationExpiryTime == null && fileRequestOptions.MaximumExecutionTime != null)
			{
				fileRequestOptions.OperationExpiryTime = new DateTime?(DateTime.Now + fileRequestOptions.MaximumExecutionTime.Value);
			}
			FileRequestOptions fileRequestOptions7 = fileRequestOptions;
			bool? flag = fileRequestOptions.DisableContentMD5Validation;
			fileRequestOptions7.DisableContentMD5Validation = new bool?(((flag != null) ? new bool?(flag.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.DisableContentMD5Validation) ?? false);
			FileRequestOptions fileRequestOptions8 = fileRequestOptions;
			bool? flag2 = fileRequestOptions.StoreFileContentMD5;
			fileRequestOptions8.StoreFileContentMD5 = new bool?(((flag2 != null) ? new bool?(flag2.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.StoreFileContentMD5) ?? false);
			FileRequestOptions fileRequestOptions9 = fileRequestOptions;
			bool? flag3 = fileRequestOptions.UseTransactionalMD5;
			fileRequestOptions9.UseTransactionalMD5 = new bool?(((flag3 != null) ? new bool?(flag3.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.UseTransactionalMD5) ?? false);
			return fileRequestOptions;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00042150 File Offset: 0x00040350
		internal void ApplyToStorageCommand<T>(RESTCommand<T> cmd)
		{
			if (this.LocationMode != null)
			{
				cmd.LocationMode = this.LocationMode.Value;
			}
			if (this.ServerTimeout != null)
			{
				cmd.ServerTimeoutInSeconds = new int?((int)this.ServerTimeout.Value.TotalSeconds);
			}
			if (this.OperationExpiryTime != null)
			{
				cmd.OperationExpiryTime = this.OperationExpiryTime;
				return;
			}
			if (this.MaximumExecutionTime != null)
			{
				cmd.OperationExpiryTime = new DateTime?(DateTime.Now + this.MaximumExecutionTime.Value);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x00042207 File Offset: 0x00040407
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x0004220F File Offset: 0x0004040F
		internal DateTime? OperationExpiryTime { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x00042218 File Offset: 0x00040418
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x00042220 File Offset: 0x00040420
		public IRetryPolicy RetryPolicy { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00042229 File Offset: 0x00040429
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x00042234 File Offset: 0x00040434
		public LocationMode? LocationMode
		{
			get
			{
				return new LocationMode?(Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			}
			set
			{
				if (value != Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly)
				{
					throw new NotSupportedException("This operation can only be executed against the primary storage location.");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00042266 File Offset: 0x00040466
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0004226E File Offset: 0x0004046E
		public bool? RequireEncryption
		{
			get
			{
				return new bool?(false);
			}
			set
			{
				if (value != null && value.Value)
				{
					throw new NotSupportedException("Encryption is not supported for files.");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x0004228D File Offset: 0x0004048D
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x00042295 File Offset: 0x00040495
		public TimeSpan? ServerTimeout { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x0004229E File Offset: 0x0004049E
		// (set) Token: 0x060011AC RID: 4524 RVA: 0x000422A6 File Offset: 0x000404A6
		public TimeSpan? MaximumExecutionTime
		{
			get
			{
				return this.maximumExecutionTime;
			}
			set
			{
				if (value != null)
				{
					CommonUtility.AssertInBounds<TimeSpan>("MaximumExecutionTime", value.Value, TimeSpan.Zero, Constants.MaxMaximumExecutionTime);
				}
				this.maximumExecutionTime = value;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x000422D3 File Offset: 0x000404D3
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x000422DB File Offset: 0x000404DB
		public int? ParallelOperationThreadCount
		{
			get
			{
				return this.parallelOperationThreadCount;
			}
			set
			{
				if (value != null)
				{
					CommonUtility.AssertInBounds<int>("ParallelOperationThreadCount", value.Value, 1, 64);
				}
				this.parallelOperationThreadCount = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x00042301 File Offset: 0x00040501
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x00042309 File Offset: 0x00040509
		public bool? UseTransactionalMD5
		{
			get
			{
				return this.useTransactionalMD5;
			}
			set
			{
				this.useTransactionalMD5 = value;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00042312 File Offset: 0x00040512
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x0004231A File Offset: 0x0004051A
		public bool? StoreFileContentMD5
		{
			get
			{
				return this.storeFileContentMD5;
			}
			set
			{
				this.storeFileContentMD5 = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00042323 File Offset: 0x00040523
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x0004232B File Offset: 0x0004052B
		public bool? DisableContentMD5Validation
		{
			get
			{
				return this.disableContentMD5Validation;
			}
			set
			{
				this.disableContentMD5Validation = value;
			}
		}

		// Token: 0x040004CF RID: 1231
		private int? parallelOperationThreadCount;

		// Token: 0x040004D0 RID: 1232
		private TimeSpan? maximumExecutionTime;

		// Token: 0x040004D1 RID: 1233
		private bool? useTransactionalMD5;

		// Token: 0x040004D2 RID: 1234
		private bool? storeFileContentMD5;

		// Token: 0x040004D3 RID: 1235
		private bool? disableContentMD5Validation;
	}
}
