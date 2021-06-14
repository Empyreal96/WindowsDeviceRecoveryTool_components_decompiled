using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000007 RID: 7
	public class ConnectedDevice
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public ConnectedDevice(string portId, string vid, string pid, ConnectedDeviceMode mode, bool deviceIsConnected, string typeDesignator, string salesName, string path, string instanceId)
		{
			this.PortId = portId;
			this.Vid = vid;
			this.Pid = pid;
			this.Mode = mode;
			this.IsDeviceConnected = deviceIsConnected;
			this.SuppressConnectedDisconnectedEvents = false;
			this.TypeDesignator = typeDesignator;
			this.SalesName = salesName;
			this.DeviceReady = false;
			this.DevicePath = string.Empty;
			this.Path = path;
			this.InstanceId = instanceId;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002B34 File Offset: 0x00000D34
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002B4B File Offset: 0x00000D4B
		public string TypeDesignator { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002B54 File Offset: 0x00000D54
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002B6B File Offset: 0x00000D6B
		public string SalesName { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002B74 File Offset: 0x00000D74
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002B8B File Offset: 0x00000D8B
		public string PortId { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002B94 File Offset: 0x00000D94
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002BAB File Offset: 0x00000DAB
		public string Vid { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002BB4 File Offset: 0x00000DB4
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002BCB File Offset: 0x00000DCB
		public string Pid { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002BD4 File Offset: 0x00000DD4
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002BEB File Offset: 0x00000DEB
		public string DevicePath { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002BF4 File Offset: 0x00000DF4
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002C0B File Offset: 0x00000E0B
		public string InstanceId { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002C14 File Offset: 0x00000E14
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002C2B File Offset: 0x00000E2B
		public ConnectedDeviceMode Mode { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002C34 File Offset: 0x00000E34
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002C4B File Offset: 0x00000E4B
		public bool IsDeviceConnected { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002C54 File Offset: 0x00000E54
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002C6B File Offset: 0x00000E6B
		public bool DeviceReady { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002C74 File Offset: 0x00000E74
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002C8B File Offset: 0x00000E8B
		public string Path { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002C94 File Offset: 0x00000E94
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002CAB File Offset: 0x00000EAB
		public bool SuppressConnectedDisconnectedEvents { get; set; }
	}
}
