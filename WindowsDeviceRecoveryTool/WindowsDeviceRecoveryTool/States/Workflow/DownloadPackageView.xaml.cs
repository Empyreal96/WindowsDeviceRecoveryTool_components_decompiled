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
	// Token: 0x020000C7 RID: 199
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class DownloadPackageView : Grid
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x0001F6AC File Offset: 0x0001D8AC
		public DownloadPackageView()
		{
			this.InitializeComponent();
		}
	}
}
