using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.JenesisAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.JenesisAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class JenesisModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateNeoModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"L310"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_WPJ40_10, identificationInfo, JenesisMsrQuery.WPJ40_10);
			return new ModelInfo(Resources.FriendlyName_WPJ40_10, Resources.WPJ40_10_image_set1, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo WPJ40_10 = JenesisModels.CreateNeoModelInfo();
	}
}
