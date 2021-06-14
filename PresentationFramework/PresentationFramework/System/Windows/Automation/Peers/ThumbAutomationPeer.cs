using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.Thumb" /> types to UI Automation.</summary>
	// Token: 0x020002EB RID: 747
	public class ThumbAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" />.</param>
		// Token: 0x06002846 RID: 10310 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ThumbAutomationPeer(Thumb owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Thumb".</returns>
		// Token: 0x06002847 RID: 10311 RVA: 0x000BC026 File Offset: 0x000BA226
		protected override string GetClassNameCore()
		{
			return "Thumb";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Thumb" /> enumeration value.</returns>
		// Token: 0x06002848 RID: 10312 RVA: 0x0009450F File Offset: 0x0009270F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Thumb;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x06002849 RID: 10313 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
