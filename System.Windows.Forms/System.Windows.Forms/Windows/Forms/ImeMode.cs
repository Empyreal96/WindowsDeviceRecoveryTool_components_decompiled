using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies a value that determines the Input Method Editor (IME) status of an object when the object is selected.</summary>
	// Token: 0x02000287 RID: 647
	[ComVisible(true)]
	public enum ImeMode
	{
		/// <summary>Inherits the IME mode of the parent control.</summary>
		// Token: 0x04001052 RID: 4178
		Inherit = -1,
		/// <summary>None (Default).</summary>
		// Token: 0x04001053 RID: 4179
		NoControl,
		/// <summary>The IME is on. This value indicates that the IME is on and characters specific to Chinese or Japanese can be entered. This setting is valid for Japanese, Simplified Chinese, and Traditional Chinese IME only.</summary>
		// Token: 0x04001054 RID: 4180
		On,
		/// <summary>The IME is off. This mode indicates that the IME is off, meaning that the object behaves the same as English entry mode. This setting is valid for Japanese, Simplified Chinese, and Traditional Chinese IME only.</summary>
		// Token: 0x04001055 RID: 4181
		Off,
		/// <summary>The IME is disabled. With this setting, the users cannot turn the IME on from the keyboard, and the IME floating window is hidden.</summary>
		// Token: 0x04001056 RID: 4182
		Disable,
		/// <summary>Hiragana DBC. This setting is valid for the Japanese IME only.</summary>
		// Token: 0x04001057 RID: 4183
		Hiragana,
		/// <summary>Katakana DBC. This setting is valid for the Japanese IME only.</summary>
		// Token: 0x04001058 RID: 4184
		Katakana,
		/// <summary>Katakana SBC. This setting is valid for the Japanese IME only.</summary>
		// Token: 0x04001059 RID: 4185
		KatakanaHalf,
		/// <summary>Alphanumeric double-byte characters. This setting is valid for Korean and Japanese IME only.</summary>
		// Token: 0x0400105A RID: 4186
		AlphaFull,
		/// <summary>Alphanumeric single-byte characters(SBC). This setting is valid for Korean and Japanese IME only.</summary>
		// Token: 0x0400105B RID: 4187
		Alpha,
		/// <summary>Hangul DBC. This setting is valid for the Korean IME only.</summary>
		// Token: 0x0400105C RID: 4188
		HangulFull,
		/// <summary>Hangul SBC. This setting is valid for the Korean IME only.</summary>
		// Token: 0x0400105D RID: 4189
		Hangul,
		/// <summary>IME closed. This setting is valid for Chinese IME only.</summary>
		// Token: 0x0400105E RID: 4190
		Close,
		/// <summary>IME on HalfShape. This setting is valid for Chinese IME only.</summary>
		// Token: 0x0400105F RID: 4191
		OnHalf
	}
}
