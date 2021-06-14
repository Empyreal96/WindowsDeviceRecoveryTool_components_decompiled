using System;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services.DataPackageRules
{
	// Token: 0x02000035 RID: 53
	internal class FileHelper
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x0000BE40 File Offset: 0x0000A040
		public string[] GetFilesFromDirectory(string directory, string searchPattern)
		{
			return Directory.GetFiles(directory, searchPattern);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000BE5C File Offset: 0x0000A05C
		public Stream GetFileStream(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
	}
}
