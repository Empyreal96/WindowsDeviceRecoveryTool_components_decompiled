using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000026 RID: 38
	[Flags]
	internal enum UsbRequest
	{
		// Token: 0x040000F6 RID: 246
		DeviceToHost = 128,
		// Token: 0x040000F7 RID: 247
		HostToDevice = 0,
		// Token: 0x040000F8 RID: 248
		Standard = 0,
		// Token: 0x040000F9 RID: 249
		Class = 32,
		// Token: 0x040000FA RID: 250
		Vendor = 64,
		// Token: 0x040000FB RID: 251
		Reserved = 96,
		// Token: 0x040000FC RID: 252
		ForDevice = 0,
		// Token: 0x040000FD RID: 253
		ForInterface = 1,
		// Token: 0x040000FE RID: 254
		ForEndpoint = 2,
		// Token: 0x040000FF RID: 255
		ForOther = 3
	}
}
