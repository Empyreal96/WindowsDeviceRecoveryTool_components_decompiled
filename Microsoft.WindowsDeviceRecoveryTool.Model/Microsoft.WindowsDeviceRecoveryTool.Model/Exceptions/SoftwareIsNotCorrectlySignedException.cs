using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public class SoftwareIsNotCorrectlySignedException : Exception
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00004E39 File Offset: 0x00003039
		public SoftwareIsNotCorrectlySignedException()
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004E44 File Offset: 0x00003044
		public SoftwareIsNotCorrectlySignedException(string message) : base(message)
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004E50 File Offset: 0x00003050
		public SoftwareIsNotCorrectlySignedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004E5D File Offset: 0x0000305D
		protected SoftwareIsNotCorrectlySignedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
