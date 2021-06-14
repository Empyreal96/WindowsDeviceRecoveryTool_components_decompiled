using System;

namespace System.Windows.Controls
{
	/// <summary>Represents information about a specific cell in a <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
	// Token: 0x02000499 RID: 1177
	public struct DataGridCellInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellInfo" /> structure using the specified data item and column.</summary>
		/// <param name="item">The data item for the row that contains the cell.</param>
		/// <param name="column">The column that contains the cell.</param>
		// Token: 0x0600472A RID: 18218 RVA: 0x00142806 File Offset: 0x00140A06
		public DataGridCellInfo(object item, DataGridColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			this._info = new ItemsControl.ItemInfo(item, null, -1);
			this._column = column;
			this._owner = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellInfo" /> structure for the specified cell.</summary>
		/// <param name="cell">The cell for which information is to be generated.</param>
		// Token: 0x0600472B RID: 18219 RVA: 0x00142834 File Offset: 0x00140A34
		public DataGridCellInfo(DataGridCell cell)
		{
			if (cell == null)
			{
				throw new ArgumentNullException("cell");
			}
			DataGrid dataGridOwner = cell.DataGridOwner;
			this._info = dataGridOwner.NewItemInfo(cell.RowDataItem, cell.RowOwner, -1);
			this._column = cell.Column;
			this._owner = new WeakReference(dataGridOwner);
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x00142887 File Offset: 0x00140A87
		internal DataGridCellInfo(object item, DataGridColumn column, DataGrid owner)
		{
			this._info = owner.NewItemInfo(item, null, -1);
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x001428AB File Offset: 0x00140AAB
		internal DataGridCellInfo(ItemsControl.ItemInfo info, DataGridColumn column, DataGrid owner)
		{
			this._info = info;
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x001428C7 File Offset: 0x00140AC7
		internal DataGridCellInfo(object item)
		{
			this._info = new ItemsControl.ItemInfo(item, null, -1);
			this._column = null;
			this._owner = null;
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x001428E5 File Offset: 0x00140AE5
		internal DataGridCellInfo(DataGridCellInfo info)
		{
			this._info = info._info.Clone();
			this._column = info._column;
			this._owner = info._owner;
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x00142910 File Offset: 0x00140B10
		private DataGridCellInfo(DataGrid owner, DataGridColumn column, object item)
		{
			this._info = owner.NewItemInfo(item, null, -1);
			this._column = column;
			this._owner = new WeakReference(owner);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x00142934 File Offset: 0x00140B34
		internal static DataGridCellInfo CreatePossiblyPartialCellInfo(object item, DataGridColumn column, DataGrid owner)
		{
			if (item == null && column == null)
			{
				return DataGridCellInfo.Unset;
			}
			return new DataGridCellInfo(owner, column, (item == null) ? DependencyProperty.UnsetValue : item);
		}

		/// <summary>Gets the data item for the row that contains the cell.</summary>
		/// <returns>The data item for the row that contains the cell.</returns>
		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x06004732 RID: 18226 RVA: 0x00142954 File Offset: 0x00140B54
		public object Item
		{
			get
			{
				if (!(this._info != null))
				{
					return null;
				}
				return this._info.Item;
			}
		}

		/// <summary>Gets the column that contains the cell.</summary>
		/// <returns>The column that contains the cell.</returns>
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x06004733 RID: 18227 RVA: 0x00142971 File Offset: 0x00140B71
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		/// <summary>Indicates whether the specified object is equal to the current instance.</summary>
		/// <param name="obj">The object to compare to the current instance.</param>
		/// <returns>
		///     <see langword="true" /> if the comparison object represents the same cell; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004734 RID: 18228 RVA: 0x00142979 File Offset: 0x00140B79
		public override bool Equals(object obj)
		{
			return obj is DataGridCellInfo && this.EqualsImpl((DataGridCellInfo)obj);
		}

		/// <summary>Indicates whether two <see cref="T:System.Windows.Controls.DataGridCellInfo" /> instances are equal.</summary>
		/// <param name="cell1">The first structure to compare.</param>
		/// <param name="cell2">The second structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two structures represent the same cell; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004735 RID: 18229 RVA: 0x00142991 File Offset: 0x00140B91
		public static bool operator ==(DataGridCellInfo cell1, DataGridCellInfo cell2)
		{
			return cell1.EqualsImpl(cell2);
		}

		/// <summary>Indicates whether two <see cref="T:System.Windows.Controls.DataGridCellInfo" /> instances are not equal.</summary>
		/// <param name="cell1">The first structure to compare.</param>
		/// <param name="cell2">The second structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two structures do not represent the same cell; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004736 RID: 18230 RVA: 0x0014299B File Offset: 0x00140B9B
		public static bool operator !=(DataGridCellInfo cell1, DataGridCellInfo cell2)
		{
			return !cell1.EqualsImpl(cell2);
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x001429A8 File Offset: 0x00140BA8
		internal bool EqualsImpl(DataGridCellInfo cell)
		{
			return cell._column == this._column && cell.Owner == this.Owner && cell._info == this._info;
		}

		/// <summary>Returns a hash code for the current <see cref="T:System.Windows.Controls.DataGridCellInfo" /> structure.</summary>
		/// <returns>A hash code for the structure.</returns>
		// Token: 0x06004738 RID: 18232 RVA: 0x001429DA File Offset: 0x00140BDA
		public override int GetHashCode()
		{
			return ((this._info == null) ? 0 : this._info.GetHashCode()) ^ ((this._column == null) ? 0 : this._column.GetHashCode());
		}

		/// <summary>Gets a value that indicates whether the structure holds valid information.</summary>
		/// <returns>
		///     <see langword="true" /> if the structure has valid information; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x00142A0F File Offset: 0x00140C0F
		public bool IsValid
		{
			get
			{
				return this.ArePropertyValuesValid;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x0600473A RID: 18234 RVA: 0x00142A17 File Offset: 0x00140C17
		internal bool IsSet
		{
			get
			{
				return this._column != null || this._info.Item != DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x00142A38 File Offset: 0x00140C38
		internal ItemsControl.ItemInfo ItemInfo
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x00142A40 File Offset: 0x00140C40
		internal bool IsValidForDataGrid(DataGrid dataGrid)
		{
			DataGrid owner = this.Owner;
			return (this.ArePropertyValuesValid && owner == dataGrid) || owner == null;
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x0600473D RID: 18237 RVA: 0x00142A66 File Offset: 0x00140C66
		private bool ArePropertyValuesValid
		{
			get
			{
				return this.Item != DependencyProperty.UnsetValue && this._column != null;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x0600473E RID: 18238 RVA: 0x00142A80 File Offset: 0x00140C80
		internal static DataGridCellInfo Unset
		{
			get
			{
				return new DataGridCellInfo(DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x00142A8C File Offset: 0x00140C8C
		private DataGrid Owner
		{
			get
			{
				if (this._owner != null)
				{
					return (DataGrid)this._owner.Target;
				}
				return null;
			}
		}

		// Token: 0x04002953 RID: 10579
		private ItemsControl.ItemInfo _info;

		// Token: 0x04002954 RID: 10580
		private DataGridColumn _column;

		// Token: 0x04002955 RID: 10581
		private WeakReference _owner;
	}
}
