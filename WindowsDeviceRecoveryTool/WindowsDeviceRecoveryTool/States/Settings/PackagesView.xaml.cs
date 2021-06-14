using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000B0 RID: 176
	[Region(new string[]
	{
		"SettingsMainArea"
	})]
	[Export]
	public partial class PackagesView : StackPanel
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x0001AC9A File Offset: 0x00018E9A
		public PackagesView()
		{
			this.InitializeComponent();
		}
	}
}
