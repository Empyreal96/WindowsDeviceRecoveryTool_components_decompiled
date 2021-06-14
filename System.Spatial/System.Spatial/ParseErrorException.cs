using System;
using System.Runtime.Serialization;

namespace System.Spatial
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class ParseErrorException : Exception
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00004288 File Offset: 0x00002488
		public ParseErrorException()
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00004290 File Offset: 0x00002490
		public ParseErrorException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000429A File Offset: 0x0000249A
		public ParseErrorException(string message) : base(message)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000042A3 File Offset: 0x000024A3
		protected ParseErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
