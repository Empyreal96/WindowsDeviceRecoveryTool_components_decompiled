using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Represents a base class for exposing <see cref="T:System.Windows.Automation.TextPattern" /> types to UI Automation.</summary>
	// Token: 0x0200029D RID: 669
	public abstract class ContentTextAutomationPeer : FrameworkContentElementAutomationPeer
	{
		/// <summary>Provides initialization for base class values when called by the constructor of a derived class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Automation.TextPattern" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentTextAutomationPeer" />.</param>
		// Token: 0x0600255E RID: 9566 RVA: 0x000B40AD File Offset: 0x000B22AD
		protected ContentTextAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000B40B6 File Offset: 0x000B22B6
		internal new IRawElementProviderSimple ProviderFromPeer(AutomationPeer peer)
		{
			return base.ProviderFromPeer(peer);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000B40C0 File Offset: 0x000B22C0
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

		// Token: 0x06002561 RID: 9569
		internal abstract List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end);
	}
}
