using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000089 RID: 137
	public class SendingRequest2EventArgs : EventArgs
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00013CD7 File Offset: 0x00011ED7
		internal SendingRequest2EventArgs(IODataRequestMessage requestMessage, Descriptor descriptor, bool isBatchPart)
		{
			this.RequestMessage = requestMessage;
			this.Descriptor = descriptor;
			this.IsBatchPart = isBatchPart;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00013CF4 File Offset: 0x00011EF4
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00013CFC File Offset: 0x00011EFC
		public IODataRequestMessage RequestMessage { get; private set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00013D05 File Offset: 0x00011F05
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00013D0D File Offset: 0x00011F0D
		public Descriptor Descriptor { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00013D16 File Offset: 0x00011F16
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00013D1E File Offset: 0x00011F1E
		public bool IsBatchPart { get; private set; }
	}
}
