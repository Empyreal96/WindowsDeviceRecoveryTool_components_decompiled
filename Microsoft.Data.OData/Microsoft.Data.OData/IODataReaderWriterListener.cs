using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000151 RID: 337
	internal interface IODataReaderWriterListener
	{
		// Token: 0x0600091B RID: 2331
		void OnException();

		// Token: 0x0600091C RID: 2332
		void OnCompleted();
	}
}
