using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000028 RID: 40
	public static class FileUtils
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00007C9D File Offset: 0x00005E9D
		public static string RerootPath(string path, string oldRoot, string newRoot)
		{
			if (oldRoot.Last<char>() != '\\')
			{
				oldRoot += '\\';
			}
			if (newRoot.Last<char>() != '\\')
			{
				newRoot += '\\';
			}
			return path.Replace(oldRoot, newRoot);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007CD9 File Offset: 0x00005ED9
		public static string GetTempFile()
		{
			return FileUtils.GetTempFile(Path.GetTempPath());
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007CE5 File Offset: 0x00005EE5
		public static string GetTempFile(string dir)
		{
			return Path.Combine(dir, Path.GetRandomFileName());
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007CF2 File Offset: 0x00005EF2
		public static void DeleteTree(string dirPath)
		{
			if (string.IsNullOrEmpty(dirPath))
			{
				throw new ArgumentException("Empty directory path");
			}
			if (LongPathFile.Exists(dirPath))
			{
				throw new IOException(string.Format("Cannot delete directory {0}, it's a file", dirPath));
			}
			if (!LongPathDirectory.Exists(dirPath))
			{
				return;
			}
			LongPathDirectory.Delete(dirPath, true);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007D30 File Offset: 0x00005F30
		public static void DeleteFile(string filePath)
		{
			if (!LongPathFile.Exists(filePath))
			{
				return;
			}
			LongPathFile.SetAttributes(filePath, FileAttributes.Normal);
			LongPathFile.Delete(filePath);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007D4C File Offset: 0x00005F4C
		public static void CleanDirectory(string dirPath)
		{
			if (string.IsNullOrEmpty(dirPath))
			{
				throw new ArgumentException("Empty directory path");
			}
			if (LongPathFile.Exists(dirPath))
			{
				throw new IOException(string.Format("Cannot create directory {0}, a file with same name exists", dirPath));
			}
			NativeMethods.IU_CleanDirectory(dirPath, false);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007D84 File Offset: 0x00005F84
		public static string GetTempDirectory()
		{
			string text;
			do
			{
				text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			}
			while (LongPathDirectory.Exists(text));
			LongPathDirectory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public static bool IsTargetUpToDate(string inputFile, string targetFile)
		{
			if (!LongPathFile.Exists(targetFile))
			{
				return false;
			}
			DateTime lastWriteTimeUtc = new FileInfo(targetFile).LastWriteTimeUtc;
			DateTime lastWriteTimeUtc2 = new FileInfo(inputFile).LastWriteTimeUtc;
			return !(lastWriteTimeUtc2 > lastWriteTimeUtc);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007DEC File Offset: 0x00005FEC
		public static string GetFileVersion(string filepath)
		{
			string result = string.Empty;
			if (LongPathFile.Exists(filepath))
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filepath);
				result = versionInfo.FileVersion;
			}
			return result;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007E16 File Offset: 0x00006016
		public static string GetCurrentAssemblyFileVersion()
		{
			return FileUtils.GetFileVersion(Process.GetCurrentProcess().MainModule.FileName);
		}

		// Token: 0x0600013D RID: 317
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath, uint cchBuffer);

		// Token: 0x0600013E RID: 318 RVA: 0x00007E2C File Offset: 0x0000602C
		public static string GetShortPathName(string dirPath)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			FileUtils.GetShortPathName(dirPath, stringBuilder, 260U);
			if (stringBuilder.Length == 0)
			{
				return dirPath;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007E64 File Offset: 0x00006064
		public static void CopyDirectory(string source, string destination)
		{
			LongPathDirectory.CreateDirectory(destination);
			foreach (string text in LongPathDirectory.GetFiles(source))
			{
				LongPathFile.Copy(text, Path.Combine(destination, Path.GetFileName(text)));
			}
			foreach (string text2 in LongPathDirectory.GetDirectories(source))
			{
				FileUtils.CopyDirectory(text2, Path.Combine(destination, Path.GetFileName(text2)));
			}
		}

		// Token: 0x04000075 RID: 117
		public const int MAX_PATH = 260;
	}
}
