using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003B8 RID: 952
	internal static class Converters
	{
		// Token: 0x060032C1 RID: 12993 RVA: 0x000E4696 File Offset: 0x000E2896
		internal static double HalfPointToPositivePx(double halfPoint)
		{
			return Converters.TwipToPositivePx(halfPoint * 10.0);
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000E46A8 File Offset: 0x000E28A8
		internal static double TwipToPx(double twip)
		{
			return twip / 1440.0 * 96.0;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x000E46C0 File Offset: 0x000E28C0
		internal static double TwipToPositivePx(double twip)
		{
			double num = twip / 1440.0 * 96.0;
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000E46FC File Offset: 0x000E28FC
		internal static double TwipToPositiveVisiblePx(double twip)
		{
			double num = twip / 1440.0 * 96.0;
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (twip > 0.0 && num < 1.0)
			{
				num = 1.0;
			}
			return num;
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x000E4758 File Offset: 0x000E2958
		internal static string TwipToPxString(double twip)
		{
			return Converters.TwipToPx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x000E4780 File Offset: 0x000E2980
		internal static string TwipToPositivePxString(double twip)
		{
			return Converters.TwipToPositivePx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x000E47A8 File Offset: 0x000E29A8
		internal static string TwipToPositiveVisiblePxString(double twip)
		{
			return Converters.TwipToPositiveVisiblePx(twip).ToString("f2", CultureInfo.InvariantCulture);
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x000E47CD File Offset: 0x000E29CD
		internal static double PxToPt(double px)
		{
			return px / 96.0 * 72.0;
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000E47E4 File Offset: 0x000E29E4
		internal static long PxToTwipRounded(double px)
		{
			double num = px / 96.0 * 1440.0;
			if (num < 0.0)
			{
				return (long)(num - 0.5);
			}
			return (long)(num + 0.5);
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000E482C File Offset: 0x000E2A2C
		internal static long PxToHalfPointRounded(double px)
		{
			double num = px / 96.0 * 1440.0;
			double num2 = num / 10.0;
			if (num2 < 0.0)
			{
				return (long)(num2 - 0.5);
			}
			return (long)(num2 + 0.5);
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x000E4880 File Offset: 0x000E2A80
		internal static bool StringToDouble(string s, ref double d)
		{
			bool result = true;
			d = 0.0;
			try
			{
				d = Convert.ToDouble(s, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x000E48D0 File Offset: 0x000E2AD0
		internal static bool StringToInt(string s, ref int i)
		{
			bool result = true;
			i = 0;
			try
			{
				i = Convert.ToInt32(s, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000E4918 File Offset: 0x000E2B18
		internal static string StringToXMLAttribute(string s)
		{
			if (s.IndexOf('"') == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '"')
				{
					stringBuilder.Append("&quot;");
				}
				else
				{
					stringBuilder.Append(s[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x000E4978 File Offset: 0x000E2B78
		internal static bool HexStringToInt(string s, ref int i)
		{
			bool result = true;
			i = 0;
			try
			{
				i = int.Parse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				result = false;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x000E49C8 File Offset: 0x000E2BC8
		internal static string MarkerStyleToString(MarkerStyle ms)
		{
			switch (ms)
			{
			case MarkerStyle.MarkerArabic:
				return "Decimal";
			case MarkerStyle.MarkerUpperRoman:
				return "UpperRoman";
			case MarkerStyle.MarkerLowerRoman:
				return "LowerRoman";
			case MarkerStyle.MarkerUpperAlpha:
				return "UpperLatin";
			case MarkerStyle.MarkerLowerAlpha:
				return "LowerLatin";
			case MarkerStyle.MarkerOrdinal:
				return "Decimal";
			case MarkerStyle.MarkerCardinal:
				return "Decimal";
			default:
				if (ms == MarkerStyle.MarkerBullet)
				{
					return "Disc";
				}
				if (ms != MarkerStyle.MarkerHidden)
				{
					return "Decimal";
				}
				return "None";
			}
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000E4A44 File Offset: 0x000E2C44
		internal static string MarkerStyleToOldRTFString(MarkerStyle ms)
		{
			switch (ms)
			{
			case MarkerStyle.MarkerArabic:
				break;
			case MarkerStyle.MarkerUpperRoman:
				return "\\pnlvlbody\\pnucrm";
			case MarkerStyle.MarkerLowerRoman:
				return "\\pnlvlbody\\pnlcrm";
			case MarkerStyle.MarkerUpperAlpha:
				return "\\pnlvlbody\\pnucltr";
			case MarkerStyle.MarkerLowerAlpha:
				return "\\pnlvlbody\\pnlcltr";
			case MarkerStyle.MarkerOrdinal:
				return "\\pnlvlbody\\pnord";
			case MarkerStyle.MarkerCardinal:
				return "\\pnlvlbody\\pncard";
			default:
				if (ms == MarkerStyle.MarkerBullet)
				{
					return "\\pnlvlblt";
				}
				break;
			}
			return "\\pnlvlbody\\pndec";
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000E4AA8 File Offset: 0x000E2CA8
		internal static bool ColorToUse(ConverterState converterState, long cb, long cf, long shade, ref Color c)
		{
			ColorTableEntry colorTableEntry = (cb >= 0L) ? converterState.ColorTable.EntryAt((int)cb) : null;
			ColorTableEntry colorTableEntry2 = (cf >= 0L) ? converterState.ColorTable.EntryAt((int)cf) : null;
			if (shade < 0L)
			{
				if (colorTableEntry == null)
				{
					return false;
				}
				c = colorTableEntry.Color;
				return true;
			}
			else
			{
				Color color = (colorTableEntry != null) ? colorTableEntry.Color : Color.FromArgb(byte.MaxValue, 0, 0, 0);
				Color color2 = (colorTableEntry2 != null) ? colorTableEntry2.Color : Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (colorTableEntry2 == null && colorTableEntry == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)(255L - 255L * shade / 10000L), (byte)(255L - 255L * shade / 10000L), (byte)(255L - 255L * shade / 10000L));
					return true;
				}
				if (colorTableEntry == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color2.R + (ulong)((long)(byte.MaxValue - color2.R) * (10000L - shade) / 10000L)), (byte)((ulong)color2.G + (ulong)((long)(byte.MaxValue - color2.G) * (10000L - shade) / 10000L)), (byte)((ulong)color2.B + (ulong)((long)(byte.MaxValue - color2.B) * (10000L - shade) / 10000L)));
					return true;
				}
				if (colorTableEntry2 == null)
				{
					c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color.R - (ulong)color.R * (ulong)shade / 10000UL), (byte)((ulong)color.G - (ulong)color.G * (ulong)shade / 10000UL), (byte)((ulong)color.B - (ulong)color.B * (ulong)shade / 10000UL));
					return true;
				}
				c = Color.FromArgb(byte.MaxValue, (byte)((ulong)color.R * (ulong)(10000L - shade) / 10000UL + (ulong)color2.R * (ulong)shade / 10000UL), (byte)((ulong)color.G * (ulong)(10000L - shade) / 10000UL + (ulong)color2.G * (ulong)shade / 10000UL), (byte)((ulong)color.B * (ulong)(10000L - shade) / 10000UL + (ulong)color2.B * (ulong)shade / 10000UL));
				return true;
			}
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000E4D24 File Offset: 0x000E2F24
		internal static string AlignmentToString(HAlign a, DirState ds)
		{
			switch (a)
			{
			case HAlign.AlignLeft:
				if (ds == DirState.DirRTL)
				{
					return "Right";
				}
				return "Left";
			case HAlign.AlignRight:
				if (ds == DirState.DirRTL)
				{
					return "Left";
				}
				return "Right";
			case HAlign.AlignCenter:
				return "Center";
			case HAlign.AlignJustify:
				return "Justify";
			}
			return "";
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000E4D80 File Offset: 0x000E2F80
		internal static string MarkerCountToString(MarkerStyle ms, long nCount)
		{
			StringBuilder sb = new StringBuilder();
			if (nCount < 0L)
			{
				nCount = 0L;
			}
			switch (ms)
			{
			case MarkerStyle.MarkerNone:
				break;
			case MarkerStyle.MarkerArabic:
			case MarkerStyle.MarkerOrdinal:
			case MarkerStyle.MarkerCardinal:
				return nCount.ToString(CultureInfo.InvariantCulture);
			case MarkerStyle.MarkerUpperRoman:
			case MarkerStyle.MarkerLowerRoman:
				return Converters.MarkerRomanCountToString(sb, ms, nCount);
			case MarkerStyle.MarkerUpperAlpha:
			case MarkerStyle.MarkerLowerAlpha:
				return Converters.MarkerAlphaCountToString(sb, ms, nCount);
			default:
				if (ms != MarkerStyle.MarkerBullet)
				{
					if (ms == MarkerStyle.MarkerHidden)
					{
						break;
					}
				}
				return "\\'B7";
			}
			return "";
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000E4E00 File Offset: 0x000E3000
		private static string MarkerRomanCountToString(StringBuilder sb, MarkerStyle ms, long nCount)
		{
			while (nCount >= 1000L)
			{
				sb.Append("M");
				nCount -= 1000L;
			}
			long num = nCount / 100L;
			long num2 = num;
			if (num2 <= 9L)
			{
				switch ((uint)num2)
				{
				case 1U:
					sb.Append("C");
					break;
				case 2U:
					sb.Append("CC");
					break;
				case 3U:
					sb.Append("CCC");
					break;
				case 4U:
					sb.Append("CD");
					break;
				case 5U:
					sb.Append("D");
					break;
				case 6U:
					sb.Append("DC");
					break;
				case 7U:
					sb.Append("DCC");
					break;
				case 8U:
					sb.Append("DCCC");
					break;
				case 9U:
					sb.Append("CM");
					break;
				}
			}
			nCount %= 100L;
			num = nCount / 10L;
			long num3 = num;
			if (num3 <= 9L)
			{
				switch ((uint)num3)
				{
				case 1U:
					sb.Append("X");
					break;
				case 2U:
					sb.Append("XX");
					break;
				case 3U:
					sb.Append("XXX");
					break;
				case 4U:
					sb.Append("XL");
					break;
				case 5U:
					sb.Append("L");
					break;
				case 6U:
					sb.Append("LX");
					break;
				case 7U:
					sb.Append("LXX");
					break;
				case 8U:
					sb.Append("LXXX");
					break;
				case 9U:
					sb.Append("XC");
					break;
				}
			}
			nCount %= 10L;
			long num4 = nCount;
			if (num4 <= 9L)
			{
				switch ((uint)num4)
				{
				case 1U:
					sb.Append("I");
					break;
				case 2U:
					sb.Append("II");
					break;
				case 3U:
					sb.Append("III");
					break;
				case 4U:
					sb.Append("IV");
					break;
				case 5U:
					sb.Append("V");
					break;
				case 6U:
					sb.Append("VI");
					break;
				case 7U:
					sb.Append("VII");
					break;
				case 8U:
					sb.Append("VIII");
					break;
				case 9U:
					sb.Append("IX");
					break;
				}
			}
			if (ms == MarkerStyle.MarkerUpperRoman)
			{
				return sb.ToString();
			}
			return sb.ToString().ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000E5090 File Offset: 0x000E3290
		private static string MarkerAlphaCountToString(StringBuilder sb, MarkerStyle ms, long nCount)
		{
			int num = 26;
			int num2 = 676;
			int num3 = 17576;
			int num4 = 456976;
			char[] array = new char[1];
			int num5 = 0;
			while (nCount > (long)(num4 + num3 + num2 + num))
			{
				num5++;
				nCount -= (long)num4;
			}
			if (num5 > 0)
			{
				if (num5 > 26)
				{
					num5 = 26;
				}
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)(num3 + num2 + num))
			{
				num5++;
				nCount -= (long)num3;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)(num2 + num))
			{
				num5++;
				nCount -= (long)num2;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			num5 = 0;
			while (nCount > (long)num)
			{
				num5++;
				nCount -= (long)num;
			}
			if (num5 > 0)
			{
				array[0] = (char)(65 + (num5 - 1));
				sb.Append(array);
			}
			array[0] = (char)(65L + (nCount - 1L));
			sb.Append(array);
			if (ms == MarkerStyle.MarkerUpperAlpha)
			{
				return sb.ToString();
			}
			return sb.ToString().ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000E51C4 File Offset: 0x000E33C4
		internal static void ByteToHex(byte byteData, out byte firstHexByte, out byte secondHexByte)
		{
			firstHexByte = (byte)(byteData >> 4 & 15);
			secondHexByte = (byteData & 15);
			if (firstHexByte >= 0 && firstHexByte <= 9)
			{
				firstHexByte += 48;
			}
			else if (firstHexByte >= 10 && firstHexByte <= 15)
			{
				firstHexByte += 87;
			}
			if (secondHexByte >= 0 && secondHexByte <= 9)
			{
				secondHexByte += 48;
				return;
			}
			if (secondHexByte >= 10 && secondHexByte <= 15)
			{
				secondHexByte += 87;
			}
		}
	}
}
