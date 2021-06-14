using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004A0 RID: 1184
	internal class DataGridColumnCollection : ObservableCollection<DataGridColumn>
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x00146E8C File Offset: 0x0014508C
		internal DataGridColumnCollection(DataGrid dataGridOwner)
		{
			this.DisplayIndexMap = new List<int>(5);
			this._dataGridOwner = dataGridOwner;
			this.RealizedColumnsBlockListForNonVirtualizedRows = null;
			this.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows = null;
			this.RebuildRealizedColumnsBlockListForNonVirtualizedRows = true;
			this.RealizedColumnsBlockListForVirtualizedRows = null;
			this.RealizedColumnsDisplayIndexBlockListForVirtualizedRows = null;
			this.RebuildRealizedColumnsBlockListForVirtualizedRows = true;
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x00146EDC File Offset: 0x001450DC
		protected override void InsertItem(int index, DataGridColumn item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item", SR.Get("DataGrid_NullColumn"));
			}
			if (item.DataGridOwner != null)
			{
				throw new ArgumentException(SR.Get("DataGrid_InvalidColumnReuse", new object[]
				{
					item.Header
				}), "item");
			}
			if (this.DisplayIndexMapInitialized)
			{
				this.ValidateDisplayIndex(item, item.DisplayIndex, true);
			}
			base.InsertItem(index, item);
			item.CoerceValue(DataGridColumn.IsFrozenProperty);
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x00146F58 File Offset: 0x00145158
		protected override void SetItem(int index, DataGridColumn item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item", SR.Get("DataGrid_NullColumn"));
			}
			if (index >= base.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("DataGrid_ColumnIndexOutOfRange", new object[]
				{
					item.Header
				}));
			}
			if (item.DataGridOwner != null && base[index] != item)
			{
				throw new ArgumentException(SR.Get("DataGrid_InvalidColumnReuse", new object[]
				{
					item.Header
				}), "item");
			}
			if (this.DisplayIndexMapInitialized)
			{
				this.ValidateDisplayIndex(item, item.DisplayIndex);
			}
			base.SetItem(index, item);
			item.CoerceValue(DataGridColumn.IsFrozenProperty);
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0014700C File Offset: 0x0014520C
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForNewColumns(e.NewItems, e.NewStartingIndex);
				}
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Remove:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForRemovedColumns(e.OldItems, e.OldStartingIndex);
				}
				this.ClearDisplayIndex(e.OldItems, e.NewItems);
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForReplacedColumn(e.OldItems, e.NewItems);
				}
				this.ClearDisplayIndex(e.OldItems, e.NewItems);
				this.InvalidateHasVisibleStarColumns();
				break;
			case NotifyCollectionChangedAction.Move:
				if (this.DisplayIndexMapInitialized)
				{
					this.UpdateDisplayIndexForMovedColumn(e.OldStartingIndex, e.NewStartingIndex);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				if (this.DisplayIndexMapInitialized)
				{
					this.DisplayIndexMap.Clear();
					this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Reset, -1, null, -1);
				}
				this.HasVisibleStarColumns = false;
				break;
			}
			this.InvalidateAverageColumnWidth();
			base.OnCollectionChanged(e);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x00147124 File Offset: 0x00145324
		protected override void ClearItems()
		{
			this.ClearDisplayIndex(this, null);
			this.DataGridOwner.UpdateDataGridReference(this, true);
			base.ClearItems();
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x00147144 File Offset: 0x00145344
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyColumnCollection(target))
			{
				if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					this.OnColumnDisplayIndexChanged((DataGridColumn)d, (int)e.OldValue, (int)e.NewValue);
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateColumnRealization(true);
					}
				}
				else if (e.Property == DataGridColumn.WidthProperty)
				{
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateColumnRealization(false);
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty)
				{
					this.InvalidateColumnRealization(false);
					this.OnDataGridFrozenColumnCountChanged((int)e.OldValue, (int)e.NewValue);
				}
				else if (e.Property == DataGridColumn.VisibilityProperty)
				{
					this.InvalidateAverageColumnWidth();
					this.InvalidateHasVisibleStarColumns();
					this.InvalidateColumnWidthsComputation();
					this.InvalidateColumnRealization(true);
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					this.InvalidateColumnRealization(true);
				}
				else if (e.Property == DataGrid.CellsPanelHorizontalOffsetProperty)
				{
					this.OnCellsPanelHorizontalOffsetChanged(e);
				}
				else if (e.Property == DataGrid.HorizontalScrollOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0)
				{
					this.InvalidateColumnRealization(false);
				}
			}
			if (DataGridHelper.ShouldNotifyColumns(target))
			{
				int count = base.Count;
				for (int i = 0; i < count; i++)
				{
					base[i].NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
				}
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x001472AF File Offset: 0x001454AF
		internal DataGridColumn ColumnFromDisplayIndex(int displayIndex)
		{
			return base[this.DisplayIndexMap[displayIndex]];
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06004812 RID: 18450 RVA: 0x001472C3 File Offset: 0x001454C3
		// (set) Token: 0x06004813 RID: 18451 RVA: 0x001472D9 File Offset: 0x001454D9
		internal List<int> DisplayIndexMap
		{
			get
			{
				if (!this.DisplayIndexMapInitialized)
				{
					this.InitializeDisplayIndexMap();
				}
				return this._displayIndexMap;
			}
			private set
			{
				this._displayIndexMap = value;
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06004814 RID: 18452 RVA: 0x001472E2 File Offset: 0x001454E2
		// (set) Token: 0x06004815 RID: 18453 RVA: 0x001472EA File Offset: 0x001454EA
		private bool IsUpdatingDisplayIndex
		{
			get
			{
				return this._isUpdatingDisplayIndex;
			}
			set
			{
				this._isUpdatingDisplayIndex = value;
			}
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x001472F3 File Offset: 0x001454F3
		private int CoerceDefaultDisplayIndex(DataGridColumn column)
		{
			return this.CoerceDefaultDisplayIndex(column, base.IndexOf(column));
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x00147304 File Offset: 0x00145504
		private int CoerceDefaultDisplayIndex(DataGridColumn column, int newDisplayIndex)
		{
			if (DataGridHelper.IsDefaultValue(column, DataGridColumn.DisplayIndexProperty))
			{
				bool isUpdatingDisplayIndex = this.IsUpdatingDisplayIndex;
				try
				{
					this.IsUpdatingDisplayIndex = true;
					column.DisplayIndex = newDisplayIndex;
				}
				finally
				{
					this.IsUpdatingDisplayIndex = isUpdatingDisplayIndex;
				}
				return newDisplayIndex;
			}
			return column.DisplayIndex;
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00147358 File Offset: 0x00145558
		private void OnColumnDisplayIndexChanged(DataGridColumn column, int oldDisplayIndex, int newDisplayIndex)
		{
			int num = oldDisplayIndex;
			if (!this._displayIndexMapInitialized)
			{
				this.InitializeDisplayIndexMap(column, oldDisplayIndex, out oldDisplayIndex);
			}
			if (this._isClearingDisplayIndex)
			{
				return;
			}
			newDisplayIndex = this.CoerceDefaultDisplayIndex(column);
			if (newDisplayIndex == oldDisplayIndex)
			{
				return;
			}
			if (num != -1)
			{
				this.DataGridOwner.OnColumnDisplayIndexChanged(new DataGridColumnEventArgs(column));
			}
			this.UpdateDisplayIndexForChangedColumn(oldDisplayIndex, newDisplayIndex);
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001473B0 File Offset: 0x001455B0
		private void UpdateDisplayIndexForChangedColumn(int oldDisplayIndex, int newDisplayIndex)
		{
			if (this.IsUpdatingDisplayIndex)
			{
				return;
			}
			try
			{
				this.IsUpdatingDisplayIndex = true;
				int item = this.DisplayIndexMap[oldDisplayIndex];
				this.DisplayIndexMap.RemoveAt(oldDisplayIndex);
				this.DisplayIndexMap.Insert(newDisplayIndex, item);
				if (newDisplayIndex < oldDisplayIndex)
				{
					for (int i = newDisplayIndex + 1; i <= oldDisplayIndex; i++)
					{
						DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
						int displayIndex = dataGridColumn.DisplayIndex;
						dataGridColumn.DisplayIndex = displayIndex + 1;
					}
				}
				else
				{
					for (int j = oldDisplayIndex; j < newDisplayIndex; j++)
					{
						DataGridColumn dataGridColumn2 = this.ColumnFromDisplayIndex(j);
						int displayIndex = dataGridColumn2.DisplayIndex;
						dataGridColumn2.DisplayIndex = displayIndex - 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Move, oldDisplayIndex, null, newDisplayIndex);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0014746C File Offset: 0x0014566C
		private void UpdateDisplayIndexForMovedColumn(int oldColumnIndex, int newColumnIndex)
		{
			int newDisplayIndex = this.RemoveFromDisplayIndexMap(oldColumnIndex);
			this.InsertInDisplayIndexMap(newDisplayIndex, newColumnIndex);
			this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Move, oldColumnIndex, null, newColumnIndex);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00147498 File Offset: 0x00145698
		private void UpdateDisplayIndexForNewColumns(IList newColumns, int startingIndex)
		{
			try
			{
				this.IsUpdatingDisplayIndex = true;
				DataGridColumn dataGridColumn = (DataGridColumn)newColumns[0];
				int num = this.CoerceDefaultDisplayIndex(dataGridColumn, startingIndex);
				this.InsertInDisplayIndexMap(num, startingIndex);
				for (int i = 0; i < this.DisplayIndexMap.Count; i++)
				{
					if (i > num)
					{
						dataGridColumn = this.ColumnFromDisplayIndex(i);
						DataGridColumn dataGridColumn2 = dataGridColumn;
						int displayIndex = dataGridColumn2.DisplayIndex;
						dataGridColumn2.DisplayIndex = displayIndex + 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Add, -1, null, num);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0014752C File Offset: 0x0014572C
		internal void InitializeDisplayIndexMap()
		{
			int num = -1;
			this.InitializeDisplayIndexMap(null, -1, out num);
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x00147548 File Offset: 0x00145748
		private void InitializeDisplayIndexMap(DataGridColumn changingColumn, int oldDisplayIndex, out int resultDisplayIndex)
		{
			resultDisplayIndex = oldDisplayIndex;
			if (this._displayIndexMapInitialized)
			{
				return;
			}
			this._displayIndexMapInitialized = true;
			int count = base.Count;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (changingColumn != null && oldDisplayIndex >= count)
			{
				throw new ArgumentOutOfRangeException("displayIndex", oldDisplayIndex, SR.Get("DataGrid_ColumnDisplayIndexOutOfRange", new object[]
				{
					changingColumn.Header
				}));
			}
			for (int i = 0; i < count; i++)
			{
				DataGridColumn dataGridColumn = base[i];
				int num = dataGridColumn.DisplayIndex;
				this.ValidateDisplayIndex(dataGridColumn, num);
				if (dataGridColumn == changingColumn)
				{
					num = oldDisplayIndex;
				}
				if (num >= 0)
				{
					if (dictionary.ContainsKey(num))
					{
						throw new ArgumentException(SR.Get("DataGrid_DuplicateDisplayIndex"));
					}
					dictionary.Add(num, i);
				}
			}
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				DataGridColumn dataGridColumn2 = base[j];
				int displayIndex = dataGridColumn2.DisplayIndex;
				bool flag = DataGridHelper.IsDefaultValue(dataGridColumn2, DataGridColumn.DisplayIndexProperty);
				if (dataGridColumn2 == changingColumn)
				{
					if (oldDisplayIndex == -1)
					{
						flag = true;
					}
				}
				if (flag)
				{
					while (dictionary.ContainsKey(num2))
					{
						num2++;
					}
					this.CoerceDefaultDisplayIndex(dataGridColumn2, num2);
					dictionary.Add(num2, j);
					if (dataGridColumn2 == changingColumn)
					{
						resultDisplayIndex = num2;
					}
					num2++;
				}
			}
			for (int k = 0; k < count; k++)
			{
				this.DisplayIndexMap.Add(dictionary[k]);
			}
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x00147698 File Offset: 0x00145898
		private void UpdateDisplayIndexForRemovedColumns(IList oldColumns, int startingIndex)
		{
			try
			{
				this.IsUpdatingDisplayIndex = true;
				int num = this.RemoveFromDisplayIndexMap(startingIndex);
				for (int i = 0; i < this.DisplayIndexMap.Count; i++)
				{
					if (i >= num)
					{
						DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
						DataGridColumn dataGridColumn2 = dataGridColumn;
						int displayIndex = dataGridColumn2.DisplayIndex;
						dataGridColumn2.DisplayIndex = displayIndex - 1;
					}
				}
				this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Remove, num, (DataGridColumn)oldColumns[0], -1);
			}
			finally
			{
				this.IsUpdatingDisplayIndex = false;
			}
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0014771C File Offset: 0x0014591C
		private void UpdateDisplayIndexForReplacedColumn(IList oldColumns, IList newColumns)
		{
			if (oldColumns != null && oldColumns.Count > 0 && newColumns != null && newColumns.Count > 0)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)oldColumns[0];
				DataGridColumn dataGridColumn2 = (DataGridColumn)newColumns[0];
				if (dataGridColumn != null && dataGridColumn2 != null)
				{
					int num = this.CoerceDefaultDisplayIndex(dataGridColumn2);
					if (dataGridColumn.DisplayIndex != num)
					{
						this.UpdateDisplayIndexForChangedColumn(dataGridColumn.DisplayIndex, num);
					}
					this.DataGridOwner.UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction.Replace, num, dataGridColumn, num);
				}
			}
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x00147790 File Offset: 0x00145990
		private void ClearDisplayIndex(IList oldColumns, IList newColumns)
		{
			if (oldColumns != null)
			{
				try
				{
					this._isClearingDisplayIndex = true;
					int count = oldColumns.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridColumn dataGridColumn = (DataGridColumn)oldColumns[i];
						if (newColumns == null || !newColumns.Contains(dataGridColumn))
						{
							dataGridColumn.ClearValue(DataGridColumn.DisplayIndexProperty);
						}
					}
				}
				finally
				{
					this._isClearingDisplayIndex = false;
				}
			}
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x001477F8 File Offset: 0x001459F8
		private bool IsDisplayIndexValid(DataGridColumn column, int displayIndex, bool isAdding)
		{
			if (displayIndex == -1 && DataGridHelper.IsDefaultValue(column, DataGridColumn.DisplayIndexProperty))
			{
				return true;
			}
			if (displayIndex < 0)
			{
				return false;
			}
			if (!isAdding)
			{
				return displayIndex < base.Count;
			}
			return displayIndex <= base.Count;
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x0014782C File Offset: 0x00145A2C
		private void InsertInDisplayIndexMap(int newDisplayIndex, int columnIndex)
		{
			this.DisplayIndexMap.Insert(newDisplayIndex, columnIndex);
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
				if (this.DisplayIndexMap[i] >= columnIndex && i != newDisplayIndex)
				{
					List<int> displayIndexMap = this.DisplayIndexMap;
					int index = i;
					int num = displayIndexMap[index];
					displayIndexMap[index] = num + 1;
				}
			}
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x00147888 File Offset: 0x00145A88
		private int RemoveFromDisplayIndexMap(int columnIndex)
		{
			int num = this.DisplayIndexMap.IndexOf(columnIndex);
			this.DisplayIndexMap.RemoveAt(num);
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
				if (this.DisplayIndexMap[i] >= columnIndex)
				{
					List<int> displayIndexMap = this.DisplayIndexMap;
					int index = i;
					int num2 = displayIndexMap[index];
					displayIndexMap[index] = num2 - 1;
				}
			}
			return num;
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x001478ED File Offset: 0x00145AED
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex)
		{
			this.ValidateDisplayIndex(column, displayIndex, false);
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x001478F8 File Offset: 0x00145AF8
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex, bool isAdding)
		{
			if (!this.IsDisplayIndexValid(column, displayIndex, isAdding))
			{
				throw new ArgumentOutOfRangeException("displayIndex", displayIndex, SR.Get("DataGrid_ColumnDisplayIndexOutOfRange", new object[]
				{
					column.Header
				}));
			}
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x00147930 File Offset: 0x00145B30
		[Conditional("DEBUG")]
		private void Debug_VerifyDisplayIndexMap()
		{
			for (int i = 0; i < this.DisplayIndexMap.Count; i++)
			{
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x00147954 File Offset: 0x00145B54
		private void OnDataGridFrozenColumnCountChanged(int oldFrozenCount, int newFrozenCount)
		{
			if (newFrozenCount > oldFrozenCount)
			{
				int num = Math.Min(newFrozenCount, base.Count);
				for (int i = oldFrozenCount; i < num; i++)
				{
					this.ColumnFromDisplayIndex(i).IsFrozen = true;
				}
				return;
			}
			int num2 = Math.Min(oldFrozenCount, base.Count);
			for (int j = newFrozenCount; j < num2; j++)
			{
				this.ColumnFromDisplayIndex(j).IsFrozen = false;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x001479B2 File Offset: 0x00145BB2
		private DataGrid DataGridOwner
		{
			get
			{
				return this._dataGridOwner;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06004829 RID: 18473 RVA: 0x001479BA File Offset: 0x00145BBA
		internal bool DisplayIndexMapInitialized
		{
			get
			{
				return this._displayIndexMapInitialized;
			}
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x001479C4 File Offset: 0x00145BC4
		private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn, out double perStarWidth)
		{
			bool result = false;
			perStarWidth = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn != ignoredColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar)
					{
						result = true;
						if (!DoubleUtil.AreClose(width.Value, 0.0) && !DoubleUtil.AreClose(width.DesiredValue, 0.0))
						{
							perStarWidth = width.DesiredValue / width.Value;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x00147A74 File Offset: 0x00145C74
		private bool HasVisibleStarColumnsInternal(out double perStarWidth)
		{
			return this.HasVisibleStarColumnsInternal(null, out perStarWidth);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00147A80 File Offset: 0x00145C80
		private bool HasVisibleStarColumnsInternal(DataGridColumn ignoredColumn)
		{
			double num;
			return this.HasVisibleStarColumnsInternal(ignoredColumn, out num);
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x0600482D RID: 18477 RVA: 0x00147A96 File Offset: 0x00145C96
		// (set) Token: 0x0600482E RID: 18478 RVA: 0x00147A9E File Offset: 0x00145C9E
		internal bool HasVisibleStarColumns
		{
			get
			{
				return this._hasVisibleStarColumns;
			}
			private set
			{
				if (this._hasVisibleStarColumns != value)
				{
					this._hasVisibleStarColumns = value;
					this.DataGridOwner.OnHasVisibleStarColumnsChanged();
				}
			}
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x00147ABB File Offset: 0x00145CBB
		internal void InvalidateHasVisibleStarColumns()
		{
			this.HasVisibleStarColumns = this.HasVisibleStarColumnsInternal(null);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x00147ACC File Offset: 0x00145CCC
		private void RecomputeStarColumnWidths()
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !width.IsStar)
				{
					num += width.DisplayValue;
				}
			}
			if (DoubleUtil.IsNaN(num))
			{
				return;
			}
			this.ComputeStarColumnWidths(viewportWidthForColumns - num);
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x00147B5C File Offset: 0x00145D5C
		private double ComputeStarColumnWidths(double availableStarSpace)
		{
			List<DataGridColumn> list = new List<DataGridColumn>();
			List<DataGridColumn> list2 = new List<DataGridColumn>();
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && width.IsStar)
				{
					list.Add(dataGridColumn);
					num += width.Value;
					num2 += dataGridColumn.MinWidth;
					num3 += dataGridColumn.MaxWidth;
				}
			}
			if (DoubleUtil.LessThan(availableStarSpace, num2))
			{
				availableStarSpace = num2;
			}
			if (DoubleUtil.GreaterThan(availableStarSpace, num3))
			{
				availableStarSpace = num3;
			}
			while (list.Count > 0)
			{
				double num5 = availableStarSpace / num;
				int i = 0;
				int num6 = list.Count;
				while (i < num6)
				{
					DataGridColumn dataGridColumn2 = list[i];
					DataGridLength width2 = dataGridColumn2.Width;
					double minWidth = dataGridColumn2.MinWidth;
					double value = availableStarSpace * width2.Value / num;
					if (DoubleUtil.GreaterThan(minWidth, value))
					{
						availableStarSpace = Math.Max(0.0, availableStarSpace - minWidth);
						num -= width2.Value;
						list.RemoveAt(i);
						i--;
						num6--;
						list2.Add(dataGridColumn2);
					}
					i++;
				}
				bool flag = false;
				int j = 0;
				int count = list.Count;
				while (j < count)
				{
					DataGridColumn dataGridColumn3 = list[j];
					DataGridLength width3 = dataGridColumn3.Width;
					double maxWidth = dataGridColumn3.MaxWidth;
					double value2 = availableStarSpace * width3.Value / num;
					if (DoubleUtil.LessThan(maxWidth, value2))
					{
						flag = true;
						list.RemoveAt(j);
						availableStarSpace -= maxWidth;
						num4 += maxWidth;
						num -= width3.Value;
						dataGridColumn3.UpdateWidthForStarColumn(maxWidth, num5 * width3.Value, width3.Value);
						break;
					}
					j++;
				}
				if (flag)
				{
					int k = 0;
					int count2 = list2.Count;
					while (k < count2)
					{
						DataGridColumn dataGridColumn4 = list2[k];
						list.Add(dataGridColumn4);
						availableStarSpace += dataGridColumn4.MinWidth;
						num += dataGridColumn4.Width.Value;
						k++;
					}
					list2.Clear();
				}
				else
				{
					int l = 0;
					int count3 = list2.Count;
					while (l < count3)
					{
						DataGridColumn dataGridColumn5 = list2[l];
						DataGridLength width4 = dataGridColumn5.Width;
						double minWidth2 = dataGridColumn5.MinWidth;
						dataGridColumn5.UpdateWidthForStarColumn(minWidth2, width4.Value * num5, width4.Value);
						num4 += minWidth2;
						l++;
					}
					list2.Clear();
					int m = 0;
					int count4 = list.Count;
					while (m < count4)
					{
						DataGridColumn dataGridColumn6 = list[m];
						DataGridLength width5 = dataGridColumn6.Width;
						double num7 = availableStarSpace * width5.Value / num;
						dataGridColumn6.UpdateWidthForStarColumn(num7, width5.Value * num5, width5.Value);
						num4 += num7;
						m++;
					}
					list.Clear();
				}
			}
			return num4;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x00147E7C File Offset: 0x0014607C
		private void OnCellsPanelHorizontalOffsetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.InvalidateColumnRealization(false);
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			this.RedistributeColumnWidthsOnAvailableSpaceChange((double)e.OldValue - (double)e.NewValue, viewportWidthForColumns);
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x00147EBC File Offset: 0x001460BC
		internal void InvalidateAverageColumnWidth()
		{
			this._averageColumnWidth = null;
			VirtualizingStackPanel virtualizingStackPanel = (this.DataGridOwner == null) ? null : (this.DataGridOwner.InternalItemsHost as VirtualizingStackPanel);
			if (virtualizingStackPanel != null)
			{
				virtualizingStackPanel.ResetMaximumDesiredSize();
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x00147EFA File Offset: 0x001460FA
		internal double AverageColumnWidth
		{
			get
			{
				if (this._averageColumnWidth == null)
				{
					this._averageColumnWidth = new double?(this.ComputeAverageColumnWidth());
				}
				return this._averageColumnWidth.Value;
			}
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x00147F28 File Offset: 0x00146128
		private double ComputeAverageColumnWidth()
		{
			double num = 0.0;
			int num2 = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !DoubleUtil.IsNaN(width.DisplayValue))
				{
					num += width.DisplayValue;
					num2++;
				}
			}
			if (num2 != 0)
			{
				return num / (double)num2;
			}
			return 0.0;
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x06004836 RID: 18486 RVA: 0x00147FB4 File Offset: 0x001461B4
		internal bool ColumnWidthsComputationPending
		{
			get
			{
				return this._columnWidthsComputationPending;
			}
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00147FBC File Offset: 0x001461BC
		internal void InvalidateColumnWidthsComputation()
		{
			if (this._columnWidthsComputationPending)
			{
				return;
			}
			this.DataGridOwner.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.ComputeColumnWidths), DispatcherPriority.Render, new object[]
			{
				this
			});
			this._columnWidthsComputationPending = true;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x00147FF8 File Offset: 0x001461F8
		private object ComputeColumnWidths(object arg)
		{
			this.ComputeColumnWidths();
			this.DataGridOwner.NotifyPropertyChanged(this.DataGridOwner, "DelayedColumnWidthComputation", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			return null;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00148030 File Offset: 0x00146230
		private void ComputeColumnWidths()
		{
			if (this.HasVisibleStarColumns)
			{
				this.InitializeColumnDisplayValues();
				this.DistributeSpaceAmongColumns(this.DataGridOwner.GetViewportWidthForColumns());
			}
			else
			{
				this.ExpandAllColumnWidthsToDesiredValue();
			}
			if (this.RefreshAutoWidthColumns)
			{
				foreach (DataGridColumn dataGridColumn in this)
				{
					if (dataGridColumn.Width.IsAuto)
					{
						dataGridColumn.Width = DataGridLength.Auto;
					}
				}
				this.RefreshAutoWidthColumns = false;
			}
			this._columnWidthsComputationPending = false;
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x001480CC File Offset: 0x001462CC
		private void InitializeColumnDisplayValues()
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double minWidth = dataGridColumn.MinWidth;
						double num = DataGridHelper.CoerceToMinMax(DoubleUtil.IsNaN(width.DesiredValue) ? minWidth : width.DesiredValue, minWidth, dataGridColumn.MaxWidth);
						if (!DoubleUtil.AreClose(width.DisplayValue, num))
						{
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
						}
					}
				}
			}
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x00148188 File Offset: 0x00146388
		internal void RedistributeColumnWidthsOnMinWidthChangeOfColumn(DataGridColumn changedColumn, double oldMinWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			double minWidth = changedColumn.MinWidth;
			if (DoubleUtil.GreaterThan(minWidth, width.DisplayValue))
			{
				if (this.HasVisibleStarColumns)
				{
					this.TakeAwayWidthFromColumns(changedColumn, minWidth - width.DisplayValue, false);
				}
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth));
				return;
			}
			if (DoubleUtil.LessThan(minWidth, oldMinWidth))
			{
				if (width.IsStar)
				{
					if (DoubleUtil.AreClose(width.DisplayValue, oldMinWidth))
					{
						this.GiveAwayWidthToColumns(changedColumn, oldMinWidth - minWidth, true);
						return;
					}
				}
				else if (DoubleUtil.GreaterThan(oldMinWidth, width.DesiredValue))
				{
					double num = Math.Max(width.DesiredValue, minWidth);
					if (this.HasVisibleStarColumns)
					{
						this.GiveAwayWidthToColumns(changedColumn, oldMinWidth - num);
					}
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				}
			}
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x00148278 File Offset: 0x00146478
		internal void RedistributeColumnWidthsOnMaxWidthChangeOfColumn(DataGridColumn changedColumn, double oldMaxWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			double maxWidth = changedColumn.MaxWidth;
			if (DoubleUtil.LessThan(maxWidth, width.DisplayValue))
			{
				if (this.HasVisibleStarColumns)
				{
					this.GiveAwayWidthToColumns(changedColumn, width.DisplayValue - maxWidth);
				}
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, maxWidth));
				return;
			}
			if (DoubleUtil.GreaterThan(maxWidth, oldMaxWidth))
			{
				if (width.IsStar)
				{
					this.RecomputeStarColumnWidths();
					return;
				}
				if (DoubleUtil.LessThan(oldMaxWidth, width.DesiredValue))
				{
					double num = Math.Min(width.DesiredValue, maxWidth);
					if (this.HasVisibleStarColumns)
					{
						double num2 = this.TakeAwayWidthFromUnusedSpace(false, num - oldMaxWidth);
						num2 = this.TakeAwayWidthFromStarColumns(changedColumn, num2);
						num -= num2;
					}
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				}
			}
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x00148360 File Offset: 0x00146560
		internal void RedistributeColumnWidthsOnWidthChangeOfColumn(DataGridColumn changedColumn, DataGridLength oldWidth)
		{
			if (this.ColumnWidthsComputationPending)
			{
				return;
			}
			DataGridLength width = changedColumn.Width;
			bool hasVisibleStarColumns = this.HasVisibleStarColumns;
			if (oldWidth.IsStar && !width.IsStar && !hasVisibleStarColumns)
			{
				this.ExpandAllColumnWidthsToDesiredValue();
				return;
			}
			if (width.IsStar && !oldWidth.IsStar)
			{
				if (!this.HasVisibleStarColumnsInternal(changedColumn))
				{
					this.ComputeColumnWidths();
					return;
				}
				double minWidth = changedColumn.MinWidth;
				double num = this.GiveAwayWidthToNonStarColumns(null, oldWidth.DisplayValue - minWidth);
				changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, minWidth + num));
				this.RecomputeStarColumnWidths();
				return;
			}
			else
			{
				if (width.IsStar && oldWidth.IsStar)
				{
					this.RecomputeStarColumnWidths();
					return;
				}
				if (hasVisibleStarColumns)
				{
					this.RedistributeColumnWidthsOnNonStarWidthChange(changedColumn, oldWidth);
				}
				return;
			}
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x00148428 File Offset: 0x00146628
		internal void RedistributeColumnWidthsOnAvailableSpaceChange(double availableSpaceChange, double newTotalAvailableSpace)
		{
			if (!this.ColumnWidthsComputationPending && this.HasVisibleStarColumns)
			{
				if (DoubleUtil.GreaterThan(availableSpaceChange, 0.0))
				{
					this.GiveAwayWidthToColumns(null, availableSpaceChange);
					return;
				}
				if (DoubleUtil.LessThan(availableSpaceChange, 0.0))
				{
					this.TakeAwayWidthFromColumns(null, Math.Abs(availableSpaceChange), false, newTotalAvailableSpace);
				}
			}
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x00148484 File Offset: 0x00146684
		private void ExpandAllColumnWidthsToDesiredValue()
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					double maxWidth = dataGridColumn.MaxWidth;
					if (DoubleUtil.GreaterThan(width.DesiredValue, width.DisplayValue) && !DoubleUtil.AreClose(width.DisplayValue, maxWidth))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, Math.Min(width.DesiredValue, maxWidth)));
					}
				}
			}
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0014852C File Offset: 0x0014672C
		private void RedistributeColumnWidthsOnNonStarWidthChange(DataGridColumn changedColumn, DataGridLength oldWidth)
		{
			DataGridLength width = changedColumn.Width;
			if (DoubleUtil.GreaterThan(width.DesiredValue, oldWidth.DisplayValue))
			{
				double num = this.TakeAwayWidthFromColumns(changedColumn, width.DesiredValue - oldWidth.DisplayValue, changedColumn != null);
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					changedColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, Math.Max(width.DisplayValue - num, changedColumn.MinWidth)));
					return;
				}
			}
			else if (DoubleUtil.LessThan(width.DesiredValue, oldWidth.DisplayValue))
			{
				double num2 = DataGridHelper.CoerceToMinMax(width.DesiredValue, changedColumn.MinWidth, changedColumn.MaxWidth);
				this.GiveAwayWidthToColumns(changedColumn, oldWidth.DisplayValue - num2);
			}
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x001485F8 File Offset: 0x001467F8
		private void DistributeSpaceAmongColumns(double availableSpace)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.MinWidth;
					num2 += dataGridColumn.MaxWidth;
					if (dataGridColumn.Width.IsStar)
					{
						num3 += dataGridColumn.MinWidth;
					}
				}
			}
			if (DoubleUtil.LessThan(availableSpace, num))
			{
				availableSpace = num;
			}
			if (DoubleUtil.GreaterThan(availableSpace, num2))
			{
				availableSpace = num2;
			}
			double num4 = this.DistributeSpaceAmongNonStarColumns(availableSpace - num3);
			this.ComputeStarColumnWidths(num3 + num4);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x001486C4 File Offset: 0x001468C4
		private double DistributeSpaceAmongNonStarColumns(double availableSpace)
		{
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				DataGridLength width = dataGridColumn.Width;
				if (dataGridColumn.IsVisible && !width.IsStar)
				{
					num += width.DisplayValue;
				}
			}
			if (DoubleUtil.LessThan(availableSpace, num))
			{
				double takeAwayWidth = num - availableSpace;
				this.TakeAwayWidthFromNonStarColumns(null, takeAwayWidth);
			}
			return Math.Max(availableSpace - num, 0.0);
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0014875C File Offset: 0x0014695C
		internal void OnColumnResizeStarted()
		{
			this._originalWidthsForResize = new Dictionary<DataGridColumn, DataGridLength>();
			foreach (DataGridColumn dataGridColumn in this)
			{
				this._originalWidthsForResize[dataGridColumn] = dataGridColumn.Width;
			}
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x001487BC File Offset: 0x001469BC
		internal void OnColumnResizeCompleted(bool cancel)
		{
			if (cancel && this._originalWidthsForResize != null)
			{
				foreach (DataGridColumn dataGridColumn in this)
				{
					if (this._originalWidthsForResize.ContainsKey(dataGridColumn))
					{
						dataGridColumn.Width = this._originalWidthsForResize[dataGridColumn];
					}
				}
			}
			this._originalWidthsForResize = null;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00148830 File Offset: 0x00146A30
		internal void RecomputeColumnWidthsOnColumnResize(DataGridColumn resizingColumn, double horizontalChange, bool retainAuto)
		{
			DataGridLength width = resizingColumn.Width;
			double value = width.DisplayValue + horizontalChange;
			if (DoubleUtil.LessThan(value, resizingColumn.MinWidth))
			{
				horizontalChange = resizingColumn.MinWidth - width.DisplayValue;
			}
			else if (DoubleUtil.GreaterThan(value, resizingColumn.MaxWidth))
			{
				horizontalChange = resizingColumn.MaxWidth - width.DisplayValue;
			}
			int displayIndex = resizingColumn.DisplayIndex;
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				this.RecomputeColumnWidthsOnColumnPositiveResize(horizontalChange, displayIndex, retainAuto);
				return;
			}
			if (DoubleUtil.LessThan(horizontalChange, 0.0))
			{
				this.RecomputeColumnWidthsOnColumnNegativeResize(-horizontalChange, displayIndex, retainAuto);
			}
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x001488CC File Offset: 0x00146ACC
		private void RecomputeColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double perStarWidth = 0.0;
			if (this.HasVisibleStarColumnsInternal(out perStarWidth))
			{
				horizontalChange = this.TakeAwayUnusedSpaceOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto, true);
				horizontalChange = this.RecomputeStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, perStarWidth, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnPositiveResize(horizontalChange, resizingColumnIndex, retainAuto, false);
				return;
			}
			DataGridColumn column = this.ColumnFromDisplayIndex(resizingColumnIndex);
			DataGridColumnCollection.SetResizedColumnWidth(column, horizontalChange, retainAuto);
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x00148930 File Offset: 0x00146B30
		private double RecomputeStarColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, double perStarWidth, bool retainAuto)
		{
			while (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double positiveInfinity = double.PositiveInfinity;
				double starFactorsForPositiveResize = this.GetStarFactorsForPositiveResize(resizingColumnIndex + 1, out positiveInfinity);
				if (!DoubleUtil.GreaterThan(starFactorsForPositiveResize, 0.0))
				{
					break;
				}
				horizontalChange = this.ReallocateStarValuesForPositiveResize(resizingColumnIndex, horizontalChange, positiveInfinity, starFactorsForPositiveResize, perStarWidth, retainAuto);
				if (DoubleUtil.AreClose(horizontalChange, 0.0))
				{
					break;
				}
			}
			return horizontalChange;
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00148998 File Offset: 0x00146B98
		private static bool CanColumnParticipateInResize(DataGridColumn column)
		{
			return column.IsVisible && column.CanUserResize;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x001489AC File Offset: 0x00146BAC
		private double GetStarFactorsForPositiveResize(int startIndex, out double minPerStarExcessRatio)
		{
			minPerStarExcessRatio = double.PositiveInfinity;
			double num = 0.0;
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && !DoubleUtil.AreClose(width.Value, 0.0) && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
					{
						num += width.Value;
						double num2 = (width.DisplayValue - dataGridColumn.MinWidth) / width.Value;
						if (DoubleUtil.LessThan(num2, minPerStarExcessRatio))
						{
							minPerStarExcessRatio = num2;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00148A68 File Offset: 0x00146C68
		private double ReallocateStarValuesForPositiveResize(int startIndex, double horizontalChange, double perStarExcessRatio, double totalStarFactors, double perStarWidth, bool retainAuto)
		{
			double num;
			double num2;
			if (DoubleUtil.LessThan(horizontalChange, perStarExcessRatio * totalStarFactors))
			{
				num = horizontalChange / totalStarFactors;
				num2 = horizontalChange;
				horizontalChange = 0.0;
			}
			else
			{
				num = perStarExcessRatio;
				num2 = num * totalStarFactors;
				horizontalChange -= num2;
			}
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				DataGridLength width = dataGridColumn.Width;
				if (i == startIndex)
				{
					DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, num2, retainAuto);
				}
				else if (dataGridColumn.Width.IsStar && DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn) && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
				{
					double num3 = width.DisplayValue - width.Value * num;
					dataGridColumn.UpdateWidthForStarColumn(Math.Max(num3, dataGridColumn.MinWidth), num3, num3 / perStarWidth);
				}
				i++;
			}
			return horizontalChange;
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x00148B50 File Offset: 0x00146D50
		private double RecomputeNonStarColumnWidthsOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto, bool onlyShrinkToDesiredWidth)
		{
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double num = 0.0;
				bool flag = true;
				int num2 = base.Count - 1;
				while (flag && num2 > resizingColumnIndex)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(num2);
					if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
					{
						DataGridLength width = dataGridColumn.Width;
						double num3 = onlyShrinkToDesiredWidth ? (width.DisplayValue - Math.Max(width.DesiredValue, dataGridColumn.MinWidth)) : (width.DisplayValue - dataGridColumn.MinWidth);
						if (!width.IsStar && DoubleUtil.GreaterThan(num3, 0.0))
						{
							if (DoubleUtil.GreaterThanOrClose(num + num3, horizontalChange))
							{
								num3 = horizontalChange - num;
								flag = false;
							}
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue - num3));
							num += num3;
						}
					}
					num2--;
				}
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					DataGridColumn column = this.ColumnFromDisplayIndex(resizingColumnIndex);
					DataGridColumnCollection.SetResizedColumnWidth(column, num, retainAuto);
					horizontalChange -= num;
				}
			}
			return horizontalChange;
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00148C6C File Offset: 0x00146E6C
		private void RecomputeColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double perStarWidth = 0.0;
			if (this.HasVisibleStarColumnsInternal(out perStarWidth))
			{
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, retainAuto, false);
				horizontalChange = this.RecomputeStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, perStarWidth, retainAuto);
				horizontalChange = this.RecomputeNonStarColumnWidthsOnColumnNegativeResize(horizontalChange, resizingColumnIndex, retainAuto, true);
				if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(resizingColumnIndex);
					if (!dataGridColumn.Width.IsStar)
					{
						DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, -horizontalChange, retainAuto);
						return;
					}
				}
			}
			else
			{
				DataGridColumn column = this.ColumnFromDisplayIndex(resizingColumnIndex);
				DataGridColumnCollection.SetResizedColumnWidth(column, -horizontalChange, retainAuto);
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00148CF8 File Offset: 0x00146EF8
		private double RecomputeNonStarColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, bool retainAuto, bool expandBeyondDesiredWidth)
		{
			if (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double num = 0.0;
				bool flag = true;
				int num2 = resizingColumnIndex + 1;
				int count = base.Count;
				while (flag && num2 < count)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(num2);
					if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
					{
						DataGridLength width = dataGridColumn.Width;
						double num3 = expandBeyondDesiredWidth ? dataGridColumn.MaxWidth : Math.Min(width.DesiredValue, dataGridColumn.MaxWidth);
						if (!width.IsStar && DoubleUtil.LessThan(width.DisplayValue, num3))
						{
							double num4 = num3 - width.DisplayValue;
							if (DoubleUtil.GreaterThanOrClose(num + num4, horizontalChange))
							{
								num4 = horizontalChange - num;
								flag = false;
							}
							dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue + num4));
							num += num4;
						}
					}
					num2++;
				}
				if (DoubleUtil.GreaterThan(num, 0.0))
				{
					DataGridColumn column = this.ColumnFromDisplayIndex(resizingColumnIndex);
					DataGridColumnCollection.SetResizedColumnWidth(column, -num, retainAuto);
					horizontalChange -= num;
				}
			}
			return horizontalChange;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x00148E18 File Offset: 0x00147018
		private double RecomputeStarColumnWidthsOnColumnNegativeResize(double horizontalChange, int resizingColumnIndex, double perStarWidth, bool retainAuto)
		{
			while (DoubleUtil.GreaterThan(horizontalChange, 0.0))
			{
				double positiveInfinity = double.PositiveInfinity;
				double starFactorsForNegativeResize = this.GetStarFactorsForNegativeResize(resizingColumnIndex + 1, out positiveInfinity);
				if (!DoubleUtil.GreaterThan(starFactorsForNegativeResize, 0.0))
				{
					break;
				}
				horizontalChange = this.ReallocateStarValuesForNegativeResize(resizingColumnIndex, horizontalChange, positiveInfinity, starFactorsForNegativeResize, perStarWidth, retainAuto);
				if (DoubleUtil.AreClose(horizontalChange, 0.0))
				{
					break;
				}
			}
			return horizontalChange;
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x00148E80 File Offset: 0x00147080
		private double GetStarFactorsForNegativeResize(int startIndex, out double minPerStarLagRatio)
		{
			minPerStarLagRatio = double.PositiveInfinity;
			double num = 0.0;
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				if (DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn))
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && !DoubleUtil.AreClose(width.Value, 0.0) && DoubleUtil.LessThan(width.DisplayValue, dataGridColumn.MaxWidth))
					{
						num += width.Value;
						double num2 = (dataGridColumn.MaxWidth - width.DisplayValue) / width.Value;
						if (DoubleUtil.LessThan(num2, minPerStarLagRatio))
						{
							minPerStarLagRatio = num2;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x00148F3C File Offset: 0x0014713C
		private double ReallocateStarValuesForNegativeResize(int startIndex, double horizontalChange, double perStarLagRatio, double totalStarFactors, double perStarWidth, bool retainAuto)
		{
			double num;
			double num2;
			if (DoubleUtil.LessThan(horizontalChange, perStarLagRatio * totalStarFactors))
			{
				num = horizontalChange / totalStarFactors;
				num2 = horizontalChange;
				horizontalChange = 0.0;
			}
			else
			{
				num = perStarLagRatio;
				num2 = num * totalStarFactors;
				horizontalChange -= num2;
			}
			int i = startIndex;
			int count = base.Count;
			while (i < count)
			{
				DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
				DataGridLength width = dataGridColumn.Width;
				if (i == startIndex)
				{
					DataGridColumnCollection.SetResizedColumnWidth(dataGridColumn, -num2, retainAuto);
				}
				else if (dataGridColumn.Width.IsStar && DataGridColumnCollection.CanColumnParticipateInResize(dataGridColumn) && DoubleUtil.LessThan(width.DisplayValue, dataGridColumn.MaxWidth))
				{
					double num3 = width.DisplayValue + width.Value * num;
					dataGridColumn.UpdateWidthForStarColumn(Math.Min(num3, dataGridColumn.MaxWidth), num3, num3 / perStarWidth);
				}
				i++;
			}
			return horizontalChange;
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x00149028 File Offset: 0x00147228
		private static void SetResizedColumnWidth(DataGridColumn column, double widthDelta, bool retainAuto)
		{
			DataGridLength width = column.Width;
			double num = DataGridHelper.CoerceToMinMax(width.DisplayValue + widthDelta, column.MinWidth, column.MaxWidth);
			if (width.IsStar)
			{
				double num2 = width.DesiredValue / width.Value;
				column.UpdateWidthForStarColumn(num, num, num / num2);
				return;
			}
			if (!width.IsAbsolute && retainAuto)
			{
				column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, num));
				return;
			}
			column.SetWidthInternal(new DataGridLength(num, DataGridLengthUnitType.Pixel, num, num));
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001490BB File Offset: 0x001472BB
		private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
		{
			return this.GiveAwayWidthToColumns(ignoredColumn, giveAwayWidth, false);
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x001490C8 File Offset: 0x001472C8
		private double GiveAwayWidthToColumns(DataGridColumn ignoredColumn, double giveAwayWidth, bool recomputeStars)
		{
			double num = giveAwayWidth;
			giveAwayWidth = this.GiveAwayWidthToScrollViewerExcess(giveAwayWidth, ignoredColumn != null);
			giveAwayWidth = this.GiveAwayWidthToNonStarColumns(ignoredColumn, giveAwayWidth);
			if (DoubleUtil.GreaterThan(giveAwayWidth, 0.0) || recomputeStars)
			{
				double num2 = 0.0;
				double num3 = 0.0;
				bool flag = false;
				foreach (DataGridColumn dataGridColumn in this)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && dataGridColumn.IsVisible)
					{
						if (dataGridColumn == ignoredColumn)
						{
							flag = true;
						}
						num2 += width.DisplayValue;
						num3 += dataGridColumn.MaxWidth;
					}
				}
				double num4 = num2;
				if (!flag)
				{
					num4 += giveAwayWidth;
				}
				else if (!DoubleUtil.AreClose(num, giveAwayWidth))
				{
					num4 -= num - giveAwayWidth;
				}
				double num5 = this.ComputeStarColumnWidths(Math.Min(num4, num3));
				giveAwayWidth = Math.Max(num5 - num4, 0.0);
			}
			return giveAwayWidth;
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x001491D0 File Offset: 0x001473D0
		private double GiveAwayWidthToNonStarColumns(DataGridColumn ignoredColumn, double giveAwayWidth)
		{
			while (DoubleUtil.GreaterThan(giveAwayWidth, 0.0))
			{
				int num = 0;
				double num2 = this.FindMinimumLaggingWidthOfNonStarColumns(ignoredColumn, out num);
				if (num == 0)
				{
					break;
				}
				double num3 = num2 * (double)num;
				if (DoubleUtil.GreaterThanOrClose(num3, giveAwayWidth))
				{
					num2 = giveAwayWidth / (double)num;
					giveAwayWidth = 0.0;
				}
				else
				{
					giveAwayWidth -= num3;
				}
				this.GiveAwayWidthToEveryNonStarColumn(ignoredColumn, num2);
			}
			return giveAwayWidth;
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00149230 File Offset: 0x00147430
		private double FindMinimumLaggingWidthOfNonStarColumns(DataGridColumn ignoredColumn, out int countOfParticipatingColumns)
		{
			double num = double.PositiveInfinity;
			countOfParticipatingColumns = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double maxWidth = dataGridColumn.MaxWidth;
						if (DoubleUtil.LessThan(width.DisplayValue, width.DesiredValue) && !DoubleUtil.AreClose(width.DisplayValue, maxWidth))
						{
							countOfParticipatingColumns++;
							double num2 = Math.Min(width.DesiredValue, maxWidth) - width.DisplayValue;
							if (DoubleUtil.LessThan(num2, num))
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x001492F8 File Offset: 0x001474F8
		private void GiveAwayWidthToEveryNonStarColumn(DataGridColumn ignoredColumn, double perColumnGiveAwayWidth)
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar && DoubleUtil.LessThan(width.DisplayValue, Math.Min(width.DesiredValue, dataGridColumn.MaxWidth)))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue + perColumnGiveAwayWidth));
					}
				}
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001493A0 File Offset: 0x001475A0
		private double GiveAwayWidthToScrollViewerExcess(double giveAwayWidth, bool includedInColumnsWidth)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.Width.DisplayValue;
				}
			}
			if (includedInColumnsWidth)
			{
				if (DoubleUtil.GreaterThan(num, viewportWidthForColumns))
				{
					double val = num - viewportWidthForColumns;
					giveAwayWidth -= Math.Min(val, giveAwayWidth);
				}
			}
			else
			{
				giveAwayWidth = Math.Min(giveAwayWidth, Math.Max(0.0, viewportWidthForColumns - num));
			}
			return giveAwayWidth;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x0014944C File Offset: 0x0014764C
		private double TakeAwayUnusedSpaceOnColumnPositiveResize(double horizontalChange, int resizingColumnIndex, bool retainAuto)
		{
			double num = this.TakeAwayWidthFromUnusedSpace(false, horizontalChange);
			if (DoubleUtil.LessThan(num, horizontalChange))
			{
				DataGridColumn column = this.ColumnFromDisplayIndex(resizingColumnIndex);
				DataGridColumnCollection.SetResizedColumnWidth(column, horizontalChange - num, retainAuto);
			}
			return num;
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00149480 File Offset: 0x00147680
		private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth, double totalAvailableWidth)
		{
			double num = 0.0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (dataGridColumn.IsVisible)
				{
					num += dataGridColumn.Width.DisplayValue;
				}
			}
			if (!spaceAlreadyUtilized)
			{
				double num2 = totalAvailableWidth - num;
				if (DoubleUtil.GreaterThan(num2, 0.0))
				{
					takeAwayWidth = Math.Max(0.0, takeAwayWidth - num2);
				}
				return takeAwayWidth;
			}
			if (DoubleUtil.GreaterThanOrClose(totalAvailableWidth, num))
			{
				return 0.0;
			}
			return Math.Min(num - totalAvailableWidth, takeAwayWidth);
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00149530 File Offset: 0x00147730
		private double TakeAwayWidthFromUnusedSpace(bool spaceAlreadyUtilized, double takeAwayWidth)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			if (DoubleUtil.GreaterThan(viewportWidthForColumns, 0.0))
			{
				return this.TakeAwayWidthFromUnusedSpace(spaceAlreadyUtilized, takeAwayWidth, viewportWidthForColumns);
			}
			return takeAwayWidth;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00149568 File Offset: 0x00147768
		private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized)
		{
			double viewportWidthForColumns = this.DataGridOwner.GetViewportWidthForColumns();
			return this.TakeAwayWidthFromColumns(ignoredColumn, takeAwayWidth, widthAlreadyUtilized, viewportWidthForColumns);
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0014958B File Offset: 0x0014778B
		private double TakeAwayWidthFromColumns(DataGridColumn ignoredColumn, double takeAwayWidth, bool widthAlreadyUtilized, double totalAvailableWidth)
		{
			takeAwayWidth = this.TakeAwayWidthFromUnusedSpace(widthAlreadyUtilized, takeAwayWidth, totalAvailableWidth);
			takeAwayWidth = this.TakeAwayWidthFromStarColumns(ignoredColumn, takeAwayWidth);
			takeAwayWidth = this.TakeAwayWidthFromNonStarColumns(ignoredColumn, takeAwayWidth);
			return takeAwayWidth;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x001495B0 File Offset: 0x001477B0
		private double TakeAwayWidthFromStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
		{
			if (DoubleUtil.GreaterThan(takeAwayWidth, 0.0))
			{
				double num = 0.0;
				double num2 = 0.0;
				foreach (DataGridColumn dataGridColumn in this)
				{
					DataGridLength width = dataGridColumn.Width;
					if (width.IsStar && dataGridColumn.IsVisible)
					{
						if (dataGridColumn == ignoredColumn)
						{
							num += takeAwayWidth;
						}
						num += width.DisplayValue;
						num2 += dataGridColumn.MinWidth;
					}
				}
				double num3 = num - takeAwayWidth;
				double num4 = this.ComputeStarColumnWidths(Math.Max(num3, num2));
				takeAwayWidth = Math.Max(num4 - num3, 0.0);
			}
			return takeAwayWidth;
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00149680 File Offset: 0x00147880
		private double TakeAwayWidthFromNonStarColumns(DataGridColumn ignoredColumn, double takeAwayWidth)
		{
			while (DoubleUtil.GreaterThan(takeAwayWidth, 0.0))
			{
				int num = 0;
				double num2 = this.FindMinimumExcessWidthOfNonStarColumns(ignoredColumn, out num);
				if (num == 0)
				{
					break;
				}
				double num3 = num2 * (double)num;
				if (DoubleUtil.GreaterThanOrClose(num3, takeAwayWidth))
				{
					num2 = takeAwayWidth / (double)num;
					takeAwayWidth = 0.0;
				}
				else
				{
					takeAwayWidth -= num3;
				}
				this.TakeAwayWidthFromEveryNonStarColumn(ignoredColumn, num2);
			}
			return takeAwayWidth;
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x001496E0 File Offset: 0x001478E0
		private double FindMinimumExcessWidthOfNonStarColumns(DataGridColumn ignoredColumn, out int countOfParticipatingColumns)
		{
			double num = double.PositiveInfinity;
			countOfParticipatingColumns = 0;
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar)
					{
						double minWidth = dataGridColumn.MinWidth;
						if (DoubleUtil.GreaterThan(width.DisplayValue, minWidth))
						{
							countOfParticipatingColumns++;
							double num2 = width.DisplayValue - minWidth;
							if (DoubleUtil.LessThan(num2, num))
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00149784 File Offset: 0x00147984
		private void TakeAwayWidthFromEveryNonStarColumn(DataGridColumn ignoredColumn, double perColumnTakeAwayWidth)
		{
			foreach (DataGridColumn dataGridColumn in this)
			{
				if (ignoredColumn != dataGridColumn && dataGridColumn.IsVisible)
				{
					DataGridLength width = dataGridColumn.Width;
					if (!width.IsStar && DoubleUtil.GreaterThan(width.DisplayValue, dataGridColumn.MinWidth))
					{
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.DisplayValue - perColumnTakeAwayWidth));
					}
				}
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06004861 RID: 18529 RVA: 0x00149820 File Offset: 0x00147A20
		// (set) Token: 0x06004862 RID: 18530 RVA: 0x00149828 File Offset: 0x00147A28
		internal bool RebuildRealizedColumnsBlockListForNonVirtualizedRows { get; set; }

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x00149831 File Offset: 0x00147A31
		// (set) Token: 0x06004864 RID: 18532 RVA: 0x0014983C File Offset: 0x00147A3C
		internal List<RealizedColumnsBlock> RealizedColumnsBlockListForNonVirtualizedRows
		{
			get
			{
				return this._realizedColumnsBlockListForNonVirtualizedRows;
			}
			set
			{
				this._realizedColumnsBlockListForNonVirtualizedRows = value;
				DataGrid dataGridOwner = this.DataGridOwner;
				dataGridOwner.NotifyPropertyChanged(dataGridOwner, "RealizedColumnsBlockListForNonVirtualizedRows", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x0014986E File Offset: 0x00147A6E
		// (set) Token: 0x06004866 RID: 18534 RVA: 0x00149876 File Offset: 0x00147A76
		internal List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows { get; set; }

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x0014987F File Offset: 0x00147A7F
		// (set) Token: 0x06004868 RID: 18536 RVA: 0x00149887 File Offset: 0x00147A87
		internal bool RebuildRealizedColumnsBlockListForVirtualizedRows { get; set; }

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06004869 RID: 18537 RVA: 0x00149890 File Offset: 0x00147A90
		// (set) Token: 0x0600486A RID: 18538 RVA: 0x00149898 File Offset: 0x00147A98
		internal List<RealizedColumnsBlock> RealizedColumnsBlockListForVirtualizedRows
		{
			get
			{
				return this._realizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				this._realizedColumnsBlockListForVirtualizedRows = value;
				DataGrid dataGridOwner = this.DataGridOwner;
				dataGridOwner.NotifyPropertyChanged(dataGridOwner, "RealizedColumnsBlockListForVirtualizedRows", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x0600486B RID: 18539 RVA: 0x001498CA File Offset: 0x00147ACA
		// (set) Token: 0x0600486C RID: 18540 RVA: 0x001498D2 File Offset: 0x00147AD2
		internal List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockListForVirtualizedRows { get; set; }

		// Token: 0x0600486D RID: 18541 RVA: 0x001498DB File Offset: 0x00147ADB
		internal void InvalidateColumnRealization(bool invalidateForNonVirtualizedRows)
		{
			this.RebuildRealizedColumnsBlockListForVirtualizedRows = true;
			if (invalidateForNonVirtualizedRows)
			{
				this.RebuildRealizedColumnsBlockListForNonVirtualizedRows = true;
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x0600486E RID: 18542 RVA: 0x001498F0 File Offset: 0x00147AF0
		internal int FirstVisibleDisplayIndex
		{
			get
			{
				int i = 0;
				int count = base.Count;
				while (i < count)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						return i;
					}
					i++;
				}
				return -1;
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x0600486F RID: 18543 RVA: 0x00149924 File Offset: 0x00147B24
		internal int LastVisibleDisplayIndex
		{
			get
			{
				for (int i = base.Count - 1; i >= 0; i--)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x00149957 File Offset: 0x00147B57
		// (set) Token: 0x06004871 RID: 18545 RVA: 0x0014995F File Offset: 0x00147B5F
		internal bool RefreshAutoWidthColumns { get; set; }

		// Token: 0x0400298E RID: 10638
		private DataGrid _dataGridOwner;

		// Token: 0x0400298F RID: 10639
		private bool _isUpdatingDisplayIndex;

		// Token: 0x04002990 RID: 10640
		private List<int> _displayIndexMap;

		// Token: 0x04002991 RID: 10641
		private bool _displayIndexMapInitialized;

		// Token: 0x04002992 RID: 10642
		private bool _isClearingDisplayIndex;

		// Token: 0x04002993 RID: 10643
		private bool _columnWidthsComputationPending;

		// Token: 0x04002994 RID: 10644
		private Dictionary<DataGridColumn, DataGridLength> _originalWidthsForResize;

		// Token: 0x04002995 RID: 10645
		private double? _averageColumnWidth;

		// Token: 0x04002996 RID: 10646
		private List<RealizedColumnsBlock> _realizedColumnsBlockListForNonVirtualizedRows;

		// Token: 0x04002997 RID: 10647
		private List<RealizedColumnsBlock> _realizedColumnsBlockListForVirtualizedRows;

		// Token: 0x04002998 RID: 10648
		private bool _hasVisibleStarColumns;
	}
}
