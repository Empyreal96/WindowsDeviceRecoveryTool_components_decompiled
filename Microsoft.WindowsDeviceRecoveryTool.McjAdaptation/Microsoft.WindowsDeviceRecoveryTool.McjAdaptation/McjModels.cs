using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class McjModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateMadosmaQ501ModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"MADOSMA Q501"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_MADOSMA_Q501, identificationInfo, McjMsrQuery.MadosmaQ501Query);
			return new ModelInfo(Resources.FriendlyName_MADOSMA_Q501, Resources.Madosma, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
		private static ModelInfo CreateMadosmaQ601ModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"Q601"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_MADOSMA_Q601, identificationInfo, McjMsrQuery.MadosmaQ601Query);
			return new ModelInfo(Resources.FriendlyName_MADOSMA_Q601, Resources.Madosma_Q601, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo MadosmaQ501 = McjModels.CreateMadosmaQ501ModelInfo();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo MadosmaQ601 = McjModels.CreateMadosmaQ601ModelInfo();
	}
}
