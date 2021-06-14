using System;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000D9 RID: 217
	public sealed class FileRange
	{
		// Token: 0x06001197 RID: 4503 RVA: 0x00041DC7 File Offset: 0x0003FFC7
		public FileRange(long start, long end)
		{
			this.StartOffset = start;
			this.EndOffset = end;
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00041DDD File Offset: 0x0003FFDD
		// (set) Token: 0x06001199 RID: 4505 RVA: 0x00041DE5 File Offset: 0x0003FFE5
		public long StartOffset { get; internal set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00041DEE File Offset: 0x0003FFEE
		// (set) Token: 0x0600119B RID: 4507 RVA: 0x00041DF6 File Offset: 0x0003FFF6
		public long EndOffset { get; internal set; }

		// Token: 0x0600119C RID: 4508 RVA: 0x00041E00 File Offset: 0x00040000
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
