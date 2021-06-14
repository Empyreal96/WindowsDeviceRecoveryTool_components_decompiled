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
	// Token: 0x02000086 RID: 134
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	[Export]
	public partial class LumiaNormalHelpView : UserControl
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x000118A5 File Offset: 0x0000FAA5
		public LumiaNormalHelpView()
		{
			this.InitializeComponent();
		}
	}
}
