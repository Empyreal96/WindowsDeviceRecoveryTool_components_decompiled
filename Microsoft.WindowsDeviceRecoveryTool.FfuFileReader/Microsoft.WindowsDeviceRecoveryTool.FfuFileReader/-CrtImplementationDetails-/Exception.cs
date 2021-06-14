using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x0200026F RID: 623
	[Serializable]
	internal class Exception : Exception
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000FFE4 File Offset: 0x0000F3E4
		protected Exception(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000FFCC File Offset: 0x0000F3CC
		public Exception(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000FFB8 File Offset: 0x0000F3B8
		public Exception(string message) : base(message)
		{
		}
	}
}
