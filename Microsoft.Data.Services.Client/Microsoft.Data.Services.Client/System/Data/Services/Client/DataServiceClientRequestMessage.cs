using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004B RID: 75
	public abstract class DataServiceClientRequestMessage : IODataRequestMessage
	{
		// Token: 0x06000260 RID: 608 RVA: 0x0000C84B File Offset: 0x0000AA4B
		public DataServiceClientRequestMessage(string actualMethod)
		{
			this.actualHttpMethod = actualMethod;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000261 RID: 609
		public abstract IEnumerable<KeyValuePair<string, string>> Headers { get; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000262 RID: 610
		// (set) Token: 0x06000263 RID: 611
		public abstract Uri Url { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000264 RID: 612
		// (set) Token: 0x06000265 RID: 613
		public abstract string Method { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000266 RID: 614
		// (set) Token: 0x06000267 RID: 615
		public abstract ICredentials Credentials { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000268 RID: 616
		// (set) Token: 0x06000269 RID: 617
		public abstract int Timeout { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600026A RID: 618
		// (set) Token: 0x0600026B RID: 619
		public abstract bool SendChunked { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000C85A File Offset: 0x0000AA5A
		protected virtual string ActualMethod
		{
			get
			{
				return this.actualHttpMethod;
			}
		}

		// Token: 0x0600026D RID: 621
		public abstract string GetHeader(string headerName);

		// Token: 0x0600026E RID: 622
		public abstract void SetHeader(string headerName, string headerValue);

		// Token: 0x0600026F RID: 623
		public abstract Stream GetStream();

		// Token: 0x06000270 RID: 624
		public abstract void Abort();

		// Token: 0x06000271 RID: 625
		public abstract IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state);

		// Token: 0x06000272 RID: 626
		public abstract Stream EndGetRequestStream(IAsyncResult asyncResult);

		// Token: 0x06000273 RID: 627
		public abstract IAsyncResult BeginGetResponse(AsyncCallback callback, object state);

		// Token: 0x06000274 RID: 628
		public abstract IODataResponseMessage EndGetResponse(IAsyncResult asyncResult);

		// Token: 0x06000275 RID: 629
		public abstract IODataResponseMessage GetResponse();

		// Token: 0x04000242 RID: 578
		private readonly string actualHttpMethod;
	}
}
