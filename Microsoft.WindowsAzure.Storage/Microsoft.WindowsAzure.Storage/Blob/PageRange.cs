using System;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000BE RID: 190
	public sealed class PageRange
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x0003F0DB File Offset: 0x0003D2DB
		public PageRange(long start, long end)
		{
			this.StartOffset = start;
			this.EndOffset = end;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x0003F0F1 File Offset: 0x0003D2F1
		// (set) Token: 0x060010E9 RID: 4329 RVA: 0x0003F0F9 File Offset: 0x0003D2F9
		public long StartOffset { get; internal set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x0003F102 File Offset: 0x0003D302
		// (set) Token: 0x060010EB RID: 4331 RVA: 0x0003F10A File Offset: 0x0003D30A
		public long EndOffset { get; internal set; }

		// Token: 0x060010EC RID: 4332 RVA: 0x0003F114 File Offset: 0x0003D314
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "bytes={0}-{1}", new object[]
			{
				this.StartOffset,
				this.EndOffset
			});
		}
	}
}
