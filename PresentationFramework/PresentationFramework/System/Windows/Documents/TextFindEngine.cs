using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000406 RID: 1030
	internal static class TextFindEngine
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x00105B1C File Offset: 0x00103D1C
		public static ITextRange Find(ITextPointer findContainerStartPosition, ITextPointer findContainerEndPosition, string findPattern, FindFlags flags, CultureInfo cultureInfo)
		{
			if (findContainerStartPosition == null || findContainerEndPosition == null || findContainerStartPosition.CompareTo(findContainerEndPosition) == 0 || findPattern == null || findPattern == string.Empty)
			{
				return null;
			}
			bool matchCase = (flags & FindFlags.MatchCase) > FindFlags.None;
			bool flag = (flags & FindFlags.FindWholeWordsOnly) > FindFlags.None;
			bool matchLast = (flags & FindFlags.FindInReverse) > FindFlags.None;
			bool matchDiacritics = (flags & FindFlags.MatchDiacritics) > FindFlags.None;
			bool matchKashida = (flags & FindFlags.MatchKashida) > FindFlags.None;
			bool matchAlefHamza = (flags & FindFlags.MatchAlefHamza) > FindFlags.None;
			if (flag)
			{
				ushort[] array = new ushort[1];
				ushort[] array2 = new ushort[1];
				char[] array3 = findPattern.ToCharArray();
				SafeNativeMethods.GetStringTypeEx(0U, 1U, new char[]
				{
					array3[0]
				}, 1, array);
				SafeNativeMethods.GetStringTypeEx(0U, 1U, new char[]
				{
					array3[findPattern.Length - 1]
				}, 1, array2);
				if ((array[0] & 8) != 0 || (array[0] & 64) != 0 || (array2[0] & 8) != 0 || (array2[0] & 64) != 0)
				{
					flag = false;
				}
			}
			if (findContainerStartPosition is DocumentSequenceTextPointer || findContainerStartPosition is FixedTextPointer)
			{
				return FixedFindEngine.Find(findContainerStartPosition, findContainerEndPosition, findPattern, cultureInfo, matchCase, flag, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
			}
			return TextFindEngine.InternalFind(findContainerStartPosition, findContainerEndPosition, findPattern, cultureInfo, matchCase, flag, matchLast, matchDiacritics, matchKashida, matchAlefHamza);
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x00105C2C File Offset: 0x00103E2C
		internal static TextRange InternalFind(ITextPointer startPosition, ITextPointer endPosition, string findPattern, CultureInfo cultureInfo, bool matchCase, bool matchWholeWord, bool matchLast, bool matchDiacritics, bool matchKashida, bool matchAlefHamza)
		{
			Invariant.Assert(startPosition.CompareTo(endPosition) <= 0);
			ITextPointer textPointer;
			LogicalDirection direction;
			if (matchLast)
			{
				textPointer = endPosition;
				direction = LogicalDirection.Backward;
			}
			else
			{
				textPointer = startPosition;
				direction = LogicalDirection.Forward;
			}
			int num = Math.Max(64, findPattern.Length * 2 * 2);
			textPointer = textPointer.CreatePointer();
			while ((matchLast ? startPosition.CompareTo(textPointer) : textPointer.CompareTo(endPosition)) < 0)
			{
				ITextPointer textPointer2 = textPointer.CreatePointer();
				char[] array = new char[num];
				int[] array2 = new int[num + 1];
				int num2 = TextFindEngine.SetFindTextAndFindTextPositionMap(startPosition, endPosition, textPointer, direction, matchLast, array, array2);
				if (!matchDiacritics || num2 >= findPattern.Length)
				{
					int num3 = matchLast ? (array.Length - num2) : 0;
					bool hasPreceedingSeparatorChar = false;
					bool hasFollowingSeparatorChar = false;
					if (matchWholeWord)
					{
						TextFindEngine.GetContextualInformation(textPointer2, matchLast ? (-array2[array2.Length - num2 - 1]) : array2[num2], out hasPreceedingSeparatorChar, out hasFollowingSeparatorChar);
					}
					string textString = new string(array, num3, num2);
					int num5;
					int num4 = TextFindEngine.FindMatchIndexFromFindContent(textString, findPattern, cultureInfo, matchCase, matchWholeWord, matchLast, matchDiacritics, matchKashida, matchAlefHamza, hasPreceedingSeparatorChar, hasFollowingSeparatorChar, out num5);
					if (num4 != -1)
					{
						ITextPointer textPointer3 = textPointer2.CreatePointer();
						textPointer3.MoveByOffset(matchLast ? (-array2[num3 + num4]) : array2[num4]);
						ITextPointer textPointer4 = textPointer2.CreatePointer();
						textPointer4.MoveByOffset(matchLast ? (-array2[num3 + num4 + num5]) : array2[num4 + num5]);
						return new TextRange(textPointer3, textPointer4);
					}
					if (num2 > findPattern.Length)
					{
						textPointer = textPointer2.CreatePointer();
						textPointer.MoveByOffset(matchLast ? (-array2[array.Length - num2 + findPattern.Length]) : array2[num2 - findPattern.Length]);
					}
				}
			}
			return null;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00105DC4 File Offset: 0x00103FC4
		private static void GetContextualInformation(ITextPointer position, int oppositeEndOffset, out bool hasPreceedingSeparatorChar, out bool hasFollowingSeparatorChar)
		{
			ITextPointer position2 = position.CreatePointer(oppositeEndOffset, position.LogicalDirection);
			if (oppositeEndOffset < 0)
			{
				hasPreceedingSeparatorChar = TextFindEngine.HasNeighboringSeparatorChar(position2, LogicalDirection.Backward);
				hasFollowingSeparatorChar = TextFindEngine.HasNeighboringSeparatorChar(position, LogicalDirection.Forward);
				return;
			}
			hasPreceedingSeparatorChar = TextFindEngine.HasNeighboringSeparatorChar(position, LogicalDirection.Backward);
			hasFollowingSeparatorChar = TextFindEngine.HasNeighboringSeparatorChar(position2, LogicalDirection.Forward);
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00105E08 File Offset: 0x00104008
		private static bool HasNeighboringSeparatorChar(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer textPointer = position.GetNextInsertionPosition(direction);
			if (textPointer == null)
			{
				return true;
			}
			if (position.CompareTo(textPointer) > 0)
			{
				ITextPointer textPointer2 = position;
				position = textPointer;
				textPointer = textPointer2;
			}
			int offsetToPosition = position.GetOffsetToPosition(textPointer);
			char[] array = new char[offsetToPosition];
			int[] findTextPositionMap = new int[offsetToPosition + 1];
			int num = TextFindEngine.SetFindTextAndFindTextPositionMap(position, textPointer, position.CreatePointer(), LogicalDirection.Forward, false, array, findTextPositionMap);
			if (num == 0)
			{
				return true;
			}
			bool result;
			if (direction == LogicalDirection.Forward)
			{
				result = TextFindEngine.IsSeparatorChar(array[0]);
			}
			else
			{
				result = TextFindEngine.IsSeparatorChar(array[num - 1]);
			}
			return result;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00105E88 File Offset: 0x00104088
		private static int FindMatchIndexFromFindContent(string textString, string findPattern, CultureInfo cultureInfo, bool matchCase, bool matchWholeWord, bool matchLast, bool matchDiacritics, bool matchKashida, bool matchAlefHamza, bool hasPreceedingSeparatorChar, bool hasFollowingSeparatorChar, out int matchLength)
		{
			bool flag;
			bool flag2;
			TextFindEngine.InitializeBidiFlags(findPattern, out flag, out flag2);
			CompareInfo compareInfo = cultureInfo.CompareInfo;
			int result;
			if (!matchDiacritics && flag)
			{
				result = TextFindEngine.BidiIgnoreDiacriticsMatchIndexCalculation(textString, findPattern, matchKashida, matchAlefHamza, matchWholeWord, matchLast, !matchCase, compareInfo, hasPreceedingSeparatorChar, hasFollowingSeparatorChar, out matchLength);
			}
			else
			{
				result = TextFindEngine.StandardMatchIndexCalculation(textString, findPattern, matchWholeWord, matchLast, !matchCase, compareInfo, hasPreceedingSeparatorChar, hasFollowingSeparatorChar, out matchLength);
			}
			return result;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x00105EE4 File Offset: 0x001040E4
		private static int StandardMatchIndexCalculation(string textString, string findPattern, bool matchWholeWord, bool matchLast, bool ignoreCase, CompareInfo compareInfo, bool hasPreceedingSeparatorChar, bool hasFollowingSeparatorChar, out int matchLength)
		{
			CompareOptions options = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;
			int num = -1;
			int num2 = 0;
			int i = textString.Length;
			matchLength = 0;
			while (i > 0)
			{
				num = (matchLast ? compareInfo.LastIndexOf(textString, findPattern, num2 + i - 1, i, options) : compareInfo.IndexOf(textString, findPattern, num2, i, options));
				matchLength = findPattern.Length;
				if (num == -1 || !matchWholeWord || TextFindEngine.IsAtWordBoundary(textString, num, matchLength, hasPreceedingSeparatorChar, hasFollowingSeparatorChar))
				{
					break;
				}
				if (matchLast)
				{
					num2 = 0;
					i = num + matchLength - 1;
				}
				else
				{
					num2 = num + 1;
					i = textString.Length - num2;
				}
				num = -1;
			}
			return num;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00105F70 File Offset: 0x00104170
		private static int BidiIgnoreDiacriticsMatchIndexCalculation(string textString, string findPattern, bool matchKashida, bool matchAlefHamza, bool matchWholeWord, bool matchLast, bool ignoreCase, CompareInfo compareInfo, bool hasPreceedingSeparatorChar, bool hasFollowingSeparatorChar, out int matchLength)
		{
			int num = -1;
			int num2 = matchLast ? (textString.Length - 1) : 0;
			int num3 = matchLast ? -1 : textString.Length;
			int num4 = matchLast ? -1 : 1;
			if (Environment.OSVersion.Version.Major >= 6)
			{
				uint num5 = 2U;
				if (ignoreCase)
				{
					num5 |= 1U;
				}
				if (matchLast)
				{
					num5 |= 8388608U;
				}
				if (matchKashida)
				{
					textString = textString.Replace('ـ', '0');
					findPattern = findPattern.Replace('ـ', '0');
				}
				if (matchAlefHamza)
				{
					textString = textString.Replace('آ', '0');
					textString = textString.Replace('أ', '1');
					textString = textString.Replace('إ', '2');
					findPattern = findPattern.Replace('آ', '0');
					findPattern = findPattern.Replace('أ', '1');
					findPattern = findPattern.Replace('إ', '2');
				}
				matchLength = 0;
				if (matchWholeWord)
				{
					int num6 = num2;
					while (num == -1)
					{
						if (num6 == num3)
						{
							break;
						}
						for (int i = num6; i < textString.Length; i++)
						{
							string sourceString = textString.Substring(num6, i - num6 + 1);
							int num7 = TextFindEngine.FindNLSString(compareInfo.LCID, num5, sourceString, findPattern, out matchLength);
							if (num7 >= 0 && TextFindEngine.IsAtWordBoundary(textString, num6 + num7, matchLength, hasPreceedingSeparatorChar, hasFollowingSeparatorChar))
							{
								num = num6 + num7;
								break;
							}
						}
						num6 += num4;
					}
				}
				else
				{
					num = TextFindEngine.FindNLSString(compareInfo.LCID, num5, textString, findPattern, out matchLength);
				}
			}
			else
			{
				CompareOptions options = CompareOptions.IgnoreNonSpace | (ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
				matchLength = 0;
				int num8 = num2;
				while (num == -1 && num8 != num3)
				{
					for (int j = num8; j < textString.Length; j++)
					{
						if (compareInfo.Compare(textString, num8, j - num8 + 1, findPattern, 0, findPattern.Length, options) == 0 && (!matchWholeWord || TextFindEngine.IsAtWordBoundary(textString, num8, j - num8 + 1, hasPreceedingSeparatorChar, hasFollowingSeparatorChar)) && (!matchKashida || TextFindEngine.IsKashidaMatch(textString.Substring(num8, j - num8 + 1), findPattern, compareInfo)) && (!matchAlefHamza || TextFindEngine.IsAlefHamzaMatch(textString.Substring(num8, j - num8 + 1), findPattern, compareInfo)))
						{
							num = num8;
							matchLength = j - num8 + 1;
							break;
						}
					}
					num8 += num4;
				}
			}
			return num;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001061B4 File Offset: 0x001043B4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static int FindNLSString(int locale, uint flags, string sourceString, string findString, out int found)
		{
			int num = UnsafeNativeMethods.FindNLSString(locale, flags, sourceString, sourceString.Length, findString, findString.Length, out found);
			if (num == -1)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 0)
				{
					throw new Win32Exception(lastWin32Error);
				}
			}
			return num;
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x001061EE File Offset: 0x001043EE
		private static bool IsKashidaMatch(string text, string pattern, CompareInfo compareInfo)
		{
			text = text.Replace('ـ', '0');
			pattern = pattern.Replace('ـ', '0');
			return compareInfo.Compare(text, pattern, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.StringSort) == 0;
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x00106220 File Offset: 0x00104420
		private static bool IsAlefHamzaMatch(string text, string pattern, CompareInfo compareInfo)
		{
			text = text.Replace('آ', '0');
			text = text.Replace('أ', '1');
			text = text.Replace('إ', '2');
			pattern = pattern.Replace('آ', '0');
			pattern = pattern.Replace('أ', '1');
			pattern = pattern.Replace('إ', '2');
			return compareInfo.Compare(text, pattern, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.StringSort) == 0;
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x00106298 File Offset: 0x00104498
		private static int SetFindTextAndFindTextPositionMap(ITextPointer startPosition, ITextPointer endPosition, ITextPointer navigator, LogicalDirection direction, bool matchLast, char[] findText, int[] findTextPositionMap)
		{
			Invariant.Assert(startPosition.CompareTo(navigator) <= 0);
			Invariant.Assert(endPosition.CompareTo(navigator) >= 0);
			int num = 0;
			int num2 = 0;
			if (matchLast && num2 == 0)
			{
				findTextPositionMap[findTextPositionMap.Length - 1] = 0;
			}
			while ((matchLast ? startPosition.CompareTo(navigator) : navigator.CompareTo(endPosition)) < 0)
			{
				switch (navigator.GetPointerContext(direction))
				{
				case TextPointerContext.None:
				case TextPointerContext.ElementStart:
				case TextPointerContext.ElementEnd:
					if (TextFindEngine.IsAdjacentToFormatElement(navigator, direction))
					{
						num++;
					}
					else if (!matchLast)
					{
						findText[num2] = '\n';
						findTextPositionMap[num2] = num2 + num;
						num2++;
					}
					else
					{
						num2++;
						findText[findText.Length - num2] = '\n';
						findTextPositionMap[findText.Length - num2] = num2 + num;
					}
					navigator.MoveToNextContextPosition(direction);
					break;
				case TextPointerContext.Text:
				{
					int num3 = navigator.GetTextRunLength(direction);
					num3 = Math.Min(num3, findText.Length - num2);
					if (!matchLast)
					{
						num3 = Math.Min(num3, navigator.GetOffsetToPosition(endPosition));
						navigator.GetTextInRun(direction, findText, num2, num3);
						for (int i = num2; i < num2 + num3; i++)
						{
							findTextPositionMap[i] = i + num;
						}
					}
					else
					{
						num3 = Math.Min(num3, startPosition.GetOffsetToPosition(navigator));
						navigator.GetTextInRun(direction, findText, findText.Length - num2 - num3, num3);
						int num4 = findText.Length - num2 - 1;
						for (int j = num2; j < num2 + num3; j++)
						{
							findTextPositionMap[num4--] = j + num + 1;
						}
					}
					navigator.MoveByOffset(matchLast ? (-num3) : num3);
					num2 += num3;
					break;
				}
				case TextPointerContext.EmbeddedElement:
					if (!matchLast)
					{
						findText[num2] = '';
						findTextPositionMap[num2] = num2 + num;
						num2++;
					}
					else
					{
						num2++;
						findText[findText.Length - num2] = '';
						findTextPositionMap[findText.Length - num2] = num2 + num;
					}
					navigator.MoveToNextContextPosition(direction);
					break;
				}
				if (num2 >= findText.Length)
				{
					break;
				}
			}
			if (!matchLast)
			{
				if (num2 > 0)
				{
					findTextPositionMap[num2] = findTextPositionMap[num2 - 1] + 1;
				}
				else
				{
					findTextPositionMap[0] = 0;
				}
			}
			return num2;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x00106490 File Offset: 0x00104690
		internal static void InitializeBidiFlags(string textString, out bool stringContainedBidiCharacter, out bool stringContainedAlefCharacter)
		{
			stringContainedBidiCharacter = false;
			stringContainedAlefCharacter = false;
			foreach (char c in textString)
			{
				if (c >= '֐' && c <= '޿')
				{
					stringContainedBidiCharacter = true;
					if (c == 'آ' || c == 'أ' || c == 'إ' || c == 'ا')
					{
						stringContainedAlefCharacter = true;
						return;
					}
				}
			}
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x001064F3 File Offset: 0x001046F3
		internal static string ReplaceAlefHamzaWithAlef(string textString)
		{
			textString = textString.Replace('آ', 'ا');
			textString = textString.Replace('أ', 'ا');
			textString = textString.Replace('إ', 'ا');
			return textString;
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x0010652C File Offset: 0x0010472C
		private static bool IsAtWordBoundary(string textString, int matchIndex, int matchLength, bool hasPreceedingSeparatorChar, bool hasFollowingSeparatorChar)
		{
			bool result = false;
			int length = textString.Length;
			Invariant.Assert(matchIndex + matchLength <= length);
			if (matchIndex == 0)
			{
				if (hasPreceedingSeparatorChar)
				{
					if (matchIndex + matchLength < length)
					{
						if (TextFindEngine.IsSeparatorChar(textString[matchIndex + matchLength]))
						{
							result = true;
						}
					}
					else if (hasFollowingSeparatorChar)
					{
						result = true;
					}
				}
			}
			else if (matchIndex + matchLength == length)
			{
				if (TextFindEngine.IsSeparatorChar(textString[matchIndex - 1]) && hasFollowingSeparatorChar)
				{
					result = true;
				}
			}
			else if (TextFindEngine.IsSeparatorChar(textString[matchIndex - 1]) && TextFindEngine.IsSeparatorChar(textString[matchIndex + matchLength]))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x001065B8 File Offset: 0x001047B8
		private static bool IsSeparatorChar(char separatorChar)
		{
			return char.IsWhiteSpace(separatorChar) || char.IsPunctuation(separatorChar) || char.IsSymbol(separatorChar) || char.IsSeparator(separatorChar);
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x001065E0 File Offset: 0x001047E0
		private static bool IsAdjacentToFormatElement(ITextPointer pointer, LogicalDirection direction)
		{
			bool result = false;
			if (direction == LogicalDirection.Forward)
			{
				TextPointerContext pointerContext = pointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart && TextSchema.IsFormattingType(pointer.GetElementType(LogicalDirection.Forward)))
				{
					result = true;
				}
				else if (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsFormattingType(pointer.ParentType))
				{
					result = true;
				}
			}
			else
			{
				TextPointerContext pointerContext = pointer.GetPointerContext(LogicalDirection.Backward);
				if (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsFormattingType(pointer.GetElementType(LogicalDirection.Backward)))
				{
					result = true;
				}
				else if (pointerContext == TextPointerContext.ElementStart && TextSchema.IsFormattingType(pointer.ParentType))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x040025C1 RID: 9665
		private const int TextBlockLength = 64;

		// Token: 0x040025C2 RID: 9666
		private const char UnicodeBidiStart = '֐';

		// Token: 0x040025C3 RID: 9667
		private const char UnicodeBidiEnd = '޿';

		// Token: 0x040025C4 RID: 9668
		private const char UnicodeArabicKashida = 'ـ';

		// Token: 0x040025C5 RID: 9669
		private const char UnicodeArabicAlefMaddaAbove = 'آ';

		// Token: 0x040025C6 RID: 9670
		private const char UnicodeArabicAlefHamzaAbove = 'أ';

		// Token: 0x040025C7 RID: 9671
		private const char UnicodeArabicAlefHamzaBelow = 'إ';

		// Token: 0x040025C8 RID: 9672
		private const char UnicodeArabicAlef = 'ا';
	}
}
