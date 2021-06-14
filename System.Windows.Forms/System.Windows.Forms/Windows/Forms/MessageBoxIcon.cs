using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants defining which information to display.</summary>
	// Token: 0x020002EC RID: 748
	public enum MessageBoxIcon
	{
		/// <summary>The message box contain no symbols.</summary>
		// Token: 0x04001341 RID: 4929
		None,
		/// <summary>The message box contains a symbol consisting of a white X in a circle with a red background.</summary>
		// Token: 0x04001342 RID: 4930
		Hand = 16,
		/// <summary>The message box contains a symbol consisting of a question mark in a circle. The question-mark message icon is no longer recommended because it does not clearly represent a specific type of message and because the phrasing of a message as a question could apply to any message type. In addition, users can confuse the message symbol question mark with Help information. Therefore, do not use this question mark message symbol in your message boxes. The system continues to support its inclusion only for backward compatibility.</summary>
		// Token: 0x04001343 RID: 4931
		Question = 32,
		/// <summary>The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.</summary>
		// Token: 0x04001344 RID: 4932
		Exclamation = 48,
		/// <summary>The message box contains a symbol consisting of a lowercase letter i in a circle.</summary>
		// Token: 0x04001345 RID: 4933
		Asterisk = 64,
		/// <summary>The message box contains a symbol consisting of white X in a circle with a red background.</summary>
		// Token: 0x04001346 RID: 4934
		Stop = 16,
		/// <summary>The message box contains a symbol consisting of white X in a circle with a red background.</summary>
		// Token: 0x04001347 RID: 4935
		Error = 16,
		/// <summary>The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.</summary>
		// Token: 0x04001348 RID: 4936
		Warning = 48,
		/// <summary>The message box contains a symbol consisting of a lowercase letter i in a circle.</summary>
		// Token: 0x04001349 RID: 4937
		Information = 64
	}
}
