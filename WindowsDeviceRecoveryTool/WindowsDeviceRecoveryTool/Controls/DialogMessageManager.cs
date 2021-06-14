using System;
using System.Windows;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000032 RID: 50
	public class DialogMessageManager
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000B057 File Offset: 0x00009257
		public DialogMessageManager()
		{
			this.ButtonLabel = LocalizationManager.GetTranslation("Close");
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000B074 File Offset: 0x00009274
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x0000B08B File Offset: 0x0000928B
		public Action NoButtonAction { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000B094 File Offset: 0x00009294
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000B0AB File Offset: 0x000092AB
		public Action YesButtonAction { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000B0B4 File Offset: 0x000092B4
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000B0CB File Offset: 0x000092CB
		public string NoButtonLabel { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000B0D4 File Offset: 0x000092D4
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000B0EB File Offset: 0x000092EB
		public string YesButtonLabel { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000B0F4 File Offset: 0x000092F4
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000B10B File Offset: 0x0000930B
		public string ButtonLabel { get; set; }

		// Token: 0x060001AF RID: 431 RVA: 0x0000B2B4 File Offset: 0x000094B4
		public void ShowInfoDialog(object data, bool stretchContent = false, string message = null, string title = null)
		{
			AppDispatcher.Execute(delegate
			{
				MetroInformationDialog metroInformationDialog = new MetroInformationDialog
				{
					Message = string.Empty,
					ButtonText = this.ButtonLabel,
					MessageTitle = "Information"
				};
				this.activeDialog = metroInformationDialog;
				if (!string.IsNullOrEmpty(message))
				{
					metroInformationDialog.Message = message;
				}
				if (!string.IsNullOrEmpty(title))
				{
					metroInformationDialog.MessageTitle = title;
				}
				if (this.YesButtonAction != null)
				{
					metroInformationDialog.ButtonYes.Visibility = Visibility.Visible;
					MetroInformationDialog metroInformationDialog2 = metroInformationDialog;
					metroInformationDialog2.YesButtonClicked = (RoutedEventHandler)Delegate.Combine(metroInformationDialog2.YesButtonClicked, new RoutedEventHandler(delegate(object o, RoutedEventArgs a)
					{
						a.Handled = true;
						this.NoButtonAction();
					}));
					metroInformationDialog.ButtonYes.Content = this.YesButtonLabel;
				}
				if (this.NoButtonAction != null)
				{
					metroInformationDialog.ButtonNo.Visibility = Visibility.Visible;
					MetroInformationDialog metroInformationDialog3 = metroInformationDialog;
					metroInformationDialog3.NoButtonClicked = (RoutedEventHandler)Delegate.Combine(metroInformationDialog3.NoButtonClicked, new RoutedEventHandler(delegate(object o, RoutedEventArgs a)
					{
						a.Handled = true;
						this.NoButtonAction();
					}));
					metroInformationDialog.ButtonNo.Content = this.NoButtonLabel;
				}
				metroInformationDialog.ShowDialog();
			}, true);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B354 File Offset: 0x00009554
		public void ShowInfoDialog(string message, string title = null)
		{
			string titleMessage = (!string.IsNullOrEmpty(title)) ? title : "Information";
			AppDispatcher.Execute(delegate
			{
				MetroInformationDialog metroInformationDialog = new MetroInformationDialog
				{
					MessageTitle = titleMessage,
					Message = message,
					ButtonText = this.ButtonLabel
				};
				this.activeDialog = metroInformationDialog;
				metroInformationDialog.ShowDialog();
			}, false);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000B40C File Offset: 0x0000960C
		public bool? ShowQuestionDialog(string message, string title = null, bool defaultButtonLabels = true)
		{
			bool? result = null;
			AppDispatcher.Execute(delegate
			{
				string messageTitle = (!string.IsNullOrEmpty(title)) ? title : LocalizationManager.GetTranslation("AreYouSure");
				MetroQuestionDialog confirmDialog = new MetroQuestionDialog
				{
					Message = message,
					MessageTitle = messageTitle
				};
				result = this.ShowQuestionDialog(confirmDialog);
			}, true);
			return result;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B49A File Offset: 0x0000969A
		public void CloseActiveDialog()
		{
			AppDispatcher.Execute(delegate
			{
				if (this.activeDialog != null && this.activeDialog.IsActive)
				{
					this.activeDialog.Close();
				}
			}, false);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B4B0 File Offset: 0x000096B0
		private bool? ShowQuestionDialog(MetroQuestionDialog confirmDialog)
		{
			this.activeDialog = confirmDialog;
			if (!string.IsNullOrEmpty(this.NoButtonLabel))
			{
				confirmDialog.NoButtonText = this.NoButtonLabel;
			}
			if (!string.IsNullOrEmpty(this.YesButtonLabel))
			{
				confirmDialog.YesButtonText = this.YesButtonLabel;
			}
			return confirmDialog.ShowDialog();
		}

		// Token: 0x040000C6 RID: 198
		private MetroDialog activeDialog;
	}
}
