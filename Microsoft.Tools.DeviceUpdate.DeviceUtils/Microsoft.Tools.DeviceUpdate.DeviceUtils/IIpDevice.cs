using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000009 RID: 9
	public interface IIpDevice : IUpdateableDevice, IDevicePropertyCollection, IDisposable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000032 RID: 50
		[DeviceProperty("deviceUniqueId")]
		Guid DeviceUniqueId { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000033 RID: 51
		[DeviceProperty("model")]
		string Model { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000034 RID: 52
		[DeviceProperty("branch")]
		string Branch { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000035 RID: 53
		[DeviceProperty("coreSysBuildNumber")]
		string CoreSysBuildNumber { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000036 RID: 54
		[DeviceProperty("coreSysBuildRevision")]
		string CoreSysBuildRevision { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000037 RID: 55
		[DeviceProperty("buildTimeStamp")]
		string BuildTimeStamp { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000038 RID: 56
		[DeviceProperty("imageTargetingType")]
		string ImageTargetingType { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000039 RID: 57
		[DeviceProperty("feedbackId")]
		string FeedbackId { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003A RID: 58
		[DeviceProperty("firmwareVersion")]
		string FirmwareVersion { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003B RID: 59
		[DeviceProperty("serialNumber")]
		string SerialNumber { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60
		[DeviceProperty("manufacturer")]
		string Manufacturer { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003D RID: 61
		[DeviceProperty("oemDeviceName")]
		string OemDeviceName { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003E RID: 62
		[DeviceProperty("uefiName")]
		string UefiName { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003F RID: 63
		[DeviceProperty("updateState")]
		string UpdateState { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000040 RID: 64
		[DeviceProperty("duResult")]
		string DuResult { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000041 RID: 65
		[DeviceProperty("batteryLevel")]
		string BatteryLevel { get; }

		// Token: 0x06000042 RID: 66
		void SetTime(DateTime time);
	}
}
