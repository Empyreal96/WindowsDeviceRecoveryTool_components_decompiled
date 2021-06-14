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
using Microsoft.WindowsDeviceRecoveryTool.YEZZAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.YEZZAdaptation
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(IDeviceSupport))]
	internal class YEZZSupport : IDeviceSupport
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002864 File Offset: 0x00000A64
		[ImportingConstructor]
		public YEZZSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(YEZZSupport.YEZZManufacturerInfo, new ModelInfo[]
			{
				YEZZModels.Billy_4_7
			});
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000289E File Offset: 0x00000A9E
		public Guid Id
		{
			get
			{
				return YEZZSupport.SupportGuid;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000028A5 File Offset: 0x00000AA5
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002BB8 File Offset: 0x00000DB8
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
				Tracer<YEZZSupport>.WriteInformation("No YEZZ device detected. Path: {0}", new object[]
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
				else
				{
					this.catalog.Models.FirstOrDefault<ModelInfo>().Variants.ToList<VariantInfo>().ForEach(delegate(VariantInfo v)
					{
						v.IdentificationInfo.DeviceReturnedValues.ToList<string>().ForEach(delegate(string dv)
						{
							Tracer<YEZZSupport>.WriteInformation("No YEZZ device detected. mtpDeviceDescription: {0}, YEZZ IdentificationInfo: {1}", new object[]
							{
								mtpDeviceDescription,
								dv
							});
						});
					});
				}
			}
		}

		// Token: 0x0400000C RID: 12
		private static readonly Guid SupportGuid = new Guid("BA2D8739-0A4E-4AE7-8287-9D0E31E1F391");

		// Token: 0x0400000D RID: 13
		private static readonly ManufacturerInfo YEZZManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.yezz_logo);

		// Token: 0x0400000E RID: 14
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000F RID: 15
		private readonly ManufacturerModelsCatalog catalog;
	}
}
