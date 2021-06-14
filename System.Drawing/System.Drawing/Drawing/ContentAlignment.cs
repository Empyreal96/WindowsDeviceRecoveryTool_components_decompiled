using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Drawing
{
	/// <summary>Specifies alignment of content on the drawing surface.</summary>
	// Token: 0x02000019 RID: 25
	[Editor("System.Drawing.Design.ContentAlignmentEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum ContentAlignment
	{
		/// <summary>Content is vertically aligned at the top, and horizontally aligned on the left.</summary>
		// Token: 0x04000159 RID: 345
		TopLeft = 1,
		/// <summary>Content is vertically aligned at the top, and horizontally aligned at the center.</summary>
		// Token: 0x0400015A RID: 346
		TopCenter,
		/// <summary>Content is vertically aligned at the top, and horizontally aligned on the right.</summary>
		// Token: 0x0400015B RID: 347
		TopRight = 4,
		/// <summary>Content is vertically aligned in the middle, and horizontally aligned on the left.</summary>
		// Token: 0x0400015C RID: 348
		MiddleLeft = 16,
		/// <summary>Content is vertically aligned in the middle, and horizontally aligned at the center.</summary>
		// Token: 0x0400015D RID: 349
		MiddleCenter = 32,
		/// <summary>Content is vertically aligned in the middle, and horizontally aligned on the right.</summary>
		// Token: 0x0400015E RID: 350
		MiddleRight = 64,
		/// <summary>Content is vertically aligned at the bottom, and horizontally aligned on the left.</summary>
		// Token: 0x0400015F RID: 351
		BottomLeft = 256,
		/// <summary>Content is vertically aligned at the bottom, and horizontally aligned at the center.</summary>
		// Token: 0x04000160 RID: 352
		BottomCenter = 512,
		/// <summary>Content is vertically aligned at the bottom, and horizontally aligned on the right.</summary>
		// Token: 0x04000161 RID: 353
		BottomRight = 1024
	}
}
