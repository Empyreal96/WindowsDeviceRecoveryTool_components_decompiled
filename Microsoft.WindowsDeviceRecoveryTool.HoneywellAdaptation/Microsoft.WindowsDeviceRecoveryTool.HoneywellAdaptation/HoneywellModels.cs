using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.HoneywellAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.HoneywellAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class HoneywellModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateDolphin_75e_W10MModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"75eL0N"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Dolphin75e_W10M, identificationInfo, HoneywellMsrQuery.Dolphin_75e_W10M);
			return new ModelInfo(Resources.FriendlyName_Dolphin75e_W10M, Resources.D75e_360Tour, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020EC File Offset: 0x000002EC
		private static ModelInfo CreateDolphin_CT50_W10MModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"DolphinCT50"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_DolphinCT50_W10M_HSKE, identificationInfo, HoneywellMsrQuery.Dolphin_CT50_HSKE_W10M);
			VariantInfo variantInfo2 = new VariantInfo(Resources.FriendlyName_DolphinCT50_W10M_LFN, identificationInfo, HoneywellMsrQuery.Dolphin_CT50_LFN_W10M);
			return new ModelInfo(Resources.FriendlyName_DolphinCT50_W10M, Resources.CT50_360Tour, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021A0 File Offset: 0x000003A0
		private static ModelInfo CreateDolphin_75e_WE8HModelIfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"75eL0N"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Dolphin75e_WE8H, identificationInfo, HoneywellMsrQuery.Dolphin_75e_WE8H);
			return new ModelInfo(Resources.FriendlyName_Dolphin75e_WE8H, Resources.dolphin75_WE8H_large, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000223C File Offset: 0x0000043C
		private static ModelInfo CreateDolphin_CT50_WE8HModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"DolphinCT50"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_DolphinCT50_WE8H, identificationInfo, HoneywellMsrQuery.Dolphin_CT50_HSKE_WE8H);
			return new ModelInfo(Resources.FriendlyName_DolphinCT50_WE8H, Resources.dolphinct50_WE8H_large, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo Dolphin_75e_W10M = HoneywellModels.CreateDolphin_75e_W10MModelInfo();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo Dolphin_CT50_W10M = HoneywellModels.CreateDolphin_CT50_W10MModelInfo();

		// Token: 0x04000003 RID: 3
		public static readonly ModelInfo Dolphin_75e_WE8H = HoneywellModels.CreateDolphin_75e_WE8HModelIfo();

		// Token: 0x04000004 RID: 4
		public static readonly ModelInfo Dolphin_CT50_WE8H = HoneywellModels.CreateDolphin_CT50_WE8HModelInfo();
	}
}
