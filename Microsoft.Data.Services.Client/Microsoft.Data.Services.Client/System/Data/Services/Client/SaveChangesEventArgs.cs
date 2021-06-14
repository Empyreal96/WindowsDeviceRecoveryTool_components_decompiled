using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000F0 RID: 240
	internal class SaveChangesEventArgs : EventArgs
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x00022301 File Offset: 0x00020501
		public SaveChangesEventArgs(DataServiceResponse response)
		{
			this.response = response;
		}

		// Token: 0x040004C4 RID: 1220
		private DataServiceResponse response;
	}
}
