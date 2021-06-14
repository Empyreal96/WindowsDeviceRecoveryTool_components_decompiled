using System;
using System.IO;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000038 RID: 56
	public interface IChecksumService
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002F5 RID: 757
		// (remove) Token: 0x060002F6 RID: 758
		event Action<int> ProgressEvent;

		// Token: 0x060002F7 RID: 759
		bool IsOfType(string checksumTypeName);

		// Token: 0x060002F8 RID: 760
		byte[] CalculateChecksum(string filePath, CancellationToken cancellationToken);

		// Token: 0x060002F9 RID: 761
		byte[] CalculateChecksum(FileStream fileStream, CancellationToken cancellationToken);
	}
}
