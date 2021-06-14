using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies how visual styles are applied to the current application.</summary>
	// Token: 0x02000449 RID: 1097
	public enum VisualStyleState
	{
		/// <summary>Visual styles are not applied to the application.</summary>
		// Token: 0x04003172 RID: 12658
		NoneEnabled,
		/// <summary>Visual styles are applied only to the client area.</summary>
		// Token: 0x04003173 RID: 12659
		ClientAreaEnabled = 2,
		/// <summary>Visual styles are applied only to the nonclient area.</summary>
		// Token: 0x04003174 RID: 12660
		NonClientAreaEnabled = 1,
		/// <summary>Visual styles are applied to client and nonclient areas.</summary>
		// Token: 0x04003175 RID: 12661
		ClientAndNonClientAreasEnabled = 3
	}
}
