using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Data.OData;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x02000049 RID: 73
	internal class HttpResponseAdapterMessage : IODataResponseMessage
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002D3B9 File Offset: 0x0002B5B9
		public HttpResponseAdapterMessage(HttpWebResponse resp, Stream str) : this(resp, str, null)
		{
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002D3C4 File Offset: 0x0002B5C4
		public HttpResponseAdapterMessage(HttpWebResponse resp, Stream str, string responseContentType)
		{
			this.resp = resp;
			this.str = str;
			this.responseContentType = responseContentType;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002D3E9 File Offset: 0x0002B5E9
		public Task<Stream> GetStreamAsync()
		{
			return Task.Factory.StartNew<Stream>(() => this.str);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0002D404 File Offset: 0x0002B604
		public string GetHeader(string headerName)
		{
			if (headerName == "Content-Type")
			{
				if (this.responseContentType != null)
				{
					return this.responseContentType;
				}
				return this.resp.ContentType;
			}
			else
			{
				if (headerName == "Content-Encoding")
				{
					return this.resp.ContentEncoding;
				}
				return this.resp.Headers[headerName];
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0002D463 File Offset: 0x0002B663
		public Stream GetStream()
		{
			return this.str;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0002D46C File Offset: 0x0002B66C
		public IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				foreach (string text in this.resp.Headers.AllKeys)
				{
					if (text == "Content-Type" && this.responseContentType != null)
					{
						list.Add(new KeyValuePair<string, string>(text, this.responseContentType));
					}
					else
					{
						list.Add(new KeyValuePair<string, string>(text, this.resp.Headers[text]));
					}
				}
				return list;
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0002D4E9 File Offset: 0x0002B6E9
		public void SetHeader(string headerName, string headerValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x0002D4FD File Offset: 0x0002B6FD
		public int StatusCode
		{
			get
			{
				return (int)this.resp.StatusCode;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0400018C RID: 396
		private HttpWebResponse resp;

		// Token: 0x0400018D RID: 397
		private Stream str;

		// Token: 0x0400018E RID: 398
		private string responseContentType;
	}
}
