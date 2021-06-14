using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Error
{
	// Token: 0x020000BE RID: 190
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public sealed partial class ErrorView : Grid
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0001DA6F File Offset: 0x0001BC6F
		public ErrorView()
		{
			this.InitializeComponent();
		}
	}
}
