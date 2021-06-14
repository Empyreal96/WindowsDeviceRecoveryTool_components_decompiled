using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the position of the text and image relative to each other on a control.</summary>
	// Token: 0x02000394 RID: 916
	public enum TextImageRelation
	{
		/// <summary>Specifies that the image and text share the same space on a control.</summary>
		// Token: 0x040022B7 RID: 8887
		Overlay,
		/// <summary>Specifies that the image is displayed horizontally before the text of a control.</summary>
		// Token: 0x040022B8 RID: 8888
		ImageBeforeText = 4,
		/// <summary>Specifies that the text is displayed horizontally before the image of a control.</summary>
		// Token: 0x040022B9 RID: 8889
		TextBeforeImage = 8,
		/// <summary>Specifies that the image is displayed vertically above the text of a control.</summary>
		// Token: 0x040022BA RID: 8890
		ImageAboveText = 1,
		/// <summary>Specifies that the text is displayed vertically above the image of a control.</summary>
		// Token: 0x040022BB RID: 8891
		TextAboveImage
	}
}
