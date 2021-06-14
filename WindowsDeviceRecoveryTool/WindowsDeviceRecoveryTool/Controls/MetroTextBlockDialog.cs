using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200002A RID: 42
	public class MetroTextBlockDialog : MetroDialog
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public MetroTextBlockDialog()
		{
			base.YesButtonFocused = true;
			this.BodyMessagePanel.Visibility = Visibility.Collapsed;
			this.textBox = new TextBox
			{
				VerticalAlignment = VerticalAlignment.Center
			};
			this.textBox.GotKeyboardFocus += delegate(object s, KeyboardFocusChangedEventArgs e)
			{
				((TextBoxBase)s).SelectAll();
			};
			AutomationProperties.SetName(this.textBox, LocalizationManager.GetTranslation("FolderName"));
			base.Loaded += delegate(object param0, RoutedEventArgs param1)
			{
				this.textBox.Focus();
			};
			this.GridContent.Children.Add(this.textBox);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000A37C File Offset: 0x0000857C
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000A399 File Offset: 0x00008599
		public string InputText
		{
			get
			{
				return this.textBox.Text;
			}
			set
			{
				this.textBox.Text = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000A3AC File Offset: 0x000085AC
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000A3C4 File Offset: 0x000085C4
		public new string NoButtonText
		{
			get
			{
				return base.NoButtonText;
			}
			set
			{
				base.NoButtonText = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000A3D0 File Offset: 0x000085D0
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000A3E8 File Offset: 0x000085E8
		public new string YesButtonText
		{
			get
			{
				return base.YesButtonText;
			}
			set
			{
				base.YesButtonText = value;
			}
		}

		// Token: 0x040000A5 RID: 165
		private readonly TextBox textBox;
	}
}
