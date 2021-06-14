using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the boot mode in which the system was started.</summary>
	// Token: 0x0200012D RID: 301
	public enum BootMode
	{
		/// <summary>The computer was started in the standard boot mode. This mode uses the normal drivers and settings for the system.</summary>
		// Token: 0x04000651 RID: 1617
		Normal,
		/// <summary>The computer was started in safe mode without network support. This mode uses a limited drivers and settings profile.</summary>
		// Token: 0x04000652 RID: 1618
		FailSafe,
		/// <summary>The computer was started in safe mode with network support. This mode uses a limited drivers and settings profile, and loads the services needed to start networking.</summary>
		// Token: 0x04000653 RID: 1619
		FailSafeWithNetwork
	}
}
