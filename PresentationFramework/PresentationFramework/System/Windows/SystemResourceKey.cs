using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;

namespace System.Windows
{
	// Token: 0x02000114 RID: 276
	[TypeConverter(typeof(SystemKeyConverter))]
	internal class SystemResourceKey : ResourceKey
	{
		// Token: 0x06000B59 RID: 2905 RVA: 0x00028024 File Offset: 0x00026224
		internal static short GetSystemResourceKeyIdFromBamlId(short bamlId, out bool isKey)
		{
			isKey = true;
			if (bamlId > 232 && bamlId < 464)
			{
				bamlId -= 232;
				isKey = false;
			}
			else if (bamlId > 464 && bamlId < 467)
			{
				bamlId -= 231;
			}
			else if (bamlId > 467 && bamlId < 470)
			{
				bamlId -= 234;
				isKey = false;
			}
			return bamlId;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00028090 File Offset: 0x00026290
		internal static short GetBamlIdBasedOnSystemResourceKeyId(Type targetType, string memberName)
		{
			short result = 0;
			bool flag = false;
			bool flag2 = true;
			SystemResourceKeyID systemResourceKeyID = SystemResourceKeyID.InternalSystemColorsStart;
			string text;
			if (memberName.EndsWith("Key", false, TypeConverterHelper.InvariantEnglishUS))
			{
				text = memberName.Remove(memberName.Length - 3);
				if (KnownTypes.Types[403] == targetType || KnownTypes.Types[669] == targetType || KnownTypes.Types[604] == targetType)
				{
					text = targetType.Name + text;
				}
				flag = true;
			}
			else
			{
				text = memberName;
			}
			try
			{
				systemResourceKeyID = (SystemResourceKeyID)Enum.Parse(typeof(SystemResourceKeyID), text);
			}
			catch (ArgumentException)
			{
				flag2 = false;
			}
			if (flag2)
			{
				bool flag3 = (short)systemResourceKeyID > 233 && (short)systemResourceKeyID < 236;
				if (flag3)
				{
					if (flag)
					{
						result = -((short)systemResourceKeyID - 233 + 464);
					}
					else
					{
						result = -((short)systemResourceKeyID - 233 + 467);
					}
				}
				else if (flag)
				{
					result = -(short)systemResourceKeyID;
				}
				else
				{
					result = -((short)systemResourceKeyID + 232);
				}
			}
			return result;
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x000281B4 File Offset: 0x000263B4
		internal object Resource
		{
			get
			{
				switch (this._id)
				{
				case SystemResourceKeyID.ActiveBorderBrush:
					return SystemColors.ActiveBorderBrush;
				case SystemResourceKeyID.ActiveCaptionBrush:
					return SystemColors.ActiveCaptionBrush;
				case SystemResourceKeyID.ActiveCaptionTextBrush:
					return SystemColors.ActiveCaptionTextBrush;
				case SystemResourceKeyID.AppWorkspaceBrush:
					return SystemColors.AppWorkspaceBrush;
				case SystemResourceKeyID.ControlBrush:
					return SystemColors.ControlBrush;
				case SystemResourceKeyID.ControlDarkBrush:
					return SystemColors.ControlDarkBrush;
				case SystemResourceKeyID.ControlDarkDarkBrush:
					return SystemColors.ControlDarkDarkBrush;
				case SystemResourceKeyID.ControlLightBrush:
					return SystemColors.ControlLightBrush;
				case SystemResourceKeyID.ControlLightLightBrush:
					return SystemColors.ControlLightLightBrush;
				case SystemResourceKeyID.ControlTextBrush:
					return SystemColors.ControlTextBrush;
				case SystemResourceKeyID.DesktopBrush:
					return SystemColors.DesktopBrush;
				case SystemResourceKeyID.GradientActiveCaptionBrush:
					return SystemColors.GradientActiveCaptionBrush;
				case SystemResourceKeyID.GradientInactiveCaptionBrush:
					return SystemColors.GradientInactiveCaptionBrush;
				case SystemResourceKeyID.GrayTextBrush:
					return SystemColors.GrayTextBrush;
				case SystemResourceKeyID.HighlightBrush:
					return SystemColors.HighlightBrush;
				case SystemResourceKeyID.HighlightTextBrush:
					return SystemColors.HighlightTextBrush;
				case SystemResourceKeyID.HotTrackBrush:
					return SystemColors.HotTrackBrush;
				case SystemResourceKeyID.InactiveBorderBrush:
					return SystemColors.InactiveBorderBrush;
				case SystemResourceKeyID.InactiveCaptionBrush:
					return SystemColors.InactiveCaptionBrush;
				case SystemResourceKeyID.InactiveCaptionTextBrush:
					return SystemColors.InactiveCaptionTextBrush;
				case SystemResourceKeyID.InfoBrush:
					return SystemColors.InfoBrush;
				case SystemResourceKeyID.InfoTextBrush:
					return SystemColors.InfoTextBrush;
				case SystemResourceKeyID.MenuBrush:
					return SystemColors.MenuBrush;
				case SystemResourceKeyID.MenuBarBrush:
					return SystemColors.MenuBarBrush;
				case SystemResourceKeyID.MenuHighlightBrush:
					return SystemColors.MenuHighlightBrush;
				case SystemResourceKeyID.MenuTextBrush:
					return SystemColors.MenuTextBrush;
				case SystemResourceKeyID.ScrollBarBrush:
					return SystemColors.ScrollBarBrush;
				case SystemResourceKeyID.WindowBrush:
					return SystemColors.WindowBrush;
				case SystemResourceKeyID.WindowFrameBrush:
					return SystemColors.WindowFrameBrush;
				case SystemResourceKeyID.WindowTextBrush:
					return SystemColors.WindowTextBrush;
				case SystemResourceKeyID.ActiveBorderColor:
					return SystemColors.ActiveBorderColor;
				case SystemResourceKeyID.ActiveCaptionColor:
					return SystemColors.ActiveCaptionColor;
				case SystemResourceKeyID.ActiveCaptionTextColor:
					return SystemColors.ActiveCaptionTextColor;
				case SystemResourceKeyID.AppWorkspaceColor:
					return SystemColors.AppWorkspaceColor;
				case SystemResourceKeyID.ControlColor:
					return SystemColors.ControlColor;
				case SystemResourceKeyID.ControlDarkColor:
					return SystemColors.ControlDarkColor;
				case SystemResourceKeyID.ControlDarkDarkColor:
					return SystemColors.ControlDarkDarkColor;
				case SystemResourceKeyID.ControlLightColor:
					return SystemColors.ControlLightColor;
				case SystemResourceKeyID.ControlLightLightColor:
					return SystemColors.ControlLightLightColor;
				case SystemResourceKeyID.ControlTextColor:
					return SystemColors.ControlTextColor;
				case SystemResourceKeyID.DesktopColor:
					return SystemColors.DesktopColor;
				case SystemResourceKeyID.GradientActiveCaptionColor:
					return SystemColors.GradientActiveCaptionColor;
				case SystemResourceKeyID.GradientInactiveCaptionColor:
					return SystemColors.GradientInactiveCaptionColor;
				case SystemResourceKeyID.GrayTextColor:
					return SystemColors.GrayTextColor;
				case SystemResourceKeyID.HighlightColor:
					return SystemColors.HighlightColor;
				case SystemResourceKeyID.HighlightTextColor:
					return SystemColors.HighlightTextColor;
				case SystemResourceKeyID.HotTrackColor:
					return SystemColors.HotTrackColor;
				case SystemResourceKeyID.InactiveBorderColor:
					return SystemColors.InactiveBorderColor;
				case SystemResourceKeyID.InactiveCaptionColor:
					return SystemColors.InactiveCaptionColor;
				case SystemResourceKeyID.InactiveCaptionTextColor:
					return SystemColors.InactiveCaptionTextColor;
				case SystemResourceKeyID.InfoColor:
					return SystemColors.InfoColor;
				case SystemResourceKeyID.InfoTextColor:
					return SystemColors.InfoTextColor;
				case SystemResourceKeyID.MenuColor:
					return SystemColors.MenuColor;
				case SystemResourceKeyID.MenuBarColor:
					return SystemColors.MenuBarColor;
				case SystemResourceKeyID.MenuHighlightColor:
					return SystemColors.MenuHighlightColor;
				case SystemResourceKeyID.MenuTextColor:
					return SystemColors.MenuTextColor;
				case SystemResourceKeyID.ScrollBarColor:
					return SystemColors.ScrollBarColor;
				case SystemResourceKeyID.WindowColor:
					return SystemColors.WindowColor;
				case SystemResourceKeyID.WindowFrameColor:
					return SystemColors.WindowFrameColor;
				case SystemResourceKeyID.WindowTextColor:
					return SystemColors.WindowTextColor;
				case SystemResourceKeyID.CaptionFontSize:
					return SystemFonts.CaptionFontSize;
				case SystemResourceKeyID.CaptionFontFamily:
					return SystemFonts.CaptionFontFamily;
				case SystemResourceKeyID.CaptionFontStyle:
					return SystemFonts.CaptionFontStyle;
				case SystemResourceKeyID.CaptionFontWeight:
					return SystemFonts.CaptionFontWeight;
				case SystemResourceKeyID.CaptionFontTextDecorations:
					return SystemFonts.CaptionFontTextDecorations;
				case SystemResourceKeyID.SmallCaptionFontSize:
					return SystemFonts.SmallCaptionFontSize;
				case SystemResourceKeyID.SmallCaptionFontFamily:
					return SystemFonts.SmallCaptionFontFamily;
				case SystemResourceKeyID.SmallCaptionFontStyle:
					return SystemFonts.SmallCaptionFontStyle;
				case SystemResourceKeyID.SmallCaptionFontWeight:
					return SystemFonts.SmallCaptionFontWeight;
				case SystemResourceKeyID.SmallCaptionFontTextDecorations:
					return SystemFonts.SmallCaptionFontTextDecorations;
				case SystemResourceKeyID.MenuFontSize:
					return SystemFonts.MenuFontSize;
				case SystemResourceKeyID.MenuFontFamily:
					return SystemFonts.MenuFontFamily;
				case SystemResourceKeyID.MenuFontStyle:
					return SystemFonts.MenuFontStyle;
				case SystemResourceKeyID.MenuFontWeight:
					return SystemFonts.MenuFontWeight;
				case SystemResourceKeyID.MenuFontTextDecorations:
					return SystemFonts.MenuFontTextDecorations;
				case SystemResourceKeyID.StatusFontSize:
					return SystemFonts.StatusFontSize;
				case SystemResourceKeyID.StatusFontFamily:
					return SystemFonts.StatusFontFamily;
				case SystemResourceKeyID.StatusFontStyle:
					return SystemFonts.StatusFontStyle;
				case SystemResourceKeyID.StatusFontWeight:
					return SystemFonts.StatusFontWeight;
				case SystemResourceKeyID.StatusFontTextDecorations:
					return SystemFonts.StatusFontTextDecorations;
				case SystemResourceKeyID.MessageFontSize:
					return SystemFonts.MessageFontSize;
				case SystemResourceKeyID.MessageFontFamily:
					return SystemFonts.MessageFontFamily;
				case SystemResourceKeyID.MessageFontStyle:
					return SystemFonts.MessageFontStyle;
				case SystemResourceKeyID.MessageFontWeight:
					return SystemFonts.MessageFontWeight;
				case SystemResourceKeyID.MessageFontTextDecorations:
					return SystemFonts.MessageFontTextDecorations;
				case SystemResourceKeyID.IconFontSize:
					return SystemFonts.IconFontSize;
				case SystemResourceKeyID.IconFontFamily:
					return SystemFonts.IconFontFamily;
				case SystemResourceKeyID.IconFontStyle:
					return SystemFonts.IconFontStyle;
				case SystemResourceKeyID.IconFontWeight:
					return SystemFonts.IconFontWeight;
				case SystemResourceKeyID.IconFontTextDecorations:
					return SystemFonts.IconFontTextDecorations;
				case SystemResourceKeyID.ThinHorizontalBorderHeight:
					return SystemParameters.ThinHorizontalBorderHeight;
				case SystemResourceKeyID.ThinVerticalBorderWidth:
					return SystemParameters.ThinVerticalBorderWidth;
				case SystemResourceKeyID.CursorWidth:
					return SystemParameters.CursorWidth;
				case SystemResourceKeyID.CursorHeight:
					return SystemParameters.CursorHeight;
				case SystemResourceKeyID.ThickHorizontalBorderHeight:
					return SystemParameters.ThickHorizontalBorderHeight;
				case SystemResourceKeyID.ThickVerticalBorderWidth:
					return SystemParameters.ThickVerticalBorderWidth;
				case SystemResourceKeyID.FixedFrameHorizontalBorderHeight:
					return SystemParameters.FixedFrameHorizontalBorderHeight;
				case SystemResourceKeyID.FixedFrameVerticalBorderWidth:
					return SystemParameters.FixedFrameVerticalBorderWidth;
				case SystemResourceKeyID.FocusHorizontalBorderHeight:
					return SystemParameters.FocusHorizontalBorderHeight;
				case SystemResourceKeyID.FocusVerticalBorderWidth:
					return SystemParameters.FocusVerticalBorderWidth;
				case SystemResourceKeyID.FullPrimaryScreenWidth:
					return SystemParameters.FullPrimaryScreenWidth;
				case SystemResourceKeyID.FullPrimaryScreenHeight:
					return SystemParameters.FullPrimaryScreenHeight;
				case SystemResourceKeyID.HorizontalScrollBarButtonWidth:
					return SystemParameters.HorizontalScrollBarButtonWidth;
				case SystemResourceKeyID.HorizontalScrollBarHeight:
					return SystemParameters.HorizontalScrollBarHeight;
				case SystemResourceKeyID.HorizontalScrollBarThumbWidth:
					return SystemParameters.HorizontalScrollBarThumbWidth;
				case SystemResourceKeyID.IconWidth:
					return SystemParameters.IconWidth;
				case SystemResourceKeyID.IconHeight:
					return SystemParameters.IconHeight;
				case SystemResourceKeyID.IconGridWidth:
					return SystemParameters.IconGridWidth;
				case SystemResourceKeyID.IconGridHeight:
					return SystemParameters.IconGridHeight;
				case SystemResourceKeyID.MaximizedPrimaryScreenWidth:
					return SystemParameters.MaximizedPrimaryScreenWidth;
				case SystemResourceKeyID.MaximizedPrimaryScreenHeight:
					return SystemParameters.MaximizedPrimaryScreenHeight;
				case SystemResourceKeyID.MaximumWindowTrackWidth:
					return SystemParameters.MaximumWindowTrackWidth;
				case SystemResourceKeyID.MaximumWindowTrackHeight:
					return SystemParameters.MaximumWindowTrackHeight;
				case SystemResourceKeyID.MenuCheckmarkWidth:
					return SystemParameters.MenuCheckmarkWidth;
				case SystemResourceKeyID.MenuCheckmarkHeight:
					return SystemParameters.MenuCheckmarkHeight;
				case SystemResourceKeyID.MenuButtonWidth:
					return SystemParameters.MenuButtonWidth;
				case SystemResourceKeyID.MenuButtonHeight:
					return SystemParameters.MenuButtonHeight;
				case SystemResourceKeyID.MinimumWindowWidth:
					return SystemParameters.MinimumWindowWidth;
				case SystemResourceKeyID.MinimumWindowHeight:
					return SystemParameters.MinimumWindowHeight;
				case SystemResourceKeyID.MinimizedWindowWidth:
					return SystemParameters.MinimizedWindowWidth;
				case SystemResourceKeyID.MinimizedWindowHeight:
					return SystemParameters.MinimizedWindowHeight;
				case SystemResourceKeyID.MinimizedGridWidth:
					return SystemParameters.MinimizedGridWidth;
				case SystemResourceKeyID.MinimizedGridHeight:
					return SystemParameters.MinimizedGridHeight;
				case SystemResourceKeyID.MinimumWindowTrackWidth:
					return SystemParameters.MinimumWindowTrackWidth;
				case SystemResourceKeyID.MinimumWindowTrackHeight:
					return SystemParameters.MinimumWindowTrackHeight;
				case SystemResourceKeyID.PrimaryScreenWidth:
					return SystemParameters.PrimaryScreenWidth;
				case SystemResourceKeyID.PrimaryScreenHeight:
					return SystemParameters.PrimaryScreenHeight;
				case SystemResourceKeyID.WindowCaptionButtonWidth:
					return SystemParameters.WindowCaptionButtonWidth;
				case SystemResourceKeyID.WindowCaptionButtonHeight:
					return SystemParameters.WindowCaptionButtonHeight;
				case SystemResourceKeyID.ResizeFrameHorizontalBorderHeight:
					return SystemParameters.ResizeFrameHorizontalBorderHeight;
				case SystemResourceKeyID.ResizeFrameVerticalBorderWidth:
					return SystemParameters.ResizeFrameVerticalBorderWidth;
				case SystemResourceKeyID.SmallIconWidth:
					return SystemParameters.SmallIconWidth;
				case SystemResourceKeyID.SmallIconHeight:
					return SystemParameters.SmallIconHeight;
				case SystemResourceKeyID.SmallWindowCaptionButtonWidth:
					return SystemParameters.SmallWindowCaptionButtonWidth;
				case SystemResourceKeyID.SmallWindowCaptionButtonHeight:
					return SystemParameters.SmallWindowCaptionButtonHeight;
				case SystemResourceKeyID.VirtualScreenWidth:
					return SystemParameters.VirtualScreenWidth;
				case SystemResourceKeyID.VirtualScreenHeight:
					return SystemParameters.VirtualScreenHeight;
				case SystemResourceKeyID.VerticalScrollBarWidth:
					return SystemParameters.VerticalScrollBarWidth;
				case SystemResourceKeyID.VerticalScrollBarButtonHeight:
					return SystemParameters.VerticalScrollBarButtonHeight;
				case SystemResourceKeyID.WindowCaptionHeight:
					return SystemParameters.WindowCaptionHeight;
				case SystemResourceKeyID.KanjiWindowHeight:
					return SystemParameters.KanjiWindowHeight;
				case SystemResourceKeyID.MenuBarHeight:
					return SystemParameters.MenuBarHeight;
				case SystemResourceKeyID.SmallCaptionHeight:
					return SystemParameters.SmallCaptionHeight;
				case SystemResourceKeyID.VerticalScrollBarThumbHeight:
					return SystemParameters.VerticalScrollBarThumbHeight;
				case SystemResourceKeyID.IsImmEnabled:
					return BooleanBoxes.Box(SystemParameters.IsImmEnabled);
				case SystemResourceKeyID.IsMediaCenter:
					return BooleanBoxes.Box(SystemParameters.IsMediaCenter);
				case SystemResourceKeyID.IsMenuDropRightAligned:
					return BooleanBoxes.Box(SystemParameters.IsMenuDropRightAligned);
				case SystemResourceKeyID.IsMiddleEastEnabled:
					return BooleanBoxes.Box(SystemParameters.IsMiddleEastEnabled);
				case SystemResourceKeyID.IsMousePresent:
					return BooleanBoxes.Box(SystemParameters.IsMousePresent);
				case SystemResourceKeyID.IsMouseWheelPresent:
					return BooleanBoxes.Box(SystemParameters.IsMouseWheelPresent);
				case SystemResourceKeyID.IsPenWindows:
					return BooleanBoxes.Box(SystemParameters.IsPenWindows);
				case SystemResourceKeyID.IsRemotelyControlled:
					return BooleanBoxes.Box(SystemParameters.IsRemotelyControlled);
				case SystemResourceKeyID.IsRemoteSession:
					return BooleanBoxes.Box(SystemParameters.IsRemoteSession);
				case SystemResourceKeyID.ShowSounds:
					return BooleanBoxes.Box(SystemParameters.ShowSounds);
				case SystemResourceKeyID.IsSlowMachine:
					return BooleanBoxes.Box(SystemParameters.IsSlowMachine);
				case SystemResourceKeyID.SwapButtons:
					return BooleanBoxes.Box(SystemParameters.SwapButtons);
				case SystemResourceKeyID.IsTabletPC:
					return BooleanBoxes.Box(SystemParameters.IsTabletPC);
				case SystemResourceKeyID.VirtualScreenLeft:
					return SystemParameters.VirtualScreenLeft;
				case SystemResourceKeyID.VirtualScreenTop:
					return SystemParameters.VirtualScreenTop;
				case SystemResourceKeyID.FocusBorderWidth:
					return SystemParameters.FocusBorderWidth;
				case SystemResourceKeyID.FocusBorderHeight:
					return SystemParameters.FocusBorderHeight;
				case SystemResourceKeyID.HighContrast:
					return BooleanBoxes.Box(SystemParameters.HighContrast);
				case SystemResourceKeyID.DropShadow:
					return BooleanBoxes.Box(SystemParameters.DropShadow);
				case SystemResourceKeyID.FlatMenu:
					return BooleanBoxes.Box(SystemParameters.FlatMenu);
				case SystemResourceKeyID.WorkArea:
					return SystemParameters.WorkArea;
				case SystemResourceKeyID.IconHorizontalSpacing:
					return SystemParameters.IconHorizontalSpacing;
				case SystemResourceKeyID.IconVerticalSpacing:
					return SystemParameters.IconVerticalSpacing;
				case SystemResourceKeyID.IconTitleWrap:
					return SystemParameters.IconTitleWrap;
				case SystemResourceKeyID.KeyboardCues:
					return BooleanBoxes.Box(SystemParameters.KeyboardCues);
				case SystemResourceKeyID.KeyboardDelay:
					return SystemParameters.KeyboardDelay;
				case SystemResourceKeyID.KeyboardPreference:
					return BooleanBoxes.Box(SystemParameters.KeyboardPreference);
				case SystemResourceKeyID.KeyboardSpeed:
					return SystemParameters.KeyboardSpeed;
				case SystemResourceKeyID.SnapToDefaultButton:
					return BooleanBoxes.Box(SystemParameters.SnapToDefaultButton);
				case SystemResourceKeyID.WheelScrollLines:
					return SystemParameters.WheelScrollLines;
				case SystemResourceKeyID.MouseHoverTime:
					return SystemParameters.MouseHoverTime;
				case SystemResourceKeyID.MouseHoverHeight:
					return SystemParameters.MouseHoverHeight;
				case SystemResourceKeyID.MouseHoverWidth:
					return SystemParameters.MouseHoverWidth;
				case SystemResourceKeyID.MenuDropAlignment:
					return BooleanBoxes.Box(SystemParameters.MenuDropAlignment);
				case SystemResourceKeyID.MenuFade:
					return BooleanBoxes.Box(SystemParameters.MenuFade);
				case SystemResourceKeyID.MenuShowDelay:
					return SystemParameters.MenuShowDelay;
				case SystemResourceKeyID.ComboBoxAnimation:
					return BooleanBoxes.Box(SystemParameters.ComboBoxAnimation);
				case SystemResourceKeyID.ClientAreaAnimation:
					return BooleanBoxes.Box(SystemParameters.ClientAreaAnimation);
				case SystemResourceKeyID.CursorShadow:
					return BooleanBoxes.Box(SystemParameters.CursorShadow);
				case SystemResourceKeyID.GradientCaptions:
					return BooleanBoxes.Box(SystemParameters.GradientCaptions);
				case SystemResourceKeyID.HotTracking:
					return BooleanBoxes.Box(SystemParameters.HotTracking);
				case SystemResourceKeyID.ListBoxSmoothScrolling:
					return BooleanBoxes.Box(SystemParameters.ListBoxSmoothScrolling);
				case SystemResourceKeyID.MenuAnimation:
					return BooleanBoxes.Box(SystemParameters.MenuAnimation);
				case SystemResourceKeyID.SelectionFade:
					return BooleanBoxes.Box(SystemParameters.SelectionFade);
				case SystemResourceKeyID.StylusHotTracking:
					return BooleanBoxes.Box(SystemParameters.StylusHotTracking);
				case SystemResourceKeyID.ToolTipAnimation:
					return BooleanBoxes.Box(SystemParameters.ToolTipAnimation);
				case SystemResourceKeyID.ToolTipFade:
					return BooleanBoxes.Box(SystemParameters.ToolTipFade);
				case SystemResourceKeyID.UIEffects:
					return BooleanBoxes.Box(SystemParameters.UIEffects);
				case SystemResourceKeyID.MinimizeAnimation:
					return BooleanBoxes.Box(SystemParameters.MinimizeAnimation);
				case SystemResourceKeyID.Border:
					return SystemParameters.Border;
				case SystemResourceKeyID.CaretWidth:
					return SystemParameters.CaretWidth;
				case SystemResourceKeyID.ForegroundFlashCount:
					return SystemParameters.ForegroundFlashCount;
				case SystemResourceKeyID.DragFullWindows:
					return BooleanBoxes.Box(SystemParameters.DragFullWindows);
				case SystemResourceKeyID.BorderWidth:
					return SystemParameters.BorderWidth;
				case SystemResourceKeyID.ScrollWidth:
					return SystemParameters.ScrollWidth;
				case SystemResourceKeyID.ScrollHeight:
					return SystemParameters.ScrollHeight;
				case SystemResourceKeyID.CaptionWidth:
					return SystemParameters.CaptionWidth;
				case SystemResourceKeyID.CaptionHeight:
					return SystemParameters.CaptionHeight;
				case SystemResourceKeyID.SmallCaptionWidth:
					return SystemParameters.SmallCaptionWidth;
				case SystemResourceKeyID.MenuWidth:
					return SystemParameters.MenuWidth;
				case SystemResourceKeyID.MenuHeight:
					return SystemParameters.MenuHeight;
				case SystemResourceKeyID.ComboBoxPopupAnimation:
					return SystemParameters.ComboBoxPopupAnimation;
				case SystemResourceKeyID.MenuPopupAnimation:
					return SystemParameters.MenuPopupAnimation;
				case SystemResourceKeyID.ToolTipPopupAnimation:
					return SystemParameters.ToolTipPopupAnimation;
				case SystemResourceKeyID.PowerLineStatus:
					return SystemParameters.PowerLineStatus;
				case SystemResourceKeyID.InactiveSelectionHighlightBrush:
					return SystemColors.InactiveSelectionHighlightBrush;
				case SystemResourceKeyID.InactiveSelectionHighlightTextBrush:
					return SystemColors.InactiveSelectionHighlightTextBrush;
				}
				return null;
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00028DB8 File Offset: 0x00026FB8
		internal static ResourceKey GetResourceKey(short id)
		{
			switch (id)
			{
			case 1:
				return SystemColors.ActiveBorderBrushKey;
			case 2:
				return SystemColors.ActiveCaptionBrushKey;
			case 3:
				return SystemColors.ActiveCaptionTextBrushKey;
			case 4:
				return SystemColors.AppWorkspaceBrushKey;
			case 5:
				return SystemColors.ControlBrushKey;
			case 6:
				return SystemColors.ControlDarkBrushKey;
			case 7:
				return SystemColors.ControlDarkDarkBrushKey;
			case 8:
				return SystemColors.ControlLightBrushKey;
			case 9:
				return SystemColors.ControlLightLightBrushKey;
			case 10:
				return SystemColors.ControlTextBrushKey;
			case 11:
				return SystemColors.DesktopBrushKey;
			case 12:
				return SystemColors.GradientActiveCaptionBrushKey;
			case 13:
				return SystemColors.GradientInactiveCaptionBrushKey;
			case 14:
				return SystemColors.GrayTextBrushKey;
			case 15:
				return SystemColors.HighlightBrushKey;
			case 16:
				return SystemColors.HighlightTextBrushKey;
			case 17:
				return SystemColors.HotTrackBrushKey;
			case 18:
				return SystemColors.InactiveBorderBrushKey;
			case 19:
				return SystemColors.InactiveCaptionBrushKey;
			case 20:
				return SystemColors.InactiveCaptionTextBrushKey;
			case 21:
				return SystemColors.InfoBrushKey;
			case 22:
				return SystemColors.InfoTextBrushKey;
			case 23:
				return SystemColors.MenuBrushKey;
			case 24:
				return SystemColors.MenuBarBrushKey;
			case 25:
				return SystemColors.MenuHighlightBrushKey;
			case 26:
				return SystemColors.MenuTextBrushKey;
			case 27:
				return SystemColors.ScrollBarBrushKey;
			case 28:
				return SystemColors.WindowBrushKey;
			case 29:
				return SystemColors.WindowFrameBrushKey;
			case 30:
				return SystemColors.WindowTextBrushKey;
			case 31:
				return SystemColors.ActiveBorderColorKey;
			case 32:
				return SystemColors.ActiveCaptionColorKey;
			case 33:
				return SystemColors.ActiveCaptionTextColorKey;
			case 34:
				return SystemColors.AppWorkspaceColorKey;
			case 35:
				return SystemColors.ControlColorKey;
			case 36:
				return SystemColors.ControlDarkColorKey;
			case 37:
				return SystemColors.ControlDarkDarkColorKey;
			case 38:
				return SystemColors.ControlLightColorKey;
			case 39:
				return SystemColors.ControlLightLightColorKey;
			case 40:
				return SystemColors.ControlTextColorKey;
			case 41:
				return SystemColors.DesktopColorKey;
			case 42:
				return SystemColors.GradientActiveCaptionColorKey;
			case 43:
				return SystemColors.GradientInactiveCaptionColorKey;
			case 44:
				return SystemColors.GrayTextColorKey;
			case 45:
				return SystemColors.HighlightColorKey;
			case 46:
				return SystemColors.HighlightTextColorKey;
			case 47:
				return SystemColors.HotTrackColorKey;
			case 48:
				return SystemColors.InactiveBorderColorKey;
			case 49:
				return SystemColors.InactiveCaptionColorKey;
			case 50:
				return SystemColors.InactiveCaptionTextColorKey;
			case 51:
				return SystemColors.InfoColorKey;
			case 52:
				return SystemColors.InfoTextColorKey;
			case 53:
				return SystemColors.MenuColorKey;
			case 54:
				return SystemColors.MenuBarColorKey;
			case 55:
				return SystemColors.MenuHighlightColorKey;
			case 56:
				return SystemColors.MenuTextColorKey;
			case 57:
				return SystemColors.ScrollBarColorKey;
			case 58:
				return SystemColors.WindowColorKey;
			case 59:
				return SystemColors.WindowFrameColorKey;
			case 60:
				return SystemColors.WindowTextColorKey;
			case 63:
				return SystemFonts.CaptionFontSizeKey;
			case 64:
				return SystemFonts.CaptionFontFamilyKey;
			case 65:
				return SystemFonts.CaptionFontStyleKey;
			case 66:
				return SystemFonts.CaptionFontWeightKey;
			case 67:
				return SystemFonts.CaptionFontTextDecorationsKey;
			case 68:
				return SystemFonts.SmallCaptionFontSizeKey;
			case 69:
				return SystemFonts.SmallCaptionFontFamilyKey;
			case 70:
				return SystemFonts.SmallCaptionFontStyleKey;
			case 71:
				return SystemFonts.SmallCaptionFontWeightKey;
			case 72:
				return SystemFonts.SmallCaptionFontTextDecorationsKey;
			case 73:
				return SystemFonts.MenuFontSizeKey;
			case 74:
				return SystemFonts.MenuFontFamilyKey;
			case 75:
				return SystemFonts.MenuFontStyleKey;
			case 76:
				return SystemFonts.MenuFontWeightKey;
			case 77:
				return SystemFonts.MenuFontTextDecorationsKey;
			case 78:
				return SystemFonts.StatusFontSizeKey;
			case 79:
				return SystemFonts.StatusFontFamilyKey;
			case 80:
				return SystemFonts.StatusFontStyleKey;
			case 81:
				return SystemFonts.StatusFontWeightKey;
			case 82:
				return SystemFonts.StatusFontTextDecorationsKey;
			case 83:
				return SystemFonts.MessageFontSizeKey;
			case 84:
				return SystemFonts.MessageFontFamilyKey;
			case 85:
				return SystemFonts.MessageFontStyleKey;
			case 86:
				return SystemFonts.MessageFontWeightKey;
			case 87:
				return SystemFonts.MessageFontTextDecorationsKey;
			case 88:
				return SystemFonts.IconFontSizeKey;
			case 89:
				return SystemFonts.IconFontFamilyKey;
			case 90:
				return SystemFonts.IconFontStyleKey;
			case 91:
				return SystemFonts.IconFontWeightKey;
			case 92:
				return SystemFonts.IconFontTextDecorationsKey;
			case 95:
				return SystemParameters.ThinHorizontalBorderHeightKey;
			case 96:
				return SystemParameters.ThinVerticalBorderWidthKey;
			case 97:
				return SystemParameters.CursorWidthKey;
			case 98:
				return SystemParameters.CursorHeightKey;
			case 99:
				return SystemParameters.ThickHorizontalBorderHeightKey;
			case 100:
				return SystemParameters.ThickVerticalBorderWidthKey;
			case 101:
				return SystemParameters.FixedFrameHorizontalBorderHeightKey;
			case 102:
				return SystemParameters.FixedFrameVerticalBorderWidthKey;
			case 103:
				return SystemParameters.FocusHorizontalBorderHeightKey;
			case 104:
				return SystemParameters.FocusVerticalBorderWidthKey;
			case 105:
				return SystemParameters.FullPrimaryScreenWidthKey;
			case 106:
				return SystemParameters.FullPrimaryScreenHeightKey;
			case 107:
				return SystemParameters.HorizontalScrollBarButtonWidthKey;
			case 108:
				return SystemParameters.HorizontalScrollBarHeightKey;
			case 109:
				return SystemParameters.HorizontalScrollBarThumbWidthKey;
			case 110:
				return SystemParameters.IconWidthKey;
			case 111:
				return SystemParameters.IconHeightKey;
			case 112:
				return SystemParameters.IconGridWidthKey;
			case 113:
				return SystemParameters.IconGridHeightKey;
			case 114:
				return SystemParameters.MaximizedPrimaryScreenWidthKey;
			case 115:
				return SystemParameters.MaximizedPrimaryScreenHeightKey;
			case 116:
				return SystemParameters.MaximumWindowTrackWidthKey;
			case 117:
				return SystemParameters.MaximumWindowTrackHeightKey;
			case 118:
				return SystemParameters.MenuCheckmarkWidthKey;
			case 119:
				return SystemParameters.MenuCheckmarkHeightKey;
			case 120:
				return SystemParameters.MenuButtonWidthKey;
			case 121:
				return SystemParameters.MenuButtonHeightKey;
			case 122:
				return SystemParameters.MinimumWindowWidthKey;
			case 123:
				return SystemParameters.MinimumWindowHeightKey;
			case 124:
				return SystemParameters.MinimizedWindowWidthKey;
			case 125:
				return SystemParameters.MinimizedWindowHeightKey;
			case 126:
				return SystemParameters.MinimizedGridWidthKey;
			case 127:
				return SystemParameters.MinimizedGridHeightKey;
			case 128:
				return SystemParameters.MinimumWindowTrackWidthKey;
			case 129:
				return SystemParameters.MinimumWindowTrackHeightKey;
			case 130:
				return SystemParameters.PrimaryScreenWidthKey;
			case 131:
				return SystemParameters.PrimaryScreenHeightKey;
			case 132:
				return SystemParameters.WindowCaptionButtonWidthKey;
			case 133:
				return SystemParameters.WindowCaptionButtonHeightKey;
			case 134:
				return SystemParameters.ResizeFrameHorizontalBorderHeightKey;
			case 135:
				return SystemParameters.ResizeFrameVerticalBorderWidthKey;
			case 136:
				return SystemParameters.SmallIconWidthKey;
			case 137:
				return SystemParameters.SmallIconHeightKey;
			case 138:
				return SystemParameters.SmallWindowCaptionButtonWidthKey;
			case 139:
				return SystemParameters.SmallWindowCaptionButtonHeightKey;
			case 140:
				return SystemParameters.VirtualScreenWidthKey;
			case 141:
				return SystemParameters.VirtualScreenHeightKey;
			case 142:
				return SystemParameters.VerticalScrollBarWidthKey;
			case 143:
				return SystemParameters.VerticalScrollBarButtonHeightKey;
			case 144:
				return SystemParameters.WindowCaptionHeightKey;
			case 145:
				return SystemParameters.KanjiWindowHeightKey;
			case 146:
				return SystemParameters.MenuBarHeightKey;
			case 147:
				return SystemParameters.SmallCaptionHeightKey;
			case 148:
				return SystemParameters.VerticalScrollBarThumbHeightKey;
			case 149:
				return SystemParameters.IsImmEnabledKey;
			case 150:
				return SystemParameters.IsMediaCenterKey;
			case 151:
				return SystemParameters.IsMenuDropRightAlignedKey;
			case 152:
				return SystemParameters.IsMiddleEastEnabledKey;
			case 153:
				return SystemParameters.IsMousePresentKey;
			case 154:
				return SystemParameters.IsMouseWheelPresentKey;
			case 155:
				return SystemParameters.IsPenWindowsKey;
			case 156:
				return SystemParameters.IsRemotelyControlledKey;
			case 157:
				return SystemParameters.IsRemoteSessionKey;
			case 158:
				return SystemParameters.ShowSoundsKey;
			case 159:
				return SystemParameters.IsSlowMachineKey;
			case 160:
				return SystemParameters.SwapButtonsKey;
			case 161:
				return SystemParameters.IsTabletPCKey;
			case 162:
				return SystemParameters.VirtualScreenLeftKey;
			case 163:
				return SystemParameters.VirtualScreenTopKey;
			case 164:
				return SystemParameters.FocusBorderWidthKey;
			case 165:
				return SystemParameters.FocusBorderHeightKey;
			case 166:
				return SystemParameters.HighContrastKey;
			case 167:
				return SystemParameters.DropShadowKey;
			case 168:
				return SystemParameters.FlatMenuKey;
			case 169:
				return SystemParameters.WorkAreaKey;
			case 170:
				return SystemParameters.IconHorizontalSpacingKey;
			case 171:
				return SystemParameters.IconVerticalSpacingKey;
			case 172:
				return SystemParameters.IconTitleWrapKey;
			case 173:
				return SystemParameters.KeyboardCuesKey;
			case 174:
				return SystemParameters.KeyboardDelayKey;
			case 175:
				return SystemParameters.KeyboardPreferenceKey;
			case 176:
				return SystemParameters.KeyboardSpeedKey;
			case 177:
				return SystemParameters.SnapToDefaultButtonKey;
			case 178:
				return SystemParameters.WheelScrollLinesKey;
			case 179:
				return SystemParameters.MouseHoverTimeKey;
			case 180:
				return SystemParameters.MouseHoverHeightKey;
			case 181:
				return SystemParameters.MouseHoverWidthKey;
			case 182:
				return SystemParameters.MenuDropAlignmentKey;
			case 183:
				return SystemParameters.MenuFadeKey;
			case 184:
				return SystemParameters.MenuShowDelayKey;
			case 185:
				return SystemParameters.ComboBoxAnimationKey;
			case 186:
				return SystemParameters.ClientAreaAnimationKey;
			case 187:
				return SystemParameters.CursorShadowKey;
			case 188:
				return SystemParameters.GradientCaptionsKey;
			case 189:
				return SystemParameters.HotTrackingKey;
			case 190:
				return SystemParameters.ListBoxSmoothScrollingKey;
			case 191:
				return SystemParameters.MenuAnimationKey;
			case 192:
				return SystemParameters.SelectionFadeKey;
			case 193:
				return SystemParameters.StylusHotTrackingKey;
			case 194:
				return SystemParameters.ToolTipAnimationKey;
			case 195:
				return SystemParameters.ToolTipFadeKey;
			case 196:
				return SystemParameters.UIEffectsKey;
			case 197:
				return SystemParameters.MinimizeAnimationKey;
			case 198:
				return SystemParameters.BorderKey;
			case 199:
				return SystemParameters.CaretWidthKey;
			case 200:
				return SystemParameters.ForegroundFlashCountKey;
			case 201:
				return SystemParameters.DragFullWindowsKey;
			case 202:
				return SystemParameters.BorderWidthKey;
			case 203:
				return SystemParameters.ScrollWidthKey;
			case 204:
				return SystemParameters.ScrollHeightKey;
			case 205:
				return SystemParameters.CaptionWidthKey;
			case 206:
				return SystemParameters.CaptionHeightKey;
			case 207:
				return SystemParameters.SmallCaptionWidthKey;
			case 208:
				return SystemParameters.MenuWidthKey;
			case 209:
				return SystemParameters.MenuHeightKey;
			case 210:
				return SystemParameters.ComboBoxPopupAnimationKey;
			case 211:
				return SystemParameters.MenuPopupAnimationKey;
			case 212:
				return SystemParameters.ToolTipPopupAnimationKey;
			case 213:
				return SystemParameters.PowerLineStatusKey;
			case 215:
				return SystemParameters.FocusVisualStyleKey;
			case 216:
				return SystemParameters.NavigationChromeDownLevelStyleKey;
			case 217:
				return SystemParameters.NavigationChromeStyleKey;
			case 219:
				return MenuItem.SeparatorStyleKey;
			case 220:
				return GridView.GridViewScrollViewerStyleKey;
			case 221:
				return GridView.GridViewStyleKey;
			case 222:
				return GridView.GridViewItemContainerStyleKey;
			case 223:
				return StatusBar.SeparatorStyleKey;
			case 224:
				return ToolBar.ButtonStyleKey;
			case 225:
				return ToolBar.ToggleButtonStyleKey;
			case 226:
				return ToolBar.SeparatorStyleKey;
			case 227:
				return ToolBar.CheckBoxStyleKey;
			case 228:
				return ToolBar.RadioButtonStyleKey;
			case 229:
				return ToolBar.ComboBoxStyleKey;
			case 230:
				return ToolBar.TextBoxStyleKey;
			case 231:
				return ToolBar.MenuStyleKey;
			case 234:
				return SystemColors.InactiveSelectionHighlightBrushKey;
			case 235:
				return SystemColors.InactiveSelectionHighlightTextBrushKey;
			}
			return null;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000296D4 File Offset: 0x000278D4
		internal static ResourceKey GetSystemResourceKey(string keyName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(keyName);
			if (num <= 2045857616U)
			{
				if (num <= 705184480U)
				{
					if (num <= 461950501U)
					{
						if (num != 145556601U)
						{
							if (num == 461950501U)
							{
								if (keyName == "StatusBar.SeparatorStyleKey")
								{
									return SystemResourceKey.StatusBarSeparatorStyleKey;
								}
							}
						}
						else if (keyName == "DataGridComboBoxColumn.TextBlockComboBoxStyleKey")
						{
							return SystemResourceKey.DataGridComboBoxColumnTextBlockComboBoxStyleKey;
						}
					}
					else if (num != 493495474U)
					{
						if (num != 643203629U)
						{
							if (num == 705184480U)
							{
								if (keyName == "MenuItem.SeparatorStyleKey")
								{
									return SystemResourceKey.MenuItemSeparatorStyleKey;
								}
							}
						}
						else if (keyName == "ToolBar.RadioButtonStyleKey")
						{
							return SystemResourceKey.ToolBarRadioButtonStyleKey;
						}
					}
					else if (keyName == "DataGrid.FocusBorderBrushKey")
					{
						return SystemResourceKey.DataGridFocusBorderBrushKey;
					}
				}
				else if (num <= 1217509750U)
				{
					if (num != 1180275602U)
					{
						if (num == 1217509750U)
						{
							if (keyName == "SystemParameters.NavigationChromeStyleKey")
							{
								return SystemParameters.NavigationChromeStyleKey;
							}
						}
					}
					else if (keyName == "ToolBar.TextBoxStyleKey")
					{
						return SystemResourceKey.ToolBarTextBoxStyleKey;
					}
				}
				else if (num != 1247245809U)
				{
					if (num != 1661977326U)
					{
						if (num == 2045857616U)
						{
							if (keyName == "GridView.GridViewScrollViewerStyleKey")
							{
								return SystemResourceKey.GridViewScrollViewerStyleKey;
							}
						}
					}
					else if (keyName == "SystemParameters.FocusVisualStyleKey")
					{
						return SystemParameters.FocusVisualStyleKey;
					}
				}
				else if (keyName == "DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey")
				{
					return SystemResourceKey.DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey;
				}
			}
			else if (num <= 2828802701U)
			{
				if (num <= 2379497950U)
				{
					if (num != 2175658447U)
					{
						if (num == 2379497950U)
						{
							if (keyName == "ToolBar.ToggleButtonStyleKey")
							{
								return SystemResourceKey.ToolBarToggleButtonStyleKey;
							}
						}
					}
					else if (keyName == "GridView.GridViewItemContainerStyleKey")
					{
						return SystemResourceKey.GridViewItemContainerStyleKey;
					}
				}
				else if (num != 2729588653U)
				{
					if (num != 2796646438U)
					{
						if (num == 2828802701U)
						{
							if (keyName == "ToolBar.SeparatorStyleKey")
							{
								return SystemResourceKey.ToolBarSeparatorStyleKey;
							}
						}
					}
					else if (keyName == "ToolBar.ButtonStyleKey")
					{
						return SystemResourceKey.ToolBarButtonStyleKey;
					}
				}
				else if (keyName == "ToolBar.MenuStyleKey")
				{
					return SystemResourceKey.ToolBarMenuStyleKey;
				}
			}
			else if (num <= 3227933789U)
			{
				if (num != 3124717228U)
				{
					if (num == 3227933789U)
					{
						if (keyName == "ToolBar.ComboBoxStyleKey")
						{
							return SystemResourceKey.ToolBarComboBoxStyleKey;
						}
					}
				}
				else if (keyName == "SystemParameters.NavigationChromeDownLevelStyleKey")
				{
					return SystemParameters.NavigationChromeDownLevelStyleKey;
				}
			}
			else if (num != 3609136063U)
			{
				if (num != 3884806943U)
				{
					if (num == 3928176313U)
					{
						if (keyName == "DataGridColumnHeader.ColumnFloatingHeaderStyleKey")
						{
							return SystemResourceKey.DataGridColumnHeaderColumnFloatingHeaderStyleKey;
						}
					}
				}
				else if (keyName == "GridView.GridViewStyleKey")
				{
					return SystemResourceKey.GridViewStyleKey;
				}
			}
			else if (keyName == "ToolBar.CheckBoxStyleKey")
			{
				return SystemResourceKey.ToolBarCheckBoxStyleKey;
			}
			return null;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00029A38 File Offset: 0x00027C38
		internal static object GetResource(short id)
		{
			if (SystemResourceKey._srk == null)
			{
				SystemResourceKey._srk = new SystemResourceKey((SystemResourceKeyID)id);
			}
			else
			{
				SystemResourceKey._srk._id = (SystemResourceKeyID)id;
			}
			return SystemResourceKey._srk.Resource;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00029A70 File Offset: 0x00027C70
		internal SystemResourceKey(SystemResourceKeyID id)
		{
			this._id = id;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00029A7F File Offset: 0x00027C7F
		internal SystemResourceKeyID InternalKey
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0000C238 File Offset: 0x0000A438
		public override Assembly Assembly
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00029A88 File Offset: 0x00027C88
		public override bool Equals(object o)
		{
			SystemResourceKey systemResourceKey = o as SystemResourceKey;
			return systemResourceKey != null && systemResourceKey._id == this._id;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00029A7F File Offset: 0x00027C7F
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00029AAF File Offset: 0x00027CAF
		public override string ToString()
		{
			return this._id.ToString();
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00029AC2 File Offset: 0x00027CC2
		internal static ComponentResourceKey DataGridFocusBorderBrushKey
		{
			get
			{
				if (SystemResourceKey._focusBorderBrushKey == null)
				{
					SystemResourceKey._focusBorderBrushKey = new ComponentResourceKey(typeof(DataGrid), "FocusBorderBrushKey");
				}
				return SystemResourceKey._focusBorderBrushKey;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00029AE9 File Offset: 0x00027CE9
		internal static ComponentResourceKey DataGridComboBoxColumnTextBlockComboBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._textBlockComboBoxStyleKey == null)
				{
					SystemResourceKey._textBlockComboBoxStyleKey = new ComponentResourceKey(typeof(DataGrid), "TextBlockComboBoxStyleKey");
				}
				return SystemResourceKey._textBlockComboBoxStyleKey;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00029B10 File Offset: 0x00027D10
		internal static ResourceKey MenuItemSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._menuItemSeparatorStyleKey == null)
				{
					SystemResourceKey._menuItemSeparatorStyleKey = new SystemThemeKey(SystemResourceKeyID.MenuItemSeparatorStyle);
				}
				return SystemResourceKey._menuItemSeparatorStyleKey;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00029B2D File Offset: 0x00027D2D
		internal static ComponentResourceKey DataGridColumnHeaderColumnFloatingHeaderStyleKey
		{
			get
			{
				if (SystemResourceKey._columnFloatingHeaderStyleKey == null)
				{
					SystemResourceKey._columnFloatingHeaderStyleKey = new ComponentResourceKey(typeof(DataGrid), "ColumnFloatingHeaderStyleKey");
				}
				return SystemResourceKey._columnFloatingHeaderStyleKey;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00029B54 File Offset: 0x00027D54
		internal static ComponentResourceKey DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._columnHeaderDropSeparatorStyleKey == null)
				{
					SystemResourceKey._columnHeaderDropSeparatorStyleKey = new ComponentResourceKey(typeof(DataGrid), "ColumnHeaderDropSeparatorStyleKey");
				}
				return SystemResourceKey._columnHeaderDropSeparatorStyleKey;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00029B7B File Offset: 0x00027D7B
		internal static ResourceKey GridViewItemContainerStyleKey
		{
			get
			{
				if (SystemResourceKey._gridViewItemContainerStyleKey == null)
				{
					SystemResourceKey._gridViewItemContainerStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewItemContainerStyle);
				}
				return SystemResourceKey._gridViewItemContainerStyleKey;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00029B98 File Offset: 0x00027D98
		internal static ResourceKey GridViewScrollViewerStyleKey
		{
			get
			{
				if (SystemResourceKey._scrollViewerStyleKey == null)
				{
					SystemResourceKey._scrollViewerStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewScrollViewerStyle);
				}
				return SystemResourceKey._scrollViewerStyleKey;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00029BB5 File Offset: 0x00027DB5
		internal static ResourceKey GridViewStyleKey
		{
			get
			{
				if (SystemResourceKey._gridViewStyleKey == null)
				{
					SystemResourceKey._gridViewStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewStyle);
				}
				return SystemResourceKey._gridViewStyleKey;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00029BD2 File Offset: 0x00027DD2
		internal static ResourceKey StatusBarSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._statusBarSeparatorStyleKey == null)
				{
					SystemResourceKey._statusBarSeparatorStyleKey = new SystemThemeKey(SystemResourceKeyID.StatusBarSeparatorStyle);
				}
				return SystemResourceKey._statusBarSeparatorStyleKey;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00029BEF File Offset: 0x00027DEF
		internal static ResourceKey ToolBarButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheButtonStyle == null)
				{
					SystemResourceKey._cacheButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarButtonStyle);
				}
				return SystemResourceKey._cacheButtonStyle;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00029C0C File Offset: 0x00027E0C
		internal static ResourceKey ToolBarToggleButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheToggleButtonStyle == null)
				{
					SystemResourceKey._cacheToggleButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarToggleButtonStyle);
				}
				return SystemResourceKey._cacheToggleButtonStyle;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00029C29 File Offset: 0x00027E29
		internal static ResourceKey ToolBarSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheSeparatorStyle == null)
				{
					SystemResourceKey._cacheSeparatorStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarSeparatorStyle);
				}
				return SystemResourceKey._cacheSeparatorStyle;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00029C46 File Offset: 0x00027E46
		internal static ResourceKey ToolBarCheckBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheCheckBoxStyle == null)
				{
					SystemResourceKey._cacheCheckBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarCheckBoxStyle);
				}
				return SystemResourceKey._cacheCheckBoxStyle;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00029C63 File Offset: 0x00027E63
		internal static ResourceKey ToolBarRadioButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheRadioButtonStyle == null)
				{
					SystemResourceKey._cacheRadioButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarRadioButtonStyle);
				}
				return SystemResourceKey._cacheRadioButtonStyle;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x00029C80 File Offset: 0x00027E80
		internal static ResourceKey ToolBarComboBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheComboBoxStyle == null)
				{
					SystemResourceKey._cacheComboBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarComboBoxStyle);
				}
				return SystemResourceKey._cacheComboBoxStyle;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00029C9D File Offset: 0x00027E9D
		internal static ResourceKey ToolBarTextBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheTextBoxStyle == null)
				{
					SystemResourceKey._cacheTextBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarTextBoxStyle);
				}
				return SystemResourceKey._cacheTextBoxStyle;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x00029CBA File Offset: 0x00027EBA
		internal static ResourceKey ToolBarMenuStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheMenuStyle == null)
				{
					SystemResourceKey._cacheMenuStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarMenuStyle);
				}
				return SystemResourceKey._cacheMenuStyle;
			}
		}

		// Token: 0x04000A7F RID: 2687
		private const short SystemResourceKeyIDStart = 0;

		// Token: 0x04000A80 RID: 2688
		private const short SystemResourceKeyIDEnd = 232;

		// Token: 0x04000A81 RID: 2689
		private const short SystemResourceKeyIDExtendedStart = 233;

		// Token: 0x04000A82 RID: 2690
		private const short SystemResourceKeyIDExtendedEnd = 236;

		// Token: 0x04000A83 RID: 2691
		private const short SystemResourceKeyBAMLIDStart = 0;

		// Token: 0x04000A84 RID: 2692
		private const short SystemResourceKeyBAMLIDEnd = 232;

		// Token: 0x04000A85 RID: 2693
		private const short SystemResourceBAMLIDStart = 232;

		// Token: 0x04000A86 RID: 2694
		private const short SystemResourceBAMLIDEnd = 464;

		// Token: 0x04000A87 RID: 2695
		private const short SystemResourceKeyBAMLIDExtendedStart = 464;

		// Token: 0x04000A88 RID: 2696
		private const short SystemResourceKeyBAMLIDExtendedEnd = 467;

		// Token: 0x04000A89 RID: 2697
		private const short SystemResourceBAMLIDExtendedStart = 467;

		// Token: 0x04000A8A RID: 2698
		private const short SystemResourceBAMLIDExtendedEnd = 470;

		// Token: 0x04000A8B RID: 2699
		private static SystemThemeKey _cacheSeparatorStyle;

		// Token: 0x04000A8C RID: 2700
		private static SystemThemeKey _cacheCheckBoxStyle;

		// Token: 0x04000A8D RID: 2701
		private static SystemThemeKey _cacheToggleButtonStyle;

		// Token: 0x04000A8E RID: 2702
		private static SystemThemeKey _cacheButtonStyle;

		// Token: 0x04000A8F RID: 2703
		private static SystemThemeKey _cacheRadioButtonStyle;

		// Token: 0x04000A90 RID: 2704
		private static SystemThemeKey _cacheComboBoxStyle;

		// Token: 0x04000A91 RID: 2705
		private static SystemThemeKey _cacheTextBoxStyle;

		// Token: 0x04000A92 RID: 2706
		private static SystemThemeKey _cacheMenuStyle;

		// Token: 0x04000A93 RID: 2707
		private static ComponentResourceKey _focusBorderBrushKey;

		// Token: 0x04000A94 RID: 2708
		private static ComponentResourceKey _textBlockComboBoxStyleKey;

		// Token: 0x04000A95 RID: 2709
		private static SystemThemeKey _menuItemSeparatorStyleKey;

		// Token: 0x04000A96 RID: 2710
		private static ComponentResourceKey _columnHeaderDropSeparatorStyleKey;

		// Token: 0x04000A97 RID: 2711
		private static ComponentResourceKey _columnFloatingHeaderStyleKey;

		// Token: 0x04000A98 RID: 2712
		private static SystemThemeKey _gridViewItemContainerStyleKey;

		// Token: 0x04000A99 RID: 2713
		private static SystemThemeKey _scrollViewerStyleKey;

		// Token: 0x04000A9A RID: 2714
		private static SystemThemeKey _gridViewStyleKey;

		// Token: 0x04000A9B RID: 2715
		private static SystemThemeKey _statusBarSeparatorStyleKey;

		// Token: 0x04000A9C RID: 2716
		private SystemResourceKeyID _id;

		// Token: 0x04000A9D RID: 2717
		[ThreadStatic]
		private static SystemResourceKey _srk;
	}
}
