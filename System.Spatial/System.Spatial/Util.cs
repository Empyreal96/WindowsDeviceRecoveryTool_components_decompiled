using System;
using System.Security;
using System.Threading;

namespace System.Spatial
{
	// Token: 0x02000088 RID: 136
	internal class Util
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00009976 File Offset: 0x00007B76
		internal static void CheckArgumentNull([Util.ValidatedNotNullAttribute] object arg, string errorMessage)
		{
			if (arg == null)
			{
				throw new ArgumentNullException(errorMessage);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009984 File Offset: 0x00007B84
		internal static bool IsCatchableExceptionType(Exception e)
		{
			Type type = e.GetType();
			return type != Util.OutOfMemoryType && type != Util.StackOverflowType && type != Util.ThreadAbortType && type != Util.AccessViolationType && type != Util.NullReferenceType && !Util.SecurityType.IsAssignableFrom(type);
		}

		// Token: 0x0400010D RID: 269
		private static readonly Type StackOverflowType = typeof(StackOverflowException);

		// Token: 0x0400010E RID: 270
		private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

		// Token: 0x0400010F RID: 271
		private static readonly Type AccessViolationType = typeof(AccessViolationException);

		// Token: 0x04000110 RID: 272
		private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

		// Token: 0x04000111 RID: 273
		private static readonly Type NullReferenceType = typeof(NullReferenceException);

		// Token: 0x04000112 RID: 274
		private static readonly Type SecurityType = typeof(SecurityException);

		// Token: 0x02000089 RID: 137
		private sealed class ValidatedNotNullAttribute : Attribute
		{
		}
	}
}
