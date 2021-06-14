using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.Selector" /> types to UI Automation.</summary>
	// Token: 0x020002DD RID: 733
	public abstract class SelectorAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.Selector" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" />.</param>
		// Token: 0x060027E8 RID: 10216 RVA: 0x000B54DC File Offset: 0x000B36DC
		protected SelectorAutomationPeer(Selector owner) : base(owner)
		{
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.Selector" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.List" /> enumeration value. </returns>
		// Token: 0x060027E9 RID: 10217 RVA: 0x0009580C File Offset: 0x00093A0C
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.Primitives.Selector" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Selection" />, this method returns a pointer to the current instance; otherwise <see langword="null" />.</returns>
		// Token: 0x060027EA RID: 10218 RVA: 0x000BB2A7 File Offset: 0x000B94A7
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Selection)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000B41CA File Offset: 0x000B23CA
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000BB2B6 File Offset: 0x000B94B6
		internal new static bool IsPropertySupportedByControlForFindItemInternal(int id)
		{
			return ItemsControlAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id) || SelectionItemPatternIdentifiers.IsSelectedProperty.Id == id;
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000B41D2 File Offset: 0x000B23D2
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return SelectorAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000BB2D4 File Offset: 0x000B94D4
		internal new static object GetSupportedPropertyValueInternal(AutomationPeer itemPeer, int propertyId)
		{
			if (SelectionItemPatternIdentifiers.IsSelectedProperty.Id != propertyId)
			{
				return ItemsControlAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
			}
			ISelectionItemProvider selectionItemProvider = itemPeer.GetPattern(PatternInterface.SelectionItem) as ISelectionItemProvider;
			if (selectionItemProvider != null)
			{
				return selectionItemProvider.IsSelected;
			}
			return null;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>A collection of UI Automation providers. true if multiple selection is allowed; otherwise false.</returns>
		// Token: 0x060027EF RID: 10223 RVA: 0x000BB314 File Offset: 0x000B9514
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			Selector selector = (Selector)base.Owner;
			int count = selector._selectedItems.Count;
			int count2 = selector.Items.Count;
			if (count > 0 && count2 > 0)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>(count);
				for (int i = 0; i < count; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer = this.FindOrCreateItemAutomationPeer(selector._selectedItems[i].Item) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer != null)
					{
						list.Add(base.ProviderFromPeer(selectorItemAutomationPeer));
					}
				}
				return list.ToArray();
			}
			return null;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selection is allowed; otherwise <see langword="false" />.</returns>
		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x000BB3A0 File Offset: 0x000B95A0
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				Selector selector = (Selector)base.Owner;
				return selector.CanSelectMultiple;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Returns <see langword="S_OK" /> if successful, or an error value otherwise.</returns>
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000BB3C0 File Offset: 0x000B95C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
		{
			if (base.ItemPeers.Count == 0)
			{
				base.RaiseAutomationEvent(AutomationEvents.SelectionPatternOnInvalidated);
				return;
			}
			Selector selector = (Selector)base.Owner;
			int count = selector._selectedItems.Count;
			int count2 = e.AddedItems.Count;
			int count3 = e.RemovedItems.Count;
			if (count == 1 && count2 == 1)
			{
				SelectorItemAutomationPeer selectorItemAutomationPeer = this.FindOrCreateItemAutomationPeer(selector._selectedItems[0].Item) as SelectorItemAutomationPeer;
				if (selectorItemAutomationPeer != null)
				{
					selectorItemAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (count2 + count3 > 20)
				{
					base.RaiseAutomationEvent(AutomationEvents.SelectionPatternOnInvalidated);
					return;
				}
				for (int i = 0; i < count2; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(e.AddedItems[i]) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer2 != null)
					{
						selectorItemAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
					}
				}
				for (int i = 0; i < count3; i++)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer3 = this.FindOrCreateItemAutomationPeer(e.RemovedItems[i]) as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer3 != null)
					{
						selectorItemAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
			}
		}
	}
}
