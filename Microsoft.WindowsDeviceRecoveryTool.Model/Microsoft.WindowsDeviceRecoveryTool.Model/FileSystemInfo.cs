using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000047 RID: 71
	public static class FileSystemInfo
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00004F2E File Offset: 0x0000312E
		public static string CustomPackagesPath { private get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00004F38 File Offset: 0x00003138
		public static string AppNamePrefix
		{
			get
			{
				return "WDRT";
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00004F50 File Offset: 0x00003150
		public static string DefaultPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Packages\\");
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00004F78 File Offset: 0x00003178
		public static string DefaultProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Packages\\Products\\");
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00004FA0 File Offset: 0x000031A0
		public static string DefaultFfuPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Packages\\");
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00004FC8 File Offset: 0x000031C8
		public static string NokiaProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Nokia\\Packages\\Products\\");
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00004FF0 File Offset: 0x000031F0
		public static string NokiaPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Nokia\\Packages\\");
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00005018 File Offset: 0x00003218
		public static string HtcPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "HTC\\Packages\\");
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00005040 File Offset: 0x00003240
		public static string HtcProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "HTC\\Packages\\Products\\");
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00005068 File Offset: 0x00003268
		public static string LgePackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LGE\\Packages\\");
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00005090 File Offset: 0x00003290
		public static string BluPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BLU\\Packages\\");
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000050B8 File Offset: 0x000032B8
		public static string BluProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BLU\\Packages\\Products\\");
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000050E0 File Offset: 0x000032E0
		public static string LgeProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LGE\\Packages\\Products\\");
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00005108 File Offset: 0x00003308
		public static string McjPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MCJ\\Packages\\");
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00005130 File Offset: 0x00003330
		public static string McjProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MCJ\\Packages\\Products\\");
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005158 File Offset: 0x00003358
		public static string AlcatelPackagesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Alcatel\\Packages");
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005180 File Offset: 0x00003380
		public static string AlcatelProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Alcatel\\Packages\\Products\\");
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000051A8 File Offset: 0x000033A8
		public static string AcerProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Acer\\Packages\\Products\\");
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000051D0 File Offset: 0x000033D0
		public static string TrinityProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Trinity\\Packages\\Products\\");
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000051F8 File Offset: 0x000033F8
		public static string UnistrongProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Unistrong\\Packages\\Products\\");
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00005220 File Offset: 0x00003420
		public static string YEZZProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "YEZZ\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00005248 File Offset: 0x00003448
		public static string DiginnosProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Diginnos\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00005270 File Offset: 0x00003470
		public static string VAIOProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VAIO\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00005298 File Offset: 0x00003498
		public static string InversenetProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Inversenet\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000052C0 File Offset: 0x000034C0
		public static string FreetelProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Freetel\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000052E8 File Offset: 0x000034E8
		public static string FunkerProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Funker\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00005310 File Offset: 0x00003510
		public static string MicromaxProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Micromax\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00005338 File Offset: 0x00003538
		public static string XOLOProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "XOLO\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00005360 File Offset: 0x00003560
		public static string KMProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "KM\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00005388 File Offset: 0x00003588
		public static string JenesisProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Jenesis\\Packages\\Products\\");
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000053B0 File Offset: 0x000035B0
		public static string GomobileProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gomobile\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x000053D8 File Offset: 0x000035D8
		public static string HPProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "HP\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00005400 File Offset: 0x00003600
		public static string LenovoProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Lenovo\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00005428 File Offset: 0x00003628
		public static string ZebraProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Zebra\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00005450 File Offset: 0x00003650
		public static string HoneywellProductPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Honeywell\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00005478 File Offset: 0x00003678
		public static string PanasonicProductPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Panasonic\\Packages\\Products\\");
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x000054A0 File Offset: 0x000036A0
		public static string TrekStorProductPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TrekStor\\Packages\\Products\\");
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x000054C8 File Offset: 0x000036C8
		public static string WileyfoxProductsPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Wilefox\\Packages\\Products\\");
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x000054F0 File Offset: 0x000036F0
		public static string AppPath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000550C File Offset: 0x0000370C
		public static string AppDataPath(SpecialFolder specialFolder)
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft\\Care Suite\\Windows Device Recovery Tool");
			switch (specialFolder)
			{
			case SpecialFolder.Traces:
				text = Path.Combine(text, "Traces");
				break;
			case SpecialFolder.Reports:
				text = Path.Combine(text, "Reports");
				break;
			case SpecialFolder.Exports:
				text = Path.Combine(text, "Exports");
				break;
			case SpecialFolder.AppUpdate:
				text = Path.Combine(text, "Updates");
				break;
			case SpecialFolder.Configurations:
				text = Path.Combine(text, "Configuration");
				break;
			}
			text += "\\";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000055B8 File Offset: 0x000037B8
		public static long GetDirectorySize(string directory)
		{
			long result;
			if (!Directory.Exists(directory))
			{
				result = 0L;
			}
			else
			{
				string[] files = Directory.GetFiles(Environment.ExpandEnvironmentVariables(directory), "*.*", SearchOption.AllDirectories);
				long num = 0L;
				foreach (string fileName in files)
				{
					FileInfo fileInfo = new FileInfo(fileName);
					try
					{
						num += fileInfo.Length;
					}
					catch (FileNotFoundException)
					{
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00005644 File Offset: 0x00003844
		public static bool CheckIfPathIsValid(string path)
		{
			return Directory.Exists(path) && !path.Contains("..");
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000567C File Offset: 0x0000387C
		public static bool CheckIfFileIsValid(string filePath)
		{
			return File.Exists(filePath);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000056A0 File Offset: 0x000038A0
		public static void CheckAndCreateFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				File.Create(filePath);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000056C4 File Offset: 0x000038C4
		public static void CheckAndCreatePath(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000056E8 File Offset: 0x000038E8
		public static bool CheckDirectoryWritePermission(string path)
		{
			bool result;
			try
			{
				using (File.Create(Path.Combine(FileSystemInfo.CustomPackagesPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
				{
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00005750 File Offset: 0x00003950
		public static bool CheckPermission(string path)
		{
			bool result = false;
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			FileIOPermission perm = new FileIOPermission(FileIOPermissionAccess.Write, path);
			permissionSet.AddPermission(perm);
			if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000579C File Offset: 0x0000399C
		public static string GetLumiaPackagesPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				result = Path.Combine(FileSystemInfo.CustomPackagesPath, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.DefaultPackagesPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000057D8 File Offset: 0x000039D8
		public static string GetLumiaProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.DefaultProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00005820 File Offset: 0x00003A20
		public static string GetHtcPackagesPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				result = Path.Combine(FileSystemInfo.CustomPackagesPath, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.HtcPackagesPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000585C File Offset: 0x00003A5C
		public static string GetHtcProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.HtcProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000058A4 File Offset: 0x00003AA4
		public static string GetLgePackagesPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				result = Path.Combine(FileSystemInfo.CustomPackagesPath, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.LgePackagesPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000058E0 File Offset: 0x00003AE0
		public static string GetLgeProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.LgeProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00005928 File Offset: 0x00003B28
		public static string GetMcjPackagesPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				result = Path.Combine(FileSystemInfo.CustomPackagesPath, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.McjPackagesPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00005964 File Offset: 0x00003B64
		public static string GetMcjProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.McjProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000059AC File Offset: 0x00003BAC
		public static string GetBluProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.BluProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000059F4 File Offset: 0x00003BF4
		public static string GetAlcatelProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.AlcatelProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00005A3C File Offset: 0x00003C3C
		public static string GetAcerProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.AcerProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00005A84 File Offset: 0x00003C84
		public static string GetTrinityProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.TrinityProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00005ACC File Offset: 0x00003CCC
		public static string GetUnistrongProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.UnistrongProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00005B14 File Offset: 0x00003D14
		public static string GetYEZZProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.YEZZProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00005B5C File Offset: 0x00003D5C
		public static string GetDiginnosProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.DiginnosProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00005BA4 File Offset: 0x00003DA4
		public static string GetVAIOProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.VAIOProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00005BEC File Offset: 0x00003DEC
		public static string GetInversenetProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.InversenetProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00005C34 File Offset: 0x00003E34
		public static string GetFreetelProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.FreetelProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00005C7C File Offset: 0x00003E7C
		public static string GetFunkerProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.FunkerProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public static string GetMicromaxProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.MicromaxProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00005D0C File Offset: 0x00003F0C
		public static string GetXOLOProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.XOLOProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00005D54 File Offset: 0x00003F54
		public static string GetKMProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.KMProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00005D9C File Offset: 0x00003F9C
		public static string GetJenesisProdcutsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.JenesisProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00005DE4 File Offset: 0x00003FE4
		public static string GetGomobileProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.GomobileProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00005E2C File Offset: 0x0000402C
		public static string GetHPProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.HPProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00005E74 File Offset: 0x00004074
		public static string GetLenovoProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.LenovoProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00005EBC File Offset: 0x000040BC
		public static string GetZebraProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.ZebraProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00005F04 File Offset: 0x00004104
		public static string GetHoneywellProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.HoneywellProductPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00005F4C File Offset: 0x0000414C
		public static string GetPanasonicProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.PanasonicProductPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00005F94 File Offset: 0x00004194
		public static string GetTrekStorProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.TrekStorProductPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00005FDC File Offset: 0x000041DC
		public static string GetWileyfoxProductsPath(string productPath = "")
		{
			string result;
			if (!string.IsNullOrWhiteSpace(FileSystemInfo.CustomPackagesPath))
			{
				string path = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
				result = Path.Combine(path, productPath);
			}
			else
			{
				result = Path.Combine(FileSystemInfo.WileyfoxProductsPath, productPath);
			}
			return result;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00006024 File Offset: 0x00004224
		public static string GetCustomProductsPath()
		{
			string result;
			if (!string.IsNullOrEmpty(FileSystemInfo.CustomPackagesPath))
			{
				result = Path.Combine(FileSystemInfo.CustomPackagesPath, "Products");
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
