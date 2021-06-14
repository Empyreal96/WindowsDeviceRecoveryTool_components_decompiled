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
	// Token: 0x020000C9 RID: 201
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class PackageIntegrityCheckView : Grid
	{
		// Token: 0x06000621 RID: 1569 RVA: 0x0001FD1C File Offset: 0x0001DF1C
		public PackageIntegrityCheckView()
		{
			this.InitializeComponent();
		}
	}
}
