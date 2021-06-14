using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class DeviceException : Exception
	{
		// Token: 0x06000040 RID: 64 RVA: 0x0000466E File Offset: 0x0000286E
		public DeviceException()
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004679 File Offset: 0x00002879
		public DeviceException(string message) : base(message)
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004685 File Offset: 0x00002885
		public DeviceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004692 File Offset: 0x00002892
		protected DeviceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
