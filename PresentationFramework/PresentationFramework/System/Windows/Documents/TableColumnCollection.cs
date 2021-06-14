using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>Provides standard facilities for creating and managing a type-safe, ordered collection of <see cref="T:System.Windows.Documents.TableColumn" /> objects.</summary>
	// Token: 0x020003E6 RID: 998
	public sealed class TableColumnCollection : IList<TableColumn>, ICollection<TableColumn>, IEnumerable<TableColumn>, IEnumerable, IList, ICollection
	{
		// Token: 0x06003685 RID: 13957 RVA: 0x000F5A43 File Offset: 0x000F3C43
		internal TableColumnCollection(Table owner)
		{
			this._columnCollection = new TableColumnCollectionInternal(owner);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified array starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableColumn" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x06003686 RID: 13958 RVA: 0x000F5A57 File Offset: 0x000F3C57
		public void CopyTo(Array array, int index)
		{
			this._columnCollection.CopyTo(array, index);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified <see cref="T:System.Windows.Documents.TableColumn" /> array of starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional <see cref="T:System.Windows.Documents.TableColumn" /> array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableColumn" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x06003687 RID: 13959 RVA: 0x000F5A66 File Offset: 0x000F3C66
		public void CopyTo(TableColumn[] array, int index)
		{
			this._columnCollection.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06003688 RID: 13960 RVA: 0x000F5A75 File Offset: 0x000F3C75
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._columnCollection.GetEnumerator();
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000F5A82 File Offset: 0x000F3C82
		IEnumerator<TableColumn> IEnumerable<TableColumn>.GetEnumerator()
		{
			return ((IEnumerable<TableColumn>)this._columnCollection).GetEnumerator();
		}

		/// <summary>Appends a specified item to the collection.</summary>
		/// <param name="item">A table column to append to the collection of columns.</param>
		/// <exception cref="T:System.ArgumentException">Raised when item already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x0600368A RID: 13962 RVA: 0x000F5A8F File Offset: 0x000F3C8F
		public void Add(TableColumn item)
		{
			this._columnCollection.Add(item);
		}

		/// <summary>Clears all items from the collection.</summary>
		// Token: 0x0600368B RID: 13963 RVA: 0x000F5A9D File Offset: 0x000F3C9D
		public void Clear()
		{
			this._columnCollection.Clear();
		}

		/// <summary>Queries for the presence of a specified item in the collection.</summary>
		/// <param name="item">An item to query for the presence of in the collection.</param>
		/// <returns>
		///     true if the specified item is present in the collection; otherwise, false.</returns>
		// Token: 0x0600368C RID: 13964 RVA: 0x000F5AAA File Offset: 0x000F3CAA
		public bool Contains(TableColumn item)
		{
			return this._columnCollection.Contains(item);
		}

		/// <summary>Returns the zero-based index of specified collection item.</summary>
		/// <param name="item">A collection item to return the index of.</param>
		/// <returns>The zero-based index of the specified collection item, or -1 if the specified item is not a member of the collection.</returns>
		// Token: 0x0600368D RID: 13965 RVA: 0x000F5AB8 File Offset: 0x000F3CB8
		public int IndexOf(TableColumn item)
		{
			return this._columnCollection.IndexOf(item);
		}

		/// <summary>Inserts a specified item in the collection at a specified index position.</summary>
		/// <param name="index">A zero-based index that specifies the position in the collection at which to insert <paramref name="item" />.</param>
		/// <param name="item">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x0600368E RID: 13966 RVA: 0x000F5AC6 File Offset: 0x000F3CC6
		public void Insert(int index, TableColumn item)
		{
			this._columnCollection.Insert(index, item);
		}

		/// <summary>Removes a specified item from the collection.</summary>
		/// <param name="item">An item to remove from the collection.</param>
		/// <returns>
		///     true if the specified item was found and removed; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if item is not present in the collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x0600368F RID: 13967 RVA: 0x000F5AD5 File Offset: 0x000F3CD5
		public bool Remove(TableColumn item)
		{
			return this._columnCollection.Remove(item);
		}

		/// <summary>Removes an item, specified by index, from the collection.</summary>
		/// <param name="index">A zero-based index that specifies the collection item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableColumnCollection.Count" />.</exception>
		// Token: 0x06003690 RID: 13968 RVA: 0x000F5AE3 File Offset: 0x000F3CE3
		public void RemoveAt(int index)
		{
			this._columnCollection.RemoveAt(index);
		}

		/// <summary>Removes a range of items, specified by beginning index and count, from the collection.</summary>
		/// <param name="index">A zero-based index indicating the beginning of a range of items to remove.</param>
		/// <param name="count">The number of items to remove, beginning from the position specified by <paramref name="index" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index or count is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableColumnCollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when index and count do not specify a valid range in this collection.</exception>
		// Token: 0x06003691 RID: 13969 RVA: 0x000F5AF1 File Offset: 0x000F3CF1
		public void RemoveRange(int index, int count)
		{
			this._columnCollection.RemoveRange(index, count);
		}

		/// <summary>Optimizes memory consumption for the collection by setting the underlying collection <see cref="P:System.Windows.Documents.TableColumnCollection.Capacity" /> equal to the <see cref="P:System.Windows.Documents.TableColumnCollection.Count" /> of items currently in the collection.</summary>
		// Token: 0x06003692 RID: 13970 RVA: 0x000F5B00 File Offset: 0x000F3D00
		public void TrimToSize()
		{
			this._columnCollection.TrimToSize();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Add(System.Windows.Documents.TableColumn)" /> method instead.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Documents.TableColumnCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06003693 RID: 13971 RVA: 0x000F5B10 File Offset: 0x000F3D10
		int IList.Add(object value)
		{
			if (!(value is TableColumn))
			{
				throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
				{
					typeof(TableColumn).Name
				}), "value");
			}
			return ((IList)this._columnCollection).Add(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Clear" /> method instead.</summary>
		// Token: 0x06003694 RID: 13972 RVA: 0x000F5B60 File Offset: 0x000F3D60
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Contains(System.Windows.Documents.TableColumn)" /> method instead.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableColumnCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Documents.TableColumnCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003695 RID: 13973 RVA: 0x000F5B68 File Offset: 0x000F3D68
		bool IList.Contains(object value)
		{
			return ((IList)this._columnCollection).Contains(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.IndexOf(System.Windows.Documents.TableColumn)" /> method instead.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableColumnCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06003696 RID: 13974 RVA: 0x000F5B76 File Offset: 0x000F3D76
		int IList.IndexOf(object value)
		{
			return ((IList)this._columnCollection).IndexOf(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Insert(System.Int32,System.Windows.Documents.TableColumn)" /> method instead.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Documents.TableColumnCollection" />.</param>
		// Token: 0x06003697 RID: 13975 RVA: 0x000F5B84 File Offset: 0x000F3D84
		void IList.Insert(int index, object value)
		{
			((IList)this._columnCollection).Insert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableCellCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003698 RID: 13976 RVA: 0x000F5B93 File Offset: 0x000F3D93
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._columnCollection).IsFixedSize;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableColumnCollection" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003699 RID: 13977 RVA: 0x000F5BA0 File Offset: 0x000F3DA0
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._columnCollection).IsReadOnly;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Remove(System.Windows.Documents.TableColumn)" />, <see cref="M:System.Windows.Documents.TableColumnCollection.RemoveAt(System.Int32)" />, or <see cref="M:System.Windows.Documents.TableColumnCollection.RemoveRange(System.Int32,System.Int32)" /> methods instead.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Documents.TableColumnCollection" />.</param>
		// Token: 0x0600369A RID: 13978 RVA: 0x000F5BAD File Offset: 0x000F3DAD
		void IList.Remove(object value)
		{
			((IList)this._columnCollection).Remove(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />. Use the type-safe <see cref="M:System.Windows.Documents.TableColumnCollection.Remove(System.Windows.Documents.TableColumn)" />, <see cref="M:System.Windows.Documents.TableColumnCollection.RemoveAt(System.Int32)" />, or <see cref="M:System.Windows.Documents.TableColumnCollection.RemoveRange(System.Int32,System.Int32)" /> methods instead.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		// Token: 0x0600369B RID: 13979 RVA: 0x000F5BBB File Offset: 0x000F3DBB
		void IList.RemoveAt(int index)
		{
			((IList)this._columnCollection).RemoveAt(index);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />. Use the type-safe <see cref="P:System.Windows.Documents.TableCellCollection.Item(System.Int32)" /> property instead.</summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000DF3 RID: 3571
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._columnCollection)[index];
			}
			set
			{
				((IList)this._columnCollection)[index] = value;
			}
		}

		/// <summary>Gets the number of items currently contained by the collection.</summary>
		/// <returns>The number of items currently contained by the collection.</returns>
		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x0600369E RID: 13982 RVA: 0x000F5BE6 File Offset: 0x000F3DE6
		public int Count
		{
			get
			{
				return this._columnCollection.Count;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x000F5BF3 File Offset: 0x000F3DF3
		public bool IsReadOnly
		{
			get
			{
				return this._columnCollection.IsReadOnly;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000F5C00 File Offset: 0x000F3E00
		public bool IsSynchronized
		{
			get
			{
				return this._columnCollection.IsSynchronized;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x000F5C0D File Offset: 0x000F3E0D
		public object SyncRoot
		{
			get
			{
				return this._columnCollection.SyncRoot;
			}
		}

		/// <summary>Gets or sets the pre-allocated collection item capacity for this collection.</summary>
		/// <returns>The pre-allocated collection item capacity for this collection.  The default value is 8.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when an attempt is made to set <see cref="P:System.Windows.Documents.TableCellCollection.Capacity" /> to a value that is less than the current value of <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x000F5C1A File Offset: 0x000F3E1A
		// (set) Token: 0x060036A3 RID: 13987 RVA: 0x000F5C27 File Offset: 0x000F3E27
		public int Capacity
		{
			get
			{
				return this._columnCollection.PrivateCapacity;
			}
			set
			{
				this._columnCollection.PrivateCapacity = value;
			}
		}

		/// <summary>Gets the collection item at a specified index.  This is an indexed property.</summary>
		/// <param name="index">A zero-based index specifying the position of the collection item to retrieve.</param>
		/// <returns>The collection item at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableColumnCollection.Count" />.</exception>
		// Token: 0x17000DF9 RID: 3577
		public TableColumn this[int index]
		{
			get
			{
				return this._columnCollection[index];
			}
			set
			{
				this._columnCollection[index] = value;
			}
		}

		// Token: 0x0400254A RID: 9546
		private TableColumnCollectionInternal _columnCollection;
	}
}
