using System;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x02000045 RID: 69
	public class DialogManager
	{
		// Token: 0x0600026E RID: 622 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		private DialogManager()
		{
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
		public static DialogManager Instance
		{
			get
			{
				return DialogManager.Nested.NestedInstance;
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F0F7 File Offset: 0x0000D2F7
		public void SetShellWindow(Window shell)
		{
			this.shellWindow = shell;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F104 File Offset: 0x0000D304
		public string OpenFileDialog(string extension, string initialDirectory = null)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				DefaultExt = "." + extension.ToLower(CultureInfo.CurrentCulture),
				Filter = string.Format("{0} files (*.{1})|*.{1}", extension.ToUpper(CultureInfo.CurrentCulture), extension.ToLower(CultureInfo.CurrentCulture))
			};
			if (initialDirectory != null)
			{
				openFileDialog.InitialDirectory = initialDirectory;
			}
			string result;
			if (openFileDialog.ShowDialog() == true)
			{
				result = openFileDialog.FileName;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public string OpenDirectoryDialog()
		{
			return this.OpenDirectoryDialog(Environment.ExpandEnvironmentVariables(Settings.Default.ZipFilePath));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		public string OpenDirectoryDialog(string initPath)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				SelectedPath = initPath,
				Description = LocalizationManager.GetTranslation("PleaseSelectAFolder")
			};
			string result;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				result = folderBrowserDialog.SelectedPath;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x040000F9 RID: 249
		private Window shellWindow;

		// Token: 0x02000046 RID: 70
		private class Nested
		{
			// Token: 0x040000FA RID: 250
			internal static readonly DialogManager NestedInstance = new DialogManager();
		}
	}
}
