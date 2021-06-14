using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class FFUDeviceDiskWriteException : FFUException
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000031CD File Offset: 0x000013CD
		public FFUDeviceDiskWriteException()
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000031D5 File Offset: 0x000013D5
		public FFUDeviceDiskWriteException(string message) : base(message)
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000031DE File Offset: 0x000013DE
		public FFUDeviceDiskWriteException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000031E8 File Offset: 0x000013E8
		public FFUDeviceDiskWriteException(IFFUDevice device, string message, Exception e) : base(device, message, e)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000031F3 File Offset: 0x000013F3
		protected FFUDeviceDiskWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
