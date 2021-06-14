using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.Exceptions
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class InternalException : Exception
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00003ADF File Offset: 0x00001CDF
		public InternalException()
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003AEA File Offset: 0x00001CEA
		public InternalException(string message) : base(message)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003AF6 File Offset: 0x00001CF6
		public InternalException(string message, Exception internalException) : base(message, internalException)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003B03 File Offset: 0x00001D03
		protected InternalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
