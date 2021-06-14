using System;

namespace FFUComponents
{
	// Token: 0x02000019 RID: 25
	public class DisconnectEventArgs : EventArgs
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003241 File Offset: 0x00001441
		private DisconnectEventArgs()
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003249 File Offset: 0x00001449
		public DisconnectEventArgs(Guid deviceUniqueId)
		{
			this.deviceUniqueId = deviceUniqueId;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003258 File Offset: 0x00001458
		public Guid DeviceUniqueId
		{
			get
			{
				return this.deviceUniqueId;
			}
		}

		// Token: 0x04000034 RID: 52
		private Guid deviceUniqueId;
	}
}
