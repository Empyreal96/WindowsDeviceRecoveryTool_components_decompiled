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
using Microsoft.WindowsDeviceRecoveryTool.TrekStorAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.TrekStorAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class TrekStorSupport : IDeviceSupport
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000029F4 File Offset: 0x00000BF4
		[ImportingConstructor]
		public TrekStorSupport(IMtpDeviceInfoProvider trekttorDeviceInfoProvider)
		{
			this.TrekStorDeviceInfoProvider = trekttorDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(TrekStorSupport.TrekStorManufacturerInfo, new ModelInfo[]
			{
				TrekStorModels.TrekStor_Winphone_5_0_LTE
			});
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A2E File Offset: 0x00000C2E
		public Guid Id
		{
			get
			{
				return TrekStorSupport.SupportGuid;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A35 File Offset: 0x00000C35
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C9C File Offset: 0x00000E9C
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
				Tracer<TrekStorSupport>.WriteInformation("No TrekStor device detected. Path: {0}", new object[]
				{
					detectionData.UsbDeviceInterfaceDevicePath
				});
			}
			else
			{
				MtpInterfaceInfo deviceInfo = await this.TrekStorDeviceInfoProvider.ReadInformationAsync(devicePath, cancellationToken);
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

		// Token: 0x0400000D RID: 13
		private static readonly Guid SupportGuid = new Guid("C3D0AA61-0D19-41C8-AD30-99D46A4CAB60");

		// Token: 0x0400000E RID: 14
		private static readonly ManufacturerInfo TrekStorManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Trekstor_logo);

		// Token: 0x0400000F RID: 15
		private readonly IMtpDeviceInfoProvider TrekStorDeviceInfoProvider;

		// Token: 0x04000010 RID: 16
		private readonly ManufacturerModelsCatalog catalog;
	}
}
