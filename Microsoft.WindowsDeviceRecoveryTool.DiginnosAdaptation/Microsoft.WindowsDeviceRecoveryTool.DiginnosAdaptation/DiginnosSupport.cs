using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class DiginnosSupport : IDeviceSupport
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002128 File Offset: 0x00000328
		[ImportingConstructor]
		public DiginnosSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(DiginnosSupport.DiginnosManufacturerInfo, new ModelInfo[]
			{
				DiginnosModels.DG_W10M
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002162 File Offset: 0x00000362
		public Guid Id
		{
			get
			{
				return DiginnosSupport.SupportGuid;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002169 File Offset: 0x00000369
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023D0 File Offset: 0x000005D0
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
				Tracer<DiginnosSupport>.WriteInformation("No Diginnos device detected. Path: {0}", new object[]
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

		// Token: 0x04000003 RID: 3
		private static readonly Guid SupportGuid = new Guid("B06E0686-FA6A-417B-B16F-B9E626CDAA71");

		// Token: 0x04000004 RID: 4
		private static readonly ManufacturerInfo DiginnosManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Diginnos_Logo);

		// Token: 0x04000005 RID: 5
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x04000006 RID: 6
		private readonly ManufacturerModelsCatalog catalog;
	}
}
