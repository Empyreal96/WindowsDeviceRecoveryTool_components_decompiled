using System;

namespace System.Drawing
{
	/// <summary>Specifies how much an image is rotated and the axis used to flip the image.</summary>
	// Token: 0x0200002F RID: 47
	public enum RotateFlipType
	{
		/// <summary>Specifies no clockwise rotation and no flipping.</summary>
		// Token: 0x04000305 RID: 773
		RotateNoneFlipNone,
		/// <summary>Specifies a 90-degree clockwise rotation without flipping.</summary>
		// Token: 0x04000306 RID: 774
		Rotate90FlipNone,
		/// <summary>Specifies a 180-degree clockwise rotation without flipping.</summary>
		// Token: 0x04000307 RID: 775
		Rotate180FlipNone,
		/// <summary>Specifies a 270-degree clockwise rotation without flipping.</summary>
		// Token: 0x04000308 RID: 776
		Rotate270FlipNone,
		/// <summary>Specifies no clockwise rotation followed by a horizontal flip.</summary>
		// Token: 0x04000309 RID: 777
		RotateNoneFlipX,
		/// <summary>Specifies a 90-degree clockwise rotation followed by a horizontal flip.</summary>
		// Token: 0x0400030A RID: 778
		Rotate90FlipX,
		/// <summary>Specifies a 180-degree clockwise rotation followed by a horizontal flip.</summary>
		// Token: 0x0400030B RID: 779
		Rotate180FlipX,
		/// <summary>Specifies a 270-degree clockwise rotation followed by a horizontal flip.</summary>
		// Token: 0x0400030C RID: 780
		Rotate270FlipX,
		/// <summary>Specifies no clockwise rotation followed by a vertical flip.</summary>
		// Token: 0x0400030D RID: 781
		RotateNoneFlipY = 6,
		/// <summary>Specifies a 90-degree clockwise rotation followed by a vertical flip.</summary>
		// Token: 0x0400030E RID: 782
		Rotate90FlipY,
		/// <summary>Specifies a 180-degree clockwise rotation followed by a vertical flip.</summary>
		// Token: 0x0400030F RID: 783
		Rotate180FlipY = 4,
		/// <summary>Specifies a 270-degree clockwise rotation followed by a vertical flip.</summary>
		// Token: 0x04000310 RID: 784
		Rotate270FlipY,
		/// <summary>Specifies no clockwise rotation followed by a horizontal and vertical flip.</summary>
		// Token: 0x04000311 RID: 785
		RotateNoneFlipXY = 2,
		/// <summary>Specifies a 90-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
		// Token: 0x04000312 RID: 786
		Rotate90FlipXY,
		/// <summary>Specifies a 180-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
		// Token: 0x04000313 RID: 787
		Rotate180FlipXY = 0,
		/// <summary>Specifies a 270-degree clockwise rotation followed by a horizontal and vertical flip.</summary>
		// Token: 0x04000314 RID: 788
		Rotate270FlipXY
	}
}
