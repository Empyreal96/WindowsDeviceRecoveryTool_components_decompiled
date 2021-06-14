using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x0200003C RID: 60
	public class DataServiceClientRequestMessageArgs
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000AB78 File Offset: 0x00008D78
		public DataServiceClientRequestMessageArgs(string method, Uri requestUri, bool useDefaultCredentials, bool usePostTunneling, IDictionary<string, string> headers)
		{
			this.Headers = headers;
			this.Method = method;
			this.RequestUri = requestUri;
			this.UsePostTunneling = usePostTunneling;
			this.UseDefaultCredentials = useDefaultCredentials;
			this.actualMethod = this.Method;
			if (this.UsePostTunneling && this.Headers.ContainsKey("X-HTTP-Method"))
			{
				this.actualMethod = "POST";
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000ABE1 File Offset: 0x00008DE1
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000ABE9 File Offset: 0x00008DE9
		public string Method { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000ABF2 File Offset: 0x00008DF2
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000ABFA File Offset: 0x00008DFA
		public Uri RequestUri { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000AC03 File Offset: 0x00008E03
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000AC0B File Offset: 0x00008E0B
		public bool UsePostTunneling { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000AC14 File Offset: 0x00008E14
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000AC1C File Offset: 0x00008E1C
		public IDictionary<string, string> Headers { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000AC25 File Offset: 0x00008E25
		public string ActualMethod
		{
			get
			{
				return this.actualMethod;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000AC2D File Offset: 0x00008E2D
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000AC35 File Offset: 0x00008E35
		public bool UseDefaultCredentials { get; private set; }

		// Token: 0x04000216 RID: 534
		private readonly string actualMethod;
	}
}
