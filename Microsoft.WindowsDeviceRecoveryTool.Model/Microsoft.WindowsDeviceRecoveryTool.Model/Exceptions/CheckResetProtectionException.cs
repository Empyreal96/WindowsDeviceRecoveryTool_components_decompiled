using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class CheckResetProtectionException : Exception
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000031F0 File Offset: 0x000013F0
		public CheckResetProtectionException()
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000031FB File Offset: 0x000013FB
		public CheckResetProtectionException(string message) : base(message)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003207 File Offset: 0x00001407
		public CheckResetProtectionException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003214 File Offset: 0x00001414
		protected CheckResetProtectionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
