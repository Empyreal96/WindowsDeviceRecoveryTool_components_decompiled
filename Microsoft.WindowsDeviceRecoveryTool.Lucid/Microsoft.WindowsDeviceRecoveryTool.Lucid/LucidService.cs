using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Nokia.Lucid;
using Nokia.Lucid.DeviceDetection;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.Lucid
{
	// Token: 0x02000008 RID: 8
	[Export(typeof(ILucidService))]
	public sealed class LucidService : ILucidService
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002304 File Offset: 0x00000504
		public Task<string> TakeFirstDevicePathForInterfaceGuidAsync(string usbDeviceInterfaceDevicePath, Guid interfaceGuid, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.UsbDeviceInterfaceTypeMap,
				Filter = ((DeviceIdentifier identifier) => true)
			};
			Guid containerId = deviceInfoSet.GetDevice(usbDeviceInterfaceDevicePath).ReadContainerId();
			VidPidPair vidPidPair = VidPidPair.Parse(usbDeviceInterfaceDevicePath);
			DeviceWatcher deviceWatcher = LucidService.GetDeviceWatcher(vidPidPair.Vid, vidPidPair.Pid, interfaceGuid, containerId);
			DeviceInfoSet deviceInfoSet2 = LucidService.GetDeviceInfoSet(vidPidPair.Vid, vidPidPair.Pid, interfaceGuid, containerId);
			return LucidService.TakeDevicePathAsync(deviceWatcher, deviceInfoSet2, cancellationToken);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023BC File Offset: 0x000005BC
		public Task<string> TakeFirstDevicePathForInterfaceGuidAsync(string usbDeviceInterfaceDevicePath, Guid interfaceGuid, int interfaceNumber, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.UsbDeviceInterfaceTypeMap,
				Filter = ((DeviceIdentifier identifier) => true)
			};
			Guid containerId = deviceInfoSet.GetDevice(usbDeviceInterfaceDevicePath).ReadContainerId();
			VidPidPair vidPidPair = VidPidPair.Parse(usbDeviceInterfaceDevicePath);
			DeviceWatcher deviceWatcher = LucidService.GetDeviceWatcher(vidPidPair.Vid, vidPidPair.Pid, interfaceNumber, interfaceGuid, containerId);
			DeviceInfoSet deviceInfoSet2 = LucidService.GetDeviceInfoSet(vidPidPair.Vid, vidPidPair.Pid, interfaceNumber, interfaceGuid, containerId);
			return LucidService.TakeDevicePathAsync(deviceWatcher, deviceInfoSet2, cancellationToken);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002478 File Offset: 0x00000678
		public Task<string> TakeFirstDevicePathForDeviceAndInterfaceGuidsAsync(string usbDeviceInterfaceDevicePath, Guid deviceInterfaceGuid, Guid deviceSetupClassGuid, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.UsbDeviceInterfaceTypeMap,
				Filter = ((DeviceIdentifier identifier) => true)
			};
			Guid containerId = deviceInfoSet.GetDevice(usbDeviceInterfaceDevicePath).ReadContainerId();
			VidPidPair vidPidPair = VidPidPair.Parse(usbDeviceInterfaceDevicePath);
			DeviceWatcher deviceWatcher = LucidService.GetDeviceWatcher(vidPidPair.Vid, vidPidPair.Pid, deviceInterfaceGuid, deviceSetupClassGuid, containerId);
			DeviceInfoSet deviceInfoSet2 = LucidService.GetDeviceInfoSet(vidPidPair.Vid, vidPidPair.Pid, deviceInterfaceGuid, deviceSetupClassGuid, containerId);
			return LucidService.TakeDevicePathAsync(deviceWatcher, deviceInfoSet2, cancellationToken);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002534 File Offset: 0x00000734
		public DeviceInfo GetDeviceInfoForInterfaceGuid(string interfaceDevicePath, Guid interfaceGuid)
		{
			DeviceTypeMap deviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid);
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = deviceTypeMap,
				Filter = ((DeviceIdentifier identifier) => true)
			};
			return deviceInfoSet.GetDevice(interfaceDevicePath);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025A4 File Offset: 0x000007A4
		private static DeviceWatcher GetDeviceWatcher(string vid, string pid, int mi, Guid interfaceGuid, Guid containerId)
		{
			return new DeviceWatcher
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, mi, interfaceGuid, containerId)
			};
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025D8 File Offset: 0x000007D8
		private static DeviceInfoSet GetDeviceInfoSet(string vid, string pid, int mi, Guid interfaceGuid, Guid containerId)
		{
			return new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, mi, interfaceGuid, containerId)
			};
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000260C File Offset: 0x0000080C
		private static DeviceWatcher GetDeviceWatcher(string vid, string pid, Guid interfaceGuid, Guid containerId)
		{
			return new DeviceWatcher
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, interfaceGuid, containerId)
			};
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000263C File Offset: 0x0000083C
		private static DeviceInfoSet GetDeviceInfoSet(string vid, string pid, Guid interfaceGuid, Guid containerId)
		{
			return new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, interfaceGuid, containerId)
			};
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000266C File Offset: 0x0000086C
		private static DeviceWatcher GetDeviceWatcher(string vid, string pid, Guid interfaceGuid, Guid deviceSetupClassGuid, Guid containerId)
		{
			return new DeviceWatcher
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, interfaceGuid, deviceSetupClassGuid, containerId)
			};
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026A0 File Offset: 0x000008A0
		private static DeviceInfoSet GetDeviceInfoSet(string vid, string pid, Guid interfaceGuid, Guid deviceSetupClassGuid, Guid containerId)
		{
			return new DeviceInfoSet
			{
				DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
				Filter = LucidService.GetFilter(vid, pid, interfaceGuid, deviceSetupClassGuid, containerId)
			};
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026D1 File Offset: 0x000008D1
		private static DeviceTypeMap GetDeviceTypeMap(Guid interfaceGuid)
		{
			return new DeviceTypeMap(interfaceGuid, DeviceType.Interface);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027E4 File Offset: 0x000009E4
		private static Expression<Func<DeviceIdentifier, bool>> GetFilter(string vid, string pid, Guid interfaceGuid, Guid containerId)
		{
			Func<DeviceIdentifier, bool> filterFunc = delegate(DeviceIdentifier identifier)
			{
				DeviceInfoSet deviceInfoSet = new DeviceInfoSet
				{
					DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
					Filter = ((DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid(vid) && deviceIdentifier.Pid(pid))
				};
				bool result;
				try
				{
					result = (deviceInfoSet.GetDevice(identifier.Value).ReadContainerId() == containerId);
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			};
			return (DeviceIdentifier identifier) => filterFunc(identifier);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000029B8 File Offset: 0x00000BB8
		private static Expression<Func<DeviceIdentifier, bool>> GetFilter(string vid, string pid, int mi, Guid interfaceGuid, Guid containerId)
		{
			Func<DeviceIdentifier, bool> filterFunc = delegate(DeviceIdentifier identifier)
			{
				DeviceInfoSet deviceInfoSet = new DeviceInfoSet
				{
					DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
					Filter = ((DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid(vid) && deviceIdentifier.Pid(pid) && deviceIdentifier.MI(mi))
				};
				bool result;
				try
				{
					result = (deviceInfoSet.GetDevice(identifier.Value).ReadContainerId() == containerId);
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			};
			return (DeviceIdentifier identifier) => filterFunc(identifier);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B74 File Offset: 0x00000D74
		private static Expression<Func<DeviceIdentifier, bool>> GetFilter(string vid, string pid, Guid interfaceGuid, Guid deviceSetupClassGuid, Guid containerId)
		{
			Func<DeviceIdentifier, bool> filterFunc = delegate(DeviceIdentifier identifier)
			{
				DeviceInfoSet deviceInfoSet = new DeviceInfoSet
				{
					DeviceTypeMap = LucidService.GetDeviceTypeMap(interfaceGuid),
					Filter = ((DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid(vid) && deviceIdentifier.Pid(pid))
				};
				bool result;
				try
				{
					DeviceInfo device = deviceInfoSet.GetDevice(identifier.Value);
					result = (device.ReadContainerId() == containerId && device.ReadClassGuid() == deviceSetupClassGuid);
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			};
			return (DeviceIdentifier identifier) => filterFunc(identifier);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002F50 File Offset: 0x00001150
		private static async Task<string> TakeDevicePathAsync(DeviceWatcher deviceWatcher, DeviceInfoSet deviceInfoSet, CancellationToken cancellationToken)
		{
			bool completed = false;
			TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();
			IDisposable deviceWatcherDisposable = null;
			Action<string> onMatchingDevicePath = delegate(string path)
			{
				if (completed)
				{
					return;
				}
				completed = true;
				try
				{
					taskSource.TrySetResult(path);
				}
				catch (OperationCanceledException)
				{
					taskSource.SetCanceled();
				}
				catch (Exception exception)
				{
					taskSource.SetException(exception);
				}
				deviceWatcherDisposable.Dispose();
			};
			EventHandler<DeviceChangedEventArgs> onDevice = delegate(object sender, DeviceChangedEventArgs args)
			{
				if (args.Action == DeviceChangeAction.Attach)
				{
					onMatchingDevicePath(args.Path);
				}
			};
			EventHandler<DeviceChangedEventArgs> onDeviceChanged = SynchronizationHelper.ExecuteInCurrentContext<DeviceChangedEventArgs>(onDevice);
			deviceWatcher.DeviceChanged += onDeviceChanged;
			deviceWatcherDisposable = deviceWatcher.Start();
			string result;
			using (cancellationToken.Register(delegate()
			{
				taskSource.TrySetCanceled();
			}))
			{
				foreach (DeviceInfo deviceInfo in deviceInfoSet.EnumeratePresentDevices())
				{
					onMatchingDevicePath(deviceInfo.Path);
				}
				try
				{
					result = await taskSource.Task;
				}
				finally
				{
					deviceWatcher.DeviceChanged -= onDeviceChanged;
				}
			}
			return result;
		}

		// Token: 0x04000007 RID: 7
		private static readonly DeviceTypeMap UsbDeviceInterfaceTypeMap = new DeviceTypeMap(new Dictionary<Guid, DeviceType>
		{
			{
				WellKnownGuids.UsbDeviceInterfaceGuid,
				DeviceType.PhysicalDevice
			}
		});
	}
}
