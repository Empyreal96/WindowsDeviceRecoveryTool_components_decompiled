using System;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014C RID: 332
	public sealed class TableRequestOptions : IRequestOptions
	{
		// Token: 0x060014D0 RID: 5328 RVA: 0x0004F794 File Offset: 0x0004D994
		public TableRequestOptions()
		{
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004F7A8 File Offset: 0x0004D9A8
		public TableRequestOptions(TableRequestOptions other)
		{
			if (other != null)
			{
				this.ServerTimeout = other.ServerTimeout;
				this.RetryPolicy = other.RetryPolicy;
				this.LocationMode = other.LocationMode;
				this.MaximumExecutionTime = other.MaximumExecutionTime;
				this.OperationExpiryTime = other.OperationExpiryTime;
				this.PayloadFormat = other.PayloadFormat;
				this.PropertyResolver = other.PropertyResolver;
				this.EncryptionPolicy = other.EncryptionPolicy;
				this.RequireEncryption = other.RequireEncryption;
				this.EncryptionResolver = other.EncryptionResolver;
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004F844 File Offset: 0x0004DA44
		internal static TableRequestOptions ApplyDefaults(TableRequestOptions requestOptions, CloudTableClient serviceClient)
		{
			TableRequestOptions tableRequestOptions = new TableRequestOptions(requestOptions);
			tableRequestOptions.RetryPolicy = (tableRequestOptions.RetryPolicy ?? serviceClient.DefaultRequestOptions.RetryPolicy);
			TableRequestOptions tableRequestOptions2 = tableRequestOptions;
			LocationMode? locationMode = tableRequestOptions.LocationMode;
			tableRequestOptions2.LocationMode = new LocationMode?(((locationMode != null) ? new LocationMode?(locationMode.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.LocationMode) ?? Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			TableRequestOptions tableRequestOptions3 = tableRequestOptions;
			TimeSpan? serverTimeout = tableRequestOptions.ServerTimeout;
			tableRequestOptions3.ServerTimeout = ((serverTimeout != null) ? new TimeSpan?(serverTimeout.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.ServerTimeout);
			TableRequestOptions tableRequestOptions4 = tableRequestOptions;
			TimeSpan? timeSpan = tableRequestOptions.MaximumExecutionTime;
			tableRequestOptions4.MaximumExecutionTime = ((timeSpan != null) ? new TimeSpan?(timeSpan.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.MaximumExecutionTime);
			TableRequestOptions tableRequestOptions5 = tableRequestOptions;
			TablePayloadFormat? tablePayloadFormat = tableRequestOptions.PayloadFormat;
			tableRequestOptions5.PayloadFormat = new TablePayloadFormat?(((tablePayloadFormat != null) ? new TablePayloadFormat?(tablePayloadFormat.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.PayloadFormat) ?? TablePayloadFormat.Json);
			if (tableRequestOptions.OperationExpiryTime == null && tableRequestOptions.MaximumExecutionTime != null)
			{
				tableRequestOptions.OperationExpiryTime = new DateTime?(DateTime.Now + tableRequestOptions.MaximumExecutionTime.Value);
			}
			tableRequestOptions.PropertyResolver = (tableRequestOptions.PropertyResolver ?? serviceClient.DefaultRequestOptions.PropertyResolver);
			tableRequestOptions.EncryptionPolicy = (tableRequestOptions.EncryptionPolicy ?? serviceClient.DefaultRequestOptions.EncryptionPolicy);
			TableRequestOptions tableRequestOptions6 = tableRequestOptions;
			bool? requireEncryption = tableRequestOptions.RequireEncryption;
			tableRequestOptions6.RequireEncryption = ((requireEncryption != null) ? new bool?(requireEncryption.GetValueOrDefault()) : serviceClient.DefaultRequestOptions.RequireEncryption);
			tableRequestOptions.EncryptionResolver = (tableRequestOptions.EncryptionResolver ?? serviceClient.DefaultRequestOptions.EncryptionResolver);
			return tableRequestOptions;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004FA2C File Offset: 0x0004DC2C
		internal void ApplyToStorageCommand<T>(RESTCommand<T> cmd)
		{
			if (this.LocationMode != null)
			{
				cmd.LocationMode = this.LocationMode.Value;
			}
			this.ApplyToStorageCommandCommon<T>(cmd);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004FA64 File Offset: 0x0004DC64
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		internal void ApplyToStorageCommand<T, INTERMEDIATE_TYPE>(TableCommand<T, INTERMEDIATE_TYPE> cmd)
		{
			if (this.LocationMode != null && this.LocationMode.Value != Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly)
			{
				throw new InvalidOperationException("This operation can only be executed against the primary storage location.");
			}
			this.ApplyToStorageCommandCommon<T>(cmd);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004FAA4 File Offset: 0x0004DCA4
		private void ApplyToStorageCommandCommon<T>(StorageCommandBase<T> cmd)
		{
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

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004FB35 File Offset: 0x0004DD35
		internal void AssertNoEncryptionPolicyOrStrictMode()
		{
			if (this.EncryptionPolicy != null)
			{
				throw new InvalidOperationException("Encryption is not supported for the current operation. Please ensure that EncryptionPolicy is not set on RequestOptions.");
			}
			this.AssertPolicyIfRequired();
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004FB50 File Offset: 0x0004DD50
		internal void AssertPolicyIfRequired()
		{
			if (this.RequireEncryption != null && this.RequireEncryption.Value && this.EncryptionPolicy == null)
			{
				throw new InvalidOperationException("Encryption Policy is mandatory when RequireEncryption is set to true. If you do not want to encrypt/decrypt data, please set RequireEncryption to false in request options.");
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0004FB90 File Offset: 0x0004DD90
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x0004FB98 File Offset: 0x0004DD98
		internal DateTime? OperationExpiryTime { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0004FBA1 File Offset: 0x0004DDA1
		// (set) Token: 0x060014DB RID: 5339 RVA: 0x0004FBA9 File Offset: 0x0004DDA9
		public IRetryPolicy RetryPolicy { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0004FBB2 File Offset: 0x0004DDB2
		// (set) Token: 0x060014DD RID: 5341 RVA: 0x0004FBBA File Offset: 0x0004DDBA
		public TableEncryptionPolicy EncryptionPolicy { get; set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x0004FBC3 File Offset: 0x0004DDC3
		// (set) Token: 0x060014DF RID: 5343 RVA: 0x0004FBCB File Offset: 0x0004DDCB
		public bool? RequireEncryption { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x0004FBD4 File Offset: 0x0004DDD4
		// (set) Token: 0x060014E1 RID: 5345 RVA: 0x0004FBDC File Offset: 0x0004DDDC
		public LocationMode? LocationMode { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0004FBE5 File Offset: 0x0004DDE5
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0004FBED File Offset: 0x0004DDED
		public TimeSpan? ServerTimeout { get; set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0004FBF6 File Offset: 0x0004DDF6
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x0004FBFE File Offset: 0x0004DDFE
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

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0004FC2B File Offset: 0x0004DE2B
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0004FC33 File Offset: 0x0004DE33
		public TablePayloadFormat? PayloadFormat
		{
			get
			{
				return this.payloadFormat;
			}
			set
			{
				if (value != null)
				{
					this.payloadFormat = new TablePayloadFormat?(value.Value);
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0004FC50 File Offset: 0x0004DE50
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x0004FC58 File Offset: 0x0004DE58
		public Func<string, string, string, string, EdmType> PropertyResolver
		{
			get
			{
				return this.propertyResolver;
			}
			set
			{
				this.propertyResolver = value;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0004FC61 File Offset: 0x0004DE61
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x0004FC69 File Offset: 0x0004DE69
		public Func<string, string, string, bool> EncryptionResolver
		{
			get
			{
				return this.encryptionResolver;
			}
			set
			{
				this.encryptionResolver = value;
			}
		}

		// Token: 0x0400081E RID: 2078
		private TablePayloadFormat? payloadFormat = null;

		// Token: 0x0400081F RID: 2079
		private TimeSpan? maximumExecutionTime;

		// Token: 0x04000820 RID: 2080
		private Func<string, string, string, string, EdmType> propertyResolver;

		// Token: 0x04000821 RID: 2081
		private Func<string, string, string, bool> encryptionResolver;
	}
}
