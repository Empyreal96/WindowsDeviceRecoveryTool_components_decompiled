using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.TrekStorAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.TrekStorAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class TrekStorModels
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002904 File Offset: 0x00000B04
		private static ModelInfo CreatePanasonic_WE81H_FZ_E1BModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "062A"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"WinPhone 5.0 LTE"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_TrekStor_WinPhone50LTE, identificationInfo, TrekStorMsrQuery.TrekStor_Winphone5_0_LTE);
			return new ModelInfo(Resources.FriendlyName_TrekStor_WinPhone50LTE, Resources.WinPhone_4_7_HD_wp47, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000B RID: 11
		public static readonly ModelInfo TrekStor_Winphone_5_0_LTE = TrekStorModels.CreatePanasonic_WE81H_FZ_E1BModelInfo();
	}
}
