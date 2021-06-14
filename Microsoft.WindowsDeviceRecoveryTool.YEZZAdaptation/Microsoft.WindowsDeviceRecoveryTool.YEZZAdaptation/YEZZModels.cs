using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.YEZZAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.YEZZAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class YEZZModels
	{
		// Token: 0x06000024 RID: 36 RVA: 0x0000278C File Offset: 0x0000098C
		private static ModelInfo CreateNeoModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"Billy 4.7"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Billy, identificationInfo, YEZZMsrQuery.Billy47);
			return new ModelInfo(Resources.FriendlyName_Billy, Resources.yezz_billy_4_7_hero_image, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo Billy_4_7 = YEZZModels.CreateNeoModelInfo();
	}
}
