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
	// Token: 0x02000093 RID: 147
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class BatteryCheckingView : Grid
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x000136AC File Offset: 0x000118AC
		public BatteryCheckingView()
		{
			this.InitializeComponent();
		}
	}
}
