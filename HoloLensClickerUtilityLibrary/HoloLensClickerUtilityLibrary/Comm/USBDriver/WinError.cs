using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200002D RID: 45
	internal enum WinError : uint
	{
		// Token: 0x04000121 RID: 289
		Success,
		// Token: 0x04000122 RID: 290
		FileNotFound = 2U,
		// Token: 0x04000123 RID: 291
		PathNotFound,
		// Token: 0x04000124 RID: 292
		AccessDenied = 5U,
		// Token: 0x04000125 RID: 293
		NoMoreFiles = 18U,
		// Token: 0x04000126 RID: 294
		NotReady = 21U,
		// Token: 0x04000127 RID: 295
		GeneralFailure = 31U,
		// Token: 0x04000128 RID: 296
		InvalidParameter = 87U,
		// Token: 0x04000129 RID: 297
		SemaphoreTimeout = 121U,
		// Token: 0x0400012A RID: 298
		InsufficientBuffer,
		// Token: 0x0400012B RID: 299
		AlreadyExists = 183U,
		// Token: 0x0400012C RID: 300
		NoMoreItems = 259U,
		// Token: 0x0400012D RID: 301
		IoPending = 997U,
		// Token: 0x0400012E RID: 302
		DeviceNotConnected = 1167U,
		// Token: 0x0400012F RID: 303
		ErrorInWow64 = 3758096949U,
		// Token: 0x04000130 RID: 304
		TimeZoneIdInvalid = 4294967295U,
		// Token: 0x04000131 RID: 305
		InvalidHandleValue = 4294967295U
	}
}
