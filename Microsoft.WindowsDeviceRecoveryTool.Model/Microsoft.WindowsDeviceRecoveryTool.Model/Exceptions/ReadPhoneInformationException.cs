using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class ReadPhoneInformationException : Exception
	{
		// Token: 0x0600016E RID: 366 RVA: 0x00004B45 File Offset: 0x00002D45
		public ReadPhoneInformationException() : base("Could not read ProductCode or ProductType")
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00004B55 File Offset: 0x00002D55
		public ReadPhoneInformationException(string message) : base(message)
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00004B61 File Offset: 0x00002D61
		public ReadPhoneInformationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004B6E File Offset: 0x00002D6E
		protected ReadPhoneInformationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
