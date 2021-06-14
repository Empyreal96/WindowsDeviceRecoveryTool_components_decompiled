using System;
using System.Net;

namespace System.Data.Services.Client
{
	// Token: 0x02000120 RID: 288
	public class SendingRequestEventArgs : EventArgs
	{
		// Token: 0x0600098C RID: 2444 RVA: 0x00026C99 File Offset: 0x00024E99
		internal SendingRequestEventArgs(WebRequest request, WebHeaderCollection requestHeaders)
		{
			this.request = request;
			this.requestHeaders = requestHeaders;
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x00026CAF File Offset: 0x00024EAF
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x00026CB7 File Offset: 0x00024EB7
		public WebRequest Request
		{
			get
			{
				return this.request;
			}
			set
			{
				Util.CheckArgumentNull<WebRequest>(value, "value");
				if (!(value is HttpWebRequest))
				{
					throw Error.Argument(Strings.Context_SendingRequestEventArgsNotHttp, "value");
				}
				this.request = value;
				this.requestHeaders = value.Headers;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x00026CF0 File Offset: 0x00024EF0
		public WebHeaderCollection RequestHeaders
		{
			get
			{
				return this.requestHeaders;
			}
		}

		// Token: 0x04000590 RID: 1424
		private WebRequest request;

		// Token: 0x04000591 RID: 1425
		private WebHeaderCollection requestHeaders;
	}
}
