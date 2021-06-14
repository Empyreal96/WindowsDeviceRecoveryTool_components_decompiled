using System;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000AF RID: 175
	public sealed class BlobRequestOptions : IRequestOptions
	{
		// Token: 0x060010A5 RID: 4261 RVA: 0x0003E945 File Offset: 0x0003CB45
		public BlobRequestOptions()
		{
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0003E950 File Offset: 0x0003CB50
		internal BlobRequestOptions(BlobRequestOptions other)
		{
			if (other != null)
			{
				this.RetryPolicy = other.RetryPolicy;
				this.AbsorbConditionalErrorsOnRetry = other.AbsorbConditionalErrorsOnRetry;
				this.EncryptionPolicy = other.EncryptionPolicy;
				this.RequireEncryption = other.RequireEncryption;
				this.SkipEncryptionPolicyValidation = other.SkipEncryptionPolicyValidation;
				this.LocationMode = other.LocationMode;
				this.ServerTimeout = other.ServerTimeout;
				this.MaximumExecutionTime = other.MaximumExecutionTime;
				this.OperationExpiryTime = other.OperationExpiryTime;
				this.UseTransactionalMD5 = other.UseTransactionalMD5;
				this.StoreBlobContentMD5 = other.StoreBlobContentMD5;
				this.DisableContentMD5Validation = other.DisableContentMD5Validation;
				this.ParallelOperationThreadCount = other.ParallelOperationThreadCount;
				this.SingleBlobUploadThresholdInBytes = other.SingleBlobUploadThresholdInBytes;
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0003EA14 File Offset: 0x0003CC14
		internal static BlobRequestOptions ApplyDefaults(BlobRequestOptions options, BlobType blobType, CloudBlobClient serviceClient, bool applyExpiry = true)
		{
			BlobRequestOptions blobRequestOptions = new BlobRequestOptions(options);
			blobRequestOptions.RetryPolicy = (blobRequestOptions.RetryPolicy ?? serviceClient.DefaultRequestOptions.RetryPolicy);
			blobRequestOptions.AbsorbConditionalErrorsOnRetry = new bool?(blobRequestOptions.AbsorbConditionalErrorsOnRetry ?? (serviceClient.DefaultRequestOptions.AbsorbConditionalErrorsOnRetry ?? false));
			blobRequestOptions.EncryptionPolicy = (blobRequestOptions.EncryptionPolicy ?? serviceClient.DefaultRequestOptions.EncryptionPolicy);
			BlobRequestOptions blobRequestOptions2 = blobRequestOptions;
			bool? requireEncryption = blobRequestOptions.RequireEncryption;
			blobRequestOptions2.RequireEncryption = ((requireEncryption != null) ? new bool?(requireEncryption.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.RequireEncryption);
			BlobRequestOptions blobRequestOptions3 = blobRequestOptions;
			LocationMode? locationMode = blobRequestOptions.LocationMode;
			blobRequestOptions3.LocationMode = new LocationMode?(((locationMode != null) ? new LocationMode?(locationMode.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.LocationMode) ?? Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			BlobRequestOptions blobRequestOptions4 = blobRequestOptions;
			TimeSpan? serverTimeout = blobRequestOptions.ServerTimeout;
			blobRequestOptions4.ServerTimeout = ((serverTimeout != null) ? new TimeSpan?(serverTimeout.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ServerTimeout);
			BlobRequestOptions blobRequestOptions5 = blobRequestOptions;
			TimeSpan? timeSpan = blobRequestOptions.MaximumExecutionTime;
			blobRequestOptions5.MaximumExecutionTime = ((timeSpan != null) ? new TimeSpan?(timeSpan.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.MaximumExecutionTime);
			BlobRequestOptions blobRequestOptions6 = blobRequestOptions;
			int? num = blobRequestOptions.ParallelOperationThreadCount;
			blobRequestOptions6.ParallelOperationThreadCount = new int?(((num != null) ? new int?(num.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ParallelOperationThreadCount) ?? 1);
			BlobRequestOptions blobRequestOptions7 = blobRequestOptions;
			long? num2 = blobRequestOptions.SingleBlobUploadThresholdInBytes;
			blobRequestOptions7.SingleBlobUploadThresholdInBytes = new long?(((num2 != null) ? new long?(num2.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.SingleBlobUploadThresholdInBytes) ?? 33554432L);
			if (applyExpiry && blobRequestOptions.OperationExpiryTime == null && blobRequestOptions.MaximumExecutionTime != null)
			{
				blobRequestOptions.OperationExpiryTime = new DateTime?(DateTime.Now + blobRequestOptions.MaximumExecutionTime.Value);
			}
			BlobRequestOptions blobRequestOptions8 = blobRequestOptions;
			bool? flag = blobRequestOptions.DisableContentMD5Validation;
			blobRequestOptions8.DisableContentMD5Validation = new bool?(((flag != null) ? new bool?(flag.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.DisableContentMD5Validation) ?? false);
			BlobRequestOptions blobRequestOptions9 = blobRequestOptions;
			bool? flag2 = blobRequestOptions.StoreBlobContentMD5;
			blobRequestOptions9.StoreBlobContentMD5 = new bool?(((flag2 != null) ? new bool?(flag2.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.StoreBlobContentMD5) ?? (blobType == BlobType.BlockBlob));
			BlobRequestOptions blobRequestOptions10 = blobRequestOptions;
			bool? flag3 = blobRequestOptions.UseTransactionalMD5;
			blobRequestOptions10.UseTransactionalMD5 = new bool?(((flag3 != null) ? new bool?(flag3.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.UseTransactionalMD5) ?? false);
			return blobRequestOptions;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0003ED40 File Offset: 0x0003CF40
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

		// Token: 0x060010A9 RID: 4265 RVA: 0x0003EDF7 File Offset: 0x0003CFF7
		internal void AssertNoEncryptionPolicyOrStrictMode()
		{
			if (this.EncryptionPolicy != null && !this.SkipEncryptionPolicyValidation)
			{
				throw new InvalidOperationException("Encryption is not supported for the current operation. Please ensure that EncryptionPolicy is not set on RequestOptions.");
			}
			this.AssertPolicyIfRequired();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0003EE1C File Offset: 0x0003D01C
		internal void AssertPolicyIfRequired()
		{
			if (this.RequireEncryption != null && this.RequireEncryption.Value && this.EncryptionPolicy == null)
			{
				throw new InvalidOperationException("Encryption Policy is mandatory when RequireEncryption is set to true. If you do not want to encrypt/decrypt data, please set RequireEncryption to false in request options.");
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x0003EE5C File Offset: 0x0003D05C
		// (set) Token: 0x060010AC RID: 4268 RVA: 0x0003EE64 File Offset: 0x0003D064
		internal DateTime? OperationExpiryTime { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x0003EE6D File Offset: 0x0003D06D
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x0003EE75 File Offset: 0x0003D075
		public IRetryPolicy RetryPolicy { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0003EE7E File Offset: 0x0003D07E
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x0003EE86 File Offset: 0x0003D086
		public BlobEncryptionPolicy EncryptionPolicy { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x0003EE8F File Offset: 0x0003D08F
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x0003EE97 File Offset: 0x0003D097
		public bool? RequireEncryption { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0003EEA0 File Offset: 0x0003D0A0
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x0003EEA8 File Offset: 0x0003D0A8
		internal bool SkipEncryptionPolicyValidation { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0003EEB1 File Offset: 0x0003D0B1
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x0003EEB9 File Offset: 0x0003D0B9
		public bool? AbsorbConditionalErrorsOnRetry { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0003EEC2 File Offset: 0x0003D0C2
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x0003EECA File Offset: 0x0003D0CA
		public LocationMode? LocationMode { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0003EED3 File Offset: 0x0003D0D3
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x0003EEDB File Offset: 0x0003D0DB
		public TimeSpan? ServerTimeout { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0003EEE4 File Offset: 0x0003D0E4
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x0003EEEC File Offset: 0x0003D0EC
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0003EF19 File Offset: 0x0003D119
		// (set) Token: 0x060010BE RID: 4286 RVA: 0x0003EF21 File Offset: 0x0003D121
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x0003EF47 File Offset: 0x0003D147
		// (set) Token: 0x060010C0 RID: 4288 RVA: 0x0003EF4F File Offset: 0x0003D14F
		public long? SingleBlobUploadThresholdInBytes
		{
			get
			{
				return this.singleBlobUploadThresholdInBytes;
			}
			set
			{
				if (value != null)
				{
					CommonUtility.AssertInBounds<long>("SingleBlobUploadThresholdInBytes", value.Value, 1048576L, 67108864L);
				}
				this.singleBlobUploadThresholdInBytes = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x0003EF7E File Offset: 0x0003D17E
		// (set) Token: 0x060010C2 RID: 4290 RVA: 0x0003EF86 File Offset: 0x0003D186
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0003EF8F File Offset: 0x0003D18F
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x0003EF97 File Offset: 0x0003D197
		public bool? StoreBlobContentMD5
		{
			get
			{
				return this.storeBlobContentMD5;
			}
			set
			{
				this.storeBlobContentMD5 = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0003EFA0 File Offset: 0x0003D1A0
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x0003EFA8 File Offset: 0x0003D1A8
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

		// Token: 0x0400040A RID: 1034
		private int? parallelOperationThreadCount;

		// Token: 0x0400040B RID: 1035
		private long? singleBlobUploadThresholdInBytes;

		// Token: 0x0400040C RID: 1036
		private TimeSpan? maximumExecutionTime;

		// Token: 0x0400040D RID: 1037
		private bool? useTransactionalMD5;

		// Token: 0x0400040E RID: 1038
		private bool? storeBlobContentMD5;

		// Token: 0x0400040F RID: 1039
		private bool? disableContentMD5Validation;
	}
}
