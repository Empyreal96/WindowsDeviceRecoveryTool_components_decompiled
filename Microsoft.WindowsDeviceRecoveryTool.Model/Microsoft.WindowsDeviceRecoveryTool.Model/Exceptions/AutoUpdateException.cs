using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public class AutoUpdateException : Exception
	{
		// Token: 0x06000169 RID: 361 RVA: 0x00004B08 File Offset: 0x00002D08
		public AutoUpdateException()
		{
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004B13 File Offset: 0x00002D13
		public AutoUpdateException(string message) : base(message)
		{
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00004B1F File Offset: 0x00002D1F
		public AutoUpdateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004B2C File Offset: 0x00002D2C
		protected AutoUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
