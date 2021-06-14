using System;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000E6 RID: 230
	public static class FileErrorCodeStrings
	{
		// Token: 0x040004F4 RID: 1268
		public static readonly string ShareNotFound = "ShareNotFound";

		// Token: 0x040004F5 RID: 1269
		public static readonly string ShareAlreadyExists = "ShareAlreadyExists";

		// Token: 0x040004F6 RID: 1270
		public static readonly string ShareDisabled = "ShareDisabled";

		// Token: 0x040004F7 RID: 1271
		public static readonly string ShareBeingDeleted = "ShareBeingDeleted";

		// Token: 0x040004F8 RID: 1272
		public static readonly string DeletePending = "DeletePending";

		// Token: 0x040004F9 RID: 1273
		public static readonly string ParentNotFound = "ParentNotFound";

		// Token: 0x040004FA RID: 1274
		public static readonly string InvalidResourceName = "InvalidResourceName";

		// Token: 0x040004FB RID: 1275
		public static readonly string ResourceAlreadyExists = "ResourceAlreadyExists";

		// Token: 0x040004FC RID: 1276
		public static readonly string ResourceTypeMismatch = "ResourceTypeMismatch";

		// Token: 0x040004FD RID: 1277
		public static readonly string SharingViolation = "SharingViolation";

		// Token: 0x040004FE RID: 1278
		public static readonly string CannotDeleteFileOrDirectory = "CannotDeleteFileOrDirectory";

		// Token: 0x040004FF RID: 1279
		public static readonly string FileLockConflict = "FileLockConflict";

		// Token: 0x04000500 RID: 1280
		public static readonly string ReadOnlyAttribute = "ReadOnlyAttribute";

		// Token: 0x04000501 RID: 1281
		public static readonly string ClientCacheFlushDelay = "ClientCacheFlushDelay";

		// Token: 0x04000502 RID: 1282
		public static readonly string InvalidFileOrDirectoryPathName = "InvalidFileOrDirectoryPathName";

		// Token: 0x04000503 RID: 1283
		public static readonly string ConditionHeadersNotSupported = "ConditionHeadersNotSupported";
	}
}
