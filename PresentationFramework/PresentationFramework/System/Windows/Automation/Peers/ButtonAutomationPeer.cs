using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Button" /> types to UI Automation.</summary>
	// Token: 0x02000296 RID: 662
	public class ButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ButtonAutomationPeer" /> class.</summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x0600251B RID: 9499 RVA: 0x000B309E File Offset: 0x000B129E
		public ButtonAutomationPeer(Button owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the control that is associated with this UI Automation peer.</summary>
		/// <returns>A string that contains "Button".</returns>
		// Token: 0x0600251C RID: 9500 RVA: 0x000B30A7 File Offset: 0x000B12A7
		protected override string GetClassNameCore()
		{
			return "Button";
		}

		/// <summary>Gets the control type of the element that is associated with the UI Automation peer.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Button" /> enumeration value.</returns>
		// Token: 0x0600251D RID: 9501 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		/// <summary>Gets the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Invoke" />, this method returns a <see langword="this" /> pointer, otherwise this method returns <see langword="null" />.</returns>
		// Token: 0x0600251E RID: 9502 RVA: 0x000B30AE File Offset: 0x000B12AE
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600251F RID: 9503 RVA: 0x000B30BC File Offset: 0x000B12BC
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				((Button)base.Owner).AutomationButtonBaseClick();
				return null;
			}), null);
		}
	}
}
