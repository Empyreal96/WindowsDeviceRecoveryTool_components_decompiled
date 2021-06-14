using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies what to render (image or text) for this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
	// Token: 0x020003C1 RID: 961
	public enum ToolStripItemDisplayStyle
	{
		/// <summary>Specifies that neither image nor text is to be rendered for this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		// Token: 0x0400249A RID: 9370
		None,
		/// <summary>Specifies that only text is to be rendered for this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		// Token: 0x0400249B RID: 9371
		Text,
		/// <summary>Specifies that only an image is to be rendered for this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		// Token: 0x0400249C RID: 9372
		Image,
		/// <summary>Specifies that both an image and text are to be rendered for this <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		// Token: 0x0400249D RID: 9373
		ImageAndText
	}
}
