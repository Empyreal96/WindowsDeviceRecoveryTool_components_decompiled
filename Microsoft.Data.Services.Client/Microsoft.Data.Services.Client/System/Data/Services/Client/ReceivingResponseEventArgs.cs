using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000084 RID: 132
	public class ReceivingResponseEventArgs : EventArgs
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x00013486 File Offset: 0x00011686
		public ReceivingResponseEventArgs(IODataResponseMessage responseMessage, Descriptor descriptor) : this(responseMessage, descriptor, false)
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00013491 File Offset: 0x00011691
		public ReceivingResponseEventArgs(IODataResponseMessage responseMessage, Descriptor descriptor, bool isBatchPart)
		{
			this.ResponseMessage = responseMessage;
			this.Descriptor = descriptor;
			this.IsBatchPart = isBatchPart;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x000134AE File Offset: 0x000116AE
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x000134B6 File Offset: 0x000116B6
		public IODataResponseMessage ResponseMessage { get; private set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x000134BF File Offset: 0x000116BF
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x000134C7 File Offset: 0x000116C7
		public bool IsBatchPart { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x000134D0 File Offset: 0x000116D0
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x000134D8 File Offset: 0x000116D8
		public Descriptor Descriptor { get; private set; }
	}
}
