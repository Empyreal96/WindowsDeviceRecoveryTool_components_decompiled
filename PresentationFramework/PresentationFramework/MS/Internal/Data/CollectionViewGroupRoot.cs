using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200070C RID: 1804
	internal class CollectionViewGroupRoot : CollectionViewGroupInternal, INotifyCollectionChanged
	{
		// Token: 0x060073B9 RID: 29625 RVA: 0x00211F7C File Offset: 0x0021017C
		internal CollectionViewGroupRoot(CollectionView view) : base("Root", null, false)
		{
			this._view = view;
		}

		// Token: 0x14000155 RID: 341
		// (add) Token: 0x060073BA RID: 29626 RVA: 0x00211FA0 File Offset: 0x002101A0
		// (remove) Token: 0x060073BB RID: 29627 RVA: 0x00211FD8 File Offset: 0x002101D8
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060073BC RID: 29628 RVA: 0x0021200D File Offset: 0x0021020D
		public void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x060073BD RID: 29629 RVA: 0x00212032 File Offset: 0x00210232
		public virtual ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._groupBy;
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x060073BE RID: 29630 RVA: 0x0021203A File Offset: 0x0021023A
		// (set) Token: 0x060073BF RID: 29631 RVA: 0x00212042 File Offset: 0x00210242
		public virtual GroupDescriptionSelectorCallback GroupBySelector
		{
			get
			{
				return this._groupBySelector;
			}
			set
			{
				this._groupBySelector = value;
			}
		}

		// Token: 0x060073C0 RID: 29632 RVA: 0x0021204B File Offset: 0x0021024B
		protected override void OnGroupByChanged()
		{
			if (this.GroupDescriptionChanged != null)
			{
				this.GroupDescriptionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000156 RID: 342
		// (add) Token: 0x060073C1 RID: 29633 RVA: 0x00212068 File Offset: 0x00210268
		// (remove) Token: 0x060073C2 RID: 29634 RVA: 0x002120A0 File Offset: 0x002102A0
		internal event EventHandler GroupDescriptionChanged;

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x060073C3 RID: 29635 RVA: 0x002120D5 File Offset: 0x002102D5
		// (set) Token: 0x060073C4 RID: 29636 RVA: 0x002120DD File Offset: 0x002102DD
		internal IComparer ActiveComparer
		{
			get
			{
				return this._comparer;
			}
			set
			{
				this._comparer = value;
			}
		}

		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x060073C5 RID: 29637 RVA: 0x002120E6 File Offset: 0x002102E6
		internal CultureInfo Culture
		{
			get
			{
				return this._view.Culture;
			}
		}

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x060073C6 RID: 29638 RVA: 0x002120F3 File Offset: 0x002102F3
		// (set) Token: 0x060073C7 RID: 29639 RVA: 0x002120FB File Offset: 0x002102FB
		internal bool IsDataInGroupOrder
		{
			get
			{
				return this._isDataInGroupOrder;
			}
			set
			{
				this._isDataInGroupOrder = value;
			}
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x060073C8 RID: 29640 RVA: 0x00212104 File Offset: 0x00210304
		internal CollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x0021210C File Offset: 0x0021030C
		internal void Initialize()
		{
			if (CollectionViewGroupRoot._topLevelGroupDescription == null)
			{
				CollectionViewGroupRoot._topLevelGroupDescription = new CollectionViewGroupRoot.TopLevelGroupDescription();
			}
			this.InitializeGroup(this, CollectionViewGroupRoot._topLevelGroupDescription, 0);
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x0021212C File Offset: 0x0021032C
		internal void AddToSubgroups(object item, LiveShapingItem lsi, bool loading)
		{
			this.AddToSubgroups(item, lsi, this, 0, loading);
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x00212139 File Offset: 0x00210339
		internal bool RemoveFromSubgroups(object item)
		{
			return this.RemoveFromSubgroups(item, this, 0);
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x00212144 File Offset: 0x00210344
		internal void RemoveItemFromSubgroupsByExhaustiveSearch(object item)
		{
			this.RemoveItemFromSubgroupsByExhaustiveSearch(this, item);
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x00212150 File Offset: 0x00210350
		internal void InsertSpecialItem(int index, object item, bool loading)
		{
			base.ChangeCounts(item, 1);
			base.ProtectedItems.Insert(index, item);
			if (!loading)
			{
				int index2 = base.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
			}
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x0021218C File Offset: 0x0021038C
		internal void RemoveSpecialItem(int index, object item, bool loading)
		{
			int index2 = -1;
			if (!loading)
			{
				index2 = base.LeafIndexFromItem(item, index);
			}
			base.ChangeCounts(item, -1);
			base.ProtectedItems.RemoveAt(index);
			if (!loading)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index2));
			}
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x002121CC File Offset: 0x002103CC
		internal void MoveWithinSubgroups(object item, LiveShapingItem lsi, IList list, int oldIndex, int newIndex)
		{
			if (lsi == null)
			{
				this.MoveWithinSubgroups(item, this, 0, list, oldIndex, newIndex);
				return;
			}
			CollectionViewGroupInternal parentGroup = lsi.ParentGroup;
			if (parentGroup != null)
			{
				this.MoveWithinSubgroup(item, parentGroup, list, oldIndex, newIndex);
				return;
			}
			foreach (CollectionViewGroupInternal group in lsi.ParentGroups)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
			}
		}

		// Token: 0x060073D0 RID: 29648 RVA: 0x00212250 File Offset: 0x00210450
		protected override int FindIndex(object item, object seed, IComparer comparer, int low, int high)
		{
			IEditableCollectionView editableCollectionView = this._view as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
				{
					low++;
					if (editableCollectionView.IsAddingNew)
					{
						low++;
					}
				}
				else
				{
					if (editableCollectionView.IsAddingNew)
					{
						high--;
					}
					if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
					{
						high--;
					}
				}
			}
			return base.FindIndex(item, seed, comparer, low, high);
		}

		// Token: 0x060073D1 RID: 29649 RVA: 0x002122B8 File Offset: 0x002104B8
		internal void RestoreGrouping(LiveShapingItem lsi, List<AbandonedGroupItem> deleteList)
		{
			CollectionViewGroupRoot.GroupTreeNode groupTreeNode = this.BuildGroupTree(lsi);
			groupTreeNode.ContainsItem = true;
			this.RestoreGrouping(lsi, groupTreeNode, 0, deleteList);
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x002122E0 File Offset: 0x002104E0
		private void RestoreGrouping(LiveShapingItem lsi, CollectionViewGroupRoot.GroupTreeNode node, int level, List<AbandonedGroupItem> deleteList)
		{
			if (node.ContainsItem)
			{
				object obj = this.GetGroupName(lsi.Item, node.Group.GroupBy, level);
				if (obj == CollectionViewGroupRoot.UseAsItemDirectly)
				{
					goto IL_12E;
				}
				ICollection collection = obj as ICollection;
				ArrayList arrayList = (collection == null) ? null : new ArrayList(collection);
				for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode = node.FirstChild; groupTreeNode != null; groupTreeNode = groupTreeNode.Sibling)
				{
					if (arrayList == null)
					{
						if (object.Equals(obj, groupTreeNode.Group.Name))
						{
							groupTreeNode.ContainsItem = true;
							obj = DependencyProperty.UnsetValue;
							break;
						}
					}
					else if (arrayList.Contains(groupTreeNode.Group.Name))
					{
						groupTreeNode.ContainsItem = true;
						arrayList.Remove(groupTreeNode.Group.Name);
					}
				}
				if (arrayList == null)
				{
					if (obj != DependencyProperty.UnsetValue)
					{
						this.AddToSubgroup(lsi.Item, lsi, node.Group, level, obj, false);
						goto IL_12E;
					}
					goto IL_12E;
				}
				else
				{
					using (IEnumerator enumerator = arrayList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object name = enumerator.Current;
							this.AddToSubgroup(lsi.Item, lsi, node.Group, level, name, false);
						}
						goto IL_12E;
					}
				}
			}
			if (node.ContainsItemDirectly)
			{
				deleteList.Add(new AbandonedGroupItem(lsi, node.Group));
			}
			IL_12E:
			for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = node.FirstChild; groupTreeNode2 != null; groupTreeNode2 = groupTreeNode2.Sibling)
			{
				this.RestoreGrouping(lsi, groupTreeNode2, level + 1, deleteList);
			}
		}

		// Token: 0x060073D3 RID: 29651 RVA: 0x00212450 File Offset: 0x00210650
		private CollectionViewGroupRoot.GroupTreeNode BuildGroupTree(LiveShapingItem lsi)
		{
			CollectionViewGroupInternal collectionViewGroupInternal = lsi.ParentGroup;
			if (collectionViewGroupInternal != null)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = collectionViewGroupInternal,
					ContainsItemDirectly = true
				};
				for (;;)
				{
					CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal;
					collectionViewGroupInternal = collectionViewGroupInternal2.Parent;
					if (collectionViewGroupInternal == null)
					{
						break;
					}
					CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = new CollectionViewGroupRoot.GroupTreeNode
					{
						Group = collectionViewGroupInternal,
						FirstChild = groupTreeNode
					};
					groupTreeNode = groupTreeNode2;
				}
				return groupTreeNode;
			}
			List<CollectionViewGroupInternal> parentGroups = lsi.ParentGroups;
			List<CollectionViewGroupRoot.GroupTreeNode> list = new List<CollectionViewGroupRoot.GroupTreeNode>(parentGroups.Count + 1);
			CollectionViewGroupRoot.GroupTreeNode result = null;
			foreach (CollectionViewGroupInternal group in parentGroups)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = group,
					ContainsItemDirectly = true
				};
				list.Add(groupTreeNode);
			}
			for (int i = 0; i < list.Count; i++)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = list[i];
				collectionViewGroupInternal = groupTreeNode.Group.Parent;
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode3 = null;
				if (collectionViewGroupInternal == null)
				{
					result = groupTreeNode;
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (list[j].Group == collectionViewGroupInternal)
						{
							groupTreeNode3 = list[j];
							break;
						}
					}
					if (groupTreeNode3 == null)
					{
						groupTreeNode3 = new CollectionViewGroupRoot.GroupTreeNode
						{
							Group = collectionViewGroupInternal,
							FirstChild = groupTreeNode
						};
						list.Add(groupTreeNode3);
					}
					else
					{
						groupTreeNode.Sibling = groupTreeNode3.FirstChild;
						groupTreeNode3.FirstChild = groupTreeNode;
					}
				}
			}
			return result;
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x002125C4 File Offset: 0x002107C4
		internal void DeleteAbandonedGroupItems(List<AbandonedGroupItem> deleteList)
		{
			foreach (AbandonedGroupItem abandonedGroupItem in deleteList)
			{
				this.RemoveFromGroupDirectly(abandonedGroupItem.Group, abandonedGroupItem.Item.Item);
				abandonedGroupItem.Item.RemoveParentGroup(abandonedGroupItem.Group);
			}
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x00212634 File Offset: 0x00210834
		private void InitializeGroup(CollectionViewGroupInternal group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = this.GetGroupDescription(group, parentDescription, level);
			group.GroupBy = groupDescription;
			ObservableCollection<object> observableCollection = (groupDescription != null) ? groupDescription.GroupNames : null;
			if (observableCollection != null)
			{
				int i = 0;
				int count = observableCollection.Count;
				while (i < count)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = new CollectionViewGroupInternal(observableCollection[i], group, true);
					this.InitializeGroup(collectionViewGroupInternal, groupDescription, level + 1);
					group.Add(collectionViewGroupInternal);
					i++;
				}
			}
			group.LastIndex = 0;
		}

		// Token: 0x060073D6 RID: 29654 RVA: 0x002126A0 File Offset: 0x002108A0
		private GroupDescription GetGroupDescription(CollectionViewGroup group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = null;
			if (group == this)
			{
				group = null;
			}
			if (groupDescription == null && this.GroupBySelector != null)
			{
				groupDescription = this.GroupBySelector(group, level);
			}
			if (groupDescription == null && level < this.GroupDescriptions.Count)
			{
				groupDescription = this.GroupDescriptions[level];
			}
			return groupDescription;
		}

		// Token: 0x060073D7 RID: 29655 RVA: 0x002126F0 File Offset: 0x002108F0
		private void AddToSubgroups(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, bool loading)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				if (lsi != null)
				{
					lsi.AddParentGroup(group);
				}
				if (loading)
				{
					group.Add(item);
					return;
				}
				int index = group.Insert(item, item, this.ActiveComparer);
				int index2 = group.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
				return;
			}
			else
			{
				ICollection collection;
				if ((collection = (groupName as ICollection)) == null)
				{
					this.AddToSubgroup(item, lsi, group, level, groupName, loading);
					return;
				}
				foreach (object name in collection)
				{
					this.AddToSubgroup(item, lsi, group, level, name, loading);
				}
				return;
			}
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x002127C0 File Offset: 0x002109C0
		private void AddToSubgroup(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, object name, bool loading)
		{
			int i = (loading && this.IsDataInGroupOrder) ? group.LastIndex : 0;
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				group.LastIndex = ((group.Items[i] == collectionViewGroupInternal) ? i : 0);
				this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
				return;
			}
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.LastIndex = i;
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
					return;
				}
				i++;
			}
			collectionViewGroupInternal = new CollectionViewGroupInternal(name, group, false);
			this.InitializeGroup(collectionViewGroupInternal, group.GroupBy, level + 1);
			if (loading)
			{
				group.Add(collectionViewGroupInternal);
				group.LastIndex = i;
			}
			else
			{
				group.Insert(collectionViewGroupInternal, item, this.ActiveComparer);
			}
			group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
			this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002128EC File Offset: 0x00210AEC
		private void MoveWithinSubgroups(object item, CollectionViewGroupInternal group, int level, IList list, int oldIndex, int newIndex)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
				return;
			}
			ICollection collection;
			if ((collection = (groupName as ICollection)) == null)
			{
				this.MoveWithinSubgroup(item, group, level, groupName, list, oldIndex, newIndex);
				return;
			}
			foreach (object name in collection)
			{
				this.MoveWithinSubgroup(item, group, level, name, list, oldIndex, newIndex);
			}
		}

		// Token: 0x060073DA RID: 29658 RVA: 0x00212988 File Offset: 0x00210B88
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, int level, object name, IList list, int oldIndex, int newIndex)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
				return;
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
					return;
				}
				i++;
			}
		}

		// Token: 0x060073DB RID: 29659 RVA: 0x00212A2D File Offset: 0x00210C2D
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, IList list, int oldIndex, int newIndex)
		{
			if (group.Move(item, list, ref oldIndex, ref newIndex))
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
			}
		}

		// Token: 0x060073DC RID: 29660 RVA: 0x00212A50 File Offset: 0x00210C50
		private object GetGroupNameKey(object name, CollectionViewGroupInternal group)
		{
			object result = name;
			PropertyGroupDescription propertyGroupDescription = group.GroupBy as PropertyGroupDescription;
			if (propertyGroupDescription != null)
			{
				string text = name as string;
				if (text != null)
				{
					if (propertyGroupDescription.StringComparison == StringComparison.OrdinalIgnoreCase || propertyGroupDescription.StringComparison == StringComparison.InvariantCultureIgnoreCase)
					{
						text = text.ToUpperInvariant();
					}
					else if (propertyGroupDescription.StringComparison == StringComparison.CurrentCultureIgnoreCase)
					{
						text = text.ToUpper(CultureInfo.CurrentCulture);
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060073DD RID: 29661 RVA: 0x00212AAC File Offset: 0x00210CAC
		private bool RemoveFromSubgroups(object item, CollectionViewGroupInternal group, int level)
		{
			bool result = false;
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			ICollection collection;
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				result = this.RemoveFromGroupDirectly(group, item);
			}
			else if ((collection = (groupName as ICollection)) == null)
			{
				if (this.RemoveFromSubgroup(item, group, level, groupName))
				{
					result = true;
				}
			}
			else
			{
				foreach (object name in collection)
				{
					if (this.RemoveFromSubgroup(item, group, level, name))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060073DE RID: 29662 RVA: 0x00212B48 File Offset: 0x00210D48
		private bool RemoveFromSubgroup(object item, CollectionViewGroupInternal group, int level, object name)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
				}
				i++;
			}
			return true;
		}

		// Token: 0x060073DF RID: 29663 RVA: 0x00212BDC File Offset: 0x00210DDC
		private bool RemoveFromGroupDirectly(CollectionViewGroupInternal group, object item)
		{
			int num = group.Remove(item, true);
			if (num >= 0)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, num));
				return false;
			}
			return true;
		}

		// Token: 0x060073E0 RID: 29664 RVA: 0x00212C08 File Offset: 0x00210E08
		private void RemoveItemFromSubgroupsByExhaustiveSearch(CollectionViewGroupInternal group, object item)
		{
			if (this.RemoveFromGroupDirectly(group, item))
			{
				for (int i = group.Items.Count - 1; i >= 0; i--)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = group.Items[i] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal != null)
					{
						this.RemoveItemFromSubgroupsByExhaustiveSearch(collectionViewGroupInternal, item);
					}
				}
			}
		}

		// Token: 0x060073E1 RID: 29665 RVA: 0x00212C54 File Offset: 0x00210E54
		private object GetGroupName(object item, GroupDescription groupDescription, int level)
		{
			if (groupDescription != null)
			{
				return groupDescription.GroupNameFromItem(item, level, this.Culture);
			}
			return CollectionViewGroupRoot.UseAsItemDirectly;
		}

		// Token: 0x040037BA RID: 14266
		private CollectionView _view;

		// Token: 0x040037BB RID: 14267
		private IComparer _comparer;

		// Token: 0x040037BC RID: 14268
		private bool _isDataInGroupOrder;

		// Token: 0x040037BD RID: 14269
		private ObservableCollection<GroupDescription> _groupBy = new ObservableCollection<GroupDescription>();

		// Token: 0x040037BE RID: 14270
		private GroupDescriptionSelectorCallback _groupBySelector;

		// Token: 0x040037BF RID: 14271
		private static GroupDescription _topLevelGroupDescription;

		// Token: 0x040037C0 RID: 14272
		private static readonly object UseAsItemDirectly = new NamedObject("UseAsItemDirectly");

		// Token: 0x02000B47 RID: 2887
		private class GroupTreeNode
		{
			// Token: 0x17001F7B RID: 8059
			// (get) Token: 0x06008D96 RID: 36246 RVA: 0x00259D58 File Offset: 0x00257F58
			// (set) Token: 0x06008D97 RID: 36247 RVA: 0x00259D60 File Offset: 0x00257F60
			public CollectionViewGroupRoot.GroupTreeNode FirstChild { get; set; }

			// Token: 0x17001F7C RID: 8060
			// (get) Token: 0x06008D98 RID: 36248 RVA: 0x00259D69 File Offset: 0x00257F69
			// (set) Token: 0x06008D99 RID: 36249 RVA: 0x00259D71 File Offset: 0x00257F71
			public CollectionViewGroupRoot.GroupTreeNode Sibling { get; set; }

			// Token: 0x17001F7D RID: 8061
			// (get) Token: 0x06008D9A RID: 36250 RVA: 0x00259D7A File Offset: 0x00257F7A
			// (set) Token: 0x06008D9B RID: 36251 RVA: 0x00259D82 File Offset: 0x00257F82
			public CollectionViewGroupInternal Group { get; set; }

			// Token: 0x17001F7E RID: 8062
			// (get) Token: 0x06008D9C RID: 36252 RVA: 0x00259D8B File Offset: 0x00257F8B
			// (set) Token: 0x06008D9D RID: 36253 RVA: 0x00259D93 File Offset: 0x00257F93
			public bool ContainsItem { get; set; }

			// Token: 0x17001F7F RID: 8063
			// (get) Token: 0x06008D9E RID: 36254 RVA: 0x00259D9C File Offset: 0x00257F9C
			// (set) Token: 0x06008D9F RID: 36255 RVA: 0x00259DA4 File Offset: 0x00257FA4
			public bool ContainsItemDirectly { get; set; }
		}

		// Token: 0x02000B48 RID: 2888
		private class TopLevelGroupDescription : GroupDescription
		{
			// Token: 0x06008DA2 RID: 36258 RVA: 0x00041D30 File Offset: 0x0003FF30
			public override object GroupNameFromItem(object item, int level, CultureInfo culture)
			{
				throw new NotSupportedException();
			}
		}
	}
}
