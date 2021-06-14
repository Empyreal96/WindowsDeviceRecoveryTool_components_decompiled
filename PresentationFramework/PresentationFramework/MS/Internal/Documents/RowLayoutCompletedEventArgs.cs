using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006ED RID: 1773
	internal class RowLayoutCompletedEventArgs : EventArgs
	{
		// Token: 0x060071F9 RID: 29177 RVA: 0x00209581 File Offset: 0x00207781
		public RowLayoutCompletedEventArgs(int pivotRowIndex)
		{
			this._pivotRowIndex = pivotRowIndex;
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x060071FA RID: 29178 RVA: 0x00209590 File Offset: 0x00207790
		public int PivotRowIndex
		{
			get
			{
				return this._pivotRowIndex;
			}
		}

		// Token: 0x0400374F RID: 14159
		private readonly int _pivotRowIndex;
	}
}
