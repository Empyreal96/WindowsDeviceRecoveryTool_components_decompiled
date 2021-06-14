using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x0200002E RID: 46
	public class DeviceModeChangedEventArgs : EventArgs
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00004778 File Offset: 0x00002978
		public DeviceModeChangedEventArgs(ConnectedDevice connectedDevice, ConnectedDeviceMode oldMode, ConnectedDeviceMode newMode)
		{
			if (connectedDevice == null)
			{
				throw new ArgumentNullException("connectedDevice");
			}
			this.ConnectedDevice = connectedDevice;
			this.OldMode = oldMode;
			this.NewMode = newMode;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000047C0 File Offset: 0x000029C0
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000047D7 File Offset: 0x000029D7
		public ConnectedDevice ConnectedDevice { get; private set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000047E0 File Offset: 0x000029E0
		// (set) Token: 0x06000141 RID: 321 RVA: 0x000047F7 File Offset: 0x000029F7
		public ConnectedDeviceMode OldMode { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00004800 File Offset: 0x00002A00
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00004817 File Offset: 0x00002A17
		public ConnectedDeviceMode NewMode { get; private set; }
	}
}
