using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public class RestartApplicationException : Exception
	{
		// Token: 0x060000AC RID: 172 RVA: 0x000032F2 File Offset: 0x000014F2
		public RestartApplicationException()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000032FD File Offset: 0x000014FD
		public RestartApplicationException(string message) : base(message)
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003309 File Offset: 0x00001509
		public RestartApplicationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003316 File Offset: 0x00001516
		protected RestartApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
