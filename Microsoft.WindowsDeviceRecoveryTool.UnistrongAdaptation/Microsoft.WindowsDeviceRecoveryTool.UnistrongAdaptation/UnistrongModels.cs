using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.UnistrongAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.UnistrongAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class UnistrongModels
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000027A4 File Offset: 0x000009A4
		private static ModelInfo CreateNeoModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false),
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"T536",
				"EE7736A2"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_T536, identificationInfo, UnistrongMsrQuery.T536);
			return new ModelInfo(Resources.FriendlyName_T536, Resources.DevicePicture_7739, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo T536 = UnistrongModels.CreateNeoModelInfo();
	}
}
