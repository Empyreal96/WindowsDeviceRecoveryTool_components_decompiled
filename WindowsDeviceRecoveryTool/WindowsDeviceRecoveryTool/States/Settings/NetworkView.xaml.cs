using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes;
using Microsoft.WindowsDeviceRecoveryTool.Controls.TextBoxes.Validation;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D6 RID: 214
	[Export]
	[Region(new string[]
	{
		"SettingsMainArea"
	})]
	public partial class NetworkView : StackPanel
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x00021FA9 File Offset: 0x000201A9
		public NetworkView()
		{
			this.InitializeComponent();
			this.capsLockTimer = (DispatcherTimer)base.FindResource("CapsLockTimer");
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00021FD1 File Offset: 0x000201D1
		private void CapsLockTimer_OnTick(object sender, EventArgs e)
		{
			this.CapsLockTextBlock.IsEnabled = (this.PasswordBox.IsKeyboardFocused && Keyboard.IsKeyToggled(Key.Capital));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00021FF7 File Offset: 0x000201F7
		private void NetworkView_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.capsLockTimer.Start();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00022006 File Offset: 0x00020206
		private void NetworkView_OnUnloaded(object sender, RoutedEventArgs e)
		{
			this.capsLockTimer.Stop();
		}

		// Token: 0x040002BF RID: 703
		private readonly DispatcherTimer capsLockTimer;
	}
}
