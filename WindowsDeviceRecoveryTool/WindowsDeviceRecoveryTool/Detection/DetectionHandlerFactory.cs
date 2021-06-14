using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Core.Cache;
using Nokia.Lucid;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x0200001F RID: 31
	[Export]
	internal sealed class DetectionHandlerFactory
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x000081CC File Offset: 0x000063CC
		[ImportingConstructor]
		public DetectionHandlerFactory([ImportMany] IEnumerable<IDeviceSupport> deviceSupports, IDeviceInformationCacheManager deviceInformationCacheManager)
		{
			this.deviceSupports = deviceSupports;
			this.deviceInformationCacheManager = deviceInformationCacheManager;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000081E8 File Offset: 0x000063E8
		public IDetectionHandler CreateDetectionHandler()
		{
			Expression<Func<DeviceIdentifier, bool>> deviceIdentifierFilter = this.CreateDeviceIdentifierFilter();
			UsbDeviceMonitor usbDeviceMonitor = UsbDeviceMonitor.StartNew(DetectionHandlerFactory.DefaultDeviceTypeMap, deviceIdentifierFilter);
			return new DetectionHandler(usbDeviceMonitor, this.deviceSupports, this.deviceInformationCacheManager);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008240 File Offset: 0x00006440
		private Expression<Func<DeviceIdentifier, bool>> CreateDeviceIdentifierFilter()
		{
			DeviceDetectionInformation[] vidPidArray = this.deviceSupports.SelectMany((IDeviceSupport support) => support.GetDeviceDetectionInformation()).ToArray<DeviceDetectionInformation>();
			return (DeviceIdentifier id) => vidPidArray.Any((DeviceDetectionInformation p) => id.Vid(p.VidPidPair.Vid) && id.Pid(p.VidPidPair.Pid));
		}

		// Token: 0x0400007F RID: 127
		public static readonly Guid UsbDeviceInterfaceClassGuid = new Guid("a5dcbf10-6530-11d2-901f-00c04fb951ed");

		// Token: 0x04000080 RID: 128
		private static readonly DeviceTypeMap DefaultDeviceTypeMap = new DeviceTypeMap(new Dictionary<Guid, DeviceType>
		{
			{
				DetectionHandlerFactory.UsbDeviceInterfaceClassGuid,
				DeviceType.PhysicalDevice
			}
		});

		// Token: 0x04000081 RID: 129
		private readonly IEnumerable<IDeviceSupport> deviceSupports;

		// Token: 0x04000082 RID: 130
		private readonly IDeviceInformationCacheManager deviceInformationCacheManager;
	}
}
