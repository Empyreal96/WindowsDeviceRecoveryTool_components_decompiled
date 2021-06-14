using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	/// <summary>Lets users select a printer and choose which sections of the document to print from a Windows Forms application.</summary>
	// Token: 0x0200043D RID: 1085
	[DefaultProperty("Document")]
	[SRDescription("DescriptionPrintDialog")]
	[Designer("System.Windows.Forms.Design.PrintDialogDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class PrintDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintDialog" /> class.</summary>
		// Token: 0x06004B74 RID: 19316 RVA: 0x000A77FF File Offset: 0x000A59FF
		public PrintDialog()
		{
			this.Reset();
		}

		/// <summary>Gets or sets a value indicating whether the Current Page option button is displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the Current Page option button is displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06004B75 RID: 19317 RVA: 0x00137085 File Offset: 0x00135285
		// (set) Token: 0x06004B76 RID: 19318 RVA: 0x0013708D File Offset: 0x0013528D
		[DefaultValue(false)]
		[SRDescription("PDallowCurrentPageDescr")]
		public bool AllowCurrentPage
		{
			get
			{
				return this.allowCurrentPage;
			}
			set
			{
				this.allowCurrentPage = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Pages option button is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the Pages option button is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06004B77 RID: 19319 RVA: 0x00137096 File Offset: 0x00135296
		// (set) Token: 0x06004B78 RID: 19320 RVA: 0x0013709E File Offset: 0x0013529E
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PDallowPagesDescr")]
		public bool AllowSomePages
		{
			get
			{
				return this.allowPages;
			}
			set
			{
				this.allowPages = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Print to file check box is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the Print to file check box is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06004B79 RID: 19321 RVA: 0x001370A7 File Offset: 0x001352A7
		// (set) Token: 0x06004B7A RID: 19322 RVA: 0x001370AF File Offset: 0x001352AF
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PDallowPrintToFileDescr")]
		public bool AllowPrintToFile
		{
			get
			{
				return this.allowPrintToFile;
			}
			set
			{
				this.allowPrintToFile = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Selection option button is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the Selection option button is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06004B7B RID: 19323 RVA: 0x001370B8 File Offset: 0x001352B8
		// (set) Token: 0x06004B7C RID: 19324 RVA: 0x001370C0 File Offset: 0x001352C0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PDallowSelectionDescr")]
		public bool AllowSelection
		{
			get
			{
				return this.allowSelection;
			}
			set
			{
				this.allowSelection = value;
			}
		}

		/// <summary>Gets or sets a value indicating the <see cref="T:System.Drawing.Printing.PrintDocument" /> used to obtain <see cref="T:System.Drawing.Printing.PrinterSettings" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrintDocument" /> used to obtain <see cref="T:System.Drawing.Printing.PrinterSettings" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06004B7D RID: 19325 RVA: 0x001370C9 File Offset: 0x001352C9
		// (set) Token: 0x06004B7E RID: 19326 RVA: 0x001370D1 File Offset: 0x001352D1
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[SRDescription("PDdocumentDescr")]
		public PrintDocument Document
		{
			get
			{
				return this.printDocument;
			}
			set
			{
				this.printDocument = value;
				if (this.printDocument == null)
				{
					this.settings = new PrinterSettings();
					return;
				}
				this.settings = this.printDocument.PrinterSettings;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06004B7F RID: 19327 RVA: 0x001370FF File Offset: 0x001352FF
		private PageSettings PageSettings
		{
			get
			{
				if (this.Document == null)
				{
					return this.PrinterSettings.DefaultPageSettings;
				}
				return this.Document.DefaultPageSettings;
			}
		}

		/// <summary>Gets or sets the printer settings the dialog box modifies.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrinterSettings" /> the dialog box modifies.</returns>
		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06004B80 RID: 19328 RVA: 0x00137120 File Offset: 0x00135320
		// (set) Token: 0x06004B81 RID: 19329 RVA: 0x0013713B File Offset: 0x0013533B
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PDprinterSettingsDescr")]
		public PrinterSettings PrinterSettings
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = new PrinterSettings();
				}
				return this.settings;
			}
			set
			{
				if (value != this.PrinterSettings)
				{
					this.settings = value;
					this.printDocument = null;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the Print to file check box is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the Print to file check box is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06004B82 RID: 19330 RVA: 0x00137154 File Offset: 0x00135354
		// (set) Token: 0x06004B83 RID: 19331 RVA: 0x0013715C File Offset: 0x0013535C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PDprintToFileDescr")]
		public bool PrintToFile
		{
			get
			{
				return this.printToFile;
			}
			set
			{
				this.printToFile = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Help button is displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the Help button is displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06004B84 RID: 19332 RVA: 0x00137165 File Offset: 0x00135365
		// (set) Token: 0x06004B85 RID: 19333 RVA: 0x0013716D File Offset: 0x0013536D
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PDshowHelpDescr")]
		public bool ShowHelp
		{
			get
			{
				return this.showHelp;
			}
			set
			{
				this.showHelp = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Network button is displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the Network button is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06004B86 RID: 19334 RVA: 0x00137176 File Offset: 0x00135376
		// (set) Token: 0x06004B87 RID: 19335 RVA: 0x0013717E File Offset: 0x0013537E
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PDshowNetworkDescr")]
		public bool ShowNetwork
		{
			get
			{
				return this.showNetwork;
			}
			set
			{
				this.showNetwork = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog should be shown in the Windows XP style for systems running Windows XP Home Edition, Windows XP Professional, Windows Server 2003 or later.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate the dialog should be shown with the Windows XP style, otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06004B88 RID: 19336 RVA: 0x00137187 File Offset: 0x00135387
		// (set) Token: 0x06004B89 RID: 19337 RVA: 0x0013718F File Offset: 0x0013538F
		[DefaultValue(false)]
		[SRDescription("PDuseEXDialog")]
		public bool UseEXDialog
		{
			get
			{
				return this.useEXDialog;
			}
			set
			{
				this.useEXDialog = value;
			}
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x00137198 File Offset: 0x00135398
		private int GetFlags()
		{
			int num = 0;
			if (!this.UseEXDialog || Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				num |= 4096;
			}
			if (!this.allowCurrentPage)
			{
				num |= 8388608;
			}
			if (!this.allowPages)
			{
				num |= 8;
			}
			if (!this.allowPrintToFile)
			{
				num |= 524288;
			}
			if (!this.allowSelection)
			{
				num |= 4;
			}
			num |= (int)this.PrinterSettings.PrintRange;
			if (this.printToFile)
			{
				num |= 32;
			}
			if (this.showHelp)
			{
				num |= 2048;
			}
			if (!this.showNetwork)
			{
				num |= 2097152;
			}
			if (this.PrinterSettings.Collate)
			{
				num |= 16;
			}
			return num;
		}

		/// <summary>Resets all options, the last selected printer, and the page settings to their default values.</summary>
		// Token: 0x06004B8B RID: 19339 RVA: 0x0013725C File Offset: 0x0013545C
		public override void Reset()
		{
			this.allowCurrentPage = false;
			this.allowPages = false;
			this.allowPrintToFile = true;
			this.allowSelection = false;
			this.printDocument = null;
			this.printToFile = false;
			this.settings = null;
			this.showHelp = false;
			this.showNetwork = true;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x001372A8 File Offset: 0x001354A8
		internal static NativeMethods.PRINTDLG CreatePRINTDLG()
		{
			NativeMethods.PRINTDLG printdlg;
			if (IntPtr.Size == 4)
			{
				printdlg = new NativeMethods.PRINTDLG_32();
			}
			else
			{
				printdlg = new NativeMethods.PRINTDLG_64();
			}
			printdlg.lStructSize = Marshal.SizeOf(printdlg);
			printdlg.hwndOwner = IntPtr.Zero;
			printdlg.hDevMode = IntPtr.Zero;
			printdlg.hDevNames = IntPtr.Zero;
			printdlg.Flags = 0;
			printdlg.hDC = IntPtr.Zero;
			printdlg.nFromPage = 1;
			printdlg.nToPage = 1;
			printdlg.nMinPage = 0;
			printdlg.nMaxPage = 9999;
			printdlg.nCopies = 1;
			printdlg.hInstance = IntPtr.Zero;
			printdlg.lCustData = IntPtr.Zero;
			printdlg.lpfnPrintHook = null;
			printdlg.lpfnSetupHook = null;
			printdlg.lpPrintTemplateName = null;
			printdlg.lpSetupTemplateName = null;
			printdlg.hPrintTemplate = IntPtr.Zero;
			printdlg.hSetupTemplate = IntPtr.Zero;
			return printdlg;
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x0013737C File Offset: 0x0013557C
		internal static NativeMethods.PRINTDLGEX CreatePRINTDLGEX()
		{
			NativeMethods.PRINTDLGEX printdlgex = new NativeMethods.PRINTDLGEX();
			printdlgex.lStructSize = Marshal.SizeOf(printdlgex);
			printdlgex.hwndOwner = IntPtr.Zero;
			printdlgex.hDevMode = IntPtr.Zero;
			printdlgex.hDevNames = IntPtr.Zero;
			printdlgex.hDC = IntPtr.Zero;
			printdlgex.Flags = 0;
			printdlgex.Flags2 = 0;
			printdlgex.ExclusionFlags = 0;
			printdlgex.nPageRanges = 0;
			printdlgex.nMaxPageRanges = 1;
			printdlgex.pageRanges = UnsafeNativeMethods.GlobalAlloc(64, printdlgex.nMaxPageRanges * Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
			printdlgex.nMinPage = 0;
			printdlgex.nMaxPage = 9999;
			printdlgex.nCopies = 1;
			printdlgex.hInstance = IntPtr.Zero;
			printdlgex.lpPrintTemplateName = null;
			printdlgex.nPropertyPages = 0;
			printdlgex.lphPropertyPages = IntPtr.Zero;
			printdlgex.nStartPage = NativeMethods.START_PAGE_GENERAL;
			printdlgex.dwResultAction = 0;
			return printdlgex;
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00137460 File Offset: 0x00135660
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			IntSecurity.SafePrinting.Demand();
			NativeMethods.WndProc hookProcPtr = new NativeMethods.WndProc(this.HookProc);
			bool result;
			if (!this.UseEXDialog || Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				NativeMethods.PRINTDLG data = PrintDialog.CreatePRINTDLG();
				result = this.ShowPrintDialog(hwndOwner, hookProcPtr, data);
			}
			else
			{
				NativeMethods.PRINTDLGEX data2 = PrintDialog.CreatePRINTDLGEX();
				result = this.ShowPrintDialog(hwndOwner, data2);
			}
			return result;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x001374D0 File Offset: 0x001356D0
		private bool ShowPrintDialog(IntPtr hwndOwner, NativeMethods.WndProc hookProcPtr, NativeMethods.PRINTDLG data)
		{
			data.Flags = this.GetFlags();
			data.nCopies = this.PrinterSettings.Copies;
			data.hwndOwner = hwndOwner;
			data.lpfnPrintHook = hookProcPtr;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				if (this.PageSettings == null)
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode();
				}
				else
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode(this.PageSettings);
				}
				data.hDevNames = this.PrinterSettings.GetHdevnames();
			}
			catch (InvalidPrinterException)
			{
				data.hDevMode = IntPtr.Zero;
				data.hDevNames = IntPtr.Zero;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (this.AllowSomePages)
				{
					if (this.PrinterSettings.FromPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.FromPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.ToPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"ToPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.FromPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					data.nFromPage = (short)this.PrinterSettings.FromPage;
					data.nToPage = (short)this.PrinterSettings.ToPage;
					data.nMinPage = (short)this.PrinterSettings.MinimumPage;
					data.nMaxPage = (short)this.PrinterSettings.MaximumPage;
				}
				if (!UnsafeNativeMethods.PrintDlg(data))
				{
					result = false;
				}
				else
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					try
					{
						PrintDialog.UpdatePrinterSettings(data.hDevMode, data.hDevNames, data.nCopies, data.Flags, this.settings, this.PageSettings);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.PrintToFile = ((data.Flags & 32) != 0);
					this.PrinterSettings.PrintToFile = this.PrintToFile;
					if (this.AllowSomePages)
					{
						this.PrinterSettings.FromPage = (int)data.nFromPage;
						this.PrinterSettings.ToPage = (int)data.nToPage;
					}
					if ((data.Flags & 262144) == 0 && Environment.OSVersion.Version.Major >= 6)
					{
						this.PrinterSettings.Copies = data.nCopies;
						this.PrinterSettings.Collate = ((data.Flags & 16) == 16);
					}
					result = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevMode));
				UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevNames));
			}
			return result;
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x0013780C File Offset: 0x00135A0C
		private unsafe bool ShowPrintDialog(IntPtr hwndOwner, NativeMethods.PRINTDLGEX data)
		{
			data.Flags = this.GetFlags();
			data.nCopies = (int)this.PrinterSettings.Copies;
			data.hwndOwner = hwndOwner;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				if (this.PageSettings == null)
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode();
				}
				else
				{
					data.hDevMode = this.PrinterSettings.GetHdevmode(this.PageSettings);
				}
				data.hDevNames = this.PrinterSettings.GetHdevnames();
			}
			catch (InvalidPrinterException)
			{
				data.hDevMode = IntPtr.Zero;
				data.hDevNames = IntPtr.Zero;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (this.AllowSomePages)
				{
					if (this.PrinterSettings.FromPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.FromPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.MinimumPage || this.PrinterSettings.ToPage > this.PrinterSettings.MaximumPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"ToPage"
						}));
					}
					if (this.PrinterSettings.ToPage < this.PrinterSettings.FromPage)
					{
						throw new ArgumentException(SR.GetString("PDpageOutOfRange", new object[]
						{
							"FromPage"
						}));
					}
					int* ptr = (int*)((void*)data.pageRanges);
					*ptr = this.PrinterSettings.FromPage;
					ptr++;
					*ptr = this.PrinterSettings.ToPage;
					data.nPageRanges = 1;
					data.nMinPage = this.PrinterSettings.MinimumPage;
					data.nMaxPage = this.PrinterSettings.MaximumPage;
				}
				data.Flags &= -2099201;
				int hr = UnsafeNativeMethods.PrintDlgEx(data);
				if (NativeMethods.Failed(hr) || data.dwResultAction == 0)
				{
					result = false;
				}
				else
				{
					IntSecurity.AllPrintingAndUnmanagedCode.Assert();
					try
					{
						PrintDialog.UpdatePrinterSettings(data.hDevMode, data.hDevNames, (short)data.nCopies, data.Flags, this.PrinterSettings, this.PageSettings);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.PrintToFile = ((data.Flags & 32) != 0);
					this.PrinterSettings.PrintToFile = this.PrintToFile;
					if (this.AllowSomePages)
					{
						int* ptr2 = (int*)((void*)data.pageRanges);
						this.PrinterSettings.FromPage = *ptr2;
						ptr2++;
						this.PrinterSettings.ToPage = *ptr2;
					}
					if ((data.Flags & 262144) == 0 && Environment.OSVersion.Version.Major >= 6)
					{
						this.PrinterSettings.Copies = (short)data.nCopies;
						this.PrinterSettings.Collate = ((data.Flags & 16) == 16);
					}
					result = (data.dwResultAction == 1);
				}
			}
			finally
			{
				if (data.hDevMode != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevMode));
				}
				if (data.hDevNames != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.hDevNames));
				}
				if (data.pageRanges != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(new HandleRef(data, data.pageRanges));
				}
			}
			return result;
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x00137BC8 File Offset: 0x00135DC8
		private static void UpdatePrinterSettings(IntPtr hDevMode, IntPtr hDevNames, short copies, int flags, PrinterSettings settings, PageSettings pageSettings)
		{
			settings.SetHdevmode(hDevMode);
			settings.SetHdevnames(hDevNames);
			if (pageSettings != null)
			{
				pageSettings.SetHdevmode(hDevMode);
			}
			if (settings.Copies == 1)
			{
				settings.Copies = copies;
			}
			settings.PrintRange = (PrintRange)(flags & 4194307);
		}

		// Token: 0x04002796 RID: 10134
		private const int printRangeMask = 4194307;

		// Token: 0x04002797 RID: 10135
		private PrinterSettings settings;

		// Token: 0x04002798 RID: 10136
		private PrintDocument printDocument;

		// Token: 0x04002799 RID: 10137
		private bool allowCurrentPage;

		// Token: 0x0400279A RID: 10138
		private bool allowPages;

		// Token: 0x0400279B RID: 10139
		private bool allowPrintToFile;

		// Token: 0x0400279C RID: 10140
		private bool allowSelection;

		// Token: 0x0400279D RID: 10141
		private bool printToFile;

		// Token: 0x0400279E RID: 10142
		private bool showHelp;

		// Token: 0x0400279F RID: 10143
		private bool showNetwork;

		// Token: 0x040027A0 RID: 10144
		private bool useEXDialog;
	}
}
