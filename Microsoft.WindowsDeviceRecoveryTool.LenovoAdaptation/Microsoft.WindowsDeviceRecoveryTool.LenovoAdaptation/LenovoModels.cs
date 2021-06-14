using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.LenovoAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.LenovoAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class LenovoModels
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
				"503LV"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Softbank_503LV, identificationInfo, LenovoMsrQuery.Softbank_503LVPara);
			return new ModelInfo(Resources.FriendlyName_Softbank_503LV, Resources.lenovo_softbank_503lv, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo Softbank_503LVModel = LenovoModels.CreateNeoModelInfo();
	}
}
