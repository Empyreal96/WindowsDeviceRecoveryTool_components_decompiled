using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020002C8 RID: 712
	internal class ListViewGroupItemCollection : ListView.ListViewItemCollection.IInnerList
	{
		// Token: 0x06002AC1 RID: 10945 RVA: 0x000C9154 File Offset: 0x000C7354
		public ListViewGroupItemCollection(ListViewGroup group)
		{
			this.group = group;
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06002AC2 RID: 10946 RVA: 0x000C9163 File Offset: 0x000C7363
		public int Count
		{
			get
			{
				return this.Items.Count;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000C9170 File Offset: 0x000C7370
		private ArrayList Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ArrayList();
				}
				return this.items;
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x000C918B File Offset: 0x000C738B
		public bool OwnerIsVirtualListView
		{
			get
			{
				return this.group.ListView != null && this.group.ListView.VirtualMode;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000C91AC File Offset: 0x000C73AC
		public bool OwnerIsDesignMode
		{
			get
			{
				if (this.group.ListView != null)
				{
					ISite site = this.group.ListView.Site;
					return site != null && site.DesignMode;
				}
				return false;
			}
		}

		// Token: 0x17000A4E RID: 2638
		public ListViewItem this[int index]
		{
			get
			{
				return (ListViewItem)this.Items[index];
			}
			set
			{
				if (value != this.Items[index])
				{
					this.MoveToGroup((ListViewItem)this.Items[index], null);
					this.Items[index] = value;
					this.MoveToGroup((ListViewItem)this.Items[index], this.group);
				}
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000C9256 File Offset: 0x000C7456
		public ListViewItem Add(ListViewItem value)
		{
			this.CheckListViewItem(value);
			this.MoveToGroup(value, this.group);
			this.Items.Add(value);
			return value;
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000C927C File Offset: 0x000C747C
		public void AddRange(ListViewItem[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				this.CheckListViewItem(items[i]);
			}
			this.Items.AddRange(items);
			for (int j = 0; j < items.Length; j++)
			{
				this.MoveToGroup(items[j], this.group);
			}
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000C92CC File Offset: 0x000C74CC
		private void CheckListViewItem(ListViewItem item)
		{
			if (item.ListView != null && item.ListView != this.group.ListView)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					item.Text
				}), "item");
			}
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000C9318 File Offset: 0x000C7518
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.MoveToGroup(this[i], null);
			}
			this.Items.Clear();
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000C934F File Offset: 0x000C754F
		public bool Contains(ListViewItem item)
		{
			return this.Items.Contains(item);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000C935D File Offset: 0x000C755D
		public void CopyTo(Array dest, int index)
		{
			this.Items.CopyTo(dest, index);
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000C936C File Offset: 0x000C756C
		public IEnumerator GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000C9379 File Offset: 0x000C7579
		public int IndexOf(ListViewItem item)
		{
			return this.Items.IndexOf(item);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000C9387 File Offset: 0x000C7587
		public ListViewItem Insert(int index, ListViewItem item)
		{
			this.CheckListViewItem(item);
			this.MoveToGroup(item, this.group);
			this.Items.Insert(index, item);
			return item;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000C93AC File Offset: 0x000C75AC
		private void MoveToGroup(ListViewItem item, ListViewGroup newGroup)
		{
			ListViewGroup listViewGroup = item.Group;
			if (listViewGroup != newGroup)
			{
				item.group = newGroup;
				if (listViewGroup != null)
				{
					listViewGroup.Items.Remove(item);
				}
				this.UpdateNativeListViewItem(item);
			}
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000C93E1 File Offset: 0x000C75E1
		public void Remove(ListViewItem item)
		{
			this.Items.Remove(item);
			if (item.group == this.group)
			{
				item.group = null;
				this.UpdateNativeListViewItem(item);
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000C940B File Offset: 0x000C760B
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000C941A File Offset: 0x000C761A
		private void UpdateNativeListViewItem(ListViewItem item)
		{
			if (item.ListView != null && item.ListView.IsHandleCreated && !item.ListView.InsertingItemsNatively)
			{
				item.UpdateStateToListView(item.Index);
			}
		}

		// Token: 0x0400125E RID: 4702
		private ListViewGroup group;

		// Token: 0x0400125F RID: 4703
		private ArrayList items;
	}
}
