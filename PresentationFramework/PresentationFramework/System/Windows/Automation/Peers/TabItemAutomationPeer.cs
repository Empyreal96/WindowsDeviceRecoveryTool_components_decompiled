using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TabItem" /> types to UI Automation.</summary>
	// Token: 0x020002E4 RID: 740
	public class TabItemAutomationPeer : SelectorItemAutomationPeer, ISelectionItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />.</param>
		/// <param name="tabControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.TabControlAutomationPeer" /> that is the parent of this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />.</param>
		// Token: 0x06002812 RID: 10258 RVA: 0x000BA0D8 File Offset: 0x000B82D8
		public TabItemAutomationPeer(object owner, TabControlAutomationPeer tabControlAutomationPeer) : base(owner, tabControlAutomationPeer)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TabItem".</returns>
		// Token: 0x06002813 RID: 10259 RVA: 0x000BB87D File Offset: 0x000B9A7D
		protected override string GetClassNameCore()
		{
			return "TabItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.TabItem" /> enumeration value.</returns>
		// Token: 0x06002814 RID: 10260 RVA: 0x0003BCFF File Offset: 0x00039EFF
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TabItem;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The string that contains the label. If set, this method returns the value of the <see cref="P:System.Windows.Automation.AutomationProperties.Name" /> property; otherwise this method will return the value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property.</returns>
		// Token: 0x06002815 RID: 10261 RVA: 0x000BB884 File Offset: 0x000B9A84
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore))
			{
				TabItem tabItem = base.GetWrapper() as TabItem;
				if (tabItem != null && tabItem.Header is string)
				{
					return AccessText.RemoveAccessKeyMarker(nameCore);
				}
			}
			return nameCore;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002816 RID: 10262 RVA: 0x000BB8C4 File Offset: 0x000B9AC4
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			TabItem tabItem = base.GetWrapper() as TabItem;
			if (tabItem != null && tabItem.IsSelected)
			{
				TabControl tabControl = base.ItemsControlAutomationPeer.Owner as TabControl;
				if (tabControl != null)
				{
					ContentPresenter selectedContentPresenter = tabControl.SelectedContentPresenter;
					if (selectedContentPresenter != null)
					{
						AutomationPeer automationPeer = new FrameworkElementAutomationPeer(selectedContentPresenter);
						List<AutomationPeer> children = automationPeer.GetChildren();
						if (children != null)
						{
							if (list == null)
							{
								list = children;
							}
							else
							{
								list.AddRange(children);
							}
						}
					}
				}
			}
			return list;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002817 RID: 10263 RVA: 0x000BB934 File Offset: 0x000B9B34
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TabItem tabItem = base.GetWrapper() as TabItem;
			if (tabItem != null && tabItem.IsSelected)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000BB978 File Offset: 0x000B9B78
		internal override void RealizeCore()
		{
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector != null && this != null)
			{
				if (selector.CanSelectMultiple)
				{
					((ISelectionItemProvider)this).AddToSelection();
					return;
				}
				((ISelectionItemProvider)this).Select();
			}
		}
	}
}
