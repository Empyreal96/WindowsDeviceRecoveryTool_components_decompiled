using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000036 RID: 54
	public sealed partial class ExtendedMessageBox : Window
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x0000DA62 File Offset: 0x0000BC62
		public ExtendedMessageBox()
		{
			this.InitializeComponent();
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000DA74 File Offset: 0x0000BC74
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000DA96 File Offset: 0x0000BC96
		public string MessageBoxText
		{
			get
			{
				return (string)base.GetValue(ExtendedMessageBox.MessageBoxTextProperty);
			}
			set
			{
				base.SetValue(ExtendedMessageBox.MessageBoxTextProperty, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		public BitmapSource BoxIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000DABC File Offset: 0x0000BCBC
		public Visibility AdvanceVisibility
		{
			get
			{
				Visibility result;
				if (string.IsNullOrEmpty(this.MessageBoxAdvance))
				{
					result = Visibility.Collapsed;
				}
				else
				{
					result = Visibility.Visible;
				}
				return result;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public string MessageBoxAdvance
		{
			get
			{
				string result;
				if (this.AdvanceTextBox != null)
				{
					result = this.AdvanceTextBox.Text;
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			set
			{
				if (this.AdvanceTextBox != null)
				{
					this.AdvanceTextBox.Text = value;
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000DB48 File Offset: 0x0000BD48
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000DB6A File Offset: 0x0000BD6A
		public Style ButtonPanelStyle
		{
			get
			{
				return (Style)base.GetValue(ExtendedMessageBox.ButtonPanelStyleProperty);
			}
			set
			{
				base.SetValue(ExtendedMessageBox.ButtonPanelStyleProperty, value);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public static void Show(string messageBoxText, string caption, string messageBoxAdvance)
		{
			ExtendedMessageBox extendedMessageBox = new ExtendedMessageBox
			{
				Title = caption,
				MessageBoxText = messageBoxText,
				MessageBoxAdvance = messageBoxAdvance
			};
			if (Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				extendedMessageBox.FlowDirection = FlowDirection.RightToLeft;
			}
			else
			{
				extendedMessageBox.FlowDirection = FlowDirection.LeftToRight;
			}
			extendedMessageBox.ShowDialog();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DBE2 File Offset: 0x0000BDE2
		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000DBEC File Offset: 0x0000BDEC
		private void ExtendedMessageBoxLoaded(object sender, RoutedEventArgs e)
		{
			base.MaxHeight = base.Height;
			base.MinHeight = base.Height;
		}

		// Token: 0x040000D6 RID: 214
		public static readonly DependencyProperty MessageBoxTextProperty = DependencyProperty.Register("MessageBoxText", typeof(string), typeof(ExtendedMessageBox));

		// Token: 0x040000D7 RID: 215
		public static readonly DependencyProperty ButtonPanelStyleProperty = DependencyProperty.Register("ButtonPanelStyle", typeof(Style), typeof(ExtendedMessageBox));
	}
}
