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
	// Token: 0x0200009D RID: 157
	[Region(new string[]
	{
		"MainArea"
	})]
	[Export]
	public partial class DownloadAppUpdateView : Grid
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x00014B64 File Offset: 0x00012D64
		public DownloadAppUpdateView()
		{
			this.InitializeComponent();
		}
	}
}
