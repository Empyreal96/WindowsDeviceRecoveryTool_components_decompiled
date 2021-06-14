using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how tabs in a tab control are sized.</summary>
	// Token: 0x0200038E RID: 910
	public enum TabSizeMode
	{
		/// <summary>The width of each tab is sized to accommodate what is displayed on the tab, and the size of tabs in a row are not adjusted to fill the entire width of the container control.</summary>
		// Token: 0x04002283 RID: 8835
		Normal,
		/// <summary>The width of each tab is sized so that each row of tabs fills the entire width of the container control. This is only applicable to tab controls with more than one row.</summary>
		// Token: 0x04002284 RID: 8836
		FillToRight,
		/// <summary>All tabs in a control are the same width.</summary>
		// Token: 0x04002285 RID: 8837
		Fixed
	}
}
