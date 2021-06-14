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
	// Token: 0x0200007A RID: 122
	[Export]
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	public partial class HtcBootloaderHelpView : UserControl
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0001132A File Offset: 0x0000F52A
		public HtcBootloaderHelpView()
		{
			this.InitializeComponent();
		}
	}
}
