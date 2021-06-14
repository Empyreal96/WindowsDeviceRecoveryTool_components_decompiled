using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the layout for an image contained in a <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
	// Token: 0x020001EF RID: 495
	public enum DataGridViewImageCellLayout
	{
		/// <summary>The layout specification has not been set.</summary>
		// Token: 0x04000D38 RID: 3384
		NotSet,
		/// <summary>The graphic is displayed centered using its native resolution.</summary>
		// Token: 0x04000D39 RID: 3385
		Normal,
		/// <summary>The graphic is stretched by the percentages required to fit the width and height of the containing cell.</summary>
		// Token: 0x04000D3A RID: 3386
		Stretch,
		/// <summary>The graphic is uniformly enlarged until it fills the width or height of the containing cell.</summary>
		// Token: 0x04000D3B RID: 3387
		Zoom
	}
}
