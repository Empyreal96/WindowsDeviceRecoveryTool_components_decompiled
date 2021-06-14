using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Automation.TextPattern" /> types to UI Automation.</summary>
	// Token: 0x020002E8 RID: 744
	public abstract class TextAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TextAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Automation.TextPattern" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextAutomationPeer" />.</param>
		// Token: 0x06002832 RID: 10290 RVA: 0x000B30F9 File Offset: 0x000B12F9
		protected TextAutomationPeer(FrameworkElement owner) : base(owner)
		{
		}

		/// <summary>Gets the text label of the element that is associated with this <see cref="T:System.Windows.Automation.Peers.TextAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The value of <see cref="P:System.Windows.Automation.AutomationProperties.Name" /> or <see cref="P:System.Windows.Automation.AutomationProperties.LabeledBy" /> if either is set; otherwise this method returns an empty string. </returns>
		// Token: 0x06002833 RID: 10291 RVA: 0x000BBDF8 File Offset: 0x000B9FF8
		protected override string GetNameCore()
		{
			string name = AutomationProperties.GetName(base.Owner);
			if (string.IsNullOrEmpty(name))
			{
				AutomationPeer labeledByCore = this.GetLabeledByCore();
				if (labeledByCore != null)
				{
					name = labeledByCore.GetName();
				}
			}
			return name ?? string.Empty;
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000B40B6 File Offset: 0x000B22B6
		internal new IRawElementProviderSimple ProviderFromPeer(AutomationPeer peer)
		{
			return base.ProviderFromPeer(peer);
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000BBE34 File Offset: 0x000BA034
		internal DependencyObject ElementFromProvider(IRawElementProviderSimple provider)
		{
			DependencyObject result = null;
			AutomationPeer automationPeer = base.PeerFromProvider(provider);
			if (automationPeer is UIElementAutomationPeer)
			{
				result = ((UIElementAutomationPeer)automationPeer).Owner;
			}
			else if (automationPeer is ContentElementAutomationPeer)
			{
				result = ((ContentElementAutomationPeer)automationPeer).Owner;
			}
			return result;
		}

		// Token: 0x06002836 RID: 10294
		internal abstract List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end);
	}
}
