using System;

namespace FFUComponents
{
	// Token: 0x02000018 RID: 24
	public class ConnectEventArgs : EventArgs
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003222 File Offset: 0x00001422
		private ConnectEventArgs()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000322A File Offset: 0x0000142A
		public ConnectEventArgs(IFFUDevice device)
		{
			this.device = device;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003239 File Offset: 0x00001439
		public IFFUDevice Device
		{
			get
			{
				return this.device;
			}
		}

		// Token: 0x04000033 RID: 51
		private IFFUDevice device;
	}
}
