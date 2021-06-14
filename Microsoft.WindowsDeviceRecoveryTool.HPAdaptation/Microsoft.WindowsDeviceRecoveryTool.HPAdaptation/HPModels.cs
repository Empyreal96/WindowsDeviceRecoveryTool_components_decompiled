using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.HPAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.HPAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class HPModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateElitex3ModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("03F0", "0155"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_HP_Elite_x3, new IdentificationInfo(new string[]
			{
				"Elite x3"
			}), HPMsrQuery.Elitex3);
			return new ModelInfo(Resources.FriendlyName_HP_Elite_x3, Resources.EliteX3_Gallery_Zoom1, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E4 File Offset: 0x000002E4
		private static ModelInfo CreateElitex3_TelstraModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("03F0", "0155"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_HP_Elite_x3_Telstra, new IdentificationInfo(new string[]
			{
				"Elite x3 Telstra"
			}), HPMsrQuery.Elitex3_Telstra);
			return new ModelInfo(Resources.FriendlyName_HP_Elite_x3_Telstra, Resources.EliteX3_Gallery_Zoom1, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002178 File Offset: 0x00000378
		private static ModelInfo CreateElitex3_VerizonModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("03F0", "0155"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_HP_Elite_x3_Verizon, new IdentificationInfo(new string[]
			{
				"Elite x3 Verizon"
			}), HPMsrQuery.Elitex3_Verizon);
			return new ModelInfo(Resources.FriendlyName_HP_Elite_x3_Verizon, Resources.EliteX3_Gallery_Zoom1, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo Elitex3 = HPModels.CreateElitex3ModelInfo();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo Elitex3_Telstra = HPModels.CreateElitex3_TelstraModelInfo();

		// Token: 0x04000003 RID: 3
		public static readonly ModelInfo Elitex3_Verizon = HPModels.CreateElitex3_VerizonModelInfo();
	}
}
