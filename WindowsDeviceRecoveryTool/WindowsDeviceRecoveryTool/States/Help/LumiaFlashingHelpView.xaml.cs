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
	// Token: 0x02000084 RID: 132
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	[Export]
	public partial class LumiaFlashingHelpView : UserControl
	{
		// Token: 0x060003AA RID: 938 RVA: 0x000117E1 File Offset: 0x0000F9E1
		public LumiaFlashingHelpView()
		{
			this.InitializeComponent();
		}
	}
}
