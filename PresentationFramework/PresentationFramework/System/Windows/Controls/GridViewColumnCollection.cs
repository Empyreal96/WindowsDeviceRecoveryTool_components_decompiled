using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Controls.GridViewColumn" /> objects.</summary>
	// Token: 0x020004D8 RID: 1240
	[Serializable]
	public class GridViewColumnCollection : ObservableCollection<GridViewColumn>
	{
		/// <summary>Removes all of the <see cref="T:System.Windows.Controls.GridViewColumn" /> objects from the <see cref="T:System.Windows.Controls.GridViewColumnCollection" />.</summary>
		// Token: 0x06004D05 RID: 19717 RVA: 0x0015B43E File Offset: 0x0015963E
		protected override void ClearItems()
		{
			this.VerifyAccess();
			this._internalEventArg = this.ClearPreprocess();
			base.ClearItems();
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.GridViewColumn" /> from the <see cref="T:System.Windows.Controls.GridViewColumnCollection" /> at the specified index.</summary>
		/// <param name="index">The position of the <see cref="T:System.Windows.Controls.GridViewColumn" /> to remove.</param>
		// Token: 0x06004D06 RID: 19718 RVA: 0x0015B458 File Offset: 0x00159658
		protected override void RemoveItem(int index)
		{
			this.VerifyAccess();
			this._internalEventArg = this.RemoveAtPreprocess(index);
			base.RemoveItem(index);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.GridViewColumn" /> to the collection at the specified index.</summary>
		/// <param name="index">The position to place the new <see cref="T:System.Windows.Controls.GridViewColumn" />.</param>
		/// <param name="column">The <see cref="T:System.Windows.Controls.GridViewColumn" /> to insert.</param>
		// Token: 0x06004D07 RID: 19719 RVA: 0x0015B474 File Offset: 0x00159674
		protected override void InsertItem(int index, GridViewColumn column)
		{
			this.VerifyAccess();
			this._internalEventArg = this.InsertPreprocess(index, column);
			base.InsertItem(index, column);
		}

		/// <summary>Replaces the <see cref="T:System.Windows.Controls.GridViewColumn" /> that is at the specified index with another <see cref="T:System.Windows.Controls.GridViewColumn" />.</summary>
		/// <param name="index">The position at which the new <see cref="T:System.Windows.Controls.GridViewColumn" /> replaces the old <see cref="T:System.Windows.Controls.GridViewColumn" />.</param>
		/// <param name="column">The <see cref="T:System.Windows.Controls.GridViewColumn" /> to place at the specified position.</param>
		// Token: 0x06004D08 RID: 19720 RVA: 0x0015B492 File Offset: 0x00159692
		protected override void SetItem(int index, GridViewColumn column)
		{
			this.VerifyAccess();
			this._internalEventArg = this.SetPreprocess(index, column);
			if (this._internalEventArg != null)
			{
				base.SetItem(index, column);
			}
		}

		/// <summary>Changes the position of a <see cref="T:System.Windows.Controls.GridViewColumn" /> in the collection.</summary>
		/// <param name="oldIndex">The original position of the <see cref="T:System.Windows.Controls.GridViewColumn" />.</param>
		/// <param name="newIndex">The new position of the <see cref="T:System.Windows.Controls.GridViewColumn" />.</param>
		// Token: 0x06004D09 RID: 19721 RVA: 0x0015B4B8 File Offset: 0x001596B8
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				this.VerifyAccess();
				this._internalEventArg = this.MovePreprocess(oldIndex, newIndex);
				base.MoveItem(oldIndex, newIndex);
			}
		}

		/// <summary>Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event when the <see cref="T:System.Windows.Controls.GridViewColumnCollection" /> changes.</summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06004D0A RID: 19722 RVA: 0x0015B4DA File Offset: 0x001596DA
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			this.OnInternalCollectionChanged();
			base.OnCollectionChanged(e);
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06004D0B RID: 19723 RVA: 0x0015B4E9 File Offset: 0x001596E9
		// (remove) Token: 0x06004D0C RID: 19724 RVA: 0x0015B4F2 File Offset: 0x001596F2
		internal event NotifyCollectionChangedEventHandler InternalCollectionChanged
		{
			add
			{
				this._internalCollectionChanged += value;
			}
			remove
			{
				this._internalCollectionChanged -= value;
			}
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x0015B4FB File Offset: 0x001596FB
		internal void BlockWrite()
		{
			this.IsImmutable = true;
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x0015B504 File Offset: 0x00159704
		internal void UnblockWrite()
		{
			this.IsImmutable = false;
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0015B50D File Offset: 0x0015970D
		internal List<GridViewColumn> ColumnCollection
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06004D10 RID: 19728 RVA: 0x0015B515 File Offset: 0x00159715
		internal List<int> IndexList
		{
			get
			{
				return this._actualIndices;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0015B51D File Offset: 0x0015971D
		// (set) Token: 0x06004D12 RID: 19730 RVA: 0x0015B528 File Offset: 0x00159728
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				if (value != this._owner)
				{
					if (value == null)
					{
						using (List<GridViewColumn>.Enumerator enumerator = this._columns.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GridViewColumn oldValue = enumerator.Current;
								InheritanceContextHelper.RemoveContextFromObject(this._owner, oldValue);
							}
							goto IL_7D;
						}
					}
					foreach (GridViewColumn newValue in this._columns)
					{
						InheritanceContextHelper.ProvideContextForObject(value, newValue);
					}
					IL_7D:
					this._owner = value;
				}
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0015B5D8 File Offset: 0x001597D8
		// (set) Token: 0x06004D14 RID: 19732 RVA: 0x0015B5E0 File Offset: 0x001597E0
		internal bool InViewMode
		{
			get
			{
				return this._inViewMode;
			}
			set
			{
				this._inViewMode = value;
			}
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x0015B5E9 File Offset: 0x001597E9
		private void OnInternalCollectionChanged()
		{
			if (this._internalCollectionChanged != null && this._internalEventArg != null)
			{
				this._internalCollectionChanged(this, this._internalEventArg);
				this._internalEventArg = null;
			}
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x0015B614 File Offset: 0x00159814
		private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			GridViewColumn gridViewColumn = sender as GridViewColumn;
			if (this._internalCollectionChanged != null && gridViewColumn != null)
			{
				this._internalCollectionChanged(this, new GridViewColumnCollectionChangedEventArgs(gridViewColumn, e.PropertyName));
			}
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0015B64C File Offset: 0x0015984C
		private GridViewColumnCollectionChangedEventArgs MovePreprocess(int oldIndex, int newIndex)
		{
			this.VerifyIndexInRange(oldIndex, "oldIndex");
			this.VerifyIndexInRange(newIndex, "newIndex");
			int num = this._actualIndices[oldIndex];
			if (oldIndex < newIndex)
			{
				for (int i = oldIndex; i < newIndex; i++)
				{
					this._actualIndices[i] = this._actualIndices[i + 1];
				}
			}
			else
			{
				for (int j = oldIndex; j > newIndex; j--)
				{
					this._actualIndices[j] = this._actualIndices[j - 1];
				}
			}
			this._actualIndices[newIndex] = num;
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this._columns[num], newIndex, oldIndex, num);
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0015B6F4 File Offset: 0x001598F4
		private GridViewColumnCollectionChangedEventArgs ClearPreprocess()
		{
			GridViewColumn[] array = new GridViewColumn[base.Count];
			if (base.Count > 0)
			{
				base.CopyTo(array, 0);
			}
			foreach (GridViewColumn gridViewColumn in this._columns)
			{
				gridViewColumn.ResetPrivateData();
				((INotifyPropertyChanged)gridViewColumn).PropertyChanged -= this.ColumnPropertyChanged;
				InheritanceContextHelper.RemoveContextFromObject(this._owner, gridViewColumn);
			}
			this._columns.Clear();
			this._actualIndices.Clear();
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, array);
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0015B7A0 File Offset: 0x001599A0
		private GridViewColumnCollectionChangedEventArgs RemoveAtPreprocess(int index)
		{
			this.VerifyIndexInRange(index, "index");
			int num = this._actualIndices[index];
			GridViewColumn gridViewColumn = this._columns[num];
			gridViewColumn.ResetPrivateData();
			((INotifyPropertyChanged)gridViewColumn).PropertyChanged -= this.ColumnPropertyChanged;
			this._columns.RemoveAt(num);
			this.UpdateIndexList(num, index);
			this.UpdateActualIndexInColumn(num);
			InheritanceContextHelper.RemoveContextFromObject(this._owner, gridViewColumn);
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, gridViewColumn, index, num);
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0015B81C File Offset: 0x00159A1C
		private void UpdateIndexList(int actualIndex, int index)
		{
			for (int i = 0; i < index; i++)
			{
				int num = this._actualIndices[i];
				if (num > actualIndex)
				{
					this._actualIndices[i] = num - 1;
				}
			}
			for (int j = index + 1; j < this._actualIndices.Count; j++)
			{
				int num2 = this._actualIndices[j];
				if (num2 < actualIndex)
				{
					this._actualIndices[j - 1] = num2;
				}
				else if (num2 > actualIndex)
				{
					this._actualIndices[j - 1] = num2 - 1;
				}
			}
			this._actualIndices.RemoveAt(this._actualIndices.Count - 1);
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0015B8BC File Offset: 0x00159ABC
		private void UpdateActualIndexInColumn(int iStart)
		{
			for (int i = iStart; i < this._columns.Count; i++)
			{
				this._columns[i].ActualIndex = i;
			}
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0015B8F4 File Offset: 0x00159AF4
		private GridViewColumnCollectionChangedEventArgs InsertPreprocess(int index, GridViewColumn column)
		{
			int count = this._columns.Count;
			if (index < 0 || index > count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.ValidateColumnForInsert(column);
			this._columns.Add(column);
			column.ActualIndex = count;
			this._actualIndices.Insert(index, count);
			InheritanceContextHelper.ProvideContextForObject(this._owner, column);
			((INotifyPropertyChanged)column).PropertyChanged += this.ColumnPropertyChanged;
			return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, column, index, count);
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0015B970 File Offset: 0x00159B70
		private GridViewColumnCollectionChangedEventArgs SetPreprocess(int index, GridViewColumn newColumn)
		{
			this.VerifyIndexInRange(index, "index");
			GridViewColumn gridViewColumn = base[index];
			if (gridViewColumn != newColumn)
			{
				int actualIndex = this._actualIndices[index];
				this.RemoveAtPreprocess(index);
				this.InsertPreprocess(index, newColumn);
				return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newColumn, gridViewColumn, index, actualIndex);
			}
			return null;
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0015B9BF File Offset: 0x00159BBF
		private void VerifyIndexInRange(int index, string indexName)
		{
			if (index < 0 || index >= this._actualIndices.Count)
			{
				throw new ArgumentOutOfRangeException(indexName);
			}
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x0015B9DA File Offset: 0x00159BDA
		private void ValidateColumnForInsert(GridViewColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			if (column.ActualIndex >= 0)
			{
				throw new InvalidOperationException(SR.Get("ListView_NotAllowShareColumnToTwoColumnCollection"));
			}
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0015BA03 File Offset: 0x00159C03
		private void VerifyAccess()
		{
			if (this.IsImmutable)
			{
				throw new InvalidOperationException(SR.Get("ListView_GridViewColumnCollectionIsReadOnly"));
			}
			base.CheckReentrancy();
		}

		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06004D21 RID: 19745 RVA: 0x0015BA23 File Offset: 0x00159C23
		// (set) Token: 0x06004D22 RID: 19746 RVA: 0x0015BA2B File Offset: 0x00159C2B
		private bool IsImmutable
		{
			get
			{
				return this._isImmutable;
			}
			set
			{
				this._isImmutable = value;
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06004D23 RID: 19747 RVA: 0x0015BA34 File Offset: 0x00159C34
		// (remove) Token: 0x06004D24 RID: 19748 RVA: 0x0015BA6C File Offset: 0x00159C6C
		private event NotifyCollectionChangedEventHandler _internalCollectionChanged;

		// Token: 0x04002B4A RID: 11082
		[NonSerialized]
		private DependencyObject _owner;

		// Token: 0x04002B4B RID: 11083
		private bool _inViewMode;

		// Token: 0x04002B4C RID: 11084
		private List<GridViewColumn> _columns = new List<GridViewColumn>();

		// Token: 0x04002B4D RID: 11085
		private List<int> _actualIndices = new List<int>();

		// Token: 0x04002B4E RID: 11086
		private bool _isImmutable;

		// Token: 0x04002B50 RID: 11088
		[NonSerialized]
		private GridViewColumnCollectionChangedEventArgs _internalEventArg;
	}
}
