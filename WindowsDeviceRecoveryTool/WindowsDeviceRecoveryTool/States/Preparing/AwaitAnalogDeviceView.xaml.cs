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
	// Token: 0x0200009A RID: 154
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AwaitAnalogDeviceView : Grid
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x00014672 File Offset: 0x00012872
		public AwaitAnalogDeviceView()
		{
			this.InitializeComponent();
		}
	}
}
