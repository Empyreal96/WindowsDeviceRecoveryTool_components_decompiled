using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000E RID: 14
	public enum UriData
	{
		// Token: 0x04000029 RID: 41
		[UriDescription("Software is not correctly signed")]
		SoftwareNotCorrectlySigned = 105601,
		// Token: 0x0400002A RID: 42
		[UriDescription("Reset protection status is incorrect")]
		ResetProtectionStatusIsIncorrect,
		// Token: 0x0400002B RID: 43
		[UriDescription("Emergency flashing failed")]
		EmergencyFlashingFailed,
		// Token: 0x0400002C RID: 44
		[UriDescription("Emergency succesfully finished")]
		EmergencyFlashingSuccesfullyFinished,
		// Token: 0x0400002D RID: 45
		[UriDescription("Recovery after emergency flashing failed")]
		RecoveryAfterEmergencyFlashingFailed,
		// Token: 0x0400002E RID: 46
		[UriDescription("Dead phone recovered after emergency flashing")]
		DeadPhoneRecoveredAfterEmergencyFlashing,
		// Token: 0x0400002F RID: 47
		[UriDescription("Reading device info after emergency flashing failed")]
		ReadingDeviceInfoAfterEmergencyFlashingFailed,
		// Token: 0x04000030 RID: 48
		[UriDescription("Uefi mode appeared after emergency flashing")]
		UefiModeAfterEmergencyFlashing,
		// Token: 0x04000031 RID: 49
		[UriDescription("Emergency mode appeared after emergency flashing")]
		EmergencyModeAfterEmergencyFlashing,
		// Token: 0x04000032 RID: 50
		[UriDescription("Await after emergency flashing canceled")]
		AwaitAfterEmergencyFlashingCanceled,
		// Token: 0x04000033 RID: 51
		[UriDescription("Emergency flash files not found on server")]
		EmergencyFlashFilesNotFoundOnServer,
		// Token: 0x04000034 RID: 52
		[UriDescription("Change to flash mode failed")]
		ChangeToFlashModeFailed = 105614,
		// Token: 0x04000035 RID: 53
		[UriDescription("Download package success")]
		DownloadPackageSuccess,
		// Token: 0x04000036 RID: 54
		[UriDescription("Download Generic package aborted by user")]
		DownloadGenericPackageAbortedByUser = 105618,
		// Token: 0x04000037 RID: 55
		[UriDescription("Download Generic package failed")]
		DownloadGenericPackageFailed,
		// Token: 0x04000038 RID: 56
		[UriDescription("Invalid Code")]
		InvalidUriCode = 0,
		// Token: 0x04000039 RID: 57
		[UriDescription("Failed to download variant package. FIRE service break")]
		FailedToDownloadVariantPackageFireServiceBreak = 323,
		// Token: 0x0400003A RID: 58
		[UriDescription("Failed to download variant package")]
		FailedToDownloadVariantPackage = 7,
		// Token: 0x0400003B RID: 59
		[UriDescription("Cannot download file, CRC32 checksum does not match")]
		FailedToDownloadVariantPackageCrc32Failed = 21027,
		// Token: 0x0400003C RID: 60
		[UriDescription("Add variant packages location to product api search path fails")]
		AddVariantPackagesLocationToProductApiSearchPathFails = 9,
		// Token: 0x0400003D RID: 61
		[UriDescription("Can not set flash programmer search path")]
		CanNotSetFlashProgrammerSearchPath = 20,
		// Token: 0x0400003E RID: 62
		[UriDescription("Product code read failed")]
		ProductCodeReadFailed = 22,
		// Token: 0x0400003F RID: 63
		[UriDescription("Download variant package aborted by user")]
		DownloadVariantPackageAbortedByUser = 24,
		// Token: 0x04000040 RID: 64
		[UriDescription("Firmware update start failed")]
		FirmwareUpdateStartFailed = 35,
		// Token: 0x04000041 RID: 65
		[UriDescription("Programming phone failed")]
		ProgrammingPhoneFailed = 44,
		// Token: 0x04000042 RID: 66
		[UriDescription("Finalizing the programming failed")]
		FinalizingTheProgrammingFailed,
		// Token: 0x04000043 RID: 67
		[UriDescription("Firmware update successful")]
		FirmwareUpdateSuccessful,
		// Token: 0x04000044 RID: 68
		[UriDescription("Device support is blocked")]
		DeviceSupportIsBlocked = 57,
		// Token: 0x04000045 RID: 69
		[UriDescription("Firmware update successful with Retry")]
		FirmwareUpdateSuccessfulWithRetry = 67,
		// Token: 0x04000046 RID: 70
		[UriDescription("Could not read product type")]
		CouldNotReadProductType = 117,
		// Token: 0x04000047 RID: 71
		[UriDescription("Download variant package files failed because of insufficient disk space")]
		DownloadVariantPackageFilesFailedBecauseOfInsufficientDiskSpace = 154,
		// Token: 0x04000048 RID: 72
		[UriDescription("Battery low, please recharge")]
		DeviceBatteryLow = 311,
		// Token: 0x04000049 RID: 73
		[UriDescription("Can not downgrade software")]
		CanNotDowngradeSoftware,
		// Token: 0x0400004A RID: 74
		[UriDescription("Recovery flashing aborted by user")]
		RecoveryFlashingAbortedByUser = 321,
		// Token: 0x0400004B RID: 75
		[UriDescription("Recovery flashing failed")]
		RecoveryFlashingFailed,
		// Token: 0x0400004C RID: 76
		[UriDescription("Automatic Driver Update disabling failed")]
		AutomaticDriverUpdateDisablingFailed = 338,
		// Token: 0x0400004D RID: 77
		[UriDescription("No package found")]
		NoPackageFound = 81602,
		// Token: 0x0400004E RID: 78
		[UriDescription("Unhandled exception")]
		UnhandledException = 210,
		// Token: 0x0400004F RID: 79
		[UriDescription("Select device failed")]
		SelectDeviceFailed,
		// Token: 0x04000050 RID: 80
		[UriDescription("Computer battery low")]
		ComputerBatteryLow,
		// Token: 0x04000051 RID: 81
		[UriDescription("Could not load dll")]
		CouldNotLoadDll,
		// Token: 0x04000052 RID: 82
		[UriDescription("Unhandled error during download preconditions check")]
		UnhandledErrorDuringDownloadPreconditionsCheck,
		// Token: 0x04000053 RID: 83
		[UriDescription("Software update aborted by user due to lack of proper memory card")]
		SoftwareUpdateAbortedByUserFromMemoryCardDialog = 220,
		// Token: 0x04000054 RID: 84
		[UriDescription("Device doesn't support secure flashing")]
		DeviceDoesntSupportSecureFlashing,
		// Token: 0x04000055 RID: 85
		[UriDescription("Changing device mode failed")]
		ChangingDeviceModeFailed,
		// Token: 0x04000056 RID: 86
		[UriDescription("Configuration file not found")]
		ConfigurationFileNotFound,
		// Token: 0x04000057 RID: 87
		[UriDescription("Dead phone detection started")]
		DeadPhoneDetectionStarted = 230,
		// Token: 0x04000058 RID: 88
		[UriDescription("Dead phone information read")]
		DeadPhoneInformationRead,
		// Token: 0x04000059 RID: 89
		[UriDescription("Dead phone detection aborted by user")]
		DeadPhoneDetectionAbortedByUser,
		// Token: 0x0400005A RID: 90
		[UriDescription("Dead phone detection failed")]
		DeadPhoneDetectionFailed,
		// Token: 0x0400005B RID: 91
		[UriDescription("Dead phone recovery started")]
		DeadPhoneRecoveryStarted,
		// Token: 0x0400005C RID: 92
		[UriDescription("Dead phone recovered")]
		DeadPhoneRecovered,
		// Token: 0x0400005D RID: 93
		[UriDescription("Dead phone recovery failed")]
		DeadPhoneRecoveryFailed
	}
}
