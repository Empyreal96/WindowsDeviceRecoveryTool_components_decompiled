using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class PackageNotFoundException : Exception
	{
		// Token: 0x06000195 RID: 405 RVA: 0x00004D64 File Offset: 0x00002F64
		public PackageNotFoundException()
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00004D6F File Offset: 0x00002F6F
		public PackageNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00004D7B File Offset: 0x00002F7B
		public PackageNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
