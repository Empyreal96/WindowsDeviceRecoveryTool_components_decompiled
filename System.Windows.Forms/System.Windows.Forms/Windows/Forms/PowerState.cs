using System;

namespace System.Windows.Forms
{
	/// <summary>Defines identifiers that indicate a suspended system power activity mode.</summary>
	// Token: 0x02000310 RID: 784
	public enum PowerState
	{
		/// <summary>Indicates a system suspended power mode. When a system is suspended, the computer switches to a low-power state called standby. When a computer is in standby mode, some devices are turned off and the computer uses less power. The system can restore itself more quickly than returning from hibernation. Because standby does not save the memory state to disk, a power failure while in standby can cause loss of information.</summary>
		// Token: 0x04001D92 RID: 7570
		Suspend,
		/// <summary>Indicates a system hibernation power mode. When a system enters hibernation, the contents of its memory are saved to disk before the computer is turned off. When the system is restarted, the desktop and previously active programs are restored.</summary>
		// Token: 0x04001D93 RID: 7571
		Hibernate
	}
}
