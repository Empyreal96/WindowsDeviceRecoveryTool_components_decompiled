using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000003 RID: 3
	public interface ITableEntity
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2
		// (set) Token: 0x06000003 RID: 3
		string PartitionKey { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4
		// (set) Token: 0x06000005 RID: 5
		string RowKey { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6
		// (set) Token: 0x06000007 RID: 7
		DateTimeOffset Timestamp { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8
		// (set) Token: 0x06000009 RID: 9
		string ETag { get; set; }

		// Token: 0x0600000A RID: 10
		void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext);

		// Token: 0x0600000B RID: 11
		IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext);
	}
}
