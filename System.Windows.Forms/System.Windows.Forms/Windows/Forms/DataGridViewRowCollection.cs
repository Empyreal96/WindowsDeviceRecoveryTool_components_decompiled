using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>A collection of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects.</summary>
	// Token: 0x020001F9 RID: 505
	[ListBindable(false)]
	[DesignerSerializer("System.Windows.Forms.Design.DataGridViewRowCollectionCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataGridViewRowCollection : ICollection, IEnumerable, IList
	{
		/// <summary>Adds a <see cref="T:System.Windows.Forms.DataGridViewRow" /> to the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of the new <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-The <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the <paramref name="value" /> is not <see langword="null" />.-or-
		///         <paramref name="value" /> has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />.-or-This operation would add a frozen row after unfrozen rows. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> has more cells than there are columns in the control.</exception>
		// Token: 0x06001EC0 RID: 7872 RVA: 0x00099AED File Offset: 0x00097CED
		int IList.Add(object value)
		{
			return this.Add((DataGridViewRow)value);
		}

		/// <summary>Removes all items from the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The collection is data bound and the underlying data source does not support clearing the row data.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           </exception>
		// Token: 0x06001EC1 RID: 7873 RVA: 0x00099AFB File Offset: 0x00097CFB
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Determines whether the collection contains the specified item.</summary>
		/// <param name="value">The item to locate in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.Windows.Forms.DataGridViewRow" /> found in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001EC2 RID: 7874 RVA: 0x00099B03 File Offset: 0x00097D03
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the index of a specified item in the collection.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if it is a <see cref="T:System.Windows.Forms.DataGridViewRow" /> found in the list; otherwise, -1.</returns>
		// Token: 0x06001EC3 RID: 7875 RVA: 0x00099B11 File Offset: 0x00097D11
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Forms.DataGridViewRow" /> into the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to insert into the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero or greater than the number of rows in the collection. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-
		///         <paramref name="index" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-The <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the <paramref name="value" /> is not <see langword="null" />.-or-
		///         <paramref name="value" /> has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />.-or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> has more cells than there are columns in the control.</exception>
		// Token: 0x06001EC4 RID: 7876 RVA: 0x00099B1F File Offset: 0x00097D1F
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (DataGridViewRow)value);
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to remove from the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.DataGridViewRow" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not contained in this collection.-or-
		///         <paramref name="value" /> is a shared row.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="value" /> is the row for new records.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is bound to an <see cref="T:System.ComponentModel.IBindingList" /> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove" /> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification" /> property values that are not both <see langword="true" />. </exception>
		// Token: 0x06001EC5 RID: 7877 RVA: 0x00099B2E File Offset: 0x00097D2E
		void IList.Remove(object value)
		{
			this.Remove((DataGridViewRow)value);
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.DataGridViewRow" /> from the collection at the specified position.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero and greater than the number of rows in the collection minus one. </exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="index" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is bound to an <see cref="T:System.ComponentModel.IBindingList" /> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove" /> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification" /> property values that are not both <see langword="true" />.</exception>
		// Token: 0x06001EC6 RID: 7878 RVA: 0x00099B3C File Offset: 0x00097D3C
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">The user tried to set this property.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.- or -
		///         <paramref name="index" /> is equal to or greater than <see cref="P:System.Windows.Forms.DataGridViewRowCollection.Count" />.</exception>
		// Token: 0x17000744 RID: 1860
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> cannot be cast automatically to the type of <paramref name="array" />. </exception>
		// Token: 0x06001ECB RID: 7883 RVA: 0x00099B4E File Offset: 0x00097D4E
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</returns>
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x00099B5D File Offset: 0x00097D5D
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</returns>
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06001ECF RID: 7887 RVA: 0x00099B65 File Offset: 0x00097D65
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewRowCollection.UnsharingRowEnumerator(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> class. </summary>
		/// <param name="dataGridView">The <see cref="T:System.Windows.Forms.DataGridView" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		// Token: 0x06001ED0 RID: 7888 RVA: 0x00099B6D File Offset: 0x00097D6D
		public DataGridViewRowCollection(DataGridView dataGridView)
		{
			this.InvalidateCachedRowCounts();
			this.InvalidateCachedRowsHeights();
			this.dataGridView = dataGridView;
			this.rowStates = new List<DataGridViewElementStates>();
			this.items = new DataGridViewRowCollection.RowArrayList(this);
		}

		/// <summary>Gets the number of rows in the collection.</summary>
		/// <returns>The number of rows in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</returns>
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x00099B9F File Offset: 0x00097D9F
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x00099BAC File Offset: 0x00097DAC
		internal bool IsCollectionChangedListenedTo
		{
			get
			{
				return this.onCollectionChanged != null;
			}
		}

		/// <summary>Gets an array of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects.</summary>
		/// <returns>An array of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects.</returns>
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x00099BB8 File Offset: 0x00097DB8
		protected ArrayList List
		{
			get
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridViewRow dataGridViewRow = this[i];
				}
				return this.items;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x00099BE6 File Offset: 0x00097DE6
		internal ArrayList SharedList
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Forms.DataGridViewRow" /> at the specified index.</summary>
		/// <param name="rowIndex">The index of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> to get.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> positioned at the specified index.</returns>
		// Token: 0x06001ED5 RID: 7893 RVA: 0x00099BEE File Offset: 0x00097DEE
		public DataGridViewRow SharedRow(int rowIndex)
		{
			return (DataGridViewRow)this.SharedList[rowIndex];
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridView" /> that owns the collection.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridView" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</returns>
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x00099C01 File Offset: 0x00097E01
		protected DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridViewRow" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> to get.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> at the specified index. Accessing a <see cref="T:System.Windows.Forms.DataGridViewRow" /> with this indexer causes the row to become unshared. To keep the row shared, use the <see cref="M:System.Windows.Forms.DataGridViewRowCollection.SharedRow(System.Int32)" /> method. For more information, see Best Practices for Scaling the Windows Forms DataGridView Control.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.- or -
		///         <paramref name="index" /> is equal to or greater than <see cref="P:System.Windows.Forms.DataGridViewRowCollection.Count" />.</exception>
		// Token: 0x1700074D RID: 1869
		public DataGridViewRow this[int index]
		{
			get
			{
				DataGridViewRow dataGridViewRow = this.SharedRow(index);
				if (dataGridViewRow.Index != -1)
				{
					return dataGridViewRow;
				}
				if (index == 0 && this.items.Count == 1)
				{
					dataGridViewRow.IndexInternal = 0;
					dataGridViewRow.StateInternal = this.SharedRowState(0);
					if (this.DataGridView != null)
					{
						this.DataGridView.OnRowUnshared(dataGridViewRow);
					}
					return dataGridViewRow;
				}
				DataGridViewRow dataGridViewRow2 = (DataGridViewRow)dataGridViewRow.Clone();
				dataGridViewRow2.IndexInternal = index;
				dataGridViewRow2.DataGridViewInternal = dataGridViewRow.DataGridView;
				dataGridViewRow2.StateInternal = this.SharedRowState(index);
				this.SharedList[index] = dataGridViewRow2;
				int num = 0;
				foreach (object obj in dataGridViewRow2.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = dataGridViewRow.DataGridView;
					dataGridViewCell.OwningRowInternal = dataGridViewRow2;
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
					num++;
				}
				if (dataGridViewRow2.HasHeaderCell)
				{
					dataGridViewRow2.HeaderCell.DataGridViewInternal = dataGridViewRow.DataGridView;
					dataGridViewRow2.HeaderCell.OwningRowInternal = dataGridViewRow2;
				}
				if (this.DataGridView != null)
				{
					this.DataGridView.OnRowUnshared(dataGridViewRow2);
				}
				return dataGridViewRow2;
			}
		}

		/// <summary>Occurs when the contents of the collection change.</summary>
		// Token: 0x1400016B RID: 363
		// (add) Token: 0x06001ED8 RID: 7896 RVA: 0x00099D5C File Offset: 0x00097F5C
		// (remove) Token: 0x06001ED9 RID: 7897 RVA: 0x00099D75 File Offset: 0x00097F75
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

		/// <summary>Adds a new row to the collection.</summary>
		/// <returns>The index of the new row.</returns>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-This operation would add a frozen row after unfrozen rows.</exception>
		/// <exception cref="T:System.ArgumentException">The row returned by the <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property has more cells than there are columns in the control.</exception>
		// Token: 0x06001EDA RID: 7898 RVA: 0x00099D90 File Offset: 0x00097F90
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add()
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(false, null);
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00099DE0 File Offset: 0x00097FE0
		internal int AddInternal(bool newRow, object[] values)
		{
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			if (newRow)
			{
				rowTemplateClone.StateInternal = (rowTemplateClone.State | DataGridViewElementStates.Visible);
				foreach (object obj in rowTemplateClone.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.Value = dataGridViewCell.DefaultNewRowValue;
				}
			}
			if (values != null)
			{
				rowTemplateClone.SetValuesInternal(values);
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.Insert(num, rowTemplateClone);
				return num;
			}
			DataGridViewElementStates state = rowTemplateClone.State;
			this.DataGridView.OnAddingRow(rowTemplateClone, state, true);
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num2 = 0;
			foreach (object obj2 in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell2 = (DataGridViewCell)obj2;
				dataGridViewCell2.DataGridViewInternal = this.dataGridView;
				dataGridViewCell2.OwningColumnInternal = this.DataGridView.Columns[num2];
				num2++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.DataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			int num3 = this.SharedList.Add(rowTemplateClone);
			this.rowStates.Add(state);
			if (values != null || !this.RowIsSharable(num3) || DataGridViewRowCollection.RowHasValueOrToolTipText(rowTemplateClone) || this.IsCollectionChangedListenedTo)
			{
				rowTemplateClone.IndexInternal = num3;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, rowTemplateClone), num3, 1);
			return num3;
		}

		/// <summary>Adds a new row to the collection, and populates the cells with the specified objects.</summary>
		/// <param name="values">A variable number of objects that populate the cells of the new <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <returns>The index of the new row.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="values" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.VirtualMode" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.- or -The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns. -or-The row returned by the <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property has more cells than there are columns in the control.-or-This operation would add a frozen row after unfrozen rows.</exception>
		// Token: 0x06001EDC RID: 7900 RVA: 0x00099FF4 File Offset: 0x000981F4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add(params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (this.DataGridView.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationInVirtualMode"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(false, values);
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> to the collection.</summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to add to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>The index of the new <see cref="T:System.Windows.Forms.DataGridViewRow" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-The <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the <paramref name="dataGridViewRow" /> is not <see langword="null" />.-or-
		///         <paramref name="dataGridViewRow" /> has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />. -or-This operation would add a frozen row after unfrozen rows.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRow" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="dataGridViewRow" /> has more cells than there are columns in the control.</exception>
		// Token: 0x06001EDD RID: 7901 RVA: 0x0009A070 File Offset: 0x00098270
		public virtual int Add(DataGridViewRow dataGridViewRow)
		{
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(dataGridViewRow);
		}

		/// <summary>Adds the specified number of new rows to the collection.</summary>
		/// <param name="count">The number of rows to add to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>The index of the last row that was added.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="count" /> is less than 1.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-The row returned by the <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property has more cells than there are columns in the control. -or-This operation would add frozen rows after unfrozen rows.</exception>
		// Token: 0x06001EDE RID: 7902 RVA: 0x0009A0E0 File Offset: 0x000982E0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			DataGridViewElementStates state = rowTemplateClone.State;
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num = 0;
			foreach (object obj in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.dataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num2 = this.Count - 1;
				this.InsertCopiesPrivate(rowTemplateClone, state, num2, count);
				return num2 + count - 1;
			}
			return this.AddCopiesPrivate(rowTemplateClone, state, count);
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0009A284 File Offset: 0x00098484
		internal int AddInternal(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowAlreadyBelongsToDataGridView"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
			}
			if (dataGridViewRow.Selected)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotAddOrInsertSelectedRow"));
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertInternal(num, dataGridViewRow);
				return num;
			}
			this.DataGridView.CompleteCellsCollection(dataGridViewRow);
			this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewRow.State, true);
			int num2 = 0;
			foreach (object obj in dataGridViewRow.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				if (dataGridViewCell.ColumnIndex == -1)
				{
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num2];
				}
				num2++;
			}
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			int num3 = this.SharedList.Add(dataGridViewRow);
			this.rowStates.Add(dataGridViewRow.State);
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			if (!this.RowIsSharable(num3) || DataGridViewRowCollection.RowHasValueOrToolTipText(dataGridViewRow) || this.IsCollectionChangedListenedTo)
			{
				dataGridViewRow.IndexInternal = num3;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), num3, 1);
			return num3;
		}

		/// <summary>Adds a new row based on the row at the specified index.</summary>
		/// <param name="indexSource">The index of the row on which to base the new row.</param>
		/// <returns>The index of the new row.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexSource" /> is less than zero or greater than or equal to the number of rows in the collection.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-This operation would add a frozen row after unfrozen rows.</exception>
		// Token: 0x06001EE0 RID: 7904 RVA: 0x0009A460 File Offset: 0x00098660
		public virtual int AddCopy(int indexSource)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddCopyInternal(indexSource, DataGridViewElementStates.None, DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected, false);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0009A4B4 File Offset: 0x000986B4
		internal int AddCopyInternal(int indexSource, DataGridViewElementStates dgvesAdd, DataGridViewElementStates dgvesRemove, bool newRow)
		{
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertCopy(indexSource, num);
				return num;
			}
			if (indexSource < 0 || indexSource >= this.Count)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			DataGridViewRow dataGridViewRow = this.SharedRow(indexSource);
			int num2;
			if (dataGridViewRow.Index == -1 && !this.IsCollectionChangedListenedTo && !newRow)
			{
				DataGridViewElementStates dataGridViewElementStates = this.rowStates[indexSource] & ~dgvesRemove;
				dataGridViewElementStates |= dgvesAdd;
				this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewElementStates, true);
				num2 = this.SharedList.Add(dataGridViewRow);
				this.rowStates.Add(dataGridViewElementStates);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), num2, 1);
				return num2;
			}
			num2 = this.AddDuplicateRow(dataGridViewRow, newRow);
			if (!this.RowIsSharable(num2) || DataGridViewRowCollection.RowHasValueOrToolTipText(this.SharedRow(num2)) || this.IsCollectionChangedListenedTo)
			{
				this.UnshareRow(num2);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(num2)), num2, 1);
			return num2;
		}

		/// <summary>Adds the specified number of rows to the collection based on the row at the specified index.</summary>
		/// <param name="indexSource">The index of the row on which to base the new rows.</param>
		/// <param name="count">The number of rows to add to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>The index of the last row that was added.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexSource" /> is less than zero or greater than or equal to the number of rows in the control.-or-
		///         <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-This operation would add a frozen row after unfrozen rows.</exception>
		// Token: 0x06001EE2 RID: 7906 RVA: 0x0009A5B4 File Offset: 0x000987B4
		public virtual int AddCopies(int indexSource, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddCopiesInternal(indexSource, count);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0009A604 File Offset: 0x00098804
		internal int AddCopiesInternal(int indexSource, int count)
		{
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertCopiesPrivate(indexSource, num, count);
				return num + count - 1;
			}
			return this.AddCopiesInternal(indexSource, count, DataGridViewElementStates.None, DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0009A644 File Offset: 0x00098844
		internal int AddCopiesInternal(int indexSource, int count, DataGridViewElementStates dgvesAdd, DataGridViewElementStates dgvesRemove)
		{
			if (indexSource < 0 || this.Count <= indexSource)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			DataGridViewElementStates dataGridViewElementStates = this.rowStates[indexSource] & ~dgvesRemove;
			dataGridViewElementStates |= dgvesAdd;
			return this.AddCopiesPrivate(this.SharedRow(indexSource), dataGridViewElementStates, count);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0009A6B0 File Offset: 0x000988B0
		private int AddCopiesPrivate(DataGridViewRow rowTemplate, DataGridViewElementStates rowTemplateState, int count)
		{
			int count2 = this.items.Count;
			int num;
			if (rowTemplate.Index == -1)
			{
				this.DataGridView.OnAddingRow(rowTemplate, rowTemplateState, true);
				for (int i = 0; i < count - 1; i++)
				{
					this.SharedList.Add(rowTemplate);
					this.rowStates.Add(rowTemplateState);
				}
				num = this.SharedList.Add(rowTemplate);
				this.rowStates.Add(rowTemplateState);
				this.DataGridView.OnAddedRow_PreNotification(num);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count2, count);
				for (int j = 0; j < count; j++)
				{
					this.DataGridView.OnAddedRow_PostNotification(num - (count - 1) + j);
				}
				return num;
			}
			num = this.AddDuplicateRow(rowTemplate, false);
			if (count > 1)
			{
				this.DataGridView.OnAddedRow_PreNotification(num);
				if (this.RowIsSharable(num))
				{
					DataGridViewRow dataGridViewRow = this.SharedRow(num);
					this.DataGridView.OnAddingRow(dataGridViewRow, rowTemplateState, true);
					for (int k = 1; k < count - 1; k++)
					{
						this.SharedList.Add(dataGridViewRow);
						this.rowStates.Add(rowTemplateState);
					}
					num = this.SharedList.Add(dataGridViewRow);
					this.rowStates.Add(rowTemplateState);
					this.DataGridView.OnAddedRow_PreNotification(num);
				}
				else
				{
					this.UnshareRow(num);
					for (int l = 1; l < count; l++)
					{
						num = this.AddDuplicateRow(rowTemplate, false);
						this.UnshareRow(num);
						this.DataGridView.OnAddedRow_PreNotification(num);
					}
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count2, count);
				for (int m = 0; m < count; m++)
				{
					this.DataGridView.OnAddedRow_PostNotification(num - (count - 1) + m);
				}
				return num;
			}
			if (this.IsCollectionChangedListenedTo)
			{
				this.UnshareRow(num);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(num)), num, 1);
			return num;
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0009A87C File Offset: 0x00098A7C
		private int AddDuplicateRow(DataGridViewRow rowTemplate, bool newRow)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)rowTemplate.Clone();
			dataGridViewRow.StateInternal = DataGridViewElementStates.None;
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			int num = 0;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if (newRow)
				{
					dataGridViewCell.Value = dataGridViewCell.DefaultNewRowValue;
				}
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			DataGridViewElementStates dataGridViewElementStates = rowTemplate.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewElementStates, true);
			this.rowStates.Add(dataGridViewElementStates);
			return this.SharedList.Add(dataGridViewRow);
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects to the collection.</summary>
		/// <param name="dataGridViewRows">An array of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects to be added to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRows" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="dataGridViewRows" /> contains only one row, and the row it contains has more cells than there are columns in the control.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-At least one entry in the <paramref name="dataGridViewRows" /> array is <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-At least one row in the <paramref name="dataGridViewRows" /> array has a <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property value that is not <see langword="null" />.-or-At least one row in the <paramref name="dataGridViewRows" /> array has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />.-or-Two or more rows in the <paramref name="dataGridViewRows" /> array are identical.-or-At least one row in the <paramref name="dataGridViewRows" /> array contains one or more cells of a type that is incompatible with the type of the corresponding column in the control.-or-At least one row in the <paramref name="dataGridViewRows" /> array contains more cells than there are columns in the control.-or-This operation would add frozen rows after unfrozen rows.</exception>
		// Token: 0x06001EE7 RID: 7911 RVA: 0x0009A988 File Offset: 0x00098B88
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual void AddRange(params DataGridViewRow[] dataGridViewRows)
		{
			if (dataGridViewRows == null)
			{
				throw new ArgumentNullException("dataGridViewRows");
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				this.InsertRange(this.Count - 1, dataGridViewRows);
				return;
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			int count = this.items.Count;
			this.DataGridView.OnAddingRows(dataGridViewRows, true);
			foreach (DataGridViewRow dataGridViewRow in dataGridViewRows)
			{
				int num = 0;
				foreach (object obj in dataGridViewRow.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = this.dataGridView;
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
					num++;
				}
				if (dataGridViewRow.HasHeaderCell)
				{
					dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
					dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
				}
				int indexInternal = this.SharedList.Add(dataGridViewRow);
				this.rowStates.Add(dataGridViewRow.State);
				dataGridViewRow.IndexInternal = indexInternal;
				dataGridViewRow.DataGridViewInternal = this.dataGridView;
			}
			this.DataGridView.OnAddedRows_PreNotification(dataGridViewRows);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count, dataGridViewRows.Length);
			this.DataGridView.OnAddedRows_PostNotification(dataGridViewRows);
		}

		/// <summary>Clears the collection. </summary>
		/// <exception cref="T:System.InvalidOperationException">The collection is data bound and the underlying data source does not support clearing the row data.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents the row collection from being modified:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           </exception>
		// Token: 0x06001EE8 RID: 7912 RVA: 0x0009AB50 File Offset: 0x00098D50
		public virtual void Clear()
		{
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource == null)
			{
				this.ClearInternal(true);
				return;
			}
			IBindingList bindingList = this.DataGridView.DataConnection.List as IBindingList;
			if (bindingList != null && bindingList.AllowRemove && bindingList.SupportsChangeNotification)
			{
				bindingList.Clear();
				return;
			}
			throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CantClearRowCollectionWithWrongSource"));
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0009ABD0 File Offset: 0x00098DD0
		internal void ClearInternal(bool recreateNewRow)
		{
			int count = this.items.Count;
			if (count > 0)
			{
				this.DataGridView.OnClearingRows();
				for (int i = 0; i < count; i++)
				{
					this.SharedRow(i).DetachFromDataGridView();
				}
				this.SharedList.Clear();
				this.rowStates.Clear();
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), 0, count, true, false, recreateNewRow, new Point(-1, -1));
				return;
			}
			if (recreateNewRow && this.DataGridView.Columns.Count != 0 && this.DataGridView.AllowUserToAddRowsInternal && this.items.Count == 0)
			{
				this.DataGridView.AddNewRow(false);
			}
		}

		/// <summary>Determines whether the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> is in the collection.</summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.DataGridViewRow" /> is in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001EEA RID: 7914 RVA: 0x0009AC7B File Offset: 0x00098E7B
		public virtual bool Contains(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow) != -1;
		}

		/// <summary>Copies the items from the collection into the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> array, starting at the specified index.</summary>
		/// <param name="array">A <see cref="T:System.Windows.Forms.DataGridViewRow" /> array that is the destination of the items copied from the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />. </exception>
		// Token: 0x06001EEB RID: 7915 RVA: 0x00099B4E File Offset: 0x00097D4E
		public void CopyTo(DataGridViewRow[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0009AC90 File Offset: 0x00098E90
		internal int DisplayIndexToRowIndex(int visibleRowIndex)
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				if ((this.GetRowState(i) & DataGridViewElementStates.Visible) == DataGridViewElementStates.Visible)
				{
					num++;
				}
				if (num == visibleRowIndex)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>Returns the index of the first <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified criteria.</summary>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the first <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EED RID: 7917 RVA: 0x0009ACCC File Offset: 0x00098ECC
		public int GetFirstRow(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter != (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleSelected == 0)
						{
							return -1;
						}
					}
				}
				else if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
			}
			else if (this.rowCountsVisible == 0)
			{
				return -1;
			}
			int num = 0;
			while (num < this.items.Count && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the index of the first <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified inclusion and exclusion criteria.</summary>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <param name="excludeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the first <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />, and does not have the attributes specified by <paramref name="excludeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentException">One or both of the specified filter values is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EEE RID: 7918 RVA: 0x0009AD60 File Offset: 0x00098F60
		public int GetFirstRow(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetFirstRow(includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"excludeFilter"
				}));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter != (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleSelected == 0)
						{
							return -1;
						}
					}
				}
				else if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
			}
			else if (this.rowCountsVisible == 0)
			{
				return -1;
			}
			int num = 0;
			while (num < this.items.Count && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the index of the last <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified criteria.</summary>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the last <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EEF RID: 7919 RVA: 0x0009AE30 File Offset: 0x00099030
		public int GetLastRow(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter != (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleSelected == 0)
						{
							return -1;
						}
					}
				}
				else if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
			}
			else if (this.rowCountsVisible == 0)
			{
				return -1;
			}
			int num = this.items.Count - 1;
			while (num >= 0 && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0009AEBC File Offset: 0x000990BC
		internal int GetNextRow(int indexStart, DataGridViewElementStates includeFilter, int skipRows)
		{
			int num = indexStart;
			do
			{
				num = this.GetNextRow(num, includeFilter);
				skipRows--;
			}
			while (skipRows >= 0 && num != -1);
			return num;
		}

		/// <summary>Returns the index of the next <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified criteria.</summary>
		/// <param name="indexStart">The index of the row where the method should begin to look for the next <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the first <see cref="T:System.Windows.Forms.DataGridViewRow" /> after <paramref name="indexStart" /> that has the attributes specified by <paramref name="includeFilter" />, or -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexStart" /> is less than -1.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF1 RID: 7921 RVA: 0x0009AEE4 File Offset: 0x000990E4
		public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (indexStart < -1)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					-1.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart + 1;
			while (num < this.items.Count && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the index of the next <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified inclusion and exclusion criteria.</summary>
		/// <param name="indexStart">The index of the row where the method should begin to look for the next <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <param name="excludeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the next <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />, and does not have the attributes specified by <paramref name="excludeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexStart" /> is less than -1.</exception>
		/// <exception cref="T:System.ArgumentException">One or both of the specified filter values is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF2 RID: 7922 RVA: 0x0009AF94 File Offset: 0x00099194
		public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetNextRow(indexStart, includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"excludeFilter"
				}));
			}
			if (indexStart < -1)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					-1.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart + 1;
			while (num < this.items.Count && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the index of the previous <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified criteria.</summary>
		/// <param name="indexStart">The index of the row where the method should begin to look for the previous <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the previous <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexStart" /> is greater than the number of rows in the collection.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF3 RID: 7923 RVA: 0x0009B07C File Offset: 0x0009927C
		public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (indexStart > this.items.Count)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					this.items.Count.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart - 1;
			while (num >= 0 && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the index of the previous <see cref="T:System.Windows.Forms.DataGridViewRow" /> that meets the specified inclusion and exclusion criteria.</summary>
		/// <param name="indexStart">The index of the row where the method should begin to look for the previous <see cref="T:System.Windows.Forms.DataGridViewRow" />.</param>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <param name="excludeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The index of the previous <see cref="T:System.Windows.Forms.DataGridViewRow" /> that has the attributes specified by <paramref name="includeFilter" />, and does not have the attributes specified by <paramref name="excludeFilter" />; -1 if no row is found.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexStart" /> is greater than the number of rows in the collection.</exception>
		/// <exception cref="T:System.ArgumentException">One or both of the specified filter values is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF4 RID: 7924 RVA: 0x0009B12C File Offset: 0x0009932C
		public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetPreviousRow(indexStart, includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"excludeFilter"
				}));
			}
			if (indexStart > this.items.Count)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					this.items.Count.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart - 1;
			while (num >= 0 && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		/// <summary>Returns the number of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects in the collection that meet the specified criteria.</summary>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The number of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> that have the attributes specified by <paramref name="includeFilter" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF5 RID: 7925 RVA: 0x0009B214 File Offset: 0x00099414
		public int GetRowCount(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter != (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleSelected != -1)
						{
							return this.rowCountsVisibleSelected;
						}
					}
				}
				else if (this.rowCountsVisibleFrozen != -1)
				{
					return this.rowCountsVisibleFrozen;
				}
			}
			else if (this.rowCountsVisible != -1)
			{
				return this.rowCountsVisible;
			}
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num++;
				}
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter != (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
					{
						this.rowCountsVisibleSelected = num;
					}
				}
				else
				{
					this.rowCountsVisibleFrozen = num;
				}
			}
			else
			{
				this.rowCountsVisible = num;
			}
			return num;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0009B2DC File Offset: 0x000994DC
		internal int GetRowCount(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex)
		{
			int num = 0;
			for (int i = fromRowIndex + 1; i <= toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num++;
				}
			}
			return num;
		}

		/// <summary>Returns the cumulative height of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects that meet the specified criteria.</summary>
		/// <param name="includeFilter">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</param>
		/// <returns>The cumulative height of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" /> that have the attributes specified by <paramref name="includeFilter" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="includeFilter" /> is not a valid bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values.</exception>
		// Token: 0x06001EF7 RID: 7927 RVA: 0x0009B30C File Offset: 0x0009950C
		public int GetRowsHeight(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[]
				{
					"includeFilter"
				}));
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					if (this.rowsHeightVisibleFrozen != -1)
					{
						return this.rowsHeightVisibleFrozen;
					}
				}
			}
			else if (this.rowsHeightVisible != -1)
			{
				return this.rowsHeightVisible;
			}
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
				}
			}
			if (includeFilter != DataGridViewElementStates.Visible)
			{
				if (includeFilter == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
				{
					this.rowsHeightVisibleFrozen = num;
				}
			}
			else
			{
				this.rowsHeightVisible = num;
			}
			return num;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0009B3C4 File Offset: 0x000995C4
		internal int GetRowsHeight(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex)
		{
			int num = 0;
			for (int i = fromRowIndex; i < toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
				}
			}
			return num;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0009B408 File Offset: 0x00099608
		private bool GetRowsHeightExceedLimit(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex, int heightLimit)
		{
			int num = 0;
			for (int i = fromRowIndex; i < toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
					if (num > heightLimit)
					{
						return true;
					}
				}
			}
			return num > heightLimit;
		}

		/// <summary>Gets the state of the row with the specified index.</summary>
		/// <param name="rowIndex">The index of the row.</param>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values indicating the state of the specified row.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero and greater than the number of rows in the collection minus one.</exception>
		// Token: 0x06001EFA RID: 7930 RVA: 0x0009B458 File Offset: 0x00099658
		public virtual DataGridViewElementStates GetRowState(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= this.items.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex);
			if (dataGridViewRow.Index == -1)
			{
				return this.SharedRowState(rowIndex);
			}
			return dataGridViewRow.GetState(rowIndex);
		}

		/// <summary>Returns the index of a specified item in the collection.</summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if it is a <see cref="T:System.Windows.Forms.DataGridViewRow" /> found in the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />; otherwise, -1.</returns>
		// Token: 0x06001EFB RID: 7931 RVA: 0x00099B11 File Offset: 0x00097D11
		public int IndexOf(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow);
		}

		/// <summary>Inserts a row into the collection at the specified position, and populates the cells with the specified objects.</summary>
		/// <param name="rowIndex">The position at which to insert the row.</param>
		/// <param name="values">A variable number of objects that populate the cells of the new row.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero or greater than the number of rows in the collection. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="values" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.VirtualMode" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-
		///         <paramref name="rowIndex" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of the row returned by the control's <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property is not <see langword="null" />. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
		/// <exception cref="T:System.ArgumentException">The row returned by the control's <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property has more cells than there are columns in the control.</exception>
		// Token: 0x06001EFC RID: 7932 RVA: 0x0009B4AC File Offset: 0x000996AC
		public virtual void Insert(int rowIndex, params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (this.DataGridView.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationInVirtualMode"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			rowTemplateClone.SetValuesInternal(values);
			this.Insert(rowIndex, rowTemplateClone);
		}

		/// <summary>Inserts the specified <see cref="T:System.Windows.Forms.DataGridViewRow" /> into the collection.</summary>
		/// <param name="rowIndex">The position at which to insert the row.</param>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to insert into the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero or greater than the number of rows in the collection. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRow" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-
		///         <paramref name="rowIndex" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-The <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property of <paramref name="dataGridViewRow" /> is not <see langword="null" />.-or-
		///         <paramref name="dataGridViewRow" /> has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="dataGridViewRow" /> has more cells than there are columns in the control.</exception>
		// Token: 0x06001EFD RID: 7933 RVA: 0x0009B520 File Offset: 0x00099720
		public virtual void Insert(int rowIndex, DataGridViewRow dataGridViewRow)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			this.InsertInternal(rowIndex, dataGridViewRow);
		}

		/// <summary>Inserts the specified number of rows into the collection at the specified location.</summary>
		/// <param name="rowIndex">The position at which to insert the rows.</param>
		/// <param name="count">The number of rows to insert into the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero or greater than the number of rows in the collection. -or-
		///         <paramref name="count" /> is less than 1.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-
		///         <paramref name="rowIndex" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The row returned by the <see cref="P:System.Windows.Forms.DataGridView.RowTemplate" /> property has more cells than there are columns in the control. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
		// Token: 0x06001EFE RID: 7934 RVA: 0x0009B570 File Offset: 0x00099770
		public virtual void Insert(int rowIndex, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (rowIndex < 0 || this.Count < rowIndex)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			DataGridViewElementStates state = rowTemplateClone.State;
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num = 0;
			foreach (object obj in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.dataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			this.InsertCopiesPrivate(rowTemplateClone, state, rowIndex, count);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0009B734 File Offset: 0x00099934
		internal void InsertInternal(int rowIndex, DataGridViewRow dataGridViewRow)
		{
			if (rowIndex < 0 || this.Count < rowIndex)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowAlreadyBelongsToDataGridView"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
			}
			if (dataGridViewRow.Selected)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotAddOrInsertSelectedRow"));
			}
			this.InsertInternal(rowIndex, dataGridViewRow, false);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0009B828 File Offset: 0x00099A28
		internal void InsertInternal(int rowIndex, DataGridViewRow dataGridViewRow, bool force)
		{
			Point newCurrentCell = new Point(-1, -1);
			if (force)
			{
				if (this.DataGridView.Columns.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
				}
				if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
				{
					throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
				}
			}
			this.DataGridView.CompleteCellsCollection(dataGridViewRow);
			this.DataGridView.OnInsertingRow(rowIndex, dataGridViewRow, dataGridViewRow.State, ref newCurrentCell, true, 1, force);
			int num = 0;
			foreach (object obj in dataGridViewRow.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				if (dataGridViewCell.ColumnIndex == -1)
				{
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				}
				num++;
			}
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.SharedList.Insert(rowIndex, dataGridViewRow);
			this.rowStates.Insert(rowIndex, dataGridViewRow.State);
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			if (!this.RowIsSharable(rowIndex) || DataGridViewRowCollection.RowHasValueOrToolTipText(dataGridViewRow) || this.IsCollectionChangedListenedTo)
			{
				dataGridViewRow.IndexInternal = rowIndex;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), rowIndex, 1, false, true, false, newCurrentCell);
		}

		/// <summary>Inserts a row into the collection at the specified position, based on the row at specified position.</summary>
		/// <param name="indexSource">The index of the row on which to base the new row.</param>
		/// <param name="indexDestination">The position at which to insert the row.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexSource" /> is less than zero or greater than the number of rows in the collection minus one.-or-
		///         <paramref name="indexDestination" /> is less than zero or greater than the number of rows in the collection.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="indexDestination" /> is equal to the number of rows in the collection and <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> is <see langword="true" />. -or-This operation would insert a frozen row after unfrozen rows or an unfrozen row before frozen rows.</exception>
		// Token: 0x06001F01 RID: 7937 RVA: 0x0009B9B8 File Offset: 0x00099BB8
		public virtual void InsertCopy(int indexSource, int indexDestination)
		{
			this.InsertCopies(indexSource, indexDestination, 1);
		}

		/// <summary>Inserts rows into the collection at the specified position.</summary>
		/// <param name="indexSource">The index of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> on which to base the new rows.</param>
		/// <param name="indexDestination">The position at which to insert the rows.</param>
		/// <param name="count">The number of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects to add to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="indexSource" /> is less than zero or greater than the number of rows in the collection minus one.-or-
		///         <paramref name="indexDestination" /> is less than zero or greater than the number of rows in the collection.-or-
		///         <paramref name="count" /> is less than 1.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="indexDestination" /> is equal to the number of rows in the collection and <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> is <see langword="true" />.-or-This operation would insert frozen rows after unfrozen rows or unfrozen rows before frozen rows.</exception>
		// Token: 0x06001F02 RID: 7938 RVA: 0x0009B9C4 File Offset: 0x00099BC4
		public virtual void InsertCopies(int indexSource, int indexDestination, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			this.InsertCopiesPrivate(indexSource, indexDestination, count);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0009BA14 File Offset: 0x00099C14
		private void InsertCopiesPrivate(int indexSource, int indexDestination, int count)
		{
			if (indexSource < 0 || this.Count <= indexSource)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			if (indexDestination < 0 || this.Count < indexDestination)
			{
				throw new ArgumentOutOfRangeException("indexDestination", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.NewRowIndex != -1 && indexDestination == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			DataGridViewElementStates rowTemplateState = this.GetRowState(indexSource) & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			this.InsertCopiesPrivate(this.SharedRow(indexSource), rowTemplateState, indexDestination, count);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0009BAC0 File Offset: 0x00099CC0
		private void InsertCopiesPrivate(DataGridViewRow rowTemplate, DataGridViewElementStates rowTemplateState, int indexDestination, int count)
		{
			Point newCurrentCell = new Point(-1, -1);
			if (rowTemplate.Index == -1)
			{
				if (count > 1)
				{
					this.DataGridView.OnInsertingRow(indexDestination, rowTemplate, rowTemplateState, ref newCurrentCell, true, count, false);
					for (int i = 0; i < count; i++)
					{
						this.SharedList.Insert(indexDestination + i, rowTemplate);
						this.rowStates.Insert(indexDestination + i, rowTemplateState);
					}
					this.DataGridView.OnInsertedRow_PreNotification(indexDestination, count);
					this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, newCurrentCell);
					for (int j = 0; j < count; j++)
					{
						this.DataGridView.OnInsertedRow_PostNotification(indexDestination + j, newCurrentCell, j == count - 1);
					}
					return;
				}
				this.DataGridView.OnInsertingRow(indexDestination, rowTemplate, rowTemplateState, ref newCurrentCell, true, 1, false);
				this.SharedList.Insert(indexDestination, rowTemplate);
				this.rowStates.Insert(indexDestination, rowTemplateState);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(indexDestination)), indexDestination, count, false, true, false, newCurrentCell);
				return;
			}
			else
			{
				this.InsertDuplicateRow(indexDestination, rowTemplate, true, ref newCurrentCell);
				if (count > 1)
				{
					this.DataGridView.OnInsertedRow_PreNotification(indexDestination, 1);
					if (this.RowIsSharable(indexDestination))
					{
						DataGridViewRow dataGridViewRow = this.SharedRow(indexDestination);
						this.DataGridView.OnInsertingRow(indexDestination + 1, dataGridViewRow, rowTemplateState, ref newCurrentCell, false, count - 1, false);
						for (int k = 1; k < count; k++)
						{
							this.SharedList.Insert(indexDestination + k, dataGridViewRow);
							this.rowStates.Insert(indexDestination + k, rowTemplateState);
						}
						this.DataGridView.OnInsertedRow_PreNotification(indexDestination + 1, count - 1);
						this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, newCurrentCell);
					}
					else
					{
						this.UnshareRow(indexDestination);
						for (int l = 1; l < count; l++)
						{
							this.InsertDuplicateRow(indexDestination + l, rowTemplate, false, ref newCurrentCell);
							this.UnshareRow(indexDestination + l);
							this.DataGridView.OnInsertedRow_PreNotification(indexDestination + l, 1);
						}
						this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, newCurrentCell);
					}
					for (int m = 0; m < count; m++)
					{
						this.DataGridView.OnInsertedRow_PostNotification(indexDestination + m, newCurrentCell, m == count - 1);
					}
					return;
				}
				if (this.IsCollectionChangedListenedTo)
				{
					this.UnshareRow(indexDestination);
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(indexDestination)), indexDestination, 1, false, true, false, newCurrentCell);
				return;
			}
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0009BD00 File Offset: 0x00099F00
		private void InsertDuplicateRow(int indexDestination, DataGridViewRow rowTemplate, bool firstInsertion, ref Point newCurrentCell)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)rowTemplate.Clone();
			dataGridViewRow.StateInternal = DataGridViewElementStates.None;
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			int num = 0;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			DataGridViewElementStates dataGridViewElementStates = rowTemplate.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.DataGridView.OnInsertingRow(indexDestination, dataGridViewRow, dataGridViewElementStates, ref newCurrentCell, firstInsertion, 1, false);
			this.SharedList.Insert(indexDestination, dataGridViewRow);
			this.rowStates.Insert(indexDestination, dataGridViewElementStates);
		}

		/// <summary>Inserts the <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects into the collection at the specified position.</summary>
		/// <param name="rowIndex">The position at which to insert the rows.</param>
		/// <param name="dataGridViewRows">An array of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects to add to the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRows" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> is less than zero or greater than the number of rows in the collection.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="dataGridViewRows" /> contains only one row, and the row it contains has more cells than there are columns in the control.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="rowIndex" /> is equal to the number of rows in the collection and <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> is <see langword="true" />.-or-The <see cref="P:System.Windows.Forms.DataGridView.DataSource" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is not <see langword="null" />.-or-At least one entry in the <paramref name="dataGridViewRows" /> array is <see langword="null" />.-or-The <see cref="T:System.Windows.Forms.DataGridView" /> has no columns.-or-At least one row in the <paramref name="dataGridViewRows" /> array has a <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property value that is not <see langword="null" />.-or-At least one row in the <paramref name="dataGridViewRows" /> array has a <see cref="P:System.Windows.Forms.DataGridViewRow.Selected" /> property value of <see langword="true" />.-or-Two or more rows in the <paramref name="dataGridViewRows" /> array are identical.-or-At least one row in the <paramref name="dataGridViewRows" /> array contains one or more cells of a type that is incompatible with the type of the corresponding column in the control.-or-At least one row in the <paramref name="dataGridViewRows" /> array contains more cells than there are columns in the control. -or-This operation would insert frozen rows after unfrozen rows or unfrozen rows before frozen rows.</exception>
		// Token: 0x06001F06 RID: 7942 RVA: 0x0009BE04 File Offset: 0x0009A004
		public virtual void InsertRange(int rowIndex, params DataGridViewRow[] dataGridViewRows)
		{
			if (dataGridViewRows == null)
			{
				throw new ArgumentNullException("dataGridViewRows");
			}
			if (dataGridViewRows.Length == 1)
			{
				this.Insert(rowIndex, dataGridViewRows[0]);
				return;
			}
			if (rowIndex < 0 || rowIndex > this.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			Point newCurrentCell = new Point(-1, -1);
			this.DataGridView.OnInsertingRows(rowIndex, dataGridViewRows, ref newCurrentCell);
			int num = rowIndex;
			foreach (DataGridViewRow dataGridViewRow in dataGridViewRows)
			{
				int num2 = 0;
				foreach (object obj in dataGridViewRow.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = this.dataGridView;
					if (dataGridViewCell.ColumnIndex == -1)
					{
						dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num2];
					}
					num2++;
				}
				if (dataGridViewRow.HasHeaderCell)
				{
					dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
					dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
				}
				this.SharedList.Insert(num, dataGridViewRow);
				this.rowStates.Insert(num, dataGridViewRow.State);
				dataGridViewRow.IndexInternal = num;
				dataGridViewRow.DataGridViewInternal = this.dataGridView;
				num++;
			}
			this.DataGridView.OnInsertedRows_PreNotification(rowIndex, dataGridViewRows);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), rowIndex, dataGridViewRows.Length, false, true, false, newCurrentCell);
			this.DataGridView.OnInsertedRows_PostNotification(dataGridViewRows, newCurrentCell);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0009C024 File Offset: 0x0009A224
		internal void InvalidateCachedRowCount(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedRowCounts();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Frozen)
			{
				this.rowCountsVisibleFrozen = -1;
				return;
			}
			if (includeFilter == DataGridViewElementStates.Selected)
			{
				this.rowCountsVisibleSelected = -1;
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0009C04C File Offset: 0x0009A24C
		internal void InvalidateCachedRowCounts()
		{
			this.rowCountsVisible = (this.rowCountsVisibleFrozen = (this.rowCountsVisibleSelected = -1));
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0009C072 File Offset: 0x0009A272
		internal void InvalidateCachedRowsHeight(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedRowsHeights();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Frozen)
			{
				this.rowsHeightVisibleFrozen = -1;
			}
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0009C08C File Offset: 0x0009A28C
		internal void InvalidateCachedRowsHeights()
		{
			this.rowsHeightVisible = (this.rowsHeightVisibleFrozen = -1);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridViewRowCollection.CollectionChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data. </param>
		// Token: 0x06001F0B RID: 7947 RVA: 0x0009C0A9 File Offset: 0x0009A2A9
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0009C0C0 File Offset: 0x0009A2C0
		private void OnCollectionChanged(CollectionChangeEventArgs e, int rowIndex, int rowCount)
		{
			Point newCurrentCell = new Point(-1, -1);
			DataGridViewRow dataGridViewRow = (DataGridViewRow)e.Element;
			int num = 0;
			if (dataGridViewRow != null && e.Action == CollectionChangeAction.Add)
			{
				num = this.SharedRow(rowIndex).Index;
			}
			this.OnCollectionChanged_PreNotification(e.Action, rowIndex, rowCount, ref dataGridViewRow, false);
			if (num == -1 && this.SharedRow(rowIndex).Index != -1)
			{
				e = new CollectionChangeEventArgs(e.Action, dataGridViewRow);
			}
			this.OnCollectionChanged(e);
			this.OnCollectionChanged_PostNotification(e.Action, rowIndex, rowCount, dataGridViewRow, false, false, false, newCurrentCell);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0009C14C File Offset: 0x0009A34C
		private void OnCollectionChanged(CollectionChangeEventArgs e, int rowIndex, int rowCount, bool changeIsDeletion, bool changeIsInsertion, bool recreateNewRow, Point newCurrentCell)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)e.Element;
			int num = 0;
			if (dataGridViewRow != null && e.Action == CollectionChangeAction.Add)
			{
				num = this.SharedRow(rowIndex).Index;
			}
			this.OnCollectionChanged_PreNotification(e.Action, rowIndex, rowCount, ref dataGridViewRow, changeIsInsertion);
			if (num == -1 && this.SharedRow(rowIndex).Index != -1)
			{
				e = new CollectionChangeEventArgs(e.Action, dataGridViewRow);
			}
			this.OnCollectionChanged(e);
			this.OnCollectionChanged_PostNotification(e.Action, rowIndex, rowCount, dataGridViewRow, changeIsDeletion, changeIsInsertion, recreateNewRow, newCurrentCell);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0009C1D4 File Offset: 0x0009A3D4
		private void OnCollectionChanged_PreNotification(CollectionChangeAction cca, int rowIndex, int rowCount, ref DataGridViewRow dataGridViewRow, bool changeIsInsertion)
		{
			bool flag = false;
			bool computeVisibleRows = false;
			switch (cca)
			{
			case CollectionChangeAction.Add:
			{
				int num = 0;
				this.UpdateRowCaches(rowIndex, ref dataGridViewRow, true);
				if ((this.GetRowState(rowIndex) & DataGridViewElementStates.Visible) == DataGridViewElementStates.None)
				{
					flag = true;
					computeVisibleRows = changeIsInsertion;
				}
				else
				{
					int firstDisplayedRowIndex = this.DataGridView.FirstDisplayedRowIndex;
					if (firstDisplayedRowIndex != -1)
					{
						num = this.SharedRow(firstDisplayedRowIndex).GetHeight(firstDisplayedRowIndex);
					}
				}
				if (changeIsInsertion)
				{
					this.DataGridView.OnInsertedRow_PreNotification(rowIndex, 1);
					if (!flag)
					{
						if ((this.GetRowState(rowIndex) & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None)
						{
							flag = (this.DataGridView.FirstDisplayedScrollingRowIndex == -1 && this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height));
						}
						else if (this.DataGridView.FirstDisplayedScrollingRowIndex != -1 && rowIndex > this.DataGridView.FirstDisplayedScrollingRowIndex)
						{
							flag = (this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + this.DataGridView.VerticalScrollingOffset) && num <= this.DataGridView.LayoutInfo.Data.Height);
						}
					}
				}
				else
				{
					this.DataGridView.OnAddedRow_PreNotification(rowIndex);
					if (!flag)
					{
						int num2 = this.GetRowsHeight(DataGridViewElementStates.Visible) - this.DataGridView.VerticalScrollingOffset - dataGridViewRow.GetHeight(rowIndex);
						dataGridViewRow = this.SharedRow(rowIndex);
						flag = (this.DataGridView.LayoutInfo.Data.Height < num2 && num <= this.DataGridView.LayoutInfo.Data.Height);
					}
				}
				break;
			}
			case CollectionChangeAction.Remove:
			{
				DataGridViewElementStates rowState = this.GetRowState(rowIndex);
				bool flag2 = (rowState & DataGridViewElementStates.Visible) > DataGridViewElementStates.None;
				bool flag3 = (rowState & DataGridViewElementStates.Frozen) > DataGridViewElementStates.None;
				this.rowStates.RemoveAt(rowIndex);
				this.SharedList.RemoveAt(rowIndex);
				this.DataGridView.OnRemovedRow_PreNotification(rowIndex);
				if (flag2)
				{
					if (flag3)
					{
						flag = (this.DataGridView.FirstDisplayedScrollingRowIndex == -1 && this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + SystemInformation.HorizontalScrollBarHeight));
					}
					else if (this.DataGridView.FirstDisplayedScrollingRowIndex != -1 && rowIndex > this.DataGridView.FirstDisplayedScrollingRowIndex)
					{
						int num3 = 0;
						int firstDisplayedRowIndex2 = this.DataGridView.FirstDisplayedRowIndex;
						if (firstDisplayedRowIndex2 != -1)
						{
							num3 = this.SharedRow(firstDisplayedRowIndex2).GetHeight(firstDisplayedRowIndex2);
						}
						flag = (this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + this.DataGridView.VerticalScrollingOffset + SystemInformation.HorizontalScrollBarHeight) && num3 <= this.DataGridView.LayoutInfo.Data.Height);
					}
				}
				else
				{
					flag = true;
				}
				break;
			}
			case CollectionChangeAction.Refresh:
				this.InvalidateCachedRowCounts();
				this.InvalidateCachedRowsHeights();
				break;
			}
			this.DataGridView.ResetUIState(flag, computeVisibleRows);
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0009C4C4 File Offset: 0x0009A6C4
		private void OnCollectionChanged_PostNotification(CollectionChangeAction cca, int rowIndex, int rowCount, DataGridViewRow dataGridViewRow, bool changeIsDeletion, bool changeIsInsertion, bool recreateNewRow, Point newCurrentCell)
		{
			if (changeIsDeletion)
			{
				this.DataGridView.OnRowsRemovedInternal(rowIndex, rowCount);
			}
			else
			{
				this.DataGridView.OnRowsAddedInternal(rowIndex, rowCount);
			}
			switch (cca)
			{
			case CollectionChangeAction.Add:
				if (changeIsInsertion)
				{
					this.DataGridView.OnInsertedRow_PostNotification(rowIndex, newCurrentCell, true);
				}
				else
				{
					this.DataGridView.OnAddedRow_PostNotification(rowIndex);
				}
				break;
			case CollectionChangeAction.Remove:
				this.DataGridView.OnRemovedRow_PostNotification(dataGridViewRow, newCurrentCell);
				break;
			case CollectionChangeAction.Refresh:
				if (changeIsDeletion)
				{
					this.DataGridView.OnClearedRows();
				}
				break;
			}
			this.DataGridView.OnRowCollectionChanged_PostNotification(recreateNewRow, newCurrentCell.X == -1, cca, dataGridViewRow, rowIndex);
		}

		/// <summary>Removes the row from the collection.</summary>
		/// <param name="dataGridViewRow">The row to remove from the <see cref="T:System.Windows.Forms.DataGridViewRowCollection" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewRow" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="dataGridViewRow" /> is not contained in this collection.-or-
		///         <paramref name="dataGridViewRow" /> is a shared row.</exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="dataGridViewRow" /> is the row for new records.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is bound to an <see cref="T:System.ComponentModel.IBindingList" /> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove" /> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification" /> property values that are not both <see langword="true" />. </exception>
		// Token: 0x06001F10 RID: 7952 RVA: 0x0009C568 File Offset: 0x0009A768
		public virtual void Remove(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != this.DataGridView)
			{
				throw new ArgumentException(SR.GetString("DataGridView_RowDoesNotBelongToDataGridView"), "dataGridViewRow");
			}
			if (dataGridViewRow.Index == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridView_RowMustBeUnshared"), "dataGridViewRow");
			}
			this.RemoveAt(dataGridViewRow.Index);
		}

		/// <summary>Removes the row at the specified position from the collection.</summary>
		/// <param name="index">The position of the row to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero and greater than the number of rows in the collection minus one. </exception>
		/// <exception cref="T:System.InvalidOperationException">The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is performing one of the following actions that temporarily prevents new rows from being added:Selecting all cells in the control.Clearing the selection.-or-This method is being called from a handler for one of the following <see cref="T:System.Windows.Forms.DataGridView" /> events:
		///             <see cref="E:System.Windows.Forms.DataGridView.CellEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidating" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.CellValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowEnter" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowLeave" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidated" />
		///
		///             <see cref="E:System.Windows.Forms.DataGridView.RowValidating" />
		///           -or-
		///         <paramref name="index" /> is equal to the number of rows in the collection and the <see cref="P:System.Windows.Forms.DataGridView.AllowUserToAddRows" /> property of the <see cref="T:System.Windows.Forms.DataGridView" /> is set to <see langword="true" />.-or-The associated <see cref="T:System.Windows.Forms.DataGridView" /> control is bound to an <see cref="T:System.ComponentModel.IBindingList" /> implementation with <see cref="P:System.ComponentModel.IBindingList.AllowRemove" /> and <see cref="P:System.ComponentModel.IBindingList.SupportsChangeNotification" /> property values that are not both <see langword="true" />.</exception>
		// Token: 0x06001F11 RID: 7953 RVA: 0x0009C5D0 File Offset: 0x0009A7D0
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			if (this.DataGridView.NewRowIndex == index)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotDeleteNewRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource == null)
			{
				this.RemoveAtInternal(index, false);
				return;
			}
			IBindingList bindingList = this.DataGridView.DataConnection.List as IBindingList;
			if (bindingList != null && bindingList.AllowRemove && bindingList.SupportsChangeNotification)
			{
				bindingList.RemoveAt(index);
				return;
			}
			throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CantRemoveRowsWithWrongSource"));
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0009C690 File Offset: 0x0009A890
		internal void RemoveAtInternal(int index, bool force)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(index);
			Point newCurrentCell = new Point(-1, -1);
			if (this.IsCollectionChangedListenedTo || dataGridViewRow.GetDisplayed(index))
			{
				dataGridViewRow = this[index];
			}
			dataGridViewRow = this.SharedRow(index);
			this.DataGridView.OnRemovingRow(index, out newCurrentCell, force);
			this.UpdateRowCaches(index, ref dataGridViewRow, false);
			if (dataGridViewRow.Index != -1)
			{
				this.rowStates[index] = dataGridViewRow.State;
				dataGridViewRow.DetachFromDataGridView();
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridViewRow), index, 1, true, false, false, newCurrentCell);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0009C71C File Offset: 0x0009A91C
		private static bool RowHasValueOrToolTipText(DataGridViewRow dataGridViewRow)
		{
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if (dataGridViewCell.HasValue || dataGridViewCell.HasToolTipText)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0009C78C File Offset: 0x0009A98C
		internal bool RowIsSharable(int index)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(index);
			if (dataGridViewRow.Index != -1)
			{
				return false;
			}
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if ((dataGridViewCell.State & ~(dataGridViewCell.CellStateFromColumnRowStates(this.rowStates[index]) != DataGridViewElementStates.None)) != DataGridViewElementStates.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0009C81C File Offset: 0x0009AA1C
		internal void SetRowState(int rowIndex, DataGridViewElementStates state, bool value)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex);
			if (dataGridViewRow.Index == -1)
			{
				if ((this.rowStates[rowIndex] & state) > DataGridViewElementStates.None != value)
				{
					if (state == DataGridViewElementStates.Frozen || state == DataGridViewElementStates.Visible || state == DataGridViewElementStates.ReadOnly)
					{
						dataGridViewRow.OnSharedStateChanging(rowIndex, state);
					}
					if (value)
					{
						this.rowStates[rowIndex] = (this.rowStates[rowIndex] | state);
					}
					else
					{
						this.rowStates[rowIndex] = (this.rowStates[rowIndex] & ~state);
					}
					dataGridViewRow.OnSharedStateChanged(rowIndex, state);
					return;
				}
			}
			else if (state <= DataGridViewElementStates.Resizable)
			{
				switch (state)
				{
				case DataGridViewElementStates.Displayed:
					dataGridViewRow.DisplayedInternal = value;
					return;
				case DataGridViewElementStates.Frozen:
					dataGridViewRow.Frozen = value;
					return;
				case DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen:
					break;
				case DataGridViewElementStates.ReadOnly:
					dataGridViewRow.ReadOnlyInternal = value;
					return;
				default:
					if (state != DataGridViewElementStates.Resizable)
					{
						return;
					}
					dataGridViewRow.Resizable = (value ? DataGridViewTriState.True : DataGridViewTriState.False);
					break;
				}
			}
			else
			{
				if (state == DataGridViewElementStates.Selected)
				{
					dataGridViewRow.SelectedInternal = value;
					return;
				}
				if (state != DataGridViewElementStates.Visible)
				{
					return;
				}
				dataGridViewRow.Visible = value;
				return;
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0009C90A File Offset: 0x0009AB0A
		internal DataGridViewElementStates SharedRowState(int rowIndex)
		{
			return this.rowStates[rowIndex];
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0009C918 File Offset: 0x0009AB18
		internal void Sort(IComparer customComparer, bool ascending)
		{
			if (this.items.Count > 0)
			{
				DataGridViewRowCollection.RowComparer rowComparer = new DataGridViewRowCollection.RowComparer(this, customComparer, ascending);
				this.items.CustomSort(rowComparer);
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0009C948 File Offset: 0x0009AB48
		internal void SwapSortedRows(int rowIndex1, int rowIndex2)
		{
			this.DataGridView.SwapSortedRows(rowIndex1, rowIndex2);
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex1);
			DataGridViewRow dataGridViewRow2 = this.SharedRow(rowIndex2);
			if (dataGridViewRow.Index != -1)
			{
				dataGridViewRow.IndexInternal = rowIndex2;
			}
			if (dataGridViewRow2.Index != -1)
			{
				dataGridViewRow2.IndexInternal = rowIndex1;
			}
			if (this.DataGridView.VirtualMode)
			{
				int count = this.DataGridView.Columns.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[i];
					DataGridViewCell dataGridViewCell2 = dataGridViewRow2.Cells[i];
					object valueInternal = dataGridViewCell.GetValueInternal(rowIndex1);
					object valueInternal2 = dataGridViewCell2.GetValueInternal(rowIndex2);
					dataGridViewCell.SetValueInternal(rowIndex1, valueInternal2);
					dataGridViewCell2.SetValueInternal(rowIndex2, valueInternal);
				}
			}
			object value = this.items[rowIndex1];
			this.items[rowIndex1] = this.items[rowIndex2];
			this.items[rowIndex2] = value;
			DataGridViewElementStates value2 = this.rowStates[rowIndex1];
			this.rowStates[rowIndex1] = this.rowStates[rowIndex2];
			this.rowStates[rowIndex2] = value2;
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0009CA6E File Offset: 0x0009AC6E
		private void UnshareRow(int rowIndex)
		{
			this.SharedRow(rowIndex).IndexInternal = rowIndex;
			this.SharedRow(rowIndex).StateInternal = this.SharedRowState(rowIndex);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0009CA90 File Offset: 0x0009AC90
		private void UpdateRowCaches(int rowIndex, ref DataGridViewRow dataGridViewRow, bool adding)
		{
			if (this.rowCountsVisible != -1 || this.rowCountsVisibleFrozen != -1 || this.rowCountsVisibleSelected != -1 || this.rowsHeightVisible != -1 || this.rowsHeightVisibleFrozen != -1)
			{
				DataGridViewElementStates rowState = this.GetRowState(rowIndex);
				if ((rowState & DataGridViewElementStates.Visible) != DataGridViewElementStates.None)
				{
					int num = adding ? 1 : -1;
					int num2 = 0;
					if (this.rowsHeightVisible != -1 || (this.rowsHeightVisibleFrozen != -1 && (rowState & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)))
					{
						num2 = (adding ? dataGridViewRow.GetHeight(rowIndex) : (-dataGridViewRow.GetHeight(rowIndex)));
						dataGridViewRow = this.SharedRow(rowIndex);
					}
					if (this.rowCountsVisible != -1)
					{
						this.rowCountsVisible += num;
					}
					if (this.rowsHeightVisible != -1)
					{
						this.rowsHeightVisible += num2;
					}
					if ((rowState & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleFrozen != -1)
						{
							this.rowCountsVisibleFrozen += num;
						}
						if (this.rowsHeightVisibleFrozen != -1)
						{
							this.rowsHeightVisibleFrozen += num2;
						}
					}
					if ((rowState & (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible) && this.rowCountsVisibleSelected != -1)
					{
						this.rowCountsVisibleSelected += num;
					}
				}
			}
		}

		// Token: 0x04000D6F RID: 3439
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04000D70 RID: 3440
		private DataGridViewRowCollection.RowArrayList items;

		// Token: 0x04000D71 RID: 3441
		private List<DataGridViewElementStates> rowStates;

		// Token: 0x04000D72 RID: 3442
		private int rowCountsVisible;

		// Token: 0x04000D73 RID: 3443
		private int rowCountsVisibleFrozen;

		// Token: 0x04000D74 RID: 3444
		private int rowCountsVisibleSelected;

		// Token: 0x04000D75 RID: 3445
		private int rowsHeightVisible;

		// Token: 0x04000D76 RID: 3446
		private int rowsHeightVisibleFrozen;

		// Token: 0x04000D77 RID: 3447
		private DataGridView dataGridView;

		// Token: 0x020005BB RID: 1467
		private class RowArrayList : ArrayList
		{
			// Token: 0x060059BB RID: 22971 RVA: 0x00179D37 File Offset: 0x00177F37
			public RowArrayList(DataGridViewRowCollection owner)
			{
				this.owner = owner;
			}

			// Token: 0x060059BC RID: 22972 RVA: 0x00179D46 File Offset: 0x00177F46
			public void CustomSort(DataGridViewRowCollection.RowComparer rowComparer)
			{
				this.rowComparer = rowComparer;
				this.CustomQuickSort(0, this.Count - 1);
			}

			// Token: 0x060059BD RID: 22973 RVA: 0x00179D60 File Offset: 0x00177F60
			private void CustomQuickSort(int left, int right)
			{
				while (right - left >= 2)
				{
					int num = left + right >> 1;
					object obj = this.Pivot(left, num, right);
					int num2 = left + 1;
					int num3 = right - 1;
					do
					{
						if (num != num2)
						{
							if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(num2), obj, num2, num) < 0)
							{
								num2++;
								continue;
							}
						}
						while (num != num3 && this.rowComparer.CompareObjects(obj, this.rowComparer.GetComparedObject(num3), num, num3) < 0)
						{
							num3--;
						}
						if (num2 > num3)
						{
							break;
						}
						if (num2 < num3)
						{
							this.owner.SwapSortedRows(num2, num3);
							if (num2 == num)
							{
								num = num3;
							}
							else if (num3 == num)
							{
								num = num2;
							}
						}
						num2++;
						num3--;
					}
					while (num2 <= num3);
					if (num3 - left <= right - num2)
					{
						if (left < num3)
						{
							this.CustomQuickSort(left, num3);
						}
						left = num2;
					}
					else
					{
						if (num2 < right)
						{
							this.CustomQuickSort(num2, right);
						}
						right = num3;
					}
					if (left >= right)
					{
						return;
					}
				}
				if (right - left > 0 && this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(right), left, right) > 0)
				{
					this.owner.SwapSortedRows(left, right);
				}
			}

			// Token: 0x060059BE RID: 22974 RVA: 0x00179E74 File Offset: 0x00178074
			private object Pivot(int left, int center, int right)
			{
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(center), left, center) > 0)
				{
					this.owner.SwapSortedRows(left, center);
				}
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(right), left, right) > 0)
				{
					this.owner.SwapSortedRows(left, right);
				}
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(center), this.rowComparer.GetComparedObject(right), center, right) > 0)
				{
					this.owner.SwapSortedRows(center, right);
				}
				return this.rowComparer.GetComparedObject(center);
			}

			// Token: 0x0400393E RID: 14654
			private DataGridViewRowCollection owner;

			// Token: 0x0400393F RID: 14655
			private DataGridViewRowCollection.RowComparer rowComparer;
		}

		// Token: 0x020005BC RID: 1468
		private class RowComparer
		{
			// Token: 0x060059BF RID: 22975 RVA: 0x00179F2C File Offset: 0x0017812C
			public RowComparer(DataGridViewRowCollection dataGridViewRows, IComparer customComparer, bool ascending)
			{
				this.dataGridView = dataGridViewRows.DataGridView;
				this.dataGridViewRows = dataGridViewRows;
				this.dataGridViewSortedColumn = this.dataGridView.SortedColumn;
				if (this.dataGridViewSortedColumn == null)
				{
					this.sortedColumnIndex = -1;
				}
				else
				{
					this.sortedColumnIndex = this.dataGridViewSortedColumn.Index;
				}
				this.customComparer = customComparer;
				this.ascending = ascending;
			}

			// Token: 0x060059C0 RID: 22976 RVA: 0x00179F94 File Offset: 0x00178194
			internal object GetComparedObject(int rowIndex)
			{
				if (this.dataGridView.NewRowIndex != -1 && rowIndex == this.dataGridView.NewRowIndex)
				{
					return DataGridViewRowCollection.RowComparer.max;
				}
				if (this.customComparer == null)
				{
					DataGridViewRow dataGridViewRow = this.dataGridViewRows.SharedRow(rowIndex);
					return dataGridViewRow.Cells[this.sortedColumnIndex].GetValueInternal(rowIndex);
				}
				return this.dataGridViewRows[rowIndex];
			}

			// Token: 0x060059C1 RID: 22977 RVA: 0x00179FFC File Offset: 0x001781FC
			internal int CompareObjects(object value1, object value2, int rowIndex1, int rowIndex2)
			{
				if (value1 is DataGridViewRowCollection.RowComparer.ComparedObjectMax)
				{
					return 1;
				}
				if (value2 is DataGridViewRowCollection.RowComparer.ComparedObjectMax)
				{
					return -1;
				}
				int num = 0;
				if (this.customComparer == null)
				{
					if (!this.dataGridView.OnSortCompare(this.dataGridViewSortedColumn, value1, value2, rowIndex1, rowIndex2, out num))
					{
						if (!(value1 is IComparable) && !(value2 is IComparable))
						{
							if (value1 == null)
							{
								if (value2 == null)
								{
									num = 0;
								}
								else
								{
									num = 1;
								}
							}
							else if (value2 == null)
							{
								num = -1;
							}
							else
							{
								num = Comparer.Default.Compare(value1.ToString(), value2.ToString());
							}
						}
						else
						{
							num = Comparer.Default.Compare(value1, value2);
						}
						if (num == 0)
						{
							if (this.ascending)
							{
								num = rowIndex1 - rowIndex2;
							}
							else
							{
								num = rowIndex2 - rowIndex1;
							}
						}
					}
				}
				else
				{
					num = this.customComparer.Compare(value1, value2);
				}
				if (this.ascending)
				{
					return num;
				}
				return -num;
			}

			// Token: 0x04003940 RID: 14656
			private DataGridView dataGridView;

			// Token: 0x04003941 RID: 14657
			private DataGridViewRowCollection dataGridViewRows;

			// Token: 0x04003942 RID: 14658
			private DataGridViewColumn dataGridViewSortedColumn;

			// Token: 0x04003943 RID: 14659
			private int sortedColumnIndex;

			// Token: 0x04003944 RID: 14660
			private IComparer customComparer;

			// Token: 0x04003945 RID: 14661
			private bool ascending;

			// Token: 0x04003946 RID: 14662
			private static DataGridViewRowCollection.RowComparer.ComparedObjectMax max = new DataGridViewRowCollection.RowComparer.ComparedObjectMax();

			// Token: 0x0200088F RID: 2191
			private class ComparedObjectMax
			{
			}
		}

		// Token: 0x020005BD RID: 1469
		private class UnsharingRowEnumerator : IEnumerator
		{
			// Token: 0x060059C3 RID: 22979 RVA: 0x0017A0CB File Offset: 0x001782CB
			public UnsharingRowEnumerator(DataGridViewRowCollection owner)
			{
				this.owner = owner;
				this.current = -1;
			}

			// Token: 0x060059C4 RID: 22980 RVA: 0x0017A0E1 File Offset: 0x001782E1
			bool IEnumerator.MoveNext()
			{
				if (this.current < this.owner.Count - 1)
				{
					this.current++;
					return true;
				}
				this.current = this.owner.Count;
				return false;
			}

			// Token: 0x060059C5 RID: 22981 RVA: 0x0017A11A File Offset: 0x0017831A
			void IEnumerator.Reset()
			{
				this.current = -1;
			}

			// Token: 0x170015B8 RID: 5560
			// (get) Token: 0x060059C6 RID: 22982 RVA: 0x0017A124 File Offset: 0x00178324
			object IEnumerator.Current
			{
				get
				{
					if (this.current == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_EnumNotStarted"));
					}
					if (this.current == this.owner.Count)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_EnumFinished"));
					}
					return this.owner[this.current];
				}
			}

			// Token: 0x04003947 RID: 14663
			private DataGridViewRowCollection owner;

			// Token: 0x04003948 RID: 14664
			private int current;
		}
	}
}
