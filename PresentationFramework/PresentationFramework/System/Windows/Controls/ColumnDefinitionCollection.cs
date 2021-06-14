using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	/// <summary>Provides access to an ordered, strongly typed collection of <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects.</summary>
	// Token: 0x02000570 RID: 1392
	public sealed class ColumnDefinitionCollection : IList<ColumnDefinition>, ICollection<ColumnDefinition>, IEnumerable<ColumnDefinition>, IEnumerable, IList, ICollection
	{
		// Token: 0x06005B92 RID: 23442 RVA: 0x0019C6C4 File Offset: 0x0019A8C4
		internal ColumnDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x06005B93 RID: 23443 RVA: 0x0019C6DC File Offset: 0x0019A8DC
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>Copies an array of <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects to a given index position within a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="array">An array of <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects.</param>
		/// <param name="index">Identifies the index position within <paramref name="array" /> to which the <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects are copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		// Token: 0x06005B94 RID: 23444 RVA: 0x0019C780 File Offset: 0x0019A980
		public void CopyTo(ColumnDefinition[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06005B95 RID: 23445 RVA: 0x0019C805 File Offset: 0x0019AA05
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as ColumnDefinition);
			return this._size - 1;
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.ColumnDefinition" /> element to a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.ColumnDefinition" /> to add to the collection.</param>
		// Token: 0x06005B96 RID: 23446 RVA: 0x0019C82E File Offset: 0x0019AA2E
		public void Add(ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		/// <summary>Clears the content of the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		// Token: 0x06005B97 RID: 23447 RVA: 0x0019C84C File Offset: 0x0019AA4C
		public void Clear()
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateOnModified();
			for (int i = 0; i < this._size; i++)
			{
				this.PrivateDisconnectChild(this._items[i]);
				this._items[i] = null;
			}
			this._size = 0;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005B98 RID: 23448 RVA: 0x0019C894 File Offset: 0x0019AA94
		bool IList.Contains(object value)
		{
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			return columnDefinition != null && columnDefinition.Parent == this._owner;
		}

		/// <summary>Determines whether a given <see cref="T:System.Windows.Controls.ColumnDefinition" /> exists within a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.ColumnDefinition" /> that is being tested.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ColumnDefinition" /> exists within the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x06005B99 RID: 23449 RVA: 0x0019C8BC File Offset: 0x0019AABC
		public bool Contains(ColumnDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06005B9A RID: 23450 RVA: 0x0019C8D2 File Offset: 0x0019AAD2
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ColumnDefinition);
		}

		/// <summary>Returns the index position of a given <see cref="T:System.Windows.Controls.ColumnDefinition" /> within a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.ColumnDefinition" /> whose index position is desired.</param>
		/// <returns>The index of <paramref name="value" /> if found in the collection; otherwise, -1.</returns>
		// Token: 0x06005B9B RID: 23451 RVA: 0x0019C8E0 File Offset: 0x0019AAE0
		public int IndexOf(ColumnDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		// Token: 0x06005B9C RID: 23452 RVA: 0x0019C8FB File Offset: 0x0019AAFB
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as ColumnDefinition);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Controls.ColumnDefinition" /> at the specified index position within a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The position within the collection where the item is inserted.</param>
		/// <param name="value">The <see cref="T:System.Windows.Controls.ColumnDefinition" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
		// Token: 0x06005B9D RID: 23453 RVA: 0x0019C934 File Offset: 0x0019AB34
		public void Insert(int index, ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</param>
		// Token: 0x06005B9E RID: 23454 RVA: 0x0019C968 File Offset: 0x0019AB68
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value as ColumnDefinition);
			}
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.ColumnDefinition" /> from a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.ColumnDefinition" /> to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ColumnDefinition" /> was found in the collection and removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005B9F RID: 23455 RVA: 0x0019C994 File Offset: 0x0019AB94
		public bool Remove(ColumnDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.ColumnDefinition" /> from a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> at the specified index position.</summary>
		/// <param name="index">The position within the collection at which the <see cref="T:System.Windows.Controls.ColumnDefinition" /> is removed.</param>
		// Token: 0x06005BA0 RID: 23456 RVA: 0x0019C9B4 File Offset: 0x0019ABB4
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateRemove(this._items[index]);
		}

		/// <summary>Removes a range of <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects from a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The position within the collection at which the first <see cref="T:System.Windows.Controls.ColumnDefinition" /> is removed.</param>
		/// <param name="count">The total number of <see cref="T:System.Windows.Controls.ColumnDefinition" /> objects to remove from the collection.</param>
		// Token: 0x06005BA1 RID: 23457 RVA: 0x0019C9E8 File Offset: 0x0019ABE8
		public void RemoveRange(int index, int count)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionCountNeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(SR.Get("TableCollectionRangeOutOfRange"));
			}
			this.PrivateOnModified();
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.PrivateDisconnectChild(this._items[i]);
				}
				this._size -= count;
				for (int j = index; j < this._size; j++)
				{
					this._items[j] = this._items[j + count];
					this._items[j].Index = j;
					this._items[j + count] = null;
				}
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06005BA2 RID: 23458 RVA: 0x0019CAB9 File Offset: 0x0019ACB9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x0019CAB9 File Offset: 0x0019ACB9
		IEnumerator<ColumnDefinition> IEnumerable<ColumnDefinition>.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		/// <summary>Gets the total number of items within this instance of <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <returns>The total number of items in the collection. This property has no default value.</returns>
		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x0019CAC6 File Offset: 0x0019ACC6
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06005BA5 RID: 23461 RVA: 0x0019CACE File Offset: 0x0019ACCE
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise <see langword="false" />. This property has no default value.</returns>
		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x06005BA6 RID: 23462 RVA: 0x0019CACE File Offset: 0x0019ACCE
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether access to this <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to this collection is synchronized; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06005BA7 RID: 23463 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</returns>
		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the list.</exception>
		// Token: 0x17001632 RID: 5682
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value as ColumnDefinition);
			}
		}

		/// <summary>Gets a value that indicates the current item within a <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The current item in the collection.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the collection.</exception>
		// Token: 0x17001633 RID: 5683
		public ColumnDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return (ColumnDefinition)this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value);
			}
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x0019CBE1 File Offset: 0x0019ADE1
		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x0019CAC6 File Offset: 0x0019ACC6
		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x06005BAF RID: 23471 RVA: 0x0019CBEF File Offset: 0x0019ADEF
		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x0019CBF7 File Offset: 0x0019ADF7
		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("GridCollection_CannotModifyReadOnly", new object[]
				{
					"ColumnDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x0019CC20 File Offset: 0x0019AE20
		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"ColumnDefinitionCollection",
					"ColumnDefinition"
				}));
			}
			if (columnDefinition.Parent != null)
			{
				throw new ArgumentException(SR.Get("GridCollection_InOtherCollection", new object[]
				{
					"value",
					"ColumnDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x0019CC9C File Offset: 0x0019AE9C
		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"ColumnDefinitionCollection",
					"ColumnDefinition"
				}));
			}
			return columnDefinition.Parent == this._owner;
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x0019CCF5 File Offset: 0x0019AEF5
		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			this._owner.AddLogicalChild(value);
			value.OnEnterParentTree();
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x0019CD19 File Offset: 0x0019AF19
		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			this._owner.RemoveLogicalChild(value);
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x0019CD44 File Offset: 0x0019AF44
		private void PrivateInsert(int index, DefinitionBase value)
		{
			this.PrivateOnModified();
			if (this._items == null)
			{
				this.PrivateSetCapacity(4);
			}
			else if (this._size == this._items.Length)
			{
				this.PrivateSetCapacity(Math.Max(this._items.Length * 2, 4));
			}
			for (int i = this._size - 1; i >= index; i--)
			{
				this._items[i + 1] = this._items[i];
				this._items[i].Index = i + 1;
			}
			this._items[index] = null;
			this._size++;
			this.PrivateConnectChild(index, value);
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x0019CDE4 File Offset: 0x0019AFE4
		private void PrivateRemove(DefinitionBase value)
		{
			this.PrivateOnModified();
			int index = value.Index;
			this.PrivateDisconnectChild(value);
			this._size--;
			for (int i = index; i < this._size; i++)
			{
				this._items[i] = this._items[i + 1];
				this._items[i].Index = i;
			}
			this._items[this._size] = null;
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x0019CE52 File Offset: 0x0019B052
		private void PrivateOnModified()
		{
			this._version++;
			this._owner.ColumnDefinitionCollectionDirty = true;
			this._owner.Invalidate();
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x0019CE7C File Offset: 0x0019B07C
		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				ColumnDefinition[] array = new ColumnDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				this._items = array;
			}
		}

		// Token: 0x04002F8D RID: 12173
		private readonly Grid _owner;

		// Token: 0x04002F8E RID: 12174
		private DefinitionBase[] _items;

		// Token: 0x04002F8F RID: 12175
		private int _size;

		// Token: 0x04002F90 RID: 12176
		private int _version;

		// Token: 0x04002F91 RID: 12177
		private const int c_defaultCapacity = 4;

		// Token: 0x020009E1 RID: 2529
		internal struct Enumerator : IEnumerator<ColumnDefinition>, IDisposable, IEnumerator
		{
			// Token: 0x06008951 RID: 35153 RVA: 0x00254ABF File Offset: 0x00252CBF
			internal Enumerator(ColumnDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			// Token: 0x06008952 RID: 35154 RVA: 0x00254AF4 File Offset: 0x00252CF4
			public bool MoveNext()
			{
				if (this._collection == null)
				{
					return false;
				}
				this.PrivateValidate();
				if (this._index < this._collection._size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection._size;
				return false;
			}

			// Token: 0x17001F0C RID: 7948
			// (get) Token: 0x06008953 RID: 35155 RVA: 0x00254B6B File Offset: 0x00252D6B
			object IEnumerator.Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001F0D RID: 7949
			// (get) Token: 0x06008954 RID: 35156 RVA: 0x00254BAC File Offset: 0x00252DAC
			public ColumnDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (ColumnDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x06008955 RID: 35157 RVA: 0x00254BFB File Offset: 0x00252DFB
			public void Reset()
			{
				if (this._collection == null)
				{
					return;
				}
				this.PrivateValidate();
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x06008956 RID: 35158 RVA: 0x00254C1F File Offset: 0x00252E1F
			public void Dispose()
			{
				this._currentElement = null;
			}

			// Token: 0x06008957 RID: 35159 RVA: 0x00254C28 File Offset: 0x00252E28
			private void PrivateValidate()
			{
				if (this._currentElement == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorCollectionDisposed"));
				}
				if (this._version != this._collection._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
			}

			// Token: 0x04004648 RID: 17992
			private ColumnDefinitionCollection _collection;

			// Token: 0x04004649 RID: 17993
			private int _index;

			// Token: 0x0400464A RID: 17994
			private int _version;

			// Token: 0x0400464B RID: 17995
			private object _currentElement;
		}
	}
}
