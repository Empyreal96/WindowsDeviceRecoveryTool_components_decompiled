using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class AcerSupport : IDeviceSupport
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000025BC File Offset: 0x000007BC
		[ImportingConstructor]
		public AcerSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(AcerSupport.ManufacturerInfo, new ModelInfo[]
			{
				AcerModels.LiquidM220,
				AcerModels.JadePrimo,
				AcerModels.LiquidM330
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002606 File Offset: 0x00000806
		public Guid Id
		{
			get
			{
				return AcerSupport.SupportGuid;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000260D File Offset: 0x0000080D
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002874 File Offset: 0x00000A74
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
				Tracer<AcerSupport>.WriteInformation("No Acer device detected. Path: {0}", new object[]
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
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000E RID: 14
		private static readonly Guid SupportGuid = new Guid("5DD6D8CE-DAB9-4E27-8846-669B343E89BE");

		// Token: 0x0400000F RID: 15
		private static readonly ManufacturerInfo ManufacturerInfo = new ManufacturerInfo(Resources.Name_Manufacturer, Resources.AcerLogo);

		// Token: 0x04000010 RID: 16
		private readonly ManufacturerModelsCatalog catalog;
	}
}
