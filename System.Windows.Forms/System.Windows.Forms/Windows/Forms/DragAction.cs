using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies how and if a drag-and-drop operation should continue.</summary>
	// Token: 0x02000225 RID: 549
	[ComVisible(true)]
	public enum DragAction
	{
		/// <summary>The operation will continue.</summary>
		// Token: 0x04000E5C RID: 3676
		Continue,
		/// <summary>The operation will stop with a drop.</summary>
		// Token: 0x04000E5D RID: 3677
		Drop,
		/// <summary>The operation is canceled with no drop message.</summary>
		// Token: 0x04000E5E RID: 3678
		Cancel
	}
}
