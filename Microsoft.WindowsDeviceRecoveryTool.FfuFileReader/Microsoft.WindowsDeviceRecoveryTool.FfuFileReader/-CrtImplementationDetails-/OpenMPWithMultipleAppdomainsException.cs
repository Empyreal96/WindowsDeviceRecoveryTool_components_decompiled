using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x02000273 RID: 627
	[Serializable]
	internal class OpenMPWithMultipleAppdomainsException : Exception
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0001049C File Offset: 0x0000F89C
		protected OpenMPWithMultipleAppdomainsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00010488 File Offset: 0x0000F888
		public OpenMPWithMultipleAppdomainsException()
		{
		}
	}
}
