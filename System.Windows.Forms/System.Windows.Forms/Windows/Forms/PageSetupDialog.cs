using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Enables users to change page-related print settings, including margins and paper orientation. This class cannot be inherited. </summary>
	// Token: 0x0200043B RID: 1083
	[DefaultProperty("Document")]
	[SRDescription("DescriptionPageSetupDialog")]
	public sealed class PageSetupDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PageSetupDialog" /> class.</summary>
		// Token: 0x06004B50 RID: 19280 RVA: 0x000A77FF File Offset: 0x000A59FF
		public PageSetupDialog()
		{
			this.Reset();
		}

		/// <summary>Gets or sets a value indicating whether the margins section of the dialog box is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the margins section of the dialog box is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x00136960 File Offset: 0x00134B60
		// (set) Token: 0x06004B52 RID: 19282 RVA: 0x00136968 File Offset: 0x00134B68
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PSDallowMarginsDescr")]
		public bool AllowMargins
		{
			get
			{
				return this.allowMargins;
			}
			set
			{
				this.allowMargins = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the orientation section of the dialog box (landscape versus portrait) is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the orientation section of the dialog box is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06004B53 RID: 19283 RVA: 0x00136971 File Offset: 0x00134B71
		// (set) Token: 0x06004B54 RID: 19284 RVA: 0x00136979 File Offset: 0x00134B79
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PSDallowOrientationDescr")]
		public bool AllowOrientation
		{
			get
			{
				return this.allowOrientation;
			}
			set
			{
				this.allowOrientation = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the paper section of the dialog box (paper size and paper source) is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the paper section of the dialog box is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06004B55 RID: 19285 RVA: 0x00136982 File Offset: 0x00134B82
		// (set) Token: 0x06004B56 RID: 19286 RVA: 0x0013698A File Offset: 0x00134B8A
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PSDallowPaperDescr")]
		public bool AllowPaper
		{
			get
			{
				return this.allowPaper;
			}
			set
			{
				this.allowPaper = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Printer button is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the Printer button is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06004B57 RID: 19287 RVA: 0x00136993 File Offset: 0x00134B93
		// (set) Token: 0x06004B58 RID: 19288 RVA: 0x0013699B File Offset: 0x00134B9B
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PSDallowPrinterDescr")]
		public bool AllowPrinter
		{
			get
			{
				return this.allowPrinter;
			}
			set
			{
				this.allowPrinter = value;
			}
		}

		/// <summary>Gets or sets a value indicating the <see cref="T:System.Drawing.Printing.PrintDocument" /> to get page settings from.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrintDocument" /> to get page settings from. The default is <see langword="null" />.</returns>
		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06004B59 RID: 19289 RVA: 0x001369A4 File Offset: 0x00134BA4
		// (set) Token: 0x06004B5A RID: 19290 RVA: 0x001369AC File Offset: 0x00134BAC
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
				if (this.printDocument != null)
				{
					this.pageSettings = this.printDocument.DefaultPageSettings;
					this.printerSettings = this.printDocument.PrinterSettings;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the margin settings, when displayed in millimeters, should be automatically converted to and from hundredths of an inch.</summary>
		/// <returns>
		///     <see langword="true" /> if the margins should be automatically converted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06004B5B RID: 19291 RVA: 0x001369DF File Offset: 0x00134BDF
		// (set) Token: 0x06004B5C RID: 19292 RVA: 0x001369E7 File Offset: 0x00134BE7
		[DefaultValue(false)]
		[SRDescription("PSDenableMetricDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public bool EnableMetric
		{
			get
			{
				return this.enableMetric;
			}
			set
			{
				this.enableMetric = value;
			}
		}

		/// <summary>Gets or sets a value indicating the minimum margins, in hundredths of an inch, the user is allowed to select.</summary>
		/// <returns>The minimum margins, in hundredths of an inch, the user is allowed to select. The default is <see langword="null" />.</returns>
		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x001369F0 File Offset: 0x00134BF0
		// (set) Token: 0x06004B5E RID: 19294 RVA: 0x001369F8 File Offset: 0x00134BF8
		[SRCategory("CatData")]
		[SRDescription("PSDminMarginsDescr")]
		public Margins MinMargins
		{
			get
			{
				return this.minMargins;
			}
			set
			{
				if (value == null)
				{
					value = new Margins(0, 0, 0, 0);
				}
				this.minMargins = value;
			}
		}

		/// <summary>Gets or sets a value indicating the page settings to modify.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PageSettings" /> to modify. The default is <see langword="null" />.</returns>
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x00136A15 File Offset: 0x00134C15
		// (set) Token: 0x06004B60 RID: 19296 RVA: 0x00136A1D File Offset: 0x00134C1D
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PSDpageSettingsDescr")]
		public PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
			set
			{
				this.pageSettings = value;
				this.printDocument = null;
			}
		}

		/// <summary>Gets or sets the printer settings that are modified when the user clicks the Printer button in the dialog.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrinterSettings" /> to modify when the user clicks the Printer button. The default is <see langword="null" />.</returns>
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x00136A2D File Offset: 0x00134C2D
		// (set) Token: 0x06004B62 RID: 19298 RVA: 0x00136A35 File Offset: 0x00134C35
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("PSDprinterSettingsDescr")]
		public PrinterSettings PrinterSettings
		{
			get
			{
				return this.printerSettings;
			}
			set
			{
				this.printerSettings = value;
				this.printDocument = null;
			}
		}

		/// <summary>Gets or sets a value indicating whether the Help button is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the Help button is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x00136A45 File Offset: 0x00134C45
		// (set) Token: 0x06004B64 RID: 19300 RVA: 0x00136A4D File Offset: 0x00134C4D
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PSDshowHelpDescr")]
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

		/// <summary>Gets or sets a value indicating whether the Network button is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the Network button is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06004B65 RID: 19301 RVA: 0x00136A56 File Offset: 0x00134C56
		// (set) Token: 0x06004B66 RID: 19302 RVA: 0x00136A5E File Offset: 0x00134C5E
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PSDshowNetworkDescr")]
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

		// Token: 0x06004B67 RID: 19303 RVA: 0x00136A68 File Offset: 0x00134C68
		private int GetFlags()
		{
			int num = 0;
			num |= 8192;
			if (!this.allowMargins)
			{
				num |= 16;
			}
			if (!this.allowOrientation)
			{
				num |= 256;
			}
			if (!this.allowPaper)
			{
				num |= 512;
			}
			if (!this.allowPrinter || this.printerSettings == null)
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
			if (this.minMargins != null)
			{
				num |= 1;
			}
			if (this.pageSettings.Margins != null)
			{
				num |= 2;
			}
			return num;
		}

		/// <summary>Resets all options to their default values.</summary>
		// Token: 0x06004B68 RID: 19304 RVA: 0x00136B0C File Offset: 0x00134D0C
		public override void Reset()
		{
			this.allowMargins = true;
			this.allowOrientation = true;
			this.allowPaper = true;
			this.allowPrinter = true;
			this.MinMargins = null;
			this.pageSettings = null;
			this.printDocument = null;
			this.printerSettings = null;
			this.showHelp = false;
			this.showNetwork = true;
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x00136B5F File Offset: 0x00134D5F
		private void ResetMinMargins()
		{
			this.MinMargins = null;
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00136B68 File Offset: 0x00134D68
		private bool ShouldSerializeMinMargins()
		{
			return this.minMargins.Left != 0 || this.minMargins.Right != 0 || this.minMargins.Top != 0 || this.minMargins.Bottom != 0;
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00136BA4 File Offset: 0x00134DA4
		private static void UpdateSettings(NativeMethods.PAGESETUPDLG data, PageSettings pageSettings, PrinterSettings printerSettings)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				pageSettings.SetHdevmode(data.hDevMode);
				if (printerSettings != null)
				{
					printerSettings.SetHdevmode(data.hDevMode);
					printerSettings.SetHdevnames(data.hDevNames);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			Margins margins = new Margins();
			margins.Left = data.marginLeft;
			margins.Top = data.marginTop;
			margins.Right = data.marginRight;
			margins.Bottom = data.marginBottom;
			PrinterUnit fromUnit = ((data.Flags & 8) != 0) ? PrinterUnit.HundredthsOfAMillimeter : PrinterUnit.ThousandthsOfAnInch;
			pageSettings.Margins = PrinterUnitConvert.Convert(margins, fromUnit, PrinterUnit.Display);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x00136C50 File Offset: 0x00134E50
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			IntSecurity.SafePrinting.Demand();
			NativeMethods.WndProc lpfnPageSetupHook = new NativeMethods.WndProc(this.HookProc);
			if (this.pageSettings == null)
			{
				throw new ArgumentException(SR.GetString("PSDcantShowWithoutPage"));
			}
			NativeMethods.PAGESETUPDLG pagesetupdlg = new NativeMethods.PAGESETUPDLG();
			pagesetupdlg.lStructSize = Marshal.SizeOf(pagesetupdlg);
			pagesetupdlg.Flags = this.GetFlags();
			pagesetupdlg.hwndOwner = hwndOwner;
			pagesetupdlg.lpfnPageSetupHook = lpfnPageSetupHook;
			PrinterUnit toUnit = PrinterUnit.ThousandthsOfAnInch;
			if (this.EnableMetric)
			{
				StringBuilder stringBuilder = new StringBuilder(2);
				int localeInfo = UnsafeNativeMethods.GetLocaleInfo(NativeMethods.LOCALE_USER_DEFAULT, 13, stringBuilder, stringBuilder.Capacity);
				if (localeInfo > 0 && int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture) == 0)
				{
					toUnit = PrinterUnit.HundredthsOfAMillimeter;
				}
			}
			if (this.MinMargins != null)
			{
				Margins margins = PrinterUnitConvert.Convert(this.MinMargins, PrinterUnit.Display, toUnit);
				pagesetupdlg.minMarginLeft = margins.Left;
				pagesetupdlg.minMarginTop = margins.Top;
				pagesetupdlg.minMarginRight = margins.Right;
				pagesetupdlg.minMarginBottom = margins.Bottom;
			}
			if (this.pageSettings.Margins != null)
			{
				Margins margins2 = PrinterUnitConvert.Convert(this.pageSettings.Margins, PrinterUnit.Display, toUnit);
				pagesetupdlg.marginLeft = margins2.Left;
				pagesetupdlg.marginTop = margins2.Top;
				pagesetupdlg.marginRight = margins2.Right;
				pagesetupdlg.marginBottom = margins2.Bottom;
			}
			pagesetupdlg.marginLeft = Math.Max(pagesetupdlg.marginLeft, pagesetupdlg.minMarginLeft);
			pagesetupdlg.marginTop = Math.Max(pagesetupdlg.marginTop, pagesetupdlg.minMarginTop);
			pagesetupdlg.marginRight = Math.Max(pagesetupdlg.marginRight, pagesetupdlg.minMarginRight);
			pagesetupdlg.marginBottom = Math.Max(pagesetupdlg.marginBottom, pagesetupdlg.minMarginBottom);
			PrinterSettings printerSettings = (this.printerSettings == null) ? this.pageSettings.PrinterSettings : this.printerSettings;
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			try
			{
				pagesetupdlg.hDevMode = printerSettings.GetHdevmode(this.pageSettings);
				pagesetupdlg.hDevNames = printerSettings.GetHdevnames();
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool result;
			try
			{
				if (!UnsafeNativeMethods.PageSetupDlg(pagesetupdlg))
				{
					result = false;
				}
				else
				{
					PageSetupDialog.UpdateSettings(pagesetupdlg, this.pageSettings, this.printerSettings);
					result = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.GlobalFree(new HandleRef(pagesetupdlg, pagesetupdlg.hDevMode));
				UnsafeNativeMethods.GlobalFree(new HandleRef(pagesetupdlg, pagesetupdlg.hDevNames));
			}
			return result;
		}

		// Token: 0x04002786 RID: 10118
		private PrintDocument printDocument;

		// Token: 0x04002787 RID: 10119
		private PageSettings pageSettings;

		// Token: 0x04002788 RID: 10120
		private PrinterSettings printerSettings;

		// Token: 0x04002789 RID: 10121
		private bool allowMargins;

		// Token: 0x0400278A RID: 10122
		private bool allowOrientation;

		// Token: 0x0400278B RID: 10123
		private bool allowPaper;

		// Token: 0x0400278C RID: 10124
		private bool allowPrinter;

		// Token: 0x0400278D RID: 10125
		private Margins minMargins;

		// Token: 0x0400278E RID: 10126
		private bool showHelp;

		// Token: 0x0400278F RID: 10127
		private bool showNetwork;

		// Token: 0x04002790 RID: 10128
		private bool enableMetric;
	}
}
