using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Auth
{
	// Token: 0x02000084 RID: 132
	public sealed class StorageCredentials
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00039C73 File Offset: 0x00037E73
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x00039C7B File Offset: 0x00037E7B
		public string SASToken { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00039C84 File Offset: 0x00037E84
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00039C8C File Offset: 0x00037E8C
		public string AccountName { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00039C95 File Offset: 0x00037E95
		public string KeyName
		{
			get
			{
				return this.Key.KeyName;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x00039CA2 File Offset: 0x00037EA2
		// (set) Token: 0x06000F23 RID: 3875 RVA: 0x00039CAA File Offset: 0x00037EAA
		internal StorageAccountKey Key { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00039CB3 File Offset: 0x00037EB3
		public bool IsAnonymous
		{
			get
			{
				return this.SASToken == null && this.AccountName == null;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00039CC8 File Offset: 0x00037EC8
		public bool IsSAS
		{
			get
			{
				return this.SASToken != null && this.AccountName == null;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00039CDD File Offset: 0x00037EDD
		public bool IsSharedKey
		{
			get
			{
				return this.SASToken == null && this.AccountName != null;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x00039CF5 File Offset: 0x00037EF5
		public string SASSignature
		{
			get
			{
				if (this.IsSAS)
				{
					return this.queryBuilder["sig"];
				}
				return null;
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00039D11 File Offset: 0x00037F11
		public StorageCredentials()
		{
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00039D19 File Offset: 0x00037F19
		public StorageCredentials(string accountName, string keyValue) : this(accountName, keyValue, null)
		{
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00039D24 File Offset: 0x00037F24
		public StorageCredentials(string accountName, byte[] keyValue) : this(accountName, keyValue, null)
		{
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00039D2F File Offset: 0x00037F2F
		public StorageCredentials(string accountName, string keyValue, string keyName)
		{
			CommonUtility.AssertNotNullOrEmpty("accountName", accountName);
			this.AccountName = accountName;
			this.UpdateKey(keyValue, keyName);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00039D51 File Offset: 0x00037F51
		public StorageCredentials(string accountName, byte[] keyValue, string keyName)
		{
			CommonUtility.AssertNotNullOrEmpty("accountName", accountName);
			this.AccountName = accountName;
			this.UpdateKey(keyValue, keyName);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00039D73 File Offset: 0x00037F73
		public StorageCredentials(string sasToken)
		{
			CommonUtility.AssertNotNullOrEmpty("sasToken", sasToken);
			this.SASToken = sasToken;
			this.UpdateQueryBuilder();
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00039D93 File Offset: 0x00037F93
		public void UpdateKey(string keyValue)
		{
			this.UpdateKey(keyValue, null);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00039D9D File Offset: 0x00037F9D
		public void UpdateKey(byte[] keyValue)
		{
			this.UpdateKey(keyValue, null);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00039DA8 File Offset: 0x00037FA8
		public void UpdateKey(string keyValue, string keyName)
		{
			if (!this.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot update key unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			CommonUtility.AssertNotNull("keyValue", keyValue);
			this.Key = new StorageAccountKey(keyName, Convert.FromBase64String(keyValue));
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00039DF8 File Offset: 0x00037FF8
		public void UpdateKey(byte[] keyValue, string keyName)
		{
			if (!this.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot update key unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			CommonUtility.AssertNotNull("keyValue", keyValue);
			this.Key = new StorageAccountKey(keyName, keyValue);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00039E44 File Offset: 0x00038044
		public void UpdateSASToken(string sasToken)
		{
			if (!this.IsSAS)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot update Shared Access Signature unless Sas credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			CommonUtility.AssertNotNullOrEmpty("sasToken", sasToken);
			this.SASToken = sasToken;
			this.UpdateQueryBuilder();
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00039E8E File Offset: 0x0003808E
		public byte[] ExportKey()
		{
			return (byte[])this.Key.KeyValue.Clone();
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00039EA5 File Offset: 0x000380A5
		public Uri TransformUri(Uri resourceUri)
		{
			if (this.IsSAS)
			{
				return this.queryBuilder.AddToUri(resourceUri);
			}
			return resourceUri;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00039EBD File Offset: 0x000380BD
		public StorageUri TransformUri(StorageUri resourceUri)
		{
			CommonUtility.AssertNotNull("resourceUri", resourceUri);
			return new StorageUri(this.TransformUri(resourceUri.PrimaryUri), this.TransformUri(resourceUri.SecondaryUri));
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00039EE8 File Offset: 0x000380E8
		public string ExportBase64EncodedKey()
		{
			StorageAccountKey key = this.Key;
			if (key.KeyValue != null)
			{
				return Convert.ToBase64String(key.KeyValue);
			}
			return null;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00039F14 File Offset: 0x00038114
		internal string ToString(bool exportSecrets)
		{
			if (this.IsSharedKey)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}={1};{2}={3}", new object[]
				{
					"AccountName",
					this.AccountName,
					"AccountKey",
					exportSecrets ? this.ExportBase64EncodedKey() : "[key hidden]"
				});
			}
			if (this.IsSAS)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}={1}", new object[]
				{
					"SharedAccessSignature",
					exportSecrets ? this.SASToken : "[signature hidden]"
				});
			}
			return string.Empty;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00039FB0 File Offset: 0x000381B0
		public bool Equals(StorageCredentials other)
		{
			return other != null && (string.Equals(this.SASToken, other.SASToken) && string.Equals(this.AccountName, other.AccountName) && string.Equals(this.KeyName, other.KeyName)) && string.Equals(this.ExportBase64EncodedKey(), other.ExportBase64EncodedKey());
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0003A010 File Offset: 0x00038210
		private void UpdateQueryBuilder()
		{
			this.queryBuilder = new UriQueryBuilder();
			IDictionary<string, string> dictionary = HttpWebUtility.ParseQueryString(this.SASToken);
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				this.queryBuilder.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.queryBuilder.Add("api-version", "2015-02-21");
		}

		// Token: 0x0400027C RID: 636
		private UriQueryBuilder queryBuilder;
	}
}
