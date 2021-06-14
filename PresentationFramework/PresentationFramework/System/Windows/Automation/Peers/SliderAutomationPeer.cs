using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Slider" /> types to UI Automation.</summary>
	// Token: 0x020002E0 RID: 736
	public class SliderAutomationPeer : RangeBaseAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.SliderAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Slider" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SliderAutomationPeer" />.</param>
		// Token: 0x060027FF RID: 10239 RVA: 0x000BA7BB File Offset: 0x000B89BB
		public SliderAutomationPeer(Slider owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Slider" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SliderAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Slider".</returns>
		// Token: 0x06002800 RID: 10240 RVA: 0x000BB688 File Offset: 0x000B9888
		protected override string GetClassNameCore()
		{
			return "Slider";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Slider" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SliderAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Slider" /> enumeration value.</returns>
		// Token: 0x06002801 RID: 10241 RVA: 0x000958DB File Offset: 0x00093ADB
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Slider;
		}

		/// <summary>Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Point" /> containing <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" />; the only clickable points in a <see cref="T:System.Windows.Controls.TabControl" /> are the child <see cref="T:System.Windows.Controls.TabItem" /> elements.</returns>
		// Token: 0x06002802 RID: 10242 RVA: 0x000BABB9 File Offset: 0x000B8DB9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}
	}
}
