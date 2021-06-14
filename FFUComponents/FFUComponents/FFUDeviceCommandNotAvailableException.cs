using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class FFUDeviceCommandNotAvailableException : FFUException
	{
		// Token: 0x0600005D RID: 93 RVA: 0x0000311E File Offset: 0x0000131E
		public FFUDeviceCommandNotAvailableException()
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003126 File Offset: 0x00001326
		public FFUDeviceCommandNotAvailableException(string message) : base(message)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000312F File Offset: 0x0000132F
		public FFUDeviceCommandNotAvailableException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003139 File Offset: 0x00001339
		public FFUDeviceCommandNotAvailableException(IFFUDevice device) : base(device)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003142 File Offset: 0x00001342
		protected FFUDeviceCommandNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
