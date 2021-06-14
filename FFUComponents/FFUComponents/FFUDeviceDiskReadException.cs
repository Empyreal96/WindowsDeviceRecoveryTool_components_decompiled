using System;
using System.Runtime.Serialization;

namespace FFUComponents
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class FFUDeviceDiskReadException : FFUException
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000319D File Offset: 0x0000139D
		public FFUDeviceDiskReadException()
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000031A5 File Offset: 0x000013A5
		public FFUDeviceDiskReadException(string message) : base(message)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000031AE File Offset: 0x000013AE
		public FFUDeviceDiskReadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000031B8 File Offset: 0x000013B8
		public FFUDeviceDiskReadException(IFFUDevice device, string message, Exception e) : base(device, message, e)
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000031C3 File Offset: 0x000013C3
		protected FFUDeviceDiskReadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
