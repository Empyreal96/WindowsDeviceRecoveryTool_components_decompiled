using System;

namespace FFUComponents
{
	// Token: 0x02000024 RID: 36
	[Flags]
	internal enum UsbRequest
	{
		// Token: 0x0400004E RID: 78
		DeviceToHost = 128,
		// Token: 0x0400004F RID: 79
		HostToDevice = 0,
		// Token: 0x04000050 RID: 80
		Standard = 0,
		// Token: 0x04000051 RID: 81
		Class = 32,
		// Token: 0x04000052 RID: 82
		Vendor = 64,
		// Token: 0x04000053 RID: 83
		Reserved = 96,
		// Token: 0x04000054 RID: 84
		ForDevice = 0,
		// Token: 0x04000055 RID: 85
		ForInterface = 1,
		// Token: 0x04000056 RID: 86
		ForEndpoint = 2,
		// Token: 0x04000057 RID: 87
		ForOther = 3
	}
}
