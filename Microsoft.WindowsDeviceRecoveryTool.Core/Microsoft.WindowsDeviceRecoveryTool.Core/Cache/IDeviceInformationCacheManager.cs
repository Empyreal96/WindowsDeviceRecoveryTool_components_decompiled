using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Core.Cache
{
	// Token: 0x02000002 RID: 2
	public interface IDeviceInformationCacheManager
	{
		// Token: 0x06000001 RID: 1
		IDisposable EnableCacheForDevicePath(string devicePath);

		// Token: 0x06000002 RID: 2
		IDevicePathBasedCacheObject GetCacheObjectForDevicePath(string devicePath);
	}
}
