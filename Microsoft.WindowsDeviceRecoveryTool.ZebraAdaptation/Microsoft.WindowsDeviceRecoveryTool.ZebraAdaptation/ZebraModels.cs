using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.ZebraAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.ZebraAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class ZebraModels
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002768 File Offset: 0x00000968
		private static ModelInfo CreateTC700JModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"TC700J"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FirendlyName_ZebraTC700J, identificationInfo, ZebraMsrQuery.TC700JPara);
			return new ModelInfo(Resources.FirendlyName_ZebraTC700J, Resources.ZebraTC700J, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo TC700JModel = ZebraModels.CreateTC700JModelInfo();
	}
}
