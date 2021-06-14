using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.WindowsDeviceRecoveryTool.Framework;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000AD RID: 173
	[Export]
	[Region(new string[]
	{
		"MainArea"
	})]
	public partial class FolderBrowsingView : Grid, IStyleConnector
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x00019860 File Offset: 0x00017A60
		public FolderBrowsingView()
		{
			this.InitializeComponent();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00019874 File Offset: 0x00017A74
		private void MetroBrowseDialogOnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			int num = e.Delta * -1;
			if (num < 0)
			{
				if (this.FolderScrollViewer.HorizontalOffset + (double)num > 0.0)
				{
					this.FolderScrollViewer.ScrollToHorizontalOffset(this.FolderScrollViewer.HorizontalOffset + (double)num);
				}
				else
				{
					this.FolderScrollViewer.ScrollToLeftEnd();
				}
			}
			else if (this.FolderScrollViewer.ExtentWidth > this.FolderScrollViewer.HorizontalOffset + (double)num)
			{
				this.FolderScrollViewer.ScrollToHorizontalOffset(this.FolderScrollViewer.HorizontalOffset + (double)num);
			}
			else
			{
				this.FolderScrollViewer.ScrollToRightEnd();
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000199E4 File Offset: 0x00017BE4
		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 4)
			{
				((WrapPanel)target).MouseWheel += this.MetroBrowseDialogOnMouseWheel;
			}
		}
	}
}
