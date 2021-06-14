using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class Crc32Exception : Exception
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00004B7B File Offset: 0x00002D7B
		public Crc32Exception(string filePath) : base(string.Format("Invalid Crc32 for file: {0}.", filePath))
		{
			this.FilePath = filePath;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00004B9C File Offset: 0x00002D9C
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00004BB3 File Offset: 0x00002DB3
		public string FilePath { get; private set; }
	}
}
