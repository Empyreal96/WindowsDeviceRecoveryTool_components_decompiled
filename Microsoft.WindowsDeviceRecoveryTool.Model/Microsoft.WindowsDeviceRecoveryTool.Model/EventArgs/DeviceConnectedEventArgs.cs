using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x0200002D RID: 45
	public class DeviceConnectedEventArgs : EventArgs
	{
		// Token: 0x0600013A RID: 314 RVA: 0x00004720 File Offset: 0x00002920
		public DeviceConnectedEventArgs(ConnectedDevice connectedDevice)
		{
			if (connectedDevice == null)
			{
				throw new ArgumentNullException("connectedDevice");
			}
			this.ConnectedDevice = connectedDevice;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00004758 File Offset: 0x00002958
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000476F File Offset: 0x0000296F
		public ConnectedDevice ConnectedDevice { get; private set; }
	}
}
