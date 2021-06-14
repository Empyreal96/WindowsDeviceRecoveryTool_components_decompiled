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
	// Token: 0x020000B8 RID: 184
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class RebootHtcView : Grid
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x0001BFCB File Offset: 0x0001A1CB
		public RebootHtcView()
		{
			this.InitializeComponent();
		}
	}
}
