using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the type of color data in the system palette. The data can be color data with alpha, grayscale data only, or halftone data.</summary>
	// Token: 0x020000AC RID: 172
	[Flags]
	public enum PaletteFlags
	{
		/// <summary>Alpha data.</summary>
		// Token: 0x04000931 RID: 2353
		HasAlpha = 1,
		/// <summary>Grayscale data.</summary>
		// Token: 0x04000932 RID: 2354
		GrayScale = 2,
		/// <summary>Halftone data.</summary>
		// Token: 0x04000933 RID: 2355
		Halftone = 4
	}
}
