using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	/// <summary>Helper class that provides information about the IME conversion mode.</summary>
	// Token: 0x0200015B RID: 347
	public struct ImeModeConversion
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00034B14 File Offset: 0x00032D14
		internal static ImeMode[] ChineseTable
		{
			get
			{
				return ImeModeConversion.chineseTable;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00034B1B File Offset: 0x00032D1B
		internal static ImeMode[] JapaneseTable
		{
			get
			{
				return ImeModeConversion.japaneseTable;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00034B22 File Offset: 0x00032D22
		internal static ImeMode[] KoreanTable
		{
			get
			{
				return ImeModeConversion.koreanTable;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00034B29 File Offset: 0x00032D29
		internal static ImeMode[] UnsupportedTable
		{
			get
			{
				return ImeModeConversion.unsupportedTable;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00034B30 File Offset: 0x00032D30
		internal static ImeMode[] InputLanguageTable
		{
			get
			{
				InputLanguage currentInputLanguage = InputLanguage.CurrentInputLanguage;
				int num = (int)((long)currentInputLanguage.Handle & 65535L);
				if (num <= 2052)
				{
					if (num <= 1041)
					{
						if (num != 1028)
						{
							if (num != 1041)
							{
								goto IL_8A;
							}
							return ImeModeConversion.japaneseTable;
						}
					}
					else
					{
						if (num == 1042)
						{
							goto IL_7E;
						}
						if (num != 2052)
						{
							goto IL_8A;
						}
					}
				}
				else if (num <= 3076)
				{
					if (num == 2066)
					{
						goto IL_7E;
					}
					if (num != 3076)
					{
						goto IL_8A;
					}
				}
				else if (num != 4100 && num != 5124)
				{
					goto IL_8A;
				}
				return ImeModeConversion.chineseTable;
				IL_7E:
				return ImeModeConversion.koreanTable;
				IL_8A:
				return ImeModeConversion.unsupportedTable;
			}
		}

		/// <summary>Gets a dictionary that contains the conversion mode flags corresponding to each <see cref="T:System.Windows.Forms.ImeMode" />.</summary>
		/// <returns>A dictionary that contains the flags to <see cref="T:System.Windows.Forms.ImeMode" /> mapping.</returns>
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00034BCC File Offset: 0x00032DCC
		public static Dictionary<ImeMode, ImeModeConversion> ImeModeConversionBits
		{
			get
			{
				if (ImeModeConversion.imeModeConversionBits == null)
				{
					ImeModeConversion.imeModeConversionBits = new Dictionary<ImeMode, ImeModeConversion>(7);
					ImeModeConversion value;
					value.setBits = 9;
					value.clearBits = 2;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Hiragana, value);
					value.setBits = 11;
					value.clearBits = 0;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Katakana, value);
					value.setBits = 3;
					value.clearBits = 8;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.KatakanaHalf, value);
					value.setBits = 8;
					value.clearBits = 3;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.AlphaFull, value);
					value.setBits = 0;
					value.clearBits = 11;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Alpha, value);
					value.setBits = 9;
					value.clearBits = 0;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.HangulFull, value);
					value.setBits = 1;
					value.clearBits = 8;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.Hangul, value);
					value.setBits = 1;
					value.clearBits = 10;
					ImeModeConversion.imeModeConversionBits.Add(ImeMode.OnHalf, value);
				}
				return ImeModeConversion.imeModeConversionBits;
			}
		}

		/// <summary>Gets a value that indicates whether the current language table is supported.</summary>
		/// <returns>
		///     <see langword="true" /> if the language table is supported; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00034CDB File Offset: 0x00032EDB
		public static bool IsCurrentConversionTableSupported
		{
			get
			{
				return ImeModeConversion.InputLanguageTable != ImeModeConversion.UnsupportedTable;
			}
		}

		// Token: 0x04000840 RID: 2112
		private static Dictionary<ImeMode, ImeModeConversion> imeModeConversionBits;

		// Token: 0x04000841 RID: 2113
		internal int setBits;

		// Token: 0x04000842 RID: 2114
		internal int clearBits;

		// Token: 0x04000843 RID: 2115
		internal const int ImeDisabled = 1;

		// Token: 0x04000844 RID: 2116
		internal const int ImeDirectInput = 2;

		// Token: 0x04000845 RID: 2117
		internal const int ImeClosed = 3;

		// Token: 0x04000846 RID: 2118
		internal const int ImeNativeInput = 4;

		// Token: 0x04000847 RID: 2119
		internal const int ImeNativeFullHiragana = 4;

		// Token: 0x04000848 RID: 2120
		internal const int ImeNativeHalfHiragana = 5;

		// Token: 0x04000849 RID: 2121
		internal const int ImeNativeFullKatakana = 6;

		// Token: 0x0400084A RID: 2122
		internal const int ImeNativeHalfKatakana = 7;

		// Token: 0x0400084B RID: 2123
		internal const int ImeAlphaFull = 8;

		// Token: 0x0400084C RID: 2124
		internal const int ImeAlphaHalf = 9;

		// Token: 0x0400084D RID: 2125
		private static ImeMode[] japaneseTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Off,
			ImeMode.Off,
			ImeMode.Hiragana,
			ImeMode.Hiragana,
			ImeMode.Katakana,
			ImeMode.KatakanaHalf,
			ImeMode.AlphaFull,
			ImeMode.Alpha
		};

		// Token: 0x0400084E RID: 2126
		private static ImeMode[] koreanTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Alpha,
			ImeMode.Alpha,
			ImeMode.HangulFull,
			ImeMode.Hangul,
			ImeMode.HangulFull,
			ImeMode.Hangul,
			ImeMode.AlphaFull,
			ImeMode.Alpha
		};

		// Token: 0x0400084F RID: 2127
		private static ImeMode[] chineseTable = new ImeMode[]
		{
			ImeMode.Inherit,
			ImeMode.Disable,
			ImeMode.Off,
			ImeMode.Close,
			ImeMode.On,
			ImeMode.OnHalf,
			ImeMode.On,
			ImeMode.OnHalf,
			ImeMode.Off,
			ImeMode.Off
		};

		// Token: 0x04000850 RID: 2128
		private static ImeMode[] unsupportedTable = new ImeMode[0];
	}
}
