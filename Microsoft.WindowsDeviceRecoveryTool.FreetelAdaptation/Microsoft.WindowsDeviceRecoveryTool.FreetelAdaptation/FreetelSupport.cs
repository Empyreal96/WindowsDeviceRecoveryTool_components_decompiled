using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class FreetelSupport : IDeviceSupport
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021E0 File Offset: 0x000003E0
		[ImportingConstructor]
		public FreetelSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(FreetelSupport.FreetelManufacturerInfo, new ModelInfo[]
			{
				FreetelModels.Katana01Model,
				FreetelModels.Katana02Model
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002222 File Offset: 0x00000422
		public Guid Id
		{
			get
			{
				return FreetelSupport.SupportGuid;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002229 File Offset: 0x00000429
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002490 File Offset: 0x00000690
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
				Tracer<FreetelSupport>.WriteInformation("No Freetel device detected. Path: {0}", new object[]
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

		// Token: 0x04000005 RID: 5
		private static readonly Guid SupportGuid = new Guid("37EB525A-9379-43C8-9B48-B5833CFEBD10");

		// Token: 0x04000006 RID: 6
		private static readonly ManufacturerInfo FreetelManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Freetel_Logo);

		// Token: 0x04000007 RID: 7
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x04000008 RID: 8
		private readonly ManufacturerModelsCatalog catalog;
	}
}
