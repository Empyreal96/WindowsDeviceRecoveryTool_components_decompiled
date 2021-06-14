using System;
using System.IO;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.Interfaces
{
	// Token: 0x02000042 RID: 66
	public interface IItem : ICloneable
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600027D RID: 637
		string OldName { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600027E RID: 638
		// (set) Token: 0x0600027F RID: 639
		string Name { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000280 RID: 640
		// (set) Token: 0x06000281 RID: 641
		string SrcFileName { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000282 RID: 642
		// (set) Token: 0x06000283 RID: 643
		ProcessOperation Operation { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000284 RID: 644
		// (set) Token: 0x06000285 RID: 645
		Stream Stream { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000286 RID: 646
		// (set) Token: 0x06000287 RID: 647
		bool IsModified { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000288 RID: 648
		// (set) Token: 0x06000289 RID: 649
		bool IsTagged { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600028A RID: 650
		// (set) Token: 0x0600028B RID: 651
		int StreamPosition { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600028C RID: 652
		// (set) Token: 0x0600028D RID: 653
		bool NeedDestroyStream { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600028E RID: 654
		// (set) Token: 0x0600028F RID: 655
		long UncompressedSize { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000290 RID: 656
		// (set) Token: 0x06000291 RID: 657
		long RelativeLocalHeaderOffset { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000292 RID: 658
		// (set) Token: 0x06000293 RID: 659
		DateTime LastFileModificationTime { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000294 RID: 660
		// (set) Token: 0x06000295 RID: 661
		FileAttributes ExternalAttributes { get; set; }

		// Token: 0x06000296 RID: 662
		void WriteLocalHeaderToStream(Stream stream, int offset);

		// Token: 0x06000297 RID: 663
		int GetDataOffset();

		// Token: 0x06000298 RID: 664
		void Reset();

		// Token: 0x06000299 RID: 665
		void GetArchiveItem(ref BaseArchiveItem archiveItem);

		// Token: 0x0600029A RID: 666
		void CopyFrom(BaseArchiveItem archiveItem);

		// Token: 0x0600029B RID: 667
		bool IsDirectory();

		// Token: 0x0600029C RID: 668
		int GetLocalHeaderSize();

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600029D RID: 669
		// (remove) Token: 0x0600029E RID: 670
		event EventHandler ItemNameChanged;
	}
}
