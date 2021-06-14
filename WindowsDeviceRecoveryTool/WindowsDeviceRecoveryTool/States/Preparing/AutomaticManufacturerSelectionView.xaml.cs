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
	// Token: 0x0200008B RID: 139
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class AutomaticManufacturerSelectionView : Grid
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x00012095 File Offset: 0x00010295
		public AutomaticManufacturerSelectionView()
		{
			this.InitializeComponent();
		}
	}
}
