using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of cells in a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</summary>
	// Token: 0x02000195 RID: 405
	[ListBindable(false)]
	public class DataGridViewCellCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Adds an item to the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewCell" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <paramref name="value" /> represents a cell that already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x06001AB6 RID: 6838 RVA: 0x00086536 File Offset: 0x00084736
		int IList.Add(object value)
		{
			return this.Add((DataGridViewCell)value);
		}

		/// <summary>Clears the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001AB7 RID: 6839 RVA: 0x00086544 File Offset: 0x00084744
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Determines whether the collection contains the specified value.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="value" /> is found in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001AB8 RID: 6840 RVA: 0x0008654C File Offset: 0x0008474C
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Determines the index of a specific item in a collection.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />.</param>
		/// <returns>The index of value if found in the list; otherwise, -1.</returns>
		// Token: 0x06001AB9 RID: 6841 RVA: 0x0008655A File Offset: 0x0008475A
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Inserts an item into the collection at the specified position.</summary>
		/// <param name="index">The zero-based index at which value should be inserted. </param>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to insert into the <see cref="M:System.Windows.Forms.DataGridViewCellCollection.System#Collections#IList#Insert(System.Int32,System.Object)" />.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewCell" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <paramref name="dataGridViewCell" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x06001ABA RID: 6842 RVA: 0x00086568 File Offset: 0x00084768
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (DataGridViewCell)value);
		}

		/// <summary>Removes the first occurrence of a specific object from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to remove from the collection.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewCell" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="cell" /> could not be found in the collection.</exception>
		// Token: 0x06001ABB RID: 6843 RVA: 0x00086577 File Offset: 0x00084777
		void IList.Remove(object value)
		{
			this.Remove((DataGridViewCell)value);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.DataGridViewCell" /> at the specified index.</summary>
		/// <param name="index">The position at which to remove the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</param>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001ABC RID: 6844 RVA: 0x00086585 File Offset: 0x00084785
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The index of the item to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> at the specified index.</returns>
		/// <exception cref="T:System.InvalidCastException">The specified value when setting this property is not a <see cref="T:System.Windows.Forms.DataGridViewCell" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of cells in the collection.</exception>
		// Token: 0x1700060F RID: 1551
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (DataGridViewCell)value;
			}
		}

		/// <summary>Copies the elements of the collection to the specified array, starting at the specified index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001AC1 RID: 6849 RVA: 0x000865A6 File Offset: 0x000847A6
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />.</returns>
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000865B5 File Offset: 0x000847B5
		int ICollection.Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />.</returns>
		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06001AC5 RID: 6853 RVA: 0x000865C2 File Offset: 0x000847C2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> class.</summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> that owns the collection.</param>
		// Token: 0x06001AC6 RID: 6854 RVA: 0x000865CF File Offset: 0x000847CF
		public DataGridViewCellCollection(DataGridViewRow dataGridViewRow)
		{
			this.owner = dataGridViewRow;
		}

		/// <summary>Gets an <see cref="T:System.Collections.ArrayList" /> containing <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> objects.</summary>
		/// <returns>
		///     <see cref="T:System.Collections.ArrayList" />.</returns>
		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x000865E9 File Offset: 0x000847E9
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets or sets the cell at the provided index location. In C#, this property is the indexer for the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> class.</summary>
		/// <param name="index">The zero-based index of the cell to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> stored at the given index.</returns>
		/// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of cells in the collection.</exception>
		// Token: 0x17000614 RID: 1556
		public DataGridViewCell this[int index]
		{
			get
			{
				return (DataGridViewCell)this.items[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.DataGridView != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridView"));
				}
				if (value.OwningRow != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
				}
				if (this.owner.DataGridView != null)
				{
					this.owner.DataGridView.OnReplacingCell(this.owner, index);
				}
				DataGridViewCell dataGridViewCell = (DataGridViewCell)this.items[index];
				this.items[index] = value;
				value.OwningRowInternal = this.owner;
				value.StateInternal = dataGridViewCell.State;
				if (this.owner.DataGridView != null)
				{
					value.DataGridViewInternal = this.owner.DataGridView;
					value.OwningColumnInternal = this.owner.DataGridView.Columns[index];
					this.owner.DataGridView.OnReplacedCell(this.owner, index);
				}
				dataGridViewCell.DataGridViewInternal = null;
				dataGridViewCell.OwningRowInternal = null;
				dataGridViewCell.OwningColumnInternal = null;
				if (dataGridViewCell.ReadOnly)
				{
					dataGridViewCell.ReadOnlyInternal = false;
				}
				if (dataGridViewCell.Selected)
				{
					dataGridViewCell.SelectedInternal = false;
				}
			}
		}

		/// <summary>Gets or sets the cell in the column with the provided name. In C#, this property is the indexer for the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> class.</summary>
		/// <param name="columnName">The name of the column in which to get or set the cell.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> stored in the column with the given name.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="columnName" /> does not match the name of any columns in the control.</exception>
		/// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-The specified cell when setting this property already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x17000615 RID: 1557
		public DataGridViewCell this[string columnName]
		{
			get
			{
				DataGridViewColumn dataGridViewColumn = null;
				if (this.owner.DataGridView != null)
				{
					dataGridViewColumn = this.owner.DataGridView.Columns[columnName];
				}
				if (dataGridViewColumn == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewColumnCollection_ColumnNotFound", new object[]
					{
						columnName
					}), "columnName");
				}
				return (DataGridViewCell)this.items[dataGridViewColumn.Index];
			}
			set
			{
				DataGridViewColumn dataGridViewColumn = null;
				if (this.owner.DataGridView != null)
				{
					dataGridViewColumn = this.owner.DataGridView.Columns[columnName];
				}
				if (dataGridViewColumn == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewColumnCollection_ColumnNotFound", new object[]
					{
						columnName
					}), "columnName");
				}
				this[dataGridViewColumn.Index] = value;
			}
		}

		/// <summary>Occurs when the collection is changed. </summary>
		// Token: 0x14000168 RID: 360
		// (add) Token: 0x06001ACC RID: 6860 RVA: 0x000867FE File Offset: 0x000849FE
		// (remove) Token: 0x06001ACD RID: 6861 RVA: 0x00086817 File Offset: 0x00084A17
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		/// <summary>Adds a cell to the collection.</summary>
		/// <param name="dataGridViewCell">A <see cref="T:System.Windows.Forms.DataGridViewCell" /> to add to the collection.</param>
		/// <returns>The position in which to insert the new element.</returns>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <paramref name="dataGridViewCell" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x06001ACE RID: 6862 RVA: 0x00086830 File Offset: 0x00084A30
		public virtual int Add(DataGridViewCell dataGridViewCell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			if (dataGridViewCell.OwningRow != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
			}
			return this.AddInternal(dataGridViewCell);
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x00086870 File Offset: 0x00084A70
		internal int AddInternal(DataGridViewCell dataGridViewCell)
		{
			int num = this.items.Add(dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			DataGridView dataGridView = this.owner.DataGridView;
			if (dataGridView != null && dataGridView.Columns.Count > num)
			{
				dataGridViewCell.OwningColumnInternal = dataGridView.Columns[num];
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
			return num;
		}

		/// <summary>Adds an array of cells to the collection.</summary>
		/// <param name="dataGridViewCells">The array of <see cref="T:System.Windows.Forms.DataGridViewCell" /> objects to add to the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewCells" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-At least one value in <paramref name="dataGridViewCells" /> is <see langword="null" />.-or-At least one cell in <paramref name="dataGridViewCells" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.-or-At least two values in <paramref name="dataGridViewCells" /> are references to the same <see cref="T:System.Windows.Forms.DataGridViewCell" />.</exception>
		// Token: 0x06001AD0 RID: 6864 RVA: 0x000868D4 File Offset: 0x00084AD4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual void AddRange(params DataGridViewCell[] dataGridViewCells)
		{
			if (dataGridViewCells == null)
			{
				throw new ArgumentNullException("dataGridViewCells");
			}
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			foreach (DataGridViewCell dataGridViewCell in dataGridViewCells)
			{
				if (dataGridViewCell == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_AtLeastOneCellIsNull"));
				}
				if (dataGridViewCell.OwningRow != null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
				}
			}
			int num = dataGridViewCells.Length;
			for (int j = 0; j < num - 1; j++)
			{
				for (int k = j + 1; k < num; k++)
				{
					if (dataGridViewCells[j] == dataGridViewCells[k])
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CannotAddIdenticalCells"));
					}
				}
			}
			this.items.AddRange(dataGridViewCells);
			foreach (DataGridViewCell dataGridViewCell2 in dataGridViewCells)
			{
				dataGridViewCell2.OwningRowInternal = this.owner;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Clears all cells from the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001AD1 RID: 6865 RVA: 0x000869D4 File Offset: 0x00084BD4
		public virtual void Clear()
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			foreach (object obj in this.items)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.OwningRowInternal = null;
			}
			this.items.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Copies the entire collection of cells into an array at a specified location within the array.</summary>
		/// <param name="array">The destination array to which the contents will be copied.</param>
		/// <param name="index">The index of the element in <paramref name="array" /> at which to start copying.</param>
		// Token: 0x06001AD2 RID: 6866 RVA: 0x000865A6 File Offset: 0x000847A6
		public void CopyTo(DataGridViewCell[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Determines whether the specified cell is contained in the collection.</summary>
		/// <param name="dataGridViewCell">A <see cref="T:System.Windows.Forms.DataGridViewCell" /> to locate in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="dataGridViewCell" /> is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001AD3 RID: 6867 RVA: 0x00086A64 File Offset: 0x00084C64
		public virtual bool Contains(DataGridViewCell dataGridViewCell)
		{
			int num = this.items.IndexOf(dataGridViewCell);
			return num != -1;
		}

		/// <summary>Returns the index of the specified cell.</summary>
		/// <param name="dataGridViewCell">The cell to locate in the collection.</param>
		/// <returns>The zero-based index of the value of <paramref name="dataGridViewCell" /> parameter, if it is found in the collection; otherwise, -1.</returns>
		// Token: 0x06001AD4 RID: 6868 RVA: 0x0008655A File Offset: 0x0008475A
		public int IndexOf(DataGridViewCell dataGridViewCell)
		{
			return this.items.IndexOf(dataGridViewCell);
		}

		/// <summary>Inserts a cell into the collection at the specified index. </summary>
		/// <param name="index">The zero-based index at which to place <paramref name="dataGridViewCell" />.</param>
		/// <param name="dataGridViewCell">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to insert.</param>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.-or-
		///         <paramref name="dataGridViewCell" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		// Token: 0x06001AD5 RID: 6869 RVA: 0x00086A88 File Offset: 0x00084C88
		public virtual void Insert(int index, DataGridViewCell dataGridViewCell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			if (dataGridViewCell.OwningRow != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow"));
			}
			this.items.Insert(index, dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00086AF0 File Offset: 0x00084CF0
		internal void InsertInternal(int index, DataGridViewCell dataGridViewCell)
		{
			this.items.Insert(index, dataGridViewCell);
			dataGridViewCell.OwningRowInternal = this.owner;
			DataGridView dataGridView = this.owner.DataGridView;
			if (dataGridView != null && dataGridView.Columns.Count > index)
			{
				dataGridViewCell.OwningColumnInternal = dataGridView.Columns[index];
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewCell));
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridViewCellCollection.CollectionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data. </param>
		// Token: 0x06001AD7 RID: 6871 RVA: 0x00086B52 File Offset: 0x00084D52
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		/// <summary>Removes the specified cell from the collection.</summary>
		/// <param name="cell">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to remove from the collection.</param>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="cell" /> could not be found in the collection.</exception>
		// Token: 0x06001AD8 RID: 6872 RVA: 0x00086B6C File Offset: 0x00084D6C
		public virtual void Remove(DataGridViewCell cell)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			int num = -1;
			int count = this.items.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.items[i] == cell)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridViewCellCollection_CellNotFound"));
			}
			this.RemoveAt(num);
		}

		/// <summary>Removes the cell at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> to be removed.</param>
		/// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> already belongs to a <see cref="T:System.Windows.Forms.DataGridView" /> control.</exception>
		// Token: 0x06001AD9 RID: 6873 RVA: 0x00086BDE File Offset: 0x00084DDE
		public virtual void RemoveAt(int index)
		{
			if (this.owner.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView"));
			}
			this.RemoveAtInternal(index);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00086C04 File Offset: 0x00084E04
		internal void RemoveAtInternal(int index)
		{
			DataGridViewCell dataGridViewCell = (DataGridViewCell)this.items[index];
			this.items.RemoveAt(index);
			dataGridViewCell.DataGridViewInternal = null;
			dataGridViewCell.OwningRowInternal = null;
			if (dataGridViewCell.ReadOnly)
			{
				dataGridViewCell.ReadOnlyInternal = false;
			}
			if (dataGridViewCell.Selected)
			{
				dataGridViewCell.SelectedInternal = false;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridViewCell));
		}

		// Token: 0x04000C04 RID: 3076
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04000C05 RID: 3077
		private ArrayList items = new ArrayList();

		// Token: 0x04000C06 RID: 3078
		private DataGridViewRow owner;
	}
}
