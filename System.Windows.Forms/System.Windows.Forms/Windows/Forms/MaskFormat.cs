using System;

namespace System.Windows.Forms
{
	/// <summary>Defines how to format the text inside of a <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
	// Token: 0x020002D7 RID: 727
	public enum MaskFormat
	{
		/// <summary>Return text input by the user as well as any instances of the prompt character.</summary>
		// Token: 0x040012D0 RID: 4816
		IncludePrompt = 1,
		/// <summary>Return text input by the user as well as any literal characters defined in the mask.</summary>
		// Token: 0x040012D1 RID: 4817
		IncludeLiterals,
		/// <summary>Return text input by the user as well as any literal characters defined in the mask and any instances of the prompt character. </summary>
		// Token: 0x040012D2 RID: 4818
		IncludePromptAndLiterals,
		/// <summary>Return only text input by the user. </summary>
		// Token: 0x040012D3 RID: 4819
		ExcludePromptAndLiterals = 0
	}
}
