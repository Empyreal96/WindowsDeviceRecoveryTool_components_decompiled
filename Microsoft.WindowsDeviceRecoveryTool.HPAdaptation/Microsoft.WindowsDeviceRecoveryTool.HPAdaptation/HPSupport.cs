using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.HPAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Lucid.Mtp;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.HPAdaptation
{
	// Token: 0x02000004 RID: 4
	[Export(typeof(IDeviceSupport))]
	internal class HPSupport : IDeviceSupport
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000022E4 File Offset: 0x000004E4
		[ImportingConstructor]
		public HPSupport(IMtpDeviceInfoProvider mtpDeviceInfoProvider)
		{
			this.mtpDeviceInfoProvider = mtpDeviceInfoProvider;
			this.catalog = new ManufacturerModelsCatalog(HPSupport.McjManufacturerInfo, new ModelInfo[]
			{
				HPModels.Elitex3,
				HPModels.Elitex3_Telstra,
				HPModels.Elitex3_Verizon
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000232E File Offset: 0x0000052E
		public Guid Id
		{
			get
			{
				return HPSupport.SupportGuid;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002335 File Offset: 0x00000535
		public DeviceDetectionInformation[] GetDeviceDetectionInformation()
		{
			return this.catalog.GetDeviceDetectionInformations();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000267C File Offset: 0x0000087C
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
				Tracer<HPSupport>.WriteInformation("No HP device detected. Path: {0}", new object[]
				{
					detectionData.UsbDeviceInterfaceDevicePath
				});
			}
			else
			{
				MtpInterfaceInfo deviceInfo = await this.mtpDeviceInfoProvider.ReadInformationAsync(devicePath, cancellationToken);
				string mtpDeviceDescription = deviceInfo.Description;
				int tIndex = mtpDeviceDescription.ToLower().IndexOf("telstra");
				int vIndex = mtpDeviceDescription.ToLower().IndexOf("verizon");
				if (tIndex < 0 && vIndex < 0)
				{
					HPModels.Elitex3.Variants[0].IdentificationInfo.DeviceReturnedValues[0] = HPModels.Elitex3.Variants[0].IdentificationInfo.DeviceReturnedValues[0].Replace("Standard", "").Trim();
				}
				else if (HPModels.Elitex3.Variants[0].IdentificationInfo.DeviceReturnedValues[0].IndexOf("Standard", StringComparison.OrdinalIgnoreCase) < 0)
				{
					string[] deviceReturnedValues = HPModels.Elitex3.Variants[0].IdentificationInfo.DeviceReturnedValues;
					deviceReturnedValues[0] = deviceReturnedValues[0] + " Standard";
				}
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

		// Token: 0x04000007 RID: 7
		private static readonly Guid SupportGuid = new Guid("2B5046B7-716B-42C2-ACBA-E5D5F1624334");

		// Token: 0x04000008 RID: 8
		private static readonly ManufacturerInfo McjManufacturerInfo = new ManufacturerInfo(Resources.FriendlyName_Manufacturer, Resources.HP_logo);

		// Token: 0x04000009 RID: 9
		private readonly IMtpDeviceInfoProvider mtpDeviceInfoProvider;

		// Token: 0x0400000A RID: 10
		private readonly ManufacturerModelsCatalog catalog;
	}
}
