using System;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062C RID: 1580
	internal sealed class ListMarkerSourceInfo
	{
		// Token: 0x060068A6 RID: 26790 RVA: 0x0000326D File Offset: 0x0000146D
		private ListMarkerSourceInfo()
		{
		}

		// Token: 0x060068A7 RID: 26791 RVA: 0x001D852C File Offset: 0x001D672C
		internal static Thickness CalculatePadding(List list, double lineHeight, double pixelsPerDip)
		{
			FormattedText formattedMarker = ListMarkerSourceInfo.GetFormattedMarker(list, pixelsPerDip);
			double num = formattedMarker.Width + 1.5 * lineHeight;
			num = (double)((int)(num / lineHeight) + 1) * lineHeight;
			return new Thickness(num, 0.0, 0.0, 0.0);
		}

		// Token: 0x060068A8 RID: 26792 RVA: 0x001D8580 File Offset: 0x001D6780
		private static FormattedText GetFormattedMarker(List list, double pixelsPerDip)
		{
			string textToFormat = "";
			FormattedText result;
			if (ListMarkerSourceInfo.IsKnownSymbolMarkerStyle(list.MarkerStyle))
			{
				switch (list.MarkerStyle)
				{
				case TextMarkerStyle.Disc:
					textToFormat = "\u009f";
					break;
				case TextMarkerStyle.Circle:
					textToFormat = "¡";
					break;
				case TextMarkerStyle.Square:
					textToFormat = "q";
					break;
				case TextMarkerStyle.Box:
					textToFormat = "§";
					break;
				}
				Typeface modifiedTypeface = DynamicPropertyReader.GetModifiedTypeface(list, new FontFamily("Wingdings"));
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, modifiedTypeface, list.FontSize, list.Foreground, pixelsPerDip);
			}
			else if (ListMarkerSourceInfo.IsKnownIndexMarkerStyle(list.MarkerStyle))
			{
				int startIndex = list.StartIndex;
				Invariant.Assert(startIndex > 0);
				int count = list.ListItems.Count;
				int num;
				if (2147483647 - count < startIndex)
				{
					num = int.MaxValue;
				}
				else
				{
					num = ((count == 0) ? startIndex : (startIndex + count - 1));
				}
				switch (list.MarkerStyle)
				{
				case TextMarkerStyle.LowerRoman:
					textToFormat = ListMarkerSourceInfo.GetStringForLargestRomanMarker(startIndex, num, false);
					break;
				case TextMarkerStyle.UpperRoman:
					textToFormat = ListMarkerSourceInfo.GetStringForLargestRomanMarker(startIndex, num, true);
					break;
				case TextMarkerStyle.LowerLatin:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, true, ListMarkerSourceInfo.LowerLatinNumerics);
					break;
				case TextMarkerStyle.UpperLatin:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, true, ListMarkerSourceInfo.UpperLatinNumerics);
					break;
				case TextMarkerStyle.Decimal:
					textToFormat = ListMarkerSourceInfo.ConvertNumberToString(num, false, ListMarkerSourceInfo.DecimalNumerics);
					break;
				}
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, DynamicPropertyReader.GetTypeface(list), list.FontSize, list.Foreground, pixelsPerDip);
			}
			else
			{
				textToFormat = "\u009f";
				Typeface modifiedTypeface2 = DynamicPropertyReader.GetModifiedTypeface(list, new FontFamily("Wingdings"));
				result = new FormattedText(textToFormat, DynamicPropertyReader.GetCultureInfo(list), list.FlowDirection, modifiedTypeface2, list.FontSize, list.Foreground, pixelsPerDip);
			}
			return result;
		}

		// Token: 0x060068A9 RID: 26793 RVA: 0x001D873C File Offset: 0x001D693C
		private static string ConvertNumberToString(int number, bool oneBased, string numericSymbols)
		{
			if (oneBased)
			{
				number--;
			}
			Invariant.Assert(number >= 0);
			int length = numericSymbols.Length;
			char[] array;
			if (number < length)
			{
				array = new char[]
				{
					numericSymbols[number],
					ListMarkerSourceInfo.NumberSuffix
				};
			}
			else
			{
				int num = oneBased ? 1 : 0;
				int num2 = 1;
				long num3 = (long)length;
				long num4 = (long)length;
				while ((long)number >= num3)
				{
					num4 *= (long)length;
					num3 = num4 + num3 * (long)num;
					num2++;
				}
				array = new char[num2 + 1];
				array[num2] = ListMarkerSourceInfo.NumberSuffix;
				for (int i = num2 - 1; i >= 0; i--)
				{
					array[i] = numericSymbols[number % length];
					number = number / length - num;
				}
			}
			return new string(array);
		}

		// Token: 0x060068AA RID: 26794 RVA: 0x001D87F0 File Offset: 0x001D69F0
		private static string GetStringForLargestRomanMarker(int startIndex, int highestIndex, bool uppercase)
		{
			if (highestIndex <= 3999)
			{
				int indexForLargestRomanMarker = ListMarkerSourceInfo.GetIndexForLargestRomanMarker(startIndex, highestIndex);
				return ListMarkerSourceInfo.ConvertNumberToRomanString(indexForLargestRomanMarker, uppercase);
			}
			if (!uppercase)
			{
				return ListMarkerSourceInfo.LargestRomanMarkerLower;
			}
			return ListMarkerSourceInfo.LargestRomanMarkerUpper;
		}

		// Token: 0x060068AB RID: 26795 RVA: 0x001D8828 File Offset: 0x001D6A28
		private static int GetIndexForLargestRomanMarker(int startIndex, int highestIndex)
		{
			int num = 0;
			if (startIndex == 1)
			{
				int num2 = highestIndex / 1000;
				highestIndex %= 1000;
				for (int i = 0; i < ListMarkerSourceInfo.RomanNumericSizeIncrements.Length; i++)
				{
					Invariant.Assert(highestIndex >= ListMarkerSourceInfo.RomanNumericSizeIncrements[i]);
					if (highestIndex == ListMarkerSourceInfo.RomanNumericSizeIncrements[i])
					{
						num = highestIndex;
						break;
					}
					Invariant.Assert(highestIndex > ListMarkerSourceInfo.RomanNumericSizeIncrements[i]);
					if (i >= ListMarkerSourceInfo.RomanNumericSizeIncrements.Length - 1 || highestIndex < ListMarkerSourceInfo.RomanNumericSizeIncrements[i + 1])
					{
						num = ListMarkerSourceInfo.RomanNumericSizeIncrements[i];
						break;
					}
				}
				if (num2 > 0)
				{
					num = num2 * 1000 + num;
				}
			}
			else
			{
				int num3 = 0;
				for (int j = startIndex; j <= highestIndex; j++)
				{
					string text = ListMarkerSourceInfo.ConvertNumberToRomanString(j, true);
					if (text.Length > num3)
					{
						num = j;
						num3 = text.Length;
					}
				}
			}
			Invariant.Assert(num > 0);
			return num;
		}

		// Token: 0x060068AC RID: 26796 RVA: 0x001D8900 File Offset: 0x001D6B00
		private static string ConvertNumberToRomanString(int number, bool uppercase)
		{
			Invariant.Assert(number <= 3999);
			StringBuilder stringBuilder = new StringBuilder();
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 1000, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][0]);
			number %= 1000;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 100, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][1]);
			number %= 100;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number / 10, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][2]);
			number %= 10;
			ListMarkerSourceInfo.AddRomanNumeric(stringBuilder, number, ListMarkerSourceInfo.RomanNumerics[uppercase ? 1 : 0][3]);
			stringBuilder.Append(ListMarkerSourceInfo.NumberSuffix);
			return stringBuilder.ToString();
		}

		// Token: 0x060068AD RID: 26797 RVA: 0x001D89B0 File Offset: 0x001D6BB0
		private static void AddRomanNumeric(StringBuilder builder, int number, string oneFiveTen)
		{
			if (number >= 1 && number <= 9)
			{
				if (number == 4 || number == 9)
				{
					builder.Append(oneFiveTen[0]);
				}
				if (number == 9)
				{
					builder.Append(oneFiveTen[2]);
					return;
				}
				if (number >= 4)
				{
					builder.Append(oneFiveTen[1]);
				}
				int num = number % 5;
				while (num > 0 && num < 4)
				{
					builder.Append(oneFiveTen[0]);
					num--;
				}
			}
		}

		// Token: 0x060068AE RID: 26798 RVA: 0x001D8A23 File Offset: 0x001D6C23
		private static bool IsKnownSymbolMarkerStyle(TextMarkerStyle markerStyle)
		{
			return markerStyle == TextMarkerStyle.Disc || markerStyle == TextMarkerStyle.Circle || markerStyle == TextMarkerStyle.Square || markerStyle == TextMarkerStyle.Box;
		}

		// Token: 0x060068AF RID: 26799 RVA: 0x001D8A37 File Offset: 0x001D6C37
		private static bool IsKnownIndexMarkerStyle(TextMarkerStyle markerStyle)
		{
			return markerStyle == TextMarkerStyle.Decimal || markerStyle == TextMarkerStyle.LowerLatin || markerStyle == TextMarkerStyle.UpperLatin || markerStyle == TextMarkerStyle.LowerRoman || markerStyle == TextMarkerStyle.UpperRoman;
		}

		// Token: 0x040033E5 RID: 13285
		private static char NumberSuffix = '.';

		// Token: 0x040033E6 RID: 13286
		private static string DecimalNumerics = "0123456789";

		// Token: 0x040033E7 RID: 13287
		private static string LowerLatinNumerics = "abcdefghijklmnopqrstuvwxyz";

		// Token: 0x040033E8 RID: 13288
		private static string UpperLatinNumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x040033E9 RID: 13289
		private static string LargestRomanMarkerUpper = "MMMDCCCLXXXVIII";

		// Token: 0x040033EA RID: 13290
		private static string LargestRomanMarkerLower = "mmmdccclxxxviii";

		// Token: 0x040033EB RID: 13291
		private static string[][] RomanNumerics = new string[][]
		{
			new string[]
			{
				"m??",
				"cdm",
				"xlc",
				"ivx"
			},
			new string[]
			{
				"M??",
				"CDM",
				"XLC",
				"IVX"
			}
		};

		// Token: 0x040033EC RID: 13292
		private static int[] RomanNumericSizeIncrements = new int[]
		{
			1,
			2,
			3,
			8,
			18,
			28,
			38,
			88,
			188,
			288,
			388,
			888
		};
	}
}
