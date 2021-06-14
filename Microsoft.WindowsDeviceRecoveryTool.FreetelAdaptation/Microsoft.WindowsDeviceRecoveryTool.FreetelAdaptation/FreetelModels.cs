using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class FreetelModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateKatana01Model()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo("FTJ152E", new IdentificationInfo(new string[]
			{
				"FTJ152"
			}), FreetelMsrQuery.FTJ152E);
			return new ModelInfo(Resources.FriendlyName_Katana01, Resources.Katana01, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020CC File Offset: 0x000002CC
		private static ModelInfo CreateKatana02Model()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			VariantInfo variantInfo = new VariantInfo("FTJ152F", new IdentificationInfo(new string[]
			{
				"FTJ152"
			}), FreetelMsrQuery.FTJ152F);
			return new ModelInfo(Resources.FriendlyName_Katana02, Resources.Katana01, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo Katana01Model = FreetelModels.CreateKatana01Model();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo Katana02Model = FreetelModels.CreateKatana02Model();
	}
}
