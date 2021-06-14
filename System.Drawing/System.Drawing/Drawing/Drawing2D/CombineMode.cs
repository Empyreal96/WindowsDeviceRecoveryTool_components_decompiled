using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies how different clipping regions can be combined.</summary>
	// Token: 0x020000B6 RID: 182
	public enum CombineMode
	{
		/// <summary>One clipping region is replaced by another.</summary>
		// Token: 0x04000968 RID: 2408
		Replace,
		/// <summary>Two clipping regions are combined by taking their intersection.</summary>
		// Token: 0x04000969 RID: 2409
		Intersect,
		/// <summary>Two clipping regions are combined by taking the union of both.</summary>
		// Token: 0x0400096A RID: 2410
		Union,
		/// <summary>Two clipping regions are combined by taking only the areas enclosed by one or the other region, but not both.</summary>
		// Token: 0x0400096B RID: 2411
		Xor,
		/// <summary>Specifies that the existing region is replaced by the result of the new region being removed from the existing region. Said differently, the new region is excluded from the existing region.</summary>
		// Token: 0x0400096C RID: 2412
		Exclude,
		/// <summary>Specifies that the existing region is replaced by the result of the existing region being removed from the new region. Said differently, the existing region is excluded from the new region.</summary>
		// Token: 0x0400096D RID: 2413
		Complement
	}
}
