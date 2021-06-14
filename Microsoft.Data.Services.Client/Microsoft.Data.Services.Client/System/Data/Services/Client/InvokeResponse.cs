using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x02000066 RID: 102
	public class InvokeResponse : OperationResponse
	{
		// Token: 0x06000365 RID: 869 RVA: 0x0000EBFB File Offset: 0x0000CDFB
		public InvokeResponse(Dictionary<string, string> headers) : base(new HeaderCollection(headers))
		{
		}
	}
}
