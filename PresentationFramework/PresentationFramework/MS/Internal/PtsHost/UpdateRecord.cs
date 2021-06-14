using System;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000653 RID: 1619
	internal sealed class UpdateRecord
	{
		// Token: 0x06006B7C RID: 27516 RVA: 0x001F0E80 File Offset: 0x001EF080
		internal UpdateRecord()
		{
			this.Dtr = new DirtyTextRange(0, 0, 0, false);
			this.FirstPara = (this.SyncPara = null);
			this.ChangeType = PTS.FSKCHANGE.fskchNone;
			this.Next = null;
			this.InProcessing = false;
		}

		// Token: 0x06006B7D RID: 27517 RVA: 0x001F0EC8 File Offset: 0x001EF0C8
		internal void MergeWithNext()
		{
			int num = this.Next.Dtr.StartIndex - this.Dtr.StartIndex;
			this.Dtr.PositionsAdded = this.Dtr.PositionsAdded + (num + this.Next.Dtr.PositionsAdded);
			this.Dtr.PositionsRemoved = this.Dtr.PositionsRemoved + (num + this.Next.Dtr.PositionsRemoved);
			this.SyncPara = this.Next.SyncPara;
			this.Next = this.Next.Next;
		}

		// Token: 0x0400346C RID: 13420
		internal DirtyTextRange Dtr;

		// Token: 0x0400346D RID: 13421
		internal BaseParagraph FirstPara;

		// Token: 0x0400346E RID: 13422
		internal BaseParagraph SyncPara;

		// Token: 0x0400346F RID: 13423
		internal PTS.FSKCHANGE ChangeType;

		// Token: 0x04003470 RID: 13424
		internal UpdateRecord Next;

		// Token: 0x04003471 RID: 13425
		internal bool InProcessing;
	}
}
