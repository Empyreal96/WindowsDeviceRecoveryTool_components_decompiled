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
using Microsoft.WindowsDeviceRecoveryTool.WileyfoxAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.WileyfoxAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class WileyfoxSupport : IDeviceSupport
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000029DC File Offset: 0x00000BDC
		[ImportingConstructor]
		public WileyfoxSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(WileyfoxSupport.WileyfoxManufacturerInfo, new ModelInfo[]
			{
				WileyfoxModels.Wileyfox_Pro
			});
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A16 File Offset: 0x00000C16
		public Guid Id
		{
			get
			{
				return WileyfoxSupport.SupportGuid;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A1D File Offset: 0x00000C1D
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C84 File Offset: 0x00000E84
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
				Tracer<WileyfoxSupport>.WriteInformation("No Wileyfox device detected. Path: {0}", new object[]
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

		// Token: 0x0400000D RID: 13
		private static readonly Guid SupportGuid = new Guid("A578DCBE-0781-45BE-AD4B-C1F8C018B8E1");

		// Token: 0x0400000E RID: 14
		private static readonly ManufacturerInfo WileyfoxManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Wileyfox_logo);

		// Token: 0x0400000F RID: 15
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x04000010 RID: 16
		private readonly ManufacturerModelsCatalog catalog;
	}
}
