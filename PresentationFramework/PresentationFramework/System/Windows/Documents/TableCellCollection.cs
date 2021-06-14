using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>Provides standard facilities for creating and managing a type-safe, ordered collection of <see cref="T:System.Windows.Documents.TableCell" /> objects.</summary>
	// Token: 0x020003E4 RID: 996
	public sealed class TableCellCollection : IList<TableCell>, ICollection<TableCell>, IEnumerable<TableCell>, IEnumerable, IList, ICollection
	{
		// Token: 0x06003647 RID: 13895 RVA: 0x000F561A File Offset: 0x000F381A
		internal TableCellCollection(TableRow owner)
		{
			this._cellCollectionInternal = new TableTextElementCollectionInternal<TableRow, TableCell>(owner);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified array starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional array to which the collection contents will be copied. This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="array" /> includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableCell" />, or if <paramref name="index" /> specifies a position that falls outside the bounds of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> is less than 0.</exception>
		// Token: 0x06003648 RID: 13896 RVA: 0x000F562E File Offset: 0x000F382E
		public void CopyTo(Array array, int index)
		{
			this._cellCollectionInternal.CopyTo(array, index);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified <see cref="T:System.Windows.Documents.TableCell" /> array of starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional <see cref="T:System.Windows.Documents.TableCell" /> array to which the collection contents will be copied. This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> that specifies the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="array" /> includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableCell" />, or if <paramref name="index" /> specifies a position that falls outside of the bounds of <paramref name="array" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> is less than 0.</exception>
		// Token: 0x06003649 RID: 13897 RVA: 0x000F563D File Offset: 0x000F383D
		public void CopyTo(TableCell[] array, int index)
		{
			this._cellCollectionInternal.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x0600364A RID: 13898 RVA: 0x000F564C File Offset: 0x000F384C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._cellCollectionInternal.GetEnumerator();
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000F5659 File Offset: 0x000F3859
		IEnumerator<TableCell> IEnumerable<TableCell>.GetEnumerator()
		{
			return ((IEnumerable<TableCell>)this._cellCollectionInternal).GetEnumerator();
		}

		/// <summary>Appends a specified <see cref="T:System.Windows.Documents.TableCell" /> to the collection of table cells.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Documents.TableCell" /> to append to the collection of table cells.</param>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="item" /> already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="item" /> is null.</exception>
		// Token: 0x0600364C RID: 13900 RVA: 0x000F5666 File Offset: 0x000F3866
		public void Add(TableCell item)
		{
			this._cellCollectionInternal.Add(item);
		}

		/// <summary>Clears all items from the collection.</summary>
		// Token: 0x0600364D RID: 13901 RVA: 0x000F5674 File Offset: 0x000F3874
		public void Clear()
		{
			this._cellCollectionInternal.Clear();
		}

		/// <summary>Queries for the presence of a specified item in the collection.</summary>
		/// <param name="item">An item to query for the presence of in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <paramref name="item" /> is present in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600364E RID: 13902 RVA: 0x000F5681 File Offset: 0x000F3881
		public bool Contains(TableCell item)
		{
			return this._cellCollectionInternal.Contains(item);
		}

		/// <summary>Returns the zero-based index of specified collection item.</summary>
		/// <param name="item">A collection item to return the index of.</param>
		/// <returns>The zero-based index of the specified collection item, or -1 if the specified item is not a member of the collection.</returns>
		// Token: 0x0600364F RID: 13903 RVA: 0x000F568F File Offset: 0x000F388F
		public int IndexOf(TableCell item)
		{
			return this._cellCollectionInternal.IndexOf(item);
		}

		/// <summary>Inserts a specified item in the collection at a specified index position.</summary>
		/// <param name="index">A zero-based index that specifies the position in the collection at which to insert <paramref name="item" />.</param>
		/// <param name="item">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="item" /> is null.</exception>
		// Token: 0x06003650 RID: 13904 RVA: 0x000F569D File Offset: 0x000F389D
		public void Insert(int index, TableCell item)
		{
			this._cellCollectionInternal.Insert(index, item);
		}

		/// <summary>Removes a specified item from the collection.</summary>
		/// <param name="item">An item to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the specified item was found and removed; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if <paramref name="item" /> is not present in the collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="item" /> is null.</exception>
		// Token: 0x06003651 RID: 13905 RVA: 0x000F56AC File Offset: 0x000F38AC
		public bool Remove(TableCell item)
		{
			return this._cellCollectionInternal.Remove(item);
		}

		/// <summary>Removes an item, specified by index, from the collection.</summary>
		/// <param name="index">A zero-based index that specifies the collection item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> is less than zero, or when <paramref name="index" /> is greater than or equal to <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x06003652 RID: 13906 RVA: 0x000F56BA File Offset: 0x000F38BA
		public void RemoveAt(int index)
		{
			this._cellCollectionInternal.RemoveAt(index);
		}

		/// <summary>Removes a range of items, specified by beginning index and count, from the collection.</summary>
		/// <param name="index">A zero-based index that indicates the beginning of a range of items to remove.</param>
		/// <param name="count">The number of items to remove, beginning from the position specified by <paramref name="index" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> or <paramref name="count" /> is less than zero, or when <paramref name="index" /> is greater than or equal to <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in this collection.</exception>
		// Token: 0x06003653 RID: 13907 RVA: 0x000F56C8 File Offset: 0x000F38C8
		public void RemoveRange(int index, int count)
		{
			this._cellCollectionInternal.RemoveRange(index, count);
		}

		/// <summary>Optimizes memory consumption for the collection by setting the underlying collection <see cref="P:System.Windows.Documents.TableCellCollection.Capacity" /> equal to the <see cref="P:System.Windows.Documents.TableCellCollection.Count" /> of items currently in the collection.</summary>
		// Token: 0x06003654 RID: 13908 RVA: 0x000F56D7 File Offset: 0x000F38D7
		public void TrimToSize()
		{
			this._cellCollectionInternal.TrimToSize();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Documents.TableCellCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06003655 RID: 13909 RVA: 0x000F56E4 File Offset: 0x000F38E4
		int IList.Add(object value)
		{
			return ((IList)this._cellCollectionInternal).Add(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
		// Token: 0x06003656 RID: 13910 RVA: 0x000F56F2 File Offset: 0x000F38F2
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableCellCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Documents.TableCellCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003657 RID: 13911 RVA: 0x000F56FA File Offset: 0x000F38FA
		bool IList.Contains(object value)
		{
			return ((IList)this._cellCollectionInternal).Contains(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableCellCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06003658 RID: 13912 RVA: 0x000F5708 File Offset: 0x000F3908
		int IList.IndexOf(object value)
		{
			return ((IList)this._cellCollectionInternal).IndexOf(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Documents.TableCellCollection" />.</param>
		// Token: 0x06003659 RID: 13913 RVA: 0x000F5716 File Offset: 0x000F3916
		void IList.Insert(int index, object value)
		{
			((IList)this._cellCollectionInternal).Insert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableCellCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x0600365A RID: 13914 RVA: 0x000F5725 File Offset: 0x000F3925
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsFixedSize;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableCellCollection" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x0600365B RID: 13915 RVA: 0x000F5732 File Offset: 0x000F3932
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsReadOnly;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Documents.TableCellCollection" />.</param>
		// Token: 0x0600365C RID: 13916 RVA: 0x000F573F File Offset: 0x000F393F
		void IList.Remove(object value)
		{
			((IList)this._cellCollectionInternal).Remove(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		// Token: 0x0600365D RID: 13917 RVA: 0x000F574D File Offset: 0x000F394D
		void IList.RemoveAt(int index)
		{
			((IList)this._cellCollectionInternal).RemoveAt(index);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000DE3 RID: 3555
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._cellCollectionInternal)[index];
			}
			set
			{
				((IList)this._cellCollectionInternal)[index] = value;
			}
		}

		/// <summary>Gets the number of items currently contained by the collection.</summary>
		/// <returns>The number of items currently contained by the collection.</returns>
		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x000F5778 File Offset: 0x000F3978
		public int Count
		{
			get
			{
				return this._cellCollectionInternal.Count;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns <see langword="false" />.</returns>
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003661 RID: 13921 RVA: 0x000F5732 File Offset: 0x000F3932
		public bool IsReadOnly
		{
			get
			{
				return ((IList)this._cellCollectionInternal).IsReadOnly;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns <see langword="false" />.</returns>
		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x000F5785 File Offset: 0x000F3985
		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)this._cellCollectionInternal).IsSynchronized;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x000F5792 File Offset: 0x000F3992
		public object SyncRoot
		{
			get
			{
				return ((ICollection)this._cellCollectionInternal).SyncRoot;
			}
		}

		/// <summary>Gets or sets the preallocated collection item capacity for this collection.</summary>
		/// <returns>The preallocated collection item capacity for this collection. The default value is 8.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when an attempt is made to set <see cref="P:System.Windows.Documents.TableCellCollection.Capacity" /> to a value that is less than the current value of <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x000F579F File Offset: 0x000F399F
		// (set) Token: 0x06003665 RID: 13925 RVA: 0x000F57AC File Offset: 0x000F39AC
		public int Capacity
		{
			get
			{
				return this._cellCollectionInternal.Capacity;
			}
			set
			{
				this._cellCollectionInternal.Capacity = value;
			}
		}

		/// <summary>Gets or sets the collection item at a specified index. This is an indexed property.</summary>
		/// <param name="index">A zero-based index that specifies the position of the collection item.</param>
		/// <returns>The collection item at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when <paramref name="index" /> is less than zero, or when <paramref name="index" /> is greater than or equal to <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x17000DE9 RID: 3561
		public TableCell this[int index]
		{
			get
			{
				return this._cellCollectionInternal[index];
			}
			set
			{
				this._cellCollectionInternal[index] = value;
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000F57D7 File Offset: 0x000F39D7
		internal void InternalAdd(TableCell item)
		{
			this._cellCollectionInternal.InternalAdd(item);
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000F57E5 File Offset: 0x000F39E5
		internal void InternalRemove(TableCell item)
		{
			this._cellCollectionInternal.InternalRemove(item);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000F57F3 File Offset: 0x000F39F3
		private void EnsureCapacity(int min)
		{
			this._cellCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000F5801 File Offset: 0x000F3A01
		private void PrivateConnectChild(int index, TableCell item)
		{
			this._cellCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000F5810 File Offset: 0x000F3A10
		private void PrivateDisconnectChild(TableCell item)
		{
			this._cellCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000F581E File Offset: 0x000F3A1E
		private bool BelongsToOwner(TableCell item)
		{
			return this._cellCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x000F582C File Offset: 0x000F3A2C
		private int FindInsertionIndex(TableCell item)
		{
			return this._cellCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x000F583A File Offset: 0x000F3A3A
		// (set) Token: 0x06003670 RID: 13936 RVA: 0x000F5847 File Offset: 0x000F3A47
		private int PrivateCapacity
		{
			get
			{
				return this._cellCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._cellCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002546 RID: 9542
		private TableTextElementCollectionInternal<TableRow, TableCell> _cellCollectionInternal;
	}
}
