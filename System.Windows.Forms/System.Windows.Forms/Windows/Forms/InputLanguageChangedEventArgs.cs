using System;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Form.InputLanguageChanged" /> event.</summary>
	// Token: 0x0200028A RID: 650
	public class InputLanguageChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.InputLanguageChangedEventArgs" /> class with the specified locale and character set.</summary>
		/// <param name="culture">The locale of the input language. </param>
		/// <param name="charSet">The character set associated with the new input language. </param>
		// Token: 0x060026AE RID: 9902 RVA: 0x000B7014 File Offset: 0x000B5214
		public InputLanguageChangedEventArgs(CultureInfo culture, byte charSet)
		{
			this.inputLanguage = InputLanguage.FromCulture(culture);
			this.culture = culture;
			this.charSet = charSet;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.InputLanguageChangedEventArgs" /> class with the specified input language and character set.</summary>
		/// <param name="inputLanguage">The input language. </param>
		/// <param name="charSet">The character set associated with the new input language. </param>
		// Token: 0x060026AF RID: 9903 RVA: 0x000B7036 File Offset: 0x000B5236
		public InputLanguageChangedEventArgs(InputLanguage inputLanguage, byte charSet)
		{
			this.inputLanguage = inputLanguage;
			this.culture = inputLanguage.Culture;
			this.charSet = charSet;
		}

		/// <summary>Gets a value indicating the input language.</summary>
		/// <returns>The input language associated with the object.</returns>
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000B7058 File Offset: 0x000B5258
		public InputLanguage InputLanguage
		{
			get
			{
				return this.inputLanguage;
			}
		}

		/// <summary>Gets the locale of the input language.</summary>
		/// <returns>A <see cref="T:System.Globalization.CultureInfo" /> that specifies the locale of the input language.</returns>
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000B7060 File Offset: 0x000B5260
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		/// <summary>Gets the character set associated with the new input language.</summary>
		/// <returns>An 8-bit unsigned integer that corresponds to the character set, as shown in the following table.Character Set Value ANSI_CHARSET 0 DEFAULT_CHARSET 1 SYMBOL_CHARSET 2 MAC_CHARSET 77 SHIFTJI_CHARSET 128 HANGEUL_CHARSET 129 HANGUL_CHARSET 129 JOHAB_CHARSET 130 GB2312_CHARSET 134 CHINESEBIG5_CHARSET 136 GREEK_CHARSET 161 TURKISH_CHARSET 162 VIETNAMESE_CHARSET 163 HEBREW_CHARSET 177 ARABIC_CHARSET 178 BALTIC_CHARSET 186 RUSSIAN_CHARSET 204 THAI_CHARSET 222 EASTEUROPE_CHARSET 238 OEM_CHARSET 255 </returns>
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000B7068 File Offset: 0x000B5268
		public byte CharSet
		{
			get
			{
				return this.charSet;
			}
		}

		// Token: 0x04001060 RID: 4192
		private readonly InputLanguage inputLanguage;

		// Token: 0x04001061 RID: 4193
		private readonly CultureInfo culture;

		// Token: 0x04001062 RID: 4194
		private readonly byte charSet;
	}
}
