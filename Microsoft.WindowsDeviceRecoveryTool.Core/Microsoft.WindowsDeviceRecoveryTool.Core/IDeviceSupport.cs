using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000D RID: 13
	public interface IDeviceSupport
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000029 RID: 41
		Guid Id { get; }

		// Token: 0x0600002A RID: 42
		DeviceDetectionInformation[] GetDeviceDetectionInformation();

		// Token: 0x0600002B RID: 43
		Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken);
	}
}
