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
using Microsoft.WindowsDeviceRecoveryTool.UnistrongAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.UnistrongAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class UnistrongSupport : IDeviceSupport
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000289C File Offset: 0x00000A9C
		[ImportingConstructor]
		public UnistrongSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(UnistrongSupport.UnistrongManufacturerInfo, new ModelInfo[]
			{
				UnistrongModels.T536
			});
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000028D6 File Offset: 0x00000AD6
		public Guid Id
		{
			get
			{
				return UnistrongSupport.SupportGuid;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028DD File Offset: 0x00000ADD
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B44 File Offset: 0x00000D44
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
				Tracer<UnistrongSupport>.WriteInformation("No Unistrong device detected. Path: {0}", new object[]
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
		private static readonly Guid SupportGuid = new Guid("7AD442A9-8CA3-4C4D-8137-64275B61EE9D");

		// Token: 0x0400000D RID: 13
		private static readonly ManufacturerInfo UnistrongManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Unistrong_Logo);

		// Token: 0x0400000E RID: 14
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000F RID: 15
		private readonly ManufacturerModelsCatalog catalog;
	}
}
