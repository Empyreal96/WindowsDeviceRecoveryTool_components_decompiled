using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000029 RID: 41
	public abstract partial class MetroDialog : Window
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00009E50 File Offset: 0x00008050
		protected MetroDialog()
		{
			Window mainWindow = Application.Current.MainWindow;
			base.Owner = mainWindow;
			this.InitializeComponent();
			base.Width = mainWindow.ActualWidth;
			base.Height = mainWindow.ActualHeight;
			this.dialogResult = false;
			if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				this.MetroDialogWindow.FlowDirection = FlowDirection.RightToLeft;
			}
			else
			{
				this.MetroDialogWindow.FlowDirection = FlowDirection.LeftToRight;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00009EDC File Offset: 0x000080DC
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00009EF3 File Offset: 0x000080F3
		public bool Warning { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00009EFC File Offset: 0x000080FC
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00009F19 File Offset: 0x00008119
		public string Message
		{
			get
			{
				return this.BodyMessage.Text;
			}
			set
			{
				this.BodyMessage.Text = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00009F2C File Offset: 0x0000812C
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00009F49 File Offset: 0x00008149
		public string MessageTitle
		{
			get
			{
				return this.TitleMessage.Text;
			}
			set
			{
				this.TitleMessage.Text = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00009F5C File Offset: 0x0000815C
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00009F7E File Offset: 0x0000817E
		protected string NoButtonText
		{
			get
			{
				return this.ButtonNo.Content as string;
			}
			set
			{
				this.ButtonNo.Content = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00009F90 File Offset: 0x00008190
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00009FB2 File Offset: 0x000081B2
		protected string YesButtonText
		{
			get
			{
				return this.ButtonYes.Content as string;
			}
			set
			{
				this.ButtonYes.Content = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00009FC4 File Offset: 0x000081C4
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00009FE8 File Offset: 0x000081E8
		protected bool YesButtonFocused
		{
			get
			{
				return object.Equals(FocusManager.GetFocusedElement(this), this.ButtonYes);
			}
			set
			{
				if (value)
				{
					FocusManager.SetFocusedElement(this, this.ButtonYes);
				}
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000A010 File Offset: 0x00008210
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			Window mainWindow = Application.Current.MainWindow;
			mainWindow.Left = base.Left;
			mainWindow.Top = base.Top;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000A04C File Offset: 0x0000824C
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000A063 File Offset: 0x00008263
		public RoutedEventHandler YesButtonClicked { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000A06C File Offset: 0x0000826C
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000A083 File Offset: 0x00008283
		public RoutedEventHandler NoButtonClicked { get; set; }

		// Token: 0x0600014D RID: 333 RVA: 0x0000A08C File Offset: 0x0000828C
		private void OnCloseDialog()
		{
			this.FadeoutBackground();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A098 File Offset: 0x00008298
		private void FadeoutBackground()
		{
			Storyboard storyboard = (Storyboard)base.FindResource("FadeoutBackground");
			storyboard.Completed += this.FadeoutBackgroundCompleted;
			storyboard.Begin(this);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A0D2 File Offset: 0x000082D2
		private void FadeoutBackgroundCompleted(object sender, EventArgs e)
		{
			base.DialogResult = new bool?(this.dialogResult);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A0E8 File Offset: 0x000082E8
		private void OnNoButtonClicked(object sender, RoutedEventArgs e)
		{
			RoutedEventHandler noButtonClicked = this.NoButtonClicked;
			if (noButtonClicked != null)
			{
				noButtonClicked(this, e);
			}
			if (!e.Handled)
			{
				this.dialogResult = false;
				this.OnCloseDialog();
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A12C File Offset: 0x0000832C
		private void OnYesButtonClicked(object sender, RoutedEventArgs e)
		{
			RoutedEventHandler yesButtonClicked = this.YesButtonClicked;
			if (yesButtonClicked != null)
			{
				yesButtonClicked(this, e);
			}
			if (!e.Handled)
			{
				this.dialogResult = true;
				this.OnCloseDialog();
			}
		}

		// Token: 0x04000097 RID: 151
		private bool dialogResult;
	}
}
