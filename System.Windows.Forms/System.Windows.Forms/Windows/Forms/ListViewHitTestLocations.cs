using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that represent areas in a <see cref="T:System.Windows.Forms.ListView" /> or <see cref="T:System.Windows.Forms.ListViewItem" />. </summary>
	// Token: 0x020002CA RID: 714
	[Flags]
	public enum ListViewHitTestLocations
	{
		/// <summary>A position outside the bounds of a <see cref="T:System.Windows.Forms.ListViewItem" /></summary>
		// Token: 0x04001264 RID: 4708
		None = 1,
		/// <summary>A position above the client portion of a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001265 RID: 4709
		AboveClientArea = 256,
		/// <summary>A position below the client portion of a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001266 RID: 4710
		BelowClientArea = 16,
		/// <summary>A position to the left of the client portion of a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001267 RID: 4711
		LeftOfClientArea = 64,
		/// <summary>A position to the right of the client portion of a <see cref="T:System.Windows.Forms.ListView" /> control.</summary>
		// Token: 0x04001268 RID: 4712
		RightOfClientArea = 32,
		/// <summary>A position within the bounds of an image contained in a <see cref="T:System.Windows.Forms.ListView" /> or <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		// Token: 0x04001269 RID: 4713
		Image = 2,
		/// <summary>A position within the bounds of an image associated with a <see cref="T:System.Windows.Forms.ListViewItem" /> that indicates the state of the item.</summary>
		// Token: 0x0400126A RID: 4714
		StateImage = 512,
		/// <summary>A position within the bounds of a text area contained in a <see cref="T:System.Windows.Forms.ListView" /> or <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		// Token: 0x0400126B RID: 4715
		Label = 4
	}
}
