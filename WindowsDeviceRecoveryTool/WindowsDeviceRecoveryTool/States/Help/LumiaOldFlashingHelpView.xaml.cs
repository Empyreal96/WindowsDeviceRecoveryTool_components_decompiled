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
	// Token: 0x02000075 RID: 117
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	[Export]
	public partial class LumiaOldFlashingHelpView : UserControl
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00010A71 File Offset: 0x0000EC71
		public LumiaOldFlashingHelpView()
		{
			this.InitializeComponent();
		}
	}
}
