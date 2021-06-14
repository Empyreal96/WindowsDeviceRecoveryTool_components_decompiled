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
	// Token: 0x02000082 RID: 130
	[Export]
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	public partial class LumiaEmergencyHelpView : UserControl
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x000116F7 File Offset: 0x0000F8F7
		public LumiaEmergencyHelpView()
		{
			this.InitializeComponent();
		}
	}
}
