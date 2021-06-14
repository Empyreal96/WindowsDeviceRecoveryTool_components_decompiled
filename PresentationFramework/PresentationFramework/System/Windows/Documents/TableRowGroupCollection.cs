using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>Provides standard facilities for creating and managing a type-safe, ordered collection of <see cref="T:System.Windows.Documents.TableRowGroup" /> objects.</summary>
	// Token: 0x020003EA RID: 1002
	public sealed class TableRowGroupCollection : IList<TableRowGroup>, ICollection<TableRowGroup>, IEnumerable<TableRowGroup>, IEnumerable, IList, ICollection
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x000F63BE File Offset: 0x000F45BE
		internal TableRowGroupCollection(Table owner)
		{
			this._rowGroupCollectionInternal = new TableTextElementCollectionInternal<Table, TableRowGroup>(owner);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified array starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableRowGroup" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x06003709 RID: 14089 RVA: 0x000F63D2 File Offset: 0x000F45D2
		public void CopyTo(Array array, int index)
		{
			this._rowGroupCollectionInternal.CopyTo(array, index);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified <see cref="T:System.Windows.Documents.TableRowGroup" /> array of starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional <see cref="T:System.Windows.Documents.TableRowGroup" /> array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableRowGroup" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x0600370A RID: 14090 RVA: 0x000F63E1 File Offset: 0x000F45E1
		public void CopyTo(TableRowGroup[] array, int index)
		{
			this._rowGroupCollectionInternal.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x0600370B RID: 14091 RVA: 0x000F63F0 File Offset: 0x000F45F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._rowGroupCollectionInternal.GetEnumerator();
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x000F63FD File Offset: 0x000F45FD
		IEnumerator<TableRowGroup> IEnumerable<TableRowGroup>.GetEnumerator()
		{
			return ((IEnumerable<TableRowGroup>)this._rowGroupCollectionInternal).GetEnumerator();
		}

		/// <summary>Appends a specified item to the collection.</summary>
		/// <param name="item">An item to append to the collection.</param>
		/// <exception cref="T:System.ArgumentException">Raised when item already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x0600370D RID: 14093 RVA: 0x000F640A File Offset: 0x000F460A
		public void Add(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.Add(item);
		}

		/// <summary>Clears all items from the collection.</summary>
		// Token: 0x0600370E RID: 14094 RVA: 0x000F6418 File Offset: 0x000F4618
		public void Clear()
		{
			this._rowGroupCollectionInternal.Clear();
		}

		/// <summary>Queries for the presence of a specified item in the collection.</summary>
		/// <param name="item">An item to query for the presence of in the collection.</param>
		/// <returns>
		///     true if the specified item is present in the collection; otherwise, false.</returns>
		// Token: 0x0600370F RID: 14095 RVA: 0x000F6425 File Offset: 0x000F4625
		public bool Contains(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.Contains(item);
		}

		/// <summary>Returns the zero-based index of specified collection item.</summary>
		/// <param name="item">A collection item to return the index of.</param>
		/// <returns>The zero-based index of the specified collection item, or -1 if the specified item is not a member of the collection.</returns>
		// Token: 0x06003710 RID: 14096 RVA: 0x000F6433 File Offset: 0x000F4633
		public int IndexOf(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.IndexOf(item);
		}

		/// <summary>Inserts a specified item in the collection at a specified index position.</summary>
		/// <param name="index">A zero-based index that specifies the position in the collection at which to insert <paramref name="item" />.</param>
		/// <param name="item">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x06003711 RID: 14097 RVA: 0x000F6441 File Offset: 0x000F4641
		public void Insert(int index, TableRowGroup item)
		{
			this._rowGroupCollectionInternal.Insert(index, item);
		}

		/// <summary>Removes a specified item from the collection.</summary>
		/// <param name="item">An item to remove from the collection.</param>
		/// <returns>
		///     true if the specified item was found and removed; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if item is not present in the collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x06003712 RID: 14098 RVA: 0x000F6450 File Offset: 0x000F4650
		public bool Remove(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.Remove(item);
		}

		/// <summary>Removes an item, specified by index, from the collection.</summary>
		/// <param name="index">A zero-based index that specifies the collection item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowGroupCollection.Count" />.</exception>
		// Token: 0x06003713 RID: 14099 RVA: 0x000F645E File Offset: 0x000F465E
		public void RemoveAt(int index)
		{
			this._rowGroupCollectionInternal.RemoveAt(index);
		}

		/// <summary>Removes a range of items, specified by beginning index and count, from the collection.</summary>
		/// <param name="index">A zero-based index indicating the beginning of a range of items to remove.</param>
		/// <param name="count">The number of items to remove, beginning form the position specified by <paramref name="index" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index or count is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowGroupCollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when index and count do not specify a valid range in this collection.</exception>
		// Token: 0x06003714 RID: 14100 RVA: 0x000F646C File Offset: 0x000F466C
		public void RemoveRange(int index, int count)
		{
			this._rowGroupCollectionInternal.RemoveRange(index, count);
		}

		/// <summary>Optimizes memory consumption for the collection by setting the underlying collection <see cref="P:System.Windows.Documents.TableRowGroupCollection.Capacity" /> equal to the <see cref="P:System.Windows.Documents.TableRowGroupCollection.Count" /> of items currently in the collection.</summary>
		// Token: 0x06003715 RID: 14101 RVA: 0x000F647B File Offset: 0x000F467B
		public void TrimToSize()
		{
			this._rowGroupCollectionInternal.TrimToSize();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06003716 RID: 14102 RVA: 0x000F6488 File Offset: 0x000F4688
		int IList.Add(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).Add(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
		// Token: 0x06003717 RID: 14103 RVA: 0x000F6496 File Offset: 0x000F4696
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003718 RID: 14104 RVA: 0x000F649E File Offset: 0x000F469E
		bool IList.Contains(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).Contains(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06003719 RID: 14105 RVA: 0x000F64AC File Offset: 0x000F46AC
		int IList.IndexOf(object value)
		{
			return ((IList)this._rowGroupCollectionInternal).IndexOf(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />.</param>
		// Token: 0x0600371A RID: 14106 RVA: 0x000F64BA File Offset: 0x000F46BA
		void IList.Insert(int index, object value)
		{
			((IList)this._rowGroupCollectionInternal).Insert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableRowGroupCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x0600371B RID: 14107 RVA: 0x000F64C9 File Offset: 0x000F46C9
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal).IsFixedSize;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableRowGroupCollection" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x0600371C RID: 14108 RVA: 0x000F64D6 File Offset: 0x000F46D6
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal).IsReadOnly;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Documents.TableRowGroupCollection" />.</param>
		// Token: 0x0600371D RID: 14109 RVA: 0x000F64E3 File Offset: 0x000F46E3
		void IList.Remove(object value)
		{
			((IList)this._rowGroupCollectionInternal).Remove(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the item to remove. </param>
		// Token: 0x0600371E RID: 14110 RVA: 0x000F64F1 File Offset: 0x000F46F1
		void IList.RemoveAt(int index)
		{
			((IList)this._rowGroupCollectionInternal).RemoveAt(index);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000E1A RID: 3610
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._rowGroupCollectionInternal)[index];
			}
			set
			{
				((IList)this._rowGroupCollectionInternal)[index] = value;
			}
		}

		/// <summary>Gets the number of items currently contained by the collection.</summary>
		/// <returns>The number of items currently contained by the collection.</returns>
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003721 RID: 14113 RVA: 0x000F651C File Offset: 0x000F471C
		public int Count
		{
			get
			{
				return this._rowGroupCollectionInternal.Count;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003722 RID: 14114 RVA: 0x000F6529 File Offset: 0x000F4729
		public bool IsReadOnly
		{
			get
			{
				return this._rowGroupCollectionInternal.IsReadOnly;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003723 RID: 14115 RVA: 0x000F6536 File Offset: 0x000F4736
		public bool IsSynchronized
		{
			get
			{
				return this._rowGroupCollectionInternal.IsSynchronized;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003724 RID: 14116 RVA: 0x000F6543 File Offset: 0x000F4743
		public object SyncRoot
		{
			get
			{
				return this._rowGroupCollectionInternal.SyncRoot;
			}
		}

		/// <summary>Gets or sets the pre-allocated collection item capacity for this collection.</summary>
		/// <returns>The pre-allocated collection item capacity for this collection.  The default value is 8.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when an attempt is made to set <see cref="P:System.Windows.Documents.TableCellCollection.Capacity" /> to a value that is less than the current value of <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x000F6550 File Offset: 0x000F4750
		// (set) Token: 0x06003726 RID: 14118 RVA: 0x000F655D File Offset: 0x000F475D
		public int Capacity
		{
			get
			{
				return this._rowGroupCollectionInternal.Capacity;
			}
			set
			{
				this._rowGroupCollectionInternal.Capacity = value;
			}
		}

		/// <summary>Gets the collection item at a specified index.  This is an indexed property.</summary>
		/// <param name="index">A zero-based index specifying the position of the collection item to retrieve.</param>
		/// <returns>The collection item at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowGroupCollection.Count" />.</exception>
		// Token: 0x17000E20 RID: 3616
		public TableRowGroup this[int index]
		{
			get
			{
				return this._rowGroupCollectionInternal[index];
			}
			set
			{
				this._rowGroupCollectionInternal[index] = value;
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000F6588 File Offset: 0x000F4788
		internal void InternalAdd(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.InternalAdd(item);
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x000F6596 File Offset: 0x000F4796
		internal void InternalRemove(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.InternalRemove(item);
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x000F65A4 File Offset: 0x000F47A4
		private void EnsureCapacity(int min)
		{
			this._rowGroupCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x000F65B2 File Offset: 0x000F47B2
		private void PrivateConnectChild(int index, TableRowGroup item)
		{
			this._rowGroupCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000F65C1 File Offset: 0x000F47C1
		private void PrivateDisconnectChild(TableRowGroup item)
		{
			this._rowGroupCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x000F65CF File Offset: 0x000F47CF
		private bool BelongsToOwner(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000F65DD File Offset: 0x000F47DD
		private int FindInsertionIndex(TableRowGroup item)
		{
			return this._rowGroupCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x000F65EB File Offset: 0x000F47EB
		// (set) Token: 0x06003731 RID: 14129 RVA: 0x000F65F8 File Offset: 0x000F47F8
		private int PrivateCapacity
		{
			get
			{
				return this._rowGroupCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._rowGroupCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002557 RID: 9559
		private TableTextElementCollectionInternal<Table, TableRowGroup> _rowGroupCollectionInternal;
	}
}
