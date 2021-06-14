using System;
using System.Net;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007B RID: 123
	public sealed class RequestEventArgs : EventArgs
	{
		// Token: 0x06000EBD RID: 3773 RVA: 0x00038BB3 File Offset: 0x00036DB3
		public RequestEventArgs(RequestResult res)
		{
			this.RequestInformation = res;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x00038BC2 File Offset: 0x00036DC2
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x00038BCA File Offset: 0x00036DCA
		public RequestResult RequestInformation { get; internal set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x00038BD3 File Offset: 0x00036DD3
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x00038BDB File Offset: 0x00036DDB
		public HttpWebRequest Request { get; internal set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x00038BE4 File Offset: 0x00036DE4
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x00038BEC File Offset: 0x00036DEC
		public HttpWebResponse Response { get; internal set; }
	}
}
