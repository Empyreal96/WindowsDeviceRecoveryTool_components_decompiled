using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class DownloadPackageException : Exception
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00004BBC File Offset: 0x00002DBC
		public DownloadPackageException()
		{
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004BC7 File Offset: 0x00002DC7
		public DownloadPackageException(string message) : base(message)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00004BD3 File Offset: 0x00002DD3
		public DownloadPackageException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00004BE0 File Offset: 0x00002DE0
		protected DownloadPackageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
