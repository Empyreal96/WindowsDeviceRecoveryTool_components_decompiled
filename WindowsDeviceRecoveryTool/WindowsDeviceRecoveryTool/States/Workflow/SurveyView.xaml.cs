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
	// Token: 0x020000E0 RID: 224
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class SurveyView : Grid
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x000262AD File Offset: 0x000244AD
		public SurveyView()
		{
			this.InitializeComponent();
		}
	}
}
