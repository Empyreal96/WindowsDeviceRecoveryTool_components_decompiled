using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a control anchors to the edges of its container.</summary>
	// Token: 0x0200010F RID: 271
	[Editor("System.Windows.Forms.Design.AnchorEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Flags]
	public enum AnchorStyles
	{
		/// <summary>The control is anchored to the top edge of its container.</summary>
		// Token: 0x04000516 RID: 1302
		Top = 1,
		/// <summary>The control is anchored to the bottom edge of its container.</summary>
		// Token: 0x04000517 RID: 1303
		Bottom = 2,
		/// <summary>The control is anchored to the left edge of its container.</summary>
		// Token: 0x04000518 RID: 1304
		Left = 4,
		/// <summary>The control is anchored to the right edge of its container.</summary>
		// Token: 0x04000519 RID: 1305
		Right = 8,
		/// <summary>The control is not anchored to any edges of its container.</summary>
		// Token: 0x0400051A RID: 1306
		None = 0
	}
}
