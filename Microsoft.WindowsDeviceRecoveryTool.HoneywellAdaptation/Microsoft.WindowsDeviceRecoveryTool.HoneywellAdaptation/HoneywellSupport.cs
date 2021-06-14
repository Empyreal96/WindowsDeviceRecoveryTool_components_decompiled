using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.HoneywellAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.HoneywellAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class HoneywellSupport : IDeviceSupport
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000242C File Offset: 0x0000062C
		[ImportingConstructor]
		public HoneywellSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(HoneywellSupport.HonewellManufacturerInfo, new ModelInfo[]
			{
				HoneywellModels.Dolphin_75e_W10M,
				HoneywellModels.Dolphin_CT50_W10M
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000246E File Offset: 0x0000066E
		public Guid Id
		{
			get
			{
				return HoneywellSupport.SupportGuid;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002475 File Offset: 0x00000675
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026DC File Offset: 0x000008DC
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
				Tracer<HoneywellSupport>.WriteInformation("No Honeywell device detected. Path: {0}", new object[]
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

		// Token: 0x0400000A RID: 10
		private static readonly Guid SupportGuid = new Guid("426BA302-E393-40BA-9CBA-C041CDA02EF4");

		// Token: 0x0400000B RID: 11
		private static readonly ManufacturerInfo HonewellManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.Honeywell_logo);

		// Token: 0x0400000C RID: 12
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000D RID: 13
		private readonly ManufacturerModelsCatalog catalog;
	}
}
