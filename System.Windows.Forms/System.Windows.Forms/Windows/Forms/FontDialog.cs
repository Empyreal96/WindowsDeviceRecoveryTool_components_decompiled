using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Prompts the user to choose a font from among those installed on the local computer.</summary>
	// Token: 0x0200024B RID: 587
	[DefaultEvent("Apply")]
	[DefaultProperty("Font")]
	[SRDescription("DescriptionFontDialog")]
	public class FontDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.FontDialog" /> class.</summary>
		// Token: 0x06002286 RID: 8838 RVA: 0x000A77FF File Offset: 0x000A59FF
		public FontDialog()
		{
			this.Reset();
		}

		/// <summary>Gets or sets a value indicating whether the dialog box allows graphics device interface (GDI) font simulations.</summary>
		/// <returns>
		///     <see langword="true" /> if font simulations are allowed; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x000A7B85 File Offset: 0x000A5D85
		// (set) Token: 0x06002288 RID: 8840 RVA: 0x000A7B95 File Offset: 0x000A5D95
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FnDallowSimulationsDescr")]
		public bool AllowSimulations
		{
			get
			{
				return !this.GetOption(4096);
			}
			set
			{
				this.SetOption(4096, !value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box allows vector font selections.</summary>
		/// <returns>
		///     <see langword="true" /> if vector fonts are allowed; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002289 RID: 8841 RVA: 0x000A7BA6 File Offset: 0x000A5DA6
		// (set) Token: 0x0600228A RID: 8842 RVA: 0x000A7BB6 File Offset: 0x000A5DB6
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FnDallowVectorFontsDescr")]
		public bool AllowVectorFonts
		{
			get
			{
				return !this.GetOption(2048);
			}
			set
			{
				this.SetOption(2048, !value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays both vertical and horizontal fonts or only horizontal fonts.</summary>
		/// <returns>
		///     <see langword="true" /> if both vertical and horizontal fonts are allowed; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x000A7BC7 File Offset: 0x000A5DC7
		// (set) Token: 0x0600228C RID: 8844 RVA: 0x000A7BD7 File Offset: 0x000A5DD7
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FnDallowVerticalFontsDescr")]
		public bool AllowVerticalFonts
		{
			get
			{
				return !this.GetOption(16777216);
			}
			set
			{
				this.SetOption(16777216, !value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can change the character set specified in the Script combo box to display a character set other than the one currently displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can change the character set specified in the Script combo box; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x000A7BE8 File Offset: 0x000A5DE8
		// (set) Token: 0x0600228E RID: 8846 RVA: 0x000A7BF8 File Offset: 0x000A5DF8
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FnDallowScriptChangeDescr")]
		public bool AllowScriptChange
		{
			get
			{
				return !this.GetOption(4194304);
			}
			set
			{
				this.SetOption(4194304, !value);
			}
		}

		/// <summary>Gets or sets the selected font color.</summary>
		/// <returns>The color of the selected font. The default value is <see cref="P:System.Drawing.Color.Black" />.</returns>
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x000A7C09 File Offset: 0x000A5E09
		// (set) Token: 0x06002290 RID: 8848 RVA: 0x000A7C2A File Offset: 0x000A5E2A
		[SRCategory("CatData")]
		[SRDescription("FnDcolorDescr")]
		[DefaultValue(typeof(Color), "Black")]
		public Color Color
		{
			get
			{
				if (this.usingDefaultIndirectColor)
				{
					return ColorTranslator.FromWin32(ColorTranslator.ToWin32(this.color));
				}
				return this.color;
			}
			set
			{
				if (!value.IsEmpty)
				{
					this.color = value;
					this.usingDefaultIndirectColor = false;
					return;
				}
				this.color = SystemColors.ControlText;
				this.usingDefaultIndirectColor = true;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box allows only the selection of fixed-pitch fonts.</summary>
		/// <returns>
		///     <see langword="true" /> if only fixed-pitch fonts can be selected; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x000A7C56 File Offset: 0x000A5E56
		// (set) Token: 0x06002292 RID: 8850 RVA: 0x000A7C63 File Offset: 0x000A5E63
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDfixedPitchOnlyDescr")]
		public bool FixedPitchOnly
		{
			get
			{
				return this.GetOption(16384);
			}
			set
			{
				this.SetOption(16384, value);
			}
		}

		/// <summary>Gets or sets the selected font.</summary>
		/// <returns>The selected font.</returns>
		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x000A7C74 File Offset: 0x000A5E74
		// (set) Token: 0x06002294 RID: 8852 RVA: 0x000A7CF1 File Offset: 0x000A5EF1
		[SRCategory("CatData")]
		[SRDescription("FnDfontDescr")]
		public Font Font
		{
			get
			{
				Font font = this.font;
				if (font == null)
				{
					font = Control.DefaultFont;
				}
				float sizeInPoints = font.SizeInPoints;
				if (this.minSize != 0 && sizeInPoints < (float)this.MinSize)
				{
					font = new Font(font.FontFamily, (float)this.MinSize, font.Style, GraphicsUnit.Point);
				}
				if (this.maxSize != 0 && sizeInPoints > (float)this.MaxSize)
				{
					font = new Font(font.FontFamily, (float)this.MaxSize, font.Style, GraphicsUnit.Point);
				}
				return font;
			}
			set
			{
				this.font = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box specifies an error condition if the user attempts to select a font or style that does not exist.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box specifies an error condition when the user tries to select a font or style that does not exist; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x000A7CFA File Offset: 0x000A5EFA
		// (set) Token: 0x06002296 RID: 8854 RVA: 0x000A7D07 File Offset: 0x000A5F07
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDfontMustExistDescr")]
		public bool FontMustExist
		{
			get
			{
				return this.GetOption(65536);
			}
			set
			{
				this.SetOption(65536, value);
			}
		}

		/// <summary>Gets or sets the maximum point size a user can select.</summary>
		/// <returns>The maximum point size a user can select. The default is 0.</returns>
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000A7D15 File Offset: 0x000A5F15
		// (set) Token: 0x06002298 RID: 8856 RVA: 0x000A7D1D File Offset: 0x000A5F1D
		[SRCategory("CatData")]
		[DefaultValue(0)]
		[SRDescription("FnDmaxSizeDescr")]
		public int MaxSize
		{
			get
			{
				return this.maxSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.maxSize = value;
				if (this.maxSize > 0 && this.maxSize < this.minSize)
				{
					this.minSize = this.maxSize;
				}
			}
		}

		/// <summary>Gets or sets the minimum point size a user can select.</summary>
		/// <returns>The minimum point size a user can select. The default is 0.</returns>
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x000A7D50 File Offset: 0x000A5F50
		// (set) Token: 0x0600229A RID: 8858 RVA: 0x000A7D58 File Offset: 0x000A5F58
		[SRCategory("CatData")]
		[DefaultValue(0)]
		[SRDescription("FnDminSizeDescr")]
		public int MinSize
		{
			get
			{
				return this.minSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minSize = value;
				if (this.maxSize > 0 && this.maxSize < this.minSize)
				{
					this.maxSize = this.minSize;
				}
			}
		}

		/// <summary>Gets values to initialize the <see cref="T:System.Windows.Forms.FontDialog" />.</summary>
		/// <returns>A bitwise combination of internal values that initializes the <see cref="T:System.Windows.Forms.FontDialog" />.</returns>
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000A7D8B File Offset: 0x000A5F8B
		protected int Options
		{
			get
			{
				return this.options;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box allows selection of fonts for all non-OEM and Symbol character sets, as well as the ANSI character set.</summary>
		/// <returns>
		///     <see langword="true" /> if selection of fonts for all non-OEM and Symbol character sets, as well as the ANSI character set, is allowed; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000A7D93 File Offset: 0x000A5F93
		// (set) Token: 0x0600229D RID: 8861 RVA: 0x000A7DA0 File Offset: 0x000A5FA0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDscriptsOnlyDescr")]
		public bool ScriptsOnly
		{
			get
			{
				return this.GetOption(1024);
			}
			set
			{
				this.SetOption(1024, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box contains an Apply button.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box contains an Apply button; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000A7DAE File Offset: 0x000A5FAE
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x000A7DBB File Offset: 0x000A5FBB
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDshowApplyDescr")]
		public bool ShowApply
		{
			get
			{
				return this.GetOption(512);
			}
			set
			{
				this.SetOption(512, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays the color choice.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays the color choice; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000A7DC9 File Offset: 0x000A5FC9
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x000A7DD1 File Offset: 0x000A5FD1
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDshowColorDescr")]
		public bool ShowColor
		{
			get
			{
				return this.showColor;
			}
			set
			{
				this.showColor = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box contains controls that allow the user to specify strikethrough, underline, and text color options.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box contains controls to set strikethrough, underline, and text color options; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000A7DDA File Offset: 0x000A5FDA
		// (set) Token: 0x060022A3 RID: 8867 RVA: 0x000A7DE7 File Offset: 0x000A5FE7
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("FnDshowEffectsDescr")]
		public bool ShowEffects
		{
			get
			{
				return this.GetOption(256);
			}
			set
			{
				this.SetOption(256, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays a Help button.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays a Help button; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000A7DF5 File Offset: 0x000A5FF5
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x000A7DFE File Offset: 0x000A5FFE
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDshowHelpDescr")]
		public bool ShowHelp
		{
			get
			{
				return this.GetOption(4);
			}
			set
			{
				this.SetOption(4, value);
			}
		}

		/// <summary>Occurs when the user clicks the Apply button in the font dialog box.</summary>
		// Token: 0x14000181 RID: 385
		// (add) Token: 0x060022A6 RID: 8870 RVA: 0x000A7E08 File Offset: 0x000A6008
		// (remove) Token: 0x060022A7 RID: 8871 RVA: 0x000A7E1B File Offset: 0x000A601B
		[SRDescription("FnDapplyDescr")]
		public event EventHandler Apply
		{
			add
			{
				base.Events.AddHandler(FontDialog.EventApply, value);
			}
			remove
			{
				base.Events.RemoveHandler(FontDialog.EventApply, value);
			}
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000A7E2E File Offset: 0x000A602E
		internal bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		/// <summary>Specifies the common dialog box hook procedure that is overridden to add specific functionality to a common dialog box.</summary>
		/// <param name="hWnd">The handle to the dialog box window. </param>
		/// <param name="msg">The message being received. </param>
		/// <param name="wparam">Additional information about the message. </param>
		/// <param name="lparam">Additional information about the message. </param>
		/// <returns>A zero value if the default dialog box procedure processes the message; a nonzero value if the default dialog box procedure ignores the message.</returns>
		// Token: 0x060022A9 RID: 8873 RVA: 0x000A7E3C File Offset: 0x000A603C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			if (msg != 272)
			{
				if (msg != 273 || (int)wparam != 1026)
				{
					goto IL_110;
				}
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				UnsafeNativeMethods.SendMessage(new HandleRef(null, hWnd), 1025, 0, logfont);
				this.UpdateFont(logfont);
				int num = (int)UnsafeNativeMethods.SendDlgItemMessage(new HandleRef(null, hWnd), 1139, 327, IntPtr.Zero, IntPtr.Zero);
				if (num != -1)
				{
					this.UpdateColor((int)UnsafeNativeMethods.SendDlgItemMessage(new HandleRef(null, hWnd), 1139, 336, (IntPtr)num, IntPtr.Zero));
				}
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					this.OnApply(EventArgs.Empty);
					goto IL_110;
				}
				try
				{
					this.OnApply(EventArgs.Empty);
					goto IL_110;
				}
				catch (Exception t)
				{
					Application.OnThreadException(t);
					goto IL_110;
				}
			}
			if (!this.showColor)
			{
				IntPtr dlgItem = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, hWnd), 1139);
				SafeNativeMethods.ShowWindow(new HandleRef(null, dlgItem), 0);
				dlgItem = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, hWnd), 1091);
				SafeNativeMethods.ShowWindow(new HandleRef(null, dlgItem), 0);
			}
			IL_110:
			return base.HookProc(hWnd, msg, wparam, lparam);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.FontDialog.Apply" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the data. </param>
		// Token: 0x060022AA RID: 8874 RVA: 0x000A7F74 File Offset: 0x000A6174
		protected virtual void OnApply(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[FontDialog.EventApply];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Resets all dialog box options to their default values.</summary>
		// Token: 0x060022AB RID: 8875 RVA: 0x000A7FA4 File Offset: 0x000A61A4
		public override void Reset()
		{
			this.options = 257;
			this.font = null;
			this.color = SystemColors.ControlText;
			this.usingDefaultIndirectColor = true;
			this.showColor = false;
			this.minSize = 0;
			this.maxSize = 0;
			this.SetOption(262144, true);
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000A7FF6 File Offset: 0x000A61F6
		private void ResetFont()
		{
			this.font = null;
		}

		/// <summary>Specifies a file dialog box.</summary>
		/// <param name="hWndOwner">The window handle of the owner window for the common dialog box.</param>
		/// <returns>
		///     <see langword="true" /> if the dialog box was successfully run; otherwise, <see langword="false" />.</returns>
		// Token: 0x060022AD RID: 8877 RVA: 0x000A8000 File Offset: 0x000A6200
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			NativeMethods.WndProc lpfnHook = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.CHOOSEFONT choosefont = new NativeMethods.CHOOSEFONT();
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			Graphics graphics = Graphics.FromHdcInternal(dc);
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				this.Font.ToLogFont(logfont, graphics);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				graphics.Dispose();
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			IntPtr intPtr = IntPtr.Zero;
			bool result;
			try
			{
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(NativeMethods.LOGFONT)));
				Marshal.StructureToPtr(logfont, intPtr, false);
				choosefont.lStructSize = Marshal.SizeOf(typeof(NativeMethods.CHOOSEFONT));
				choosefont.hwndOwner = hWndOwner;
				choosefont.hDC = IntPtr.Zero;
				choosefont.lpLogFont = intPtr;
				choosefont.Flags = (this.Options | 64 | 8);
				if (this.minSize > 0 || this.maxSize > 0)
				{
					choosefont.Flags |= 8192;
				}
				if (this.ShowColor || this.ShowEffects)
				{
					choosefont.rgbColors = ColorTranslator.ToWin32(this.color);
				}
				else
				{
					choosefont.rgbColors = ColorTranslator.ToWin32(SystemColors.ControlText);
				}
				choosefont.lpfnHook = lpfnHook;
				choosefont.hInstance = UnsafeNativeMethods.GetModuleHandle(null);
				choosefont.nSizeMin = this.minSize;
				if (this.maxSize == 0)
				{
					choosefont.nSizeMax = int.MaxValue;
				}
				else
				{
					choosefont.nSizeMax = this.maxSize;
				}
				if (!SafeNativeMethods.ChooseFont(choosefont))
				{
					result = false;
				}
				else
				{
					NativeMethods.LOGFONT logfont2 = (NativeMethods.LOGFONT)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(NativeMethods.LOGFONT));
					if (logfont2.lfFaceName != null && logfont2.lfFaceName.Length > 0)
					{
						logfont = logfont2;
						this.UpdateFont(logfont);
						this.UpdateColor(choosefont.rgbColors);
					}
					result = true;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return result;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000A8218 File Offset: 0x000A6418
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000A823B File Offset: 0x000A643B
		private bool ShouldSerializeFont()
		{
			return !this.Font.Equals(Control.DefaultFont);
		}

		/// <summary>Retrieves a string that includes the name of the current font selected in the dialog box.</summary>
		/// <returns>A string that includes the name of the currently selected font.</returns>
		// Token: 0x060022B0 RID: 8880 RVA: 0x000A8250 File Offset: 0x000A6450
		public override string ToString()
		{
			string str = base.ToString();
			return str + ",  Font: " + this.Font.ToString();
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000A827A File Offset: 0x000A647A
		private void UpdateColor(int rgb)
		{
			if (ColorTranslator.ToWin32(this.color) != rgb)
			{
				this.color = ColorTranslator.FromOle(rgb);
				this.usingDefaultIndirectColor = false;
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000A82A0 File Offset: 0x000A64A0
		private void UpdateFont(NativeMethods.LOGFONT lf)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			try
			{
				Font font = null;
				try
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						font = Font.FromLogFont(lf, dc);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.font = ControlPaint.FontInPoints(font);
				}
				finally
				{
					if (font != null)
					{
						font.Dispose();
					}
				}
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
		}

		/// <summary>Owns the <see cref="E:System.Windows.Forms.FontDialog.Apply" /> event.</summary>
		// Token: 0x04000F04 RID: 3844
		protected static readonly object EventApply = new object();

		// Token: 0x04000F05 RID: 3845
		private const int defaultMinSize = 0;

		// Token: 0x04000F06 RID: 3846
		private const int defaultMaxSize = 0;

		// Token: 0x04000F07 RID: 3847
		private int options;

		// Token: 0x04000F08 RID: 3848
		private Font font;

		// Token: 0x04000F09 RID: 3849
		private Color color;

		// Token: 0x04000F0A RID: 3850
		private int minSize;

		// Token: 0x04000F0B RID: 3851
		private int maxSize;

		// Token: 0x04000F0C RID: 3852
		private bool showColor;

		// Token: 0x04000F0D RID: 3853
		private bool usingDefaultIndirectColor;
	}
}
