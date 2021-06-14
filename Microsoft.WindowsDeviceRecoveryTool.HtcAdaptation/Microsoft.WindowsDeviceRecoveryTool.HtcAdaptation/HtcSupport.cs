using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Lucid;
using Nokia.Lucid.DeviceInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class HtcSupport : IDeviceSupport
	{
		// Token: 0x0600000F RID: 15 RVA: 0x0000232D File Offset: 0x0000052D
		[ImportingConstructor]
		public HtcSupport(HtcModelsCatalog catalog, ILucidService lucidService)
		{
			this.catalog = catalog;
			this.lucidService = lucidService;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002348 File Offset: 0x00000548
		public Guid Id
		{
			get
			{
				return HtcSupport.SupportGuid;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002390 File Offset: 0x00000590
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return (from vp in this.catalog.Models.SelectMany((ModelInfo m) => m.VidPidPairs)
			select new DeviceDetectionInformation(vp, false)).ToArray<DeviceDetectionInformation>();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025C8 File Offset: 0x000007C8
		public async Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken)
		{
			if (detectionData.IsDeviceSupported)
			{
				throw new InvalidOperationException("Device is already supported.");
			}
			ModelInfo modelInfo = this.catalog.Models.FirstOrDefault((ModelInfo m) => m.VidPidPairs.Contains(detectionData.VidPidPair, new VidPidPairEqualityComparer()));
			if (modelInfo != null)
			{
				DeviceInfo deviceInfoForInterfaceGuid = this.lucidService.GetDeviceInfoForInterfaceGuid(detectionData.UsbDeviceInterfaceDevicePath, WellKnownGuids.UsbDeviceInterfaceGuid);
				detectionData.IsDeviceSupported = true;
				detectionData.DeviceSalesName = deviceInfoForInterfaceGuid.ReadBusReportedDeviceDescription();
				detectionData.DeviceBitmapBytes = modelInfo.Bitmap.ToBytes();
				Tracer<HtcSupport>.WriteInformation("Detected: {0}. Path: {1}", new object[]
				{
					modelInfo.FriendlyName,
					detectionData.UsbDeviceInterfaceDevicePath
				});
			}
			else
			{
				Tracer<HtcSupport>.WriteInformation("No HTC device detected. Path: {0}", new object[]
				{
					detectionData.UsbDeviceInterfaceDevicePath
				});
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly HtcModelsCatalog catalog;

		// Token: 0x0400000C RID: 12
		private readonly ILucidService lucidService;

		// Token: 0x0400000D RID: 13
		private static readonly Guid SupportGuid = new Guid("44A15B98-32C3-4641-A695-FE897AAF4777");
	}
}
