using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.BluAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.BluAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class BluSupport : IDeviceSupport
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002780 File Offset: 0x00000980
		[ImportingConstructor]
		public BluSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(BluSupport.ManufacturerInfo, new ModelInfo[]
			{
				BluModels.WinJrLte,
				BluModels.WinHdLte,
				BluModels.WinJR410,
				BluModels.WinHd510
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000027D8 File Offset: 0x000009D8
		public Guid Id
		{
			get
			{
				return BluSupport.SupportGuid;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000027F0 File Offset: 0x000009F0
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public async Task UpdateDeviceDetectionDataAsync(DeviceDetectionData detectionData, CancellationToken cancellationToken)
		{
			if (detectionData.IsDeviceSupported)
			{
				throw new InvalidOperationException("Device is already supported.");
			}
			cancellationToken.ThrowIfCancellationRequested();
			VidPidPair vidPidPair = detectionData.VidPidPair;
			string devicePath = detectionData.UsbDeviceInterfaceDevicePath;
			ModelInfo model = this.catalog.Models.FirstOrDefault((ModelInfo m) => m.DetectionInfo.DeviceDetectionInformations.Any((DeviceDetectionInformation di) => di.VidPidPair == vidPidPair));
			if (model == null)
			{
				Tracer<BluSupport>.WriteInformation("No Blu device detected. Path: {0}", new object[]
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

		// Token: 0x0400000F RID: 15
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x04000010 RID: 16
		private static readonly Guid SupportGuid = new Guid("93AB08C4-B626-420D-BCD8-B4C3D45B1B04");

		// Token: 0x04000011 RID: 17
		private static readonly ManufacturerInfo ManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.blulogo);

		// Token: 0x04000012 RID: 18
		private readonly ManufacturerModelsCatalog catalog;
	}
}
