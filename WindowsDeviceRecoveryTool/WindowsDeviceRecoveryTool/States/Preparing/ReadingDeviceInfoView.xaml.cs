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
	// Token: 0x020000A7 RID: 167
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class ReadingDeviceInfoView : Grid
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x00016FC4 File Offset: 0x000151C4
		public ReadingDeviceInfoView()
		{
			this.InitializeComponent();
		}
	}
}
