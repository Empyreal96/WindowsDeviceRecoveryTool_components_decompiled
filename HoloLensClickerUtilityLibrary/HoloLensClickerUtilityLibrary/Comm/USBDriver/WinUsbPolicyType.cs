using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000028 RID: 40
	internal enum WinUsbPolicyType : uint
	{
		// Token: 0x04000106 RID: 262
		ShortPacketTerminate = 1U,
		// Token: 0x04000107 RID: 263
		AutoClearStall,
		// Token: 0x04000108 RID: 264
		PipeTransferTimeout,
		// Token: 0x04000109 RID: 265
		IgnoreShortPackets,
		// Token: 0x0400010A RID: 266
		AllowPartialReads,
		// Token: 0x0400010B RID: 267
		AutoFlush,
		// Token: 0x0400010C RID: 268
		RawIO,
		// Token: 0x0400010D RID: 269
		MaximumTransferSize,
		// Token: 0x0400010E RID: 270
		ResetPipeOnResume
	}
}
