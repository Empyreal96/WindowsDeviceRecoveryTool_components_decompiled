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
	// Token: 0x020000C2 RID: 194
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AutomaticPackageSelectionView : Grid
	{
		// Token: 0x060005CE RID: 1486 RVA: 0x0001E2A3 File Offset: 0x0001C4A3
		public AutomaticPackageSelectionView()
		{
			this.InitializeComponent();
		}
	}
}
