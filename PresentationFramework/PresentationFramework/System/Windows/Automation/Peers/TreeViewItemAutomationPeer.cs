using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TreeViewItem" /> types to UI Automation.</summary>
	// Token: 0x020002F2 RID: 754
	public class TreeViewItemAutomationPeer : ItemsControlAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider, IScrollItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.</param>
		// Token: 0x06002881 RID: 10369 RVA: 0x000B54DC File Offset: 0x000B36DC
		public TreeViewItemAutomationPeer(TreeViewItem owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TreeViewItem".</returns>
		// Token: 0x06002882 RID: 10370 RVA: 0x000BC78E File Offset: 0x000BA98E
		protected override string GetClassNameCore()
		{
			return "TreeViewItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.TreeItem" /> enumeration value.</returns>
		// Token: 0x06002883 RID: 10371 RVA: 0x000BC795 File Offset: 0x000BA995
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TreeItem;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.ScrollItem" />, <see cref="F:System.Windows.Automation.Peers.PatternInterface.SelectionItem" />, or <see cref="F:System.Windows.Automation.Peers.PatternInterface.ExpandCollapse" />, this method returns the current instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />; otherwise, it returns <see langword="null" />.</returns>
		// Token: 0x06002884 RID: 10372 RVA: 0x000BC9FB File Offset: 0x000BABFB
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				return this;
			}
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			if (patternInterface == PatternInterface.ScrollItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002885 RID: 10373 RVA: 0x000BCA18 File Offset: 0x000BAC18
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> children = null;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			TreeViewItem treeViewItem = base.Owner as TreeViewItem;
			if (treeViewItem != null)
			{
				TreeViewItemAutomationPeer.iterate(this, treeViewItem, delegate(AutomationPeer peer)
				{
					if (children == null)
					{
						children = new List<AutomationPeer>();
					}
					children.Add(peer);
					return false;
				}, base.ItemPeers, itemPeers);
			}
			return children;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000BCA74 File Offset: 0x000BAC74
		private static bool iterate(TreeViewItemAutomationPeer logicalParentAp, DependencyObject parent, TreeViewItemAutomationPeer.IteratorCallback callback, ItemPeersStorage<ItemAutomationPeer> dataChildren, ItemPeersStorage<ItemAutomationPeer> oldChildren)
		{
			bool flag = false;
			if (parent != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
				int num = 0;
				while (num < childrenCount && !flag)
				{
					DependencyObject child = VisualTreeHelper.GetChild(parent, num);
					if (child != null && child is UIElement)
					{
						AutomationPeer automationPeer;
						if (child is TreeViewItem)
						{
							object item = (child is UIElement) ? (logicalParentAp.Owner as ItemsControl).GetItemOrContainerFromContainer(child as UIElement) : child;
							automationPeer = oldChildren[item];
							if (automationPeer == null)
							{
								automationPeer = logicalParentAp.GetPeerFromWeakRefStorage(item);
								if (automationPeer != null)
								{
									automationPeer.AncestorsInvalid = false;
									automationPeer.ChildrenValid = false;
								}
							}
							if (automationPeer == null)
							{
								automationPeer = logicalParentAp.CreateItemAutomationPeer(item);
							}
							if (automationPeer != null)
							{
								AutomationPeer wrapperPeer = (automationPeer as ItemAutomationPeer).GetWrapperPeer();
								if (wrapperPeer != null)
								{
									wrapperPeer.EventsSource = automationPeer;
								}
								if (dataChildren[item] == null && automationPeer is ItemAutomationPeer)
								{
									callback(automationPeer);
									dataChildren[item] = (automationPeer as ItemAutomationPeer);
								}
							}
						}
						else
						{
							automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)child);
							if (automationPeer != null)
							{
								flag = callback(automationPeer);
							}
						}
						if (automationPeer == null)
						{
							flag = TreeViewItemAutomationPeer.iterate(logicalParentAp, child, callback, dataChildren, oldChildren);
						}
					}
					else
					{
						flag = TreeViewItemAutomationPeer.iterate(logicalParentAp, child, callback, dataChildren, oldChildren);
					}
					num++;
				}
			}
			return flag;
		}

		/// <summary>Returns an <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for the specified object.</summary>
		/// <param name="item">The item to get an <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for.</param>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for the specified object.</returns>
		// Token: 0x06002887 RID: 10375 RVA: 0x000BCBA8 File Offset: 0x000BADA8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected internal override ItemAutomationPeer FindOrCreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer itemAutomationPeer = base.ItemPeers[item];
			AutomationPeer peer = this;
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				peer = (base.EventsSource as TreeViewDataItemAutomationPeer);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = base.GetPeerFromWeakRefStorage(item);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.CreateItemAutomationPeer(item);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.TrySetParentInfo(peer);
				}
			}
			if (itemAutomationPeer != null)
			{
				AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
				if (wrapperPeer != null)
				{
					wrapperPeer.EventsSource = itemAutomationPeer;
				}
			}
			return itemAutomationPeer;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000BC637 File Offset: 0x000BA837
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return base.IsPropertySupportedByControlForFindItem(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000BCC14 File Offset: 0x000BAE14
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			if (SelectionItemPatternIdentifiers.IsSelectedProperty.Id != propertyId)
			{
				return base.GetSupportedPropertyValue(itemPeer, propertyId);
			}
			ISelectionItemProvider selectionItemProvider = itemPeer.GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
			if (selectionItemProvider != null)
			{
				return selectionItemProvider.IsSelected;
			}
			return null;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of this <see cref="T:System.Windows.Controls.TreeView" />.</summary>
		/// <param name="item">The data item that is associated with the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />.</param>
		/// <returns>A new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> for <paramref name="item" />.</returns>
		// Token: 0x0600288A RID: 10378 RVA: 0x000BCC55 File Offset: 0x000BAE55
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TreeViewDataItemAutomationPeer(item, this, base.EventsSource as TreeViewDataItemAutomationPeer);
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000BCC6C File Offset: 0x000BAE6C
		internal override IDisposable UpdateChildren()
		{
			TreeViewDataItemAutomationPeer treeViewDataItemAutomationPeer = base.EventsSource as TreeViewDataItemAutomationPeer;
			if (treeViewDataItemAutomationPeer != null)
			{
				treeViewDataItemAutomationPeer.UpdateChildrenInternal(5);
			}
			else
			{
				base.UpdateChildrenInternal(5);
			}
			base.WeakRefElementProxyStorage.PurgeWeakRefCollection();
			return null;
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000BCCA4 File Offset: 0x000BAEA4
		internal void AddDataPeerInfo(TreeViewDataItemAutomationPeer dataPeer)
		{
			base.EventsSource = dataPeer;
			this.UpdateWeakRefStorageFromDataPeer();
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000BCCB4 File Offset: 0x000BAEB4
		internal void UpdateWeakRefStorageFromDataPeer()
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				if ((base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache == null)
				{
					(base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache = base.WeakRefElementProxyStorage;
					return;
				}
				if (base.WeakRefElementProxyStorage.Count == 0)
				{
					base.WeakRefElementProxyStorage = (base.EventsSource as TreeViewDataItemAutomationPeer).WeakRefElementProxyStorageCache;
				}
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600288E RID: 10382 RVA: 0x000BCD1C File Offset: 0x000BAF1C
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
			if (!treeViewItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			treeViewItem.IsExpanded = true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600288F RID: 10383 RVA: 0x000BCD64 File Offset: 0x000BAF64
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
			if (!treeViewItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			treeViewItem.IsExpanded = false;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The state (expanded or collapsed) of the control.</returns>
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x000BCDAC File Offset: 0x000BAFAC
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.Owner;
				if (!treeViewItem.HasItems)
				{
					return ExpandCollapseState.LeafNode;
				}
				if (!treeViewItem.IsExpanded)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000BCDDC File Offset: 0x000BAFDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				(base.EventsSource as TreeViewDataItemAutomationPeer).RaiseExpandCollapseAutomationEvent(oldValue, newValue);
				return;
			}
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002892 RID: 10386 RVA: 0x000BCE2C File Offset: 0x000BB02C
		void ISelectionItemProvider.Select()
		{
			((TreeViewItem)base.Owner).IsSelected = true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002893 RID: 10387 RVA: 0x000BCE40 File Offset: 0x000BB040
		void ISelectionItemProvider.AddToSelection()
		{
			TreeView parentTreeView = ((TreeViewItem)base.Owner).ParentTreeView;
			if (parentTreeView == null || (parentTreeView.SelectedItem != null && parentTreeView.SelectedContainer != base.Owner))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			((TreeViewItem)base.Owner).IsSelected = true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002894 RID: 10388 RVA: 0x000BCE98 File Offset: 0x000BB098
		void ISelectionItemProvider.RemoveFromSelection()
		{
			((TreeViewItem)base.Owner).IsSelected = false;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise <see langword="false" />.</returns>
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000BCEAB File Offset: 0x000BB0AB
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return ((TreeViewItem)base.Owner).IsSelected;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The selection container.</returns>
		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x000BCEC0 File Offset: 0x000BB0C0
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				ItemsControl parentItemsControl = ((TreeViewItem)base.Owner).ParentItemsControl;
				if (parentItemsControl != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(parentItemsControl);
					if (automationPeer != null)
					{
						return base.ProviderFromPeer(automationPeer);
					}
				}
				return null;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002897 RID: 10391 RVA: 0x000BCEF4 File Offset: 0x000BB0F4
		void IScrollItemProvider.ScrollIntoView()
		{
			((TreeViewItem)base.Owner).BringIntoView();
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000BCF06 File Offset: 0x000BB106
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			if (base.EventsSource is TreeViewDataItemAutomationPeer)
			{
				(base.EventsSource as TreeViewDataItemAutomationPeer).RaiseAutomationIsSelectedChanged(isSelected);
				return;
			}
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000BCF41 File Offset: 0x000BB141
		internal void RaiseAutomationSelectionEvent(AutomationEvents eventId)
		{
			if (base.EventsSource != null)
			{
				base.EventsSource.RaiseAutomationEvent(eventId);
				return;
			}
			base.RaiseAutomationEvent(eventId);
		}

		// Token: 0x020008BF RID: 2239
		// (Invoke) Token: 0x0600844C RID: 33868
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
