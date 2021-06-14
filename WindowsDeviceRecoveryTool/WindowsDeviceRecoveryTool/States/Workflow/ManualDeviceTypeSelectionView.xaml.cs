using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000B7 RID: 183
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class ManualDeviceTypeSelectionView : Grid
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x0001BF75 File Offset: 0x0001A175
		public ManualDeviceTypeSelectionView()
		{
			this.InitializeComponent();
		}
	}
}
