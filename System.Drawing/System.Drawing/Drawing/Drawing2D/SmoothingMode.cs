using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies whether smoothing (antialiasing) is applied to lines and curves and the edges of filled areas.</summary>
	// Token: 0x020000D5 RID: 213
	public enum SmoothingMode
	{
		/// <summary>Specifies an invalid mode.</summary>
		// Token: 0x04000A15 RID: 2581
		Invalid = -1,
		/// <summary>Specifies no antialiasing.</summary>
		// Token: 0x04000A16 RID: 2582
		Default,
		/// <summary>Specifies no antialiasing.</summary>
		// Token: 0x04000A17 RID: 2583
		HighSpeed,
		/// <summary>Specifies antialiased rendering.</summary>
		// Token: 0x04000A18 RID: 2584
		HighQuality,
		/// <summary>Specifies no antialiasing.</summary>
		// Token: 0x04000A19 RID: 2585
		None,
		/// <summary>Specifies antialiased rendering.</summary>
		// Token: 0x04000A1A RID: 2586
		AntiAlias
	}
}
