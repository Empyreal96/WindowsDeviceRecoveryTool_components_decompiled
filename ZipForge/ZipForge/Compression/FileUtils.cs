using System;
using System.IO;
using System.Management;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression
{
	// Token: 0x0200002A RID: 42
	internal class FileUtils
	{
		// Token: 0x060001DC RID: 476 RVA: 0x000150C8 File Offset: 0x000140C8
		public static string GetCurrentDirectory()
		{
			return FileUtils._currentDirectory;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000150CF File Offset: 0x000140CF
		public static void SetCurrentDirectory(string path)
		{
			FileUtils._currentDirectory = Path.GetFullPath(path);
			if (FileUtils._currentDirectory.EndsWith("\\"))
			{
				FileUtils._currentDirectory = FileUtils._currentDirectory.Remove(FileUtils._currentDirectory.Length - 1, 1);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00015109 File Offset: 0x00014109
		public static string GetFullPath(string path)
		{
			if (!Path.IsPathRooted(path))
			{
				return CompressionUtils.IncludeTrailingBackslash(FileUtils.GetCurrentDirectory()) + path;
			}
			return path;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00015128 File Offset: 0x00014128
		public static FileAttributes GetAttributes(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			return fileInfo.Attributes;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00015142 File Offset: 0x00014142
		public static void SetFileLastWriteTime(string fileName, DateTime dateTime)
		{
			new FileInfo(fileName).LastWriteTime = dateTime;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00015150 File Offset: 0x00014150
		public static string StripSlash(string path)
		{
			if (!path.EndsWith("\\"))
			{
				return path;
			}
			return path.Substring(0, path.Length - 1);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00015170 File Offset: 0x00014170
		public static bool DirectotyExists(string path)
		{
			return Directory.Exists(FileUtils.StripSlash(path));
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0001517D File Offset: 0x0001417D
		public static int GetFileOwnerId(string filePath)
		{
			return 0;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00015180 File Offset: 0x00014180
		public static string GetFileOwnerAccount(string filePath)
		{
			string result;
			try
			{
				ManagementObject managementObject = new ManagementObject("Win32_LogicalFileSecuritySetting.path='" + filePath + "'");
				ManagementBaseObject managementBaseObject = managementObject.InvokeMethod("GetSecurityDescriptor", null, null);
				ManagementBaseObject managementBaseObject2 = managementBaseObject.Properties["Descriptor"].Value as ManagementBaseObject;
				ManagementBaseObject managementBaseObject3 = managementBaseObject2.Properties["Owner"].Value as ManagementBaseObject;
				result = managementBaseObject3.Properties["Domain"].Value.ToString() + "\\" + managementBaseObject3.Properties["Name"].Value.ToString();
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00015248 File Offset: 0x00014248
		public static int GetFileOwnerGroupId(string filePath)
		{
			return 0;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0001524B File Offset: 0x0001424B
		public static string GetFileOwnerGroupAccount(string filePath)
		{
			return string.Empty;
		}

		// Token: 0x04000131 RID: 305
		private static string _currentDirectory = Directory.GetCurrentDirectory();
	}
}
