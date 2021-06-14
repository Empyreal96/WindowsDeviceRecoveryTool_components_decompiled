using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000111 RID: 273
	internal enum InternalError
	{
		// Token: 0x04000519 RID: 1305
		UnexpectedReadState = 4,
		// Token: 0x0400051A RID: 1306
		UnvalidatedEntityState = 6,
		// Token: 0x0400051B RID: 1307
		NullResponseStream,
		// Token: 0x0400051C RID: 1308
		EntityNotDeleted,
		// Token: 0x0400051D RID: 1309
		EntityNotAddedState,
		// Token: 0x0400051E RID: 1310
		LinkNotAddedState,
		// Token: 0x0400051F RID: 1311
		EntryNotModified,
		// Token: 0x04000520 RID: 1312
		LinkBadState,
		// Token: 0x04000521 RID: 1313
		UnexpectedBeginChangeSet,
		// Token: 0x04000522 RID: 1314
		UnexpectedBatchState,
		// Token: 0x04000523 RID: 1315
		ChangeResponseMissingContentID,
		// Token: 0x04000524 RID: 1316
		ChangeResponseUnknownContentID,
		// Token: 0x04000525 RID: 1317
		InvalidHandleOperationResponse = 18,
		// Token: 0x04000526 RID: 1318
		InvalidEndGetRequestStream = 20,
		// Token: 0x04000527 RID: 1319
		InvalidEndGetRequestCompleted,
		// Token: 0x04000528 RID: 1320
		InvalidEndGetRequestStreamRequest,
		// Token: 0x04000529 RID: 1321
		InvalidEndGetRequestStreamStream,
		// Token: 0x0400052A RID: 1322
		InvalidEndGetRequestStreamContent,
		// Token: 0x0400052B RID: 1323
		InvalidEndGetRequestStreamContentLength,
		// Token: 0x0400052C RID: 1324
		InvalidEndWrite = 30,
		// Token: 0x0400052D RID: 1325
		InvalidEndWriteCompleted,
		// Token: 0x0400052E RID: 1326
		InvalidEndWriteRequest,
		// Token: 0x0400052F RID: 1327
		InvalidEndWriteStream,
		// Token: 0x04000530 RID: 1328
		InvalidEndGetResponse = 40,
		// Token: 0x04000531 RID: 1329
		InvalidEndGetResponseCompleted,
		// Token: 0x04000532 RID: 1330
		InvalidEndGetResponseRequest,
		// Token: 0x04000533 RID: 1331
		InvalidEndGetResponseResponse,
		// Token: 0x04000534 RID: 1332
		InvalidAsyncResponseStreamCopy,
		// Token: 0x04000535 RID: 1333
		InvalidAsyncResponseStreamCopyBuffer,
		// Token: 0x04000536 RID: 1334
		InvalidEndRead = 50,
		// Token: 0x04000537 RID: 1335
		InvalidEndReadCompleted,
		// Token: 0x04000538 RID: 1336
		InvalidEndReadStream,
		// Token: 0x04000539 RID: 1337
		InvalidEndReadCopy,
		// Token: 0x0400053A RID: 1338
		InvalidEndReadBuffer,
		// Token: 0x0400053B RID: 1339
		InvalidSaveNextChange = 60,
		// Token: 0x0400053C RID: 1340
		InvalidBeginNextChange,
		// Token: 0x0400053D RID: 1341
		SaveNextChangeIncomplete,
		// Token: 0x0400053E RID: 1342
		MaterializerReturningMoreThanOneEntity,
		// Token: 0x0400053F RID: 1343
		InvalidGetResponse = 71,
		// Token: 0x04000540 RID: 1344
		InvalidHandleCompleted,
		// Token: 0x04000541 RID: 1345
		InvalidMethodCallWhenNotReadingJsonLight
	}
}
