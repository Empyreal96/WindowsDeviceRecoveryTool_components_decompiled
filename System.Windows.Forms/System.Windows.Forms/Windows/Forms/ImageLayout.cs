using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the position of the image on the control.</summary>
	// Token: 0x02000283 RID: 643
	public enum ImageLayout
	{
		/// <summary>The image is left-aligned at the top across the control's client rectangle.</summary>
		// Token: 0x04001036 RID: 4150
		None,
		/// <summary>The image is tiled across the control's client rectangle.</summary>
		// Token: 0x04001037 RID: 4151
		Tile,
		/// <summary>The image is centered within the control's client rectangle.</summary>
		// Token: 0x04001038 RID: 4152
		Center,
		/// <summary>The image is streched across the control's client rectangle.</summary>
		// Token: 0x04001039 RID: 4153
		Stretch,
		/// <summary>The image is enlarged within the control's client rectangle.</summary>
		// Token: 0x0400103A RID: 4154
		Zoom
	}
}
