using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies individual channels in the CMYK (cyan, magenta, yellow, black) color space. This enumeration is used by the <see cref="Overload:System.Drawing.Imaging.ImageAttributes.SetOutputChannel" /> methods.</summary>
	// Token: 0x0200008E RID: 142
	public enum ColorChannelFlag
	{
		/// <summary>The cyan color channel.</summary>
		// Token: 0x04000742 RID: 1858
		ColorChannelC,
		/// <summary>The magenta color channel.</summary>
		// Token: 0x04000743 RID: 1859
		ColorChannelM,
		/// <summary>The yellow color channel.</summary>
		// Token: 0x04000744 RID: 1860
		ColorChannelY,
		/// <summary>The black color channel.</summary>
		// Token: 0x04000745 RID: 1861
		ColorChannelK,
		/// <summary>The last selected channel should be used.</summary>
		// Token: 0x04000746 RID: 1862
		ColorChannelLast
	}
}
