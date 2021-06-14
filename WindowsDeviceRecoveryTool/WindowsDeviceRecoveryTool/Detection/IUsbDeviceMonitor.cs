using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x0200001C RID: 28
	public interface IUsbDeviceMonitor : IDisposable
	{
		// Token: 0x060000E2 RID: 226
		Task<UsbDeviceChangeEvent> TakeDeviceChangeEventAsync(CancellationToken cancellationToken);
	}
}
