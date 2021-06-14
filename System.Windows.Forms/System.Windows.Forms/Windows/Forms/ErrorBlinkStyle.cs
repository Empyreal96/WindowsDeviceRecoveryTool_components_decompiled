using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants indicating when the error icon, supplied by an <see cref="T:System.Windows.Forms.ErrorProvider" />, should blink to alert the user that an error has occurred.</summary>
	// Token: 0x0200023A RID: 570
	public enum ErrorBlinkStyle
	{
		/// <summary>Blinks when the icon is already displayed and a new error string is set for the control.</summary>
		// Token: 0x04000EB3 RID: 3763
		BlinkIfDifferentError,
		/// <summary>Always blink when the error icon is first displayed, or when a error description string is set for the control and the error icon is already displayed.</summary>
		// Token: 0x04000EB4 RID: 3764
		AlwaysBlink,
		/// <summary>Never blink the error icon.</summary>
		// Token: 0x04000EB5 RID: 3765
		NeverBlink
	}
}
