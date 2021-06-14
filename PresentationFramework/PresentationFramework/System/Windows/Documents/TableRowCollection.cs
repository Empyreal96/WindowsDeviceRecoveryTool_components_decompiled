using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>Provides standard facilities for creating and managing a type-safe, ordered collection of <see cref="T:System.Windows.Documents.TableRow" /> objects.</summary>
	// Token: 0x020003E8 RID: 1000
	public sealed class TableRowCollection : IList<TableRow>, ICollection<TableRow>, IEnumerable<TableRow>, IEnumerable, IList, ICollection
	{
		// Token: 0x060036C6 RID: 14022 RVA: 0x000F5F62 File Offset: 0x000F4162
		internal TableRowCollection(TableRowGroup owner)
		{
			this._rowCollectionInternal = new TableTextElementCollectionInternal<TableRowGroup, TableRow>(owner);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified array starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableRow" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x060036C7 RID: 14023 RVA: 0x000F5F76 File Offset: 0x000F4176
		public void CopyTo(Array array, int index)
		{
			this._rowCollectionInternal.CopyTo(array, index);
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified <see cref="T:System.Windows.Documents.TableRow" /> array of starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional <see cref="T:System.Windows.Documents.TableRow" /> array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="index">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TableRow" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x060036C8 RID: 14024 RVA: 0x000F5F85 File Offset: 0x000F4185
		public void CopyTo(TableRow[] array, int index)
		{
			this._rowCollectionInternal.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x060036C9 RID: 14025 RVA: 0x000F5F94 File Offset: 0x000F4194
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._rowCollectionInternal.GetEnumerator();
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x000F5FA1 File Offset: 0x000F41A1
		IEnumerator<TableRow> IEnumerable<TableRow>.GetEnumerator()
		{
			return ((IEnumerable<TableRow>)this._rowCollectionInternal).GetEnumerator();
		}

		/// <summary>Appends a specified item to the collection.</summary>
		/// <param name="item">A table row to append to the collection or rows.</param>
		/// <exception cref="T:System.ArgumentException">Raised when item already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x060036CB RID: 14027 RVA: 0x000F5FAE File Offset: 0x000F41AE
		public void Add(TableRow item)
		{
			this._rowCollectionInternal.Add(item);
		}

		/// <summary>Clears all items from the collection.</summary>
		// Token: 0x060036CC RID: 14028 RVA: 0x000F5FBC File Offset: 0x000F41BC
		public void Clear()
		{
			this._rowCollectionInternal.Clear();
		}

		/// <summary>Queries for the presence of a specified item in the collection.</summary>
		/// <param name="item">An item to query for the presence of in the collection.</param>
		/// <returns>
		///     true if the specified item is present in the collection; otherwise, false.</returns>
		// Token: 0x060036CD RID: 14029 RVA: 0x000F5FC9 File Offset: 0x000F41C9
		public bool Contains(TableRow item)
		{
			return this._rowCollectionInternal.Contains(item);
		}

		/// <summary>Returns the zero-based index of specified collection item.</summary>
		/// <param name="item">A collection item to return the index of.</param>
		/// <returns>The zero-based index of the specified collection item, or -1 if the specified item is not a member of the collection.</returns>
		// Token: 0x060036CE RID: 14030 RVA: 0x000F5FD7 File Offset: 0x000F41D7
		public int IndexOf(TableRow item)
		{
			return this._rowCollectionInternal.IndexOf(item);
		}

		/// <summary>Inserts a specified item in the collection at a specified index position.</summary>
		/// <param name="index">A zero-based index that specifies the position in the collection at which to insert <paramref name="item" />.</param>
		/// <param name="item">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x060036CF RID: 14031 RVA: 0x000F5FE5 File Offset: 0x000F41E5
		public void Insert(int index, TableRow item)
		{
			this._rowCollectionInternal.Insert(index, item);
		}

		/// <summary>Removes a specified item from the collection.</summary>
		/// <param name="item">An item to remove from the collection.</param>
		/// <returns>
		///     true if the specified item was found and removed; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if item is not present in the collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x060036D0 RID: 14032 RVA: 0x000F5FF4 File Offset: 0x000F41F4
		public bool Remove(TableRow item)
		{
			return this._rowCollectionInternal.Remove(item);
		}

		/// <summary>Removes an item, specified by index, from the collection.</summary>
		/// <param name="index">A zero-based index that specifies the collection item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowCollection.Count" />.</exception>
		// Token: 0x060036D1 RID: 14033 RVA: 0x000F6002 File Offset: 0x000F4202
		public void RemoveAt(int index)
		{
			this._rowCollectionInternal.RemoveAt(index);
		}

		/// <summary>Removes a range of items, specified by beginning index and count, from the collection.</summary>
		/// <param name="index">A zero-based index indicating the beginning of a range of items to remove.</param>
		/// <param name="count">The number of items to remove, beginning form the position specified by <paramref name="index" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index or count is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowCollection.Count" />.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when index and count do not specify a valid range in this collection.</exception>
		// Token: 0x060036D2 RID: 14034 RVA: 0x000F6010 File Offset: 0x000F4210
		public void RemoveRange(int index, int count)
		{
			this._rowCollectionInternal.RemoveRange(index, count);
		}

		/// <summary>Optimizes memory consumption for the collection by setting the underlying collection <see cref="P:System.Windows.Documents.TableRowCollection.Capacity" /> equal to the <see cref="P:System.Windows.Documents.TableRowCollection.Count" /> of items currently in the collection.</summary>
		// Token: 0x060036D3 RID: 14035 RVA: 0x000F601F File Offset: 0x000F421F
		public void TrimToSize()
		{
			this._rowCollectionInternal.TrimToSize();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Documents.TableRowCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x060036D4 RID: 14036 RVA: 0x000F602C File Offset: 0x000F422C
		int IList.Add(object value)
		{
			return ((IList)this._rowCollectionInternal).Add(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Clear" />.</summary>
		// Token: 0x060036D5 RID: 14037 RVA: 0x000F603A File Offset: 0x000F423A
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableRowCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Documents.TableRowCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060036D6 RID: 14038 RVA: 0x000F6042 File Offset: 0x000F4242
		bool IList.Contains(object value)
		{
			return ((IList)this._rowCollectionInternal).Contains(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Documents.TableRowCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x060036D7 RID: 14039 RVA: 0x000F6050 File Offset: 0x000F4250
		int IList.IndexOf(object value)
		{
			return ((IList)this._rowCollectionInternal).IndexOf(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Documents.TableRowCollection" />.</param>
		// Token: 0x060036D8 RID: 14040 RVA: 0x000F605E File Offset: 0x000F425E
		void IList.Insert(int index, object value)
		{
			((IList)this._rowCollectionInternal).Insert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableRowCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x060036D9 RID: 14041 RVA: 0x000F606D File Offset: 0x000F426D
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsFixedSize;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Documents.TableRowCollection" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x060036DA RID: 14042 RVA: 0x000F607A File Offset: 0x000F427A
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsReadOnly;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Documents.TableRowCollection" />.</param>
		// Token: 0x060036DB RID: 14043 RVA: 0x000F6087 File Offset: 0x000F4287
		void IList.Remove(object value)
		{
			((IList)this._rowCollectionInternal).Remove(value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the item to remove. </param>
		// Token: 0x060036DC RID: 14044 RVA: 0x000F6095 File Offset: 0x000F4295
		void IList.RemoveAt(int index)
		{
			((IList)this._rowCollectionInternal).RemoveAt(index);
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000E09 RID: 3593
		object IList.this[int index]
		{
			get
			{
				return ((IList)this._rowCollectionInternal)[index];
			}
			set
			{
				((IList)this._rowCollectionInternal)[index] = value;
			}
		}

		/// <summary>Gets the number of items currently contained by the collection.</summary>
		/// <returns>The number of items currently contained by the collection.</returns>
		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x060036DF RID: 14047 RVA: 0x000F60C0 File Offset: 0x000F42C0
		public int Count
		{
			get
			{
				return this._rowCollectionInternal.Count;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x060036E0 RID: 14048 RVA: 0x000F607A File Offset: 0x000F427A
		public bool IsReadOnly
		{
			get
			{
				return ((IList)this._rowCollectionInternal).IsReadOnly;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Currently, this property always returns false.</returns>
		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x060036E1 RID: 14049 RVA: 0x000F60CD File Offset: 0x000F42CD
		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)this._rowCollectionInternal).IsSynchronized;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x060036E2 RID: 14050 RVA: 0x000F60DA File Offset: 0x000F42DA
		public object SyncRoot
		{
			get
			{
				return ((ICollection)this._rowCollectionInternal).SyncRoot;
			}
		}

		/// <summary>Gets or sets the pre-allocated collection item capacity for this collection.</summary>
		/// <returns>The pre-allocated collection item capacity for this collection.  The default value is 8.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when an attempt is made to set <see cref="P:System.Windows.Documents.TableCellCollection.Capacity" /> to a value that is less than the current value of <see cref="P:System.Windows.Documents.TableCellCollection.Count" />.</exception>
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x060036E3 RID: 14051 RVA: 0x000F60E7 File Offset: 0x000F42E7
		// (set) Token: 0x060036E4 RID: 14052 RVA: 0x000F60F4 File Offset: 0x000F42F4
		public int Capacity
		{
			get
			{
				return this._rowCollectionInternal.Capacity;
			}
			set
			{
				this._rowCollectionInternal.Capacity = value;
			}
		}

		/// <summary>Gets the collection item at a specified index.  This is an indexed property.</summary>
		/// <param name="index">A zero-based index specifying the position of the collection item to retrieve.</param>
		/// <returns>The collection item at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when index is less than zero, or when index is greater than or equal to <see cref="P:System.Windows.Documents.TableRowCollection.Count" />.</exception>
		// Token: 0x17000E0F RID: 3599
		public TableRow this[int index]
		{
			get
			{
				return this._rowCollectionInternal[index];
			}
			set
			{
				this._rowCollectionInternal[index] = value;
			}
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000F611F File Offset: 0x000F431F
		internal void InternalAdd(TableRow item)
		{
			this._rowCollectionInternal.InternalAdd(item);
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x000F612D File Offset: 0x000F432D
		internal void InternalRemove(TableRow item)
		{
			this._rowCollectionInternal.InternalRemove(item);
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000F613B File Offset: 0x000F433B
		private void EnsureCapacity(int min)
		{
			this._rowCollectionInternal.EnsureCapacity(min);
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000F6149 File Offset: 0x000F4349
		private void PrivateConnectChild(int index, TableRow item)
		{
			this._rowCollectionInternal.PrivateConnectChild(index, item);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000F6158 File Offset: 0x000F4358
		private void PrivateDisconnectChild(TableRow item)
		{
			this._rowCollectionInternal.PrivateDisconnectChild(item);
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000F6166 File Offset: 0x000F4366
		private bool BelongsToOwner(TableRow item)
		{
			return this._rowCollectionInternal.BelongsToOwner(item);
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000F6174 File Offset: 0x000F4374
		private int FindInsertionIndex(TableRow item)
		{
			return this._rowCollectionInternal.FindInsertionIndex(item);
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x060036EE RID: 14062 RVA: 0x000F6182 File Offset: 0x000F4382
		// (set) Token: 0x060036EF RID: 14063 RVA: 0x000F618F File Offset: 0x000F438F
		private int PrivateCapacity
		{
			get
			{
				return this._rowCollectionInternal.PrivateCapacity;
			}
			set
			{
				this._rowCollectionInternal.PrivateCapacity = value;
			}
		}

		// Token: 0x04002552 RID: 9554
		private TableTextElementCollectionInternal<TableRowGroup, TableRow> _rowCollectionInternal;
	}
}
