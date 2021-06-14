using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003D0 RID: 976
	internal class ColumnStateArray : ArrayList
	{
		// Token: 0x06003488 RID: 13448 RVA: 0x000E3C42 File Offset: 0x000E1E42
		internal ColumnStateArray() : base(20)
		{
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000E9967 File Offset: 0x000E7B67
		internal ColumnState EntryAt(int i)
		{
			return (ColumnState)this[i];
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000E9978 File Offset: 0x000E7B78
		internal int GetMinUnfilledRowIndex()
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				ColumnState columnState = this.EntryAt(i);
				if (!columnState.IsFilled && (num < 0 || num > columnState.Row.Index) && !columnState.Row.FormatState.RowFormat.IsVMerge)
				{
					num = columnState.Row.Index;
				}
			}
			return num;
		}
	}
}
