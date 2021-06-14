using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000C RID: 12
	public interface IDeviceInformationProvider<T>
	{
		// Token: 0x06000028 RID: 40
		Task<T> ReadInformationAsync(string devicePath, CancellationToken token);
	}
}
