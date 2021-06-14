using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class EmergencyPackageNotFoundOnServerException : Exception
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00003252 File Offset: 0x00001452
		public EmergencyPackageNotFoundOnServerException() : base("Emergency package was not found on server")
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003262 File Offset: 0x00001462
		public EmergencyPackageNotFoundOnServerException(string message) : base(message)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000326E File Offset: 0x0000146E
		public EmergencyPackageNotFoundOnServerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000327B File Offset: 0x0000147B
		protected EmergencyPackageNotFoundOnServerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
