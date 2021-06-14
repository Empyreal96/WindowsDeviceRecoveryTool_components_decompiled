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
	// Token: 0x0200007C RID: 124
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	[Export]
	public partial class HtcChooseHelpView : UserControl
	{
		// Token: 0x06000393 RID: 915 RVA: 0x00011403 File Offset: 0x0000F603
		public HtcChooseHelpView()
		{
			this.InitializeComponent();
		}
	}
}
