using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> types to UI Automation.</summary>
	// Token: 0x020002D9 RID: 729
	public class RepeatButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.RepeatButtonAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RepeatButtonAutomationPeer" />.</param>
		// Token: 0x060027C3 RID: 10179 RVA: 0x000B309E File Offset: 0x000B129E
		public RepeatButtonAutomationPeer(RepeatButton owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "RepeatButton".</returns>
		// Token: 0x060027C4 RID: 10180 RVA: 0x000BAA7E File Offset: 0x000B8C7E
		protected override string GetClassNameCore()
		{
			return "RepeatButton";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RepeatButtonAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Button" /> enumeration value.</returns>
		// Token: 0x060027C5 RID: 10181 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Invoke" />, this method returns a reference to the current instance of the <see cref="T:System.Windows.Automation.Peers.RepeatButtonAutomationPeer" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x060027C6 RID: 10182 RVA: 0x000B30AE File Offset: 0x000B12AE
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027C7 RID: 10183 RVA: 0x000BAA88 File Offset: 0x000B8C88
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			RepeatButton repeatButton = (RepeatButton)base.Owner;
			repeatButton.AutomationButtonBaseClick();
		}
	}
}
