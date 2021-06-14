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
	// Token: 0x0200008F RID: 143
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AwaitGenericDeviceView : Grid
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x00012DFB File Offset: 0x00010FFB
		public AwaitGenericDeviceView()
		{
			this.InitializeComponent();
		}
	}
}
