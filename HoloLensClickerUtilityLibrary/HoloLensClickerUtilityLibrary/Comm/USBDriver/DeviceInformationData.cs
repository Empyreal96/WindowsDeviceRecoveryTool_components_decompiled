using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000033 RID: 51
	internal struct DeviceInformationData
	{
		// Token: 0x0400014B RID: 331
		public int Size;

		// Token: 0x0400014C RID: 332
		public Guid ClassGuid;

		// Token: 0x0400014D RID: 333
		public int DevInst;

		// Token: 0x0400014E RID: 334
		public IntPtr Reserved;
	}
}
