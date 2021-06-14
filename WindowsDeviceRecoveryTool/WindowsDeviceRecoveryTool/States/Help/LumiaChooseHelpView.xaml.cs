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
	// Token: 0x02000081 RID: 129
	[Region(new string[]
	{
		"HelpMainArea"
	})]
	[Export]
	public partial class LumiaChooseHelpView : UserControl
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0001162E File Offset: 0x0000F82E
		public LumiaChooseHelpView()
		{
			this.InitializeComponent();
		}
	}
}
