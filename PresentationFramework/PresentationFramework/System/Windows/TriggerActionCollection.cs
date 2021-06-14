using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.TriggerAction" /> objects.</summary>
	// Token: 0x02000133 RID: 307
	public sealed class TriggerActionCollection : IList, ICollection, IEnumerable, IList<TriggerAction>, ICollection<TriggerAction>, IEnumerable<TriggerAction>
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.TriggerActionCollection" /> class.</summary>
		// Token: 0x06000C92 RID: 3218 RVA: 0x0002F396 File Offset: 0x0002D596
		public TriggerActionCollection()
		{
			this._rawList = new List<TriggerAction>();
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.TriggerActionCollection" /> class that has the specified initial size.</summary>
		/// <param name="initialSize">The size of the collection.</param>
		// Token: 0x06000C93 RID: 3219 RVA: 0x0002F3A9 File Offset: 0x0002D5A9
		public TriggerActionCollection(int initialSize)
		{
			this._rawList = new List<TriggerAction>(initialSize);
		}

		/// <summary>Gets the number of items in the collection.</summary>
		/// <returns>The number of items that the collection contains.</returns>
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0002F3BD File Offset: 0x0002D5BD
		public int Count
		{
			get
			{
				return this._rawList.Count;
			}
		}

		/// <summary>Gets a value that indicates whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002F3CA File Offset: 0x0002D5CA
		public bool IsReadOnly
		{
			get
			{
				return this._sealed;
			}
		}

		/// <summary>Removes all items from the collection.</summary>
		// Token: 0x06000C96 RID: 3222 RVA: 0x0002F3D4 File Offset: 0x0002D5D4
		public void Clear()
		{
			this.CheckSealed();
			for (int i = this._rawList.Count - 1; i >= 0; i--)
			{
				InheritanceContextHelper.RemoveContextFromObject(this._owner, this._rawList[i]);
			}
			this._rawList.Clear();
		}

		/// <summary>Removes from the collection the item that is located at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		// Token: 0x06000C97 RID: 3223 RVA: 0x0002F424 File Offset: 0x0002D624
		public void RemoveAt(int index)
		{
			this.CheckSealed();
			TriggerAction oldValue = this._rawList[index];
			InheritanceContextHelper.RemoveContextFromObject(this._owner, oldValue);
			this._rawList.RemoveAt(index);
		}

		/// <summary>Adds an item to the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.TriggerAction" /> object to add.</param>
		// Token: 0x06000C98 RID: 3224 RVA: 0x0002F45C File Offset: 0x0002D65C
		public void Add(TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value);
			this._rawList.Add(value);
		}

		/// <summary>Returns a value that indicates whether the collection contains the specified <see cref="T:System.Windows.TriggerAction" /> object.</summary>
		/// <param name="value">The <see cref="T:System.Windows.TriggerAction" /> object to locate in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.TriggerAction" /> object is found in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C99 RID: 3225 RVA: 0x0002F47C File Offset: 0x0002D67C
		public bool Contains(TriggerAction value)
		{
			return this._rawList.Contains(value);
		}

		/// <summary>Begins at the specified index and copies the collection items to the specified array.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the items that are copied from the collection. The array must use zero-based indexing.</param>
		/// <param name="index">The zero-based index in the <paramref name="array" /> where copying starts.</param>
		// Token: 0x06000C9A RID: 3226 RVA: 0x0002F48A File Offset: 0x0002D68A
		public void CopyTo(TriggerAction[] array, int index)
		{
			this._rawList.CopyTo(array, index);
		}

		/// <summary>Returns the index of the specified item in the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.TriggerAction" /> object to locate in the collection.</param>
		/// <returns>The index of <paramref name="value" /> if the <see cref="T:System.Windows.TriggerAction" /> object is found in the collection; otherwise, -1.</returns>
		// Token: 0x06000C9B RID: 3227 RVA: 0x0002F499 File Offset: 0x0002D699
		public int IndexOf(TriggerAction value)
		{
			return this._rawList.IndexOf(value);
		}

		/// <summary>Inserts the specified item into the collection at the specified index.</summary>
		/// <param name="index">The zero-based index where the <paramref name="value" /> must be inserted.</param>
		/// <param name="value">The <see cref="T:System.Windows.TriggerAction" /> object to insert into the collection.</param>
		// Token: 0x06000C9C RID: 3228 RVA: 0x0002F4A7 File Offset: 0x0002D6A7
		public void Insert(int index, TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value);
			this._rawList.Insert(index, value);
		}

		/// <summary>Removes the first occurrence of the specified object from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.TriggerAction" /> object to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if item is successfully removed; otherwise, <see langword="false" />. This method also returns false if item was not found in the <see cref="T:System.Windows.TriggerActionCollection" />.</returns>
		// Token: 0x06000C9D RID: 3229 RVA: 0x0002F4C8 File Offset: 0x0002D6C8
		public bool Remove(TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.RemoveContextFromObject(this._owner, value);
			return this._rawList.Remove(value);
		}

		/// <summary>Gets or sets the item that is at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.TriggerAction" /> object that is at the specified index.</returns>
		// Token: 0x17000402 RID: 1026
		public TriggerAction this[int index]
		{
			get
			{
				return this._rawList[index];
			}
			set
			{
				this.CheckSealed();
				object obj = this._rawList[index];
				InheritanceContextHelper.RemoveContextFromObject(this.Owner, obj as DependencyObject);
				this._rawList[index] = value;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002F542 File Offset: 0x0002D742
		[CLSCompliant(false)]
		public IEnumerator<TriggerAction> GetEnumerator()
		{
			return this._rawList.GetEnumerator();
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002F554 File Offset: 0x0002D754
		int IList.Add(object value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value as DependencyObject);
			return ((IList)this._rawList).Add(this.VerifyIsTriggerAction(value));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, false.</returns>
		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002F58C File Offset: 0x0002D78C
		bool IList.Contains(object value)
		{
			return this._rawList.Contains(this.VerifyIsTriggerAction(value));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002F5A0 File Offset: 0x0002D7A0
		int IList.IndexOf(object value)
		{
			return this._rawList.IndexOf(this.VerifyIsTriggerAction(value));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002F5B4 File Offset: 0x0002D7B4
		void IList.Insert(int index, object value)
		{
			this.Insert(index, this.VerifyIsTriggerAction(value));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0002F3CA File Offset: 0x0002D5CA
		bool IList.IsFixedSize
		{
			get
			{
				return this._sealed;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002F5C4 File Offset: 0x0002D7C4
		void IList.Remove(object value)
		{
			this.Remove(this.VerifyIsTriggerAction(value));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <returns>The object that is at the specified index.</returns>
		// Token: 0x17000404 RID: 1028
		object IList.this[int index]
		{
			get
			{
				return this._rawList[index];
			}
			set
			{
				this[index] = this.VerifyIsTriggerAction(value);
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the items that are copied from the collection. The array must use zero-based indexing.</param>
		/// <param name="index">The zero-based index in the <paramref name="array" /> where copying starts.</param>
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002F5E4 File Offset: 0x0002D7E4
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._rawList).CopyTo(array, index);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0001B7E3 File Offset: 0x000199E3
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06000CAC RID: 3244 RVA: 0x0002F5F3 File Offset: 0x0002D7F3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this._rawList).GetEnumerator();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0002F600 File Offset: 0x0002D800
		internal void Seal(TriggerBase containingTrigger)
		{
			for (int i = 0; i < this._rawList.Count; i++)
			{
				this._rawList[i].Seal(containingTrigger);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0002F635 File Offset: 0x0002D835
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x0002F63D File Offset: 0x0002D83D
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0002F646 File Offset: 0x0002D846
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerActionCollection"
				}));
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002F670 File Offset: 0x0002D870
		private TriggerAction VerifyIsTriggerAction(object value)
		{
			TriggerAction triggerAction = value as TriggerAction;
			if (triggerAction != null)
			{
				return triggerAction;
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			throw new ArgumentException(SR.Get("MustBeTriggerAction"));
		}

		// Token: 0x04000B16 RID: 2838
		private List<TriggerAction> _rawList;

		// Token: 0x04000B17 RID: 2839
		private bool _sealed;

		// Token: 0x04000B18 RID: 2840
		private DependencyObject _owner;
	}
}
