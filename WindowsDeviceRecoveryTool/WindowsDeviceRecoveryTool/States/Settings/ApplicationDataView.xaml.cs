using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000AB RID: 171
	[Region(new string[]
	{
		"SettingsMainArea"
	})]
	[Export]
	public partial class ApplicationDataView : StackPanel
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x00018B7B File Offset: 0x00016D7B
		public ApplicationDataView()
		{
			this.InitializeComponent();
		}
	}
}
