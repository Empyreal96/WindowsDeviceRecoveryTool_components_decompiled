using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000085 RID: 133
	internal class HeaderCollection
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x000134E1 File Offset: 0x000116E1
		internal HeaderCollection(IEnumerable<KeyValuePair<string, string>> headers) : this()
		{
			this.SetHeaders(headers);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000134F0 File Offset: 0x000116F0
		internal HeaderCollection(IODataResponseMessage responseMessage) : this()
		{
			if (responseMessage != null)
			{
				this.SetHeaders(responseMessage.Headers);
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00013508 File Offset: 0x00011708
		internal HeaderCollection(WebHeaderCollection headers) : this()
		{
			foreach (string text in headers.AllKeys)
			{
				this.SetHeader(text, headers[text]);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00013542 File Offset: 0x00011742
		internal HeaderCollection()
		{
			this.headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001355A File Offset: 0x0001175A
		internal IDictionary<string, string> UnderlyingDictionary
		{
			get
			{
				return this.headers;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00013562 File Offset: 0x00011762
		internal IEnumerable<string> HeaderNames
		{
			get
			{
				return this.headers.Keys;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001356F File Offset: 0x0001176F
		internal void SetDefaultHeaders()
		{
			this.SetUserAgent();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00013577 File Offset: 0x00011777
		internal bool TryGetHeader(string headerName, out string headerValue)
		{
			return this.headers.TryGetValue(headerName, out headerValue);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00013588 File Offset: 0x00011788
		internal string GetHeader(string headerName)
		{
			string result;
			if (!this.TryGetHeader(headerName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000135A3 File Offset: 0x000117A3
		internal void SetHeader(string headerName, string headerValue)
		{
			if (headerValue == null)
			{
				this.headers.Remove(headerName);
				return;
			}
			this.headers[headerName] = headerValue;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000135C3 File Offset: 0x000117C3
		internal void SetHeaders(IEnumerable<KeyValuePair<string, string>> headersToSet)
		{
			this.headers.SetRange(headersToSet);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000135D1 File Offset: 0x000117D1
		internal IEnumerable<KeyValuePair<string, string>> AsEnumerable()
		{
			return this.headers.AsEnumerable<KeyValuePair<string, string>>();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000135E0 File Offset: 0x000117E0
		internal void SetRequestVersion(Version requestVersion, Version maxProtocolVersion)
		{
			if (requestVersion != null)
			{
				if (requestVersion > maxProtocolVersion)
				{
					string message = Strings.Context_RequestVersionIsBiggerThanProtocolVersion(requestVersion.ToString(), maxProtocolVersion.ToString());
					throw Error.InvalidOperation(message);
				}
				if (requestVersion.Major > 0)
				{
					Version dataServiceVersion = this.GetDataServiceVersion();
					if (dataServiceVersion == null || requestVersion > dataServiceVersion)
					{
						this.SetHeader("DataServiceVersion", requestVersion + ";NetFx");
					}
				}
			}
			this.SetHeader("MaxDataServiceVersion", maxProtocolVersion + ";NetFx");
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00013666 File Offset: 0x00011866
		internal void SetHeaderIfUnset(string headerToSet, string headerValue)
		{
			if (this.GetHeader(headerToSet) == null)
			{
				this.SetHeader(headerToSet, headerValue);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00013679 File Offset: 0x00011879
		internal void SetUserAgent()
		{
			this.SetHeader("User-Agent", "Microsoft ADO.NET Data Services");
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001368B File Offset: 0x0001188B
		internal HeaderCollection Copy()
		{
			return new HeaderCollection(this.headers);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00013698 File Offset: 0x00011898
		private Version GetDataServiceVersion()
		{
			string text;
			if (!this.TryGetHeader("DataServiceVersion", out text))
			{
				return null;
			}
			if (text.EndsWith(";NetFx", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.IndexOf(";NetFx", StringComparison.Ordinal));
			}
			return Version.Parse(text);
		}

		// Token: 0x040002E9 RID: 745
		private readonly IDictionary<string, string> headers;
	}
}
