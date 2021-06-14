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
using Microsoft.WindowsDeviceRecoveryTool.VAIOAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.VAIOAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class VAIOSupport : IDeviceSupport
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000283C File Offset: 0x00000A3C
		[ImportingConstructor]
		public VAIOSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(VAIOSupport.McjManufacturerInfo, new ModelInfo[]
			{
				VAIOModels.VPB0511S
			});
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002876 File Offset: 0x00000A76
		public Guid Id
		{
			get
			{
				return VAIOSupport.SupportGuid;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000287D File Offset: 0x00000A7D
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002AE4 File Offset: 0x00000CE4
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
				Tracer<VAIOSupport>.WriteInformation("No VAIO device detected. Path: {0}", new object[]
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
		private static readonly Guid SupportGuid = new Guid("1097789E-98FF-4215-AF33-5D3A8CD286B4");

		// Token: 0x0400000D RID: 13
		private static readonly ManufacturerInfo McjManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.logo_white_vaio);

		// Token: 0x0400000E RID: 14
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000F RID: 15
		private readonly ManufacturerModelsCatalog catalog;
	}
}
