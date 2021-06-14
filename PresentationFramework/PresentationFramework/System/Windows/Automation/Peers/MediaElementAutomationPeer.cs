using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.MediaElement" /> types to UI Automation.</summary>
	// Token: 0x020002D0 RID: 720
	public class MediaElementAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.MediaElementAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.MediaElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MediaElementAutomationPeer" />.</param>
		// Token: 0x0600277C RID: 10108 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public MediaElementAutomationPeer(MediaElement owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.MediaElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MediaElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "MediaElement".</returns>
		// Token: 0x0600277D RID: 10109 RVA: 0x000BA273 File Offset: 0x000B8473
		protected override string GetClassNameCore()
		{
			return "MediaElement";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.MediaElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MediaElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x0600277E RID: 10110 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
