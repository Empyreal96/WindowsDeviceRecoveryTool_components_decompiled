using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x0200002D RID: 45
	public class BuildingRequestEventArgs : EventArgs
	{
		// Token: 0x0600014E RID: 334 RVA: 0x0000805D File Offset: 0x0000625D
		internal BuildingRequestEventArgs(string method, Uri requestUri, HeaderCollection headers, Descriptor descriptor, HttpStack httpStack)
		{
			this.Method = method;
			this.RequestUri = requestUri;
			this.HeaderCollection = (headers ?? new HeaderCollection());
			this.ClientHttpStack = httpStack;
			this.Descriptor = descriptor;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00008093 File Offset: 0x00006293
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000809B File Offset: 0x0000629B
		public string Method { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000080A4 File Offset: 0x000062A4
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000080AC File Offset: 0x000062AC
		public Uri RequestUri
		{
			get
			{
				return this.requestUri;
			}
			set
			{
				this.requestUri = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000080B5 File Offset: 0x000062B5
		public IDictionary<string, string> Headers
		{
			get
			{
				return this.HeaderCollection.UnderlyingDictionary;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000080C2 File Offset: 0x000062C2
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000080CA File Offset: 0x000062CA
		public Descriptor Descriptor { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000080D3 File Offset: 0x000062D3
		// (set) Token: 0x06000157 RID: 343 RVA: 0x000080DB File Offset: 0x000062DB
		internal HttpStack ClientHttpStack { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000080E4 File Offset: 0x000062E4
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000080EC File Offset: 0x000062EC
		internal HeaderCollection HeaderCollection { get; private set; }

		// Token: 0x0600015A RID: 346 RVA: 0x000080F5 File Offset: 0x000062F5
		internal BuildingRequestEventArgs Clone()
		{
			return new BuildingRequestEventArgs(this.Method, this.RequestUri, this.HeaderCollection, this.Descriptor, this.ClientHttpStack);
		}

		// Token: 0x040001E2 RID: 482
		private Uri requestUri;
	}
}
