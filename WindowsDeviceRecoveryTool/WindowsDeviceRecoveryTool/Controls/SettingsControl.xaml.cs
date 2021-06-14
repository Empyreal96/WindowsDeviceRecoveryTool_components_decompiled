using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200003F RID: 63
	public partial class SettingsControl : Grid
	{
		// Token: 0x06000237 RID: 567 RVA: 0x0000E75C File Offset: 0x0000C95C
		public SettingsControl()
		{
			this.InitializeComponent();
			this.timer = new Timer(5000.0);
			this.timer.Elapsed += this.TimerOnElapsed;
			base.GotFocus += this.SettingsControlGotFocus;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000238 RID: 568 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		// (remove) Token: 0x06000239 RID: 569 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		public event RoutedEventHandler Open
		{
			add
			{
				base.AddHandler(SettingsControl.OpenEvent, value);
			}
			remove
			{
				base.RemoveHandler(SettingsControl.OpenEvent, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600023A RID: 570 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		// (remove) Token: 0x0600023B RID: 571 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
		public event RoutedEventHandler Close
		{
			add
			{
				base.AddHandler(SettingsControl.CloseEvent, value);
			}
			remove
			{
				base.RemoveHandler(SettingsControl.CloseEvent, value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000E81A File Offset: 0x0000CA1A
		public bool IsOpened
		{
			get
			{
				return (bool)base.GetValue(SettingsControl.IsOpenedProperty);
			}
			set
			{
				base.SetValue(SettingsControl.IsOpenedProperty, value);
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E82F File Offset: 0x0000CA2F
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			this.timer.Stop();
			base.OnMouseEnter(e);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E846 File Offset: 0x0000CA46
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			this.timer.Start();
			base.OnMouseLeave(e);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E860 File Offset: 0x0000CA60
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!this.IsOpened)
			{
				base.SetCurrentValue(SettingsControl.IsOpenedProperty, true);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E88C File Offset: 0x0000CA8C
		protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!this.IsOpened)
			{
				base.SetCurrentValue(SettingsControl.IsOpenedProperty, true);
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			e.Handled = true;
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E8CB File Offset: 0x0000CACB
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			e.Handled = true;
			base.OnMouseRightButtonDown(e);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		private static void OnIsOpenedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SettingsControl settingsControl = d as SettingsControl;
			if (settingsControl != null && (bool)e.NewValue != (bool)e.OldValue)
			{
				if ((bool)e.NewValue)
				{
					settingsControl.RaiseOpenEvent();
					settingsControl.timer.Start();
				}
				else
				{
					settingsControl.timer.Stop();
					settingsControl.RaiseCloseEvent();
				}
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			base.Dispatcher.BeginInvoke(new Action(delegate()
			{
				if (!base.IsMouseOver && !base.IsFocused)
				{
					this.timer.Stop();
					base.SetCurrentValue(SettingsControl.IsOpenedProperty, false);
				}
			}), new object[0]);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		private void RaiseCloseEvent()
		{
			RoutedEventArgs e = new RoutedEventArgs(SettingsControl.CloseEvent);
			base.RaiseEvent(e);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E9E8 File Offset: 0x0000CBE8
		private void RaiseOpenEvent()
		{
			RoutedEventArgs e = new RoutedEventArgs(SettingsControl.OpenEvent);
			base.RaiseEvent(e);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000EA09 File Offset: 0x0000CC09
		private void SettingsButtonOnClick(object sender, RoutedEventArgs e)
		{
			base.SetCurrentValue(SettingsControl.IsOpenedProperty, false);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000EA20 File Offset: 0x0000CC20
		private void SettingsControlGotFocus(object sender, RoutedEventArgs e)
		{
			this.timer.Stop();
			if (!this.IsOpened)
			{
				base.Focus();
				base.SetCurrentValue(SettingsControl.IsOpenedProperty, true);
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000EA5F File Offset: 0x0000CC5F
		private void HyperlinkButtonOnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000EA80 File Offset: 0x0000CC80
		private void HelpButtonOnClick(object sender, RoutedEventArgs e)
		{
			base.SetCurrentValue(SettingsControl.IsOpenedProperty, false);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000EB78 File Offset: 0x0000CD78
		// Note: this type is marked as 'beforefieldinit'.
		static SettingsControl()
		{
			SettingsControl.OpenEvent = EventManager.RegisterRoutedEvent("Open", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SettingsControl));
			SettingsControl.CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SettingsControl));
		}

		// Token: 0x040000EA RID: 234
		public static readonly DependencyProperty IsOpenedProperty = DependencyProperty.Register("IsOpened", typeof(bool), typeof(SettingsControl), new PropertyMetadata(false, new PropertyChangedCallback(SettingsControl.OnIsOpenedChanged)));

		// Token: 0x040000ED RID: 237
		private readonly Timer timer;
	}
}
