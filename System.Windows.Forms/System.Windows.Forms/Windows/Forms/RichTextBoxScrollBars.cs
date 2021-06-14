using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the type of scroll bars to display in a <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
	// Token: 0x02000339 RID: 825
	public enum RichTextBoxScrollBars
	{
		/// <summary>No scroll bars are displayed.</summary>
		// Token: 0x04002019 RID: 8217
		None,
		/// <summary>Display a horizontal scroll bar only when text is longer than the width of the control.</summary>
		// Token: 0x0400201A RID: 8218
		Horizontal,
		/// <summary>Display a vertical scroll bar only when text is longer than the height of the control.</summary>
		// Token: 0x0400201B RID: 8219
		Vertical,
		/// <summary>Display both a horizontal and a vertical scroll bar when needed.</summary>
		// Token: 0x0400201C RID: 8220
		Both,
		/// <summary>Always display a horizontal scroll bar.</summary>
		// Token: 0x0400201D RID: 8221
		ForcedHorizontal = 17,
		/// <summary>Always display a vertical scroll bar.</summary>
		// Token: 0x0400201E RID: 8222
		ForcedVertical,
		/// <summary>Always display both a horizontal and a vertical scroll bar.</summary>
		// Token: 0x0400201F RID: 8223
		ForcedBoth
	}
}
