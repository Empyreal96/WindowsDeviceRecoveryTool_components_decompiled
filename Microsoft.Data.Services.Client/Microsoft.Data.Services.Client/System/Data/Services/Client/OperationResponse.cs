using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x02000065 RID: 101
	public abstract class OperationResponse
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000EBB5 File Offset: 0x0000CDB5
		internal OperationResponse(HeaderCollection headers)
		{
			this.headers = headers;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		public IDictionary<string, string> Headers
		{
			get
			{
				return this.headers.UnderlyingDictionary;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000EBD1 File Offset: 0x0000CDD1
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0000EBD9 File Offset: 0x0000CDD9
		public int StatusCode
		{
			get
			{
				return this.statusCode;
			}
			internal set
			{
				this.statusCode = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000EBE2 File Offset: 0x0000CDE2
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0000EBEA File Offset: 0x0000CDEA
		public Exception Error
		{
			get
			{
				return this.innerException;
			}
			set
			{
				this.innerException = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000EBF3 File Offset: 0x0000CDF3
		internal HeaderCollection HeaderCollection
		{
			get
			{
				return this.headers;
			}
		}

		// Token: 0x04000294 RID: 660
		private readonly HeaderCollection headers;

		// Token: 0x04000295 RID: 661
		private int statusCode;

		// Token: 0x04000296 RID: 662
		private Exception innerException;
	}
}
