using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>The <see cref="T:System.Drawing.Drawing2D.InterpolationMode" /> enumeration specifies the algorithm that is used when images are scaled or rotated. </summary>
	// Token: 0x020000C5 RID: 197
	public enum InterpolationMode
	{
		/// <summary>Equivalent to the <see cref="F:System.Drawing.Drawing2D.QualityMode.Invalid" /> element of the <see cref="T:System.Drawing.Drawing2D.QualityMode" /> enumeration.</summary>
		// Token: 0x040009CC RID: 2508
		Invalid = -1,
		/// <summary>Specifies default mode.</summary>
		// Token: 0x040009CD RID: 2509
		Default,
		/// <summary>Specifies low quality interpolation.</summary>
		// Token: 0x040009CE RID: 2510
		Low,
		/// <summary>Specifies high quality interpolation.</summary>
		// Token: 0x040009CF RID: 2511
		High,
		/// <summary>Specifies bilinear interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 50 percent of its original size. </summary>
		// Token: 0x040009D0 RID: 2512
		Bilinear,
		/// <summary>Specifies bicubic interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 25 percent of its original size.</summary>
		// Token: 0x040009D1 RID: 2513
		Bicubic,
		/// <summary>Specifies nearest-neighbor interpolation.</summary>
		// Token: 0x040009D2 RID: 2514
		NearestNeighbor,
		/// <summary>Specifies high-quality, bilinear interpolation. Prefiltering is performed to ensure high-quality shrinking. </summary>
		// Token: 0x040009D3 RID: 2515
		HighQualityBilinear,
		/// <summary>Specifies high-quality, bicubic interpolation. Prefiltering is performed to ensure high-quality shrinking. This mode produces the highest quality transformed images.</summary>
		// Token: 0x040009D4 RID: 2516
		HighQualityBicubic
	}
}
