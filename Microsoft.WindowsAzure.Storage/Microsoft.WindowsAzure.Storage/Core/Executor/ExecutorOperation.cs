using System;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000091 RID: 145
	internal enum ExecutorOperation
	{
		// Token: 0x0400038E RID: 910
		NotStarted,
		// Token: 0x0400038F RID: 911
		BeginOperation,
		// Token: 0x04000390 RID: 912
		BeginGetRequestStream,
		// Token: 0x04000391 RID: 913
		EndGetRequestStream,
		// Token: 0x04000392 RID: 914
		BeginUploadRequest,
		// Token: 0x04000393 RID: 915
		EndUploadRequest,
		// Token: 0x04000394 RID: 916
		BeginGetResponse,
		// Token: 0x04000395 RID: 917
		EndGetResponse,
		// Token: 0x04000396 RID: 918
		PreProcess,
		// Token: 0x04000397 RID: 919
		GetResponseStream,
		// Token: 0x04000398 RID: 920
		BeginDownloadResponse,
		// Token: 0x04000399 RID: 921
		EndDownloadResponse,
		// Token: 0x0400039A RID: 922
		PostProcess,
		// Token: 0x0400039B RID: 923
		EndOperation
	}
}
