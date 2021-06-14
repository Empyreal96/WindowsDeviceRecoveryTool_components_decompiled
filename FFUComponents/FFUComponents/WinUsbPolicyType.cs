using System;

namespace FFUComponents
{
	// Token: 0x02000026 RID: 38
	internal enum WinUsbPolicyType : uint
	{
		// Token: 0x0400005E RID: 94
		ShortPacketTerminate = 1U,
		// Token: 0x0400005F RID: 95
		AutoClearStall,
		// Token: 0x04000060 RID: 96
		PipeTransferTimeout,
		// Token: 0x04000061 RID: 97
		IgnoreShortPackets,
		// Token: 0x04000062 RID: 98
		AllowPartialReads,
		// Token: 0x04000063 RID: 99
		AutoFlush,
		// Token: 0x04000064 RID: 100
		RawIO
	}
}
