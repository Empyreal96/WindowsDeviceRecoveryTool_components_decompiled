using System;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000097 RID: 151
	internal class Exceptions
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x0003CB78 File Offset: 0x0003AD78
		internal static StorageException GenerateTimeoutException(RequestResult res, Exception inner)
		{
			if (res != null)
			{
				res.HttpStatusCode = 408;
			}
			TimeoutException ex = new TimeoutException("The client could not finish the operation within specified timeout.", inner);
			return new StorageException(res, ex.Message, ex)
			{
				IsRetryable = false
			};
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0003CBB8 File Offset: 0x0003ADB8
		internal static StorageException GenerateCancellationException(RequestResult res, Exception inner)
		{
			if (res != null)
			{
				res.HttpStatusCode = 306;
				res.HttpStatusMessage = "Unused";
			}
			OperationCanceledException ex = new OperationCanceledException("Operation was canceled by user.", inner);
			return new StorageException(res, ex.Message, ex)
			{
				IsRetryable = false
			};
		}
	}
}
