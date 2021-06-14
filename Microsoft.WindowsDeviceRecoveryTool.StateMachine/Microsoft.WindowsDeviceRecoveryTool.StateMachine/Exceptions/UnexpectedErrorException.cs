using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.Exceptions
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class UnexpectedErrorException : Exception
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003B10 File Offset: 0x00001D10
		public UnexpectedErrorException()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003B1B File Offset: 0x00001D1B
		public UnexpectedErrorException(string message) : base(message)
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003B27 File Offset: 0x00001D27
		public UnexpectedErrorException(string message, Exception internalException) : base(message, internalException)
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003B34 File Offset: 0x00001D34
		protected UnexpectedErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
