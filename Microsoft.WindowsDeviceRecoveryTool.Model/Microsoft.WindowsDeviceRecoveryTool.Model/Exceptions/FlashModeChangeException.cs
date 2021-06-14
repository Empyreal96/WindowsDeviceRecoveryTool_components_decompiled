using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class FlashModeChangeException : Exception
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x000032B9 File Offset: 0x000014B9
		public FlashModeChangeException()
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000032C4 File Offset: 0x000014C4
		public FlashModeChangeException(string message) : base(message)
		{
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000032D0 File Offset: 0x000014D0
		public FlashModeChangeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000032DD File Offset: 0x000014DD
		protected FlashModeChangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
