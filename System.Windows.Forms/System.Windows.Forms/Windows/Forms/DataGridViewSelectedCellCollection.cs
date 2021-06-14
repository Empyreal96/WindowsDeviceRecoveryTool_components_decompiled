using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of cells that are selected in a <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
	// Token: 0x02000207 RID: 519
	[ListBindable(false)]
	public class DataGridViewSelectedCellCollection : BaseCollection, IList, ICollection, IEnumerable
	{
		/// <summary>Implements the <see cref="M:System.Collections.IList.Add(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The item to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F83 RID: 8067 RVA: 0x0009ED59 File Offset: 0x0009CF59
		int IList.Add(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Clear" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F84 RID: 8068 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified cell is contained in the collection.</summary>
		/// <param name="value">A <see cref="T:System.Windows.Forms.DataGridViewCell" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F85 RID: 8069 RVA: 0x0009ED6A File Offset: 0x0009CF6A
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		/// <summary>Returns the index of the specified cell.</summary>
		/// <param name="value">The cell to locate in the collection.</param>
		/// <returns>The zero-based index of the <paramref name="value" /> parameter if it is found in the collection; otherwise, -1.</returns>
		// Token: 0x06001F86 RID: 8070 RVA: 0x0009ED78 File Offset: 0x0009CF78
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F87 RID: 8071 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.Remove(System.Object)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The object to be removed from the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F88 RID: 8072 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.Remove(object value)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Implements the <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" /> method. Always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F89 RID: 8073 RVA: 0x0009ED59 File Offset: 0x0009CF59
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x0000E214 File Offset: 0x0000C414
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
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0000E214 File Offset: 0x0000C414
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> to get from the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">The property is set.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of cells in the collection.</exception>
		// Token: 0x1700077C RID: 1916
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
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001F8E RID: 8078 RVA: 0x0009ED94 File Offset: 0x0009CF94
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of elements in the collection.</summary>
		/// <returns>The number of elements in the collection.</returns>
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0009EDA3 File Offset: 0x0009CFA3
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
		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" />.</returns>
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns an enumerator that can be used to iterate through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06001F92 RID: 8082 RVA: 0x0009EDB0 File Offset: 0x0009CFB0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0009EDBD File Offset: 0x0009CFBD
		internal DataGridViewSelectedCellCollection()
		{
		}

		/// <summary>Gets a list of elements in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing the elements of the collection.</returns>
		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0009EDD0 File Offset: 0x0009CFD0
		protected override ArrayList List
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>Gets the cell at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> to get from the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewCell" /> at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.-or-
		///         <paramref name="index" /> is equal to or greater than the number of cells in the collection.</exception>
		// Token: 0x17000781 RID: 1921
		public DataGridViewCell this[int index]
		{
			get
			{
				return (DataGridViewCell)this.items[index];
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0009EDEB File Offset: 0x0009CFEB
		internal int Add(DataGridViewCell dataGridViewCell)
		{
			return this.items.Add(dataGridViewCell);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0009EDFC File Offset: 0x0009CFFC
		internal void AddCellLinkedList(DataGridViewCellLinkedList dataGridViewCells)
		{
			foreach (object obj in ((IEnumerable)dataGridViewCells))
			{
				DataGridViewCell value = (DataGridViewCell)obj;
				this.items.Add(value);
			}
		}

		/// <summary>Clears the collection. </summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F98 RID: 8088 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Clear()
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		/// <summary>Determines whether the specified cell is contained in the collection.</summary>
		/// <param name="dataGridViewCell">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> to locate in the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="dataGridViewCell" /> is in the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F99 RID: 8089 RVA: 0x0009EE58 File Offset: 0x0009D058
		public bool Contains(DataGridViewCell dataGridViewCell)
		{
			return this.items.IndexOf(dataGridViewCell) != -1;
		}

		/// <summary>Copies the elements of the collection to the specified <see cref="T:System.Windows.Forms.DataGridViewCell" /> array, starting at the specified index.</summary>
		/// <param name="array">The one-dimensional array of type <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Windows.Forms.DataGridViewCellCollection" /> cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06001F9A RID: 8090 RVA: 0x0009ED94 File Offset: 0x0009CF94
		public void CopyTo(DataGridViewCell[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Inserts a cell into the collection.</summary>
		/// <param name="index">The index at which <paramref name="dataGridViewCell" /> should be inserted.</param>
		/// <param name="dataGridViewCell">The object to be added to the <see cref="T:System.Windows.Forms.DataGridViewSelectedCellCollection" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06001F9B RID: 8091 RVA: 0x0009ED59 File Offset: 0x0009CF59
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Insert(int index, DataGridViewCell dataGridViewCell)
		{
			throw new NotSupportedException(SR.GetString("DataGridView_ReadOnlyCollection"));
		}

		// Token: 0x04000DAF RID: 3503
		private ArrayList items = new ArrayList();
	}
}
