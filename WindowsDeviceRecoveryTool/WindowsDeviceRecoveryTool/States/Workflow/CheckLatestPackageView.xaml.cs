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
	// Token: 0x020000B9 RID: 185
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class CheckLatestPackageView : Grid
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x0001C040 File Offset: 0x0001A240
		public CheckLatestPackageView()
		{
			this.InitializeComponent();
		}
	}
}
