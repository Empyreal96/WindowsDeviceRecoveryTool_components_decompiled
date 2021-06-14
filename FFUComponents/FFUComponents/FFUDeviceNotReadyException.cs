using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class FFUDeviceNotReadyException : FFUException
	{
		// Token: 0x06000058 RID: 88 RVA: 0x000030F0 File Offset: 0x000012F0
		public FFUDeviceNotReadyException()
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000030F8 File Offset: 0x000012F8
		public FFUDeviceNotReadyException(string message) : base(message)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003101 File Offset: 0x00001301
		public FFUDeviceNotReadyException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000310B File Offset: 0x0000130B
		public FFUDeviceNotReadyException(IFFUDevice device) : base(device)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003114 File Offset: 0x00001314
		protected FFUDeviceNotReadyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
