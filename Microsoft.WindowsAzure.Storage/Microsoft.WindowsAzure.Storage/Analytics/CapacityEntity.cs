using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.WindowsAzure.Storage.Analytics
{
	// Token: 0x02000005 RID: 5
	public class CapacityEntity : TableEntity
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003A01 File Offset: 0x00001C01
		public DateTimeOffset Time
		{
			get
			{
				return DateTimeOffset.ParseExact(base.PartitionKey, "yyyyMMdd'T'HHmm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003A1A File Offset: 0x00001C1A
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00003A22 File Offset: 0x00001C22
		public long Capacity { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003A2B File Offset: 0x00001C2B
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003A33 File Offset: 0x00001C33
		public long ContainerCount { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003A3C File Offset: 0x00001C3C
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003A44 File Offset: 0x00001C44
		public long ObjectCount { get; set; }
	}
}
