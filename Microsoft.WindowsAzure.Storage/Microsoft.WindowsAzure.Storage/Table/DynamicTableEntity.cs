using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200013A RID: 314
	public sealed class DynamicTableEntity : ITableEntity
	{
		// Token: 0x0600143F RID: 5183 RVA: 0x0004DFBD File Offset: 0x0004C1BD
		public DynamicTableEntity()
		{
			this.Properties = new Dictionary<string, EntityProperty>();
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0004DFD0 File Offset: 0x0004C1D0
		public DynamicTableEntity(string partitionKey, string rowKey) : this(partitionKey, rowKey, DateTimeOffset.MinValue, null, new Dictionary<string, EntityProperty>())
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0004DFE5 File Offset: 0x0004C1E5
		public DynamicTableEntity(string partitionKey, string rowKey, string etag, IDictionary<string, EntityProperty> properties) : this(partitionKey, rowKey, DateTimeOffset.MinValue, etag, properties)
		{
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004DFF8 File Offset: 0x0004C1F8
		internal DynamicTableEntity(string partitionKey, string rowKey, DateTimeOffset timestamp, string etag, IDictionary<string, EntityProperty> properties)
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowKey", rowKey);
			CommonUtility.AssertNotNull("properties", properties);
			this.PartitionKey = partitionKey;
			this.RowKey = rowKey;
			this.Timestamp = timestamp;
			this.ETag = etag;
			this.Properties = properties;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0004E052 File Offset: 0x0004C252
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x0004E05A File Offset: 0x0004C25A
		public IDictionary<string, EntityProperty> Properties { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0004E063 File Offset: 0x0004C263
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x0004E06B File Offset: 0x0004C26B
		public string PartitionKey { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0004E074 File Offset: 0x0004C274
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x0004E07C File Offset: 0x0004C27C
		public string RowKey { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0004E085 File Offset: 0x0004C285
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x0004E08D File Offset: 0x0004C28D
		public DateTimeOffset Timestamp { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0004E096 File Offset: 0x0004C296
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x0004E09E File Offset: 0x0004C29E
		public string ETag { get; set; }

		// Token: 0x1700032A RID: 810
		public EntityProperty this[string key]
		{
			get
			{
				return this.Properties[key];
			}
			set
			{
				this.Properties[key] = value;
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004E0C4 File Offset: 0x0004C2C4
		public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
		{
			this.Properties = properties;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004E0CD File Offset: 0x0004C2CD
		public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
		{
			return this.Properties;
		}
	}
}
