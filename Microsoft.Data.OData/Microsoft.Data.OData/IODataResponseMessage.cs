using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x0200025D RID: 605
	public interface IODataResponseMessage
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060013F5 RID: 5109
		IEnumerable<KeyValuePair<string, string>> Headers { get; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060013F6 RID: 5110
		// (set) Token: 0x060013F7 RID: 5111
		int StatusCode { get; set; }

		// Token: 0x060013F8 RID: 5112
		string GetHeader(string headerName);

		// Token: 0x060013F9 RID: 5113
		void SetHeader(string headerName, string headerValue);

		// Token: 0x060013FA RID: 5114
		Stream GetStream();
	}
}
