using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Displays a standard dialog box that prompts the user to open a file. This class cannot be inherited.</summary>
	// Token: 0x02000301 RID: 769
	[SRDescription("DescriptionOpenFileDialog")]
	public sealed class OpenFileDialog : FileDialog
	{
		/// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist. </summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays a warning when the user specifies a file name that does not exist; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000D8D27 File Offset: 0x000D6F27
		// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x000D8D2F File Offset: 0x000D6F2F
		[DefaultValue(true)]
		[SRDescription("OFDcheckFileExistsDescr")]
		public override bool CheckFileExists
		{
			get
			{
				return base.CheckFileExists;
			}
			set
			{
				base.CheckFileExists = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box allows multiple files to be selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box allows multiple files to be selected together or concurrently; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x000D8D38 File Offset: 0x000D6F38
		// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x000D8D45 File Offset: 0x000D6F45
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("OFDmultiSelectDescr")]
		public bool Multiselect
		{
			get
			{
				return base.GetOption(512);
			}
			set
			{
				base.SetOption(512, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the read-only check box is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the read-only check box is selected; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x000D8D53 File Offset: 0x000D6F53
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000D8D5C File Offset: 0x000D6F5C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("OFDreadOnlyCheckedDescr")]
		public bool ReadOnlyChecked
		{
			get
			{
				return base.GetOption(1);
			}
			set
			{
				base.SetOption(1, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box contains a read-only check box. </summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box contains a read-only check box; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x000D8D66 File Offset: 0x000D6F66
		// (set) Token: 0x06002EAC RID: 11948 RVA: 0x000D8D72 File Offset: 0x000D6F72
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("OFDshowReadOnlyDescr")]
		public bool ShowReadOnly
		{
			get
			{
				return !base.GetOption(4);
			}
			set
			{
				base.SetOption(4, !value);
			}
		}

		/// <summary>Opens the file selected by the user, with read-only permission. The file is specified by the <see cref="P:System.Windows.Forms.FileDialog.FileName" /> property. </summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> that specifies the read-only file selected by the user.</returns>
		/// <exception cref="T:System.ArgumentNullException">The file name is <see langword="null" />. </exception>
		// Token: 0x06002EAD RID: 11949 RVA: 0x000D8D80 File Offset: 0x000D6F80
		public Stream OpenFile()
		{
			IntSecurity.FileDialogOpenFile.Demand();
			string text = base.FileNamesInternal[0];
			if (text == null || text.Length == 0)
			{
				throw new ArgumentNullException("FileName");
			}
			Stream result = null;
			new FileIOPermission(FileIOPermissionAccess.Read, IntSecurity.UnsafeGetFullPath(text)).Assert();
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

		/// <summary>Resets all properties to their default values. </summary>
		// Token: 0x06002EAE RID: 11950 RVA: 0x000D8DEC File Offset: 0x000D6FEC
		public override void Reset()
		{
			base.Reset();
			base.SetOption(4096, true);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000D8E00 File Offset: 0x000D7000
		internal override void EnsureFileDialogPermission()
		{
			IntSecurity.FileDialogOpenFile.Demand();
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000D8E0C File Offset: 0x000D700C
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			IntSecurity.FileDialogOpenFile.Demand();
			bool openFileName = UnsafeNativeMethods.GetOpenFileName(ofn);
			if (!openFileName)
			{
				switch (SafeNativeMethods.CommDlgExtendedError())
				{
				case 12289:
					throw new InvalidOperationException(SR.GetString("FileDialogSubLassFailure"));
				case 12290:
					throw new InvalidOperationException(SR.GetString("FileDialogInvalidFileName", new object[]
					{
						base.FileName
					}));
				case 12291:
					throw new InvalidOperationException(SR.GetString("FileDialogBufferTooSmall"));
				}
			}
			return openFileName;
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000D8E90 File Offset: 0x000D7090
		internal override string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.IFileOpenDialog fileOpenDialog = (FileDialogNative.IFileOpenDialog)dialog;
			if (this.Multiselect)
			{
				FileDialogNative.IShellItemArray shellItemArray;
				fileOpenDialog.GetResults(out shellItemArray);
				uint num;
				shellItemArray.GetCount(out num);
				string[] array = new string[num];
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					FileDialogNative.IShellItem item;
					shellItemArray.GetItemAt(num2, out item);
					array[(int)num2] = FileDialog.GetFilePathFromShellItem(item);
				}
				return array;
			}
			FileDialogNative.IShellItem item2;
			fileOpenDialog.GetResult(out item2);
			return new string[]
			{
				FileDialog.GetFilePathFromShellItem(item2)
			};
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000D8F02 File Offset: 0x000D7102
		internal override FileDialogNative.IFileDialog CreateVistaDialog()
		{
			return (FileDialogNative.NativeFileOpenDialog)new FileDialogNative.FileOpenDialogRCW();
		}

		/// <summary>Gets the file name and extension for the file selected in the dialog box. The file name does not include the path.</summary>
		/// <returns>The file name and extension for the file selected in the dialog box. The file name does not include the path. The default value is an empty string.</returns>
		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000D8F10 File Offset: 0x000D7110
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SafeFileName
		{
			get
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string fileName = base.FileName;
				CodeAccessPermission.RevertAssert();
				if (string.IsNullOrEmpty(fileName))
				{
					return "";
				}
				return OpenFileDialog.RemoveSensitivePathInformation(fileName);
			}
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000D8F4A File Offset: 0x000D714A
		private static string RemoveSensitivePathInformation(string fullPath)
		{
			return Path.GetFileName(fullPath);
		}

		/// <summary>Gets an array of file names and extensions for all the selected files in the dialog box. The file names do not include the path.</summary>
		/// <returns>An array of file names and extensions for all the selected files in the dialog box. The file names do not include the path. If no files are selected, an empty array is returned.</returns>
		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000D8F54 File Offset: 0x000D7154
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] SafeFileNames
		{
			get
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string[] fileNames = base.FileNames;
				CodeAccessPermission.RevertAssert();
				if (fileNames == null || fileNames.Length == 0)
				{
					return new string[0];
				}
				string[] array = new string[fileNames.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = OpenFileDialog.RemoveSensitivePathInformation(fileNames[i]);
				}
				return array;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000D8FA9 File Offset: 0x000D71A9
		internal override bool SettingsSupportVistaDialog
		{
			get
			{
				return base.SettingsSupportVistaDialog && !this.ShowReadOnly;
			}
		}
	}
}
