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
	// Token: 0x0200007E RID: 126
	[Export]
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	public partial class HtcNormalHelpView : UserControl
	{
		// Token: 0x0600039A RID: 922 RVA: 0x00011557 File Offset: 0x0000F757
		public HtcNormalHelpView()
		{
			this.InitializeComponent();
		}
	}
}
