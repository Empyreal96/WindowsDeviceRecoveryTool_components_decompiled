using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies how pixels are offset during rendering.</summary>
	// Token: 0x020000D1 RID: 209
	public enum PixelOffsetMode
	{
		/// <summary>Specifies an invalid mode.</summary>
		// Token: 0x04000A08 RID: 2568
		Invalid = -1,
		/// <summary>Specifies the default mode.</summary>
		// Token: 0x04000A09 RID: 2569
		Default,
		/// <summary>Specifies high speed, low quality rendering.</summary>
		// Token: 0x04000A0A RID: 2570
		HighSpeed,
		/// <summary>Specifies high quality, low speed rendering.</summary>
		// Token: 0x04000A0B RID: 2571
		HighQuality,
		/// <summary>Specifies no pixel offset.</summary>
		// Token: 0x04000A0C RID: 2572
		None,
		/// <summary>Specifies that pixels are offset by -.5 units, both horizontally and vertically, for high speed antialiasing.</summary>
		// Token: 0x04000A0D RID: 2573
		Half
	}
}
