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
	// Token: 0x020000CF RID: 207
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualPackageSelectionView : Grid
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		public ManualPackageSelectionView()
		{
			this.InitializeComponent();
		}
	}
}
