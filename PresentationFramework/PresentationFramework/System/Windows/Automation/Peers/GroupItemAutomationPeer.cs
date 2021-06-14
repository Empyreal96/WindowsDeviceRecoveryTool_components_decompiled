using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GroupItem" /> types to UI Automation.</summary>
	// Token: 0x020002BE RID: 702
	public class GroupItemAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GroupItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />.</param>
		// Token: 0x060026E4 RID: 9956 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public GroupItemAutomationPeer(GroupItem owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.GroupItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "GroupItem".</returns>
		// Token: 0x060026E5 RID: 9957 RVA: 0x000B8551 File Offset: 0x000B6751
		protected override string GetClassNameCore()
		{
			return "GroupItem";
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000B8558 File Offset: 0x000B6758
		protected override int GetPositionInSetCore()
		{
			int num = base.GetPositionInSetCore();
			if (num == -1)
			{
				GroupItem groupItem = (GroupItem)base.Owner;
				CollectionViewGroupInternal collectionViewGroupInternal = groupItem.GetValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					CollectionViewGroup parent = collectionViewGroupInternal.Parent;
					if (parent != null)
					{
						num = parent.Items.IndexOf(collectionViewGroupInternal) + 1;
					}
				}
			}
			return num;
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000B85AC File Offset: 0x000B67AC
		protected override int GetSizeOfSetCore()
		{
			int num = base.GetSizeOfSetCore();
			if (num == -1)
			{
				GroupItem groupItem = (GroupItem)base.Owner;
				CollectionViewGroupInternal collectionViewGroupInternal = groupItem.GetValue(ItemContainerGenerator.ItemForItemContainerProperty) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					CollectionViewGroup parent = collectionViewGroupInternal.Parent;
					if (parent != null)
					{
						num = parent.Items.Count;
					}
				}
			}
			return num;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.GroupItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Group" /> enumeration value.</returns>
		// Token: 0x060026E8 RID: 9960 RVA: 0x000B7289 File Offset: 0x000B5489
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.GroupItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="pattern" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.ExpandCollapse" /> and the <see cref="T:System.Windows.Controls.GroupItem" />that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" /> contains an <see cref="T:System.Windows.Controls.Expander" />, this method returns a reference to the current instance of the <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />.  Otherwise, this method calls the base implementation on <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer" /> which returns <see langword="null" />.</returns>
		// Token: 0x060026E9 RID: 9961 RVA: 0x000B85FC File Offset: 0x000B67FC
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				GroupItem groupItem = (GroupItem)base.Owner;
				if (groupItem.Expander != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(groupItem.Expander);
					if (automationPeer != null && automationPeer is IExpandCollapseProvider)
					{
						automationPeer.EventsSource = this;
						return (IExpandCollapseProvider)automationPeer;
					}
				}
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.GroupItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x060026EA RID: 9962 RVA: 0x000B8650 File Offset: 0x000B6850
		protected override List<AutomationPeer> GetChildrenCore()
		{
			GroupItem groupItem = (GroupItem)base.Owner;
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.Owner);
			if (itemsControl != null)
			{
				ItemsControlAutomationPeer itemsControlAutomationPeer = itemsControl.CreateAutomationPeer() as ItemsControlAutomationPeer;
				if (itemsControlAutomationPeer != null)
				{
					Panel itemsHost = groupItem.ItemsHost;
					if (itemsHost == null)
					{
						return null;
					}
					IList children = itemsHost.Children;
					List<AutomationPeer> list = new List<AutomationPeer>(children.Count);
					ItemPeersStorage<ItemAutomationPeer> itemPeersStorage = new ItemPeersStorage<ItemAutomationPeer>();
					bool useNetFx472CompatibleAccessibilityFeatures = AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures;
					if (!useNetFx472CompatibleAccessibilityFeatures && groupItem.Expander != null)
					{
						this._expanderPeer = UIElementAutomationPeer.CreatePeerForElement(groupItem.Expander);
						if (this._expanderPeer != null)
						{
							this._expanderPeer.EventsSource = this;
							this._expanderPeer.GetChildren();
						}
					}
					foreach (object obj in children)
					{
						UIElement uielement = (UIElement)obj;
						if (!((IGeneratorHost)itemsControl).IsItemItsOwnContainer(uielement))
						{
							UIElementAutomationPeer uielementAutomationPeer = uielement.CreateAutomationPeer() as UIElementAutomationPeer;
							if (uielementAutomationPeer != null)
							{
								list.Add(uielementAutomationPeer);
								if (useNetFx472CompatibleAccessibilityFeatures)
								{
									if (itemsControlAutomationPeer.RecentlyRealizedPeers.Count > 0 && this.AncestorsInvalid)
									{
										GroupItemAutomationPeer groupItemAutomationPeer = uielementAutomationPeer as GroupItemAutomationPeer;
										if (groupItemAutomationPeer != null)
										{
											groupItemAutomationPeer.InvalidateGroupItemPeersContainingRecentlyRealizedPeers(itemsControlAutomationPeer.RecentlyRealizedPeers);
										}
									}
								}
								else if (this.AncestorsInvalid)
								{
									GroupItemAutomationPeer groupItemAutomationPeer2 = uielementAutomationPeer as GroupItemAutomationPeer;
									if (groupItemAutomationPeer2 != null)
									{
										groupItemAutomationPeer2.AncestorsInvalid = true;
										groupItemAutomationPeer2.ChildrenValid = true;
									}
								}
							}
						}
						else
						{
							object obj2 = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
							if (obj2 != DependencyProperty.UnsetValue)
							{
								ItemAutomationPeer itemAutomationPeer = useNetFx472CompatibleAccessibilityFeatures ? itemsControlAutomationPeer.ItemPeers[obj2] : itemsControlAutomationPeer.ReusablePeerFor(obj2);
								itemAutomationPeer = itemsControlAutomationPeer.ReusePeerForItem(itemAutomationPeer, obj2);
								if (itemAutomationPeer != null)
								{
									if (useNetFx472CompatibleAccessibilityFeatures)
									{
										int num = itemsControlAutomationPeer.RecentlyRealizedPeers.IndexOf(itemAutomationPeer);
										if (num >= 0)
										{
											itemsControlAutomationPeer.RecentlyRealizedPeers.RemoveAt(num);
										}
									}
								}
								else
								{
									itemAutomationPeer = itemsControlAutomationPeer.CreateItemAutomationPeerInternal(obj2);
								}
								if (itemAutomationPeer != null)
								{
									AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
									if (wrapperPeer != null)
									{
										wrapperPeer.EventsSource = itemAutomationPeer;
										if (itemAutomationPeer.ChildrenValid && itemAutomationPeer.Children == null && this.AncestorsInvalid)
										{
											itemAutomationPeer.AncestorsInvalid = true;
											wrapperPeer.AncestorsInvalid = true;
										}
									}
								}
								bool flag = itemsControlAutomationPeer.ItemPeers[obj2] == null;
								if (itemAutomationPeer != null && (flag || (itemAutomationPeer.GetParent() == this && itemPeersStorage[obj2] == null)))
								{
									list.Add(itemAutomationPeer);
									itemPeersStorage[obj2] = itemAutomationPeer;
									if (flag)
									{
										itemsControlAutomationPeer.ItemPeers[obj2] = itemAutomationPeer;
									}
								}
							}
						}
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000B890C File Offset: 0x000B6B0C
		internal void InvalidateGroupItemPeersContainingRecentlyRealizedPeers(List<ItemAutomationPeer> recentlyRealizedPeers)
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.Owner);
			if (itemsControl != null)
			{
				CollectionViewGroupInternal collectionViewGroupInternal = itemsControl.ItemContainerGenerator.ItemFromContainer(base.Owner) as CollectionViewGroupInternal;
				if (collectionViewGroupInternal != null)
				{
					for (int i = 0; i < recentlyRealizedPeers.Count; i++)
					{
						ItemAutomationPeer itemAutomationPeer = recentlyRealizedPeers[i];
						object item = itemAutomationPeer.Item;
						if (collectionViewGroupInternal.LeafIndexOf(item) >= 0)
						{
							this.AncestorsInvalid = true;
							base.ChildrenValid = true;
						}
					}
				}
			}
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000B897C File Offset: 0x000B6B7C
		protected override void SetFocusCore()
		{
			GroupItem groupItem = (GroupItem)base.Owner;
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && groupItem.Expander != null)
			{
				ToggleButton expanderToggleButton = groupItem.Expander.ExpanderToggleButton;
				if (expanderToggleButton == null || !expanderToggleButton.Focus())
				{
					throw new InvalidOperationException(SR.Get("SetFocusFailed"));
				}
			}
			else
			{
				base.SetFocusCore();
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000B89D6 File Offset: 0x000B6BD6
		protected override bool IsKeyboardFocusableCore()
		{
			if (this._expanderPeer != null)
			{
				return this._expanderPeer.IsKeyboardFocusable();
			}
			return base.IsKeyboardFocusableCore();
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000B89F2 File Offset: 0x000B6BF2
		protected override bool HasKeyboardFocusCore()
		{
			if (this._expanderPeer != null)
			{
				return this._expanderPeer.HasKeyboardFocus();
			}
			return base.HasKeyboardFocusCore();
		}

		// Token: 0x04001B81 RID: 7041
		private AutomationPeer _expanderPeer;
	}
}
