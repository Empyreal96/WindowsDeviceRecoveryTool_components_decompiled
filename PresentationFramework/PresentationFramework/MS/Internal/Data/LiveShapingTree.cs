using System;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x02000731 RID: 1841
	internal class LiveShapingTree : RBTree<LiveShapingItem>
	{
		// Token: 0x060075E6 RID: 30182 RVA: 0x0021A404 File Offset: 0x00218604
		internal LiveShapingTree(LiveShapingList list)
		{
			this._list = list;
		}

		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x060075E7 RID: 30183 RVA: 0x0021A413 File Offset: 0x00218613
		internal LiveShapingList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x060075E8 RID: 30184 RVA: 0x0021A41B File Offset: 0x0021861B
		internal LiveShapingBlock PlaceholderBlock
		{
			get
			{
				if (this._placeholderBlock == null)
				{
					this._placeholderBlock = new LiveShapingBlock(false);
					this._placeholderBlock.Parent = this;
				}
				return this._placeholderBlock;
			}
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x0021A443 File Offset: 0x00218643
		internal override RBNode<LiveShapingItem> NewNode()
		{
			return new LiveShapingBlock();
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x0021A44C File Offset: 0x0021864C
		internal void Move(int oldIndex, int newIndex)
		{
			LiveShapingItem item = base[oldIndex];
			base.RemoveAt(oldIndex);
			base.Insert(newIndex, item);
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x0021A470 File Offset: 0x00218670
		internal void RestoreLiveSortingByInsertionSort(Action<NotifyCollectionChangedEventArgs, int, int> RaiseMoveEvent)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				LiveShapingItem item = finger.Item;
				item.IsSortDirty = false;
				item.IsSortPendingClean = false;
				RBFinger<LiveShapingItem> newFinger = base.LocateItem(finger, base.Comparison);
				int index = finger.Index;
				int index2 = newFinger.Index;
				if (index != index2)
				{
					base.ReInsert(ref finger, newFinger);
					RaiseMoveEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item.Item, index, index2), index, index2);
				}
				finger = ++finger;
			}
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x0021A4F4 File Offset: 0x002186F4
		internal void FindPosition(LiveShapingItem lsi, out int oldIndex, out int newIndex)
		{
			RBFinger<LiveShapingItem> rbfinger;
			RBFinger<LiveShapingItem> rbfinger2;
			lsi.FindPosition(out rbfinger, out rbfinger2, base.Comparison);
			oldIndex = rbfinger.Index;
			newIndex = rbfinger2.Index;
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x0021A524 File Offset: 0x00218724
		internal void ReplaceAt(int index, object item)
		{
			RBFinger<LiveShapingItem> rbfinger = base.FindIndex(index, true);
			LiveShapingItem item2 = rbfinger.Item;
			item2.Clear();
			rbfinger.Node.SetItemAt(rbfinger.Offset, new LiveShapingItem(item, this.List, false, null, false));
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x0021A56C File Offset: 0x0021876C
		internal LiveShapingItem FindItem(object item)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				if (ItemsControl.EqualsEx(finger.Item.Item, item))
				{
					return finger.Item;
				}
				finger = ++finger;
			}
			return null;
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x0021A5B4 File Offset: 0x002187B4
		public override int IndexOf(LiveShapingItem lsi)
		{
			RBFinger<LiveShapingItem> finger = lsi.GetFinger();
			if (!finger.Found)
			{
				return -1;
			}
			return finger.Index;
		}

		// Token: 0x0400384E RID: 14414
		private LiveShapingList _list;

		// Token: 0x0400384F RID: 14415
		private LiveShapingBlock _placeholderBlock;
	}
}
