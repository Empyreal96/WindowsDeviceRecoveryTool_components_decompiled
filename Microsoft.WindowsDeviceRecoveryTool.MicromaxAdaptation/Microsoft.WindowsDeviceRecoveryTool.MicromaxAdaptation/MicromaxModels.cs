using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.MicromaxAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.MicromaxAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class MicromaxModels
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
				"W121"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_W121, identificationInfo, MicromaxMsrQuery.W121);
			return new ModelInfo(Resources.FriendlyName_W121, Resources.Micromax_Canvas_Win_W121__gallery4, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo W121 = MicromaxModels.CreateNeoModelInfo();
	}
}
