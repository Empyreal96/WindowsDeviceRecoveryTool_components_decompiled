using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000095 RID: 149
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class AppUpdateCheckingView : Grid
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x00013D3A File Offset: 0x00011F3A
		public AppUpdateCheckingView()
		{
			this.InitializeComponent();
		}
	}
}
