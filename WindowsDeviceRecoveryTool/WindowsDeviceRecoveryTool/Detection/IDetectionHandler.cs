using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x02000017 RID: 23
	internal interface IDetectionHandler : IDisposable
	{
		// Token: 0x060000BB RID: 187
		Task<DeviceInfoEventArgs> TakeDeviceInfoEventAsync(CancellationToken cancellationToken);

		// Token: 0x060000BC RID: 188
		Task UpdateDeviceInfoAsync(DeviceInfo deviceInfo, CancellationToken cancellationToken);
	}
}
