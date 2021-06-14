using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool
{
	// Token: 0x0200004B RID: 75
	[Export("ShellWindow")]
	public sealed partial class MainWindow : Window
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000F658 File Offset: 0x0000D858
		public MainWindow()
		{
			this.InitializeComponent();
			LocalizationManager.Instance().LanguageChanged += this.OnLanguageChanged;
			this.OnLanguageChanged(this, null);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000F68C File Offset: 0x0000D88C
		private void OnLanguageChanged(object sender, EventArgs e)
		{
			FlowDirection flowDirection = LocalizationManager.Instance().CurrentLanguage.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
			base.SetCurrentValue(FrameworkElement.FlowDirectionProperty, flowDirection);
		}
	}
}
