using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies when the visual style selects a different multiple-image file to draw an element.</summary>
	// Token: 0x02000463 RID: 1123
	public enum ImageSelectType
	{
		/// <summary>The image file does not change.</summary>
		// Token: 0x0400320E RID: 12814
		None,
		/// <summary>Image file changes are based on size settings.</summary>
		// Token: 0x0400320F RID: 12815
		Size,
		/// <summary>Image file changes are based on dots per inch (DPI) settings.</summary>
		// Token: 0x04003210 RID: 12816
		Dpi
	}
}
