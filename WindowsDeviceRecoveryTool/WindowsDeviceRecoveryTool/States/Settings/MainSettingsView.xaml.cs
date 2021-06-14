using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D4 RID: 212
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class MainSettingsView : Grid
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00021C0C File Offset: 0x0001FE0C
		public MainSettingsView()
		{
			this.InitializeComponent();
			base.Loaded += delegate(object sender, RoutedEventArgs e)
			{
				this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			};
		}
	}
}
