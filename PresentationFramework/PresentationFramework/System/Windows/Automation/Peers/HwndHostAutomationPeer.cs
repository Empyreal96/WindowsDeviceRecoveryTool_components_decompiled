using System;
using System.Security;
using System.Windows.Interop;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002BF RID: 703
	internal class HwndHostAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060026EF RID: 9967 RVA: 0x000B8A0E File Offset: 0x000B6C0E
		public HwndHostAutomationPeer(HwndHost owner) : base(owner)
		{
			base.IsInteropPeer = true;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000B8A1E File Offset: 0x000B6C1E
		protected override string GetClassNameCore()
		{
			return "HwndHost";
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x00094CE7 File Offset: 0x00092EE7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000B8A28 File Offset: 0x000B6C28
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override InteropAutomationProvider GetInteropChild()
		{
			if (this._interopProvider == null)
			{
				HostedWindowWrapper wrapper = null;
				HwndHost hwndHost = (HwndHost)base.Owner;
				IntPtr criticalHandle = hwndHost.CriticalHandle;
				if (criticalHandle != IntPtr.Zero)
				{
					wrapper = HostedWindowWrapper.CreateInternal(criticalHandle);
				}
				this._interopProvider = new InteropAutomationProvider(wrapper, this);
			}
			return this._interopProvider;
		}

		// Token: 0x04001B82 RID: 7042
		private InteropAutomationProvider _interopProvider;
	}
}
