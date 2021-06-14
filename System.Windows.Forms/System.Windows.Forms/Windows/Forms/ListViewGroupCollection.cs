using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents the collection of groups within a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
	// Token: 0x020002C6 RID: 710
	[ListBindable(false)]
	public class ListViewGroupCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06002A99 RID: 10905 RVA: 0x000C8A03 File Offset: 0x000C6C03
		internal ListViewGroupCollection(ListView listView)
		{
			this.listView = listView;
		}

		/// <summary>Gets the number of groups in the collection.</summary>
		/// <returns>The number of groups in the collection.</returns>
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002A9A RID: 10906 RVA: 0x000C8A12 File Offset: 0x000C6C12
		public int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>The object used to synchronize the collection.</returns>
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x0000E214 File Offset: 0x0000C414
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000C8A1F File Offset: 0x000C6C1F
		private ArrayList List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ListViewGroup" /> at the specified index within the collection.</summary>
		/// <param name="index">The index within the collection of the <see cref="T:System.Windows.Forms.ListViewGroup" /> to get or set. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewGroup" /> at the specified index within the collection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than 0 or greater than or equal to <see cref="P:System.Windows.Forms.ListViewGroupCollection.Count" />.</exception>
		// Token: 0x17000A47 RID: 2631
		public ListViewGroup this[int index]
		{
			get
			{
				return (ListViewGroup)this.List[index];
			}
			set
			{
				if (this.List.Contains(value))
				{
					return;
				}
				this.List[index] = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ListViewGroup" /> with the specified <see cref="P:System.Windows.Forms.ListViewGroup.Name" /> property value. </summary>
		/// <param name="key">The name of the group to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewGroup" /> with the specified name, or <see langword="null" /> if no such <see cref="T:System.Windows.Forms.ListViewGroup" /> exists.</returns>
		// Token: 0x17000A48 RID: 2632
		public ListViewGroup this[string key]
		{
			get
			{
				if (this.list == null)
				{
					return null;
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					if (string.Compare(key, this[i].Name, false, CultureInfo.CurrentCulture) == 0)
					{
						return this[i];
					}
				}
				return null;
			}
			set
			{
				int num = -1;
				if (this.list == null)
				{
					return;
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					if (string.Compare(key, this[i].Name, false, CultureInfo.CurrentCulture) == 0)
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					this.list[num] = value;
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ListViewGroup" /> at the specified index within the collection.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewGroup" /> that represents the item located at the specified index within the collection.</returns>
		// Token: 0x17000A49 RID: 2633
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value is ListViewGroup)
				{
					this[index] = (ListViewGroup)value;
				}
			}
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.Forms.ListViewGroup" /> to the collection.</summary>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to add to the collection. </param>
		/// <returns>The index of the group within the collection, or -1 if the group is already present in the collection.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="group" /> contains at least one <see cref="T:System.Windows.Forms.ListViewItem" /> that belongs to a <see cref="T:System.Windows.Forms.ListView" /> control other than the one that owns this <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</exception>
		// Token: 0x06002AA6 RID: 10918 RVA: 0x000C8B3C File Offset: 0x000C6D3C
		public int Add(ListViewGroup group)
		{
			if (this.Contains(group))
			{
				return -1;
			}
			this.CheckListViewItems(group);
			group.ListViewInternal = this.listView;
			int result = this.List.Add(group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.InsertGroupInListView(this.List.Count, group);
				this.MoveGroupItems(group);
			}
			return result;
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Forms.ListViewGroup" /> to the collection using the specified values to initialize the <see cref="P:System.Windows.Forms.ListViewGroup.Name" /> and <see cref="P:System.Windows.Forms.ListViewGroup.Header" /> properties </summary>
		/// <param name="key">The initial value of the <see cref="P:System.Windows.Forms.ListViewGroup.Name" /> property for the new group.</param>
		/// <param name="headerText">The initial value of the <see cref="P:System.Windows.Forms.ListViewGroup.Header" /> property for the new group.</param>
		/// <returns>The new <see cref="T:System.Windows.Forms.ListViewGroup" />.</returns>
		// Token: 0x06002AA7 RID: 10919 RVA: 0x000C8BA0 File Offset: 0x000C6DA0
		public ListViewGroup Add(string key, string headerText)
		{
			ListViewGroup listViewGroup = new ListViewGroup(key, headerText);
			this.Add(listViewGroup);
			return listViewGroup;
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Forms.ListViewGroup" /> to the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to add to the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</param>
		/// <returns>The index at which the <see cref="T:System.Windows.Forms.ListViewGroup" /> has been added.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.ListViewGroup" />.-or-
		///         <paramref name="value" /> contains at least one <see cref="T:System.Windows.Forms.ListViewItem" /> that belongs to a <see cref="T:System.Windows.Forms.ListView" /> control other than the one that owns this <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</exception>
		// Token: 0x06002AA8 RID: 10920 RVA: 0x000C8BBE File Offset: 0x000C6DBE
		int IList.Add(object value)
		{
			if (value is ListViewGroup)
			{
				return this.Add((ListViewGroup)value);
			}
			throw new ArgumentException("value");
		}

		/// <summary>Adds an array of groups to the collection.</summary>
		/// <param name="groups">An array of type <see cref="T:System.Windows.Forms.ListViewGroup" /> that specifies the groups to add to the collection. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="groups" /> contains at least one group with at least one <see cref="T:System.Windows.Forms.ListViewItem" /> that belongs to a <see cref="T:System.Windows.Forms.ListView" /> control other than the one that owns this <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</exception>
		// Token: 0x06002AA9 RID: 10921 RVA: 0x000C8BE0 File Offset: 0x000C6DE0
		public void AddRange(ListViewGroup[] groups)
		{
			for (int i = 0; i < groups.Length; i++)
			{
				this.Add(groups[i]);
			}
		}

		/// <summary>Adds the groups in an existing <see cref="T:System.Windows.Forms.ListViewGroupCollection" /> to the collection.</summary>
		/// <param name="groups">A <see cref="T:System.Windows.Forms.ListViewGroupCollection" /> containing the groups to add to the collection. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="groups" /> contains at least one group with at least one <see cref="T:System.Windows.Forms.ListViewItem" /> that belongs to a <see cref="T:System.Windows.Forms.ListView" /> control other than the one that owns this <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</exception>
		// Token: 0x06002AAA RID: 10922 RVA: 0x000C8C08 File Offset: 0x000C6E08
		public void AddRange(ListViewGroupCollection groups)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				this.Add(groups[i]);
			}
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000C8C34 File Offset: 0x000C6E34
		private void CheckListViewItems(ListViewGroup group)
		{
			for (int i = 0; i < group.Items.Count; i++)
			{
				ListViewItem listViewItem = group.Items[i];
				if (listViewItem.ListView != null && listViewItem.ListView != this.listView)
				{
					throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
					{
						listViewItem.Text
					}));
				}
			}
		}

		/// <summary>Removes all groups from the collection.</summary>
		// Token: 0x06002AAC RID: 10924 RVA: 0x000C8C9C File Offset: 0x000C6E9C
		public void Clear()
		{
			if (this.listView.IsHandleCreated)
			{
				for (int i = 0; i < this.Count; i++)
				{
					this.listView.RemoveGroupFromListView(this[i]);
				}
			}
			for (int j = 0; j < this.Count; j++)
			{
				this[j].ListViewInternal = null;
			}
			this.List.Clear();
			this.listView.UpdateGroupView();
		}

		/// <summary>Determines whether the specified group is located in the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to locate in the collection. </param>
		/// <returns>
		///     <see langword="true" /> if the group is in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002AAD RID: 10925 RVA: 0x000C8D0D File Offset: 0x000C6F0D
		public bool Contains(ListViewGroup value)
		{
			return this.List.Contains(value);
		}

		/// <summary>Determines whether the specified value is located in the collection.</summary>
		/// <param name="value">An object that represents the <see cref="T:System.Windows.Forms.ListViewGroup" /> to locate in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.Windows.Forms.ListViewGroup" /> contained in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002AAE RID: 10926 RVA: 0x000C8D1B File Offset: 0x000C6F1B
		bool IList.Contains(object value)
		{
			return value is ListViewGroup && this.Contains((ListViewGroup)value);
		}

		/// <summary>Copies the groups in the collection to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The <see cref="T:System.Array" /> to which the groups are copied. </param>
		/// <param name="index">The first index within the array to which the groups are copied. </param>
		// Token: 0x06002AAF RID: 10927 RVA: 0x000C8D33 File Offset: 0x000C6F33
		public void CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>Returns an enumerator used to iterate through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the collection.</returns>
		// Token: 0x06002AB0 RID: 10928 RVA: 0x000C8D42 File Offset: 0x000C6F42
		public IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		/// <summary>Returns the index of the specified <see cref="T:System.Windows.Forms.ListViewGroup" /> within the collection.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to locate in the collection. </param>
		/// <returns>The zero-based index of the group within the collection, or -1 if the group is not in the collection.</returns>
		// Token: 0x06002AB1 RID: 10929 RVA: 0x000C8D4F File Offset: 0x000C6F4F
		public int IndexOf(ListViewGroup value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>Returns the index within the collection of the specified value.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to find in the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</param>
		/// <returns>The zero-based index of <paramref name="value" /> if it is in the collection; otherwise, -1.</returns>
		// Token: 0x06002AB2 RID: 10930 RVA: 0x000C8D5D File Offset: 0x000C6F5D
		int IList.IndexOf(object value)
		{
			if (value is ListViewGroup)
			{
				return this.IndexOf((ListViewGroup)value);
			}
			return -1;
		}

		/// <summary>Inserts the specified <see cref="T:System.Windows.Forms.ListViewGroup" /> into the collection at the specified index.</summary>
		/// <param name="index">The index within the collection at which to insert the group. </param>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to insert into the collection. </param>
		// Token: 0x06002AB3 RID: 10931 RVA: 0x000C8D78 File Offset: 0x000C6F78
		public void Insert(int index, ListViewGroup group)
		{
			if (this.Contains(group))
			{
				return;
			}
			group.ListViewInternal = this.listView;
			this.List.Insert(index, group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.InsertGroupInListView(index, group);
				this.MoveGroupItems(group);
			}
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Forms.ListViewGroup" /> into the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</summary>
		/// <param name="index">The position at which the <see cref="T:System.Windows.Forms.ListViewGroup" /> is added to the collection.</param>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to add to the collection.</param>
		// Token: 0x06002AB4 RID: 10932 RVA: 0x000C8DC9 File Offset: 0x000C6FC9
		void IList.Insert(int index, object value)
		{
			if (value is ListViewGroup)
			{
				this.Insert(index, (ListViewGroup)value);
			}
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x000C8DE0 File Offset: 0x000C6FE0
		private void MoveGroupItems(ListViewGroup group)
		{
			foreach (object obj in group.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.ListView == this.listView)
				{
					listViewItem.UpdateStateToListView(listViewItem.Index);
				}
			}
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.ListViewGroup" /> from the collection.</summary>
		/// <param name="group">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to remove from the collection. </param>
		// Token: 0x06002AB6 RID: 10934 RVA: 0x000C8E4C File Offset: 0x000C704C
		public void Remove(ListViewGroup group)
		{
			group.ListViewInternal = null;
			this.List.Remove(group);
			if (this.listView.IsHandleCreated)
			{
				this.listView.RemoveGroupFromListView(group);
			}
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.ListViewGroup" /> from the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.ListViewGroup" /> to remove from the <see cref="T:System.Windows.Forms.ListViewGroupCollection" />.</param>
		// Token: 0x06002AB7 RID: 10935 RVA: 0x000C8E7A File Offset: 0x000C707A
		void IList.Remove(object value)
		{
			if (value is ListViewGroup)
			{
				this.Remove((ListViewGroup)value);
			}
		}

		/// <summary>Removes the <see cref="T:System.Windows.Forms.ListViewGroup" /> at the specified index within the collection.</summary>
		/// <param name="index">The index within the collection of the <see cref="T:System.Windows.Forms.ListViewGroup" /> to remove. </param>
		// Token: 0x06002AB8 RID: 10936 RVA: 0x000C8E90 File Offset: 0x000C7090
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x0400125C RID: 4700
		private ListView listView;

		// Token: 0x0400125D RID: 4701
		private ArrayList list;
	}
}
