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
	// Token: 0x020000E2 RID: 226
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class UnsupportedDeviceView : Grid
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x00026AF6 File Offset: 0x00024CF6
		public UnsupportedDeviceView()
		{
			this.InitializeComponent();
		}
	}
}
