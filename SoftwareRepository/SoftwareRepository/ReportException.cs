using System;
using System.Runtime.Serialization;

namespace SoftwareRepository
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class ReportException : Exception
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002187 File Offset: 0x00000387
		public ReportException()
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000218F File Offset: 0x0000038F
		public ReportException(string message) : base(message)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002198 File Offset: 0x00000398
		public ReportException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021A2 File Offset: 0x000003A2
		protected ReportException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
