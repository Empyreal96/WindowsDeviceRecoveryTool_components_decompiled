using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace Microsoft.Win32
{
	/// <summary>Represents a common dialog that allows the user to specify a filename to save a file as. <see cref="T:Microsoft.Win32.SaveFileDialog" /> cannot be used by an application that is executing under partial trust.</summary>
	// Token: 0x02000093 RID: 147
	public sealed class SaveFileDialog : FileDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SaveFileDialog" /> class.</summary>
		// Token: 0x06000248 RID: 584 RVA: 0x00005C76 File Offset: 0x00003E76
		[SecurityCritical]
		public SaveFileDialog()
		{
			this.Initialize();
		}

		/// <summary>Creates a read-write file stream for the filename selected by the user using <see cref="T:Microsoft.Win32.SaveFileDialog" />.</summary>
		/// <returns>A new <see cref="T:System.IO.Stream" /> that contains the selected file.</returns>
		/// <exception cref="T:System.InvalidOperationException">No files were selected in the dialog.</exception>
		// Token: 0x06000249 RID: 585 RVA: 0x00005C84 File Offset: 0x00003E84
		[SecurityCritical]
		public Stream OpenFile()
		{
			SecurityHelper.DemandUIWindowPermission();
			string text = (base.FileNamesInternal.Length != 0) ? base.FileNamesInternal[0] : null;
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, text).Assert();
			return new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
		}

		/// <summary>Resets all <see cref="T:Microsoft.Win32.SaveFileDialog" /> properties to their default values.</summary>
		// Token: 0x0600024A RID: 586 RVA: 0x00005CD7 File Offset: 0x00003ED7
		[SecurityCritical]
		public override void Reset()
		{
			SecurityHelper.DemandUIWindowPermission();
			base.Reset();
			this.Initialize();
		}

		/// <summary>Gets or sets a value indicating whether <see cref="T:Microsoft.Win32.SaveFileDialog" /> prompts the user for permission to create a file if the user specifies a file that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if dialog should prompt prior to saving to a filename that did not previously exist; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00005CEA File Offset: 0x00003EEA
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00005CF7 File Offset: 0x00003EF7
		public bool CreatePrompt
		{
			get
			{
				return base.GetOption(8192);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				base.SetOption(8192, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether <see cref="T:Microsoft.Win32.SaveFileDialog" /> displays a warning if the user specifies the name of a file that already exists.</summary>
		/// <returns>
		///     <see langword="true" /> if dialog should prompt prior to saving over a filename that previously existed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00005D0A File Offset: 0x00003F0A
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00005D13 File Offset: 0x00003F13
		public bool OverwritePrompt
		{
			get
			{
				return base.GetOption(2);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				base.SetOption(2, value);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00005D24 File Offset: 0x00003F24
		[SecurityCritical]
		internal override bool PromptUserIfAppropriate(string fileName)
		{
			if (!base.PromptUserIfAppropriate(fileName))
			{
				return false;
			}
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			bool flag;
			try
			{
				flag = File.Exists(Path.GetFullPath(fileName));
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return (!this.CreatePrompt || flag || this.PromptFileCreate(fileName)) && (!this.OverwritePrompt || !flag || this.PromptFileOverwrite(fileName));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00005D98 File Offset: 0x00003F98
		[SecurityCritical]
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			bool saveFileName = UnsafeNativeMethods.GetSaveFileName(ofn);
			if (!saveFileName)
			{
				switch (UnsafeNativeMethods.CommDlgExtendedError())
				{
				case 12289:
					throw new InvalidOperationException(SR.Get("FileDialogSubClassFailure"));
				case 12290:
					throw new InvalidOperationException(SR.Get("FileDialogInvalidFileName", new object[]
					{
						base.SafeFileName
					}));
				case 12291:
					throw new InvalidOperationException(SR.Get("FileDialogBufferTooSmall"));
				}
			}
			return saveFileName;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00005E14 File Offset: 0x00004014
		[SecurityCritical]
		internal override string[] ProcessVistaFiles(IFileDialog dialog)
		{
			IShellItem result = dialog.GetResult();
			return new string[]
			{
				result.GetDisplayName((SIGDN)2147647488U)
			};
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00005E3C File Offset: 0x0000403C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override IFileDialog CreateVistaDialog()
		{
			SecurityHelper.DemandUIWindowPermission();
			new SecurityPermission(PermissionState.Unrestricted).Assert();
			return (IFileDialog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")));
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00005E67 File Offset: 0x00004067
		[SecurityCritical]
		private void Initialize()
		{
			base.SetOption(2, true);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00005E71 File Offset: 0x00004071
		[SecurityCritical]
		private bool PromptFileCreate(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.Get("FileDialogCreatePrompt", new object[]
			{
				fileName
			}), MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00005E90 File Offset: 0x00004090
		[SecurityCritical]
		private bool PromptFileOverwrite(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.Get("FileDialogOverwritePrompt", new object[]
			{
				fileName
			}), MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
		}
	}
}
