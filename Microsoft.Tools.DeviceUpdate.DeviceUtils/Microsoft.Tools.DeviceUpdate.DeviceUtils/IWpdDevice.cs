using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000012 RID: 18
	public interface IWpdDevice : IUpdateableDevice, IDevicePropertyCollection, IDisposable
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B0 RID: 176
		[DeviceProperty("deviceId")]
		string DeviceId { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B1 RID: 177
		[DeviceProperty("model")]
		string Model { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B2 RID: 178
		[DeviceProperty("friendlyName")]
		string FriendlyName { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B3 RID: 179
		[DeviceProperty("isLocked")]
		bool IsLocked { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B4 RID: 180
		[DeviceProperty("isMtpSessionUnlocked")]
		bool IsMtpSessionUnlocked { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B5 RID: 181
		[DeviceProperty("branch")]
		string Branch { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B6 RID: 182
		[DeviceProperty("windowsPhoneBuildNumber")]
		string WindowsPhoneBuildNumber { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B7 RID: 183
		[DeviceProperty("coreSysBuildNumber")]
		string CoreSysBuildNumber { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B8 RID: 184
		[DeviceProperty("buildTimeStamp")]
		string BuildTimeStamp { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B9 RID: 185
		[DeviceProperty("imageTargetingType")]
		string ImageTargetingType { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BA RID: 186
		[DeviceProperty("feedbackId")]
		string FeedbackId { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BB RID: 187
		[DeviceProperty("osVersion")]
		string OsVersion { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000BC RID: 188
		[DeviceProperty("firmwareVersion")]
		string FirmwareVersion { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000BD RID: 189
		[DeviceProperty("moId")]
		string MoId { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BE RID: 190
		[DeviceProperty("serialNumber")]
		string SerialNumber { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BF RID: 191
		[DeviceProperty("manufacturer")]
		string Manufacturer { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C0 RID: 192
		[DeviceProperty("oem")]
		string Oem { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C1 RID: 193
		[DeviceProperty("oemDeviceName")]
		string OemDeviceName { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C2 RID: 194
		[DeviceProperty("resolution")]
		string Resolution { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C3 RID: 195
		[DeviceProperty("uefiName")]
		string UefiName { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C4 RID: 196
		[DeviceProperty("duEngineState")]
		string DuEngineState { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C5 RID: 197
		[DeviceProperty("duResult")]
		string DuResult { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C6 RID: 198
		[DeviceProperty("uniqueID")]
		string UniqueID { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C7 RID: 199
		[DeviceProperty("duDeviceUpdateResult")]
		string DuDeviceUpdateResult { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C8 RID: 200
		[DeviceProperty("duShellStartReady")]
		string DuShellStartReady { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C9 RID: 201
		[DeviceProperty("wpSerialNumber")]
		string WPSerialNumber { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CA RID: 202
		[DeviceProperty("duShellApiReady")]
		string DuShellApiReady { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CB RID: 203
		[DeviceProperty("duTotalStorage")]
		string TotalStorage { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CC RID: 204
		[DeviceProperty("duTotalRAM")]
		string TotalRAM { get; }
	}
}
