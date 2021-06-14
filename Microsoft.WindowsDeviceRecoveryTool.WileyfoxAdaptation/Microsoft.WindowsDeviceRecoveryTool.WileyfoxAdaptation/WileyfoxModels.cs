using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.WileyfoxAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.WileyfoxAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class WileyfoxModels
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002904 File Offset: 0x00000B04
		private static ModelInfo CreateWileyfox_Pro_ModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"Pro"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Wileyfox_Pro, identificationInfo, WileyfoxMsrQuery.Wileyfox_Pro);
			return new ModelInfo(Resources.FriendlyName_Wileyfox_Pro, Resources.Wileyfox_Pro_mobileImage, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000B RID: 11
		public static readonly ModelInfo Wileyfox_Pro = WileyfoxModels.CreateWileyfox_Pro_ModelInfo();
	}
}
