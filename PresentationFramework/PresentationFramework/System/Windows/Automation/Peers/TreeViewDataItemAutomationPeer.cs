using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TreeViewItem" /> types containing data items to UI Automation.</summary>
	// Token: 0x020002F1 RID: 753
	public class TreeViewDataItemAutomationPeer : ItemAutomationPeer, ISelectionItemProvider, IScrollItemProvider, IExpandCollapseProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> class. </summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />.</param>
		/// <param name="itemsControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</param>
		/// <param name="parentDataItemAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> that is the parent to this <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />.</param>
		// Token: 0x0600286B RID: 10347 RVA: 0x000BC6F0 File Offset: 0x000BA8F0
		public TreeViewDataItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer, TreeViewDataItemAutomationPeer parentDataItemAutomationPeer) : base(item, null)
		{
			if (itemsControlAutomationPeer.Owner is TreeView || parentDataItemAutomationPeer == null)
			{
				base.ItemsControlAutomationPeer = itemsControlAutomationPeer;
			}
			this._parentDataItemAutomationPeer = parentDataItemAutomationPeer;
		}

		/// <summary>Gets the control pattern for the element that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />.</summary>
		/// <param name="patternInterface">The type of pattern implemented by the element to retrieve.</param>
		/// <returns>The object that implements the pattern interface, or <see langword="null" /> if the specified pattern interface is not implemented by this peer.</returns>
		// Token: 0x0600286C RID: 10348 RVA: 0x000BC718 File Offset: 0x000BA918
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
			if (patternInterface == PatternInterface.ItemContainer || patternInterface == PatternInterface.SynchronizedInput)
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					if (patternInterface == PatternInterface.SynchronizedInput)
					{
						return treeViewItemAutomationPeer.GetPattern(patternInterface);
					}
					return treeViewItemAutomationPeer;
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000BC768 File Offset: 0x000BA968
		internal override AutomationPeer GetWrapperPeer()
		{
			AutomationPeer wrapperPeer = base.GetWrapperPeer();
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = wrapperPeer as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.AddDataPeerInfo(this);
			}
			return wrapperPeer;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string containing the value <see langword="TreeViewItem" />.</returns>
		// Token: 0x0600286E RID: 10350 RVA: 0x000BC78E File Offset: 0x000BA98E
		protected override string GetClassNameCore()
		{
			return "TreeViewItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.TreeViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Automation.Peers.AutomationControlType.TreeItem" />
		///      in all cases.</returns>
		// Token: 0x0600286F RID: 10351 RVA: 0x000BC795 File Offset: 0x000BA995
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TreeItem;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> that is the parent to this automation peer.</summary>
		/// <returns>The parent automation peer.</returns>
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x000BC799 File Offset: 0x000BA999
		public TreeViewDataItemAutomationPeer ParentDataItemAutomationPeer
		{
			get
			{
				return this._parentDataItemAutomationPeer;
			}
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000BC7A1 File Offset: 0x000BA9A1
		internal override ItemsControlAutomationPeer GetItemsControlAutomationPeer()
		{
			if (this._parentDataItemAutomationPeer == null)
			{
				return base.GetItemsControlAutomationPeer();
			}
			return this._parentDataItemAutomationPeer.GetWrapperPeer() as ItemsControlAutomationPeer;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000BC7C2 File Offset: 0x000BA9C2
		internal override void RealizeCore()
		{
			this.RecursiveScrollIntoView();
		}

		/// <summary>Displays all child nodes, controls, or content of the control.</summary>
		// Token: 0x06002873 RID: 10355 RVA: 0x000BC7CC File Offset: 0x000BA9CC
		void IExpandCollapseProvider.Expand()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				IExpandCollapseProvider expandCollapseProvider = treeViewItemAutomationPeer;
				expandCollapseProvider.Expand();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Hides all nodes, controls, or content that are descendants of the control.</summary>
		// Token: 0x06002874 RID: 10356 RVA: 0x000BC7F8 File Offset: 0x000BA9F8
		void IExpandCollapseProvider.Collapse()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				IExpandCollapseProvider expandCollapseProvider = treeViewItemAutomationPeer;
				expandCollapseProvider.Collapse();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Gets the state, expanded or collapsed, of the control.</summary>
		/// <returns>The state, expanded or collapsed, of the control.</returns>
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x000BC824 File Offset: 0x000BAA24
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					IExpandCollapseProvider expandCollapseProvider = treeViewItemAutomationPeer;
					return expandCollapseProvider.ExpandCollapseState;
				}
				base.ThrowElementNotAvailableException();
				return ExpandCollapseState.LeafNode;
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000B4088 File Offset: 0x000B2288
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		/// <summary>Clears any selection and then selects the current element.</summary>
		/// <exception cref="T:System.Windows.Automation.ElementNotAvailableException">UI Automation element is no longer available.</exception>
		// Token: 0x06002877 RID: 10359 RVA: 0x000BC850 File Offset: 0x000BAA50
		void ISelectionItemProvider.Select()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
				selectionItemProvider.Select();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Adds the current element to the collection of selected items.</summary>
		/// <exception cref="T:System.Windows.Automation.ElementNotAvailableException">UI Automation element is no longer available.</exception>
		// Token: 0x06002878 RID: 10360 RVA: 0x000BC87C File Offset: 0x000BAA7C
		void ISelectionItemProvider.AddToSelection()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
				selectionItemProvider.AddToSelection();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Removes the current element from the collection of selected items.</summary>
		/// <exception cref="T:System.Windows.Automation.ElementNotAvailableException">UI Automation element is no longer available.</exception>
		// Token: 0x06002879 RID: 10361 RVA: 0x000BC8A8 File Offset: 0x000BAAA8
		void ISelectionItemProvider.RemoveFromSelection()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
				selectionItemProvider.RemoveFromSelection();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Gets a value that indicates whether an item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if an item is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x000BC8D4 File Offset: 0x000BAAD4
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
					return selectionItemProvider.IsSelected;
				}
				return false;
			}
		}

		/// <summary>Gets the UI automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" /> and acts as the container for the calling object.</summary>
		/// <returns>The UI automation provider.</returns>
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000BC8FC File Offset: 0x000BAAFC
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null)
				{
					ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
					return selectionItemProvider.SelectionContainer;
				}
				base.ThrowElementNotAvailableException();
				return null;
			}
		}

		/// <summary>Scrolls the content area of a container object in order to display the control within the visible region (viewport) of the container.</summary>
		// Token: 0x0600287C RID: 10364 RVA: 0x000BC928 File Offset: 0x000BAB28
		void IScrollItemProvider.ScrollIntoView()
		{
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = this.GetWrapperPeer() as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				IScrollItemProvider scrollItemProvider = treeViewItemAutomationPeer;
				scrollItemProvider.ScrollIntoView();
				return;
			}
			this.RecursiveScrollIntoView();
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000BB665 File Offset: 0x000B9865
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000BC954 File Offset: 0x000BAB54
		private void RecursiveScrollIntoView()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = base.ItemsControlAutomationPeer;
			if (this.ParentDataItemAutomationPeer != null && itemsControlAutomationPeer == null)
			{
				this.ParentDataItemAutomationPeer.RecursiveScrollIntoView();
				itemsControlAutomationPeer = base.ItemsControlAutomationPeer;
			}
			if (itemsControlAutomationPeer != null)
			{
				TreeViewItemAutomationPeer treeViewItemAutomationPeer = itemsControlAutomationPeer as TreeViewItemAutomationPeer;
				if (treeViewItemAutomationPeer != null && ((IExpandCollapseProvider)treeViewItemAutomationPeer).ExpandCollapseState == ExpandCollapseState.Collapsed)
				{
					((IExpandCollapseProvider)treeViewItemAutomationPeer).Expand();
				}
				ItemsControl itemsControl = itemsControlAutomationPeer.Owner as ItemsControl;
				if (itemsControl != null)
				{
					if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
					{
						itemsControl.OnBringItemIntoView(base.Item);
						return;
					}
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(itemsControl.OnBringItemIntoView), base.Item);
				}
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000BC9EA File Offset: 0x000BABEA
		// (set) Token: 0x06002880 RID: 10368 RVA: 0x000BC9F2 File Offset: 0x000BABF2
		internal ItemPeersStorage<WeakReference> WeakRefElementProxyStorageCache
		{
			get
			{
				return this._WeakRefElementProxyStorageCache;
			}
			set
			{
				this._WeakRefElementProxyStorageCache = value;
			}
		}

		// Token: 0x04001B98 RID: 7064
		private TreeViewDataItemAutomationPeer _parentDataItemAutomationPeer;

		// Token: 0x04001B99 RID: 7065
		private ItemPeersStorage<WeakReference> _WeakRefElementProxyStorageCache;
	}
}
