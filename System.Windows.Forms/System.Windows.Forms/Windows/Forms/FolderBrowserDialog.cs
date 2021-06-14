using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Prompts the user to select a folder. This class cannot be inherited.</summary>
	// Token: 0x0200024A RID: 586
	[DefaultEvent("HelpRequest")]
	[DefaultProperty("SelectedPath")]
	[Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionFolderBrowserDialog")]
	public sealed class FolderBrowserDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FolderBrowserDialog" /> class.</summary>
		// Token: 0x06002277 RID: 8823 RVA: 0x000A77FF File Offset: 0x000A59FF
		public FolderBrowserDialog()
		{
			this.Reset();
		}

		/// <summary>Occurs when the user clicks the Help button on the dialog box.</summary>
		// Token: 0x14000180 RID: 384
		// (add) Token: 0x06002278 RID: 8824 RVA: 0x000A780D File Offset: 0x000A5A0D
		// (remove) Token: 0x06002279 RID: 8825 RVA: 0x000A7816 File Offset: 0x000A5A16
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler HelpRequest
		{
			add
			{
				base.HelpRequest += value;
			}
			remove
			{
				base.HelpRequest -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box.</summary>
		/// <returns>
		///     <see langword="true" /> if the New Folder button is shown in the dialog box; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x000A781F File Offset: 0x000A5A1F
		// (set) Token: 0x0600227B RID: 8827 RVA: 0x000A7827 File Offset: 0x000A5A27
		[Browsable(true)]
		[DefaultValue(true)]
		[Localizable(false)]
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogShowNewFolderButton")]
		public bool ShowNewFolderButton
		{
			get
			{
				return this.showNewFolderButton;
			}
			set
			{
				this.showNewFolderButton = value;
			}
		}

		/// <summary>Gets or sets the path selected by the user.</summary>
		/// <returns>The path of the folder first selected in the dialog box or the last folder selected by the user. The default is an empty string ("").</returns>
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x000A7830 File Offset: 0x000A5A30
		// (set) Token: 0x0600227D RID: 8829 RVA: 0x000A786D File Offset: 0x000A5A6D
		[Browsable(true)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogSelectedPath")]
		public string SelectedPath
		{
			get
			{
				if (this.selectedPath == null || this.selectedPath.Length == 0)
				{
					return this.selectedPath;
				}
				if (this.selectedPathNeedsCheck)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.selectedPath).Demand();
				}
				return this.selectedPath;
			}
			set
			{
				this.selectedPath = ((value == null) ? string.Empty : value);
				this.selectedPathNeedsCheck = false;
			}
		}

		/// <summary>Gets or sets the root folder where the browsing starts from.</summary>
		/// <returns>One of the <see cref="T:System.Environment.SpecialFolder" /> values. The default is <see langword="Desktop" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Environment.SpecialFolder" /> values. </exception>
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000A7887 File Offset: 0x000A5A87
		// (set) Token: 0x0600227F RID: 8831 RVA: 0x000A788F File Offset: 0x000A5A8F
		[Browsable(true)]
		[DefaultValue(Environment.SpecialFolder.Desktop)]
		[Localizable(false)]
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogRootFolder")]
		[TypeConverter(typeof(SpecialFolderEnumConverter))]
		public Environment.SpecialFolder RootFolder
		{
			get
			{
				return this.rootFolder;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Environment.SpecialFolder));
				}
				this.rootFolder = value;
			}
		}

		/// <summary>Gets or sets the descriptive text displayed above the tree view control in the dialog box.</summary>
		/// <returns>The description to display. The default is an empty string ("").</returns>
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000A78C5 File Offset: 0x000A5AC5
		// (set) Token: 0x06002281 RID: 8833 RVA: 0x000A78CD File Offset: 0x000A5ACD
		[Browsable(true)]
		[DefaultValue("")]
		[Localizable(true)]
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogDescription")]
		public string Description
		{
			get
			{
				return this.descriptionText;
			}
			set
			{
				this.descriptionText = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000A78E0 File Offset: 0x000A5AE0
		private static UnsafeNativeMethods.IMalloc GetSHMalloc()
		{
			UnsafeNativeMethods.IMalloc[] array = new UnsafeNativeMethods.IMalloc[1];
			UnsafeNativeMethods.Shell32.SHGetMalloc(array);
			return array[0];
		}

		/// <summary>Resets properties to their default values.</summary>
		// Token: 0x06002283 RID: 8835 RVA: 0x000A78FE File Offset: 0x000A5AFE
		public override void Reset()
		{
			this.rootFolder = Environment.SpecialFolder.Desktop;
			this.descriptionText = string.Empty;
			this.selectedPath = string.Empty;
			this.selectedPathNeedsCheck = false;
			this.showNewFolderButton = true;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000A792C File Offset: 0x000A5B2C
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			IntPtr zero = IntPtr.Zero;
			bool result = false;
			UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int)this.rootFolder, ref zero);
			if (zero == IntPtr.Zero)
			{
				UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hWndOwner, 0, ref zero);
				if (zero == IntPtr.Zero)
				{
					throw new InvalidOperationException(SR.GetString("FolderBrowserDialogNoRootFolder"));
				}
			}
			int num = 64;
			if (!this.showNewFolderButton)
			{
				num += 512;
			}
			if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("DebuggingExceptionOnly", new object[]
				{
					SR.GetString("ThreadMustBeSTA")
				}));
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.BROWSEINFO browseinfo = new UnsafeNativeMethods.BROWSEINFO();
				intPtr2 = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
				intPtr3 = Marshal.AllocHGlobal(261 * Marshal.SystemDefaultCharSize);
				this.callback = new UnsafeNativeMethods.BrowseCallbackProc(this.FolderBrowserDialog_BrowseCallbackProc);
				browseinfo.pidlRoot = zero;
				browseinfo.hwndOwner = hWndOwner;
				browseinfo.pszDisplayName = intPtr2;
				browseinfo.lpszTitle = this.descriptionText;
				browseinfo.ulFlags = num;
				browseinfo.lpfn = this.callback;
				browseinfo.lParam = IntPtr.Zero;
				browseinfo.iImage = 0;
				intPtr = UnsafeNativeMethods.Shell32.SHBrowseForFolder(browseinfo);
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.Shell32.SHGetPathFromIDListLongPath(intPtr, ref intPtr3);
					this.selectedPathNeedsCheck = true;
					this.selectedPath = Marshal.PtrToStringAuto(intPtr3);
					result = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.CoTaskMemFree(zero);
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.CoTaskMemFree(intPtr);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr3);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
				this.callback = null;
			}
			return result;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000A7AFC File Offset: 0x000A5CFC
		private int FolderBrowserDialog_BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
		{
			if (msg != 1)
			{
				if (msg == 2)
				{
					if (lParam != IntPtr.Zero)
					{
						IntPtr hglobal = Marshal.AllocHGlobal(261 * Marshal.SystemDefaultCharSize);
						bool flag = UnsafeNativeMethods.Shell32.SHGetPathFromIDListLongPath(lParam, ref hglobal);
						Marshal.FreeHGlobal(hglobal);
						UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), 1125, 0, flag ? 1 : 0);
					}
				}
			}
			else if (this.selectedPath.Length != 0)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), NativeMethods.BFFM_SETSELECTION, 1, this.selectedPath);
			}
			return 0;
		}

		// Token: 0x04000EFE RID: 3838
		private Environment.SpecialFolder rootFolder;

		// Token: 0x04000EFF RID: 3839
		private string descriptionText;

		// Token: 0x04000F00 RID: 3840
		private string selectedPath;

		// Token: 0x04000F01 RID: 3841
		private bool showNewFolderButton;

		// Token: 0x04000F02 RID: 3842
		private bool selectedPathNeedsCheck;

		// Token: 0x04000F03 RID: 3843
		private UnsafeNativeMethods.BrowseCallbackProc callback;
	}
}
