using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies how a texture or gradient is tiled when it is smaller than the area being filled.</summary>
	// Token: 0x020000D7 RID: 215
	public enum WrapMode
	{
		/// <summary>Tiles the gradient or texture.</summary>
		// Token: 0x04000A1F RID: 2591
		Tile,
		/// <summary>Reverses the texture or gradient horizontally and then tiles the texture or gradient.</summary>
		// Token: 0x04000A20 RID: 2592
		TileFlipX,
		/// <summary>Reverses the texture or gradient vertically and then tiles the texture or gradient.</summary>
		// Token: 0x04000A21 RID: 2593
		TileFlipY,
		/// <summary>Reverses the texture or gradient horizontally and vertically and then tiles the texture or gradient.</summary>
		// Token: 0x04000A22 RID: 2594
		TileFlipXY,
		/// <summary>The texture or gradient is not tiled.</summary>
		// Token: 0x04000A23 RID: 2595
		Clamp
	}
}
