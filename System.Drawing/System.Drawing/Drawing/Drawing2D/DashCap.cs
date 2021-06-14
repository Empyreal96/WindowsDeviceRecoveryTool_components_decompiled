using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the type of graphic shape to use on both ends of each dash in a dashed line.</summary>
	// Token: 0x020000B1 RID: 177
	public enum DashCap
	{
		/// <summary>Specifies a square cap that squares off both ends of each dash.</summary>
		// Token: 0x0400095A RID: 2394
		Flat,
		/// <summary>Specifies a circular cap that rounds off both ends of each dash.</summary>
		// Token: 0x0400095B RID: 2395
		Round = 2,
		/// <summary>Specifies a triangular cap that points both ends of each dash.</summary>
		// Token: 0x0400095C RID: 2396
		Triangle
	}
}
