using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class DeviceDisconnectedException : Exception
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00003221 File Offset: 0x00001421
		public DeviceDisconnectedException()
		{
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000322C File Offset: 0x0000142C
		public DeviceDisconnectedException(string message) : base(message)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003238 File Offset: 0x00001438
		public DeviceDisconnectedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003245 File Offset: 0x00001445
		protected DeviceDisconnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
