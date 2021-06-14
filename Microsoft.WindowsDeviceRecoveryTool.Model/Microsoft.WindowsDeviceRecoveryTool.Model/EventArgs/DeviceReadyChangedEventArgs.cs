using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x0200002F RID: 47
	public class DeviceReadyChangedEventArgs : EventArgs
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00004820 File Offset: 0x00002A20
		public DeviceReadyChangedEventArgs(ConnectedDevice device, bool deviceReady, ConnectedDeviceMode mode)
		{
			this.ConnectedDevice = device;
			this.DeviceReady = deviceReady;
			this.Mode = mode;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00004844 File Offset: 0x00002A44
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000485B File Offset: 0x00002A5B
		public ConnectedDevice ConnectedDevice { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00004864 File Offset: 0x00002A64
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000487B File Offset: 0x00002A7B
		public bool DeviceReady { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00004884 File Offset: 0x00002A84
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000489B File Offset: 0x00002A9B
		public ConnectedDeviceMode Mode { get; private set; }
	}
}
