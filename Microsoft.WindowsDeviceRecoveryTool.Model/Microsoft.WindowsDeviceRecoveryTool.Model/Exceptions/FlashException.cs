using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x0200003A RID: 58
	[Serializable]
	public class FlashException : Exception
	{
		// Token: 0x0600017D RID: 381 RVA: 0x00004C3C File Offset: 0x00002E3C
		public FlashException()
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00004C47 File Offset: 0x00002E47
		public FlashException(string message) : base(message)
		{
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00004C53 File Offset: 0x00002E53
		public FlashException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00004C60 File Offset: 0x00002E60
		protected FlashException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
