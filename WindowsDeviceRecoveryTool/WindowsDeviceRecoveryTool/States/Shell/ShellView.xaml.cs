using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Shell
{
	// Token: 0x020000EA RID: 234
	[Export]
	public sealed partial class ShellView : ContentControl
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x00027C88 File Offset: 0x00025E88
		public ShellView()
		{
			this.InitializeComponent();
			this.animation = (Application.Current.Resources["StartAnimationStoryboard"] as Storyboard);
			this.focusDelayTimer = (DispatcherTimer)base.Resources["FocusDelayTimer"];
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00027CE0 File Offset: 0x00025EE0
		private void FocusDelayTimer_OnTick(object sender, EventArgs e)
		{
			this.focusDelayTimer.Stop();
			if (!this.GenericRoot.IsKeyboardFocusWithin || this.BackButton.IsKeyboardFocused)
			{
				this.GenericRoot.Focus();
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00027D2B File Offset: 0x00025F2B
		private void ShellView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.SettingsControl.SetCurrentValue(SettingsControl.IsOpenedProperty, !this.SettingsControl.IsOpened);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00027D54 File Offset: 0x00025F54
		private void ShellView_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this.SettingsControl.IsOpened)
			{
				this.SettingsControl.SetCurrentValue(SettingsControl.IsOpenedProperty, false);
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00027D8D File Offset: 0x00025F8D
		private void MainContent_OnContentChanged(object sender, RoutedEventArgs e)
		{
			this.animation.Begin(this.GenericRoot);
			Keyboard.ClearFocus();
			this.focusDelayTimer.Start();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00027DB4 File Offset: 0x00025FB4
		private void ShellView_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			this.SettingsControl.SetCurrentValue(SettingsControl.IsOpenedProperty, false);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00027DCE File Offset: 0x00025FCE
		private void ShellView_OnUnloaded(object sender, RoutedEventArgs e)
		{
			this.focusDelayTimer.Stop();
		}

		// Token: 0x0400035C RID: 860
		private readonly Storyboard animation;

		// Token: 0x0400035D RID: 861
		private readonly DispatcherTimer focusDelayTimer;
	}
}
