using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x0200025C RID: 604
	public interface IODataRequestMessage
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060013ED RID: 5101
		IEnumerable<KeyValuePair<string, string>> Headers { get; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060013EE RID: 5102
		// (set) Token: 0x060013EF RID: 5103
		Uri Url { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060013F0 RID: 5104
		// (set) Token: 0x060013F1 RID: 5105
		string Method { get; set; }

		// Token: 0x060013F2 RID: 5106
		string GetHeader(string headerName);

		// Token: 0x060013F3 RID: 5107
		void SetHeader(string headerName, string headerValue);

		// Token: 0x060013F4 RID: 5108
		Stream GetStream();
	}
}
