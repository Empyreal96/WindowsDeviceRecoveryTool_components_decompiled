using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000041 RID: 65
	public class NoDeviceException : Exception
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00004C9D File Offset: 0x00002E9D
		public NoDeviceException()
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public NoDeviceException(string message) : base(message)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00004CB4 File Offset: 0x00002EB4
		public NoDeviceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00004CC1 File Offset: 0x00002EC1
		protected NoDeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
