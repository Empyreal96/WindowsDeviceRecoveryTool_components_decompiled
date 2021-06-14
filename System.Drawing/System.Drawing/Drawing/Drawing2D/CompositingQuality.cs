using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the quality level to use during compositing.</summary>
	// Token: 0x020000B8 RID: 184
	public enum CompositingQuality
	{
		/// <summary>Invalid quality.</summary>
		// Token: 0x04000972 RID: 2418
		Invalid = -1,
		/// <summary>Default quality.</summary>
		// Token: 0x04000973 RID: 2419
		Default,
		/// <summary>High speed, low quality.</summary>
		// Token: 0x04000974 RID: 2420
		HighSpeed,
		/// <summary>High quality, low speed compositing.</summary>
		// Token: 0x04000975 RID: 2421
		HighQuality,
		/// <summary>Gamma correction is used.</summary>
		// Token: 0x04000976 RID: 2422
		GammaCorrected,
		/// <summary>Assume linear values.</summary>
		// Token: 0x04000977 RID: 2423
		AssumeLinear
	}
}
