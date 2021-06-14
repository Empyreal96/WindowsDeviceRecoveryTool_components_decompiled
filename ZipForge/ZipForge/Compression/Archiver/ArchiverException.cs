using System;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000003 RID: 3
	public class ArchiverException : Exception
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000233C File Offset: 0x0000133C
		public ArchiverException(string message, ErrorCode errorCode, object[] args, Exception innerException) : base(message, innerException)
		{
			this.Args = args;
			this.ErrorCode = errorCode;
		}

		// Token: 0x0400000D RID: 13
		public object[] Args;

		// Token: 0x0400000E RID: 14
		public ErrorCode ErrorCode;
	}
}
