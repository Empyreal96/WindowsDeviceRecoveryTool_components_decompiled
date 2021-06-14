using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x0200010C RID: 268
	public sealed class DataServiceResponse : IEnumerable<OperationResponse>, IEnumerable
	{
		// Token: 0x060008B8 RID: 2232 RVA: 0x000244EA File Offset: 0x000226EA
		internal DataServiceResponse(HeaderCollection headers, int statusCode, IEnumerable<OperationResponse> response, bool batchResponse)
		{
			this.headers = (headers ?? new HeaderCollection());
			this.statusCode = statusCode;
			this.batchResponse = batchResponse;
			this.response = response;
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00024518 File Offset: 0x00022718
		public IDictionary<string, string> BatchHeaders
		{
			get
			{
				return this.headers.UnderlyingDictionary;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00024525 File Offset: 0x00022725
		public int BatchStatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0002452D File Offset: 0x0002272D
		public bool IsBatchResponse
		{
			get
			{
				return this.batchResponse;
			}
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00024535 File Offset: 0x00022735
		public IEnumerator<OperationResponse> GetEnumerator()
		{
			return this.response.GetEnumerator();
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00024542 File Offset: 0x00022742
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400050F RID: 1295
		private readonly HeaderCollection headers;

		// Token: 0x04000510 RID: 1296
		private readonly int statusCode;

		// Token: 0x04000511 RID: 1297
		private readonly IEnumerable<OperationResponse> response;

		// Token: 0x04000512 RID: 1298
		private readonly bool batchResponse;
	}
}
