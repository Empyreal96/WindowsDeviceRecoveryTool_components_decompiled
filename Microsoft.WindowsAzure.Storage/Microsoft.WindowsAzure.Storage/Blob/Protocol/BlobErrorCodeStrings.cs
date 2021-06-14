using System;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000C8 RID: 200
	public static class BlobErrorCodeStrings
	{
		// Token: 0x04000474 RID: 1140
		public static readonly string InvalidAppendCondition = "AppendPositionConditionNotMet";

		// Token: 0x04000475 RID: 1141
		public static readonly string InvalidMaxBlobSizeCondition = "MaxBlobSizeConditionNotMet";

		// Token: 0x04000476 RID: 1142
		public static readonly string InvalidBlobOrBlock = "InvalidBlobOrBlock";

		// Token: 0x04000477 RID: 1143
		public static readonly string InvalidBlockId = "InvalidBlockId";

		// Token: 0x04000478 RID: 1144
		public static readonly string InvalidBlockList = "InvalidBlockList";

		// Token: 0x04000479 RID: 1145
		public static readonly string ContainerNotFound = "ContainerNotFound";

		// Token: 0x0400047A RID: 1146
		public static readonly string BlobNotFound = "BlobNotFound";

		// Token: 0x0400047B RID: 1147
		public static readonly string ContainerAlreadyExists = "ContainerAlreadyExists";

		// Token: 0x0400047C RID: 1148
		public static readonly string ContainerDisabled = "ContainerDisabled";

		// Token: 0x0400047D RID: 1149
		public static readonly string ContainerBeingDeleted = "ContainerBeingDeleted";

		// Token: 0x0400047E RID: 1150
		public static readonly string BlobAlreadyExists = "BlobAlreadyExists";

		// Token: 0x0400047F RID: 1151
		public static readonly string LeaseNotPresentWithBlobOperation = "LeaseNotPresentWithBlobOperation";

		// Token: 0x04000480 RID: 1152
		public static readonly string LeaseNotPresentWithContainerOperation = "LeaseNotPresentWithContainerOperation";

		// Token: 0x04000481 RID: 1153
		public static readonly string LeaseLost = "LeaseLost";

		// Token: 0x04000482 RID: 1154
		public static readonly string LeaseIdMismatchWithBlobOperation = "LeaseIdMismatchWithBlobOperation";

		// Token: 0x04000483 RID: 1155
		public static readonly string LeaseIdMismatchWithContainerOperation = "LeaseIdMismatchWithContainerOperation";

		// Token: 0x04000484 RID: 1156
		public static readonly string LeaseIdMissing = "LeaseIdMissing";

		// Token: 0x04000485 RID: 1157
		public static readonly string LeaseNotPresentWithLeaseOperation = "LeaseNotPresentWithLeaseOperation";

		// Token: 0x04000486 RID: 1158
		public static readonly string LeaseIdMismatchWithLeaseOperation = "LeaseIdMismatchWithLeaseOperation";

		// Token: 0x04000487 RID: 1159
		public static readonly string LeaseAlreadyPresent = "LeaseAlreadyPresent";

		// Token: 0x04000488 RID: 1160
		public static readonly string LeaseAlreadyBroken = "LeaseAlreadyBroken";

		// Token: 0x04000489 RID: 1161
		public static readonly string LeaseIsBrokenAndCannotBeRenewed = "LeaseIsBrokenAndCannotBeRenewed";

		// Token: 0x0400048A RID: 1162
		public static readonly string LeaseIsBreakingAndCannotBeAcquired = "LeaseIsBreakingAndCannotBeAcquired";

		// Token: 0x0400048B RID: 1163
		public static readonly string LeaseIsBreakingAndCannotBeChanged = "LeaseIsBreakingAndCannotBeChanged";

		// Token: 0x0400048C RID: 1164
		public static readonly string InfiniteLeaseDurationRequired = "InfiniteLeaseDurationRequired";

		// Token: 0x0400048D RID: 1165
		public static readonly string SnapshotsPresent = "SnapshotsPresent";

		// Token: 0x0400048E RID: 1166
		public static readonly string InvalidBlobType = "InvalidBlobType";

		// Token: 0x0400048F RID: 1167
		public static readonly string InvalidVersionForPageBlobOperation = "InvalidVersionForPageBlobOperation";

		// Token: 0x04000490 RID: 1168
		public static readonly string InvalidPageRange = "InvalidPageRange";

		// Token: 0x04000491 RID: 1169
		public static readonly string SequenceNumberConditionNotMet = "SequenceNumberConditionNotMet";

		// Token: 0x04000492 RID: 1170
		public static readonly string SequenceNumberIncrementTooLarge = "SequenceNumberIncrementTooLarge";

		// Token: 0x04000493 RID: 1171
		public static readonly string SourceConditionNotMet = "SourceConditionNotMet";

		// Token: 0x04000494 RID: 1172
		public static readonly string TargetConditionNotMet = "TargetConditionNotMet";

		// Token: 0x04000495 RID: 1173
		public static readonly string CopyAcrossAccountsNotSupported = "CopyAcrossAccountsNotSupported";

		// Token: 0x04000496 RID: 1174
		public static readonly string CannotVerifyCopySource = "CannotVerifyCopySource";

		// Token: 0x04000497 RID: 1175
		public static readonly string PendingCopyOperation = "PendingCopyOperation";

		// Token: 0x04000498 RID: 1176
		public static readonly string NoPendingCopyOperation = "NoPendingCopyOperation";

		// Token: 0x04000499 RID: 1177
		public static readonly string CopyIdMismatch = "CopyIdMismatch";
	}
}
