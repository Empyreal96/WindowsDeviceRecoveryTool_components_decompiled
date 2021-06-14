using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TabControl" /> types to UI Automation.</summary>
	// Token: 0x020002E3 RID: 739
	public class TabControlAutomationPeer : SelectorAutomationPeer, ISelectionProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TabControlAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TabControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabControlAutomationPeer" />.</param>
		// Token: 0x0600280C RID: 10252 RVA: 0x000B3DF4 File Offset: 0x000B1FF4
		public TabControlAutomationPeer(TabControl owner) : base(owner)
		{
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.TabItem" /> that is associated with the new <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />.</param>
		/// <returns>The created <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" /> object.</returns>
		// Token: 0x0600280D RID: 10253 RVA: 0x000BB86D File Offset: 0x000B9A6D
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new TabItemAutomationPeer(item, this);
		}

		/// <summary>Gets the collection of child elements for the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with the new <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Tab" /> enumeration value.</returns>
		// Token: 0x0600280E RID: 10254 RVA: 0x00095873 File Offset: 0x00093A73
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Tab;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TabItem" /> that is associated with the new <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TabControl".</returns>
		// Token: 0x0600280F RID: 10255 RVA: 0x000BB876 File Offset: 0x000B9A76
		protected override string GetClassNameCore()
		{
			return "TabControl";
		}

		/// <summary>This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Point" /> containing <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" />; the only clickable points in a <see cref="T:System.Windows.Controls.TabControl" /> are the child <see cref="T:System.Windows.Controls.TabItem" /> elements. </returns>
		// Token: 0x06002810 RID: 10256 RVA: 0x000BABB9 File Offset: 0x000B8DB9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Returns <see langword="S_OK" /> if successful, or an error value otherwise.</returns>
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x00016748 File Offset: 0x00014948
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return true;
			}
		}
	}
}
