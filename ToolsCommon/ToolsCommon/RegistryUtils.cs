using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000033 RID: 51
	public static class RegistryUtils
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000852C File Offset: 0x0000672C
		public static Dictionary<SystemRegistryHiveFiles, string> KnownMountPoints
		{
			get
			{
				return RegistryUtils.MountPoints;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008534 File Offset: 0x00006734
		public static void ConvertSystemHiveToRegFile(DriveInfo systemDrive, SystemRegistryHiveFiles hive, string outputRegFile)
		{
			string directoryName = LongPath.GetDirectoryName(outputRegFile);
			LongPathDirectory.CreateDirectory(directoryName);
			string path = Path.Combine(systemDrive.RootDirectory.FullName, "windows\\system32\\config");
			string inputhive = Path.Combine(path, Enum.GetName(typeof(SystemRegistryHiveFiles), hive));
			RegistryUtils.ConvertHiveToRegFile(inputhive, RegistryUtils.MapHiveToMountPoint(hive), outputRegFile);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000858D File Offset: 0x0000678D
		public static void ConvertHiveToRegFile(string inputhive, string targetRootKey, string outputRegFile)
		{
			OfflineRegUtils.ConvertHiveToReg(inputhive, outputRegFile, targetRootKey);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008598 File Offset: 0x00006798
		public static void LoadHive(string inputhive, string mountpoint)
		{
			string args = string.Format("LOAD {0} {1}", mountpoint, inputhive);
			string command = Environment.ExpandEnvironmentVariables("%windir%\\System32\\REG.EXE");
			int num = CommonUtils.RunProcess(Environment.CurrentDirectory, command, args, true);
			if (0 < num)
			{
				throw new Win32Exception(num);
			}
			Thread.Sleep(500);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000085E0 File Offset: 0x000067E0
		public static void ExportHive(string mountpoint, string outputfile)
		{
			string args = string.Format("EXPORT {0} {1}", mountpoint, outputfile);
			string command = Environment.ExpandEnvironmentVariables("%windir%\\System32\\REG.EXE");
			int num = CommonUtils.RunProcess(Environment.CurrentDirectory, command, args, true);
			if (0 < num)
			{
				throw new Win32Exception(num);
			}
			Thread.Sleep(500);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008628 File Offset: 0x00006828
		public static void UnloadHive(string mountpoint)
		{
			string args = string.Format("UNLOAD {0}", mountpoint);
			string command = Environment.ExpandEnvironmentVariables("%windir%\\System32\\REG.EXE");
			int num = CommonUtils.RunProcess(Environment.CurrentDirectory, command, args, true);
			if (0 < num)
			{
				throw new Win32Exception(num);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008665 File Offset: 0x00006865
		public static string MapHiveToMountPoint(SystemRegistryHiveFiles hive)
		{
			return RegistryUtils.KnownMountPoints[hive];
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008674 File Offset: 0x00006874
		public static string MapHiveFileToMountPoint(string hiveFile)
		{
			if (string.IsNullOrEmpty(hiveFile))
			{
				throw new InvalidOperationException("hiveFile cannot be empty");
			}
			SystemRegistryHiveFiles hive;
			if (!RegistryUtils.hiveMap.TryGetValue(Path.GetFileName(hiveFile), out hive))
			{
				return "";
			}
			return RegistryUtils.MapHiveToMountPoint(hive);
		}

		// Token: 0x040000CF RID: 207
		private const string STR_REG_LOAD = "LOAD {0} {1}";

		// Token: 0x040000D0 RID: 208
		private const string STR_REG_EXPORT = "EXPORT {0} {1}";

		// Token: 0x040000D1 RID: 209
		private const string STR_REG_UNLOAD = "UNLOAD {0}";

		// Token: 0x040000D2 RID: 210
		private const string STR_REGEXE = "%windir%\\System32\\REG.EXE";

		// Token: 0x040000D3 RID: 211
		private static Dictionary<string, SystemRegistryHiveFiles> hiveMap = new Dictionary<string, SystemRegistryHiveFiles>(StringComparer.InvariantCultureIgnoreCase)
		{
			{
				"SOFTWARE",
				SystemRegistryHiveFiles.SOFTWARE
			},
			{
				"SYSTEM",
				SystemRegistryHiveFiles.SYSTEM
			},
			{
				"DRIVERS",
				SystemRegistryHiveFiles.DRIVERS
			},
			{
				"DEFAULT",
				SystemRegistryHiveFiles.DEFAULT
			},
			{
				"SAM",
				SystemRegistryHiveFiles.SAM
			},
			{
				"SECURITY",
				SystemRegistryHiveFiles.SECURITY
			},
			{
				"BCD",
				SystemRegistryHiveFiles.BCD
			},
			{
				"NTUSER.DAT",
				SystemRegistryHiveFiles.CURRENTUSER
			}
		};

		// Token: 0x040000D4 RID: 212
		private static readonly Dictionary<SystemRegistryHiveFiles, string> MountPoints = new Dictionary<SystemRegistryHiveFiles, string>
		{
			{
				SystemRegistryHiveFiles.SOFTWARE,
				"HKEY_LOCAL_MACHINE\\SOFTWARE"
			},
			{
				SystemRegistryHiveFiles.SYSTEM,
				"HKEY_LOCAL_MACHINE\\SYSTEM"
			},
			{
				SystemRegistryHiveFiles.DRIVERS,
				"HKEY_LOCAL_MACHINE\\DRIVERS"
			},
			{
				SystemRegistryHiveFiles.DEFAULT,
				"HKEY_USERS\\.DEFAULT"
			},
			{
				SystemRegistryHiveFiles.SAM,
				"HKEY_LOCAL_MACHINE\\SAM"
			},
			{
				SystemRegistryHiveFiles.SECURITY,
				"HKEY_LOCAL_MACHINE\\SECURITY"
			},
			{
				SystemRegistryHiveFiles.BCD,
				"HKEY_LOCAL_MACHINE\\BCD"
			},
			{
				SystemRegistryHiveFiles.CURRENTUSER,
				"HKEY_CURRENT_USER"
			}
		};
	}
}
