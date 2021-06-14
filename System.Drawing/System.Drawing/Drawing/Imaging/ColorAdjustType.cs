using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies which GDI+ objects use color adjustment information.</summary>
	// Token: 0x0200008D RID: 141
	public enum ColorAdjustType
	{
		/// <summary>Color adjustment information that is used by all GDI+ objects that do not have their own color adjustment information.</summary>
		// Token: 0x0400073A RID: 1850
		Default,
		/// <summary>Color adjustment information for <see cref="T:System.Drawing.Bitmap" /> objects.</summary>
		// Token: 0x0400073B RID: 1851
		Bitmap,
		/// <summary>Color adjustment information for <see cref="T:System.Drawing.Brush" /> objects.</summary>
		// Token: 0x0400073C RID: 1852
		Brush,
		/// <summary>Color adjustment information for <see cref="T:System.Drawing.Pen" /> objects.</summary>
		// Token: 0x0400073D RID: 1853
		Pen,
		/// <summary>Color adjustment information for text.</summary>
		// Token: 0x0400073E RID: 1854
		Text,
		/// <summary>The number of types specified.</summary>
		// Token: 0x0400073F RID: 1855
		Count,
		/// <summary>The number of types specified.</summary>
		// Token: 0x04000740 RID: 1856
		Any
	}
}
