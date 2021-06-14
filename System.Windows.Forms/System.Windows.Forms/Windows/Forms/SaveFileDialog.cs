using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Prompts the user to select a location for saving a file. This class cannot be inherited.</summary>
	// Token: 0x02000341 RID: 833
	[Designer("System.Windows.Forms.Design.SaveFileDialogDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionSaveFileDialog")]
	public sealed class SaveFileDialog : FileDialog
	{
		/// <summary>Gets or sets a value indicating whether the dialog box prompts the user for permission to create a file if the user specifies a file that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box prompts the user before creating a file if the user specifies a file name that does not exist; <see langword="false" /> if the dialog box automatically creates the new file without prompting the user for permission. The default value is <see langword="false" />.</returns>
		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000EF10C File Offset: 0x000ED30C
		// (set) Token: 0x0600341F RID: 13343 RVA: 0x000EF119 File Offset: 0x000ED319
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("SaveFileDialogCreatePrompt")]
		public bool CreatePrompt
		{
			get
			{
				return base.GetOption(8192);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				base.SetOption(8192, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see langword="Save As" /> dialog box displays a warning if the user specifies a file name that already exists.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box prompts the user before overwriting an existing file if the user specifies a file name that already exists; <see langword="false" /> if the dialog box automatically overwrites the existing file without prompting the user for permission. The default value is <see langword="true" />.</returns>
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000EF131 File Offset: 0x000ED331
		// (set) Token: 0x06003421 RID: 13345 RVA: 0x000EF13A File Offset: 0x000ED33A
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("SaveFileDialogOverWritePrompt")]
		public bool OverwritePrompt
		{
			get
			{
				return base.GetOption(2);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				base.SetOption(2, value);
			}
		}

		/// <summary>Opens the file with read/write permission selected by the user.</summary>
		/// <returns>The read/write file selected by the user.</returns>
		// Token: 0x06003422 RID: 13346 RVA: 0x000EF150 File Offset: 0x000ED350
		public Stream OpenFile()
		{
			IntSecurity.FileDialogSaveFile.Demand();
			string text = base.FileNamesInternal[0];
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("FileName");
			}
			Stream result = null;
			new FileIOPermission(FileIOPermissionAccess.AllAccess, IntSecurity.UnsafeGetFullPath(text)).Assert();
			try
			{
				result = new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000EF1BC File Offset: 0x000ED3BC
		private bool PromptFileCreate(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.GetString("FileDialogCreatePrompt", new object[]
			{
				fileName
			}), base.DialogCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000EF1E1 File Offset: 0x000ED3E1
		private bool PromptFileOverwrite(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.GetString("FileDialogOverwritePrompt", new object[]
			{
				fileName
			}), base.DialogCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000EF208 File Offset: 0x000ED408
		internal override bool PromptUserIfAppropriate(string fileName)
		{
			return base.PromptUserIfAppropriate(fileName) && ((this.options & 2) == 0 || !FileDialog.FileExists(fileName) || base.UseVistaDialogInternal || this.PromptFileOverwrite(fileName)) && ((this.options & 8192) == 0 || FileDialog.FileExists(fileName) || this.PromptFileCreate(fileName));
		}

		/// <summary>Resets all dialog box options to their default values.</summary>
		// Token: 0x06003426 RID: 13350 RVA: 0x000EF267 File Offset: 0x000ED467
		public override void Reset()
		{
			base.Reset();
			base.SetOption(2, true);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000EF277 File Offset: 0x000ED477
		internal override void EnsureFileDialogPermission()
		{
			IntSecurity.FileDialogSaveFile.Demand();
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000EF284 File Offset: 0x000ED484
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			IntSecurity.FileDialogSaveFile.Demand();
			bool saveFileName = UnsafeNativeMethods.GetSaveFileName(ofn);
			if (!saveFileName)
			{
				int num = SafeNativeMethods.CommDlgExtendedError();
				if (num == 12290)
				{
					throw new InvalidOperationException(SR.GetString("FileDialogInvalidFileName", new object[]
					{
						base.FileName
					}));
				}
			}
			return saveFileName;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000EF2D4 File Offset: 0x000ED4D4
		internal override string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.IFileSaveDialog fileSaveDialog = (FileDialogNative.IFileSaveDialog)dialog;
			FileDialogNative.IShellItem item;
			dialog.GetResult(out item);
			return new string[]
			{
				FileDialog.GetFilePathFromShellItem(item)
			};
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000EF2FF File Offset: 0x000ED4FF
		internal override FileDialogNative.IFileDialog CreateVistaDialog()
		{
			return (FileDialogNative.NativeFileSaveDialog)new FileDialogNative.FileSaveDialogRCW();
		}
	}
}
