using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the type of fill a <see cref="T:System.Drawing.Pen" /> object uses to fill lines.</summary>
	// Token: 0x020000D0 RID: 208
	public enum PenType
	{
		/// <summary>Specifies a solid fill.</summary>
		// Token: 0x04000A02 RID: 2562
		SolidColor,
		/// <summary>Specifies a hatch fill.</summary>
		// Token: 0x04000A03 RID: 2563
		HatchFill,
		/// <summary>Specifies a bitmap texture fill.</summary>
		// Token: 0x04000A04 RID: 2564
		TextureFill,
		/// <summary>Specifies a path gradient fill.</summary>
		// Token: 0x04000A05 RID: 2565
		PathGradient,
		/// <summary>Specifies a linear gradient fill.</summary>
		// Token: 0x04000A06 RID: 2566
		LinearGradient
	}
}
