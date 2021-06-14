using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TreeView" /> types to UI Automation.</summary>
	// Token: 0x020002F0 RID: 752
	public class TreeViewAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TreeView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewAutomationPeer" />.</param>
		// Token: 0x06002860 RID: 10336 RVA: 0x000B54DC File Offset: 0x000B36DC
		public TreeViewAutomationPeer(TreeView owner) : base(owner)
		{
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Tree" /> enumeration value.</returns>
		// Token: 0x06002861 RID: 10337 RVA: 0x000BC4C6 File Offset: 0x000BA6C6
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Tree;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TreeView".</returns>
		// Token: 0x06002862 RID: 10338 RVA: 0x000BC4CA File Offset: 0x000BA6CA
		protected override string GetClassNameCore()
		{
			return "TreeView";
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>The current instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />, or <see langword="null" />.</returns>
		// Token: 0x06002863 RID: 10339 RVA: 0x000BC4D4 File Offset: 0x000BA6D4
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Selection)
			{
				return this;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				ItemsControl itemsControl = (ItemsControl)base.Owner;
				if (itemsControl.ScrollHost != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(itemsControl.ScrollHost);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						return (IScrollProvider)automationPeer;
					}
				}
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" /> elements, or <see langword="null" /> if the <see cref="T:System.Windows.Controls.TreeView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TreeViewAutomationPeer" /> is empty.</returns>
		// Token: 0x06002864 RID: 10340 RVA: 0x000BC52C File Offset: 0x000BA72C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this.IsVirtualized)
			{
				return base.GetChildrenCore();
			}
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection items = itemsControl.Items;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			if (items.Count > 0)
			{
				List<AutomationPeer> list = new List<AutomationPeer>(items.Count);
				for (int i = 0; i < items.Count; i++)
				{
					TreeViewItem treeViewItem = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
					if (treeViewItem != null)
					{
						ItemAutomationPeer itemAutomationPeer = itemPeers[items[i]];
						if (itemAutomationPeer == null)
						{
							itemAutomationPeer = this.CreateItemAutomationPeer(items[i]);
						}
						if (itemAutomationPeer != null)
						{
							AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
							if (wrapperPeer != null)
							{
								wrapperPeer.EventsSource = itemAutomationPeer;
							}
						}
						if (base.ItemPeers[items[i]] == null)
						{
							list.Add(itemAutomationPeer);
							base.ItemPeers[items[i]] = itemAutomationPeer;
						}
					}
				}
				return list;
			}
			return null;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of this <see cref="T:System.Windows.Controls.TreeView" />.</summary>
		/// <param name="item">The data item that is associated with the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" />.</param>
		/// <returns>A new instance of the <see cref="T:System.Windows.Automation.Peers.TreeViewDataItemAutomationPeer" /> for <paramref name="item" />.</returns>
		// Token: 0x06002865 RID: 10341 RVA: 0x000BC62D File Offset: 0x000BA82D
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TreeViewDataItemAutomationPeer(item, this, null);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000BC637 File Offset: 0x000BA837
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return base.IsPropertySupportedByControlForFindItem(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000BC654 File Offset: 0x000BA854
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

		/// <summary>Retrieves a UI Automation provider for each child element that is selected.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x06002868 RID: 10344 RVA: 0x000BC698 File Offset: 0x000BA898
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			IRawElementProviderSimple[] array = null;
			TreeViewItem selectedContainer = ((TreeView)base.Owner).SelectedContainer;
			if (selectedContainer != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(selectedContainer);
				if (automationPeer.EventsSource != null)
				{
					automationPeer = automationPeer.EventsSource;
				}
				if (automationPeer != null)
				{
					array = new IRawElementProviderSimple[]
					{
						base.ProviderFromPeer(automationPeer)
					};
				}
			}
			if (array == null)
			{
				array = new IRawElementProviderSimple[0];
			}
			return array;
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider allows more than one child element to be selected concurrently.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selection is allowed; otherwise <see langword="false" />.</returns>
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider requires at least one child element to be selected.</summary>
		/// <returns>
		///     <see langword="true" /> if selection is required; otherwise <see langword="false" />.</returns>
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}
	}
}
