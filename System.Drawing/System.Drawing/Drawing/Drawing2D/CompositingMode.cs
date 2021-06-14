using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies how the source colors are combined with the background colors.</summary>
	// Token: 0x020000B7 RID: 183
	public enum CompositingMode
	{
		/// <summary>Specifies that when a color is rendered, it is blended with the background color. The blend is determined by the alpha component of the color being rendered.</summary>
		// Token: 0x0400096F RID: 2415
		SourceOver,
		/// <summary>Specifies that when a color is rendered, it overwrites the background color.</summary>
		// Token: 0x04000970 RID: 2416
		SourceCopy
	}
}
