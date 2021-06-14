using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Form.InputLanguageChanging" /> event.</summary>
	// Token: 0x0200028C RID: 652
	public class InputLanguageChangingEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.InputLanguageChangingEventArgs" /> class with the specified locale, character set, and acceptance.</summary>
		/// <param name="culture">The locale of the requested input language. </param>
		/// <param name="sysCharSet">
		///       <see langword="true" /> if the system default font supports the character set required for the requested input language; otherwise, <see langword="false" />. </param>
		// Token: 0x060026B7 RID: 9911 RVA: 0x000B7070 File Offset: 0x000B5270
		public InputLanguageChangingEventArgs(CultureInfo culture, bool sysCharSet)
		{
			this.inputLanguage = InputLanguage.FromCulture(culture);
			this.culture = culture;
			this.sysCharSet = sysCharSet;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.InputLanguageChangingEventArgs" /> class with the specified input language, character set, and acceptance of a language change.</summary>
		/// <param name="inputLanguage">The requested input language. </param>
		/// <param name="sysCharSet">
		///       <see langword="true" /> if the system default font supports the character set required for the requested input language; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inputLanguage" /> is <see langword="null" />. </exception>
		// Token: 0x060026B8 RID: 9912 RVA: 0x000B7092 File Offset: 0x000B5292
		public InputLanguageChangingEventArgs(InputLanguage inputLanguage, bool sysCharSet)
		{
			if (inputLanguage == null)
			{
				throw new ArgumentNullException("inputLanguage");
			}
			this.inputLanguage = inputLanguage;
			this.culture = inputLanguage.Culture;
			this.sysCharSet = sysCharSet;
		}

		/// <summary>Gets a value indicating the input language.</summary>
		/// <returns>The input language.</returns>
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x000B70C2 File Offset: 0x000B52C2
		public InputLanguage InputLanguage
		{
			get
			{
				return this.inputLanguage;
			}
		}

		/// <summary>Gets the locale of the requested input language.</summary>
		/// <returns>A <see cref="T:System.Globalization.CultureInfo" /> that specifies the locale of the requested input language.</returns>
		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x000B70CA File Offset: 0x000B52CA
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		/// <summary>Gets a value indicating whether the system default font supports the character set required for the requested input language.</summary>
		/// <returns>
		///     <see langword="true" /> if the system default font supports the character set required for the requested input language; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060026BB RID: 9915 RVA: 0x000B70D2 File Offset: 0x000B52D2
		public bool SysCharSet
		{
			get
			{
				return this.sysCharSet;
			}
		}

		// Token: 0x04001063 RID: 4195
		private readonly InputLanguage inputLanguage;

		// Token: 0x04001064 RID: 4196
		private readonly CultureInfo culture;

		// Token: 0x04001065 RID: 4197
		private readonly bool sysCharSet;
	}
}
