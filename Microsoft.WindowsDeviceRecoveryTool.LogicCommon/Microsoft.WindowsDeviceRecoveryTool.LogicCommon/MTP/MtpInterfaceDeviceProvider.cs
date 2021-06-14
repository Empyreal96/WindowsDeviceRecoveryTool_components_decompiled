using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Nokia.Lucid;
using Nokia.Lucid.DeviceDetection;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.MTP
{
	// Token: 0x0200001E RID: 30
	public sealed class MtpInterfaceDeviceProvider
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00007274 File Offset: 0x00005474
		public async Task<MtpInterfaceDevice> GetChildMtpInterfaceDeviceAsync(string vid, string pid, string parentPath, CancellationToken cancellationToken)
		{
			bool completed = false;
			Guid parentContainerId = MtpInterfaceDeviceProvider.GetParentContainerId(parentPath);
			DeviceWatcher deviceWatcher = MtpInterfaceDeviceProvider.GetDeviceWatcher(vid, pid, parentContainerId);
			DeviceInfoSet deviceInfoSet = MtpInterfaceDeviceProvider.GetDeviceInfoSet(vid, pid, parentContainerId);
			TaskCompletionSource<MtpInterfaceDevice> taskSource = new TaskCompletionSource<MtpInterfaceDevice>();
			IDisposable deviceWatcherDisposable = null;
			Action<string> onMatchingDevicePath = delegate(string path)
			{
				if (!completed)
				{
					completed = true;
					Tracer<MtpInterfaceDeviceProvider>.WriteInformation("MTP interface found {0}", new object[]
					{
						path
					});
					taskSource.TrySetResult(new MtpInterfaceDevice(path));
					deviceWatcherDisposable.Dispose();
				}
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
			foreach (DeviceInfo deviceInfo in deviceInfoSet.EnumeratePresentDevices())
			{
				onMatchingDevicePath(deviceInfo.Path);
			}
			MtpInterfaceDevice result;
			using (cancellationToken.Register(delegate()
			{
				taskSource.TrySetCanceled();
			}))
			{
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

		// Token: 0x0600011C RID: 284 RVA: 0x000072E0 File Offset: 0x000054E0
		private static Guid GetParentContainerId(string path)
		{
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = new DeviceTypeMap(WindowsPhoneIdentifiers.GenericUsbDeviceInterfaceGuid, DeviceType.PhysicalDevice),
				Filter = ((DeviceIdentifier deviceIdentifier) => true)
			};
			return deviceInfoSet.GetDevice(path).ReadContainerId();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007360 File Offset: 0x00005560
		private static DeviceWatcher GetDeviceWatcher(string vid, string pid, Guid conatinerId)
		{
			return new DeviceWatcher
			{
				DeviceTypeMap = MtpInterfaceDeviceProvider.GetDeviceTypeMap(),
				Filter = MtpInterfaceDeviceProvider.GetFilter(vid, pid, conatinerId)
			};
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007394 File Offset: 0x00005594
		private static DeviceInfoSet GetDeviceInfoSet(string vid, string pid, Guid conatinerId)
		{
			return new DeviceInfoSet
			{
				DeviceTypeMap = MtpInterfaceDeviceProvider.GetDeviceTypeMap(),
				Filter = MtpInterfaceDeviceProvider.GetFilter(vid, pid, conatinerId)
			};
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000073C8 File Offset: 0x000055C8
		private static DeviceTypeMap GetDeviceTypeMap()
		{
			return new DeviceTypeMap(new Guid("6ac27878-a6fa-4155-ba85-f98f491d4f33"), DeviceType.Interface);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000074B4 File Offset: 0x000056B4
		private static Expression<Func<DeviceIdentifier, bool>> GetFilter(string vid, string pid, Guid containerId)
		{
			Func<DeviceIdentifier, bool> filterFunc = delegate(DeviceIdentifier identifier)
			{
				DeviceInfoSet deviceInfoSet = new DeviceInfoSet
				{
					DeviceTypeMap = MtpInterfaceDeviceProvider.GetDeviceTypeMap(),
					Filter = ((DeviceIdentifier deviceIdentifier) => true)
				};
				bool result;
				try
				{
					result = (identifier.Vid(vid) && identifier.Pid(pid) && deviceInfoSet.GetDevice(identifier.Value).ReadContainerId() == containerId);
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			};
			return (DeviceIdentifier identifier) => filterFunc(identifier);
		}

		// Token: 0x0400008F RID: 143
		private const string MtpInterfaceGuid = "6ac27878-a6fa-4155-ba85-f98f491d4f33";
	}
}
