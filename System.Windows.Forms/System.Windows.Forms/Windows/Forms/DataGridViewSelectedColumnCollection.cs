using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.DataGridViewColumn" /> objects that are selected in a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x02000208 RID: 520
	[ListBindable(false)]
	public class DataGridViewSelectedColumnCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Implements the <see cref="M:System.Collections.IList.Add(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The item to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <returns>Not applicable. Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F9C RID: 8092 RVA: 0x0009ED59 File Offset: 0x0009CF59
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Clear" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F9D RID: 8093 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified value is contained in the collection.</summary>
		/// <param name="value">An object to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="value" /> parameter is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F9E RID: 8094 RVA: 0x0009EE6C File Offset: 0x0009D06C
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the index of the specified element.</summary>
		/// <param name="value">The element to locate in the collection.</param>
		/// <returns>The zero-based index of the <paramref name="value" /> parameter if it is found in the collection; otherwise, -1.</returns>
		// Token: 0x06001F9F RID: 8095 RVA: 0x0009EE7A File Offset: 0x0009D07A
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FA0 RID: 8096 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Remove(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FA1 RID: 8097 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FA2 RID: 8098 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0000E214 File Offset: 0x0000C414
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
		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The index of the element to get from the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">The property is set.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of columns in the collection.</exception>
		// Token: 0x17000784 RID: 1924
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
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Windows.Forms.DataGridViewColumnCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001FA7 RID: 8103 RVA: 0x0009EE96 File Offset: 0x0009D096
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements in the collection.</summary>
		/// <returns>The number of elements in the collection.</returns>
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0009EEA5 File Offset: 0x0009D0A5
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
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewColumnCollection" />.</returns>
		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06001FAB RID: 8107 RVA: 0x0009EEB2 File Offset: 0x0009D0B2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0009EEBF File Offset: 0x0009D0BF
		internal DataGridViewSelectedColumnCollection()
		{
		}

		/// <summary>Gets the list of elements contained in the <see cref="T:System.Windows.Forms.BaseCollection" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing the elements of the collection. This property returns <see langword="null" /> unless overridden in a derived class.</returns>
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x0009EED2 File Offset: 0x0009D0D2
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the column at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridViewColumn" /> to get from the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of columns in the collection.</exception>
		// Token: 0x17000789 RID: 1929
		public DataGridViewColumn this[int index]
		{
			get
			{
				return (DataGridViewColumn)this.items[index];
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0009EEED File Offset: 0x0009D0ED
		internal int Add(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.Add(dataGridViewColumn);
		}

		/// <summary>Clears the collection.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB0 RID: 8112 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified column is contained in the collection.</summary>
		/// <param name="dataGridViewColumn">A <see cref="T:System.Windows.Forms.DataGridViewColumn" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="dataGridViewColumn" /> parameter is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001FB1 RID: 8113 RVA: 0x0009EEFB File Offset: 0x0009D0FB
		public bool Contains(DataGridViewColumn dataGridViewColumn)
		{
			return this.items.IndexOf(dataGridViewColumn) != -1;
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
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Windows.Forms.DataGridViewColumnCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001FB2 RID: 8114 RVA: 0x0009EE96 File Offset: 0x0009D096
		public void CopyTo(DataGridViewColumn[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Inserts a column into the collection at the specified position.</summary>
		/// <param name="index">The zero-based index at which the column should be inserted. </param>
		/// <param name="dataGridViewColumn">The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> to insert into the <see cref="T:System.Windows.Forms.DataGridViewSelectedColumnCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001FB3 RID: 8115 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewColumn dataGridViewColumn)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04000DB0 RID: 3504
		private ArrayList items = new ArrayList();
	}
}
