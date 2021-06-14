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
	// Token: 0x020000D2 RID: 210
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class SummaryView : Grid
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x00021749 File Offset: 0x0001F949
		public SummaryView()
		{
			this.InitializeComponent();
		}
	}
}
