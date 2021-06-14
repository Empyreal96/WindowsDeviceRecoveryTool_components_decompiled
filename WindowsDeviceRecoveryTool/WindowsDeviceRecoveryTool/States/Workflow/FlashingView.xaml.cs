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
	// Token: 0x020000CD RID: 205
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class FlashingView : Grid
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x000207D4 File Offset: 0x0001E9D4
		public FlashingView()
		{
			this.InitializeComponent();
		}
	}
}
