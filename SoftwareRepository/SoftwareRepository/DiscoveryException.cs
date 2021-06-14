using System;
using System.Runtime.Serialization;

namespace SoftwareRepository
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class DiscoveryException : Exception
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002187 File Offset: 0x00000387
		public DiscoveryException()
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000218F File Offset: 0x0000038F
		public DiscoveryException(string message) : base(message)
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002198 File Offset: 0x00000398
		public DiscoveryException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021A2 File Offset: 0x000003A2
		protected DiscoveryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
