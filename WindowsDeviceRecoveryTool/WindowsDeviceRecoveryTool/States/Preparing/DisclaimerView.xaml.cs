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
	// Token: 0x020000A5 RID: 165
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class DisclaimerView : Grid
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x0001645C File Offset: 0x0001465C
		public DisclaimerView()
		{
			this.InitializeComponent();
		}
	}
}
