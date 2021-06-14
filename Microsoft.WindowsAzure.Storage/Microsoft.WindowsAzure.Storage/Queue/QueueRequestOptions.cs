using System;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000F6 RID: 246
	public sealed class QueueRequestOptions : IRequestOptions
	{
		// Token: 0x0600123B RID: 4667 RVA: 0x00043DA9 File Offset: 0x00041FA9
		public QueueRequestOptions()
		{
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00043DB4 File Offset: 0x00041FB4
		internal QueueRequestOptions(QueueRequestOptions other) : this()
		{
			if (other != null)
			{
				this.RetryPolicy = other.RetryPolicy;
				this.EncryptionPolicy = other.EncryptionPolicy;
				this.RequireEncryption = other.RequireEncryption;
				this.ServerTimeout = other.ServerTimeout;
				this.LocationMode = other.LocationMode;
				this.MaximumExecutionTime = other.MaximumExecutionTime;
				this.OperationExpiryTime = other.OperationExpiryTime;
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00043E20 File Offset: 0x00042020
		internal static QueueRequestOptions ApplyDefaults(QueueRequestOptions options, CloudQueueClient serviceClient)
		{
			QueueRequestOptions queueRequestOptions = new QueueRequestOptions(options);
			queueRequestOptions.RetryPolicy = (queueRequestOptions.RetryPolicy ?? serviceClient.DefaultRequestOptions.RetryPolicy);
			queueRequestOptions.EncryptionPolicy = (queueRequestOptions.EncryptionPolicy ?? serviceClient.DefaultRequestOptions.EncryptionPolicy);
			QueueRequestOptions queueRequestOptions2 = queueRequestOptions;
			bool? requireEncryption = queueRequestOptions.RequireEncryption;
			queueRequestOptions2.RequireEncryption = ((requireEncryption != null) ? new bool?(requireEncryption.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.RequireEncryption);
			QueueRequestOptions queueRequestOptions3 = queueRequestOptions;
			LocationMode? locationMode = queueRequestOptions.LocationMode;
			queueRequestOptions3.LocationMode = new LocationMode?(((locationMode != null) ? new LocationMode?(locationMode.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.LocationMode) ?? Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			QueueRequestOptions queueRequestOptions4 = queueRequestOptions;
			TimeSpan? serverTimeout = queueRequestOptions.ServerTimeout;
			queueRequestOptions4.ServerTimeout = ((serverTimeout != null) ? new TimeSpan?(serverTimeout.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ServerTimeout);
			QueueRequestOptions queueRequestOptions5 = queueRequestOptions;
			TimeSpan? timeSpan = queueRequestOptions.MaximumExecutionTime;
			queueRequestOptions5.MaximumExecutionTime = ((timeSpan != null) ? new TimeSpan?(timeSpan.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.MaximumExecutionTime);
			if (queueRequestOptions.OperationExpiryTime == null && queueRequestOptions.MaximumExecutionTime != null)
			{
				queueRequestOptions.OperationExpiryTime = new DateTime?(DateTime.Now + queueRequestOptions.MaximumExecutionTime.Value);
			}
			return queueRequestOptions;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00043F88 File Offset: 0x00042188
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

		// Token: 0x0600123F RID: 4671 RVA: 0x00044040 File Offset: 0x00042240
		internal void AssertPolicyIfRequired()
		{
			if (this.RequireEncryption != null && this.RequireEncryption.Value && this.EncryptionPolicy == null)
			{
				throw new InvalidOperationException("Encryption Policy is mandatory when RequireEncryption is set to true. If you do not want to encrypt/decrypt data, please set RequireEncryption to false in request options.");
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x00044080 File Offset: 0x00042280
		// (set) Token: 0x06001241 RID: 4673 RVA: 0x00044088 File Offset: 0x00042288
		internal DateTime? OperationExpiryTime { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x00044091 File Offset: 0x00042291
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x00044099 File Offset: 0x00042299
		public IRetryPolicy RetryPolicy { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x000440A2 File Offset: 0x000422A2
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x000440AA File Offset: 0x000422AA
		public QueueEncryptionPolicy EncryptionPolicy { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x000440B3 File Offset: 0x000422B3
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x000440BB File Offset: 0x000422BB
		public bool? RequireEncryption { get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x000440C4 File Offset: 0x000422C4
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x000440CC File Offset: 0x000422CC
		public LocationMode? LocationMode { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x000440D5 File Offset: 0x000422D5
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x000440DD File Offset: 0x000422DD
		public TimeSpan? ServerTimeout { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x000440E6 File Offset: 0x000422E6
		// (set) Token: 0x0600124D RID: 4685 RVA: 0x000440EE File Offset: 0x000422EE
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

		// Token: 0x0400052E RID: 1326
		private TimeSpan? maximumExecutionTime;
	}
}
