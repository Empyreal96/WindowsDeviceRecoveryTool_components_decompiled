using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Core.Cache
{
	// Token: 0x02000009 RID: 9
	public static class DeviceInformationProviderExtensions
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000240C File Offset: 0x0000060C
		public static async Task<T> ReadInformationAsync<T>(this IDeviceInformationProvider<T> deviceInformationReader, string parentDevicePath, IDeviceInformationCacheManager cacheManager, CancellationToken cancellationToken)
		{
			IDevicePathBasedCacheObject cacheObject = cacheManager.GetCacheObjectForDevicePath(parentDevicePath);
			Task<T> salesNameTask;
			T result;
			if (cacheObject.TryGetReadInformationTaskForReader<T>(deviceInformationReader.GetType(), out salesNameTask))
			{
				result = await salesNameTask;
			}
			else
			{
				salesNameTask = deviceInformationReader.ReadInformationAsync(parentDevicePath, cancellationToken);
				cacheObject.AddReadInformationTaskForReader<T>(deviceInformationReader.GetType(), salesNameTask);
				result = await salesNameTask;
			}
			return result;
		}
	}
}
