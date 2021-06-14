using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x0200052D RID: 1325
	internal class SelectedItemCollection : ObservableCollection<object>
	{
		// Token: 0x060055DF RID: 21983 RVA: 0x0017CCFD File Offset: 0x0017AEFD
		public SelectedItemCollection(Selector selector)
		{
			this._selector = selector;
			this._changer = new SelectedItemCollection.Changer(this);
		}

		// Token: 0x060055E0 RID: 21984 RVA: 0x0017CD18 File Offset: 0x0017AF18
		protected override void ClearItems()
		{
			if (this._updatingSelectedItems)
			{
				using (IEnumerator<ItemsControl.ItemInfo> enumerator = ((IEnumerable<ItemsControl.ItemInfo>)this._selector._selectedItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemsControl.ItemInfo info = enumerator.Current;
						this._selector.SelectionChange.Unselect(info);
					}
					return;
				}
			}
			using (this.ChangeSelectedItems())
			{
				base.ClearItems();
			}
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x0017CDA4 File Offset: 0x0017AFA4
		protected override void RemoveItem(int index)
		{
			if (this._updatingSelectedItems)
			{
				this._selector.SelectionChange.Unselect(this._selector.NewItemInfo(base[index], null, -1));
				return;
			}
			using (this.ChangeSelectedItems())
			{
				base.RemoveItem(index);
			}
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x0017CE0C File Offset: 0x0017B00C
		protected override void InsertItem(int index, object item)
		{
			if (!this._updatingSelectedItems)
			{
				using (this.ChangeSelectedItems())
				{
					base.InsertItem(index, item);
				}
				return;
			}
			if (index == base.Count)
			{
				this._selector.SelectionChange.Select(this._selector.NewItemInfo(item, null, -1), true);
				return;
			}
			throw new InvalidOperationException(SR.Get("InsertInDeferSelectionActive"));
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x0017CE88 File Offset: 0x0017B088
		protected override void SetItem(int index, object item)
		{
			if (this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("SetInDeferSelectionActive"));
			}
			using (this.ChangeSelectedItems())
			{
				base.SetItem(index, item);
			}
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x0017CED8 File Offset: 0x0017B0D8
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				if (this._updatingSelectedItems)
				{
					throw new InvalidOperationException(SR.Get("MoveInDeferSelectionActive"));
				}
				using (this.ChangeSelectedItems())
				{
					base.MoveItem(oldIndex, newIndex);
				}
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x060055E5 RID: 21989 RVA: 0x0017CF2C File Offset: 0x0017B12C
		internal bool IsChanging
		{
			get
			{
				return this._changeCount > 0;
			}
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x0017CF37 File Offset: 0x0017B137
		private IDisposable ChangeSelectedItems()
		{
			this._changeCount++;
			return this._changer;
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x0017CF50 File Offset: 0x0017B150
		private void FinishChange()
		{
			int num = this._changeCount - 1;
			this._changeCount = num;
			if (num == 0)
			{
				this._selector.FinishSelectedItemsChange();
			}
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x0017CF7C File Offset: 0x0017B17C
		internal void BeginUpdateSelectedItems()
		{
			if (this._selector.SelectionChange.IsActive || this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionActive"));
			}
			this._updatingSelectedItems = true;
			this._selector.SelectionChange.Begin();
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x0017CFCC File Offset: 0x0017B1CC
		internal void EndUpdateSelectedItems()
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._updatingSelectedItems = false;
			this._selector.SelectionChange.End();
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x060055EA RID: 21994 RVA: 0x0017D01A File Offset: 0x0017B21A
		internal bool IsUpdatingSelectedItems
		{
			get
			{
				return this._selector.SelectionChange.IsActive || this._updatingSelectedItems;
			}
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x0017D036 File Offset: 0x0017B236
		internal void Add(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Select(info, true);
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x0017D075 File Offset: 0x0017B275
		internal void Remove(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Unselect(info);
		}

		// Token: 0x04002E15 RID: 11797
		private int _changeCount;

		// Token: 0x04002E16 RID: 11798
		private SelectedItemCollection.Changer _changer;

		// Token: 0x04002E17 RID: 11799
		private Selector _selector;

		// Token: 0x04002E18 RID: 11800
		private bool _updatingSelectedItems;

		// Token: 0x020009BA RID: 2490
		private class Changer : IDisposable
		{
			// Token: 0x06008874 RID: 34932 RVA: 0x0025241B File Offset: 0x0025061B
			public Changer(SelectedItemCollection owner)
			{
				this._owner = owner;
			}

			// Token: 0x06008875 RID: 34933 RVA: 0x0025242A File Offset: 0x0025062A
			public void Dispose()
			{
				this._owner.FinishChange();
			}

			// Token: 0x04004560 RID: 17760
			private SelectedItemCollection _owner;
		}
	}
}
