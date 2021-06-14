using System;
using System.Threading;

namespace Nokia.Lucid.Primitives
{
	// Token: 0x02000036 RID: 54
	internal static class ExceptionServices
	{
		// Token: 0x06000175 RID: 373 RVA: 0x0000B7A3 File Offset: 0x000099A3
		public static bool IsCriticalException(Exception exception)
		{
			return exception is ThreadAbortException || exception is StackOverflowException || exception is OutOfMemoryException;
		}
	}
}
