using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class FFUManagerException : Exception
	{
		// Token: 0x06000074 RID: 116 RVA: 0x000031FD File Offset: 0x000013FD
		public FFUManagerException()
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003205 File Offset: 0x00001405
		public FFUManagerException(string message) : base(message)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000320E File Offset: 0x0000140E
		public FFUManagerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003218 File Offset: 0x00001418
		protected FFUManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
