using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public sealed class FfuSignatureCheckException : Exception
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000046A0 File Offset: 0x000028A0
		// (set) Token: 0x06000045 RID: 69 RVA: 0x000046B7 File Offset: 0x000028B7
		public int ProcessExitCode { get; private set; }

		// Token: 0x06000046 RID: 70 RVA: 0x000046C0 File Offset: 0x000028C0
		public FfuSignatureCheckException()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000046CB File Offset: 0x000028CB
		public FfuSignatureCheckException(string message) : base(message)
		{
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000046D7 File Offset: 0x000028D7
		public FfuSignatureCheckException(string message, int processExitCode) : base(message)
		{
			this.ProcessExitCode = processExitCode;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000046EB File Offset: 0x000028EB
		public FfuSignatureCheckException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000046F8 File Offset: 0x000028F8
		private FfuSignatureCheckException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
