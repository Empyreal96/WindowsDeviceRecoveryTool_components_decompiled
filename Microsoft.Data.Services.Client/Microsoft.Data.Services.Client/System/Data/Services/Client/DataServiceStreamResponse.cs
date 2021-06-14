using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x020000FF RID: 255
	public sealed class DataServiceStreamResponse : IDisposable
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x000230CA File Offset: 0x000212CA
		internal DataServiceStreamResponse(IODataResponseMessage response)
		{
			this.responseMessage = response;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000230D9 File Offset: 0x000212D9
		public string ContentType
		{
			get
			{
				this.CheckDisposed();
				return this.responseMessage.GetHeader("Content-Type");
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x000230F1 File Offset: 0x000212F1
		public string ContentDisposition
		{
			get
			{
				this.CheckDisposed();
				return this.responseMessage.GetHeader("Content-Disposition");
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00023109 File Offset: 0x00021309
		public Dictionary<string, string> Headers
		{
			get
			{
				this.CheckDisposed();
				if (this.headers == null)
				{
					this.headers = (Dictionary<string, string>)new HeaderCollection(this.responseMessage).UnderlyingDictionary;
				}
				return this.headers;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0002313A File Offset: 0x0002133A
		public Stream Stream
		{
			get
			{
				this.CheckDisposed();
				if (this.responseStream == null)
				{
					this.responseStream = this.responseMessage.GetStream();
				}
				return this.responseStream;
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00023161 File Offset: 0x00021361
		public void Dispose()
		{
			WebUtil.DisposeMessage(this.responseMessage);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002316E File Offset: 0x0002136E
		private void CheckDisposed()
		{
			if (this.responseMessage == null)
			{
				Error.ThrowObjectDisposed(base.GetType());
			}
		}

		// Token: 0x040004EB RID: 1259
		private IODataResponseMessage responseMessage;

		// Token: 0x040004EC RID: 1260
		private Dictionary<string, string> headers;

		// Token: 0x040004ED RID: 1261
		private Stream responseStream;
	}
}
