using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the overall quality when rendering GDI+ objects.</summary>
	// Token: 0x020000D2 RID: 210
	public enum QualityMode
	{
		/// <summary>Specifies an invalid mode.</summary>
		// Token: 0x04000A0F RID: 2575
		Invalid = -1,
		/// <summary>Specifies the default mode.</summary>
		// Token: 0x04000A10 RID: 2576
		Default,
		/// <summary>Specifies low quality, high speed rendering.</summary>
		// Token: 0x04000A11 RID: 2577
		Low,
		/// <summary>Specifies high quality, low speed rendering.</summary>
		// Token: 0x04000A12 RID: 2578
		High
	}
}
