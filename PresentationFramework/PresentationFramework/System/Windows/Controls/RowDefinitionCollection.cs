using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	/// <summary>Provides access to an ordered, strongly typed collection of <see cref="T:System.Windows.Controls.RowDefinition" /> objects.</summary>
	// Token: 0x02000572 RID: 1394
	public sealed class RowDefinitionCollection : IList<RowDefinition>, ICollection<RowDefinition>, IEnumerable<RowDefinition>, IEnumerable, IList, ICollection
	{
		// Token: 0x06005BC3 RID: 23491 RVA: 0x0019D07D File Offset: 0x0019B27D
		internal RowDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x06005BC4 RID: 23492 RVA: 0x0019D094 File Offset: 0x0019B294
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

		/// <summary>Copies an array of <see cref="T:System.Windows.Controls.RowDefinition" /> objects to a given index position within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="array">An array of <see cref="T:System.Windows.Controls.RowDefinition" /> objects.</param>
		/// <param name="index">Identifies the index position within <paramref name="array" /> to which the <see cref="T:System.Windows.Controls.RowDefinition" /> objects are copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from index to the end of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		// Token: 0x06005BC5 RID: 23493 RVA: 0x0019D138 File Offset: 0x0019B338
		public void CopyTo(RowDefinition[] array, int index)
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

		/// <summary>Adds an item to the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06005BC6 RID: 23494 RVA: 0x0019D1BD File Offset: 0x0019B3BD
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as RowDefinition);
			return this._size - 1;
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.RowDefinition" /> element to a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.RowDefinition" /> to add to the collection.</param>
		// Token: 0x06005BC7 RID: 23495 RVA: 0x0019D1E6 File Offset: 0x0019B3E6
		public void Add(RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		/// <summary>Clears the content of the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		// Token: 0x06005BC8 RID: 23496 RVA: 0x0019D204 File Offset: 0x0019B404
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

		/// <summary>Determines whether the collection contains a specific value.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005BC9 RID: 23497 RVA: 0x0019D24C File Offset: 0x0019B44C
		bool IList.Contains(object value)
		{
			RowDefinition rowDefinition = value as RowDefinition;
			return rowDefinition != null && rowDefinition.Parent == this._owner;
		}

		/// <summary>Determines whether a given <see cref="T:System.Windows.Controls.RowDefinition" /> exists within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.RowDefinition" /> that is being tested.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.RowDefinition" /> exists within the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x06005BCA RID: 23498 RVA: 0x0019D274 File Offset: 0x0019B474
		public bool Contains(RowDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		/// <summary>Determines the index of a specific item in the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06005BCB RID: 23499 RVA: 0x0019D28A File Offset: 0x0019B48A
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as RowDefinition);
		}

		/// <summary>Returns the index position of a given <see cref="T:System.Windows.Controls.RowDefinition" /> within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> whose index position is desired.</param>
		/// <returns>The index of <paramref name="value" /> if found in the collection; otherwise, -1.</returns>
		// Token: 0x06005BCC RID: 23500 RVA: 0x0019D298 File Offset: 0x0019B498
		public int IndexOf(RowDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		/// <summary>Inserts an item to the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		// Token: 0x06005BCD RID: 23501 RVA: 0x0019D2B3 File Offset: 0x0019B4B3
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as RowDefinition);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Controls.RowDefinition" /> at the specified index position within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The position within the collection where the item is inserted.</param>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
		// Token: 0x06005BCE RID: 23502 RVA: 0x0019D2EC File Offset: 0x0019B4EC
		public void Insert(int index, RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		/// <summary>Removes the first occurrence of a specific object from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		// Token: 0x06005BCF RID: 23503 RVA: 0x0019D320 File Offset: 0x0019B520
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value as RowDefinition);
			}
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.RowDefinition" /> from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.RowDefinition" /> was found in the collection and removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005BD0 RID: 23504 RVA: 0x0019D34C File Offset: 0x0019B54C
		public bool Remove(RowDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.RowDefinition" /> from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> at the specified index position.</summary>
		/// <param name="index">The position within the collection at which the <see cref="T:System.Windows.Controls.RowDefinition" /> is removed.</param>
		// Token: 0x06005BD1 RID: 23505 RVA: 0x0019D36C File Offset: 0x0019B56C
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateRemove(this._items[index]);
		}

		/// <summary>Removes a range of <see cref="T:System.Windows.Controls.RowDefinition" /> objects from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The position within the collection at which the first <see cref="T:System.Windows.Controls.RowDefinition" /> is removed.</param>
		/// <param name="count">The total number of <see cref="T:System.Windows.Controls.RowDefinition" /> objects to remove from the collection.</param>
		// Token: 0x06005BD2 RID: 23506 RVA: 0x0019D3A0 File Offset: 0x0019B5A0
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

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06005BD3 RID: 23507 RVA: 0x0019D471 File Offset: 0x0019B671
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		// Token: 0x06005BD4 RID: 23508 RVA: 0x0019D471 File Offset: 0x0019B671
		IEnumerator<RowDefinition> IEnumerable<RowDefinition>.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		/// <summary>Gets the total number of items within this instance of <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <returns>The total number of items in the collection. This property has no default value.</returns>
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06005BD5 RID: 23509 RVA: 0x0019D47E File Offset: 0x0019B67E
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06005BD6 RID: 23510 RVA: 0x0019D486 File Offset: 0x0019B686
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise <see langword="false" />. This property has no default value.</returns>
		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x0019D486 File Offset: 0x0019B686
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether access to this <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> is synchronized (thread-safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to this collection is synchronized; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</returns>
		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the list.</exception>
		// Token: 0x17001640 RID: 5696
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
				this.PrivateConnectChild(index, value as RowDefinition);
			}
		}

		/// <summary>Gets a value that indicates the current item within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The current item in the collection.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the collection.</exception>
		// Token: 0x17001641 RID: 5697
		public RowDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return (RowDefinition)this._items[index];
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

		// Token: 0x06005BDE RID: 23518 RVA: 0x0019D599 File Offset: 0x0019B799
		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06005BDF RID: 23519 RVA: 0x0019D47E File Offset: 0x0019B67E
		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x0019D5A7 File Offset: 0x0019B7A7
		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x0019D5AF File Offset: 0x0019B7AF
		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("GridCollection_CannotModifyReadOnly", new object[]
				{
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BE2 RID: 23522 RVA: 0x0019D5D8 File Offset: 0x0019B7D8
		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			if (rowDefinition.Parent != null)
			{
				throw new ArgumentException(SR.Get("GridCollection_InOtherCollection", new object[]
				{
					"value",
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x0019D654 File Offset: 0x0019B854
		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			return rowDefinition.Parent == this._owner;
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x0019D6AD File Offset: 0x0019B8AD
		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			this._owner.AddLogicalChild(value);
			value.OnEnterParentTree();
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x0019D6D1 File Offset: 0x0019B8D1
		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			this._owner.RemoveLogicalChild(value);
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x0019D6FC File Offset: 0x0019B8FC
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

		// Token: 0x06005BE7 RID: 23527 RVA: 0x0019D79C File Offset: 0x0019B99C
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

		// Token: 0x06005BE8 RID: 23528 RVA: 0x0019D80A File Offset: 0x0019BA0A
		private void PrivateOnModified()
		{
			this._version++;
			this._owner.RowDefinitionCollectionDirty = true;
			this._owner.Invalidate();
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x0019D834 File Offset: 0x0019BA34
		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				RowDefinition[] array = new RowDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				this._items = array;
			}
		}

		// Token: 0x04002F95 RID: 12181
		private readonly Grid _owner;

		// Token: 0x04002F96 RID: 12182
		private DefinitionBase[] _items;

		// Token: 0x04002F97 RID: 12183
		private int _size;

		// Token: 0x04002F98 RID: 12184
		private int _version;

		// Token: 0x04002F99 RID: 12185
		private const int c_defaultCapacity = 4;

		// Token: 0x020009E2 RID: 2530
		internal struct Enumerator : IEnumerator<RowDefinition>, IDisposable, IEnumerator
		{
			// Token: 0x06008958 RID: 35160 RVA: 0x00254C65 File Offset: 0x00252E65
			internal Enumerator(RowDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			// Token: 0x06008959 RID: 35161 RVA: 0x00254C98 File Offset: 0x00252E98
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

			// Token: 0x17001F0E RID: 7950
			// (get) Token: 0x0600895A RID: 35162 RVA: 0x00254D0F File Offset: 0x00252F0F
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

			// Token: 0x17001F0F RID: 7951
			// (get) Token: 0x0600895B RID: 35163 RVA: 0x00254D50 File Offset: 0x00252F50
			public RowDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (RowDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x0600895C RID: 35164 RVA: 0x00254D9F File Offset: 0x00252F9F
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

			// Token: 0x0600895D RID: 35165 RVA: 0x00254DC3 File Offset: 0x00252FC3
			public void Dispose()
			{
				this._currentElement = null;
			}

			// Token: 0x0600895E RID: 35166 RVA: 0x00254DCC File Offset: 0x00252FCC
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

			// Token: 0x0400464C RID: 17996
			private RowDefinitionCollection _collection;

			// Token: 0x0400464D RID: 17997
			private int _index;

			// Token: 0x0400464E RID: 17998
			private int _version;

			// Token: 0x0400464F RID: 17999
			private object _currentElement;
		}
	}
}
