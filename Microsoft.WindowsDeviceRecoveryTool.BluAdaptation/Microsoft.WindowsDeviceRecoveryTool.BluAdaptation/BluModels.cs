using System;
using Microsoft.WindowsDeviceRecoveryTool.BluAdaptation.Properties;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.BluAdaptation
{
	// Token: 0x02000002 RID: 2
	internal static class BluModels
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static ModelInfo CreateWinJrLteModel()
		{
			DetectionInfo detectionInfo = new DetectionInfo(new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			});
			VariantInfo variantInfo = new VariantInfo("Win JR LTE X130Q", new IdentificationInfo(new string[]
			{
				"JR LTE"
			}), BluMsrQuery.WinJrLteX130QQuery);
			VariantInfo variantInfo2 = new VariantInfo("Win JR LTE X130E", new IdentificationInfo(new string[]
			{
				"JR LTE"
			}), BluMsrQuery.WinJrLteX130EQuery);
			return new ModelInfo(Resources.FriendlyName_Win_JR_LTE, Resources.winjrlte, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002118 File Offset: 0x00000318
		private static ModelInfo CreateWinHdLteModel()
		{
			DetectionInfo detectionInfo = new DetectionInfo(new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			});
			VariantInfo variantInfo = new VariantInfo("Win HD LTE X150Q", new IdentificationInfo(new string[]
			{
				"HD LTE"
			}), BluMsrQuery.WinHdLteX150QQuery);
			VariantInfo variantInfo2 = new VariantInfo("Win HD LTE X150E", new IdentificationInfo(new string[]
			{
				"HD LTE"
			}), BluMsrQuery.WinHdLteX150EQuery);
			return new ModelInfo(Resources.FriendlyName_Win_HD_LTE, Resources.winhdlte, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000021E0 File Offset: 0x000003E0
		private static ModelInfo CreateWin510Model()
		{
			DetectionInfo detectionInfo = new DetectionInfo(new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			});
			VariantInfo variantInfo = new VariantInfo("WIN HD W510u", new IdentificationInfo(new string[]
			{
				"W510"
			}), BluMsrQuery.WinHdW510UQuery);
			VariantInfo variantInfo2 = new VariantInfo("WIN HD W510l", new IdentificationInfo(new string[]
			{
				"w510"
			}), BluMsrQuery.WinHdW510lQuery);
			return new ModelInfo(Resources.FriendlyName_WIN_HD_W510, Resources.winhd, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000022A8 File Offset: 0x000004A8
		private static ModelInfo CreateWinJR410Model()
		{
			DetectionInfo detectionInfo = new DetectionInfo(new DeviceDetectionInformation[]
			{
				new DeviceDetectionInformation(new VidPidPair("05C6", "9093"), false),
				new DeviceDetectionInformation(new VidPidPair("045E", "F0CA"), false)
			});
			VariantInfo variantInfo = new VariantInfo("WIN JR W410a", new IdentificationInfo(new string[]
			{
				"W410"
			}), BluMsrQuery.WinJRW410AQuery);
			VariantInfo variantInfo2 = new VariantInfo("WIN JR W410i", new IdentificationInfo(new string[]
			{
				"W410"
			}), BluMsrQuery.WinJRW410iQuery);
			VariantInfo variantInfo3 = new VariantInfo("WIN JR W410u", new IdentificationInfo(new string[]
			{
				"W410"
			}), BluMsrQuery.WinJRW410uQuery);
			VariantInfo variantInfo4 = new VariantInfo("WIN JR W410l", new IdentificationInfo(new string[]
			{
				"W410"
			}), BluMsrQuery.WinJRW410lQuery);
			return new ModelInfo(Resources.FriendlyName_WIN_JR_W410, Resources.winjr, detectionInfo, new VariantInfo[]
			{
				variantInfo,
				variantInfo2,
				variantInfo3,
				variantInfo4
			});
		}

		// Token: 0x04000001 RID: 1
		public static readonly ModelInfo WinJrLte = BluModels.CreateWinJrLteModel();

		// Token: 0x04000002 RID: 2
		public static readonly ModelInfo WinHdLte = BluModels.CreateWinHdLteModel();

		// Token: 0x04000003 RID: 3
		public static readonly ModelInfo WinJR410 = BluModels.CreateWinJR410Model();

		// Token: 0x04000004 RID: 4
		public static readonly ModelInfo WinHd510 = BluModels.CreateWin510Model();
	}
}
