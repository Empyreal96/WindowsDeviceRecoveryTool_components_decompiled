using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000484 RID: 1156
	internal class GridEntryRecreateChildrenEventArgs : EventArgs
	{
		// Token: 0x06004E20 RID: 20000 RVA: 0x0014019C File Offset: 0x0013E39C
		public GridEntryRecreateChildrenEventArgs(int oldCount, int newCount)
		{
			this.OldChildCount = oldCount;
			this.NewChildCount = newCount;
		}

		// Token: 0x0400332F RID: 13103
		public readonly int OldChildCount;

		// Token: 0x04003330 RID: 13104
		public readonly int NewChildCount;
	}
}
