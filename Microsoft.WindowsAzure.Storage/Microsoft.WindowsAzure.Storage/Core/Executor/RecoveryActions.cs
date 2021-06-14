using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x02000069 RID: 105
	internal static class RecoveryActions
	{
		// Token: 0x06000DED RID: 3565 RVA: 0x00034FE2 File Offset: 0x000331E2
		internal static void RewindStream<T>(StorageCommandBase<T> cmd, Exception ex, OperationContext ctx)
		{
			RecoveryActions.SeekStream<T>(cmd, 0L);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00034FEC File Offset: 0x000331EC
		internal static void SeekStream<T>(StorageCommandBase<T> cmd, long offset)
		{
			CommonUtility.AssertNotNull("cmd", cmd);
			RESTCommand<T> restcommand = (RESTCommand<T>)cmd;
			restcommand.SendStream.Seek(offset, SeekOrigin.Begin);
		}
	}
}
