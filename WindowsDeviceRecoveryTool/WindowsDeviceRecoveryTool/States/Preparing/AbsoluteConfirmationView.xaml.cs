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
	// Token: 0x02000098 RID: 152
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class AbsoluteConfirmationView : Grid
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00014425 File Offset: 0x00012625
		public AbsoluteConfirmationView()
		{
			this.InitializeComponent();
		}
	}
}
