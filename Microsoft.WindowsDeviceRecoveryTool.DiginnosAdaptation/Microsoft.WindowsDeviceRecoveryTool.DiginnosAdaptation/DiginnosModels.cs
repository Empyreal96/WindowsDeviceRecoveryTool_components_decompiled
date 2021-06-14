using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class DiginnosModels
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
				"DG-W10M"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_DG_W10M, identificationInfo, DiginnosMsrQuery.DG_W10M);
			return new ModelInfo(Resources.FriendlyName_DG_W10M, Resources.Device_Image, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo DG_W10M = DiginnosModels.CreateNeoModelInfo();
	}
}
