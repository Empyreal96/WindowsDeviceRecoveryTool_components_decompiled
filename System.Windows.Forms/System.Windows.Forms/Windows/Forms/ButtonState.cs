using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the appearance of a button.</summary>
	// Token: 0x02000136 RID: 310
	[Flags]
	public enum ButtonState
	{
		/// <summary>The button has a checked or latched appearance. Use this appearance to show that a toggle button has been pressed.</summary>
		// Token: 0x04000698 RID: 1688
		Checked = 1024,
		/// <summary>The button has a flat, two-dimensional appearance.</summary>
		// Token: 0x04000699 RID: 1689
		Flat = 16384,
		/// <summary>The button is inactive (grayed).</summary>
		// Token: 0x0400069A RID: 1690
		Inactive = 256,
		/// <summary>The button has its normal appearance (three-dimensional).</summary>
		// Token: 0x0400069B RID: 1691
		Normal = 0,
		/// <summary>The button appears pressed.</summary>
		// Token: 0x0400069C RID: 1692
		Pushed = 512,
		/// <summary>All flags except <see langword="Normal" /> are set.</summary>
		// Token: 0x0400069D RID: 1693
		All = 18176
	}
}
