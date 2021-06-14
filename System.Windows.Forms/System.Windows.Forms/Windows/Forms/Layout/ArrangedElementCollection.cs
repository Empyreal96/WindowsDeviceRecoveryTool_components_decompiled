using System;
using System.Collections;

namespace System.Windows.Forms.Layout
{
	/// <summary>Represents a collection of objects.</summary>
	// Token: 0x020004D6 RID: 1238
	public class ArrangedElementCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x0600520C RID: 21004 RVA: 0x0015722C File Offset: 0x0015542C
		internal ArrangedElementCollection()
		{
			this._innerList = new ArrayList(4);
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x00157240 File Offset: 0x00155440
		internal ArrangedElementCollection(ArrayList innerList)
		{
			this._innerList = innerList;
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x0015724F File Offset: 0x0015544F
		private ArrangedElementCollection(int size)
		{
			this._innerList = new ArrayList(size);
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x0600520F RID: 21007 RVA: 0x00157263 File Offset: 0x00155463
		internal ArrayList InnerList
		{
			get
			{
				return this._innerList;
			}
		}

		// Token: 0x17001413 RID: 5139
		internal virtual IArrangedElement this[int index]
		{
			get
			{
				return (IArrangedElement)this.InnerList[index];
			}
		}

		/// <summary>Determines whether two <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" /> instances are equal.</summary>
		/// <param name="obj">The <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" /> to compare with the current <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" /> is equal to the current <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005211 RID: 21009 RVA: 0x00157280 File Offset: 0x00155480
		public override bool Equals(object obj)
		{
			ArrangedElementCollection arrangedElementCollection = obj as ArrangedElementCollection;
			if (arrangedElementCollection == null || this.Count != arrangedElementCollection.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.InnerList[i] != arrangedElementCollection.InnerList[i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" />.</returns>
		// Token: 0x06005212 RID: 21010 RVA: 0x001572D5 File Offset: 0x001554D5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x001572E0 File Offset: 0x001554E0
		internal void MoveElement(IArrangedElement element, int fromIndex, int toIndex)
		{
			int num = toIndex - fromIndex;
			if (num == -1 || num == 1)
			{
				this.InnerList[fromIndex] = this.InnerList[toIndex];
			}
			else
			{
				int sourceIndex;
				int destinationIndex;
				if (num > 0)
				{
					sourceIndex = fromIndex + 1;
					destinationIndex = fromIndex;
				}
				else
				{
					sourceIndex = toIndex;
					destinationIndex = toIndex + 1;
					num = -num;
				}
				ArrangedElementCollection.Copy(this, sourceIndex, this, destinationIndex, num);
			}
			this.InnerList[toIndex] = element;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x00157344 File Offset: 0x00155544
		private static void Copy(ArrangedElementCollection sourceList, int sourceIndex, ArrangedElementCollection destinationList, int destinationIndex, int length)
		{
			if (sourceIndex < destinationIndex)
			{
				sourceIndex += length;
				destinationIndex += length;
				while (length > 0)
				{
					destinationList.InnerList[--destinationIndex] = sourceList.InnerList[--sourceIndex];
					length--;
				}
				return;
			}
			while (length > 0)
			{
				destinationList.InnerList[destinationIndex++] = sourceList.InnerList[sourceIndex++];
				length--;
			}
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.Clear" /> method.</summary>
		// Token: 0x06005215 RID: 21013 RVA: 0x001573BE File Offset: 0x001555BE
		void IList.Clear()
		{
			this.InnerList.Clear();
		}

		/// <summary>For a description of this member, see the <see cref="P:System.Collections.IList.IsFixedSize" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06005216 RID: 21014 RVA: 0x00115FFC File Offset: 0x001141FC
		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.Contains(System.Object)" /> method.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005217 RID: 21015 RVA: 0x00115D88 File Offset: 0x00113F88
		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x001573CB File Offset: 0x001555CB
		public virtual bool IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.RemoveAt(System.Int32)" /> method.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		// Token: 0x06005219 RID: 21017 RVA: 0x001573D8 File Offset: 0x001555D8
		void IList.RemoveAt(int index)
		{
			this.InnerList.RemoveAt(index);
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.Remove(System.Object)" /> method.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x0600521A RID: 21018 RVA: 0x001573E6 File Offset: 0x001555E6
		void IList.Remove(object value)
		{
			this.InnerList.Remove(value);
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.Add(System.Object)" /> method.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x0600521B RID: 21019 RVA: 0x001573F4 File Offset: 0x001555F4
		int IList.Add(object value)
		{
			return this.InnerList.Add(value);
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.IndexOf(System.Object)" /> method.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x0600521C RID: 21020 RVA: 0x001160EC File Offset: 0x001142EC
		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x0600521D RID: 21021 RVA: 0x0000A2AB File Offset: 0x000084AB
		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>For a description of this member, see the <see cref="P:System.Collections.IList.Item(System.Int32)" /> property.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17001416 RID: 5142
		object IList.this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets the number of elements in the collection.</summary>
		/// <returns>The number of elements currently contained in the collection.</returns>
		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x00157402 File Offset: 0x00155602
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		/// <summary>For a description of this member, see the <see cref="P:System.Collections.ICollection.SyncRoot" /> property.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" />.</returns>
		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06005221 RID: 21025 RVA: 0x0015740F File Offset: 0x0015560F
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		/// <summary>Copies the entire contents of this collection to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the current collection. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or-The number of elements in the source collection is greater than the available space from <paramref name="index" /> to the end of <paramref name="array" />.</exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source element cannot be cast automatically to the type of <paramref name="array" />.</exception>
		// Token: 0x06005222 RID: 21026 RVA: 0x001162C9 File Offset: 0x001144C9
		public void CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		/// <summary>For a description of this member, see the <see cref="P:System.Collections.ICollection.IsSynchronized" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Windows.Forms.Layout.ArrangedElementCollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x0015741C File Offset: 0x0015561C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		/// <summary>Returns an enumerator for the entire collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the entire collection.</returns>
		// Token: 0x06005224 RID: 21028 RVA: 0x00157429 File Offset: 0x00155629
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x0400347D RID: 13437
		internal static ArrangedElementCollection Empty = new ArrangedElementCollection(0);

		// Token: 0x0400347E RID: 13438
		private ArrayList _innerList;
	}
}
