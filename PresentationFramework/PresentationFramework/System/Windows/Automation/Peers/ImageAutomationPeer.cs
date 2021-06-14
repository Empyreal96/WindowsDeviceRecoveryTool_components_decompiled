using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Image" /> types to UI Automation.</summary>
	// Token: 0x020002C1 RID: 705
	public class ImageAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ImageAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Image" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ImageAutomationPeer" />.</param>
		// Token: 0x060026FA RID: 9978 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ImageAutomationPeer(Image owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Image" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ImageAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Image".</returns>
		// Token: 0x060026FB RID: 9979 RVA: 0x000B8B39 File Offset: 0x000B6D39
		protected override string GetClassNameCore()
		{
			return "Image";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Image" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ImageAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Image" /> enumeration value.</returns>
		// Token: 0x060026FC RID: 9980 RVA: 0x00094FD6 File Offset: 0x000931D6
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Image;
		}
	}
}
