using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200072D RID: 1837
	internal class LiveShapingItem : DependencyObject
	{
		// Token: 0x06007588 RID: 30088 RVA: 0x00219550 File Offset: 0x00217750
		internal LiveShapingItem(object item, LiveShapingList list, bool filtered = false, LiveShapingBlock block = null, bool oneTime = false)
		{
			this._block = block;
			list.InitializeItem(this, item, filtered, oneTime);
			this.ForwardChanges = !oneTime;
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x06007589 RID: 30089 RVA: 0x00219576 File Offset: 0x00217776
		// (set) Token: 0x0600758A RID: 30090 RVA: 0x0021957E File Offset: 0x0021777E
		internal object Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600758B RID: 30091 RVA: 0x00219587 File Offset: 0x00217787
		// (set) Token: 0x0600758C RID: 30092 RVA: 0x0021958F File Offset: 0x0021778F
		internal LiveShapingBlock Block
		{
			get
			{
				return this._block;
			}
			set
			{
				this._block = value;
			}
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600758D RID: 30093 RVA: 0x00219598 File Offset: 0x00217798
		private LiveShapingList List
		{
			get
			{
				return this.Block.List;
			}
		}

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600758E RID: 30094 RVA: 0x002195A5 File Offset: 0x002177A5
		// (set) Token: 0x0600758F RID: 30095 RVA: 0x002195AE File Offset: 0x002177AE
		internal bool IsSortDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortDirty, value);
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x06007590 RID: 30096 RVA: 0x002195B8 File Offset: 0x002177B8
		// (set) Token: 0x06007591 RID: 30097 RVA: 0x002195C1 File Offset: 0x002177C1
		internal bool IsSortPendingClean
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsSortPendingClean, value);
			}
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x06007592 RID: 30098 RVA: 0x002195CB File Offset: 0x002177CB
		// (set) Token: 0x06007593 RID: 30099 RVA: 0x002195D4 File Offset: 0x002177D4
		internal bool IsFilterDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsFilterDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsFilterDirty, value);
			}
		}

		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x06007594 RID: 30100 RVA: 0x002195DE File Offset: 0x002177DE
		// (set) Token: 0x06007595 RID: 30101 RVA: 0x002195E7 File Offset: 0x002177E7
		internal bool IsGroupDirty
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsGroupDirty);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsGroupDirty, value);
			}
		}

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x06007596 RID: 30102 RVA: 0x002195F1 File Offset: 0x002177F1
		// (set) Token: 0x06007597 RID: 30103 RVA: 0x002195FB File Offset: 0x002177FB
		internal bool FailsFilter
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.FailsFilter);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.FailsFilter, value);
			}
		}

		// Token: 0x17001C01 RID: 7169
		// (get) Token: 0x06007598 RID: 30104 RVA: 0x00219606 File Offset: 0x00217806
		// (set) Token: 0x06007599 RID: 30105 RVA: 0x00219610 File Offset: 0x00217810
		internal bool ForwardChanges
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.ForwardChanges);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.ForwardChanges, value);
			}
		}

		// Token: 0x17001C02 RID: 7170
		// (get) Token: 0x0600759A RID: 30106 RVA: 0x0021961B File Offset: 0x0021781B
		// (set) Token: 0x0600759B RID: 30107 RVA: 0x00219625 File Offset: 0x00217825
		internal bool IsDeleted
		{
			get
			{
				return this.TestFlag(LiveShapingItem.PrivateFlags.IsDeleted);
			}
			set
			{
				this.ChangeFlag(LiveShapingItem.PrivateFlags.IsDeleted, value);
			}
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x00219630 File Offset: 0x00217830
		internal void FindPosition(out RBFinger<LiveShapingItem> oldFinger, out RBFinger<LiveShapingItem> newFinger, Comparison<LiveShapingItem> comparison)
		{
			this.Block.FindPosition(this, out oldFinger, out newFinger, comparison);
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x00219641 File Offset: 0x00217841
		internal RBFinger<LiveShapingItem> GetFinger()
		{
			return this.Block.GetFinger(this);
		}

		// Token: 0x17001C03 RID: 7171
		// (get) Token: 0x0600759E RID: 30110 RVA: 0x0021964F File Offset: 0x0021784F
		// (set) Token: 0x0600759F RID: 30111 RVA: 0x00219661 File Offset: 0x00217861
		internal int StartingIndex
		{
			get
			{
				return (int)base.GetValue(LiveShapingItem.StartingIndexProperty);
			}
			set
			{
				base.SetValue(LiveShapingItem.StartingIndexProperty, value);
			}
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x00219674 File Offset: 0x00217874
		internal int GetAndClearStartingIndex()
		{
			int startingIndex = this.StartingIndex;
			base.ClearValue(LiveShapingItem.StartingIndexProperty);
			return startingIndex;
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x00219694 File Offset: 0x00217894
		internal void SetBinding(string path, DependencyProperty dp, bool oneTime = false, bool enableXT = false)
		{
			if (enableXT && oneTime)
			{
				enableXT = false;
			}
			if (!base.LookupEntry(dp.GlobalIndex).Found)
			{
				if (!string.IsNullOrEmpty(path))
				{
					Binding binding;
					if (SystemXmlHelper.IsXmlNode(this._item))
					{
						binding = new Binding();
						binding.XPath = path;
					}
					else
					{
						binding = new Binding(path);
					}
					binding.Source = this._item;
					if (oneTime)
					{
						binding.Mode = BindingMode.OneTime;
					}
					BindingExpressionBase bindingExpressionBase = binding.CreateBindingExpression(this, dp);
					if (enableXT)
					{
						bindingExpressionBase.TargetWantsCrossThreadNotifications = true;
					}
					base.SetValue(dp, bindingExpressionBase);
					return;
				}
				if (!oneTime)
				{
					INotifyPropertyChanged notifyPropertyChanged = this.Item as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						PropertyChangedEventManager.AddHandler(notifyPropertyChanged, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), string.Empty);
					}
				}
			}
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x0021974A File Offset: 0x0021794A
		internal object GetValue(string path, DependencyProperty dp)
		{
			if (!string.IsNullOrEmpty(path))
			{
				this.SetBinding(path, dp, false, false);
				return base.GetValue(dp);
			}
			return this.Item;
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x0021976C File Offset: 0x0021796C
		internal void Clear()
		{
			this.List.ClearItem(this);
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x0021977A File Offset: 0x0021797A
		internal void OnCrossThreadPropertyChange(DependencyProperty dp)
		{
			this.List.OnItemPropertyChangedCrossThread(this, dp);
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x0021978C File Offset: 0x0021798C
		internal void AddParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			if (value == null)
			{
				base.SetValue(LiveShapingItem.ParentGroupsProperty, group);
				return;
			}
			List<CollectionViewGroupInternal> list;
			if ((list = (value as List<CollectionViewGroupInternal>)) == null)
			{
				list = new List<CollectionViewGroupInternal>(2);
				list.Add(value as CollectionViewGroupInternal);
				list.Add(group);
				base.SetValue(LiveShapingItem.ParentGroupsProperty, list);
				return;
			}
			list.Add(group);
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x002197F0 File Offset: 0x002179F0
		internal void RemoveParentGroup(CollectionViewGroupInternal group)
		{
			object value = base.GetValue(LiveShapingItem.ParentGroupsProperty);
			List<CollectionViewGroupInternal> list = value as List<CollectionViewGroupInternal>;
			if (list == null)
			{
				if (value == group)
				{
					base.ClearValue(LiveShapingItem.ParentGroupsProperty);
					return;
				}
			}
			else
			{
				list.Remove(group);
				if (list.Count == 1)
				{
					base.SetValue(LiveShapingItem.ParentGroupsProperty, list[0]);
				}
			}
		}

		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x060075A7 RID: 30119 RVA: 0x00219846 File Offset: 0x00217A46
		internal List<CollectionViewGroupInternal> ParentGroups
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as List<CollectionViewGroupInternal>;
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x060075A8 RID: 30120 RVA: 0x00219858 File Offset: 0x00217A58
		internal CollectionViewGroupInternal ParentGroup
		{
			get
			{
				return base.GetValue(LiveShapingItem.ParentGroupsProperty) as CollectionViewGroupInternal;
			}
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x0021986A File Offset: 0x00217A6A
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.ForwardChanges)
			{
				this.List.OnItemPropertyChanged(this, e.Property);
			}
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x00219887 File Offset: 0x00217A87
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.List.OnItemPropertyChanged(this, null);
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x00219896 File Offset: 0x00217A96
		private bool TestFlag(LiveShapingItem.PrivateFlags flag)
		{
			return (this._flags & flag) > (LiveShapingItem.PrivateFlags)0;
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x002198A3 File Offset: 0x00217AA3
		private void ChangeFlag(LiveShapingItem.PrivateFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x04003832 RID: 14386
		private static readonly DependencyProperty StartingIndexProperty = DependencyProperty.Register("StartingIndex", typeof(int), typeof(LiveShapingItem));

		// Token: 0x04003833 RID: 14387
		private static readonly DependencyProperty ParentGroupsProperty = DependencyProperty.Register("ParentGroups", typeof(object), typeof(LiveShapingItem));

		// Token: 0x04003834 RID: 14388
		private LiveShapingBlock _block;

		// Token: 0x04003835 RID: 14389
		private object _item;

		// Token: 0x04003836 RID: 14390
		private LiveShapingItem.PrivateFlags _flags;

		// Token: 0x02000B51 RID: 2897
		[Flags]
		private enum PrivateFlags
		{
			// Token: 0x04004AEF RID: 19183
			IsSortDirty = 1,
			// Token: 0x04004AF0 RID: 19184
			IsSortPendingClean = 2,
			// Token: 0x04004AF1 RID: 19185
			IsFilterDirty = 4,
			// Token: 0x04004AF2 RID: 19186
			IsGroupDirty = 8,
			// Token: 0x04004AF3 RID: 19187
			FailsFilter = 16,
			// Token: 0x04004AF4 RID: 19188
			ForwardChanges = 32,
			// Token: 0x04004AF5 RID: 19189
			IsDeleted = 64
		}
	}
}
