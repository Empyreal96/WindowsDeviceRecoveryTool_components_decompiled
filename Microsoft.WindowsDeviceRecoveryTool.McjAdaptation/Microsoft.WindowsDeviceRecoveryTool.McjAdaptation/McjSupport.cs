using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class McjSupport : IDeviceSupport
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002240 File Offset: 0x00000440
		[ImportingConstructor]
		public McjSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(McjSupport.McjManufacturerInfo, new ModelInfo[]
			{
				McjModels.MadosmaQ501,
				McjModels.MadosmaQ601
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002288 File Offset: 0x00000488
		public Guid Id
		{
			get
			{
				return McjSupport.SupportGuid;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022A0 File Offset: 0x000004A0
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002578 File Offset: 0x00000778
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
				Tracer<McjSupport>.WriteInformation("No MCJ device detected. Path: {0}", new object[]
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
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x04000006 RID: 6
		private readonly ManufacturerModelsCatalog catalog;

		// Token: 0x04000007 RID: 7
		private static readonly Guid SupportGuid = new Guid("E1213532-3425-4DD8-A468-0E72A75DEF04");

		// Token: 0x04000008 RID: 8
		private static readonly ManufacturerInfo McjManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.MadosmaLogo);
	}
}
