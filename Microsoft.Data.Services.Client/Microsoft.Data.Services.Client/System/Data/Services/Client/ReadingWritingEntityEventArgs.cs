using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x0200010F RID: 271
	public sealed class ReadingWritingEntityEventArgs : EventArgs
	{
		// Token: 0x060008CE RID: 2254 RVA: 0x0002474B File Offset: 0x0002294B
		internal ReadingWritingEntityEventArgs(object entity, XElement data, Uri baseUri)
		{
			this.entity = entity;
			this.data = data;
			this.baseUri = baseUri;
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00024768 File Offset: 0x00022968
		public object Entity
		{
			get
			{
				return this.entity;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00024770 File Offset: 0x00022970
		public XElement Data
		{
			[DebuggerStepThrough]
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00024778 File Offset: 0x00022978
		public Uri BaseUri
		{
			[DebuggerStepThrough]
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x04000515 RID: 1301
		private object entity;

		// Token: 0x04000516 RID: 1302
		private XElement data;

		// Token: 0x04000517 RID: 1303
		private Uri baseUri;
	}
}
