using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000B4 RID: 180
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class DownloadEmergencyPackageView : Grid
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x0001B460 File Offset: 0x00019660
		public DownloadEmergencyPackageView()
		{
			this.InitializeComponent();
		}
	}
}
