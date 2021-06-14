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
	// Token: 0x02000090 RID: 144
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class AwaitHtcView : Grid
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x00012E53 File Offset: 0x00011053
		public AwaitHtcView()
		{
			this.InitializeComponent();
		}
	}
}
