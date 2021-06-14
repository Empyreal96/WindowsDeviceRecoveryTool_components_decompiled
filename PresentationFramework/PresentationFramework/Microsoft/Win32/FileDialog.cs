using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace Microsoft.Win32
{
	/// <summary>An abstract base class that encapsulates functionality that is common to file dialogs, including <see cref="T:Microsoft.Win32.OpenFileDialog" /> and <see cref="T:Microsoft.Win32.SaveFileDialog" />.</summary>
	// Token: 0x0200008F RID: 143
	public abstract class FileDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.FileDialog" /> class.</summary>
		// Token: 0x060001E4 RID: 484 RVA: 0x000047D1 File Offset: 0x000029D1
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected FileDialog()
		{
			this.Initialize();
		}

		/// <summary>Sets all properties of a file dialog back to their initial values.</summary>
		// Token: 0x060001E5 RID: 485 RVA: 0x000047DF File Offset: 0x000029DF
		[SecurityCritical]
		public override void Reset()
		{
			SecurityHelper.DemandUnrestrictedFileIOPermission();
			this.Initialize();
		}

		/// <summary>Returns a string that represents a file dialog.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of <see cref="T:Microsoft.Win32.FileDialog" /> that contains the full pathname for any files selected from either <see cref="T:Microsoft.Win32.OpenFileDialog" />, <see cref="T:Microsoft.Win32.SaveFileDialog" />.</returns>
		// Token: 0x060001E6 RID: 486 RVA: 0x000047EC File Offset: 0x000029EC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString() + ": Title: " + this.Title + ", FileName: ");
			stringBuilder.Append(this.FileName);
			return stringBuilder.ToString();
		}

		/// <summary>Gets or sets a value indicating whether a file dialog automatically adds an extension to a file name if the user omits an extension.</summary>
		/// <returns>
		///     <see langword="true" /> if extensions are added; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000482D File Offset: 0x00002A2D
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000483A File Offset: 0x00002A3A
		public bool AddExtension
		{
			get
			{
				return this.GetOption(int.MinValue);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(int.MinValue, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether a file dialog displays a warning if the user specifies a file name that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if warnings are displayed; otherwise, <see langword="false" />. The default in this base class is <see langword="false" />.</returns>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000484D File Offset: 0x00002A4D
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000485A File Offset: 0x00002A5A
		public virtual bool CheckFileExists
		{
			get
			{
				return this.GetOption(4096);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(4096, value);
			}
		}

		/// <summary>Gets or sets a value that specifies whether warnings are displayed if the user types invalid paths and file names.</summary>
		/// <returns>
		///     <see langword="true" /> if warnings are displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000486D File Offset: 0x00002A6D
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000487A File Offset: 0x00002A7A
		public bool CheckPathExists
		{
			get
			{
				return this.GetOption(2048);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(2048, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the default extension string to use to filter the list of files that are displayed.</summary>
		/// <returns>The default extension string. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000488D File Offset: 0x00002A8D
		// (set) Token: 0x060001EE RID: 494 RVA: 0x000048A3 File Offset: 0x00002AA3
		public string DefaultExt
		{
			get
			{
				if (this._defaultExtension != null)
				{
					return this._defaultExtension;
				}
				return string.Empty;
			}
			set
			{
				if (value != null)
				{
					if (value.StartsWith(".", StringComparison.Ordinal))
					{
						value = value.Substring(1);
					}
					else if (value.Length == 0)
					{
						value = null;
					}
				}
				this._defaultExtension = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether a file dialog returns either the location of the file referenced by a shortcut or the location of the shortcut file (.lnk).</summary>
		/// <returns>
		///     <see langword="true" /> to return the location referenced; <see langword="false" /> to return the shortcut location. The default is <see langword="false" />.</returns>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000048D3 File Offset: 0x00002AD3
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x000048E3 File Offset: 0x00002AE3
		public bool DereferenceLinks
		{
			get
			{
				return !this.GetOption(1048576);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(1048576, !value);
			}
		}

		/// <summary>Gets a string that only contains the file name for the selected file.</summary>
		/// <returns>A <see cref="T:System.String" /> that only contains the file name for the selected file. The default is <see cref="F:System.String.Empty" />, which is also the value when either no file is selected or a directory is selected.</returns>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000048FC File Offset: 0x00002AFC
		public string SafeFileName
		{
			[SecurityCritical]
			get
			{
				string text = Path.GetFileName(this.CriticalFileName);
				if (text == null)
				{
					text = string.Empty;
				}
				return text;
			}
		}

		/// <summary>Gets an array that contains one safe file name for each selected file.</summary>
		/// <returns>An array of <see cref="T:System.String" /> that contains one safe file name for each selected file. The default is an array with a single item whose value is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00004920 File Offset: 0x00002B20
		public string[] SafeFileNames
		{
			[SecurityCritical]
			get
			{
				string[] fileNamesInternal = this.FileNamesInternal;
				string[] array = new string[fileNamesInternal.Length];
				for (int i = 0; i < fileNamesInternal.Length; i++)
				{
					array[i] = Path.GetFileName(fileNamesInternal[i]);
					if (array[i] == null)
					{
						array[i] = string.Empty;
					}
				}
				return array;
			}
		}

		/// <summary>Gets or sets a string containing the full path of the file selected in a file dialog.</summary>
		/// <returns>A <see cref="T:System.String" /> that is the full path of the file selected in the file dialog. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00004964 File Offset: 0x00002B64
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x00004971 File Offset: 0x00002B71
		public string FileName
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				return this.CriticalFileName;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				if (value == null)
				{
					this._fileNames = null;
					return;
				}
				this._fileNames = new string[]
				{
					value
				};
			}
		}

		/// <summary>Gets an array that contains one file name for each selected file.</summary>
		/// <returns>An array of <see cref="T:System.String" /> that contains one file name for each selected file. The default is an array with a single item whose value is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00004994 File Offset: 0x00002B94
		public string[] FileNames
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				return this.FileNamesInternal;
			}
		}

		/// <summary>Gets or sets the filter string that determines what types of files are displayed from either the <see cref="T:Microsoft.Win32.OpenFileDialog" /> or <see cref="T:Microsoft.Win32.SaveFileDialog" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the filter. The default is <see cref="F:System.String.Empty" />, which means that no filter is applied and all file types are displayed.</returns>
		/// <exception cref="T:System.ArgumentException">The filter string is invalid.</exception>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000049AE File Offset: 0x00002BAE
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x000049C4 File Offset: 0x00002BC4
		public string Filter
		{
			get
			{
				if (this._filter != null)
				{
					return this._filter;
				}
				return string.Empty;
			}
			set
			{
				if (string.CompareOrdinal(value, this._filter) != 0)
				{
					string text = value;
					if (!string.IsNullOrEmpty(text))
					{
						string[] array = text.Split(new char[]
						{
							'|'
						});
						if (array.Length % 2 != 0)
						{
							throw new ArgumentException(SR.Get("FileDialogInvalidFilter"));
						}
					}
					else
					{
						text = null;
					}
					this._filter = text;
				}
			}
		}

		/// <summary>Gets or sets the index of the filter currently selected in a file dialog.</summary>
		/// <returns>The <see cref="T:System.Int32" /> that is the index of the selected filter. The default is 1.</returns>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00004A1B File Offset: 0x00002C1B
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00004A23 File Offset: 0x00002C23
		public int FilterIndex
		{
			get
			{
				return this._filterIndex;
			}
			set
			{
				this._filterIndex = value;
			}
		}

		/// <summary>Gets or sets the initial directory that is displayed by a file dialog.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the initial directory. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00004A2C File Offset: 0x00002C2C
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00004A4C File Offset: 0x00002C4C
		public string InitialDirectory
		{
			get
			{
				if (this._initialDirectory.Value != null)
				{
					return this._initialDirectory.Value;
				}
				return string.Empty;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this._initialDirectory.Value = value;
			}
		}

		/// <summary>This property is not implemented.  </summary>
		/// <returns>Not implemented.</returns>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00004A5F File Offset: 0x00002C5F
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00004A68 File Offset: 0x00002C68
		public bool RestoreDirectory
		{
			get
			{
				return this.GetOption(8);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(8, value);
			}
		}

		/// <summary>Gets or sets the text that appears in the title bar of a file dialog.</summary>
		/// <returns>A <see cref="T:System.String" /> that is the text that appears in the title bar of a file dialog. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00004A77 File Offset: 0x00002C77
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00004A97 File Offset: 0x00002C97
		public string Title
		{
			get
			{
				if (this._title.Value != null)
				{
					return this._title.Value;
				}
				return string.Empty;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this._title.Value = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog accepts only valid Win32 file names.</summary>
		/// <returns>
		///     <see langword="true" /> if warnings will be shown when an invalid file name is provided; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00004AAA File Offset: 0x00002CAA
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00004ABA File Offset: 0x00002CBA
		public bool ValidateNames
		{
			get
			{
				return !this.GetOption(256);
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUnrestrictedFileIOPermission();
				this.SetOption(256, !value);
			}
		}

		/// <summary>Occurs when the user selects a file name by either clicking the Open button of the <see cref="T:Microsoft.Win32.OpenFileDialog" /> or the Save button of the <see cref="T:Microsoft.Win32.SaveFileDialog" />.</summary>
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000202 RID: 514 RVA: 0x00004AD0 File Offset: 0x00002CD0
		// (remove) Token: 0x06000203 RID: 515 RVA: 0x00004B08 File Offset: 0x00002D08
		public event CancelEventHandler FileOk;

		/// <summary>Defines the common file dialog hook procedure that is overridden to add common functionality to a file dialog.</summary>
		/// <param name="hwnd">Window handle for the Win32 dialog.</param>
		/// <param name="msg">Message to be processed by the Win32 dialog.</param>
		/// <param name="wParam">Parameters for dialog actions.</param>
		/// <param name="lParam">Parameters for dialog actions.</param>
		/// <returns>Returns <see cref="F:System.IntPtr.Zero" /> to indicate success; otherwise, a non-zero value is returned to indicate failure.</returns>
		// Token: 0x06000204 RID: 516 RVA: 0x00004B40 File Offset: 0x00002D40
		[SecurityCritical]
		protected override IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = IntPtr.Zero;
			if (msg == 78)
			{
				this._hwndFileDialog = UnsafeNativeMethods.GetParent(new HandleRef(this, hwnd));
				NativeMethods.OFNOTIFY ofnotify = (NativeMethods.OFNOTIFY)UnsafeNativeMethods.PtrToStructure(lParam, typeof(NativeMethods.OFNOTIFY));
				switch (ofnotify.hdr_code)
				{
				case -606:
					if (this._ignoreSecondFileOkNotification)
					{
						if (this._fileOkNotificationCount != 0)
						{
							this._ignoreSecondFileOkNotification = false;
							UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, hwnd), 0, NativeMethods.InvalidIntPtr);
							result = NativeMethods.InvalidIntPtr;
							break;
						}
						this._fileOkNotificationCount = 1;
					}
					if (!this.DoFileOk(ofnotify.lpOFN))
					{
						UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, hwnd), 0, NativeMethods.InvalidIntPtr);
						result = NativeMethods.InvalidIntPtr;
					}
					break;
				case -604:
					this._ignoreSecondFileOkNotification = true;
					this._fileOkNotificationCount = 0;
					break;
				case -602:
				{
					NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(ofnotify.lpOFN, typeof(NativeMethods.OPENFILENAME_I));
					int num = (int)UnsafeNativeMethods.UnsafeSendMessage(this._hwndFileDialog, (WindowMessage)1124, IntPtr.Zero, IntPtr.Zero);
					if (num > openfilename_I.nMaxFile)
					{
						int num2 = num + 2048;
						NativeMethods.CharBuffer charBuffer = NativeMethods.CharBuffer.CreateBuffer(num2);
						IntPtr lpstrFile = charBuffer.AllocCoTaskMem();
						Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
						openfilename_I.lpstrFile = lpstrFile;
						openfilename_I.nMaxFile = num2;
						this._charBuffer = charBuffer;
						Marshal.StructureToPtr(openfilename_I, ofnotify.lpOFN, true);
						Marshal.StructureToPtr(ofnotify, lParam, true);
					}
					break;
				}
				case -601:
					base.MoveToScreenCenter(new HandleRef(this, this._hwndFileDialog));
					break;
				}
			}
			else
			{
				result = base.HookProc(hwnd, msg, wParam, lParam);
			}
			return result;
		}

		/// <summary>Raises the <see cref="E:Microsoft.Win32.FileDialog.FileOk" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
		// Token: 0x06000205 RID: 517 RVA: 0x00004CF7 File Offset: 0x00002EF7
		protected void OnFileOk(CancelEventArgs e)
		{
			if (this.FileOk != null)
			{
				this.FileOk(this, e);
			}
		}

		/// <summary>
		///     <see cref="M:Microsoft.Win32.CommonDialog.RunDialog(System.IntPtr)" /> is called to display a file dialog in a derived class, such as <see cref="T:Microsoft.Win32.OpenFileDialog" /> and <see cref="T:Microsoft.Win32.SaveFileDialog" />.</summary>
		/// <param name="hwndOwner">Handle to the window that owns the dialog. </param>
		/// <returns>
		///     <see langword="true" /> if the user clicks the OK button of the dialog that is displayed (for example, <see cref="T:Microsoft.Win32.OpenFileDialog" />, <see cref="T:Microsoft.Win32.SaveFileDialog" />); otherwise, <see langword="false" />.</returns>
		// Token: 0x06000206 RID: 518 RVA: 0x00004D0E File Offset: 0x00002F0E
		[SecurityCritical]
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			if (this.UseVistaDialog)
			{
				return this.RunVistaDialog(hwndOwner);
			}
			return this.RunLegacyDialog(hwndOwner);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00004D28 File Offset: 0x00002F28
		[SecurityCritical]
		private bool RunLegacyDialog(IntPtr hwndOwner)
		{
			NativeMethods.WndProc lpfnHook = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.OPENFILENAME_I openfilename_I = new NativeMethods.OPENFILENAME_I();
			bool result;
			try
			{
				this._charBuffer = NativeMethods.CharBuffer.CreateBuffer(8192);
				if (this._fileNames != null)
				{
					this._charBuffer.PutString(this._fileNames[0]);
				}
				openfilename_I.lStructSize = Marshal.SizeOf(typeof(NativeMethods.OPENFILENAME_I));
				openfilename_I.hwndOwner = hwndOwner;
				openfilename_I.hInstance = IntPtr.Zero;
				openfilename_I.lpstrFilter = FileDialog.MakeFilterString(this._filter, this.DereferenceLinks);
				openfilename_I.nFilterIndex = this._filterIndex;
				openfilename_I.lpstrFile = this._charBuffer.AllocCoTaskMem();
				openfilename_I.nMaxFile = this._charBuffer.Length;
				openfilename_I.lpstrInitialDir = this._initialDirectory.Value;
				openfilename_I.lpstrTitle = this._title.Value;
				openfilename_I.Flags = (this.Options | 8912928);
				openfilename_I.lpfnHook = lpfnHook;
				openfilename_I.FlagsEx = 16777216;
				if (this._defaultExtension != null && this.AddExtension)
				{
					openfilename_I.lpstrDefExt = this._defaultExtension;
				}
				result = this.RunFileDialog(openfilename_I);
			}
			finally
			{
				this._charBuffer = null;
				if (openfilename_I.lpstrFile != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
				}
			}
			return result;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00004E90 File Offset: 0x00003090
		internal bool GetOption(int option)
		{
			return (this._dialogOptions.Value & option) != 0;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00004EA2 File Offset: 0x000030A2
		[SecurityCritical]
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this._dialogOptions.Value = (this._dialogOptions.Value | option);
				return;
			}
			this._dialogOptions.Value = (this._dialogOptions.Value & ~option);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00004ED0 File Offset: 0x000030D0
		[SecurityCritical]
		internal bool MessageBoxWithFocusRestore(string message, MessageBoxButton buttons, MessageBoxImage image)
		{
			bool result = false;
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			try
			{
				result = (MessageBox.Show(message, this.DialogCaption, buttons, image, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Yes);
			}
			finally
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(this, focus));
			}
			return result;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00004F1C File Offset: 0x0000311C
		[SecurityCritical]
		internal virtual bool PromptUserIfAppropriate(string fileName)
		{
			bool flag = true;
			if (this.GetOption(4096))
			{
				try
				{
					new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, fileName).Assert();
					try
					{
						string fullPath = Path.GetFullPath(fileName);
						flag = File.Exists(fullPath);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				catch (PathTooLongException)
				{
					flag = false;
				}
				if (!flag)
				{
					this.PromptFileNotFound(fileName);
				}
			}
			return flag;
		}

		// Token: 0x0600020C RID: 524
		internal abstract bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00004F8C File Offset: 0x0000318C
		internal string[] FileNamesInternal
		{
			[SecurityCritical]
			get
			{
				if (this._fileNames == null)
				{
					return new string[0];
				}
				return (string[])this._fileNames.Clone();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00004FB0 File Offset: 0x000031B0
		[SecurityCritical]
		private bool DoFileOk(IntPtr lpOFN)
		{
			NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(lpOFN, typeof(NativeMethods.OPENFILENAME_I));
			int value = this._dialogOptions.Value;
			int filterIndex = this._filterIndex;
			string[] fileNames = this._fileNames;
			bool flag = false;
			try
			{
				this._dialogOptions.Value = ((this._dialogOptions.Value & -2) | (openfilename_I.Flags & 1));
				this._filterIndex = openfilename_I.nFilterIndex;
				this._charBuffer.PutCoTaskMem(openfilename_I.lpstrFile);
				if (!this.GetOption(512))
				{
					this._fileNames = new string[]
					{
						this._charBuffer.GetString()
					};
				}
				else
				{
					this._fileNames = FileDialog.GetMultiselectFiles(this._charBuffer);
				}
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.OnFileOk(cancelEventArgs);
					flag = !cancelEventArgs.Cancel;
				}
			}
			finally
			{
				if (!flag)
				{
					this._dialogOptions.Value = value;
					this._filterIndex = filterIndex;
					this._fileNames = fileNames;
				}
			}
			return flag;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000050C0 File Offset: 0x000032C0
		private static string[] GetMultiselectFiles(NativeMethods.CharBuffer charBuffer)
		{
			string text = charBuffer.GetString();
			string text2 = charBuffer.GetString();
			if (text2.Length == 0)
			{
				return new string[]
				{
					text
				};
			}
			if (!text.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
			{
				text += Path.DirectorySeparatorChar.ToString();
			}
			List<string> list = new List<string>();
			do
			{
				bool flag = text2[0] == Path.DirectorySeparatorChar && text2[1] == Path.DirectorySeparatorChar;
				bool flag2 = text2.Length > 3 && text2[1] == Path.VolumeSeparatorChar && text2[2] == Path.DirectorySeparatorChar;
				if (!flag && !flag2)
				{
					text2 = text + text2;
				}
				list.Add(text2);
				text2 = charBuffer.GetString();
			}
			while (!string.IsNullOrEmpty(text2));
			return list.ToArray();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00005194 File Offset: 0x00003394
		[SecurityCritical]
		private void Initialize()
		{
			this._dialogOptions.Value = 0;
			this.SetOption(4, true);
			this.SetOption(2048, true);
			this.SetOption(int.MinValue, true);
			this._title.Value = null;
			this._initialDirectory.Value = null;
			this._defaultExtension = null;
			this._fileNames = null;
			this._filter = null;
			this._filterIndex = 1;
			this._ignoreSecondFileOkNotification = false;
			this._fileOkNotificationCount = 0;
			this.CustomPlaces = new List<FileDialogCustomPlace>();
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000521C File Offset: 0x0000341C
		private static string MakeFilterString(string s, bool dereferenceLinks)
		{
			if (string.IsNullOrEmpty(s))
			{
				if (!dereferenceLinks || Environment.OSVersion.Version.Major < 5)
				{
					return null;
				}
				s = " |*.*";
			}
			StringBuilder stringBuilder = new StringBuilder(s);
			stringBuilder.Replace('|', '\0');
			stringBuilder.Append('\0');
			stringBuilder.Append('\0');
			return stringBuilder.ToString();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00005278 File Offset: 0x00003478
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool ProcessFileNames()
		{
			if (!this.GetOption(256))
			{
				string[] filterExtensions = this.GetFilterExtensions();
				for (int i = 0; i < this._fileNames.Length; i++)
				{
					string text = this._fileNames[i];
					if (this.AddExtension && !Path.HasExtension(text))
					{
						for (int j = 0; j < filterExtensions.Length; j++)
						{
							Invariant.Assert(!filterExtensions[j].StartsWith(".", StringComparison.Ordinal), "FileDialog.GetFilterExtensions should not return things starting with '.'");
							string extension = Path.GetExtension(text);
							Invariant.Assert(extension.Length == 0 || extension.StartsWith(".", StringComparison.Ordinal), "Path.GetExtension should return something that starts with '.'");
							StringBuilder stringBuilder = new StringBuilder(text.Substring(0, text.Length - extension.Length));
							if (filterExtensions[j].IndexOfAny(new char[]
							{
								'*',
								'?'
							}) == -1)
							{
								stringBuilder.Append(".");
								stringBuilder.Append(filterExtensions[j]);
							}
							if (!this.GetOption(4096) || File.Exists(stringBuilder.ToString()))
							{
								text = stringBuilder.ToString();
								break;
							}
						}
						this._fileNames[i] = text;
					}
					if (!this.PromptUserIfAppropriate(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000053B7 File Offset: 0x000035B7
		[SecurityCritical]
		private void PromptFileNotFound(string fileName)
		{
			this.MessageBoxWithFocusRestore(SR.Get("FileDialogFileNotFound", new object[]
			{
				fileName
			}), MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000053D7 File Offset: 0x000035D7
		private string CriticalFileName
		{
			[SecurityCritical]
			get
			{
				if (this._fileNames == null)
				{
					return string.Empty;
				}
				if (this._fileNames[0].Length > 0)
				{
					return this._fileNames[0];
				}
				return string.Empty;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00005408 File Offset: 0x00003608
		private string DialogCaption
		{
			[SecurityCritical]
			get
			{
				if (!UnsafeNativeMethods.IsWindow(new HandleRef(this, this._hwndFileDialog)))
				{
					return string.Empty;
				}
				int windowTextLength = UnsafeNativeMethods.GetWindowTextLength(new HandleRef(this, this._hwndFileDialog));
				StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
				UnsafeNativeMethods.GetWindowText(new HandleRef(this, this._hwndFileDialog), stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00005468 File Offset: 0x00003668
		private string[] GetFilterExtensions()
		{
			string filter = this._filter;
			List<string> list = new List<string>();
			if (this._defaultExtension != null)
			{
				list.Add(this._defaultExtension);
			}
			if (filter != null)
			{
				string[] array = filter.Split(new char[]
				{
					'|'
				}, StringSplitOptions.RemoveEmptyEntries);
				int num = this._filterIndex * 2 - 1;
				if (num >= array.Length)
				{
					throw new InvalidOperationException(SR.Get("FileDialogInvalidFilterIndex"));
				}
				if (this._filterIndex > 0)
				{
					string[] array2 = array[num].Split(new char[]
					{
						';'
					});
					foreach (string text in array2)
					{
						int num2 = text.LastIndexOf('.');
						if (num2 >= 0)
						{
							list.Add(text.Substring(num2 + 1, text.Length - (num2 + 1)));
						}
					}
				}
			}
			return list.ToArray();
		}

		/// <summary>Gets the Win32 common file dialog flags that are used by file dialogs for initialization.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that contains the Win32 common file dialog flags that are used by file dialogs for initialization.</returns>
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000553F File Offset: 0x0000373F
		protected int Options
		{
			get
			{
				return this._dialogOptions.Value & 1051405;
			}
		}

		/// <summary>Gets or sets the list of custom places for file dialog boxes. </summary>
		/// <returns>The list of custom places.</returns>
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00005552 File Offset: 0x00003752
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000555A File Offset: 0x0000375A
		public IList<FileDialogCustomPlace> CustomPlaces { get; set; }

		// Token: 0x0600021A RID: 538
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal abstract IFileDialog CreateVistaDialog();

		// Token: 0x0600021B RID: 539
		[SecurityCritical]
		internal abstract string[] ProcessVistaFiles(IFileDialog dialog);

		// Token: 0x0600021C RID: 540 RVA: 0x00005564 File Offset: 0x00003764
		[SecurityCritical]
		internal virtual void PrepareVistaDialog(IFileDialog dialog)
		{
			dialog.SetDefaultExtension(this.DefaultExt);
			dialog.SetFileName(this.CriticalFileName);
			if (!string.IsNullOrEmpty(this.InitialDirectory))
			{
				IShellItem shellItemForPath = ShellUtil.GetShellItemForPath(this.InitialDirectory);
				if (shellItemForPath != null)
				{
					dialog.SetDefaultFolder(shellItemForPath);
					dialog.SetFolder(shellItemForPath);
				}
			}
			dialog.SetTitle(this.Title);
			FOS options = (FOS)((this.Options & 1063690) | 536870912 | 64);
			dialog.SetOptions(options);
			COMDLG_FILTERSPEC[] filterItems = FileDialog.GetFilterItems(this.Filter);
			if (filterItems.Length != 0)
			{
				dialog.SetFileTypes((uint)filterItems.Length, filterItems);
				dialog.SetFileTypeIndex((uint)this.FilterIndex);
			}
			IList<FileDialogCustomPlace> customPlaces = this.CustomPlaces;
			if (customPlaces != null && customPlaces.Count != 0)
			{
				foreach (FileDialogCustomPlace customPlace in customPlaces)
				{
					IShellItem shellItem = FileDialog.ResolveCustomPlace(customPlace);
					if (shellItem != null)
					{
						try
						{
							dialog.AddPlace(shellItem, FDAP.BOTTOM);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00005678 File Offset: 0x00003878
		private bool UseVistaDialog
		{
			get
			{
				return Environment.OSVersion.Version.Major >= 6;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00005690 File Offset: 0x00003890
		[SecurityCritical]
		private bool RunVistaDialog(IntPtr hwndOwner)
		{
			IFileDialog fileDialog = this.CreateVistaDialog();
			this.PrepareVistaDialog(fileDialog);
			bool succeeded;
			using (new FileDialog.VistaDialogEvents(fileDialog, new FileDialog.VistaDialogEvents.OnOkCallback(this.HandleVistaFileOk)))
			{
				succeeded = fileDialog.Show(hwndOwner).Succeeded;
			}
			return succeeded;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000056EC File Offset: 0x000038EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool HandleVistaFileOk(IFileDialog dialog)
		{
			UnsafeNativeMethods.IOleWindow oleWindow = (UnsafeNativeMethods.IOleWindow)dialog;
			oleWindow.GetWindow(out this._hwndFileDialog);
			int value = this._dialogOptions.Value;
			int filterIndex = this._filterIndex;
			string[] fileNames = this._fileNames;
			bool flag = false;
			try
			{
				uint fileTypeIndex = dialog.GetFileTypeIndex();
				this._filterIndex = (int)fileTypeIndex;
				this._fileNames = this.ProcessVistaFiles(dialog);
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.OnFileOk(cancelEventArgs);
					flag = !cancelEventArgs.Cancel;
				}
			}
			finally
			{
				if (!flag)
				{
					this._fileNames = fileNames;
					this._dialogOptions.Value = value;
					this._filterIndex = filterIndex;
				}
				else if ((this.Options & 4) != 0)
				{
					this._dialogOptions.Value = (this._dialogOptions.Value & -2);
				}
			}
			return flag;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000057C0 File Offset: 0x000039C0
		private static COMDLG_FILTERSPEC[] GetFilterItems(string filter)
		{
			List<COMDLG_FILTERSPEC> list = new List<COMDLG_FILTERSPEC>();
			if (!string.IsNullOrEmpty(filter))
			{
				string[] array = filter.Split(new char[]
				{
					'|'
				});
				if (array.Length % 2 == 0)
				{
					for (int i = 1; i < array.Length; i += 2)
					{
						list.Add(new COMDLG_FILTERSPEC
						{
							pszName = array[i - 1],
							pszSpec = array[i]
						});
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000582D File Offset: 0x00003A2D
		[SecurityCritical]
		private static IShellItem ResolveCustomPlace(FileDialogCustomPlace customPlace)
		{
			return ShellUtil.GetShellItemForPath(ShellUtil.GetPathForKnownFolder(customPlace.KnownFolder) ?? customPlace.Path);
		}

		// Token: 0x0400057C RID: 1404
		private const FOS c_VistaFileDialogMask = FOS.OVERWRITEPROMPT | FOS.NOCHANGEDIR | FOS.NOVALIDATE | FOS.ALLOWMULTISELECT | FOS.PATHMUSTEXIST | FOS.FILEMUSTEXIST | FOS.CREATEPROMPT | FOS.NODEREFERENCELINKS;

		// Token: 0x0400057D RID: 1405
		private SecurityCriticalDataForSet<int> _dialogOptions;

		// Token: 0x0400057E RID: 1406
		private bool _ignoreSecondFileOkNotification;

		// Token: 0x0400057F RID: 1407
		private int _fileOkNotificationCount;

		// Token: 0x04000580 RID: 1408
		private SecurityCriticalDataForSet<string> _title;

		// Token: 0x04000581 RID: 1409
		private SecurityCriticalDataForSet<string> _initialDirectory;

		// Token: 0x04000582 RID: 1410
		private string _defaultExtension;

		// Token: 0x04000583 RID: 1411
		private string _filter;

		// Token: 0x04000584 RID: 1412
		private int _filterIndex;

		// Token: 0x04000585 RID: 1413
		[SecurityCritical]
		private NativeMethods.CharBuffer _charBuffer;

		// Token: 0x04000586 RID: 1414
		[SecurityCritical]
		private IntPtr _hwndFileDialog;

		// Token: 0x04000587 RID: 1415
		[SecurityCritical]
		private string[] _fileNames;

		// Token: 0x04000588 RID: 1416
		private const int FILEBUFSIZE = 8192;

		// Token: 0x04000589 RID: 1417
		private const int OPTION_ADDEXTENSION = -2147483648;

		// Token: 0x0200080B RID: 2059
		private sealed class VistaDialogEvents : IFileDialogEvents, IDisposable
		{
			// Token: 0x06007E22 RID: 32290 RVA: 0x0023554C File Offset: 0x0023374C
			[SecurityCritical]
			public VistaDialogEvents(IFileDialog dialog, FileDialog.VistaDialogEvents.OnOkCallback okCallback)
			{
				this._dialog = dialog;
				this._eventCookie = dialog.Advise(this);
				this._okCallback = okCallback;
			}

			// Token: 0x06007E23 RID: 32291 RVA: 0x0023556F File Offset: 0x0023376F
			[SecurityCritical]
			HRESULT IFileDialogEvents.OnFileOk(IFileDialog pfd)
			{
				if (!this._okCallback(pfd))
				{
					return HRESULT.S_FALSE;
				}
				return HRESULT.S_OK;
			}

			// Token: 0x06007E24 RID: 32292 RVA: 0x0023558A File Offset: 0x0023378A
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnFolderChanging(IFileDialog pfd, IShellItem psiFolder)
			{
				return HRESULT.E_NOTIMPL;
			}

			// Token: 0x06007E25 RID: 32293 RVA: 0x00235591 File Offset: 0x00233791
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnFolderChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x06007E26 RID: 32294 RVA: 0x00235591 File Offset: 0x00233791
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnSelectionChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x06007E27 RID: 32295 RVA: 0x00235598 File Offset: 0x00233798
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse)
			{
				pResponse = FDESVR.DEFAULT;
				return HRESULT.S_OK;
			}

			// Token: 0x06007E28 RID: 32296 RVA: 0x00235591 File Offset: 0x00233791
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnTypeChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x06007E29 RID: 32297 RVA: 0x00235598 File Offset: 0x00233798
			[SecurityCritical]
			[SecurityTreatAsSafe]
			HRESULT IFileDialogEvents.OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse)
			{
				pResponse = FDEOR.DEFAULT;
				return HRESULT.S_OK;
			}

			// Token: 0x06007E2A RID: 32298 RVA: 0x002355A2 File Offset: 0x002337A2
			[SecurityCritical]
			void IDisposable.Dispose()
			{
				this._dialog.Unadvise(this._eventCookie);
			}

			// Token: 0x04003B96 RID: 15254
			[SecurityCritical]
			private IFileDialog _dialog;

			// Token: 0x04003B97 RID: 15255
			[SecurityCritical]
			private FileDialog.VistaDialogEvents.OnOkCallback _okCallback;

			// Token: 0x04003B98 RID: 15256
			private uint _eventCookie;

			// Token: 0x02000B9C RID: 2972
			// (Invoke) Token: 0x06008E87 RID: 36487
			[SecurityCritical(SecurityCriticalScope.Everything)]
			public delegate bool OnOkCallback(IFileDialog dialog);
		}
	}
}
