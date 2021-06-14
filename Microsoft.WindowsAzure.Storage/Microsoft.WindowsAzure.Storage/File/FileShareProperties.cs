using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000DD RID: 221
	public sealed class FileShareProperties
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00042390 File Offset: 0x00040590
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x00042398 File Offset: 0x00040598
		public string ETag { get; internal set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x000423A1 File Offset: 0x000405A1
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x000423A9 File Offset: 0x000405A9
		public DateTimeOffset? LastModified { get; internal set; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x000423B2 File Offset: 0x000405B2
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x000423BA File Offset: 0x000405BA
		public int? Quota
		{
			get
			{
				return this.quota;
			}
			set
			{
				if (value != null)
				{
					CommonUtility.AssertInBounds<int>("Quota", value.Value, 1);
				}
				this.quota = value;
			}
		}

		// Token: 0x040004DA RID: 1242
		private int? quota;
	}
}
