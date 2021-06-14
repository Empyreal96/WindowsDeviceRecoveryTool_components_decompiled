using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.TrinityAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.TrinityAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class TrinityModels
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002764 File Offset: 0x00000964
		private static ModelInfo CreateNeoModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"NEO"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_NEO, identificationInfo, TrinityMsrQuery.Neo);
			return new ModelInfo(Resources.FriendlyName_NEO, Resources.Neo_Device_front, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo Neo = TrinityModels.CreateNeoModelInfo();
	}
}
