using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public class DeviceException : Exception
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public DeviceException(string message) : base(message)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
		public DeviceException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
