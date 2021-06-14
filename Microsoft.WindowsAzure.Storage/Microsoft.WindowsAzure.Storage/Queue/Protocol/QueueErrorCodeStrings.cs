using System;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x020000FF RID: 255
	public static class QueueErrorCodeStrings
	{
		// Token: 0x0400054C RID: 1356
		public static readonly string QueueNotFound = "QueueNotFound";

		// Token: 0x0400054D RID: 1357
		public static readonly string QueueDisabled = "QueueDisabled";

		// Token: 0x0400054E RID: 1358
		public static readonly string QueueAlreadyExists = "QueueAlreadyExists";

		// Token: 0x0400054F RID: 1359
		public static readonly string QueueNotEmpty = "QueueNotEmpty";

		// Token: 0x04000550 RID: 1360
		public static readonly string QueueBeingDeleted = "QueueBeingDeleted";

		// Token: 0x04000551 RID: 1361
		public static readonly string PopReceiptMismatch = "PopReceiptMismatch";

		// Token: 0x04000552 RID: 1362
		public static readonly string InvalidParameter = "InvalidParameter";

		// Token: 0x04000553 RID: 1363
		public static readonly string MessageNotFound = "MessageNotFound";

		// Token: 0x04000554 RID: 1364
		public static readonly string MessageTooLarge = "MessageTooLarge";

		// Token: 0x04000555 RID: 1365
		public static readonly string InvalidMarker = "InvalidMarker";
	}
}
