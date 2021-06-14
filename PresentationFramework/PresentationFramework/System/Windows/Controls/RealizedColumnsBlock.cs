using System;

namespace System.Windows.Controls
{
	// Token: 0x0200051F RID: 1311
	internal struct RealizedColumnsBlock
	{
		// Token: 0x060054B2 RID: 21682 RVA: 0x001773EA File Offset: 0x001755EA
		public RealizedColumnsBlock(int startIndex, int endIndex, int startIndexOffset)
		{
			this = default(RealizedColumnsBlock);
			this.StartIndex = startIndex;
			this.EndIndex = endIndex;
			this.StartIndexOffset = startIndexOffset;
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x00177408 File Offset: 0x00175608
		// (set) Token: 0x060054B4 RID: 21684 RVA: 0x00177410 File Offset: 0x00175610
		public int StartIndex { get; private set; }

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x060054B5 RID: 21685 RVA: 0x00177419 File Offset: 0x00175619
		// (set) Token: 0x060054B6 RID: 21686 RVA: 0x00177421 File Offset: 0x00175621
		public int EndIndex { get; private set; }

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x060054B7 RID: 21687 RVA: 0x0017742A File Offset: 0x0017562A
		// (set) Token: 0x060054B8 RID: 21688 RVA: 0x00177432 File Offset: 0x00175632
		public int StartIndexOffset { get; private set; }
	}
}
