using System;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003D8 RID: 984
	internal static class SelectionWordBreaker
	{
		// Token: 0x06003533 RID: 13619 RVA: 0x000F0FA8 File Offset: 0x000EF1A8
		internal static bool IsAtWordBoundary(char[] text, int position, LogicalDirection insideWordDirection)
		{
			SelectionWordBreaker.CharClass[] classes = SelectionWordBreaker.GetClasses(text);
			if (insideWordDirection == LogicalDirection.Backward)
			{
				if (position == text.Length)
				{
					return true;
				}
				if (position == 0 || SelectionWordBreaker.IsWhiteSpace(text[position - 1], classes[position - 1]))
				{
					return false;
				}
			}
			else
			{
				if (position == 0)
				{
					return true;
				}
				if (position == text.Length || SelectionWordBreaker.IsWhiteSpace(text[position], classes[position]))
				{
					return false;
				}
			}
			ushort[] array = new ushort[2];
			SafeNativeMethods.GetStringTypeEx(0U, 4U, new char[]
			{
				text[position - 1],
				text[position]
			}, 2, array);
			return SelectionWordBreaker.IsWordBoundary(text[position - 1], text[position]) || (!SelectionWordBreaker.IsSameClass(array[0], classes[position - 1], array[1], classes[position]) && !SelectionWordBreaker.IsMidLetter(text, position - 1, classes) && !SelectionWordBreaker.IsMidLetter(text, position, classes));
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003534 RID: 13620 RVA: 0x00094C44 File Offset: 0x00092E44
		internal static int MinContextLength
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000F105C File Offset: 0x000EF25C
		private static bool IsWordBoundary(char previousChar, char followingChar)
		{
			bool result = false;
			if (followingChar == '\r')
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000F1074 File Offset: 0x000EF274
		private static bool IsMidLetter(char[] text, int index, SelectionWordBreaker.CharClass[] classes)
		{
			Invariant.Assert(text.Length == classes.Length);
			return (text[index] == '\'' || text[index] == '’' || text[index] == '­') && index > 0 && index + 1 < classes.Length && ((classes[index - 1] == SelectionWordBreaker.CharClass.Alphanumeric && classes[index + 1] == SelectionWordBreaker.CharClass.Alphanumeric) || (text[index] == '"' && SelectionWordBreaker.IsHebrew(text[index - 1]) && SelectionWordBreaker.IsHebrew(text[index + 1])));
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000F10E6 File Offset: 0x000EF2E6
		private static bool IsIdeographicCharType(ushort charType3)
		{
			return (charType3 & 304) > 0;
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000F10F4 File Offset: 0x000EF2F4
		private static bool IsSameClass(ushort preceedingType3, SelectionWordBreaker.CharClass preceedingClass, ushort followingType3, SelectionWordBreaker.CharClass followingClass)
		{
			bool result = false;
			if (SelectionWordBreaker.IsIdeographicCharType(preceedingType3) && SelectionWordBreaker.IsIdeographicCharType(followingType3))
			{
				ushort num = (preceedingType3 & 496) ^ (followingType3 & 496);
				result = ((preceedingType3 & 240) != 0 && (num == 0 || num == 128 || num == 32 || num == 160));
			}
			else if (!SelectionWordBreaker.IsIdeographicCharType(preceedingType3) && !SelectionWordBreaker.IsIdeographicCharType(followingType3))
			{
				result = ((preceedingClass & SelectionWordBreaker.CharClass.WBF_CLASS) == (followingClass & SelectionWordBreaker.CharClass.WBF_CLASS));
			}
			return result;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000F116A File Offset: 0x000EF36A
		private static bool IsWhiteSpace(char ch, SelectionWordBreaker.CharClass charClass)
		{
			return (charClass & SelectionWordBreaker.CharClass.WBF_CLASS) == SelectionWordBreaker.CharClass.Blank && ch != '￼';
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000F1180 File Offset: 0x000EF380
		private static SelectionWordBreaker.CharClass[] GetClasses(char[] text)
		{
			SelectionWordBreaker.CharClass[] array = new SelectionWordBreaker.CharClass[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				SelectionWordBreaker.CharClass charClass;
				if (c < 'Ā')
				{
					charClass = (SelectionWordBreaker.CharClass)SelectionWordBreaker._latinClasses[(int)c];
				}
				else if (SelectionWordBreaker.IsKorean(c))
				{
					charClass = SelectionWordBreaker.CharClass.Alphanumeric;
				}
				else if (SelectionWordBreaker.IsThai(c))
				{
					charClass = SelectionWordBreaker.CharClass.Alphanumeric;
				}
				else if (c == '￼')
				{
					charClass = (SelectionWordBreaker.CharClass.Blank | SelectionWordBreaker.CharClass.WBF_BREAKAFTER);
				}
				else
				{
					ushort[] array2 = new ushort[1];
					SafeNativeMethods.GetStringTypeEx(0U, 1U, new char[]
					{
						c
					}, 1, array2);
					if ((array2[0] & 8) != 0)
					{
						if ((array2[0] & 64) != 0)
						{
							charClass = (SelectionWordBreaker.CharClass.Blank | SelectionWordBreaker.CharClass.WBF_ISWHITE);
						}
						else
						{
							charClass = (SelectionWordBreaker.CharClass.WhiteSpace | SelectionWordBreaker.CharClass.WBF_ISWHITE);
						}
					}
					else if ((array2[0] & 16) != 0 && !SelectionWordBreaker.IsDiacriticOrKashida(c))
					{
						charClass = SelectionWordBreaker.CharClass.Punctuation;
					}
					else
					{
						charClass = SelectionWordBreaker.CharClass.Alphanumeric;
					}
				}
				array[i] = charClass;
			}
			return array;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000F123C File Offset: 0x000EF43C
		private static bool IsDiacriticOrKashida(char ch)
		{
			ushort[] array = new ushort[1];
			SafeNativeMethods.GetStringTypeEx(0U, 4U, new char[]
			{
				ch
			}, 1, array);
			return (array[0] & 519) > 0;
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000F1270 File Offset: 0x000EF470
		private static bool IsInRange(uint lower, char ch, uint upper)
		{
			return lower <= (uint)ch && (uint)ch <= upper;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000F127F File Offset: 0x000EF47F
		private static bool IsKorean(char ch)
		{
			return SelectionWordBreaker.IsInRange(44032U, ch, 55295U);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000F1291 File Offset: 0x000EF491
		private static bool IsThai(char ch)
		{
			return SelectionWordBreaker.IsInRange(3584U, ch, 3711U);
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000F12A3 File Offset: 0x000EF4A3
		private static bool IsHebrew(char ch)
		{
			return SelectionWordBreaker.IsInRange(1488U, ch, 1522U);
		}

		// Token: 0x04002502 RID: 9474
		private const char LineFeedChar = '\n';

		// Token: 0x04002503 RID: 9475
		private const char CarriageReturnChar = '\r';

		// Token: 0x04002504 RID: 9476
		private const char QuotationMarkChar = '"';

		// Token: 0x04002505 RID: 9477
		private const char ApostropheChar = '\'';

		// Token: 0x04002506 RID: 9478
		private const char SoftHyphenChar = '­';

		// Token: 0x04002507 RID: 9479
		private const char RightSingleQuotationChar = '’';

		// Token: 0x04002508 RID: 9480
		private const char ObjectReplacementChar = '￼';

		// Token: 0x04002509 RID: 9481
		private static readonly byte[] _latinClasses = new byte[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			20,
			0,
			19,
			20,
			20,
			20,
			20,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			50,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			65,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			18,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		// Token: 0x020008DD RID: 2269
		[Flags]
		private enum CharClass : byte
		{
			// Token: 0x04004296 RID: 17046
			Alphanumeric = 0,
			// Token: 0x04004297 RID: 17047
			Punctuation = 1,
			// Token: 0x04004298 RID: 17048
			Blank = 2,
			// Token: 0x04004299 RID: 17049
			WhiteSpace = 4,
			// Token: 0x0400429A RID: 17050
			WBF_CLASS = 15,
			// Token: 0x0400429B RID: 17051
			WBF_ISWHITE = 16,
			// Token: 0x0400429C RID: 17052
			WBF_BREAKAFTER = 64
		}
	}
}
