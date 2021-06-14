using System;

namespace System.Windows.Forms
{
	/// <summary>Provides options that specify the relationship between the control and preprocessing messages.</summary>
	// Token: 0x02000312 RID: 786
	public enum PreProcessControlState
	{
		/// <summary>Specifies that the message has been processed and no further processing is required.</summary>
		// Token: 0x04001D96 RID: 7574
		MessageProcessed,
		/// <summary>Specifies that the control requires the message and that processing should continue.</summary>
		// Token: 0x04001D97 RID: 7575
		MessageNeeded,
		/// <summary>Specifies that the control does not require the message.</summary>
		// Token: 0x04001D98 RID: 7576
		MessageNotNeeded
	}
}
