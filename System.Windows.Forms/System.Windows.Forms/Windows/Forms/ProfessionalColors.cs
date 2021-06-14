using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides <see cref="T:System.Drawing.Color" /> structures that are colors of a Windows display element. This class cannot be inherited. </summary>
	// Token: 0x02000315 RID: 789
	public sealed class ProfessionalColors
	{
		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06002FAD RID: 12205 RVA: 0x000DB33E File Offset: 0x000D953E
		internal static ProfessionalColorTable ColorTable
		{
			get
			{
				if (ProfessionalColors.professionalColorTable == null)
				{
					ProfessionalColors.professionalColorTable = new ProfessionalColorTable();
				}
				return ProfessionalColors.professionalColorTable;
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000DB356 File Offset: 0x000D9556
		static ProfessionalColors()
		{
			SystemEvents.UserPreferenceChanged += ProfessionalColors.OnUserPreferenceChanged;
			ProfessionalColors.SetScheme();
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000027DB File Offset: 0x000009DB
		private ProfessionalColors()
		{
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x000DB36E File Offset: 0x000D956E
		internal static string ColorScheme
		{
			get
			{
				return ProfessionalColors.colorScheme;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x000DB375 File Offset: 0x000D9575
		internal static object ColorFreshnessKey
		{
			get
			{
				return ProfessionalColors.colorFreshnessKey;
			}
		}

		/// <summary>Gets the solid color used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is selected.</returns>
		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x000DB37C File Offset: 0x000D957C
		[SRDescription("ProfessionalColorsButtonSelectedHighlightDescr")]
		public static Color ButtonSelectedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedHighlight;
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedHighlight" />.</returns>
		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x000DB388 File Offset: 0x000D9588
		[SRDescription("ProfessionalColorsButtonSelectedHighlightBorderDescr")]
		public static Color ButtonSelectedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedHighlightBorder;
			}
		}

		/// <summary>Gets the solid color used when the button is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is pressed down.</returns>
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000DB394 File Offset: 0x000D9594
		[SRDescription("ProfessionalColorsButtonPressedHighlightDescr")]
		public static Color ButtonPressedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedHighlight;
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedHighlight" />.</returns>
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000DB3A0 File Offset: 0x000D95A0
		[SRDescription("ProfessionalColorsButtonPressedHighlightBorderDescr")]
		public static Color ButtonPressedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedHighlightBorder;
			}
		}

		/// <summary>Gets the solid color used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color used when the button is checked.</returns>
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x000DB3AC File Offset: 0x000D95AC
		[SRDescription("ProfessionalColorsButtonCheckedHighlightDescr")]
		public static Color ButtonCheckedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedHighlight;
			}
		}

		/// <summary>Gets the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonCheckedHighlight" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonCheckedHighlight" />.</returns>
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000DB3B8 File Offset: 0x000D95B8
		[SRDescription("ProfessionalColorsButtonCheckedHighlightBorderDescr")]
		public static Color ButtonCheckedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedHighlightBorder;
			}
		}

		/// <summary>Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientEnd" /> colors.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonPressedGradientEnd" /> colors.</returns>
		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000DB3C4 File Offset: 0x000D95C4
		[SRDescription("ProfessionalColorsButtonPressedBorderDescr")]
		public static Color ButtonPressedBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedBorder;
			}
		}

		/// <summary>Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientEnd" /> colors.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientBegin" />, <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientMiddle" />, and <see cref="P:System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientEnd" /> colors.</returns>
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x000DB3D0 File Offset: 0x000D95D0
		[SRDescription("ProfessionalColorsButtonSelectedBorderDescr")]
		public static Color ButtonSelectedBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedBorder;
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the gradient used when the button is checked.</returns>
		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000DB3DC File Offset: 0x000D95DC
		[SRDescription("ProfessionalColorsButtonCheckedGradientBeginDescr")]
		public static Color ButtonCheckedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is checked.</returns>
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06002FBB RID: 12219 RVA: 0x000DB3E8 File Offset: 0x000D95E8
		[SRDescription("ProfessionalColorsButtonCheckedGradientMiddleDescr")]
		public static Color ButtonCheckedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is checked.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is checked.</returns>
		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000DB3F4 File Offset: 0x000D95F4
		[SRDescription("ProfessionalColorsButtonCheckedGradientEndDescr")]
		public static Color ButtonCheckedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the button is selected.</returns>
		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000DB400 File Offset: 0x000D9600
		[SRDescription("ProfessionalColorsButtonSelectedGradientBeginDescr")]
		public static Color ButtonSelectedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is selected.</returns>
		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000DB40C File Offset: 0x000D960C
		[SRDescription("ProfessionalColorsButtonSelectedGradientMiddleDescr")]
		public static Color ButtonSelectedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is selected.</returns>
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06002FBF RID: 12223 RVA: 0x000DB418 File Offset: 0x000D9618
		[SRDescription("ProfessionalColorsButtonSelectedGradientEndDescr")]
		public static Color ButtonSelectedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used when the button is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the button is pressed down.</returns>
		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000DB424 File Offset: 0x000D9624
		[SRDescription("ProfessionalColorsButtonPressedGradientBeginDescr")]
		public static Color ButtonPressedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used when the button is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when the button is pressed.</returns>
		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06002FC1 RID: 12225 RVA: 0x000DB430 File Offset: 0x000D9630
		[SRDescription("ProfessionalColorsButtonPressedGradientMiddleDescr")]
		public static Color ButtonPressedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used when the button is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the button is pressed down.</returns>
		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000DB43C File Offset: 0x000D963C
		[SRDescription("ProfessionalColorsButtonPressedGradientEndDescr")]
		public static Color ButtonPressedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientEnd;
			}
		}

		/// <summary>Gets the solid color to use when the check box is selected and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the check box is selected and gradients are being used.</returns>
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000DB448 File Offset: 0x000D9648
		[SRDescription("ProfessionalColorsCheckBackgroundDescr")]
		public static Color CheckBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckBackground;
			}
		}

		/// <summary>Gets the solid color to use when the check box is selected and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the check box is selected and gradients are being used.</returns>
		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000DB454 File Offset: 0x000D9654
		[SRDescription("ProfessionalColorsCheckSelectedBackgroundDescr")]
		public static Color CheckSelectedBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckSelectedBackground;
			}
		}

		/// <summary>Gets the solid color to use when the check box is selected and gradients are being used.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when the check box is selected and gradients are being used.</returns>
		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x000DB460 File Offset: 0x000D9660
		[SRDescription("ProfessionalColorsCheckPressedBackgroundDescr")]
		public static Color CheckPressedBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckPressedBackground;
			}
		}

		/// <summary>Gets the color to use for shadow effects on the grip or move handle.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use for shadow effects on the grip or move handle.</returns>
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000DB46C File Offset: 0x000D966C
		[SRDescription("ProfessionalColorsGripDarkDescr")]
		public static Color GripDark
		{
			get
			{
				return ProfessionalColors.ColorTable.GripDark;
			}
		}

		/// <summary>Gets the color to use for highlight effects on the grip or move handle.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use for highlight effects on the grip or move handle.</returns>
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000DB478 File Offset: 0x000D9678
		[SRDescription("ProfessionalColorsGripLightDescr")]
		public static Color GripLight
		{
			get
			{
				return ProfessionalColors.ColorTable.GripLight;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06002FC8 RID: 12232 RVA: 0x000DB484 File Offset: 0x000D9684
		[SRDescription("ProfessionalColorsImageMarginGradientBeginDescr")]
		public static Color ImageMarginGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x000DB490 File Offset: 0x000D9690
		[SRDescription("ProfessionalColorsImageMarginGradientMiddleDescr")]
		public static Color ImageMarginGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</returns>
		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06002FCA RID: 12234 RVA: 0x000DB49C File Offset: 0x000D969C
		[SRDescription("ProfessionalColorsImageMarginGradientEndDescr")]
		public static Color ImageMarginGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x000DB4A8 File Offset: 0x000D96A8
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientBeginDescr")]
		public static Color ImageMarginRevealedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x000DB4B4 File Offset: 0x000D96B4
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientMiddleDescr")]
		public static Color ImageMarginRevealedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> when an item is revealed.</returns>
		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x000DB4C0 File Offset: 0x000D96C0
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientEndDescr")]
		public static Color ImageMarginRevealedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x000DB4CC File Offset: 0x000D96CC
		[SRDescription("ProfessionalColorsMenuStripGradientBeginDescr")]
		public static Color MenuStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuStripGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x000DB4D8 File Offset: 0x000D96D8
		[SRDescription("ProfessionalColorsMenuStripGradientEndDescr")]
		public static Color MenuStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuStripGradientEnd;
			}
		}

		/// <summary>Gets the border color or a <see cref="T:System.Windows.Forms.MenuStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color or a <see cref="T:System.Windows.Forms.MenuStrip" />.</returns>
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06002FD0 RID: 12240 RVA: 0x000DB4E4 File Offset: 0x000D96E4
		[SRDescription("ProfessionalColorsMenuBorderDescr")]
		public static Color MenuBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuBorder;
			}
		}

		/// <summary>Gets the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x000DB4F0 File Offset: 0x000D96F0
		[SRDescription("ProfessionalColorsMenuItemSelectedDescr")]
		public static Color MenuItemSelected
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemSelected;
			}
		}

		/// <summary>Gets the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</returns>
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06002FD2 RID: 12242 RVA: 0x000DB4FC File Offset: 0x000D96FC
		[SRDescription("ProfessionalColorsMenuItemBorderDescr")]
		public static Color MenuItemBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemBorder;
			}
		}

		/// <summary>Gets the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x000DB508 File Offset: 0x000D9708
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientBeginDescr")]
		public static Color MenuItemSelectedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemSelectedGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is selected.</returns>
		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x000DB514 File Offset: 0x000D9714
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientEndDescr")]
		public static Color MenuItemSelectedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemSelectedGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</returns>
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x000DB520 File Offset: 0x000D9720
		[SRDescription("ProfessionalColorsMenuItemPressedGradientBeginDescr")]
		public static Color MenuItemPressedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</returns>
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x000DB52C File Offset: 0x000D972C
		[SRDescription("ProfessionalColorsMenuItemPressedGradientMiddleDescr")]
		public static Color MenuItemPressedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is pressed down.</returns>
		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x000DB538 File Offset: 0x000D9738
		[SRDescription("ProfessionalColorsMenuItemPressedGradientEndDescr")]
		public static Color MenuItemPressedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x000DB544 File Offset: 0x000D9744
		[SRDescription("ProfessionalColorsRaftingContainerGradientBeginDescr")]
		public static Color RaftingContainerGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.RaftingContainerGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer" />.</returns>
		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x000DB550 File Offset: 0x000D9750
		[SRDescription("ProfessionalColorsRaftingContainerGradientEndDescr")]
		public static Color RaftingContainerGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.RaftingContainerGradientEnd;
			}
		}

		/// <summary>Gets the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06002FDA RID: 12250 RVA: 0x000DB55C File Offset: 0x000D975C
		[SRDescription("ProfessionalColorsSeparatorDarkDescr")]
		public static Color SeparatorDark
		{
			get
			{
				return ProfessionalColors.ColorTable.SeparatorDark;
			}
		}

		/// <summary>Gets the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color to use to create highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</returns>
		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06002FDB RID: 12251 RVA: 0x000DB568 File Offset: 0x000D9768
		[SRDescription("ProfessionalColorsSeparatorLightDescr")]
		public static Color SeparatorLight
		{
			get
			{
				return ProfessionalColors.ColorTable.SeparatorLight;
			}
		}

		/// <summary>Gets the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</returns>
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x000DB574 File Offset: 0x000D9774
		[SRDescription("ProfessionalColorsStatusStripGradientBeginDescr")]
		public static Color StatusStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.StatusStripGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used on the <see cref="T:System.Windows.Forms.StatusStrip" />.</returns>
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06002FDD RID: 12253 RVA: 0x000DB580 File Offset: 0x000D9780
		[SRDescription("ProfessionalColorsStatusStripGradientEndDescr")]
		public static Color StatusStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.StatusStripGradientEnd;
			}
		}

		/// <summary>Gets the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000DB58C File Offset: 0x000D978C
		[SRDescription("ProfessionalColorsToolStripBorderDescr")]
		public static Color ToolStripBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripBorder;
			}
		}

		/// <summary>Gets the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown" />.</returns>
		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000DB598 File Offset: 0x000D9798
		[SRDescription("ProfessionalColorsToolStripDropDownBackgroundDescr")]
		public static Color ToolStripDropDownBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripDropDownBackground;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x000DB5A4 File Offset: 0x000D97A4
		[SRDescription("ProfessionalColorsToolStripGradientBeginDescr")]
		public static Color ToolStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x000DB5B0 File Offset: 0x000D97B0
		[SRDescription("ProfessionalColorsToolStripGradientMiddleDescr")]
		public static Color ToolStripGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip" /> background.</returns>
		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x000DB5BC File Offset: 0x000D97BC
		[SRDescription("ProfessionalColorsToolStripGradientEndDescr")]
		public static Color ToolStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</returns>
		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06002FE3 RID: 12259 RVA: 0x000DB5C8 File Offset: 0x000D97C8
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientBeginDescr")]
		public static Color ToolStripContentPanelGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripContentPanelGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContentPanel" />.</returns>
		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x000DB5D4 File Offset: 0x000D97D4
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientEndDescr")]
		public static Color ToolStripContentPanelGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripContentPanelGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</returns>
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06002FE5 RID: 12261 RVA: 0x000DB5E0 File Offset: 0x000D97E0
		[SRDescription("ProfessionalColorsToolStripPanelGradientBeginDescr")]
		public static Color ToolStripPanelGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripPanelGradientBegin;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</returns>
		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x000DB5EC File Offset: 0x000D97EC
		[SRDescription("ProfessionalColorsToolStripPanelGradientEndDescr")]
		public static Color ToolStripPanelGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripPanelGradientEnd;
			}
		}

		/// <summary>Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000DB5F8 File Offset: 0x000D97F8
		[SRDescription("ProfessionalColorsOverflowButtonGradientBeginDescr")]
		public static Color OverflowButtonGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientBegin;
			}
		}

		/// <summary>Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000DB604 File Offset: 0x000D9804
		[SRDescription("ProfessionalColorsOverflowButtonGradientMiddleDescr")]
		public static Color OverflowButtonGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientMiddle;
			}
		}

		/// <summary>Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</returns>
		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000DB610 File Offset: 0x000D9810
		[SRDescription("ProfessionalColorsOverflowButtonGradientEndDescr")]
		public static Color OverflowButtonGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientEnd;
			}
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000DB61C File Offset: 0x000D981C
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			ProfessionalColors.SetScheme();
			if (e.Category == UserPreferenceCategory.Color)
			{
				ProfessionalColors.colorFreshnessKey = new object();
			}
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000DB636 File Offset: 0x000D9836
		private static void SetScheme()
		{
			if (VisualStyleRenderer.IsSupported)
			{
				ProfessionalColors.colorScheme = VisualStyleInformation.ColorScheme;
				return;
			}
			ProfessionalColors.colorScheme = null;
		}

		// Token: 0x04001D9B RID: 7579
		[ThreadStatic]
		private static ProfessionalColorTable professionalColorTable;

		// Token: 0x04001D9C RID: 7580
		[ThreadStatic]
		private static string colorScheme;

		// Token: 0x04001D9D RID: 7581
		[ThreadStatic]
		private static object colorFreshnessKey;
	}
}
