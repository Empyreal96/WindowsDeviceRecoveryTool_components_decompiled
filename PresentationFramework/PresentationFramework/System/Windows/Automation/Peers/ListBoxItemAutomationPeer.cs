using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the items in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of a <see cref="T:System.Windows.Controls.ListBox" /> to UI Automation.</summary>
	// Token: 0x020002CD RID: 717
	public class ListBoxItemAutomationPeer : SelectorItemAutomationPeer, IScrollItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />.</param>
		/// <param name="selectorAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" /> that is the parent of this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />.</param>
		// Token: 0x0600276B RID: 10091 RVA: 0x000BA0D8 File Offset: 0x000B82D8
		public ListBoxItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer) : base(owner, selectorAutomationPeer)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ListBoxItem".</returns>
		// Token: 0x0600276C RID: 10092 RVA: 0x000BA0E2 File Offset: 0x000B82E2
		protected override string GetClassNameCore()
		{
			return "ListBoxItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ListItem" /> enumeration value.</returns>
		// Token: 0x0600276D RID: 10093 RVA: 0x0001321D File Offset: 0x0001141D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ListItem;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.ScrollItem" />, this method returns the current instance of this <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" />.</returns>
		// Token: 0x0600276E RID: 10094 RVA: 0x000BA0E9 File Offset: 0x000B82E9
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ScrollItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000BA0F8 File Offset: 0x000B82F8
		internal override void RealizeCore()
		{
			ComboBox comboBox = base.ItemsControlAutomationPeer.Owner as ComboBox;
			if (comboBox != null)
			{
				IExpandCollapseProvider expandCollapseProvider = ((IExpandCollapseProvider)UIElementAutomationPeer.FromElement(comboBox)) as ComboBoxAutomationPeer;
				if (expandCollapseProvider.ExpandCollapseState != ExpandCollapseState.Expanded)
				{
					expandCollapseProvider.Expand();
				}
			}
			base.RealizeCore();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002770 RID: 10096 RVA: 0x000BA140 File Offset: 0x000B8340
		void IScrollItemProvider.ScrollIntoView()
		{
			ListBox listBox = base.ItemsControlAutomationPeer.Owner as ListBox;
			if (listBox != null)
			{
				listBox.ScrollIntoView(base.Item);
				return;
			}
			ComboBoxAutomationPeer comboBoxAutomationPeer = base.ItemsControlAutomationPeer as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.ScrollItemIntoView(base.Item);
			}
		}
	}
}
