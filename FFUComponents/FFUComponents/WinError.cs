using System;

namespace FFUComponents
{
	// Token: 0x0200002A RID: 42
	internal enum WinError : uint
	{
		// Token: 0x04000073 RID: 115
		Success,
		// Token: 0x04000074 RID: 116
		FileNotFound = 2U,
		// Token: 0x04000075 RID: 117
		NoMoreFiles = 18U,
		// Token: 0x04000076 RID: 118
		NotReady = 21U,
		// Token: 0x04000077 RID: 119
		GeneralFailure = 31U,
		// Token: 0x04000078 RID: 120
		InvalidParameter = 87U,
		// Token: 0x04000079 RID: 121
		SemTimeout = 121U,
		// Token: 0x0400007A RID: 122
		InsufficientBuffer,
		// Token: 0x0400007B RID: 123
		WaitTimeout = 258U,
		// Token: 0x0400007C RID: 124
		OperationAborted = 995U,
		// Token: 0x0400007D RID: 125
		IoPending = 997U,
		// Token: 0x0400007E RID: 126
		DeviceNotConnected = 1167U,
		// Token: 0x0400007F RID: 127
		TimeZoneIdInvalid = 4294967295U,
		// Token: 0x04000080 RID: 128
		InvalidHandleValue = 4294967295U,
		// Token: 0x04000081 RID: 129
		PathNotFound = 3U,
		// Token: 0x04000082 RID: 130
		AlreadyExists = 183U,
		// Token: 0x04000083 RID: 131
		NoMoreItems = 259U
	}
}
