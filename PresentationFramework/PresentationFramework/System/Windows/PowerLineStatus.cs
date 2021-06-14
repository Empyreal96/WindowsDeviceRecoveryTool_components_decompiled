using System;

namespace System.Windows
{
	/// <summary>Indicates whether the system power is online, or that the system power status is unknown.</summary>
	// Token: 0x02000110 RID: 272
	public enum PowerLineStatus
	{
		/// <summary>The system power is not on.</summary>
		// Token: 0x04000899 RID: 2201
		Offline,
		/// <summary>The system power is on.</summary>
		// Token: 0x0400089A RID: 2202
		Online,
		/// <summary>The status of the system power cannot be determined.</summary>
		// Token: 0x0400089B RID: 2203
		Unknown = 255
	}
}
