using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200000D RID: 13
	public class InvokableToggleButton : ToggleButton
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003546 File Offset: 0x00001746
		static InvokableToggleButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(InvokableToggleButton), new FrameworkPropertyMetadata(typeof(InvokableToggleButton)));
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000356D File Offset: 0x0000176D
		internal void DoAutomationPeerClick()
		{
			this.OnClick();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003578 File Offset: 0x00001778
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new InvokableToggleButton.InvokableToggleButtonAutomationPeer(this);
		}

		// Token: 0x0200000E RID: 14
		private sealed class InvokableToggleButtonAutomationPeer : ToggleButtonAutomationPeer, IInvokeProvider
		{
			// Token: 0x0600005A RID: 90 RVA: 0x000035A4 File Offset: 0x000017A4
			public InvokableToggleButtonAutomationPeer(InvokableToggleButton owner) : base(owner)
			{
				owner.Click += delegate(object s, RoutedEventArgs a)
				{
					base.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				};
			}

			// Token: 0x0600005B RID: 91 RVA: 0x000035FC File Offset: 0x000017FC
			void IInvokeProvider.Invoke()
			{
				if (!base.IsEnabled())
				{
					throw new ElementNotEnabledException();
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param0)
				{
					((HyperlinkButton)base.Owner).DoAutomationPeerClick();
					return null;
				}), null);
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00003638 File Offset: 0x00001838
			public override object GetPattern(PatternInterface patternInterface)
			{
				return (patternInterface == PatternInterface.Invoke) ? this : base.GetPattern(patternInterface);
			}
		}
	}
}
