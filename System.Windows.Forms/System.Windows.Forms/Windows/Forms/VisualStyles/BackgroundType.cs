using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies the source of a visual style element's background.</summary>
	// Token: 0x02000457 RID: 1111
	public enum BackgroundType
	{
		/// <summary>The background of the element is a bitmap. If this value is set, then the property corresponding to the <see cref="F:System.Windows.Forms.VisualStyles.FilenameProperty.ImageFile" /> value will contain the name of a valid image file.</summary>
		// Token: 0x040031D0 RID: 12752
		ImageFile,
		/// <summary>The background of the element is a rectangle filled with a color or pattern. </summary>
		// Token: 0x040031D1 RID: 12753
		BorderFill,
		/// <summary>The element has no background.</summary>
		// Token: 0x040031D2 RID: 12754
		None
	}
}
