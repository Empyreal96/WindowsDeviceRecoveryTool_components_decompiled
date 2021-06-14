using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Data.OData;
using Microsoft.WindowsAzure.Storage.Core;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004A RID: 74
	internal class HttpWebRequestAdapterMessage : IODataRequestMessage, IDisposable
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002D504 File Offset: 0x0002B704
		public HttpWebRequestAdapterMessage(HttpWebRequest msg, IBufferManager buffManager)
		{
			this.msg = msg;
			this.outStr = new MultiBufferMemoryStream(buffManager, 65536);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0002D524 File Offset: 0x0002B724
		public HttpWebRequest GetPopulatedMessage()
		{
			this.outStr.Seek(0L, SeekOrigin.Begin);
			this.msg.ContentLength = this.outStr.Length;
			return this.msg;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002D551 File Offset: 0x0002B751
		public string GetHeader(string headerName)
		{
			if (headerName == "Content-Type")
			{
				return this.msg.ContentType;
			}
			return this.msg.Headers[headerName];
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0002D57D File Offset: 0x0002B77D
		public Stream GetStream()
		{
			return this.outStr;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0002D588 File Offset: 0x0002B788
		public IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				foreach (string text in this.msg.Headers.AllKeys)
				{
					list.Add(new KeyValuePair<string, string>(text, this.msg.Headers[text]));
				}
				return list;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0002D5DC File Offset: 0x0002B7DC
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x0002D5E9 File Offset: 0x0002B7E9
		public string Method
		{
			get
			{
				return this.msg.Method;
			}
			set
			{
				this.msg.Method = value;
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002D5F7 File Offset: 0x0002B7F7
		public void SetHeader(string headerName, string headerValue)
		{
			if (headerName == "Content-Type")
			{
				this.msg.ContentType = headerValue;
				return;
			}
			this.msg.Headers[headerName] = headerValue;
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0002D625 File Offset: 0x0002B825
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x0002D632 File Offset: 0x0002B832
		public Uri Url
		{
			get
			{
				return this.msg.RequestUri;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002D639 File Offset: 0x0002B839
		public void Dispose()
		{
			this.msg = null;
			this.outStr = null;
		}

		// Token: 0x0400018F RID: 399
		private HttpWebRequest msg;

		// Token: 0x04000190 RID: 400
		private MultiBufferMemoryStream outStr;
	}
}
