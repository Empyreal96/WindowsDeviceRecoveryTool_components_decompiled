using System;
using System.Data.Services.Common;

namespace Microsoft.WindowsAzure.Storage.Table.DataServices
{
	// Token: 0x02000046 RID: 70
	[DataServiceKey(new string[]
	{
		"PartitionKey",
		"RowKey"
	})]
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	public abstract class TableServiceEntity
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x0002CF56 File Offset: 0x0002B156
		protected TableServiceEntity(string partitionKey, string rowKey)
		{
			this.PartitionKey = partitionKey;
			this.RowKey = rowKey;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0002CF6C File Offset: 0x0002B16C
		protected TableServiceEntity()
		{
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0002CF74 File Offset: 0x0002B174
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x0002CF7C File Offset: 0x0002B17C
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public DateTime Timestamp { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0002CF85 File Offset: 0x0002B185
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0002CF8D File Offset: 0x0002B18D
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public virtual string PartitionKey { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0002CF96 File Offset: 0x0002B196
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0002CF9E File Offset: 0x0002B19E
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public virtual string RowKey { get; set; }
	}
}
