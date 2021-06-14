using System;

namespace System.Windows
{
	/// <summary>Indicates where an element should be displayed on the horizontal axis relative to the allocated layout slot of the parent element. </summary>
	// Token: 0x020000C1 RID: 193
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public enum HorizontalAlignment
	{
		/// <summary>An element aligned to the left of the layout slot for the parent element. </summary>
		// Token: 0x0400064D RID: 1613
		Left,
		/// <summary>An element aligned to the center of the layout slot for the parent element. </summary>
		// Token: 0x0400064E RID: 1614
		Center,
		/// <summary>An element aligned to the right of the layout slot for the parent element. </summary>
		// Token: 0x0400064F RID: 1615
		Right,
		/// <summary>An element stretched to fill the entire layout slot of the parent element. </summary>
		// Token: 0x04000650 RID: 1616
		Stretch
	}
}
