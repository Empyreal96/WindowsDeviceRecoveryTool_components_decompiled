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
	/// <summary>Represents a common dialog box that allows a user to specify a filename for one or more files to open.</summary>
	// Token: 0x02000092 RID: 146
	public sealed class OpenFileDialog : FileDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.OpenFileDialog" /> class.</summary>
		// Token: 0x06000239 RID: 569 RVA: 0x000059B3 File Offset: 0x00003BB3
		[SecurityCritical]
		public OpenFileDialog()
		{
			this.Initialize();
		}

		/// <summary>Opens a read-only stream for the file that is selected by the user using <see cref="T:Microsoft.Win32.OpenFileDialog" />.</summary>
		/// <returns>A new <see cref="T:System.IO.Stream" /> that contains the selected file.</returns>
		/// <exception cref="T:System.InvalidOperationException">No files were selected in the dialog.</exception>
		// Token: 0x0600023A RID: 570 RVA: 0x000059C4 File Offset: 0x00003BC4
		[SecurityCritical]
		public Stream OpenFile()
		{
			SecurityHelper.DemandFileDialogOpenPermission();
			string text = null;
			string[] fileNamesInternal = base.FileNamesInternal;
			if (fileNamesInternal.Length != 0)
			{
				text = fileNamesInternal[0];
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
			}
			FileStream result = null;
			new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
			try
			{
				result = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		/// <summary>Creates an array that contains one read-only stream for each file selected by the user using <see cref="T:Microsoft.Win32.OpenFileDialog" />.</summary>
		/// <returns>An array of multiple new <see cref="T:System.IO.Stream" /> objects that contain the selected files.</returns>
		/// <exception cref="T:System.InvalidOperationException">No files were selected in the dialog.</exception>
		// Token: 0x0600023B RID: 571 RVA: 0x00005A30 File Offset: 0x00003C30
		[SecurityCritical]
		public Stream[] OpenFiles()
		{
			SecurityHelper.DemandFileDialogOpenPermission();
			string[] fileNamesInternal = base.FileNamesInternal;
			Stream[] array = new Stream[fileNamesInternal.Length];
			for (int i = 0; i < fileNamesInternal.Length; i++)
			{
				string text = fileNamesInternal[i];
				if (string.IsNullOrEmpty(text))
				{
					throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
				}
				FileStream fileStream = null;
				new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
				try
				{
					fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				array[i] = fileStream;
			}
			return array;
		}

		/// <summary>Resets all <see cref="T:Microsoft.Win32.OpenFileDialog" /> properties to their default values.</summary>
		// Token: 0x0600023C RID: 572 RVA: 0x00005AB4 File Offset: 0x00003CB4
		[SecurityCritical]
		public override void Reset()
		{
			SecurityHelper.DemandUnrestrictedFileIOPermission();
			base.Reset();
			this.Initialize();
		}

		/// <summary>Gets or sets an option indicating whether <see cref="T:Microsoft.Win32.OpenFileDialog" /> allows users to select multiple files.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selections are allowed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00005AC7 File Offset: 0x00003CC7
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public bool Multiselect
		{
			get
			{
				return base.GetOption(512);
			}
			[SecurityCritical]
			set
			{
				base.SetOption(512, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the read-only check box displayed by <see cref="T:Microsoft.Win32.OpenFileDialog" /> is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the checkbox is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00005AE2 File Offset: 0x00003CE2
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00005AEB File Offset: 0x00003CEB
		public bool ReadOnlyChecked
		{
			get
			{
				return base.GetOption(1);
			}
			[SecurityCritical]
			set
			{
				base.SetOption(1, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether <see cref="T:Microsoft.Win32.OpenFileDialog" /> contains a read-only check box.</summary>
		/// <returns>
		///     <see langword="true" /> if the checkbox is displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00005AF5 File Offset: 0x00003CF5
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00005B01 File Offset: 0x00003D01
		public bool ShowReadOnly
		{
			get
			{
				return !base.GetOption(4);
			}
			[SecurityCritical]
			set
			{
				base.SetOption(4, !value);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00005B10 File Offset: 0x00003D10
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void CheckPermissionsToShowDialog()
		{
			SecurityHelper.DemandFileDialogOpenPermission();
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			try
			{
				base.CheckPermissionsToShowDialog();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00005B4C File Offset: 0x00003D4C
		[SecurityCritical]
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			bool openFileName = UnsafeNativeMethods.GetOpenFileName(ofn);
			if (!openFileName)
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
			return openFileName;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00005BC8 File Offset: 0x00003DC8
		[SecurityCritical]
		internal override string[] ProcessVistaFiles(IFileDialog dialog)
		{
			IFileOpenDialog fileOpenDialog = (IFileOpenDialog)dialog;
			if (this.Multiselect)
			{
				IShellItemArray results = fileOpenDialog.GetResults();
				uint count = results.GetCount();
				string[] array = new string[count];
				for (uint num = 0U; num < count; num += 1U)
				{
					IShellItem itemAt = results.GetItemAt(num);
					array[(int)num] = itemAt.GetDisplayName((SIGDN)2147647488U);
				}
				return array;
			}
			IShellItem result = fileOpenDialog.GetResult();
			return new string[]
			{
				result.GetDisplayName((SIGDN)2147647488U)
			};
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00005C42 File Offset: 0x00003E42
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override IFileDialog CreateVistaDialog()
		{
			new SecurityPermission(PermissionState.Unrestricted).Assert();
			return (IFileDialog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00005C68 File Offset: 0x00003E68
		[SecurityCritical]
		private void Initialize()
		{
			base.SetOption(4096, true);
		}
	}
}
