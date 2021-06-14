using System;
using System.IO;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000067 RID: 103
	public class DevicePaths
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000BE27 File Offset: 0x0000A027
		public static string ImageUpdatePath
		{
			get
			{
				return DevicePaths._imageUpdatePath;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000BE2E File Offset: 0x0000A02E
		public static string DeviceLayoutFileName
		{
			get
			{
				return DevicePaths._deviceLayoutFileName;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000BE35 File Offset: 0x0000A035
		public static string DeviceLayoutFilePath
		{
			get
			{
				return Path.Combine(DevicePaths.ImageUpdatePath, DevicePaths.DeviceLayoutFileName);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000BE46 File Offset: 0x0000A046
		public static string OemDevicePlatformFileName
		{
			get
			{
				return DevicePaths._oemDevicePlatformFileName;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000BE4D File Offset: 0x0000A04D
		public static string OemDevicePlatformFilePath
		{
			get
			{
				return Path.Combine(DevicePaths.ImageUpdatePath, DevicePaths.OemDevicePlatformFileName);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000BE5E File Offset: 0x0000A05E
		public static string UpdateOutputFile
		{
			get
			{
				return DevicePaths._updateOutputFile;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000BE65 File Offset: 0x0000A065
		public static string UpdateOutputFilePath
		{
			get
			{
				return Path.Combine(DevicePaths._updateFilesPath, DevicePaths._updateOutputFile);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000BE76 File Offset: 0x0000A076
		public static string UpdateHistoryFile
		{
			get
			{
				return DevicePaths._updateHistoryFile;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000BE7D File Offset: 0x0000A07D
		public static string UpdateHistoryFilePath
		{
			get
			{
				return Path.Combine(DevicePaths._imageUpdatePath, DevicePaths._updateHistoryFile);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000BE8E File Offset: 0x0000A08E
		public static string UpdateOSWIMName
		{
			get
			{
				return DevicePaths._updateOSWIMName;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000BE95 File Offset: 0x0000A095
		public static string UpdateOSWIMFilePath
		{
			get
			{
				return Path.Combine(DevicePaths._UpdateOSPath, DevicePaths.UpdateOSWIMName);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000BEA6 File Offset: 0x0000A0A6
		public static string MMOSWIMName
		{
			get
			{
				return DevicePaths._mmosWIMName;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000BEAD File Offset: 0x0000A0AD
		public static string MMOSWIMFilePath
		{
			get
			{
				return DevicePaths.MMOSWIMName;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000BEB4 File Offset: 0x0000A0B4
		public static string RegistryHivePath
		{
			get
			{
				return DevicePaths._registryHivePath;
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000BEBB File Offset: 0x0000A0BB
		public static string GetBCDHivePath(bool isUefiBoot)
		{
			if (!isUefiBoot)
			{
				return DevicePaths._BiosBCDHivePath;
			}
			return DevicePaths._UefiBCDHivePath;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public static string GetRegistryHiveFilePath(SystemRegistryHiveFiles hiveType, bool isUefiBoot = true)
		{
			string result = "";
			switch (hiveType)
			{
			case SystemRegistryHiveFiles.SYSTEM:
				result = Path.Combine(DevicePaths.RegistryHivePath, "SYSTEM");
				break;
			case SystemRegistryHiveFiles.SOFTWARE:
				result = Path.Combine(DevicePaths.RegistryHivePath, "SOFTWARE");
				break;
			case SystemRegistryHiveFiles.DEFAULT:
				result = Path.Combine(DevicePaths.RegistryHivePath, "DEFAULT");
				break;
			case SystemRegistryHiveFiles.DRIVERS:
				result = Path.Combine(DevicePaths.RegistryHivePath, "DRIVERS");
				break;
			case SystemRegistryHiveFiles.SAM:
				result = Path.Combine(DevicePaths.RegistryHivePath, "SAM");
				break;
			case SystemRegistryHiveFiles.SECURITY:
				result = Path.Combine(DevicePaths.RegistryHivePath, "SECURITY");
				break;
			case SystemRegistryHiveFiles.BCD:
				result = Path.Combine(DevicePaths.GetBCDHivePath(isUefiBoot), "BCD");
				break;
			}
			return result;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000BF83 File Offset: 0x0000A183
		public static string DeviceLayoutSchema
		{
			get
			{
				return "DeviceLayout.xsd";
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000BF8A File Offset: 0x0000A18A
		public static string DeviceLayoutSchema2
		{
			get
			{
				return "DeviceLayoutv2.xsd";
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000BF91 File Offset: 0x0000A191
		public static string UpdateOSInputSchema
		{
			get
			{
				return "UpdateOSInput.xsd";
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000BF98 File Offset: 0x0000A198
		public static string OEMInputSchema
		{
			get
			{
				return "OEMInput.xsd";
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000BF9F File Offset: 0x0000A19F
		public static string FeatureManifestSchema
		{
			get
			{
				return "FeatureManifest.xsd";
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000BFA6 File Offset: 0x0000A1A6
		public static string MicrosoftPhoneSKUSchema
		{
			get
			{
				return "MicrosoftPhoneSKU.xsd";
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000BFAD File Offset: 0x0000A1AD
		public static string UpdateOSOutputSchema
		{
			get
			{
				return "UpdateOSOutput.xsd";
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public static string UpdateHistorySchema
		{
			get
			{
				return "UpdateHistory.xsd";
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000BFBB File Offset: 0x0000A1BB
		public static string OEMDevicePlatformSchema
		{
			get
			{
				return "OEMDevicePlatform.xsd";
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000BFC2 File Offset: 0x0000A1C2
		public static string MSFMPath
		{
			get
			{
				return Path.Combine(DevicePaths.ImageUpdatePath, DevicePaths._FMFilesDirectory, "Microsoft");
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
		public static string MSFMPathOld
		{
			get
			{
				return DevicePaths.ImageUpdatePath;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000BFDF File Offset: 0x0000A1DF
		public static string OEMFMPath
		{
			get
			{
				return Path.Combine(DevicePaths.ImageUpdatePath, DevicePaths._FMFilesDirectory, "OEM");
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
		public static string OEMInputPath
		{
			get
			{
				return Path.Combine(DevicePaths.ImageUpdatePath, DevicePaths._OEMInputPath);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000C006 File Offset: 0x0000A206
		public static string OEMInputFile
		{
			get
			{
				return Path.Combine(DevicePaths.OEMInputPath, DevicePaths._OEMInputFile);
			}
		}

		// Token: 0x0400017F RID: 383
		public const string MAINOS_PARTITION_NAME = "MainOS";

		// Token: 0x04000180 RID: 384
		public const string MMOS_PARTITION_NAME = "MMOS";

		// Token: 0x04000181 RID: 385
		private static string _imageUpdatePath = "Windows\\ImageUpdate";

		// Token: 0x04000182 RID: 386
		private static string _updateFilesPath = "SharedData\\DuShared";

		// Token: 0x04000183 RID: 387
		private static string _registryHivePath = "Windows\\System32\\Config";

		// Token: 0x04000184 RID: 388
		private static string _BiosBCDHivePath = "boot";

		// Token: 0x04000185 RID: 389
		private static string _UefiBCDHivePath = "efi\\Microsoft\\boot";

		// Token: 0x04000186 RID: 390
		private static string _dsmPath = DevicePaths._imageUpdatePath;

		// Token: 0x04000187 RID: 391
		private static string _UpdateOSPath = "PROGRAMS\\UpdateOS\\";

		// Token: 0x04000188 RID: 392
		private static string _FMFilesDirectory = "FeatureManifest";

		// Token: 0x04000189 RID: 393
		private static string _OEMInputPath = "OEMInput";

		// Token: 0x0400018A RID: 394
		private static string _OEMInputFile = "OEMInput.xml";

		// Token: 0x0400018B RID: 395
		private static string _deviceLayoutFileName = "DeviceLayout.xml";

		// Token: 0x0400018C RID: 396
		private static string _oemDevicePlatformFileName = "OEMDevicePlatform.xml";

		// Token: 0x0400018D RID: 397
		private static string _updateOutputFile = "UpdateOutput.xml";

		// Token: 0x0400018E RID: 398
		private static string _updateHistoryFile = "UpdateHistory.xml";

		// Token: 0x0400018F RID: 399
		private static string _updateOSWIMName = "UpdateOS.wim";

		// Token: 0x04000190 RID: 400
		private static string _mmosWIMName = "MMOS.wim";
	}
}
