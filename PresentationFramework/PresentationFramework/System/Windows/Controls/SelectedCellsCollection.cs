using System;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x0200052B RID: 1323
	internal sealed class SelectedCellsCollection : VirtualizedCellInfoCollection
	{
		// Token: 0x060055C7 RID: 21959 RVA: 0x0017C439 File Offset: 0x0017A639
		internal SelectedCellsCollection(DataGrid owner) : base(owner)
		{
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x0017C442 File Offset: 0x0017A642
		internal bool GetSelectionRange(out int minColumnDisplayIndex, out int maxColumnDisplayIndex, out int minRowIndex, out int maxRowIndex)
		{
			if (base.IsEmpty)
			{
				minColumnDisplayIndex = -1;
				maxColumnDisplayIndex = -1;
				minRowIndex = -1;
				maxRowIndex = -1;
				return false;
			}
			base.GetBoundingRegion(out minColumnDisplayIndex, out minRowIndex, out maxColumnDisplayIndex, out maxRowIndex);
			return true;
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x0017C467 File Offset: 0x0017A667
		protected override void OnCollectionChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
			base.Owner.OnSelectedCellsChanged(action, oldItems, newItems);
		}
	}
}
