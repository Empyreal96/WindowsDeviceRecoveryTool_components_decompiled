using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class FfuFileInfoReadException : Exception
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00004BED File Offset: 0x00002DED
		public FfuFileInfoReadException(int errorCode, string path) : base(string.Format("WP8 FFU file reading failed with error {0}, file path: {1}.", errorCode, path))
		{
			this.ErrorCode = errorCode;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004C14 File Offset: 0x00002E14
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00004C2B File Offset: 0x00002E2B
		public int ErrorCode { get; private set; }
	}
}
