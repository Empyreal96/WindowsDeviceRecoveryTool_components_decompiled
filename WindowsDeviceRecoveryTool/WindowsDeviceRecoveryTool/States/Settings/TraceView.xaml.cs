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
	// Token: 0x020000DB RID: 219
	[Export]
	[Region(new string[]
	{
		"SettingsMainArea"
	})]
	public partial class TraceView : StackPanel
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x0002393F File Offset: 0x00021B3F
		public TraceView()
		{
			this.InitializeComponent();
		}
	}
}
