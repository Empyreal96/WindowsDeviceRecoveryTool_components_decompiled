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
	// Token: 0x020000A3 RID: 163
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualHtcRestartView : Grid
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x00015F92 File Offset: 0x00014192
		public ManualHtcRestartView()
		{
			this.InitializeComponent();
		}
	}
}
