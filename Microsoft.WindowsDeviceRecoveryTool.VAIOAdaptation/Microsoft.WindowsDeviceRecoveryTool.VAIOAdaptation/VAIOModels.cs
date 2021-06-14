using System;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;
using Microsoft.WindowsDeviceRecoveryTool.VAIOAdaptation.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.VAIOAdaptation
{
	// Token: 0x02000004 RID: 4
	internal static class VAIOModels
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
				"VPB051"
			});
			VariantInfo variantInfo = new VariantInfo(Resources.FriendlyName_VPB051, identificationInfo, VAIOMsrQuery.VPB0511S);
			return new ModelInfo(Resources.FriendlyName_VPB051, Resources.VAIO_Phone_Biz_VPB0511S, detectionInfo, new VariantInfo[]
			{
				variantInfo
			});
		}

		// Token: 0x0400000A RID: 10
		public static readonly ModelInfo VPB0511S = VAIOModels.CreateNeoModelInfo();
	}
}
