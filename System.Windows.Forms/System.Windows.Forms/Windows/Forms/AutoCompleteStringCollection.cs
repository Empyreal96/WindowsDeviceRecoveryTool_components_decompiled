using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Contains a collection of strings to use for the auto-complete feature on certain Windows Forms controls. </summary>
	// Token: 0x02000119 RID: 281
	public class AutoCompleteStringCollection : IList, ICollection, IEnumerable
	{
		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The index at which to get or set the <see cref="T:System.String" />.</param>
		/// <returns>The <see cref="T:System.String" /> at the specified position.</returns>
		// Token: 0x17000215 RID: 533
		public string this[int index]
		{
			get
			{
				return (string)this.data[index];
			}
			set
			{
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, this.data[index]));
				this.data[index] = value;
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
			}
		}

		/// <summary>Gets the number of items in the <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> .</summary>
		/// <returns>The number of items in the <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" />.</returns>
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00011C03 File Offset: 0x0000FE03
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only. For a description of this member, see <see cref="P:System.Collections.IList.IsReadOnly" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size. For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Occurs when the collection changes.</summary>
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000632 RID: 1586 RVA: 0x00011C10 File Offset: 0x0000FE10
		// (remove) Token: 0x06000633 RID: 1587 RVA: 0x00011C29 File Offset: 0x0000FE29
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.AutoCompleteStringCollection.CollectionChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data.</param>
		// Token: 0x06000634 RID: 1588 RVA: 0x00011C42 File Offset: 0x0000FE42
		protected void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		/// <summary>Inserts a new <see cref="T:System.String" /> into the collection.</summary>
		/// <param name="value">The <see cref="T:System.String" /> to add to the collection.</param>
		/// <returns>The position in the collection where the <see cref="T:System.String" /> was added.</returns>
		// Token: 0x06000635 RID: 1589 RVA: 0x00011C5C File Offset: 0x0000FE5C
		public int Add(string value)
		{
			int result = this.data.Add(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
			return result;
		}

		/// <summary>Adds the elements of a <see cref="T:System.String" /> collection to the end. </summary>
		/// <param name="value">The strings to add to the collection.</param>
		// Token: 0x06000636 RID: 1590 RVA: 0x00011C84 File Offset: 0x0000FE84
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.data.AddRange(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Removes all strings from the collection.</summary>
		// Token: 0x06000637 RID: 1591 RVA: 0x00011CAD File Offset: 0x0000FEAD
		public void Clear()
		{
			this.data.Clear();
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		/// <summary>Indicates whether the <see cref="T:System.String" /> exists within the collection.</summary>
		/// <param name="value">The <see cref="T:System.String" /> for which to search.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.String" /> exists within the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000638 RID: 1592 RVA: 0x00011CC7 File Offset: 0x0000FEC7
		public bool Contains(string value)
		{
			return this.data.Contains(value);
		}

		/// <summary>Copies an array of <see cref="T:System.String" /> objects into the collection, starting at the specified position.</summary>
		/// <param name="array">The <see cref="T:System.String" /> objects to add to the collection.</param>
		/// <param name="index">The position within the collection at which to start the insertion. </param>
		// Token: 0x06000639 RID: 1593 RVA: 0x00011CD5 File Offset: 0x0000FED5
		public void CopyTo(string[] array, int index)
		{
			this.data.CopyTo(array, index);
		}

		/// <summary>Obtains the position of the specified string within the collection.</summary>
		/// <param name="value">The <see cref="T:System.String" /> for which to search.</param>
		/// <returns>The index for the specified item.</returns>
		// Token: 0x0600063A RID: 1594 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		public int IndexOf(string value)
		{
			return this.data.IndexOf(value);
		}

		/// <summary>Inserts the string into a specific index in the collection.</summary>
		/// <param name="index">The position at which to insert the string.</param>
		/// <param name="value">The string to insert.</param>
		// Token: 0x0600063B RID: 1595 RVA: 0x00011CF2 File Offset: 0x0000FEF2
		public void Insert(int index, string value)
		{
			this.data.Insert(index, value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, value));
		}

		/// <summary>Gets a value indicating whether the contents of the collection are read-only.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Removes a string from the collection. </summary>
		/// <param name="value">The <see cref="T:System.String" /> to remove.</param>
		// Token: 0x0600063E RID: 1598 RVA: 0x00011D0E File Offset: 0x0000FF0E
		public void Remove(string value)
		{
			this.data.Remove(value);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, value));
		}

		/// <summary>Removes the string at the specified index.</summary>
		/// <param name="index">The zero-based index of the string to remove.</param>
		// Token: 0x0600063F RID: 1599 RVA: 0x00011D2C File Offset: 0x0000FF2C
		public void RemoveAt(int index)
		{
			string element = (string)this.data[index];
			this.data.RemoveAt(index);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, element));
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" />.</summary>
		/// <returns>Returns this <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" />.</returns>
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000069BD File Offset: 0x00004BBD
		public object SyncRoot
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				return this;
			}
		}

		/// <summary>Gets the element at a specified index. For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x1700021C RID: 540
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (string)value;
			}
		}

		/// <summary>Adds a string to the collection. For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The string to be added to the collection</param>
		/// <returns>The index at which the <paramref name="value" /> has been added. </returns>
		// Token: 0x06000643 RID: 1603 RVA: 0x00011D7C File Offset: 0x0000FF7C
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		/// <summary>Determines where the collection contains a specified string. For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The string to locate in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is found in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000644 RID: 1604 RVA: 0x00011D8A File Offset: 0x0000FF8A
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		/// <summary>Determines the index of a specified string in the collection. For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The string to locate in the collection.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06000645 RID: 1605 RVA: 0x00011D98 File Offset: 0x0000FF98
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		/// <summary>Inserts an item to the collection at the specified index. For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The string to insert into the collection.</param>
		// Token: 0x06000646 RID: 1606 RVA: 0x00011DA6 File Offset: 0x0000FFA6
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		/// <summary>Removes the first occurrence of a specific string from the collection. For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The string to remove from the collection.</param>
		// Token: 0x06000647 RID: 1607 RVA: 0x00011DB5 File Offset: 0x0000FFB5
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		/// <summary>Copies the strings of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index. For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the strings copied from collection. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		// Token: 0x06000648 RID: 1608 RVA: 0x00011CD5 File Offset: 0x0000FED5
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" />.</summary>
		/// <returns>An enumerator that iterates through the collection.</returns>
		// Token: 0x06000649 RID: 1609 RVA: 0x00011DC3 File Offset: 0x0000FFC3
		public IEnumerator GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x04000562 RID: 1378
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04000563 RID: 1379
		private ArrayList data = new ArrayList();
	}
}
