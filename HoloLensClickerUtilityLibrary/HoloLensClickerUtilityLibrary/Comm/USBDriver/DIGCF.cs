using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200002E RID: 46
	[Flags]
	internal enum DIGCF
	{
		// Token: 0x04000133 RID: 307
		Default = 1,
		// Token: 0x04000134 RID: 308
		Present = 2,
		// Token: 0x04000135 RID: 309
		AllClasses = 4,
		// Token: 0x04000136 RID: 310
		Profile = 8,
		// Token: 0x04000137 RID: 311
		DeviceInterface = 16
	}
}
