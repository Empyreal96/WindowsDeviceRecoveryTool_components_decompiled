using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies the visual state of a text box that is drawn with visual styles.</summary>
	// Token: 0x02000454 RID: 1108
	public enum TextBoxState
	{
		/// <summary>The text box appears normal.</summary>
		// Token: 0x040031BD RID: 12733
		Normal = 1,
		/// <summary>The text box appears hot.</summary>
		// Token: 0x040031BE RID: 12734
		Hot,
		/// <summary>The text box appears selected.</summary>
		// Token: 0x040031BF RID: 12735
		Selected,
		/// <summary>The text box appears disabled.</summary>
		// Token: 0x040031C0 RID: 12736
		Disabled,
		/// <summary>The text box appears read-only.</summary>
		// Token: 0x040031C1 RID: 12737
		Readonly = 6,
		/// <summary>The text box appears in assist mode.</summary>
		// Token: 0x040031C2 RID: 12738
		Assist
	}
}
