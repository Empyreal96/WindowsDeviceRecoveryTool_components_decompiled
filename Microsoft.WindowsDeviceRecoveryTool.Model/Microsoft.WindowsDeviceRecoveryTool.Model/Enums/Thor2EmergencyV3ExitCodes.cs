using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Enums
{
	// Token: 0x0200000F RID: 15
	public enum Thor2EmergencyV3ExitCodes : uint
	{
		// Token: 0x04000049 RID: 73
		EmergencyflashErrorUnknownSaharaState = 80996U,
		// Token: 0x0400004A RID: 74
		EmergencyflashErrorInvalidMessagingInterface,
		// Token: 0x0400004B RID: 75
		EmergencyflashErrorGeneralError,
		// Token: 0x0400004C RID: 76
		EmergencyflashErrorNotImplemented,
		// Token: 0x0400004D RID: 77
		EmergencyflashErrorXmlParsingFailed = 85000U,
		// Token: 0x0400004E RID: 78
		EmergencyflashErrorImageFileOpenFailed,
		// Token: 0x0400004F RID: 79
		EmergencyflashErrorImageFileReadFailed,
		// Token: 0x04000050 RID: 80
		EmergencyflashErrorImageFilesMissing,
		// Token: 0x04000051 RID: 81
		EmergencyflashErrorEdimageParsingFailed,
		// Token: 0x04000052 RID: 82
		EmergencyflashErrorMsgUnexpectedResponse = 85020U,
		// Token: 0x04000053 RID: 83
		EmergencyflashErrorMsgSendReceiveFailed,
		// Token: 0x04000054 RID: 84
		EmergencyflashErrorMsgNakReceived,
		// Token: 0x04000055 RID: 85
		EmergencyflashErrorProgrammerSendFailed = 85030U,
		// Token: 0x04000056 RID: 86
		EmergencyflashErrorNoBootImages,
		// Token: 0x04000057 RID: 87
		EmergencyflashErrorSafeHexViolation,
		// Token: 0x04000058 RID: 88
		EmergencyflashErrorHwInitFailed,
		// Token: 0x04000059 RID: 89
		EmergencyflashErrorEmmcInitFailed = 85035U,
		// Token: 0x0400005A RID: 90
		EmergencyflashErrorEmmcWriteFailed,
		// Token: 0x0400005B RID: 91
		EmergencyflashErrorEmmcEraseFailed,
		// Token: 0x0400005C RID: 92
		EmergencyflashErrorEmmcReadFailed,
		// Token: 0x0400005D RID: 93
		EmergencyflashErrorSignatureAuthFailed,
		// Token: 0x0400005E RID: 94
		EmergencyflashErrorFfuParsingFailed,
		// Token: 0x0400005F RID: 95
		EmergencyflashErrorFfuOpenFailed,
		// Token: 0x04000060 RID: 96
		EmergencyflashErrorFfuPartitionSizeMismatch,
		// Token: 0x04000061 RID: 97
		EmergencyflashErrorFfuGptFailure,
		// Token: 0x04000062 RID: 98
		EmergencyflashErrorFfuPartitionNotFound,
		// Token: 0x04000063 RID: 99
		EmergencyflashErrorReadbackVerifyFailed = 85050U,
		// Token: 0x04000064 RID: 100
		EmergencyflashErrorSdramTestFailed
	}
}
