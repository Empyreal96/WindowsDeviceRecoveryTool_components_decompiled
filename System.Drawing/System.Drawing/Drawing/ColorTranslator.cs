using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing
{
	/// <summary>Translates colors to and from GDI+ <see cref="T:System.Drawing.Color" /> structures. This class cannot be inherited.</summary>
	// Token: 0x0200003A RID: 58
	public sealed class ColorTranslator
	{
		// Token: 0x060005AE RID: 1454 RVA: 0x00003800 File Offset: 0x00001A00
		private ColorTranslator()
		{
		}

		/// <summary>Translates the specified <see cref="T:System.Drawing.Color" /> structure to a Windows color.</summary>
		/// <param name="c">The <see cref="T:System.Drawing.Color" /> structure to translate. </param>
		/// <returns>The Windows color value.</returns>
		// Token: 0x060005AF RID: 1455 RVA: 0x000188B2 File Offset: 0x00016AB2
		public static int ToWin32(Color c)
		{
			return (int)c.R | (int)c.G << 8 | (int)c.B << 16;
		}

		/// <summary>Translates the specified <see cref="T:System.Drawing.Color" /> structure to an OLE color.</summary>
		/// <param name="c">The <see cref="T:System.Drawing.Color" /> structure to translate. </param>
		/// <returns>The OLE color value.</returns>
		// Token: 0x060005B0 RID: 1456 RVA: 0x000188D0 File Offset: 0x00016AD0
		public static int ToOle(Color c)
		{
			if (c.IsKnownColor)
			{
				KnownColor knownColor = c.ToKnownColor();
				switch (knownColor)
				{
				case KnownColor.ActiveBorder:
					return -2147483638;
				case KnownColor.ActiveCaption:
					return -2147483646;
				case KnownColor.ActiveCaptionText:
					return -2147483639;
				case KnownColor.AppWorkspace:
					return -2147483636;
				case KnownColor.Control:
					return -2147483633;
				case KnownColor.ControlDark:
					return -2147483632;
				case KnownColor.ControlDarkDark:
					return -2147483627;
				case KnownColor.ControlLight:
					return -2147483626;
				case KnownColor.ControlLightLight:
					return -2147483628;
				case KnownColor.ControlText:
					return -2147483630;
				case KnownColor.Desktop:
					return -2147483647;
				case KnownColor.GrayText:
					return -2147483631;
				case KnownColor.Highlight:
					return -2147483635;
				case KnownColor.HighlightText:
					return -2147483634;
				case KnownColor.HotTrack:
					return -2147483622;
				case KnownColor.InactiveBorder:
					return -2147483637;
				case KnownColor.InactiveCaption:
					return -2147483645;
				case KnownColor.InactiveCaptionText:
					return -2147483629;
				case KnownColor.Info:
					return -2147483624;
				case KnownColor.InfoText:
					return -2147483625;
				case KnownColor.Menu:
					return -2147483644;
				case KnownColor.MenuText:
					return -2147483641;
				case KnownColor.ScrollBar:
					return int.MinValue;
				case KnownColor.Window:
					return -2147483643;
				case KnownColor.WindowFrame:
					return -2147483642;
				case KnownColor.WindowText:
					return -2147483640;
				default:
					switch (knownColor)
					{
					case KnownColor.ButtonFace:
						return -2147483633;
					case KnownColor.ButtonHighlight:
						return -2147483628;
					case KnownColor.ButtonShadow:
						return -2147483632;
					case KnownColor.GradientActiveCaption:
						return -2147483621;
					case KnownColor.GradientInactiveCaption:
						return -2147483620;
					case KnownColor.MenuBar:
						return -2147483618;
					case KnownColor.MenuHighlight:
						return -2147483619;
					}
					break;
				}
			}
			return ColorTranslator.ToWin32(c);
		}

		/// <summary>Translates an OLE color value to a GDI+ <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="oleColor">The OLE color to translate. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that represents the translated OLE color.</returns>
		// Token: 0x060005B1 RID: 1457 RVA: 0x00018A5C File Offset: 0x00016C5C
		public static Color FromOle(int oleColor)
		{
			if ((int)((long)oleColor & (long)((ulong)-16777216)) == -2147483648 && (oleColor & 16777215) <= 30)
			{
				switch (oleColor)
				{
				case -2147483648:
					return Color.FromKnownColor(KnownColor.ScrollBar);
				case -2147483647:
					return Color.FromKnownColor(KnownColor.Desktop);
				case -2147483646:
					return Color.FromKnownColor(KnownColor.ActiveCaption);
				case -2147483645:
					return Color.FromKnownColor(KnownColor.InactiveCaption);
				case -2147483644:
					return Color.FromKnownColor(KnownColor.Menu);
				case -2147483643:
					return Color.FromKnownColor(KnownColor.Window);
				case -2147483642:
					return Color.FromKnownColor(KnownColor.WindowFrame);
				case -2147483641:
					return Color.FromKnownColor(KnownColor.MenuText);
				case -2147483640:
					return Color.FromKnownColor(KnownColor.WindowText);
				case -2147483639:
					return Color.FromKnownColor(KnownColor.ActiveCaptionText);
				case -2147483638:
					return Color.FromKnownColor(KnownColor.ActiveBorder);
				case -2147483637:
					return Color.FromKnownColor(KnownColor.InactiveBorder);
				case -2147483636:
					return Color.FromKnownColor(KnownColor.AppWorkspace);
				case -2147483635:
					return Color.FromKnownColor(KnownColor.Highlight);
				case -2147483634:
					return Color.FromKnownColor(KnownColor.HighlightText);
				case -2147483633:
					return Color.FromKnownColor(KnownColor.Control);
				case -2147483632:
					return Color.FromKnownColor(KnownColor.ControlDark);
				case -2147483631:
					return Color.FromKnownColor(KnownColor.GrayText);
				case -2147483630:
					return Color.FromKnownColor(KnownColor.ControlText);
				case -2147483629:
					return Color.FromKnownColor(KnownColor.InactiveCaptionText);
				case -2147483628:
					return Color.FromKnownColor(KnownColor.ControlLightLight);
				case -2147483627:
					return Color.FromKnownColor(KnownColor.ControlDarkDark);
				case -2147483626:
					return Color.FromKnownColor(KnownColor.ControlLight);
				case -2147483625:
					return Color.FromKnownColor(KnownColor.InfoText);
				case -2147483624:
					return Color.FromKnownColor(KnownColor.Info);
				case -2147483622:
					return Color.FromKnownColor(KnownColor.HotTrack);
				case -2147483621:
					return Color.FromKnownColor(KnownColor.GradientActiveCaption);
				case -2147483620:
					return Color.FromKnownColor(KnownColor.GradientInactiveCaption);
				case -2147483619:
					return Color.FromKnownColor(KnownColor.MenuHighlight);
				case -2147483618:
					return Color.FromKnownColor(KnownColor.MenuBar);
				}
			}
			return KnownColorTable.ArgbToKnownColor(Color.FromArgb((int)((byte)(oleColor & 255)), (int)((byte)(oleColor >> 8 & 255)), (int)((byte)(oleColor >> 16 & 255))).ToArgb());
		}

		/// <summary>Translates a Windows color value to a GDI+ <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="win32Color">The Windows color to translate. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that represents the translated Windows color.</returns>
		// Token: 0x060005B2 RID: 1458 RVA: 0x00018C3B File Offset: 0x00016E3B
		public static Color FromWin32(int win32Color)
		{
			return ColorTranslator.FromOle(win32Color);
		}

		/// <summary>Translates an HTML color representation to a GDI+ <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="htmlColor">The string representation of the Html color to translate. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that represents the translated HTML color or <see cref="F:System.Drawing.Color.Empty" /> if <paramref name="htmlColor" /> is <see langword="null" />.</returns>
		/// <exception cref="T:System.Exception">
		///         <paramref name="htmlColor" /> is not a valid HTML color name.</exception>
		// Token: 0x060005B3 RID: 1459 RVA: 0x00018C44 File Offset: 0x00016E44
		public static Color FromHtml(string htmlColor)
		{
			Color result = Color.Empty;
			if (htmlColor == null || htmlColor.Length == 0)
			{
				return result;
			}
			if (htmlColor[0] == '#' && (htmlColor.Length == 7 || htmlColor.Length == 4))
			{
				if (htmlColor.Length == 7)
				{
					result = Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16), Convert.ToInt32(htmlColor.Substring(3, 2), 16), Convert.ToInt32(htmlColor.Substring(5, 2), 16));
				}
				else
				{
					string text = char.ToString(htmlColor[1]);
					string text2 = char.ToString(htmlColor[2]);
					string text3 = char.ToString(htmlColor[3]);
					result = Color.FromArgb(Convert.ToInt32(text + text, 16), Convert.ToInt32(text2 + text2, 16), Convert.ToInt32(text3 + text3, 16));
				}
			}
			if (result.IsEmpty && string.Equals(htmlColor, "LightGrey", StringComparison.OrdinalIgnoreCase))
			{
				result = Color.LightGray;
			}
			if (result.IsEmpty)
			{
				if (ColorTranslator.htmlSysColorTable == null)
				{
					ColorTranslator.InitializeHtmlSysColorTable();
				}
				object obj = ColorTranslator.htmlSysColorTable[htmlColor.ToLower(CultureInfo.InvariantCulture)];
				if (obj != null)
				{
					result = (Color)obj;
				}
			}
			if (result.IsEmpty)
			{
				result = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(htmlColor);
			}
			return result;
		}

		/// <summary>Translates the specified <see cref="T:System.Drawing.Color" /> structure to an HTML string color representation.</summary>
		/// <param name="c">The <see cref="T:System.Drawing.Color" /> structure to translate. </param>
		/// <returns>The string that represents the HTML color.</returns>
		// Token: 0x060005B4 RID: 1460 RVA: 0x00018D98 File Offset: 0x00016F98
		public static string ToHtml(Color c)
		{
			string result = string.Empty;
			if (c.IsEmpty)
			{
				return result;
			}
			if (c.IsSystemColor)
			{
				KnownColor knownColor = c.ToKnownColor();
				switch (knownColor)
				{
				case KnownColor.ActiveBorder:
					return "activeborder";
				case KnownColor.ActiveCaption:
					break;
				case KnownColor.ActiveCaptionText:
					return "captiontext";
				case KnownColor.AppWorkspace:
					return "appworkspace";
				case KnownColor.Control:
					return "buttonface";
				case KnownColor.ControlDark:
					return "buttonshadow";
				case KnownColor.ControlDarkDark:
					return "threeddarkshadow";
				case KnownColor.ControlLight:
					return "buttonface";
				case KnownColor.ControlLightLight:
					return "buttonhighlight";
				case KnownColor.ControlText:
					return "buttontext";
				case KnownColor.Desktop:
					return "background";
				case KnownColor.GrayText:
					return "graytext";
				case KnownColor.Highlight:
				case KnownColor.HotTrack:
					return "highlight";
				case KnownColor.HighlightText:
					goto IL_12F;
				case KnownColor.InactiveBorder:
					return "inactiveborder";
				case KnownColor.InactiveCaption:
					goto IL_145;
				case KnownColor.InactiveCaptionText:
					return "inactivecaptiontext";
				case KnownColor.Info:
					return "infobackground";
				case KnownColor.InfoText:
					return "infotext";
				case KnownColor.Menu:
					goto IL_171;
				case KnownColor.MenuText:
					return "menutext";
				case KnownColor.ScrollBar:
					return "scrollbar";
				case KnownColor.Window:
					return "window";
				case KnownColor.WindowFrame:
					return "windowframe";
				case KnownColor.WindowText:
					return "windowtext";
				default:
					switch (knownColor)
					{
					case KnownColor.GradientActiveCaption:
						break;
					case KnownColor.GradientInactiveCaption:
						goto IL_145;
					case KnownColor.MenuBar:
						goto IL_171;
					case KnownColor.MenuHighlight:
						goto IL_12F;
					default:
						return result;
					}
					break;
				}
				return "activecaption";
				IL_12F:
				return "highlighttext";
				IL_145:
				return "inactivecaption";
				IL_171:
				result = "menu";
			}
			else if (c.IsNamedColor)
			{
				if (c == Color.LightGray)
				{
					result = "LightGrey";
				}
				else
				{
					result = c.Name;
				}
			}
			else
			{
				result = "#" + c.R.ToString("X2", null) + c.G.ToString("X2", null) + c.B.ToString("X2", null);
			}
			return result;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00018FDC File Offset: 0x000171DC
		private static void InitializeHtmlSysColorTable()
		{
			ColorTranslator.htmlSysColorTable = new Hashtable(26);
			ColorTranslator.htmlSysColorTable["activeborder"] = Color.FromKnownColor(KnownColor.ActiveBorder);
			ColorTranslator.htmlSysColorTable["activecaption"] = Color.FromKnownColor(KnownColor.ActiveCaption);
			ColorTranslator.htmlSysColorTable["appworkspace"] = Color.FromKnownColor(KnownColor.AppWorkspace);
			ColorTranslator.htmlSysColorTable["background"] = Color.FromKnownColor(KnownColor.Desktop);
			ColorTranslator.htmlSysColorTable["buttonface"] = Color.FromKnownColor(KnownColor.Control);
			ColorTranslator.htmlSysColorTable["buttonhighlight"] = Color.FromKnownColor(KnownColor.ControlLightLight);
			ColorTranslator.htmlSysColorTable["buttonshadow"] = Color.FromKnownColor(KnownColor.ControlDark);
			ColorTranslator.htmlSysColorTable["buttontext"] = Color.FromKnownColor(KnownColor.ControlText);
			ColorTranslator.htmlSysColorTable["captiontext"] = Color.FromKnownColor(KnownColor.ActiveCaptionText);
			ColorTranslator.htmlSysColorTable["graytext"] = Color.FromKnownColor(KnownColor.GrayText);
			ColorTranslator.htmlSysColorTable["highlight"] = Color.FromKnownColor(KnownColor.Highlight);
			ColorTranslator.htmlSysColorTable["highlighttext"] = Color.FromKnownColor(KnownColor.HighlightText);
			ColorTranslator.htmlSysColorTable["inactiveborder"] = Color.FromKnownColor(KnownColor.InactiveBorder);
			ColorTranslator.htmlSysColorTable["inactivecaption"] = Color.FromKnownColor(KnownColor.InactiveCaption);
			ColorTranslator.htmlSysColorTable["inactivecaptiontext"] = Color.FromKnownColor(KnownColor.InactiveCaptionText);
			ColorTranslator.htmlSysColorTable["infobackground"] = Color.FromKnownColor(KnownColor.Info);
			ColorTranslator.htmlSysColorTable["infotext"] = Color.FromKnownColor(KnownColor.InfoText);
			ColorTranslator.htmlSysColorTable["menu"] = Color.FromKnownColor(KnownColor.Menu);
			ColorTranslator.htmlSysColorTable["menutext"] = Color.FromKnownColor(KnownColor.MenuText);
			ColorTranslator.htmlSysColorTable["scrollbar"] = Color.FromKnownColor(KnownColor.ScrollBar);
			ColorTranslator.htmlSysColorTable["threeddarkshadow"] = Color.FromKnownColor(KnownColor.ControlDarkDark);
			ColorTranslator.htmlSysColorTable["threedface"] = Color.FromKnownColor(KnownColor.Control);
			ColorTranslator.htmlSysColorTable["threedhighlight"] = Color.FromKnownColor(KnownColor.ControlLight);
			ColorTranslator.htmlSysColorTable["threedlightshadow"] = Color.FromKnownColor(KnownColor.ControlLightLight);
			ColorTranslator.htmlSysColorTable["window"] = Color.FromKnownColor(KnownColor.Window);
			ColorTranslator.htmlSysColorTable["windowframe"] = Color.FromKnownColor(KnownColor.WindowFrame);
			ColorTranslator.htmlSysColorTable["windowtext"] = Color.FromKnownColor(KnownColor.WindowText);
		}

		// Token: 0x04000333 RID: 819
		private const int Win32RedShift = 0;

		// Token: 0x04000334 RID: 820
		private const int Win32GreenShift = 8;

		// Token: 0x04000335 RID: 821
		private const int Win32BlueShift = 16;

		// Token: 0x04000336 RID: 822
		private static Hashtable htmlSysColorTable;
	}
}
