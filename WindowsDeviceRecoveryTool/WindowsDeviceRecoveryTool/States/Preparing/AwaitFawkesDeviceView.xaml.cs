using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x0200008D RID: 141
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AwaitFawkesDeviceView : Grid
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x00012AB8 File Offset: 0x00010CB8
		public AwaitFawkesDeviceView()
		{
			this.InitializeComponent();
		}
	}
}
