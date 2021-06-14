using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200000F RID: 15
	public class HyperlinkButton : ButtonBase
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003658 File Offset: 0x00001858
		// (set) Token: 0x06000060 RID: 96 RVA: 0x0000367A File Offset: 0x0000187A
		[TypeConverter(typeof(UriTypeConverter))]
		[Localizability(LocalizationCategory.Hyperlink)]
		[Bindable(true)]
		public Uri NavigateUri
		{
			get
			{
				return base.GetValue(HyperlinkButton.NavigateUriProperty) as Uri;
			}
			set
			{
				base.SetValue(HyperlinkButton.NavigateUriProperty, value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000368C File Offset: 0x0000188C
		// (set) Token: 0x06000062 RID: 98 RVA: 0x000036AE File Offset: 0x000018AE
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable)]
		[Bindable(true)]
		public string TargetName
		{
			get
			{
				return base.GetValue(HyperlinkButton.TargetNameProperty) as string;
			}
			set
			{
				base.SetValue(HyperlinkButton.TargetNameProperty, value);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000036C0 File Offset: 0x000018C0
		static HyperlinkButton()
		{
			HyperlinkButton.RequestNavigateEvent = Hyperlink.RequestNavigateEvent.AddOwner(typeof(HyperlinkButton));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(typeof(HyperlinkButton)));
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000064 RID: 100 RVA: 0x0000373E File Offset: 0x0000193E
		// (remove) Token: 0x06000065 RID: 101 RVA: 0x0000374E File Offset: 0x0000194E
		public event RequestNavigateEventHandler RequestNavigate
		{
			add
			{
				base.AddHandler(HyperlinkButton.RequestNavigateEvent, value);
			}
			remove
			{
				base.RemoveHandler(HyperlinkButton.RequestNavigateEvent, value);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000375E File Offset: 0x0000195E
		internal void DoAutomationPeerClick()
		{
			this.OnClick();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003768 File Offset: 0x00001968
		protected override void OnClick()
		{
			if (this.NavigateUri != null)
			{
				base.RaiseEvent(new RequestNavigateEventArgs(this.NavigateUri, this.TargetName));
			}
			base.OnClick();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000037AC File Offset: 0x000019AC
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HyperlinkButton.HyperlinkButtonAutomationPeer(this);
		}

		// Token: 0x04000018 RID: 24
		public static readonly DependencyProperty NavigateUriProperty = Hyperlink.NavigateUriProperty.AddOwner(typeof(HyperlinkButton));

		// Token: 0x04000019 RID: 25
		public static readonly DependencyProperty TargetNameProperty = Hyperlink.TargetNameProperty.AddOwner(typeof(HyperlinkButton));

		// Token: 0x02000010 RID: 16
		private sealed class HyperlinkButtonAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
		{
			// Token: 0x0600006A RID: 106 RVA: 0x000037D8 File Offset: 0x000019D8
			public HyperlinkButtonAutomationPeer(HyperlinkButton owner) : base(owner)
			{
				owner.Click += delegate(object s, RoutedEventArgs a)
				{
					base.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				};
			}

			// Token: 0x0600006B RID: 107 RVA: 0x0000380C File Offset: 0x00001A0C
			protected override AutomationControlType GetAutomationControlTypeCore()
			{
				return AutomationControlType.Hyperlink;
			}

			// Token: 0x0600006C RID: 108 RVA: 0x00003820 File Offset: 0x00001A20
			protected override string GetClassNameCore()
			{
				return "Hyperlink";
			}

			// Token: 0x0600006D RID: 109 RVA: 0x00003838 File Offset: 0x00001A38
			protected override bool IsControlElementCore()
			{
				return true;
			}

			// Token: 0x0600006E RID: 110 RVA: 0x00003870 File Offset: 0x00001A70
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

			// Token: 0x0600006F RID: 111 RVA: 0x000038AC File Offset: 0x00001AAC
			public override object GetPattern(PatternInterface patternInterface)
			{
				return (patternInterface == PatternInterface.Invoke) ? this : base.GetPattern(patternInterface);
			}
		}
	}
}
