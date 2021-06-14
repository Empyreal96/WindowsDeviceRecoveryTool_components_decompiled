using System;
using Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class AcerModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateLiquidM220ModelInfo()
		{
			DetectionInfo detectionInfo = AcerModels.CreateDefaultDetectionInfo();
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"M220"
			});
			VariantInfo variantInfo = new VariantInfo("M220 WW_GEN1H", identificationInfo, AcerMsrQuery.M220WwGen1H);
			VariantInfo variantInfo2 = new VariantInfo("M220 PA_CUS1H", identificationInfo, AcerMsrQuery.M220PaCus1H);
			VariantInfo variantInfo3 = new VariantInfo("M220 WW_GEN1", identificationInfo, AcerMsrQuery.M220WwGen1);
			VariantInfo variantInfo4 = new VariantInfo("M220 AAP_GLOBE", identificationInfo, AcerMsrQuery.M220AapGlobe);
			return new ModelInfo(Resources.Name_Liquid_M220, Resources.M220, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2,
				variantInfo3,
				variantInfo4
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
		private static ModelInfo CreateJadePrimoModelInfo()
		{
			DetectionInfo detectionInfo = AcerModels.CreateDefaultDetectionInfo();
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"S58"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.Name_Jade_Primo, identificationInfo, AcerMsrQuery.JadePrimo);
			return new ModelInfo(Resources.Name_Jade_Primo, Resources.JadePrimo, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002150 File Offset: 0x00000350
		private static DetectionInfo CreateDefaultDetectionInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("0502", "37A3"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			return new DetectionInfo(deviceDetectionInformations);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000219C File Offset: 0x0000039C
		private static ModelInfo CreateLiquidM330ModelInfo()
		{
			DetectionInfo detectionInfo = AcerModels.CreateDefaultDetectionInfo();
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"TM01"
			});
			VariantInfo variantInfo = new VariantInfo("M330 EMEA_BGCS", identificationInfo, AcerMsrQuery.M330EmeaBgcs);
			VariantInfo variantInfo2 = new VariantInfo("M330 WW_GEN1S", identificationInfo, AcerMsrQuery.M330WwGen1S);
			VariantInfo variantInfo3 = new VariantInfo("M330 WW_GEN1", identificationInfo, AcerMsrQuery.M330WwGen1);
			VariantInfo variantInfo4 = new VariantInfo("M330 PA_GEN1", identificationInfo, AcerMsrQuery.M330PaGen1);
			return new ModelInfo(Resources.Name_Liquid_M330, Resources.M330, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2,
				variantInfo3,
				variantInfo4
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo LiquidM220 = AcerModels.CreateLiquidM220ModelInfo();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo JadePrimo = AcerModels.CreateJadePrimoModelInfo();

		// Token: 0x04000003 RID: 3
		public static readonly ModelInfo LiquidM330 = AcerModels.CreateLiquidM330ModelInfo();
	}
}
