using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the behaviors of a link in a <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
	// Token: 0x020002B1 RID: 689
	public enum LinkBehavior
	{
		/// <summary>The behavior of this setting depends on the options set using the Internet Options dialog box in Control Panel or Internet Explorer.</summary>
		// Token: 0x04001176 RID: 4470
		SystemDefault,
		/// <summary>The link always displays with underlined text.</summary>
		// Token: 0x04001177 RID: 4471
		AlwaysUnderline,
		/// <summary>The link displays underlined text only when the mouse is hovered over the link text.</summary>
		// Token: 0x04001178 RID: 4472
		HoverUnderline,
		/// <summary>The link text is never underlined. The link can still be distinguished from other text by use of the <see cref="P:System.Windows.Forms.LinkLabel.LinkColor" /> property of the <see cref="T:System.Windows.Forms.LinkLabel" /> control.</summary>
		// Token: 0x04001179 RID: 4473
		NeverUnderline
	}
}
