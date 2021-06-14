using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Displays a dialog box from which the user can select a file.</summary>
	// Token: 0x0200023E RID: 574
	[DefaultEvent("FileOk")]
	[DefaultProperty("FileName")]
	public abstract class FileDialog : CommonDialog
	{
		// Token: 0x06002202 RID: 8706 RVA: 0x000A60D9 File Offset: 0x000A42D9
		internal FileDialog()
		{
			this.Reset();
		}

		/// <summary>Gets or sets a value indicating whether the dialog box automatically adds an extension to a file name if the user omits the extension.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box adds an extension to a file name if the user omits the extension; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x000A60F9 File Offset: 0x000A42F9
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x000A6106 File Offset: 0x000A4306
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FDaddExtensionDescr")]
		public bool AddExtension
		{
			get
			{
				return this.GetOption(int.MinValue);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(int.MinValue, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays a warning if the user specifies a file name that does not exist; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x000A611E File Offset: 0x000A431E
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x000A612B File Offset: 0x000A432B
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FDcheckFileExistsDescr")]
		public virtual bool CheckFileExists
		{
			get
			{
				return this.GetOption(4096);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(4096, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a path that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays a warning when the user specifies a path that does not exist; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x000A6143 File Offset: 0x000A4343
		// (set) Token: 0x06002208 RID: 8712 RVA: 0x000A6150 File Offset: 0x000A4350
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FDcheckPathExistsDescr")]
		public bool CheckPathExists
		{
			get
			{
				return this.GetOption(2048);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(2048, value);
			}
		}

		/// <summary>Gets or sets the default file name extension.</summary>
		/// <returns>The default file name extension. The returned string does not include the period. The default value is an empty string ("").</returns>
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x000A6168 File Offset: 0x000A4368
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x000A617E File Offset: 0x000A437E
		[SRCategory("CatBehavior")]
		[DefaultValue("")]
		[SRDescription("FDdefaultExtDescr")]
		public string DefaultExt
		{
			get
			{
				if (this.defaultExt != null)
				{
					return this.defaultExt;
				}
				return "";
			}
			set
			{
				if (value != null)
				{
					if (value.StartsWith("."))
					{
						value = value.Substring(1);
					}
					else if (value.Length == 0)
					{
						value = null;
					}
				}
				this.defaultExt = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box returns the location of the file referenced by the shortcut or whether it returns the location of the shortcut (.lnk).</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box returns the location of the file referenced by the shortcut; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000A61AD File Offset: 0x000A43AD
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x000A61BD File Offset: 0x000A43BD
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FDdereferenceLinksDescr")]
		public bool DereferenceLinks
		{
			get
			{
				return !this.GetOption(1048576);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(1048576, !value);
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000A61D8 File Offset: 0x000A43D8
		internal string DialogCaption
		{
			get
			{
				int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, this.dialogHWnd));
				StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
				UnsafeNativeMethods.GetWindowText(new HandleRef(this, this.dialogHWnd), stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
		}

		/// <summary>Gets or sets a string containing the file name selected in the file dialog box.</summary>
		/// <returns>The file name selected in the file dialog box. The default value is an empty string ("").</returns>
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x000A6220 File Offset: 0x000A4420
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x000A6270 File Offset: 0x000A4470
		[SRCategory("CatData")]
		[DefaultValue("")]
		[SRDescription("FDfileNameDescr")]
		public string FileName
		{
			get
			{
				if (this.fileNames == null)
				{
					return "";
				}
				if (this.fileNames[0].Length > 0)
				{
					if (this.securityCheckFileNames)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.AllAccess, this.fileNames[0]);
					}
					return this.fileNames[0];
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				if (value == null)
				{
					this.fileNames = null;
				}
				else
				{
					this.fileNames = new string[]
					{
						value
					};
				}
				this.securityCheckFileNames = false;
			}
		}

		/// <summary>Gets the file names of all selected files in the dialog box.</summary>
		/// <returns>An array of type <see cref="T:System.String" />, containing the file names of all selected files in the dialog box.</returns>
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x000A62A0 File Offset: 0x000A44A0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FDFileNamesDescr")]
		public string[] FileNames
		{
			get
			{
				string[] fileNamesInternal = this.FileNamesInternal;
				if (this.securityCheckFileNames)
				{
					foreach (string fileName in fileNamesInternal)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.AllAccess, fileName);
					}
				}
				return fileNamesInternal;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000A62D9 File Offset: 0x000A44D9
		internal string[] FileNamesInternal
		{
			get
			{
				if (this.fileNames == null)
				{
					return new string[0];
				}
				return (string[])this.fileNames.Clone();
			}
		}

		/// <summary>Gets or sets the current file name filter string, which determines the choices that appear in the "Save as file type" or "Files of type" box in the dialog box.</summary>
		/// <returns>The file filtering options available in the dialog box.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="Filter" /> format is invalid. </exception>
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x000A62FA File Offset: 0x000A44FA
		// (set) Token: 0x06002213 RID: 8723 RVA: 0x000A6310 File Offset: 0x000A4510
		[SRCategory("CatBehavior")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("FDfilterDescr")]
		public string Filter
		{
			get
			{
				if (this.filter != null)
				{
					return this.filter;
				}
				return "";
			}
			set
			{
				if (value != this.filter)
				{
					if (value != null && value.Length > 0)
					{
						string[] array = value.Split(new char[]
						{
							'|'
						});
						if (array == null || array.Length % 2 != 0)
						{
							throw new ArgumentException(SR.GetString("FileDialogInvalidFilter"));
						}
					}
					else
					{
						value = null;
					}
					this.filter = value;
				}
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x000A6370 File Offset: 0x000A4570
		private string[] FilterExtensions
		{
			get
			{
				string text = this.filter;
				ArrayList arrayList = new ArrayList();
				if (this.defaultExt != null)
				{
					arrayList.Add(this.defaultExt);
				}
				if (text != null)
				{
					string[] array = text.Split(new char[]
					{
						'|'
					});
					if (this.filterIndex * 2 - 1 >= array.Length)
					{
						throw new InvalidOperationException(SR.GetString("FileDialogInvalidFilterIndex"));
					}
					if (this.filterIndex > 0)
					{
						string[] array2 = array[this.filterIndex * 2 - 1].Split(new char[]
						{
							';'
						});
						foreach (string text2 in array2)
						{
							int num = this.supportMultiDottedExtensions ? text2.IndexOf('.') : text2.LastIndexOf('.');
							if (num >= 0)
							{
								arrayList.Add(text2.Substring(num + 1, text2.Length - (num + 1)));
							}
						}
					}
				}
				string[] array4 = new string[arrayList.Count];
				arrayList.CopyTo(array4, 0);
				return array4;
			}
		}

		/// <summary>Gets or sets the index of the filter currently selected in the file dialog box.</summary>
		/// <returns>A value containing the index of the filter currently selected in the file dialog box. The default value is 1.</returns>
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000A6471 File Offset: 0x000A4671
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x000A6479 File Offset: 0x000A4679
		[SRCategory("CatBehavior")]
		[DefaultValue(1)]
		[SRDescription("FDfilterIndexDescr")]
		public int FilterIndex
		{
			get
			{
				return this.filterIndex;
			}
			set
			{
				this.filterIndex = value;
			}
		}

		/// <summary>Gets or sets the initial directory displayed by the file dialog box.</summary>
		/// <returns>The initial directory displayed by the file dialog box. The default is an empty string ("").</returns>
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000A6482 File Offset: 0x000A4682
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x000A6498 File Offset: 0x000A4698
		[SRCategory("CatData")]
		[DefaultValue("")]
		[SRDescription("FDinitialDirDescr")]
		public string InitialDirectory
		{
			get
			{
				if (this.initialDir != null)
				{
					return this.initialDir;
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.initialDir = value;
			}
		}

		/// <summary>Gets the Win32 instance handle for the application.</summary>
		/// <returns>A Win32 instance handle for the application.</returns>
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x0001EFAE File Offset: 0x0001D1AE
		protected virtual IntPtr Instance
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return UnsafeNativeMethods.GetModuleHandle(null);
			}
		}

		/// <summary>Gets values to initialize the <see cref="T:System.Windows.Forms.FileDialog" />.</summary>
		/// <returns>A bitwise combination of internal values that initializes the <see cref="T:System.Windows.Forms.FileDialog" />.</returns>
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000A64AB File Offset: 0x000A46AB
		protected int Options
		{
			get
			{
				return this.options & 1051421;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box restores the directory to the previously selected directory before closing.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box restores the current directory to the previously selected directory if the user changed the directory while searching for files; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000A64B9 File Offset: 0x000A46B9
		// (set) Token: 0x0600221C RID: 8732 RVA: 0x000A64C2 File Offset: 0x000A46C2
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FDrestoreDirectoryDescr")]
		public bool RestoreDirectory
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(8, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the Help button is displayed in the file dialog box.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box includes a help button; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x000A64D6 File Offset: 0x000A46D6
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x000A64E0 File Offset: 0x000A46E0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FDshowHelpDescr")]
		public bool ShowHelp
		{
			get
			{
				return this.GetOption(16);
			}
			set
			{
				this.SetOption(16, value);
			}
		}

		/// <summary>Gets or sets whether the dialog box supports displaying and saving files that have multiple file name extensions.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box supports multiple file name extensions; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x000A64EB File Offset: 0x000A46EB
		// (set) Token: 0x06002220 RID: 8736 RVA: 0x000A64F3 File Offset: 0x000A46F3
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FDsupportMultiDottedExtensionsDescr")]
		public bool SupportMultiDottedExtensions
		{
			get
			{
				return this.supportMultiDottedExtensions;
			}
			set
			{
				this.supportMultiDottedExtensions = value;
			}
		}

		/// <summary>Gets or sets the file dialog box title.</summary>
		/// <returns>The file dialog box title. The default value is an empty string ("").</returns>
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x000A64FC File Offset: 0x000A46FC
		// (set) Token: 0x06002222 RID: 8738 RVA: 0x000A6512 File Offset: 0x000A4712
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("FDtitleDescr")]
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.title = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box accepts only valid Win32 file names.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box accepts only valid Win32 file names; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x000A6525 File Offset: 0x000A4725
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x000A6535 File Offset: 0x000A4735
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FDvalidateNamesDescr")]
		public bool ValidateNames
		{
			get
			{
				return !this.GetOption(256);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(256, !value);
			}
		}

		/// <summary>Occurs when the user clicks on the Open or Save button on a file dialog box.</summary>
		// Token: 0x1400017F RID: 383
		// (add) Token: 0x06002225 RID: 8741 RVA: 0x000A6550 File Offset: 0x000A4750
		// (remove) Token: 0x06002226 RID: 8742 RVA: 0x000A6563 File Offset: 0x000A4763
		[SRDescription("FDfileOkDescr")]
		public event CancelEventHandler FileOk
		{
			add
			{
				base.Events.AddHandler(FileDialog.EventFileOk, value);
			}
			remove
			{
				base.Events.RemoveHandler(FileDialog.EventFileOk, value);
			}
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000A6578 File Offset: 0x000A4778
		private bool DoFileOk(IntPtr lpOFN)
		{
			NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(lpOFN, typeof(NativeMethods.OPENFILENAME_I));
			int num = this.options;
			int num2 = this.filterIndex;
			string[] array = this.fileNames;
			bool flag = this.securityCheckFileNames;
			bool flag2 = false;
			try
			{
				this.options = ((this.options & -2) | (openfilename_I.Flags & 1));
				this.filterIndex = openfilename_I.nFilterIndex;
				this.charBuffer.PutCoTaskMem(openfilename_I.lpstrFile);
				this.securityCheckFileNames = true;
				Thread.MemoryBarrier();
				if ((this.options & 512) == 0)
				{
					this.fileNames = new string[]
					{
						this.charBuffer.GetString()
					};
				}
				else
				{
					this.fileNames = this.GetMultiselectFiles(this.charBuffer);
				}
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						this.OnFileOk(cancelEventArgs);
						flag2 = !cancelEventArgs.Cancel;
					}
					else
					{
						try
						{
							this.OnFileOk(cancelEventArgs);
							flag2 = !cancelEventArgs.Cancel;
						}
						catch (Exception t)
						{
							Application.OnThreadException(t);
						}
					}
				}
			}
			finally
			{
				if (!flag2)
				{
					this.securityCheckFileNames = flag;
					Thread.MemoryBarrier();
					this.fileNames = array;
					this.options = num;
					this.filterIndex = num2;
				}
			}
			return flag2;
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000A66D0 File Offset: 0x000A48D0
		internal static bool FileExists(string fileName)
		{
			bool result = false;
			try
			{
				new FileIOPermission(FileIOPermissionAccess.Read, IntSecurity.UnsafeGetFullPath(fileName)).Assert();
				try
				{
					result = File.Exists(fileName);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			catch (PathTooLongException)
			{
			}
			return result;
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000A6724 File Offset: 0x000A4924
		private string[] GetMultiselectFiles(UnsafeNativeMethods.CharBuffer charBuffer)
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
			if (text[text.Length - 1] != '\\')
			{
				text += "\\";
			}
			ArrayList arrayList = new ArrayList();
			do
			{
				if (text2[0] != '\\' && (text2.Length <= 3 || text2[1] != ':' || text2[2] != '\\'))
				{
					text2 = text + text2;
				}
				arrayList.Add(text2);
				text2 = charBuffer.GetString();
			}
			while (text2.Length > 0);
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000A67D5 File Offset: 0x000A49D5
		internal bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		/// <summary>Defines the common dialog box hook procedure that is overridden to add specific functionality to the file dialog box.</summary>
		/// <param name="hWnd">The handle to the dialog box window. </param>
		/// <param name="msg">The message received by the dialog box. </param>
		/// <param name="wparam">Additional information about the message. </param>
		/// <param name="lparam">Additional information about the message. </param>
		/// <returns>Returns zero if the default dialog box procedure processes the message; returns a nonzero value if the default dialog box procedure ignores the message.</returns>
		// Token: 0x0600222B RID: 8747 RVA: 0x000A67E4 File Offset: 0x000A49E4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			if (msg == 78)
			{
				this.dialogHWnd = UnsafeNativeMethods.GetParent(new HandleRef(null, hWnd));
				try
				{
					UnsafeNativeMethods.OFNOTIFY ofnotify = (UnsafeNativeMethods.OFNOTIFY)UnsafeNativeMethods.PtrToStructure(lparam, typeof(UnsafeNativeMethods.OFNOTIFY));
					switch (ofnotify.hdr_code)
					{
					case -606:
						if (this.ignoreSecondFileOkNotification)
						{
							if (this.okNotificationCount != 0)
							{
								this.ignoreSecondFileOkNotification = false;
								UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), 0, new HandleRef(null, NativeMethods.InvalidIntPtr));
								return NativeMethods.InvalidIntPtr;
							}
							this.okNotificationCount = 1;
						}
						if (!this.DoFileOk(ofnotify.lpOFN))
						{
							UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), 0, new HandleRef(null, NativeMethods.InvalidIntPtr));
							return NativeMethods.InvalidIntPtr;
						}
						break;
					case -604:
						this.ignoreSecondFileOkNotification = true;
						this.okNotificationCount = 0;
						break;
					case -602:
					{
						NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(ofnotify.lpOFN, typeof(NativeMethods.OPENFILENAME_I));
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.dialogHWnd), 1124, IntPtr.Zero, IntPtr.Zero);
						if (num > openfilename_I.nMaxFile)
						{
							try
							{
								int num2 = num + 2048;
								UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(num2);
								IntPtr lpstrFile = charBuffer.AllocCoTaskMem();
								Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
								openfilename_I.lpstrFile = lpstrFile;
								openfilename_I.nMaxFile = num2;
								this.charBuffer = charBuffer;
								Marshal.StructureToPtr(openfilename_I, ofnotify.lpOFN, true);
								Marshal.StructureToPtr(ofnotify, lparam, true);
							}
							catch
							{
							}
						}
						this.ignoreSecondFileOkNotification = false;
						break;
					}
					case -601:
						CommonDialog.MoveToScreenCenter(this.dialogHWnd);
						break;
					}
				}
				catch
				{
					if (this.dialogHWnd != IntPtr.Zero)
					{
						UnsafeNativeMethods.EndDialog(new HandleRef(this, this.dialogHWnd), IntPtr.Zero);
					}
					throw;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000A6A00 File Offset: 0x000A4C00
		private static string MakeFilterString(string s, bool dereferenceLinks)
		{
			if (s == null || s.Length == 0)
			{
				if (dereferenceLinks && Environment.OSVersion.Version.Major >= 5)
				{
					s = " |*.*";
				}
				else if (s == null)
				{
					return null;
				}
			}
			int length = s.Length;
			char[] array = new char[length + 2];
			s.CopyTo(0, array, 0, length);
			for (int i = 0; i < length; i++)
			{
				if (array[i] == '|')
				{
					array[i] = '\0';
				}
			}
			array[length + 1] = '\0';
			return new string(array);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.FileDialog.FileOk" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		// Token: 0x0600222D RID: 8749 RVA: 0x000A6A78 File Offset: 0x000A4C78
		protected void OnFileOk(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[FileDialog.EventFileOk];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000A6AA8 File Offset: 0x000A4CA8
		private bool ProcessFileNames()
		{
			if ((this.options & 256) == 0)
			{
				string[] filterExtensions = this.FilterExtensions;
				for (int i = 0; i < this.fileNames.Length; i++)
				{
					string text = this.fileNames[i];
					if ((this.options & -2147483648) != 0 && !Path.HasExtension(text))
					{
						bool flag = (this.options & 4096) != 0;
						for (int j = 0; j < filterExtensions.Length; j++)
						{
							string extension = Path.GetExtension(text);
							string text2 = text.Substring(0, text.Length - extension.Length);
							if (filterExtensions[j].IndexOfAny(new char[]
							{
								'*',
								'?'
							}) == -1)
							{
								text2 = text2 + "." + filterExtensions[j];
							}
							if (!flag || FileDialog.FileExists(text2))
							{
								text = text2;
								break;
							}
						}
						this.fileNames[i] = text;
					}
					if (!this.PromptUserIfAppropriate(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000A6BA0 File Offset: 0x000A4DA0
		internal bool MessageBoxWithFocusRestore(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			bool result;
			try
			{
				result = (RTLAwareMessageBox.Show(null, message, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0) == DialogResult.Yes);
			}
			finally
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
			}
			return result;
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000A6BE8 File Offset: 0x000A4DE8
		private void PromptFileNotFound(string fileName)
		{
			this.MessageBoxWithFocusRestore(SR.GetString("FileDialogFileNotFound", new object[]
			{
				fileName
			}), this.DialogCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000A6C0E File Offset: 0x000A4E0E
		internal virtual bool PromptUserIfAppropriate(string fileName)
		{
			if ((this.options & 4096) != 0 && !FileDialog.FileExists(fileName))
			{
				this.PromptFileNotFound(fileName);
				return false;
			}
			return true;
		}

		/// <summary>Resets all properties to their default values.</summary>
		// Token: 0x06002232 RID: 8754 RVA: 0x000A6C30 File Offset: 0x000A4E30
		public override void Reset()
		{
			this.options = -2147481596;
			this.title = null;
			this.initialDir = null;
			this.defaultExt = null;
			this.fileNames = null;
			this.filter = null;
			this.filterIndex = 1;
			this.supportMultiDottedExtensions = false;
			this._customPlaces.Clear();
		}

		/// <summary>Specifies a common dialog box.</summary>
		/// <param name="hWndOwner">A value that represents the window handle of the owner window for the common dialog box. </param>
		/// <returns>
		///     <see langword="true" /> if the file could be opened; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002233 RID: 8755 RVA: 0x000A6C84 File Offset: 0x000A4E84
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("DebuggingExceptionOnly", new object[]
				{
					SR.GetString("ThreadMustBeSTA")
				}));
			}
			this.EnsureFileDialogPermission();
			if (this.UseVistaDialogInternal)
			{
				return this.RunDialogVista(hWndOwner);
			}
			return this.RunDialogOld(hWndOwner);
		}

		// Token: 0x06002234 RID: 8756
		internal abstract void EnsureFileDialogPermission();

		// Token: 0x06002235 RID: 8757 RVA: 0x000A6CE0 File Offset: 0x000A4EE0
		private bool RunDialogOld(IntPtr hWndOwner)
		{
			NativeMethods.WndProc lpfnHook = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.OPENFILENAME_I openfilename_I = new NativeMethods.OPENFILENAME_I();
			bool result;
			try
			{
				this.charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(8192);
				if (this.fileNames != null)
				{
					this.charBuffer.PutString(this.fileNames[0]);
				}
				openfilename_I.lStructSize = Marshal.SizeOf(typeof(NativeMethods.OPENFILENAME_I));
				if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
				{
					openfilename_I.lStructSize = 76;
				}
				openfilename_I.hwndOwner = hWndOwner;
				openfilename_I.hInstance = this.Instance;
				openfilename_I.lpstrFilter = FileDialog.MakeFilterString(this.filter, this.DereferenceLinks);
				openfilename_I.nFilterIndex = this.filterIndex;
				openfilename_I.lpstrFile = this.charBuffer.AllocCoTaskMem();
				openfilename_I.nMaxFile = 8192;
				openfilename_I.lpstrInitialDir = this.initialDir;
				openfilename_I.lpstrTitle = this.title;
				openfilename_I.Flags = (this.Options | 8912928);
				openfilename_I.lpfnHook = lpfnHook;
				openfilename_I.FlagsEx = 16777216;
				if (this.defaultExt != null && this.AddExtension)
				{
					openfilename_I.lpstrDefExt = this.defaultExt;
				}
				result = this.RunFileDialog(openfilename_I);
			}
			finally
			{
				this.charBuffer = null;
				if (openfilename_I.lpstrFile != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
				}
			}
			return result;
		}

		// Token: 0x06002236 RID: 8758
		internal abstract bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x06002237 RID: 8759 RVA: 0x000A6E60 File Offset: 0x000A5060
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		/// <summary>Provides a string version of this object.</summary>
		/// <returns>A string version of this object.</returns>
		// Token: 0x06002238 RID: 8760 RVA: 0x000A6E84 File Offset: 0x000A5084
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString() + ": Title: " + this.Title + ", FileName: ");
			try
			{
				stringBuilder.Append(this.FileName);
			}
			catch (Exception ex)
			{
				stringBuilder.Append("<");
				stringBuilder.Append(ex.GetType().FullName);
				stringBuilder.Append(">");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x000A6F04 File Offset: 0x000A5104
		internal virtual bool SettingsSupportVistaDialog
		{
			get
			{
				return !this.ShowHelp && (Application.VisualStyleState & VisualStyleState.ClientAreaEnabled) == VisualStyleState.ClientAreaEnabled;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x000A6F1C File Offset: 0x000A511C
		internal bool UseVistaDialogInternal
		{
			get
			{
				if (UnsafeNativeMethods.IsVista && this._autoUpgradeEnabled && this.SettingsSupportVistaDialog)
				{
					new EnvironmentPermission(PermissionState.Unrestricted).Assert();
					try
					{
						return SystemInformation.BootMode == BootMode.Normal;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x0600223B RID: 8763
		internal abstract FileDialogNative.IFileDialog CreateVistaDialog();

		// Token: 0x0600223C RID: 8764 RVA: 0x000A6F70 File Offset: 0x000A5170
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool RunDialogVista(IntPtr hWndOwner)
		{
			FileDialogNative.IFileDialog fileDialog = this.CreateVistaDialog();
			this.OnBeforeVistaDialog(fileDialog);
			FileDialog.VistaDialogEvents vistaDialogEvents = new FileDialog.VistaDialogEvents(this);
			uint dwCookie;
			fileDialog.Advise(vistaDialogEvents, out dwCookie);
			bool result;
			try
			{
				int num = fileDialog.Show(hWndOwner);
				result = (num == 0);
			}
			finally
			{
				fileDialog.Unadvise(dwCookie);
				GC.KeepAlive(vistaDialogEvents);
			}
			return result;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000A6FCC File Offset: 0x000A51CC
		internal virtual void OnBeforeVistaDialog(FileDialogNative.IFileDialog dialog)
		{
			dialog.SetDefaultExtension(this.DefaultExt);
			dialog.SetFileName(this.FileName);
			if (!string.IsNullOrEmpty(this.InitialDirectory))
			{
				try
				{
					FileDialogNative.IShellItem shellItemForPath = FileDialog.GetShellItemForPath(this.InitialDirectory);
					dialog.SetDefaultFolder(shellItemForPath);
					dialog.SetFolder(shellItemForPath);
				}
				catch (FileNotFoundException)
				{
				}
			}
			dialog.SetTitle(this.Title);
			dialog.SetOptions(this.GetOptions());
			this.SetFileTypes(dialog);
			this._customPlaces.Apply(dialog);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x000A7058 File Offset: 0x000A5258
		private FileDialogNative.FOS GetOptions()
		{
			FileDialogNative.FOS fos = (FileDialogNative.FOS)(this.options & 1063690);
			fos |= FileDialogNative.FOS.FOS_DEFAULTNOMINIMODE;
			return fos | FileDialogNative.FOS.FOS_FORCEFILESYSTEM;
		}

		// Token: 0x0600223F RID: 8767
		internal abstract string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog);

		// Token: 0x06002240 RID: 8768 RVA: 0x000A7080 File Offset: 0x000A5280
		private bool HandleVistaFileOk(FileDialogNative.IFileDialog dialog)
		{
			int num = this.options;
			int num2 = this.filterIndex;
			string[] array = this.fileNames;
			bool flag = this.securityCheckFileNames;
			bool flag2 = false;
			try
			{
				this.securityCheckFileNames = true;
				Thread.MemoryBarrier();
				uint num3;
				dialog.GetFileTypeIndex(out num3);
				this.filterIndex = (int)num3;
				this.fileNames = this.ProcessVistaFiles(dialog);
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						this.OnFileOk(cancelEventArgs);
						flag2 = !cancelEventArgs.Cancel;
					}
					else
					{
						try
						{
							this.OnFileOk(cancelEventArgs);
							flag2 = !cancelEventArgs.Cancel;
						}
						catch (Exception t)
						{
							Application.OnThreadException(t);
						}
					}
				}
			}
			finally
			{
				if (!flag2)
				{
					this.securityCheckFileNames = flag;
					Thread.MemoryBarrier();
					this.fileNames = array;
					this.options = num;
					this.filterIndex = num2;
				}
				else if ((this.options & 4) != 0)
				{
					this.options &= -2;
				}
			}
			return flag2;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000A7184 File Offset: 0x000A5384
		private void SetFileTypes(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.COMDLG_FILTERSPEC[] filterItems = this.FilterItems;
			dialog.SetFileTypes((uint)filterItems.Length, filterItems);
			if (filterItems.Length != 0)
			{
				dialog.SetFileTypeIndex((uint)this.filterIndex);
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x000A71B2 File Offset: 0x000A53B2
		private FileDialogNative.COMDLG_FILTERSPEC[] FilterItems
		{
			get
			{
				return FileDialog.GetFilterItems(this.filter);
			}
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000A71C0 File Offset: 0x000A53C0
		private static FileDialogNative.COMDLG_FILTERSPEC[] GetFilterItems(string filter)
		{
			List<FileDialogNative.COMDLG_FILTERSPEC> list = new List<FileDialogNative.COMDLG_FILTERSPEC>();
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
						FileDialogNative.COMDLG_FILTERSPEC item;
						item.pszSpec = array[i];
						item.pszName = array[i - 1];
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000A7228 File Offset: 0x000A5428
		internal static FileDialogNative.IShellItem GetShellItemForPath(string path)
		{
			FileDialogNative.IShellItem result = null;
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			if (0 <= UnsafeNativeMethods.Shell32.SHILCreateFromPath(path, out zero, ref num) && 0 <= UnsafeNativeMethods.Shell32.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, zero, out result))
			{
				return result;
			}
			throw new FileNotFoundException();
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000A7268 File Offset: 0x000A5468
		internal static string GetFilePathFromShellItem(FileDialogNative.IShellItem item)
		{
			string result;
			item.GetDisplayName((FileDialogNative.SIGDN)2147647488U, out result);
			return result;
		}

		/// <summary>Gets the custom places collection for this <see cref="T:System.Windows.Forms.FileDialog" /> instance.</summary>
		/// <returns>The custom places collection for this <see cref="T:System.Windows.Forms.FileDialog" /> instance.</returns>
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x000A7283 File Offset: 0x000A5483
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FileDialogCustomPlacesCollection CustomPlaces
		{
			get
			{
				return this._customPlaces;
			}
		}

		/// <summary>Gets or sets a value indicating whether this <see cref="T:System.Windows.Forms.FileDialog" /> instance should automatically upgrade appearance and behavior when running on Windows Vista.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Windows.Forms.FileDialog" /> instance should automatically upgrade appearance and behavior when running on Windows Vista; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x000A728B File Offset: 0x000A548B
		// (set) Token: 0x06002248 RID: 8776 RVA: 0x000A7293 File Offset: 0x000A5493
		[DefaultValue(true)]
		public bool AutoUpgradeEnabled
		{
			get
			{
				return this._autoUpgradeEnabled;
			}
			set
			{
				this._autoUpgradeEnabled = value;
			}
		}

		// Token: 0x04000ED5 RID: 3797
		private const int FILEBUFSIZE = 8192;

		/// <summary>Owns the <see cref="E:System.Windows.Forms.FileDialog.FileOk" /> event.</summary>
		// Token: 0x04000ED6 RID: 3798
		protected static readonly object EventFileOk = new object();

		// Token: 0x04000ED7 RID: 3799
		internal const int OPTION_ADDEXTENSION = -2147483648;

		// Token: 0x04000ED8 RID: 3800
		internal int options;

		// Token: 0x04000ED9 RID: 3801
		private string title;

		// Token: 0x04000EDA RID: 3802
		private string initialDir;

		// Token: 0x04000EDB RID: 3803
		private string defaultExt;

		// Token: 0x04000EDC RID: 3804
		private string[] fileNames;

		// Token: 0x04000EDD RID: 3805
		private bool securityCheckFileNames;

		// Token: 0x04000EDE RID: 3806
		private string filter;

		// Token: 0x04000EDF RID: 3807
		private int filterIndex;

		// Token: 0x04000EE0 RID: 3808
		private bool supportMultiDottedExtensions;

		// Token: 0x04000EE1 RID: 3809
		private bool ignoreSecondFileOkNotification;

		// Token: 0x04000EE2 RID: 3810
		private int okNotificationCount;

		// Token: 0x04000EE3 RID: 3811
		private UnsafeNativeMethods.CharBuffer charBuffer;

		// Token: 0x04000EE4 RID: 3812
		private IntPtr dialogHWnd;

		// Token: 0x04000EE5 RID: 3813
		private bool _autoUpgradeEnabled = true;

		// Token: 0x04000EE6 RID: 3814
		private FileDialogCustomPlacesCollection _customPlaces = new FileDialogCustomPlacesCollection();

		// Token: 0x020005D0 RID: 1488
		private class VistaDialogEvents : FileDialogNative.IFileDialogEvents
		{
			// Token: 0x06005A71 RID: 23153 RVA: 0x0017D494 File Offset: 0x0017B694
			public VistaDialogEvents(FileDialog dialog)
			{
				this._dialog = dialog;
			}

			// Token: 0x06005A72 RID: 23154 RVA: 0x0017D4A3 File Offset: 0x0017B6A3
			public int OnFileOk(FileDialogNative.IFileDialog pfd)
			{
				if (!this._dialog.HandleVistaFileOk(pfd))
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06005A73 RID: 23155 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public int OnFolderChanging(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psiFolder)
			{
				return 0;
			}

			// Token: 0x06005A74 RID: 23156 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnFolderChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06005A75 RID: 23157 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnSelectionChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06005A76 RID: 23158 RVA: 0x0017D4B6 File Offset: 0x0017B6B6
			public void OnShareViolation(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psi, out FileDialogNative.FDE_SHAREVIOLATION_RESPONSE pResponse)
			{
				pResponse = FileDialogNative.FDE_SHAREVIOLATION_RESPONSE.FDESVR_DEFAULT;
			}

			// Token: 0x06005A77 RID: 23159 RVA: 0x0000701A File Offset: 0x0000521A
			public void OnTypeChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06005A78 RID: 23160 RVA: 0x0017D4B6 File Offset: 0x0017B6B6
			public void OnOverwrite(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psi, out FileDialogNative.FDE_OVERWRITE_RESPONSE pResponse)
			{
				pResponse = FileDialogNative.FDE_OVERWRITE_RESPONSE.FDEOR_DEFAULT;
			}

			// Token: 0x0400396C RID: 14700
			private FileDialog _dialog;
		}
	}
}
