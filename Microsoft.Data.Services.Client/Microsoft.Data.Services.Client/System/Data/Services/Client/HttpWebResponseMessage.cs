using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200008C RID: 140
	public class HttpWebResponseMessage : IODataResponseMessage, IDisposable
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x000144F0 File Offset: 0x000126F0
		public HttpWebResponseMessage(IDictionary<string, string> headers, int statusCode, Func<Stream> getResponseStream)
		{
			this.headers = new HeaderCollection(headers);
			this.statusCode = statusCode;
			this.getResponseStream = getResponseStream;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00014514 File Offset: 0x00012714
		public HttpWebResponseMessage(HttpWebResponse httpResponse)
		{
			Util.CheckArgumentNull<HttpWebResponse>(httpResponse, "httpResponse");
			this.headers = new HeaderCollection(httpResponse.Headers);
			this.statusCode = (int)httpResponse.StatusCode;
			this.getResponseStream = new Func<Stream>(httpResponse.GetResponseStream);
			this.httpWebResponse = httpResponse;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001456A File Offset: 0x0001276A
		internal HttpWebResponseMessage(HeaderCollection headers, int statusCode, Func<Stream> getResponseStream)
		{
			this.headers = headers;
			this.statusCode = statusCode;
			this.getResponseStream = getResponseStream;
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00014587 File Offset: 0x00012787
		public virtual IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				return this.headers.AsEnumerable();
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00014594 File Offset: 0x00012794
		public HttpWebResponse Response
		{
			get
			{
				return this.httpWebResponse;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001459C File Offset: 0x0001279C
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x000145A4 File Offset: 0x000127A4
		public virtual int StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000145AC File Offset: 0x000127AC
		public virtual string GetHeader(string headerName)
		{
			Util.CheckArgumentNullAndEmpty(headerName, "headerName");
			string result;
			if (this.headers.TryGetHeader(headerName, out result))
			{
				return result;
			}
			if (string.Equals(headerName, "Content-Length", StringComparison.Ordinal))
			{
				return "-1";
			}
			return null;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000145EB File Offset: 0x000127EB
		public virtual void SetHeader(string headerName, string headerValue)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000145F2 File Offset: 0x000127F2
		public virtual Stream GetStream()
		{
			return this.getResponseStream();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000145FF File Offset: 0x000127FF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00014610 File Offset: 0x00012810
		protected virtual void Dispose(bool disposing)
		{
			HttpWebResponse httpWebResponse = this.httpWebResponse;
			this.httpWebResponse = null;
			if (httpWebResponse != null)
			{
				((IDisposable)httpWebResponse).Dispose();
			}
		}

		// Token: 0x040002FF RID: 767
		private readonly HeaderCollection headers;

		// Token: 0x04000300 RID: 768
		private readonly Func<Stream> getResponseStream;

		// Token: 0x04000301 RID: 769
		private readonly int statusCode;

		// Token: 0x04000302 RID: 770
		private HttpWebResponse httpWebResponse;
	}
}
