using System;
using System.Runtime.Serialization;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class OperationFailedException : Exception
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00004327 File Offset: 0x00002527
		public OperationFailedException() : base("Remote Device Operation Failed")
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004334 File Offset: 0x00002534
		public OperationFailedException(string message) : base(message)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000433D File Offset: 0x0000253D
		public OperationFailedException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004347 File Offset: 0x00002547
		protected OperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
