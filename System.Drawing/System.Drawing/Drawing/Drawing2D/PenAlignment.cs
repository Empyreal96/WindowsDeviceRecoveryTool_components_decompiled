using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the alignment of a <see cref="T:System.Drawing.Pen" /> object in relation to the theoretical, zero-width line.</summary>
	// Token: 0x020000CF RID: 207
	public enum PenAlignment
	{
		/// <summary>Specifies that the <see cref="T:System.Drawing.Pen" /> object is centered over the theoretical line.</summary>
		// Token: 0x040009FC RID: 2556
		Center,
		/// <summary>Specifies that the <see cref="T:System.Drawing.Pen" /> is positioned on the inside of the theoretical line.</summary>
		// Token: 0x040009FD RID: 2557
		Inset,
		/// <summary>Specifies the <see cref="T:System.Drawing.Pen" /> is positioned on the outside of the theoretical line.</summary>
		// Token: 0x040009FE RID: 2558
		Outset,
		/// <summary>Specifies the <see cref="T:System.Drawing.Pen" /> is positioned to the left of the theoretical line.</summary>
		// Token: 0x040009FF RID: 2559
		Left,
		/// <summary>Specifies the <see cref="T:System.Drawing.Pen" /> is positioned to the right of the theoretical line.</summary>
		// Token: 0x04000A00 RID: 2560
		Right
	}
}
