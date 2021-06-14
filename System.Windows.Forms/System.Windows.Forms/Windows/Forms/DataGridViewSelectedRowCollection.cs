using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.DataGridViewRow" /> objects that are selected in a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x02000209 RID: 521
	[ListBindable(false)]
	public class DataGridViewSelectedRowCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Implements the <see cref="M:System.Collections.IList.Add(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The item to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <returns>The index at which <paramref name="value" /> was inserted.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB4 RID: 8116 RVA: 0x0009ED59 File Offset: 0x0009CF59
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Clear" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB5 RID: 8117 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified value is contained in the collection. </summary>
		/// <param name="value">An object to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="value" /> parameter is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001FB6 RID: 8118 RVA: 0x0009EF0F File Offset: 0x0009D10F
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the index of the specified element. </summary>
		/// <param name="value">The element to locate in the collection.</param>
		/// <returns>The zero-based index of the <paramref name="value" /> parameter if it is found in the collection; otherwise, -1.</returns>
		// Token: 0x06001FB7 RID: 8119 RVA: 0x0009EF1D File Offset: 0x0009D11D
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to add to the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB8 RID: 8120 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Remove(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB9 RID: 8121 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FBA RID: 8122 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The index of the element to get from the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <returns>The element at the specified index. </returns>
		/// <exception cref="T:System.NotSupportedException">The property is set.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of rows in the collection.</exception>
		// Token: 0x1700078C RID: 1932
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
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
		// Token: 0x06001FBF RID: 8127 RVA: 0x0009EF39 File Offset: 0x0009D139
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements in the collection.</summary>
		/// <returns>The number of elements in the collection.</returns>
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x0009EF48 File Offset: 0x0009D148
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
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</returns>
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that can be used to iterate through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06001FC3 RID: 8131 RVA: 0x0009EF55 File Offset: 0x0009D155
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0009EF62 File Offset: 0x0009D162
		internal DataGridViewSelectedRowCollection()
		{
		}

		/// <summary>Gets the list of elements contained in the <see cref="T:System.Windows.Forms.BaseCollection" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing the elements of the collection. This property returns <see langword="null" /> unless overridden in a derived class.</returns>
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0009EF75 File Offset: 0x0009D175
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the row at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridViewRow" /> in the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewRow" /> at the current index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of rows in the collection.</exception>
		// Token: 0x17000791 RID: 1937
		public DataGridViewRow this[int index]
		{
			get
			{
				return (DataGridViewRow)this.items[index];
			}
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0009EF90 File Offset: 0x0009D190
		internal int Add(DataGridViewRow dataGridViewRow)
		{
			return this.items.Add(dataGridViewRow);
		}

		/// <summary>Clears the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FC8 RID: 8136 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified row is contained in the collection.</summary>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="dataGridViewRow" /> is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001FC9 RID: 8137 RVA: 0x0009EF9E File Offset: 0x0009D19E
		public bool Contains(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow) != -1;
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
		// Token: 0x06001FCA RID: 8138 RVA: 0x0009EF39 File Offset: 0x0009D139
		public void CopyTo(DataGridViewRow[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Inserts a row into the collection at the specified position.</summary>
		/// <param name="index">The zero-based index at which <paramref name="dataGridViewRow" /> should be inserted. </param>
		/// <param name="dataGridViewRow">The <see cref="T:System.Windows.Forms.DataGridViewRow" /> to insert into the <see cref="T:System.Windows.Forms.DataGridViewSelectedRowCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FCB RID: 8139 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewRow dataGridViewRow)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04000DB1 RID: 3505
		private ArrayList items = new ArrayList();
	}
}
