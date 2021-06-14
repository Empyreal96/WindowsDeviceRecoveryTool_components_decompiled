using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000088 RID: 136
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class MainHelpView : ScrollViewer
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0001193D File Offset: 0x0000FB3D
		public MainHelpView()
		{
			this.InitializeComponent();
		}
	}
}
