using System;

namespace Nokia.Lucid.DeviceDetection
{
	// Token: 0x0200000C RID: 12
	public sealed class DeviceChangedEventArgs : EventArgs
	{
		// Token: 0x06000028 RID: 40 RVA: 0x0000309C File Offset: 0x0000129C
		public DeviceChangedEventArgs(DeviceChangeAction action, string path, DeviceType deviceType)
		{
			this.action = action;
			this.path = path;
			this.deviceType = deviceType;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000030B9 File Offset: 0x000012B9
		public DeviceChangeAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000030C1 File Offset: 0x000012C1
		public string Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000030C9 File Offset: 0x000012C9
		public DeviceType DeviceType
		{
			get
			{
				return this.deviceType;
			}
		}

		// Token: 0x0400001A RID: 26
		private readonly DeviceChangeAction action;

		// Token: 0x0400001B RID: 27
		private readonly string path;

		// Token: 0x0400001C RID: 28
		private readonly DeviceType deviceType;
	}
}
