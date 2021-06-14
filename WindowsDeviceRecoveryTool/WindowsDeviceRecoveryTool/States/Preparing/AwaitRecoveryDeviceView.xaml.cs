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
	// Token: 0x020000C4 RID: 196
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AwaitRecoveryDeviceView : Grid
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0001EBD5 File Offset: 0x0001CDD5
		public AwaitRecoveryDeviceView()
		{
			this.InitializeComponent();
		}
	}
}
