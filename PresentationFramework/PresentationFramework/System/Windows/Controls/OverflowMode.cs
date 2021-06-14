using System;

namespace System.Windows.Controls
{
	/// <summary> Specifies how <see cref="T:System.Windows.Controls.ToolBar" /> items are placed in the main toolbar panel and in the overflow panel. </summary>
	// Token: 0x02000543 RID: 1347
	public enum OverflowMode
	{
		/// <summary> Item moves between the main panel and overflow panel, depending on the available space. </summary>
		// Token: 0x04002EAA RID: 11946
		AsNeeded,
		/// <summary> Item is permanently placed in the overflow panel. </summary>
		// Token: 0x04002EAB RID: 11947
		Always,
		/// <summary> Item is never allowed to overflow. </summary>
		// Token: 0x04002EAC RID: 11948
		Never
	}
}
