using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides colors used for Microsoft Office display elements.</summary>
	// Token: 0x02000316 RID: 790
	public class ProfessionalColorTable
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06002FED RID: 12269 RVA: 0x000DB664 File Offset: 0x000D9864
		private Dictionary<ProfessionalColorTable.KnownColors, Color> ColorTable
		{
			get
			{
				if (this.UseSystemColors)
				{
					if (!this.usingSystemColors || this.professionalRGB == null)
					{
						if (this.professionalRGB == null)
						{
							this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
						}
						this.InitSystemColors(ref this.professionalRGB);
					}
				}
				else if (ToolStripManager.VisualStylesEnabled)
				{
					if (this.usingSystemColors || this.professionalRGB == null)
					{
						if (this.professionalRGB == null)
						{
							this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
						}
						this.InitThemedColors(ref this.professionalRGB);
					}
				}
				else if (!this.usingSystemColors || this.professionalRGB == null)
				{
					if (this.professionalRGB == null)
					{
						this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
					}
					this.InitSystemColors(ref this.professionalRGB);
				}
				return this.professionalRGB;
			}
		}

		/// <summary>Gets or sets a value indicating whether to use <see cref="T:System.Drawing.SystemColors" /> rather than colors that match the current visual style. </summary>
		/// <returns>
		///     <see langword="true" /> to use <see cref="T:System.Drawing.SystemColors" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x000DB729 File Offset: 0x000D9929
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x000DB731 File Offset: 0x000D9931
		public bool UseSystemColors
		{
			get
			{
				return this.useSystemColors;
			}
			set
			{
				if (this.useSystemColors != value)
				{
					this.useSystemColors = value;
					this.ResetRGBTable();
				}
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000DB74C File Offset: 0x000D994C
		internal Color FromKnownColor(ProfessionalColorTable.KnownColors color)
		{
			if (ProfessionalColors.ColorFreshnessKey != this.colorFreshnessKey || ProfessionalColors.ColorScheme != this.lastKnownColorScheme)
			{
				this.ResetRGBTable();
			}
			this.colorFreshnessKey = ProfessionalColors.ColorFreshnessKey;
			this.lastKnownColorScheme = ProfessionalColors.ColorScheme;
			return this.ColorTable[color];
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000DB7A0 File Offset: 0x000D99A0
		private void ResetRGBTable()
		{
			if (this.professionalRGB != null)
			{
				this.professionalRGB.Clear();
			}
			this.professionalRGB = null;
		}

		/// <summary>Gets the solid color used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is selected.</returns>
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x000DB7BC File Offset: 0x000D99BC
		[SRDescription("ProfessionalColorsButtonSelectedHighlightDescr")]
		public virtual Color ButtonSelectedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonSelectedHighlight);
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight" />.</returns>
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x000DB7C9 File Offset: 0x000D99C9
		[SRDescription("ProfessionalColorsButtonSelectedHighlightBorderDescr")]
		public virtual Color ButtonSelectedHighlightBorder
		{
			get
			{
				return this.ButtonPressedBorder;
			}
		}

		/// <summary>Gets the solid color used when the button is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is pressed.</returns>
		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000DB7D1 File Offset: 0x000D99D1
		[SRDescription("ProfessionalColorsButtonPressedHighlightDescr")]
		public virtual Color ButtonPressedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonPressedHighlight);
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight" />.</returns>
		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x000DB7DE File Offset: 0x000D99DE
		[SRDescription("ProfessionalColorsButtonPressedHighlightBorderDescr")]
		public virtual Color ButtonPressedHighlightBorder
		{
			get
			{
				return SystemColors.Highlight;
			}
		}

		/// <summary>Gets the solid color used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is checked.</returns>
		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x000DB7E5 File Offset: 0x000D99E5
		[SRDescription("ProfessionalColorsButtonCheckedHighlightDescr")]
		public virtual Color ButtonCheckedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonCheckedHighlight);
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight" />.</returns>
		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x000DB7DE File Offset: 0x000D99DE
		[SRDescription("ProfessionalColorsButtonCheckedHighlightBorderDescr")]
		public virtual Color ButtonCheckedHighlightBorder
		{
			get
			{
				return SystemColors.Highlight;
			}
		}

		/// <summary>Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd" /> colors.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd" /> colors.</returns>
		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x000DB7F2 File Offset: 0x000D99F2
		[SRDescription("ProfessionalColorsButtonPressedBorderDescr")]
		public virtual Color ButtonPressedBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
			}
		}

		/// <summary>Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd" /> colors.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd" /> colors.</returns>
		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x000DB7F2 File Offset: 0x000D99F2
		[SRDescription("ProfessionalColorsButtonSelectedBorderDescr")]
		public virtual Color ButtonSelectedBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the button is checked.</returns>
		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x000DB7FB File Offset: 0x000D99FB
		[SRDescription("ProfessionalColorsButtonCheckedGradientBeginDescr")]
		public virtual Color ButtonCheckedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is checked.</returns>
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x000DB805 File Offset: 0x000D9A05
		[SRDescription("ProfessionalColorsButtonCheckedGradientMiddleDescr")]
		public virtual Color ButtonCheckedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is checked.</returns>
		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000DB80F File Offset: 0x000D9A0F
		[SRDescription("ProfessionalColorsButtonCheckedGradientEndDescr")]
		public virtual Color ButtonCheckedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the button is selected.</returns>
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x000DB819 File Offset: 0x000D9A19
		[SRDescription("ProfessionalColorsButtonSelectedGradientBeginDescr")]
		public virtual Color ButtonSelectedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is selected.</returns>
		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x000DB823 File Offset: 0x000D9A23
		[SRDescription("ProfessionalColorsButtonSelectedGradientMiddleDescr")]
		public virtual Color ButtonSelectedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is selected.</returns>
		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000DB82D File Offset: 0x000D9A2D
		[SRDescription("ProfessionalColorsButtonSelectedGradientEndDescr")]
		public virtual Color ButtonSelectedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the button is pressed.</returns>
		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x000DB837 File Offset: 0x000D9A37
		[SRDescription("ProfessionalColorsButtonPressedGradientBeginDescr")]
		public virtual Color ButtonPressedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is pressed.</returns>
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003001 RID: 12289 RVA: 0x000DB841 File Offset: 0x000D9A41
		[SRDescription("ProfessionalColorsButtonPressedGradientMiddleDescr")]
		public virtual Color ButtonPressedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is pressed.</returns>
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x000DB84B File Offset: 0x000D9A4B
		[SRDescription("ProfessionalColorsButtonPressedGradientEndDescr")]
		public virtual Color ButtonPressedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd);
			}
		}

		/// <summary>Gets the solid color to use when the button is checked and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the button is checked and gradients are being used.</returns>
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x000DB855 File Offset: 0x000D9A55
		[SRDescription("ProfessionalColorsCheckBackgroundDescr")]
		public virtual Color CheckBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected);
			}
		}

		/// <summary>Gets the solid color to use when the button is checked and selected and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x000DB85F File Offset: 0x000D9A5F
		[SRDescription("ProfessionalColorsCheckSelectedBackgroundDescr")]
		public virtual Color CheckSelectedBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
			}
		}

		/// <summary>Gets the solid color to use when the button is checked and selected and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x000DB85F File Offset: 0x000D9A5F
		[SRDescription("ProfessionalColorsCheckPressedBackgroundDescr")]
		public virtual Color CheckPressedBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
			}
		}

		/// <summary>Gets the color to use for shadow effects on the grip (move handle).</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use for shadow effects on the grip (move handle).</returns>
		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000DB869 File Offset: 0x000D9A69
		[SRDescription("ProfessionalColorsGripDarkDescr")]
		public virtual Color GripDark
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle);
			}
		}

		/// <summary>Gets the color to use for highlight effects on the grip (move handle).</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use for highlight effects on the grip (move handle).</returns>
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x000DB873 File Offset: 0x000D9A73
		[SRDescription("ProfessionalColorsGripLightDescr")]
		public virtual Color GripLight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x000DB87D File Offset: 0x000D9A7D
		[SRDescription("ProfessionalColorsImageMarginGradientBeginDescr")]
		public virtual Color ImageMarginGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x000DB887 File Offset: 0x000D9A87
		[SRDescription("ProfessionalColorsImageMarginGradientMiddleDescr")]
		public virtual Color ImageMarginGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x000DB891 File Offset: 0x000D9A91
		[SRDescription("ProfessionalColorsImageMarginGradientEndDescr")]
		public virtual Color ImageMarginGradientEnd
		{
			get
			{
				if (!this.usingSystemColors)
				{
					return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd);
				}
				return SystemColors.Control;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000DB8A9 File Offset: 0x000D9AA9
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientBeginDescr")]
		public virtual Color ImageMarginRevealedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x000DB8B3 File Offset: 0x000D9AB3
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientMiddleDescr")]
		public virtual Color ImageMarginRevealedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x000DB8BD File Offset: 0x000D9ABD
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientEndDescr")]
		public virtual Color ImageMarginRevealedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000DB8C7 File Offset: 0x000D9AC7
		[SRDescription("ProfessionalColorsMenuStripGradientBeginDescr")]
		public virtual Color MenuStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x000DB8D1 File Offset: 0x000D9AD1
		[SRDescription("ProfessionalColorsMenuStripGradientEndDescr")]
		public virtual Color MenuStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		/// <summary>Gets the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x000DB8DB File Offset: 0x000D9ADB
		[SRDescription("ProfessionalColorsMenuItemSelectedDescr")]
		public virtual Color MenuItemSelected
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver);
			}
		}

		/// <summary>Gets the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</returns>
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000DB8E5 File Offset: 0x000D9AE5
		[SRDescription("ProfessionalColorsMenuItemBorderDescr")]
		public virtual Color MenuItemBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected);
			}
		}

		/// <summary>Gets the color that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000DB8EE File Offset: 0x000D9AEE
		[SRDescription("ProfessionalColorsMenuBorderDescr")]
		public virtual Color MenuBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter);
			}
		}

		/// <summary>Gets the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x000DB819 File Offset: 0x000D9A19
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientBeginDescr")]
		public virtual Color MenuItemSelectedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000DB82D File Offset: 0x000D9A2D
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientEndDescr")]
		public virtual Color MenuItemSelectedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</returns>
		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x000DB8F8 File Offset: 0x000D9AF8
		[SRDescription("ProfessionalColorsMenuItemPressedGradientBeginDescr")]
		public virtual Color MenuItemPressedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</returns>
		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x000DB8B3 File Offset: 0x000D9AB3
		[SRDescription("ProfessionalColorsMenuItemPressedGradientMiddleDescr")]
		public virtual Color MenuItemPressedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed.</returns>
		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x000DB902 File Offset: 0x000D9B02
		[SRDescription("ProfessionalColorsMenuItemPressedGradientEndDescr")]
		public virtual Color MenuItemPressedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x000DB8C7 File Offset: 0x000D9AC7
		[SRDescription("ProfessionalColorsRaftingContainerGradientBeginDescr")]
		public virtual Color RaftingContainerGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000DB8D1 File Offset: 0x000D9AD1
		[SRDescription("ProfessionalColorsRaftingContainerGradientEndDescr")]
		public virtual Color RaftingContainerGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		/// <summary>Gets the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x000DB90C File Offset: 0x000D9B0C
		[SRDescription("ProfessionalColorsSeparatorDarkDescr")]
		public virtual Color SeparatorDark
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine);
			}
		}

		/// <summary>Gets the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x000DB916 File Offset: 0x000D9B16
		[SRDescription("ProfessionalColorsSeparatorLightDescr")]
		public virtual Color SeparatorLight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight);
			}
		}

		/// <summary>Gets the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</returns>
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000DB8C7 File Offset: 0x000D9AC7
		[SRDescription("ProfessionalColorsStatusStripGradientBeginDescr")]
		public virtual Color StatusStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</returns>
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000DB8D1 File Offset: 0x000D9AD1
		[SRDescription("ProfessionalColorsStatusStripGradientEndDescr")]
		public virtual Color StatusStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		/// <summary>Gets the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000DB920 File Offset: 0x000D9B20
		[SRDescription("ProfessionalColorsToolStripBorderDescr")]
		public virtual Color ToolStripBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBShadow);
			}
		}

		/// <summary>Gets the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</returns>
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000DB92A File Offset: 0x000D9B2A
		[SRDescription("ProfessionalColorsToolStripDropDownBackgroundDescr")]
		public virtual Color ToolStripDropDownBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000DB87D File Offset: 0x000D9A7D
		[SRDescription("ProfessionalColorsToolStripGradientBeginDescr")]
		public virtual Color ToolStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000DB887 File Offset: 0x000D9A87
		[SRDescription("ProfessionalColorsToolStripGradientMiddleDescr")]
		public virtual Color ToolStripGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000DB934 File Offset: 0x000D9B34
		[SRDescription("ProfessionalColorsToolStripGradientEndDescr")]
		public virtual Color ToolStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</returns>
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000DB8C7 File Offset: 0x000D9AC7
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientBeginDescr")]
		public virtual Color ToolStripContentPanelGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</returns>
		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x000DB8D1 File Offset: 0x000D9AD1
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientEndDescr")]
		public virtual Color ToolStripContentPanelGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</returns>
		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000DB8C7 File Offset: 0x000D9AC7
		[SRDescription("ProfessionalColorsToolStripPanelGradientBeginDescr")]
		public virtual Color ToolStripPanelGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</returns>
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000DB8D1 File Offset: 0x000D9AD1
		[SRDescription("ProfessionalColorsToolStripPanelGradientEndDescr")]
		public virtual Color ToolStripPanelGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000DB93E File Offset: 0x000D9B3E
		[SRDescription("ProfessionalColorsOverflowButtonGradientBeginDescr")]
		public virtual Color OverflowButtonGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin);
			}
		}

		/// <summary>Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x000DB948 File Offset: 0x000D9B48
		[SRDescription("ProfessionalColorsOverflowButtonGradientMiddleDescr")]
		public virtual Color OverflowButtonGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle);
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x000DB952 File Offset: 0x000D9B52
		[SRDescription("ProfessionalColorsOverflowButtonGradientEndDescr")]
		public virtual Color OverflowButtonGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd);
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000DB95C File Offset: 0x000D9B5C
		internal Color ComboBoxButtonGradientBegin
		{
			get
			{
				return this.MenuItemPressedGradientBegin;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x000DB964 File Offset: 0x000D9B64
		internal Color ComboBoxButtonGradientEnd
		{
			get
			{
				return this.MenuItemPressedGradientEnd;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x000DB96C File Offset: 0x000D9B6C
		internal Color ComboBoxButtonSelectedGradientBegin
		{
			get
			{
				return this.MenuItemSelectedGradientBegin;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x000DB974 File Offset: 0x000D9B74
		internal Color ComboBoxButtonSelectedGradientEnd
		{
			get
			{
				return this.MenuItemSelectedGradientEnd;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x000DB97C File Offset: 0x000D9B7C
		internal Color ComboBoxButtonPressedGradientBegin
		{
			get
			{
				return this.ButtonPressedGradientBegin;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600302F RID: 12335 RVA: 0x000DB984 File Offset: 0x000D9B84
		internal Color ComboBoxButtonPressedGradientEnd
		{
			get
			{
				return this.ButtonPressedGradientEnd;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000DB98C File Offset: 0x000D9B8C
		internal Color ComboBoxButtonOnOverflow
		{
			get
			{
				return this.ToolStripDropDownBackground;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x000DB994 File Offset: 0x000D9B94
		internal Color ComboBoxBorder
		{
			get
			{
				return this.ButtonSelectedHighlightBorder;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x000DB994 File Offset: 0x000D9B94
		internal Color TextBoxBorder
		{
			get
			{
				return this.ButtonSelectedHighlightBorder;
			}
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x000DB99C File Offset: 0x000D9B9C
		private static Color GetAlphaBlendedColor(Graphics g, Color src, Color dest, int alpha)
		{
			int red = ((int)src.R * alpha + (255 - alpha) * (int)dest.R) / 255;
			int green = ((int)src.G * alpha + (255 - alpha) * (int)dest.G) / 255;
			int blue = ((int)src.B * alpha + (255 - alpha) * (int)dest.B) / 255;
			int alpha2 = ((int)src.A * alpha + (255 - alpha) * (int)dest.A) / 255;
			if (g == null)
			{
				return Color.FromArgb(alpha2, red, green, blue);
			}
			return g.GetNearestColor(Color.FromArgb(alpha2, red, green, blue));
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000DBA48 File Offset: 0x000D9C48
		private static Color GetAlphaBlendedColorHighRes(Graphics graphics, Color src, Color dest, int alpha)
		{
			int num;
			int num2;
			if (alpha < 100)
			{
				num = 100 - alpha;
				num2 = 100;
			}
			else
			{
				num = 1000 - alpha;
				num2 = 1000;
			}
			int red = (alpha * (int)src.R + num * (int)dest.R + num2 / 2) / num2;
			int green = (alpha * (int)src.G + num * (int)dest.G + num2 / 2) / num2;
			int blue = (alpha * (int)src.B + num * (int)dest.B + num2 / 2) / num2;
			if (graphics == null)
			{
				return Color.FromArgb(red, green, blue);
			}
			return graphics.GetNearestColor(Color.FromArgb(red, green, blue));
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000DBAE8 File Offset: 0x000D9CE8
		private void InitCommonColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			if (!DisplayInformation.LowResolution)
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					rgbTable[ProfessionalColorTable.KnownColors.ButtonPressedHighlight] = ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 160), 50);
					rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight] = ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 80), 20);
					rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight] = rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight];
					return;
				}
			}
			rgbTable[ProfessionalColorTable.KnownColors.ButtonPressedHighlight] = SystemColors.Highlight;
			rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight] = SystemColors.ControlLight;
			rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight] = SystemColors.ControlLight;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000DBBC8 File Offset: 0x000D9DC8
		internal void InitSystemColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			this.usingSystemColors = true;
			this.InitCommonColors(ref rgbTable);
			Color buttonFace = SystemColors.ButtonFace;
			Color buttonShadow = SystemColors.ButtonShadow;
			Color highlight = SystemColors.Highlight;
			Color window = SystemColors.Window;
			Color empty = Color.Empty;
			Color controlText = SystemColors.ControlText;
			Color buttonHighlight = SystemColors.ButtonHighlight;
			Color grayText = SystemColors.GrayText;
			Color highlightText = SystemColors.HighlightText;
			Color windowText = SystemColors.WindowText;
			Color value = buttonFace;
			Color value2 = buttonFace;
			Color value3 = buttonFace;
			Color value4 = highlight;
			Color value5 = highlight;
			bool lowResolution = DisplayInformation.LowResolution;
			bool highContrast = DisplayInformation.HighContrast;
			if (lowResolution)
			{
				value4 = window;
			}
			else if (!highContrast)
			{
				value = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 23);
				value2 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 50);
				value3 = SystemColors.ButtonFace;
				value4 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 30);
				value5 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
			}
			if (lowResolution || highContrast)
			{
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = buttonFace;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = SystemColors.ControlLight;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = controlText;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = buttonFace;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = controlText;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = window;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = buttonShadow;
			}
			else
			{
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, window, buttonFace, 165);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 75);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 205);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 40);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, controlText, buttonShadow, 20);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 143);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 70);
			}
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = (lowResolution ? SystemColors.ControlLight : highlight);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = value;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = value2;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = value3;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = value;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = value2;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd];
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = SystemColors.InactiveCaption;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = SystemColors.InactiveCaptionText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = SystemColors.Info;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = SystemColors.InfoText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = buttonFace;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000DC7F0 File Offset: 0x000DA9F0
		internal void InitOliveLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(209, 222, 173);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(209, 222, 173);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(96, 119, 66);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(230, 230, 209);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(160, 177, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(186, 201, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(237, 240, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 204, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(96, 119, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(141, 160, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(255, 255, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(206, 220, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(131, 144, 113);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(243, 244, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(159, 174, 122);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(117, 141, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(244, 244, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(216, 227, 182);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(134, 148, 108);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(244, 247, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(197, 212, 159);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(220, 224, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(153, 84, 10);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(96, 119, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(176, 194, 140);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(217, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(230, 234, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(161, 176, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(210, 223, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(243, 242, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(255, 255, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(226, 231, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(171, 192, 138);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(117, 141, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(96, 119, 66);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(175, 192, 130);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(175, 186, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(115, 137, 84);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(200, 212, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(176, 191, 138);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(64, 81, 59);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(206, 220, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 198, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(249, 249, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(237, 242, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(191, 206, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(243, 242, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(164, 185, 127);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(197, 212, 159);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(188, 187, 177);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(216, 227, 182);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(188, 205, 131);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(217, 217, 167);
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000DE0F0 File Offset: 0x000DC2F0
		internal void InitSilverLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(173, 174, 193);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(219, 218, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(219, 218, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(84, 84, 117);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(215, 215, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(118, 116, 151);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(184, 185, 202);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(232, 233, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(172, 170, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(156, 155, 180);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(249, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(147, 145, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(247, 245, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(168, 167, 190);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(198, 200, 215);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(253, 250, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(214, 211, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(154, 140, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(192, 192, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(59, 59, 63);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(7, 70, 213);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(223, 223, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(212, 213, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(227, 227, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(169, 168, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(208, 208, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(238, 238, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(249, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(223, 223, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(177, 176, 195);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(239, 239, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(165, 164, 189);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(229, 229, 235);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(112, 111, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(204, 206, 219);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(147, 145, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(195, 195, 210);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(236, 234, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(247, 247, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(239, 239, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(238, 238, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(165, 172, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(192, 192, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(161, 160, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(184, 188, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(198, 198, 217);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(215, 215, 229);
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x000DF9E0 File Offset: 0x000DDBE0
		private void InitRoyaleColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(189, 188, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(142, 141, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(176, 175, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(134, 133, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(228, 226, 230);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(176, 175, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(251, 250, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(242, 242, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(224, 224, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(247, 246, 248);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(241, 240, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(228, 226, 230);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(237, 235, 239);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(155, 154, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(188, 202, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(193, 210, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(154, 183, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(246, 244, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(152, 181, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 210, 238);
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000E1098 File Offset: 0x000DF298
		internal void InitThemedColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			string colorScheme = VisualStyleInformation.ColorScheme;
			string fileName = Path.GetFileName(VisualStyleInformation.ThemeFilename);
			bool flag = false;
			if (string.Equals("luna.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
			{
				if (colorScheme == "NormalColor")
				{
					this.InitBlueLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
				else if (colorScheme == "HomeStead")
				{
					this.InitOliveLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
				else if (colorScheme == "Metallic")
				{
					this.InitSilverLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
			}
			else if (string.Equals("aero.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
			{
				this.InitSystemColors(ref rgbTable);
				this.usingSystemColors = true;
				flag = true;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight];
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver];
			}
			else if (string.Equals("royale.msstyles", fileName, StringComparison.OrdinalIgnoreCase) && (colorScheme == "NormalColor" || colorScheme == "Royale"))
			{
				this.InitRoyaleColors(ref rgbTable);
				this.usingSystemColors = false;
				flag = true;
			}
			if (!flag)
			{
				this.InitSystemColors(ref rgbTable);
				this.usingSystemColors = true;
			}
			this.InitCommonColors(ref rgbTable);
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000E11C4 File Offset: 0x000DF3C4
		internal void InitBlueLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(39, 65, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(203, 221, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(114, 155, 215);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(161, 197, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(127, 177, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(82, 127, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(97, 122, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(233, 236, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(109, 150, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(153, 204, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(0, 45, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(246, 246, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(95, 130, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(106, 140, 203);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(241, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(169, 199, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(187, 206, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 70, 213);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(0, 53, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(117, 166, 241);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(89, 89, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(185, 208, 241);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(101, 143, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(216, 231, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(75, 120, 202);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(190, 218, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(55, 104, 185);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(215, 228, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(0, 45, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(195, 218, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(189, 194, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(251, 251, 248);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(220, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(167, 197, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(185, 212, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(127, 157, 185);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(169, 199, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(148, 187, 239);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(158, 190, 245);
		}

		// Token: 0x04001D9E RID: 7582
		private Dictionary<ProfessionalColorTable.KnownColors, Color> professionalRGB;

		// Token: 0x04001D9F RID: 7583
		private bool usingSystemColors;

		// Token: 0x04001DA0 RID: 7584
		private bool useSystemColors;

		// Token: 0x04001DA1 RID: 7585
		private string lastKnownColorScheme = string.Empty;

		// Token: 0x04001DA2 RID: 7586
		private const string oliveColorScheme = "HomeStead";

		// Token: 0x04001DA3 RID: 7587
		private const string normalColorScheme = "NormalColor";

		// Token: 0x04001DA4 RID: 7588
		private const string silverColorScheme = "Metallic";

		// Token: 0x04001DA5 RID: 7589
		private const string royaleColorScheme = "Royale";

		// Token: 0x04001DA6 RID: 7590
		private const string lunaFileName = "luna.msstyles";

		// Token: 0x04001DA7 RID: 7591
		private const string royaleFileName = "royale.msstyles";

		// Token: 0x04001DA8 RID: 7592
		private const string aeroFileName = "aero.msstyles";

		// Token: 0x04001DA9 RID: 7593
		private object colorFreshnessKey;

		// Token: 0x02000700 RID: 1792
		internal enum KnownColors
		{
			// Token: 0x04004045 RID: 16453
			msocbvcrCBBdrOuterDocked,
			// Token: 0x04004046 RID: 16454
			msocbvcrCBBdrOuterFloating,
			// Token: 0x04004047 RID: 16455
			msocbvcrCBBkgd,
			// Token: 0x04004048 RID: 16456
			msocbvcrCBCtlBdrMouseDown,
			// Token: 0x04004049 RID: 16457
			msocbvcrCBCtlBdrMouseOver,
			// Token: 0x0400404A RID: 16458
			msocbvcrCBCtlBdrSelected,
			// Token: 0x0400404B RID: 16459
			msocbvcrCBCtlBdrSelectedMouseOver,
			// Token: 0x0400404C RID: 16460
			msocbvcrCBCtlBkgd,
			// Token: 0x0400404D RID: 16461
			msocbvcrCBCtlBkgdLight,
			// Token: 0x0400404E RID: 16462
			msocbvcrCBCtlBkgdMouseDown,
			// Token: 0x0400404F RID: 16463
			msocbvcrCBCtlBkgdMouseOver,
			// Token: 0x04004050 RID: 16464
			msocbvcrCBCtlBkgdSelected,
			// Token: 0x04004051 RID: 16465
			msocbvcrCBCtlBkgdSelectedMouseOver,
			// Token: 0x04004052 RID: 16466
			msocbvcrCBCtlText,
			// Token: 0x04004053 RID: 16467
			msocbvcrCBCtlTextDisabled,
			// Token: 0x04004054 RID: 16468
			msocbvcrCBCtlTextLight,
			// Token: 0x04004055 RID: 16469
			msocbvcrCBCtlTextMouseDown,
			// Token: 0x04004056 RID: 16470
			msocbvcrCBCtlTextMouseOver,
			// Token: 0x04004057 RID: 16471
			msocbvcrCBDockSeparatorLine,
			// Token: 0x04004058 RID: 16472
			msocbvcrCBDragHandle,
			// Token: 0x04004059 RID: 16473
			msocbvcrCBDragHandleShadow,
			// Token: 0x0400405A RID: 16474
			msocbvcrCBDropDownArrow,
			// Token: 0x0400405B RID: 16475
			msocbvcrCBGradMainMenuHorzBegin,
			// Token: 0x0400405C RID: 16476
			msocbvcrCBGradMainMenuHorzEnd,
			// Token: 0x0400405D RID: 16477
			msocbvcrCBGradMenuIconBkgdDroppedBegin,
			// Token: 0x0400405E RID: 16478
			msocbvcrCBGradMenuIconBkgdDroppedEnd,
			// Token: 0x0400405F RID: 16479
			msocbvcrCBGradMenuIconBkgdDroppedMiddle,
			// Token: 0x04004060 RID: 16480
			msocbvcrCBGradMenuTitleBkgdBegin,
			// Token: 0x04004061 RID: 16481
			msocbvcrCBGradMenuTitleBkgdEnd,
			// Token: 0x04004062 RID: 16482
			msocbvcrCBGradMouseDownBegin,
			// Token: 0x04004063 RID: 16483
			msocbvcrCBGradMouseDownEnd,
			// Token: 0x04004064 RID: 16484
			msocbvcrCBGradMouseDownMiddle,
			// Token: 0x04004065 RID: 16485
			msocbvcrCBGradMouseOverBegin,
			// Token: 0x04004066 RID: 16486
			msocbvcrCBGradMouseOverEnd,
			// Token: 0x04004067 RID: 16487
			msocbvcrCBGradMouseOverMiddle,
			// Token: 0x04004068 RID: 16488
			msocbvcrCBGradOptionsBegin,
			// Token: 0x04004069 RID: 16489
			msocbvcrCBGradOptionsEnd,
			// Token: 0x0400406A RID: 16490
			msocbvcrCBGradOptionsMiddle,
			// Token: 0x0400406B RID: 16491
			msocbvcrCBGradOptionsMouseOverBegin,
			// Token: 0x0400406C RID: 16492
			msocbvcrCBGradOptionsMouseOverEnd,
			// Token: 0x0400406D RID: 16493
			msocbvcrCBGradOptionsMouseOverMiddle,
			// Token: 0x0400406E RID: 16494
			msocbvcrCBGradOptionsSelectedBegin,
			// Token: 0x0400406F RID: 16495
			msocbvcrCBGradOptionsSelectedEnd,
			// Token: 0x04004070 RID: 16496
			msocbvcrCBGradOptionsSelectedMiddle,
			// Token: 0x04004071 RID: 16497
			msocbvcrCBGradSelectedBegin,
			// Token: 0x04004072 RID: 16498
			msocbvcrCBGradSelectedEnd,
			// Token: 0x04004073 RID: 16499
			msocbvcrCBGradSelectedMiddle,
			// Token: 0x04004074 RID: 16500
			msocbvcrCBGradVertBegin,
			// Token: 0x04004075 RID: 16501
			msocbvcrCBGradVertEnd,
			// Token: 0x04004076 RID: 16502
			msocbvcrCBGradVertMiddle,
			// Token: 0x04004077 RID: 16503
			msocbvcrCBIconDisabledDark,
			// Token: 0x04004078 RID: 16504
			msocbvcrCBIconDisabledLight,
			// Token: 0x04004079 RID: 16505
			msocbvcrCBLabelBkgnd,
			// Token: 0x0400407A RID: 16506
			msocbvcrCBLowColorIconDisabled,
			// Token: 0x0400407B RID: 16507
			msocbvcrCBMainMenuBkgd,
			// Token: 0x0400407C RID: 16508
			msocbvcrCBMenuBdrOuter,
			// Token: 0x0400407D RID: 16509
			msocbvcrCBMenuBkgd,
			// Token: 0x0400407E RID: 16510
			msocbvcrCBMenuCtlText,
			// Token: 0x0400407F RID: 16511
			msocbvcrCBMenuCtlTextDisabled,
			// Token: 0x04004080 RID: 16512
			msocbvcrCBMenuIconBkgd,
			// Token: 0x04004081 RID: 16513
			msocbvcrCBMenuIconBkgdDropped,
			// Token: 0x04004082 RID: 16514
			msocbvcrCBMenuShadow,
			// Token: 0x04004083 RID: 16515
			msocbvcrCBMenuSplitArrow,
			// Token: 0x04004084 RID: 16516
			msocbvcrCBOptionsButtonShadow,
			// Token: 0x04004085 RID: 16517
			msocbvcrCBShadow,
			// Token: 0x04004086 RID: 16518
			msocbvcrCBSplitterLine,
			// Token: 0x04004087 RID: 16519
			msocbvcrCBSplitterLineLight,
			// Token: 0x04004088 RID: 16520
			msocbvcrCBTearOffHandle,
			// Token: 0x04004089 RID: 16521
			msocbvcrCBTearOffHandleMouseOver,
			// Token: 0x0400408A RID: 16522
			msocbvcrCBTitleBkgd,
			// Token: 0x0400408B RID: 16523
			msocbvcrCBTitleText,
			// Token: 0x0400408C RID: 16524
			msocbvcrDisabledFocuslessHighlightedText,
			// Token: 0x0400408D RID: 16525
			msocbvcrDisabledHighlightedText,
			// Token: 0x0400408E RID: 16526
			msocbvcrDlgGroupBoxText,
			// Token: 0x0400408F RID: 16527
			msocbvcrDocTabBdr,
			// Token: 0x04004090 RID: 16528
			msocbvcrDocTabBdrDark,
			// Token: 0x04004091 RID: 16529
			msocbvcrDocTabBdrDarkMouseDown,
			// Token: 0x04004092 RID: 16530
			msocbvcrDocTabBdrDarkMouseOver,
			// Token: 0x04004093 RID: 16531
			msocbvcrDocTabBdrLight,
			// Token: 0x04004094 RID: 16532
			msocbvcrDocTabBdrLightMouseDown,
			// Token: 0x04004095 RID: 16533
			msocbvcrDocTabBdrLightMouseOver,
			// Token: 0x04004096 RID: 16534
			msocbvcrDocTabBdrMouseDown,
			// Token: 0x04004097 RID: 16535
			msocbvcrDocTabBdrMouseOver,
			// Token: 0x04004098 RID: 16536
			msocbvcrDocTabBdrSelected,
			// Token: 0x04004099 RID: 16537
			msocbvcrDocTabBkgd,
			// Token: 0x0400409A RID: 16538
			msocbvcrDocTabBkgdMouseDown,
			// Token: 0x0400409B RID: 16539
			msocbvcrDocTabBkgdMouseOver,
			// Token: 0x0400409C RID: 16540
			msocbvcrDocTabBkgdSelected,
			// Token: 0x0400409D RID: 16541
			msocbvcrDocTabText,
			// Token: 0x0400409E RID: 16542
			msocbvcrDocTabTextMouseDown,
			// Token: 0x0400409F RID: 16543
			msocbvcrDocTabTextMouseOver,
			// Token: 0x040040A0 RID: 16544
			msocbvcrDocTabTextSelected,
			// Token: 0x040040A1 RID: 16545
			msocbvcrDWActiveTabBkgd,
			// Token: 0x040040A2 RID: 16546
			msocbvcrDWActiveTabText,
			// Token: 0x040040A3 RID: 16547
			msocbvcrDWActiveTabTextDisabled,
			// Token: 0x040040A4 RID: 16548
			msocbvcrDWInactiveTabBkgd,
			// Token: 0x040040A5 RID: 16549
			msocbvcrDWInactiveTabText,
			// Token: 0x040040A6 RID: 16550
			msocbvcrDWTabBkgdMouseDown,
			// Token: 0x040040A7 RID: 16551
			msocbvcrDWTabBkgdMouseOver,
			// Token: 0x040040A8 RID: 16552
			msocbvcrDWTabTextMouseDown,
			// Token: 0x040040A9 RID: 16553
			msocbvcrDWTabTextMouseOver,
			// Token: 0x040040AA RID: 16554
			msocbvcrFocuslessHighlightedBkgd,
			// Token: 0x040040AB RID: 16555
			msocbvcrFocuslessHighlightedText,
			// Token: 0x040040AC RID: 16556
			msocbvcrGDHeaderBdr,
			// Token: 0x040040AD RID: 16557
			msocbvcrGDHeaderBkgd,
			// Token: 0x040040AE RID: 16558
			msocbvcrGDHeaderCellBdr,
			// Token: 0x040040AF RID: 16559
			msocbvcrGDHeaderCellBkgd,
			// Token: 0x040040B0 RID: 16560
			msocbvcrGDHeaderCellBkgdSelected,
			// Token: 0x040040B1 RID: 16561
			msocbvcrGDHeaderSeeThroughSelection,
			// Token: 0x040040B2 RID: 16562
			msocbvcrGSPDarkBkgd,
			// Token: 0x040040B3 RID: 16563
			msocbvcrGSPGroupContentDarkBkgd,
			// Token: 0x040040B4 RID: 16564
			msocbvcrGSPGroupContentLightBkgd,
			// Token: 0x040040B5 RID: 16565
			msocbvcrGSPGroupContentText,
			// Token: 0x040040B6 RID: 16566
			msocbvcrGSPGroupContentTextDisabled,
			// Token: 0x040040B7 RID: 16567
			msocbvcrGSPGroupHeaderDarkBkgd,
			// Token: 0x040040B8 RID: 16568
			msocbvcrGSPGroupHeaderLightBkgd,
			// Token: 0x040040B9 RID: 16569
			msocbvcrGSPGroupHeaderText,
			// Token: 0x040040BA RID: 16570
			msocbvcrGSPGroupline,
			// Token: 0x040040BB RID: 16571
			msocbvcrGSPHyperlink,
			// Token: 0x040040BC RID: 16572
			msocbvcrGSPLightBkgd,
			// Token: 0x040040BD RID: 16573
			msocbvcrHyperlink,
			// Token: 0x040040BE RID: 16574
			msocbvcrHyperlinkFollowed,
			// Token: 0x040040BF RID: 16575
			msocbvcrJotNavUIBdr,
			// Token: 0x040040C0 RID: 16576
			msocbvcrJotNavUIGradBegin,
			// Token: 0x040040C1 RID: 16577
			msocbvcrJotNavUIGradEnd,
			// Token: 0x040040C2 RID: 16578
			msocbvcrJotNavUIGradMiddle,
			// Token: 0x040040C3 RID: 16579
			msocbvcrJotNavUIText,
			// Token: 0x040040C4 RID: 16580
			msocbvcrListHeaderArrow,
			// Token: 0x040040C5 RID: 16581
			msocbvcrNetLookBkgnd,
			// Token: 0x040040C6 RID: 16582
			msocbvcrOABBkgd,
			// Token: 0x040040C7 RID: 16583
			msocbvcrOBBkgdBdr,
			// Token: 0x040040C8 RID: 16584
			msocbvcrOBBkgdBdrContrast,
			// Token: 0x040040C9 RID: 16585
			msocbvcrOGMDIParentWorkspaceBkgd,
			// Token: 0x040040CA RID: 16586
			msocbvcrOGRulerActiveBkgd,
			// Token: 0x040040CB RID: 16587
			msocbvcrOGRulerBdr,
			// Token: 0x040040CC RID: 16588
			msocbvcrOGRulerBkgd,
			// Token: 0x040040CD RID: 16589
			msocbvcrOGRulerInactiveBkgd,
			// Token: 0x040040CE RID: 16590
			msocbvcrOGRulerTabBoxBdr,
			// Token: 0x040040CF RID: 16591
			msocbvcrOGRulerTabBoxBdrHighlight,
			// Token: 0x040040D0 RID: 16592
			msocbvcrOGRulerTabStopTicks,
			// Token: 0x040040D1 RID: 16593
			msocbvcrOGRulerText,
			// Token: 0x040040D2 RID: 16594
			msocbvcrOGTaskPaneGroupBoxHeaderBkgd,
			// Token: 0x040040D3 RID: 16595
			msocbvcrOGWorkspaceBkgd,
			// Token: 0x040040D4 RID: 16596
			msocbvcrOLKFlagNone,
			// Token: 0x040040D5 RID: 16597
			msocbvcrOLKFolderbarDark,
			// Token: 0x040040D6 RID: 16598
			msocbvcrOLKFolderbarLight,
			// Token: 0x040040D7 RID: 16599
			msocbvcrOLKFolderbarText,
			// Token: 0x040040D8 RID: 16600
			msocbvcrOLKGridlines,
			// Token: 0x040040D9 RID: 16601
			msocbvcrOLKGroupLine,
			// Token: 0x040040DA RID: 16602
			msocbvcrOLKGroupNested,
			// Token: 0x040040DB RID: 16603
			msocbvcrOLKGroupShaded,
			// Token: 0x040040DC RID: 16604
			msocbvcrOLKGroupText,
			// Token: 0x040040DD RID: 16605
			msocbvcrOLKIconBar,
			// Token: 0x040040DE RID: 16606
			msocbvcrOLKInfoBarBkgd,
			// Token: 0x040040DF RID: 16607
			msocbvcrOLKInfoBarText,
			// Token: 0x040040E0 RID: 16608
			msocbvcrOLKPreviewPaneLabelText,
			// Token: 0x040040E1 RID: 16609
			msocbvcrOLKTodayIndicatorDark,
			// Token: 0x040040E2 RID: 16610
			msocbvcrOLKTodayIndicatorLight,
			// Token: 0x040040E3 RID: 16611
			msocbvcrOLKWBActionDividerLine,
			// Token: 0x040040E4 RID: 16612
			msocbvcrOLKWBButtonDark,
			// Token: 0x040040E5 RID: 16613
			msocbvcrOLKWBButtonLight,
			// Token: 0x040040E6 RID: 16614
			msocbvcrOLKWBDarkOutline,
			// Token: 0x040040E7 RID: 16615
			msocbvcrOLKWBFoldersBackground,
			// Token: 0x040040E8 RID: 16616
			msocbvcrOLKWBHoverButtonDark,
			// Token: 0x040040E9 RID: 16617
			msocbvcrOLKWBHoverButtonLight,
			// Token: 0x040040EA RID: 16618
			msocbvcrOLKWBLabelText,
			// Token: 0x040040EB RID: 16619
			msocbvcrOLKWBPressedButtonDark,
			// Token: 0x040040EC RID: 16620
			msocbvcrOLKWBPressedButtonLight,
			// Token: 0x040040ED RID: 16621
			msocbvcrOLKWBSelectedButtonDark,
			// Token: 0x040040EE RID: 16622
			msocbvcrOLKWBSelectedButtonLight,
			// Token: 0x040040EF RID: 16623
			msocbvcrOLKWBSplitterDark,
			// Token: 0x040040F0 RID: 16624
			msocbvcrOLKWBSplitterLight,
			// Token: 0x040040F1 RID: 16625
			msocbvcrPlacesBarBkgd,
			// Token: 0x040040F2 RID: 16626
			msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd,
			// Token: 0x040040F3 RID: 16627
			msocbvcrPPOutlineThumbnailsPaneTabBdr,
			// Token: 0x040040F4 RID: 16628
			msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd,
			// Token: 0x040040F5 RID: 16629
			msocbvcrPPOutlineThumbnailsPaneTabText,
			// Token: 0x040040F6 RID: 16630
			msocbvcrPPSlideBdrActiveSelected,
			// Token: 0x040040F7 RID: 16631
			msocbvcrPPSlideBdrActiveSelectedMouseOver,
			// Token: 0x040040F8 RID: 16632
			msocbvcrPPSlideBdrInactiveSelected,
			// Token: 0x040040F9 RID: 16633
			msocbvcrPPSlideBdrMouseOver,
			// Token: 0x040040FA RID: 16634
			msocbvcrPubPrintDocScratchPageBkgd,
			// Token: 0x040040FB RID: 16635
			msocbvcrPubWebDocScratchPageBkgd,
			// Token: 0x040040FC RID: 16636
			msocbvcrSBBdr,
			// Token: 0x040040FD RID: 16637
			msocbvcrScrollbarBkgd,
			// Token: 0x040040FE RID: 16638
			msocbvcrToastGradBegin,
			// Token: 0x040040FF RID: 16639
			msocbvcrToastGradEnd,
			// Token: 0x04004100 RID: 16640
			msocbvcrWPBdrInnerDocked,
			// Token: 0x04004101 RID: 16641
			msocbvcrWPBdrOuterDocked,
			// Token: 0x04004102 RID: 16642
			msocbvcrWPBdrOuterFloating,
			// Token: 0x04004103 RID: 16643
			msocbvcrWPBkgd,
			// Token: 0x04004104 RID: 16644
			msocbvcrWPCtlBdr,
			// Token: 0x04004105 RID: 16645
			msocbvcrWPCtlBdrDefault,
			// Token: 0x04004106 RID: 16646
			msocbvcrWPCtlBdrDisabled,
			// Token: 0x04004107 RID: 16647
			msocbvcrWPCtlBkgd,
			// Token: 0x04004108 RID: 16648
			msocbvcrWPCtlBkgdDisabled,
			// Token: 0x04004109 RID: 16649
			msocbvcrWPCtlText,
			// Token: 0x0400410A RID: 16650
			msocbvcrWPCtlTextDisabled,
			// Token: 0x0400410B RID: 16651
			msocbvcrWPCtlTextMouseDown,
			// Token: 0x0400410C RID: 16652
			msocbvcrWPGroupline,
			// Token: 0x0400410D RID: 16653
			msocbvcrWPInfoTipBkgd,
			// Token: 0x0400410E RID: 16654
			msocbvcrWPInfoTipText,
			// Token: 0x0400410F RID: 16655
			msocbvcrWPNavBarBkgnd,
			// Token: 0x04004110 RID: 16656
			msocbvcrWPText,
			// Token: 0x04004111 RID: 16657
			msocbvcrWPTextDisabled,
			// Token: 0x04004112 RID: 16658
			msocbvcrWPTitleBkgdActive,
			// Token: 0x04004113 RID: 16659
			msocbvcrWPTitleBkgdInactive,
			// Token: 0x04004114 RID: 16660
			msocbvcrWPTitleTextActive,
			// Token: 0x04004115 RID: 16661
			msocbvcrWPTitleTextInactive,
			// Token: 0x04004116 RID: 16662
			msocbvcrXLFormulaBarBkgd,
			// Token: 0x04004117 RID: 16663
			ButtonSelectedHighlight,
			// Token: 0x04004118 RID: 16664
			ButtonPressedHighlight,
			// Token: 0x04004119 RID: 16665
			ButtonCheckedHighlight,
			// Token: 0x0400411A RID: 16666
			lastKnownColor = 212
		}
	}
}
