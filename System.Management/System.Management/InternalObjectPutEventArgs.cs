using System;

namespace System.Management
{
	// Token: 0x0200000D RID: 13
	internal class InternalObjectPutEventArgs : EventArgs
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00003E0B File Offset: 0x0000200B
		internal InternalObjectPutEventArgs(ManagementPath path)
		{
			this.path = path.Clone();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003E1F File Offset: 0x0000201F
		internal ManagementPath Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x0400007B RID: 123
		private ManagementPath path;
	}
}
