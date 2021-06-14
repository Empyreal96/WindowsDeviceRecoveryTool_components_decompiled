using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.XOLOAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.XOLOAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class XOLOModels
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002768 File Offset: 0x00000968
		private static ModelInfo CreateNeoModelInfo()
		{
			DeviceDetectionInformation[] deviceDetectionInformations = new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			};
			DetectionInfo detectionInfo = new DetectionInfo(deviceDetectionInformations);
			IdentificationInfo identificationInfo = new IdentificationInfo(new string[]
			{
				"Win-Q900S"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_Win_Q900S, identificationInfo, XOLOMsrQuery.Win_Q900s);
			return new ModelInfo(Resources.FriendlyName_Win_Q900S, Resources.Xolo_Win_Q900s_f2, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo Win_Q900s = XOLOModels.CreateNeoModelInfo();
	}
}
