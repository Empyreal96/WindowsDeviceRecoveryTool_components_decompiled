using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Nokia.Lucid;
using Nokia.Lucid.DeviceDetection;
using Nokia.Lucid.DeviceInformation;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x0200001D RID: 29
	public sealed class UsbDeviceMonitor : IUsbDeviceMonitor, IDisposable
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00006048 File Offset: 0x00004248
		private UsbDeviceMonitor(IDisposable notificationTicket)
		{
			this.events = new BlockingCollection<UsbDeviceChangeEvent>(25);
			this.notificationTicket = notificationTicket;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000608C File Offset: 0x0000428C
		public static UsbDeviceMonitor StartNew(DeviceTypeMap deviceTypeMap, Expression<Func<DeviceIdentifier, bool>> deviceIdentifierFilter)
		{
			if (deviceIdentifierFilter == null)
			{
				throw new ArgumentNullException("deviceIdentifierFilter");
			}
			SynchronizationContext context = SynchronizationContext.Current;
			DeviceWatcher deviceWatcher = new DeviceWatcher
			{
				DeviceTypeMap = deviceTypeMap,
				Filter = deviceIdentifierFilter
			};
			IDisposable disposable = deviceWatcher.Start();
			UsbDeviceMonitor deviceMonitor = new UsbDeviceMonitor(disposable);
			deviceWatcher.DeviceChanged += delegate(object s, DeviceChangedEventArgs a)
			{
				deviceMonitor.DeviceWatcherOnDeviceChanged(context, s, new UsbDeviceChangeEvent(a, false));
			};
			DeviceInfoSet deviceInfoSet = new DeviceInfoSet
			{
				DeviceTypeMap = deviceTypeMap,
				Filter = deviceIdentifierFilter
			};
			foreach (DeviceInfo deviceInfo in deviceInfoSet.EnumeratePresentDevices())
			{
				DeviceChangedEventArgs data = new DeviceChangedEventArgs(DeviceChangeAction.Attach, deviceInfo.Path, deviceInfo.DeviceType);
				deviceMonitor.DeviceWatcherOnDeviceChanged(context, deviceWatcher, new UsbDeviceChangeEvent(data, true));
			}
			return deviceMonitor;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000063D0 File Offset: 0x000045D0
		public async Task<UsbDeviceChangeEvent> TakeDeviceChangeEventAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			UsbDeviceChangeEvent @event;
			UsbDeviceChangeEvent result;
			if (this.events.TryTake(out @event))
			{
				result = @event;
			}
			else
			{
				TaskCompletionSource<UsbDeviceChangeEvent> completionSource = new TaskCompletionSource<UsbDeviceChangeEvent>();
				using (cancellationToken.Register(delegate()
				{
					completionSource.TrySetCanceled();
				}))
				{
					this.pendingTask = completionSource;
					@event = await this.pendingTask.Task;
					result = @event;
				}
			}
			return result;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006422 File Offset: 0x00004622
		public void Dispose()
		{
			this.notificationTicket.Dispose();
			this.events.CompleteAdding();
			this.events.Dispose();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000644C File Offset: 0x0000464C
		private static void TraceDeviceChanged(DeviceChangedEventArgs args)
		{
			if (args != null)
			{
				if (args.Action == DeviceChangeAction.Attach)
				{
					Tracer<UsbDeviceMonitor>.WriteInformation("Device attached: {0}, Path: {1}", new object[]
					{
						args.DeviceType.ToString(),
						args.Path
					});
				}
				else
				{
					Tracer<UsbDeviceMonitor>.WriteInformation("Device detached: {0}, Path: {1}", new object[]
					{
						args.DeviceType.ToString(),
						args.Path
					});
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006600 File Offset: 0x00004800
		private void DeviceWatcherOnDeviceChanged(SynchronizationContext synchronizationContext, object sender, UsbDeviceChangeEvent args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			UsbDeviceMonitor.TraceDeviceChanged(args.Data);
			EventHandler<UsbDeviceChangeEvent> baseHandler = delegate(object s, UsbDeviceChangeEvent a)
			{
				if (this.pendingTask != null && this.pendingTask.TrySetResult(a))
				{
					this.pendingTask = null;
				}
				else if (!this.events.Any((UsbDeviceChangeEvent e) => string.Equals(e.Data.Path, a.Data.Path, StringComparison.InvariantCultureIgnoreCase) && e.Data.Action == a.Data.Action && e.Data.DeviceType == a.Data.DeviceType))
				{
					this.events.TryAdd(a);
				}
			};
			if (synchronizationContext == null || synchronizationContext.GetType() == typeof(SynchronizationContext))
			{
				baseHandler(sender, args);
			}
			else
			{
				synchronizationContext.Post(delegate(object state)
				{
					baseHandler(sender, args);
				}, null);
			}
		}

		// Token: 0x04000054 RID: 84
		private readonly BlockingCollection<UsbDeviceChangeEvent> events;

		// Token: 0x04000055 RID: 85
		private readonly IDisposable notificationTicket;

		// Token: 0x04000056 RID: 86
		private TaskCompletionSource<UsbDeviceChangeEvent> pendingTask;
	}
}
