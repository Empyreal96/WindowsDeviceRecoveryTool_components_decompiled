using System;
using Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.AlcatelAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class AlcatelModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateIDOL4SModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false),
				new DeviceDetectionInformation(new VidPidPair("03F0", "0155"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_IDOL4S_TMibile, new IdentificationInfo(new string[]
			{
				"IDOL 4S with Windows 10"
			}), AlcatelMsrQuery.IDOL4S);
			VariantInfo variantInfo2 = new VariantInfo(Resources.FriendlyName_IDOL4S_NA, new IdentificationInfo(new string[]
			{
				"IDOL 4S with Windows 10"
			}), AlcatelMsrQuery.IDOL4S_NA);
			return new ModelInfo(Resources.FriendlyName_IDOL4S, Resources.IDOL_4S_device_front, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002138 File Offset: 0x00000338
		private static ModelInfo CreateFierceXLModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Fierce_XL, new IdentificationInfo(new string[]
			{
				"FierceXL"
			}), AlcatelMsrQuery.FierceXLPara);
			return new ModelInfo(Resources.FriendlyName_Fierce_XL, Resources._5055W_front, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021D8 File Offset: 0x000003D8
		private static ModelInfo CreateIDOLPROModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_IDOL4_PRO, new IdentificationInfo(new string[]
			{
				"IDOL 4 PRO"
			}), AlcatelMsrQuery.IDO4S_PRO);
			return new ModelInfo(Resources.FriendlyName_IDOL4_PRO, Resources.IDOL_4S_device_front, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo IDOL4S = AlcatelModels.CreateIDOL4SModelInfo();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo IDO4SPRO = AlcatelModels.CreateIDOLPROModelInfo();

		// Token: 0x04000003 RID: 3
		public static readonly ModelInfo FierceXL = AlcatelModels.CreateFierceXLModelInfo();
	}
}
