using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Core.Cache
{
	// Token: 0x02000003 RID: 3
	[Export(typeof(IDeviceInformationCacheManager))]
	public class DeviceInformationCacheManager : IDeviceInformationCacheManager
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00000274
		public IDisposable EnableCacheForDevicePath(string devicePath)
		{
			DeviceInformationCacheManager.DevicePathBasedCacheObject newCacheObject = new DeviceInformationCacheManager.DevicePathBasedCacheObject(devicePath);
			this.cacheObjects.Add(newCacheObject);
			return new DeviceInformationCacheManager.Disposable(delegate()
			{
				this.cacheObjects.Remove(newCacheObject);
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D8 File Offset: 0x000002D8
		public IDevicePathBasedCacheObject GetCacheObjectForDevicePath(string devicePath)
		{
			DeviceInformationCacheManager.DevicePathBasedCacheObject devicePathBasedCacheObject = this.cacheObjects.FirstOrDefault((DeviceInformationCacheManager.DevicePathBasedCacheObject cache) => cache.DevicePath == devicePath);
			if (devicePathBasedCacheObject != null)
			{
				return devicePathBasedCacheObject;
			}
			return new DeviceInformationCacheManager.EmptyDevicePathBasedCacheObject();
		}

		// Token: 0x04000001 RID: 1
		public static readonly DeviceInformationCacheManager None = new DeviceInformationCacheManager();

		// Token: 0x04000002 RID: 2
		private readonly List<DeviceInformationCacheManager.DevicePathBasedCacheObject> cacheObjects = new List<DeviceInformationCacheManager.DevicePathBasedCacheObject>();

		// Token: 0x02000004 RID: 4
		private class Disposable : IDisposable
		{
			// Token: 0x06000007 RID: 7 RVA: 0x00002133 File Offset: 0x00000333
			public Disposable(Action onDispose)
			{
				this.onDispose = onDispose;
			}

			// Token: 0x06000008 RID: 8 RVA: 0x00002142 File Offset: 0x00000342
			public void Dispose()
			{
				if (this.isDisposed)
				{
					return;
				}
				this.onDispose();
				this.isDisposed = true;
			}

			// Token: 0x04000003 RID: 3
			private readonly Action onDispose;

			// Token: 0x04000004 RID: 4
			private bool isDisposed;
		}

		// Token: 0x02000006 RID: 6
		private class EmptyDevicePathBasedCacheObject : IDevicePathBasedCacheObject
		{
			// Token: 0x0600000B RID: 11 RVA: 0x0000215F File Offset: 0x0000035F
			public bool TryGetReadInformationTaskForReader<T>(Type readerType, out Task<T> readInformationTask)
			{
				readInformationTask = null;
				return false;
			}

			// Token: 0x0600000C RID: 12 RVA: 0x00002165 File Offset: 0x00000365
			public void AddReadInformationTaskForReader<T>(Type readerType, Task<T> readInformationTask)
			{
			}
		}

		// Token: 0x02000007 RID: 7
		private sealed class DevicePathBasedCacheObject : IDevicePathBasedCacheObject
		{
			// Token: 0x0600000E RID: 14 RVA: 0x0000216F File Offset: 0x0000036F
			internal DevicePathBasedCacheObject(string devicePath)
			{
				this.devicePath = devicePath;
			}

			// Token: 0x17000001 RID: 1
			// (get) Token: 0x0600000F RID: 15 RVA: 0x00002189 File Offset: 0x00000389
			public string DevicePath
			{
				get
				{
					return this.devicePath;
				}
			}

			// Token: 0x06000010 RID: 16 RVA: 0x000021AC File Offset: 0x000003AC
			public bool TryGetReadInformationTaskForReader<T>(Type readerType, out Task<T> readInformationTask)
			{
				DeviceInformationCacheManager.DevicePathBasedCacheObject.CacheItem cacheItem = this.cache.FirstOrDefault((DeviceInformationCacheManager.DevicePathBasedCacheObject.CacheItem item) => item.ReaderType == readerType);
				if (cacheItem == null)
				{
					readInformationTask = null;
					return false;
				}
				readInformationTask = (Task<T>)cacheItem.ReadInformationTask;
				return true;
			}

			// Token: 0x06000011 RID: 17 RVA: 0x000021F4 File Offset: 0x000003F4
			public void AddReadInformationTaskForReader<T>(Type readerType, Task<T> readInformationTask)
			{
				this.cache.Add(new DeviceInformationCacheManager.DevicePathBasedCacheObject.CacheItem
				{
					ReaderType = readerType,
					ReadInformationTask = readInformationTask
				});
			}

			// Token: 0x04000005 RID: 5
			private readonly string devicePath;

			// Token: 0x04000006 RID: 6
			private readonly List<DeviceInformationCacheManager.DevicePathBasedCacheObject.CacheItem> cache = new List<DeviceInformationCacheManager.DevicePathBasedCacheObject.CacheItem>();

			// Token: 0x02000008 RID: 8
			private sealed class CacheItem
			{
				// Token: 0x17000002 RID: 2
				// (get) Token: 0x06000012 RID: 18 RVA: 0x00002221 File Offset: 0x00000421
				// (set) Token: 0x06000013 RID: 19 RVA: 0x00002229 File Offset: 0x00000429
				public object ReadInformationTask { get; set; }

				// Token: 0x17000003 RID: 3
				// (get) Token: 0x06000014 RID: 20 RVA: 0x00002232 File Offset: 0x00000432
				// (set) Token: 0x06000015 RID: 21 RVA: 0x0000223A File Offset: 0x0000043A
				public Type ReaderType { get; set; }
			}
		}
	}
}
