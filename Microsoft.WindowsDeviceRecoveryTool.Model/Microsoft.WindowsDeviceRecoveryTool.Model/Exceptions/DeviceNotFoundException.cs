using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class DeviceNotFoundException : Exception
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00003288 File Offset: 0x00001488
		public DeviceNotFoundException()
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003293 File Offset: 0x00001493
		public DeviceNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000329F File Offset: 0x0000149F
		public DeviceNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000032AC File Offset: 0x000014AC
		protected DeviceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
