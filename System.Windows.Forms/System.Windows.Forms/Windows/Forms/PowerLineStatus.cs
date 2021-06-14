using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the system power status.</summary>
	// Token: 0x0200030E RID: 782
	public enum PowerLineStatus
	{
		/// <summary>The system is offline.</summary>
		// Token: 0x04001D87 RID: 7559
		Offline,
		/// <summary>The system is online.</summary>
		// Token: 0x04001D88 RID: 7560
		Online,
		/// <summary>The power status of the system is unknown.</summary>
		// Token: 0x04001D89 RID: 7561
		Unknown = 255
	}
}
