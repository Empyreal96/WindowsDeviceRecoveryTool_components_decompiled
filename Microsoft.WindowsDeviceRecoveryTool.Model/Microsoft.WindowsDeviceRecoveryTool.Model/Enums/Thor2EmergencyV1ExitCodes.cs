using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Enums
{
	// Token: 0x02000010 RID: 16
	public enum Thor2EmergencyV1ExitCodes : uint
	{
		// Token: 0x04000066 RID: 102
		Emergencyflashv1ErrorUnknownSaharaState = 80996U,
		// Token: 0x04000067 RID: 103
		Emergencyflashv1ErrorInvalidMessagingInterface,
		// Token: 0x04000068 RID: 104
		Emergencyflashv1ErrorGeneralError,
		// Token: 0x04000069 RID: 105
		Emergencyflashv1ErrorNotImplemented,
		// Token: 0x0400006A RID: 106
		Emergencyflashv1ErrorXmlParsingFailed = 85000U,
		// Token: 0x0400006B RID: 107
		Emergencyflashv1ErrorImageFileOpenFailed,
		// Token: 0x0400006C RID: 108
		Emergencyflashv1ErrorImageFileReadFailed,
		// Token: 0x0400006D RID: 109
		Emergencyflashv1ErrorImageFilesMissing,
		// Token: 0x0400006E RID: 110
		Emergencyflashv1ErrorMsgUnexpectedResponse = 85020U,
		// Token: 0x0400006F RID: 111
		Emergencyflashv1ErrorMsgSendReceiveFailed,
		// Token: 0x04000070 RID: 112
		Emergencyflashv1ErrorProgrammerSendFailed = 85030U,
		// Token: 0x04000071 RID: 113
		Emergencyflashv1ErrorNoBootImages,
		// Token: 0x04000072 RID: 114
		Emergencyflashv1ErrorSafeHexViolation,
		// Token: 0x04000073 RID: 115
		Emergencyflashv1ErrorHexFlasherDoesNotRespond,
		// Token: 0x04000074 RID: 116
		Emergencyflashv1ErrorSafeHexAddressViolation,
		// Token: 0x04000075 RID: 117
		Emergencyflashv1ErrorFfuParsingFailed = 85040U,
		// Token: 0x04000076 RID: 118
		Emergencyflashv1ErrorFfuOpenFailed,
		// Token: 0x04000077 RID: 119
		Emergencyflashv1ErrorFfuPartitionSizeMismatch,
		// Token: 0x04000078 RID: 120
		Emergencyflashv1ErrorFfuGptFailure,
		// Token: 0x04000079 RID: 121
		Emergencyflashv1ErrorBinFileOpen,
		// Token: 0x0400007A RID: 122
		Emergencyflashv1ErrorGptNotFoundFromMbnFile,
		// Token: 0x0400007B RID: 123
		Emergencyflashv1ErrorInvalidData,
		// Token: 0x0400007C RID: 124
		Emergencyflashv1ErrorBinaryGptFailure
	}
}
