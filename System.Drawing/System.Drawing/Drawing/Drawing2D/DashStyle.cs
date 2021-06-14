using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the style of dashed lines drawn with a <see cref="T:System.Drawing.Pen" /> object.</summary>
	// Token: 0x020000BC RID: 188
	public enum DashStyle
	{
		/// <summary>Specifies a solid line.</summary>
		// Token: 0x04000982 RID: 2434
		Solid,
		/// <summary>Specifies a line consisting of dashes.</summary>
		// Token: 0x04000983 RID: 2435
		Dash,
		/// <summary>Specifies a line consisting of dots.</summary>
		// Token: 0x04000984 RID: 2436
		Dot,
		/// <summary>Specifies a line consisting of a repeating pattern of dash-dot.</summary>
		// Token: 0x04000985 RID: 2437
		DashDot,
		/// <summary>Specifies a line consisting of a repeating pattern of dash-dot-dot.</summary>
		// Token: 0x04000986 RID: 2438
		DashDotDot,
		/// <summary>Specifies a user-defined custom dash style.</summary>
		// Token: 0x04000987 RID: 2439
		Custom
	}
}
