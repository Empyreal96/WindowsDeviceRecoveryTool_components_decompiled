using System;
using System.Runtime.Serialization;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class AccessDeniedException : Exception
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000042FD File Offset: 0x000024FD
		public AccessDeniedException() : base("Remote Device Access Denied")
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000430A File Offset: 0x0000250A
		public AccessDeniedException(string message) : base(message)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004313 File Offset: 0x00002513
		public AccessDeniedException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000431D File Offset: 0x0000251D
		protected AccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
