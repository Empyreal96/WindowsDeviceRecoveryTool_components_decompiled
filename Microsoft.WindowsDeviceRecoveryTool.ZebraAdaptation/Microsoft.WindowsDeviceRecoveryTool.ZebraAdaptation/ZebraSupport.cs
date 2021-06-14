using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.ZebraAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.ZebraAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class ZebraSupport : IDeviceSupport
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002855 File Offset: 0x00000A55
		public Guid Id
		{
			get
			{
				return ZebraSupport.SupportGuid;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000285C File Offset: 0x00000A5C
		[ImportingConstructor]
		public ZebraSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(ZebraSupport.ManufacturerInfo, new ModelInfo[]
			{
				ZebraModels.TC700JModel
			});
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002896 File Offset: 0x00000A96
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002AFC File Offset: 0x00000CFC
		public async Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken)
		{
			if (detectionData.IsDeviceSupported)
			{
				throw new InvalidOperationException("Device is already supported.");
			}
			cancellationToken.ThrowIfCancellationRequested();
			VidPidPair vidPidPair = detectionData.VidPidPair;
			string devicePath = detectionData.UsbDeviceInterfaceDevicePath;
			if (this.catalog.Models.FirstOrDefault((ModelInfo m) => m.DetectionInfo.DeviceDetectionInformations.Any((DeviceDetectionInformation di) => di.VidPidPair == vidPidPair)) == null)
			{
				Tracer<ZebraSupport>.WriteInformation("No Zebra device detected. Path: {0}", new object[]
				{
					detectionData.UsbDeviceInterfaceDevicePath
				});
			}
			else
			{
				MtpInterfaceInfo deviceInfo = await this.mtpDeviceInfoProvider.ReadInformationAsync(devicePath, cancellationToken);
				string mtpDeviceDescription = deviceInfo.Description;
				ModelInfo modelInfo;
				if (this.catalog.TryGetModelInfo(mtpDeviceDescription, out modelInfo))
				{
					string name = modelInfo.Name;
					byte[] deviceBitmapBytes = modelInfo.Bitmap.ToBytes();
					detectionData.DeviceBitmapBytes = deviceBitmapBytes;
					detectionData.DeviceSalesName = name;
					detectionData.IsDeviceSupported = true;
				}
			}
		}

		// Token: 0x0400000C RID: 12
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000D RID: 13
		private static readonly Guid SupportGuid = new Guid("E41DFB86-BD53-46DF-88BD-BECE11D45A12");

		// Token: 0x0400000E RID: 14
		private static readonly ManufacturerInfo ManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Maufacturer, Resources.Zebra_Logo);

		// Token: 0x0400000F RID: 15
		private readonly ManufacturerModelsCatalog catalog;
	}
}
