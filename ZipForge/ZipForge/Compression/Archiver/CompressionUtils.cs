using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200006E RID: 110
	internal class CompressionUtils
	{
		// Token: 0x060004A2 RID: 1186 RVA: 0x00020B95 File Offset: 0x0001FB95
		public static bool IsNullOrEmpty(string str)
		{
			return str == null || str == string.Empty;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00020BA8 File Offset: 0x0001FBA8
		public static void Memset(ref byte[] p, byte b, int count)
		{
			if (p == null)
			{
				return;
			}
			int num = (p.Length < count) ? p.Length : count;
			for (int i = 0; i < num; i++)
			{
				p[i] = b;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00020BDA File Offset: 0x0001FBDA
		public static void Memcpy(ref byte[] dest, byte[] source, int count)
		{
			if (dest == null || source == null)
			{
				return;
			}
			Array.Copy(source, 0, dest, 0, (dest.Length < count) ? dest.Length : count);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00020BFC File Offset: 0x0001FBFC
		public static string GetSlashedDir(string dir)
		{
			if (dir == string.Empty)
			{
				return FileUtils.GetCurrentDirectory() + Path.PathSeparator;
			}
			return CompressionUtils.IncludeTrailingBackslash(dir);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00020C28 File Offset: 0x0001FC28
		private static string GetFileSystemCasing(string path)
		{
			if (Path.IsPathRooted(path))
			{
				path = path.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				});
				try
				{
					string fileName = Path.GetFileName(path);
					if (fileName == "")
					{
						return path.ToUpper() + Path.DirectorySeparatorChar;
					}
					string text = Path.GetDirectoryName(path);
					text = CompressionUtils.GetFileSystemCasing(text);
					DirectoryInfo directoryInfo = new DirectoryInfo(text);
					FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos(fileName);
					if (fileSystemInfos.Length > 0)
					{
						FileSystemInfo fileSystemInfo = fileSystemInfos[0];
						return fileSystemInfo.FullName;
					}
					return Path.Combine(text, fileName);
				}
				catch
				{
					throw new ArgumentException("Invalid path");
				}
			}
			throw new ArgumentException("Absolute path needed, not relative");
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00020CF0 File Offset: 0x0001FCF0
		public static string GetFullFileName(string baseDir, string fileName)
		{
			string text;
			if (baseDir == string.Empty)
			{
				text = FileUtils.GetCurrentDirectory();
			}
			else
			{
				text = CompressionUtils.IncludeTrailingBackslash(baseDir);
				if (!Path.IsPathRooted(baseDir))
				{
					text = Path.GetFullPath(text);
				}
				try
				{
					text = ((text.IndexOf('/') > -1) ? CompressionUtils.GetFileSystemCasing(text.Replace('/', '\\')).Replace('\\', '/') : CompressionUtils.GetFileSystemCasing(text));
					text = CompressionUtils.IncludeTrailingBackslash(text);
				}
				catch
				{
				}
			}
			string text2;
			if (fileName == "")
			{
				if (baseDir.IndexOf(':') > 0)
				{
					try
					{
						text2 = ((baseDir.IndexOf('/') > -1) ? CompressionUtils.GetFileSystemCasing(baseDir.Replace('/', '\\')).Replace('\\', '/') : CompressionUtils.GetFileSystemCasing(baseDir));
						if (baseDir.EndsWith("\\") || baseDir.EndsWith("/"))
						{
							text2 = CompressionUtils.IncludeTrailingBackslash(text2);
						}
						return text2;
					}
					catch
					{
						return baseDir;
					}
				}
				if (FileUtils.DirectotyExists(text) || baseDir == "")
				{
					return text;
				}
				return baseDir;
			}
			else
			{
				lock (typeof(FileUtils))
				{
					string currentDirectory = FileUtils.GetCurrentDirectory();
					FileUtils.SetCurrentDirectory(text);
					text2 = FileUtils.GetFullPath(fileName);
					FileUtils.SetCurrentDirectory(currentDirectory);
				}
			}
			return text2;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00020E44 File Offset: 0x0001FE44
		public static string RemoveEscapeSequences(string source)
		{
			StringBuilder stringBuilder = new StringBuilder(source);
			stringBuilder = stringBuilder.Replace("\r", " ");
			stringBuilder = stringBuilder.Replace("\n", " ");
			stringBuilder = stringBuilder.Replace("\f", " ");
			stringBuilder = stringBuilder.Replace("\t", " ");
			stringBuilder = stringBuilder.Replace("\v", " ");
			return stringBuilder.ToString();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00020EB4 File Offset: 0x0001FEB4
		public static string ExtractFileDrive(string path)
		{
			string pathRoot = Path.GetPathRoot(path);
			if (pathRoot.Length <= 0)
			{
				return string.Empty;
			}
			return pathRoot.Substring(0, pathRoot.Length - 1);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00020EE8 File Offset: 0x0001FEE8
		public static string ExtractRelativePath(string basePath, string path)
		{
			if (!Path.IsPathRooted(path) || !Path.IsPathRooted(basePath))
			{
				return path;
			}
			string directoryName = Path.GetDirectoryName(FileUtils.GetFullPath(basePath));
			string directoryName2 = Path.GetDirectoryName(FileUtils.GetFullPath(path));
			string pathRoot = Path.GetPathRoot(directoryName);
			string pathRoot2 = Path.GetPathRoot(directoryName2);
			if (pathRoot != pathRoot2)
			{
				return path;
			}
			string[] array = directoryName.Split("\\/".ToCharArray());
			string[] array2 = directoryName2.Split("\\/".ToCharArray());
			StringBuilder stringBuilder = new StringBuilder();
			int i;
			for (i = 0; i < array.Length; i++)
			{
				if (i >= array2.Length)
				{
					stringBuilder.Append("..\\");
				}
				else if (!array[i].Equals(array2[i]))
				{
					stringBuilder.Append("..\\");
					break;
				}
			}
			for (int j = i; j < array2.Length; j++)
			{
				stringBuilder.Append(array2[j]);
				stringBuilder.Append('\\');
			}
			stringBuilder.Append(Path.GetFileName(path));
			return stringBuilder.ToString();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00020FF0 File Offset: 0x0001FFF0
		public static bool MatchesMask(string fileName, string fileMask)
		{
			if (fileMask == "*.*")
			{
				return true;
			}
			string text = "^" + Regex.Escape(fileMask);
			text = text.Replace("\\*", ".*");
			text = text.Replace("\\?", ".");
			text += "$";
			Regex regex = new Regex(text.ToLower());
			return regex.IsMatch(fileName.ToLower());
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00021064 File Offset: 0x00020064
		public static void FileSetAttr(string fileName, FileAttributes attr)
		{
			try
			{
				new FileInfo(fileName).Attributes = attr;
			}
			catch (ArgumentException)
			{
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00021094 File Offset: 0x00020094
		public static byte[] StringToByteArray(string from, int oemCodePage)
		{
			return Encoding.GetEncoding(oemCodePage).GetBytes(from);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000210A2 File Offset: 0x000200A2
		public static string ByteArrayToString(byte[] from, int oemCodePage)
		{
			return Encoding.GetEncoding(oemCodePage).GetString(from, 0, from.Length);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000210B4 File Offset: 0x000200B4
		public static void CopyStream(Stream input, Stream output)
		{
			lock (CompressionUtils._tempBuffer)
			{
				int count;
				while ((count = input.Read(CompressionUtils._tempBuffer, 0, 2048)) > 0)
				{
					output.Write(CompressionUtils._tempBuffer, 0, count);
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0002110C File Offset: 0x0002010C
		public static void CopyStream(Stream input, Stream output, long count)
		{
			lock (CompressionUtils._tempBuffer)
			{
				long num = 0L;
				long val;
				while ((val = (long)input.Read(CompressionUtils._tempBuffer, 0, 2048)) > 0L && num < count)
				{
					output.Write(CompressionUtils._tempBuffer, 0, (int)Math.Min(val, count - num));
					num += Math.Min(val, count);
				}
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00021180 File Offset: 0x00020180
		public static void CopyStream(Stream input, long offset, Stream output, long count)
		{
			lock (CompressionUtils._tempBuffer)
			{
				long num = 0L;
				long val;
				while ((val = (long)input.Read(CompressionUtils._tempBuffer, (int)offset, 2048)) > 0L && num < count)
				{
					output.Write(CompressionUtils._tempBuffer, 0, (int)Math.Min(val, count - num));
					num += Math.Min(val, count);
				}
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000211F4 File Offset: 0x000201F4
		public static string IncludeTrailingBackslash(string s)
		{
			if (s == null)
			{
				return "\\";
			}
			char c = s[s.Length - 1];
			if (c == '\\' || c == '/')
			{
				return s;
			}
			if (s.IndexOf("/") > -1)
			{
				return s + '/';
			}
			return s + '\\';
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00021250 File Offset: 0x00020250
		public static string ExcludeTrailingBackslash(string s)
		{
			if (!CompressionUtils.IsNullOrEmpty(s))
			{
				char c = s[s.Length - 1];
				if (c == '\\' || c == '/')
				{
					return s.Substring(0, s.Length - 1);
				}
			}
			return s;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00021290 File Offset: 0x00020290
		public static uint DateTimeToDosDateTime(DateTime dt)
		{
			int num = dt.Second >> 1 | dt.Minute << 5 | dt.Hour << 11;
			int num2 = (dt.Year - 1980 & 127) << 9 | dt.Month << 5 | dt.Day;
			return (uint)(num2 << 16 | num);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000212E8 File Offset: 0x000202E8
		public static DateTime DosDateTimeToDateTime(uint dosDateTime)
		{
			int num = (int)(dosDateTime >> 16);
			int num2 = (int)(dosDateTime & 65535U);
			DateTime result;
			try
			{
				result = new DateTime((num >> 9) + 1980, (num >> 5) % 16, num % 32, num2 >> 11, (num2 >> 5) % 64, num2 % 32 * 2);
			}
			catch
			{
				result = DateTime.Now;
			}
			return result;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00021348 File Offset: 0x00020348
		public static string GetArchiveFileName(string fileName, string baseDir, StorePathMode storePathMode)
		{
			string text = string.Empty;
			string fullFileName = CompressionUtils.GetFullFileName(baseDir.Replace("\\", "/"), "");
			string text2 = CompressionUtils.GetFullFileName(fullFileName, fileName.Replace("\\", "/"));
			if (text2 != "" && (text2[text2.Length - 1] == '\\' || text2[text2.Length - 1] == '/'))
			{
				text2 = text2.Remove(text2.Length - 1, 1);
			}
			if (text2 != "" && text2[text2.Length - 1] == '\\')
			{
				text2 = text2.Remove(text2.Length - 1, 1);
			}
			switch (storePathMode)
			{
			case StorePathMode.NoPath:
				text2 = Path.GetFileName(text2);
				break;
			case StorePathMode.RelativePath:
			{
				string text3 = CompressionUtils.ExtractRelativePath(CompressionUtils.GetSlashedDir(fullFileName), text2);
				if (text3.IndexOf("..\\") < 0 && text3.IndexOf("../") < 0)
				{
					text2 = text3;
				}
				break;
			}
			case StorePathMode.FullPath:
			{
				string text4 = (text2.Substring(1, 2) == ":\\") ? text2.Substring(3) : text2;
				if (text4.IndexOf("..\\") < 0 && text4.IndexOf("../") < 0)
				{
					text2 = text4;
				}
				break;
			}
			}
			int i = 0;
			int length = text2.Length;
			if (storePathMode != StorePathMode.FullPathWithDrive)
			{
				for (int j = 0; j < length; j++)
				{
					if (text2[j] == ':')
					{
						i = j + 1;
						break;
					}
				}
				if (i >= length)
				{
					return text;
				}
				if (i == 0)
				{
					for (int j = 0; j < length - 1; j++)
					{
						if ((text2[j] == '/' || text2[j] == '\\') && (text2[j + 1] == '/' || text2[j + 1] == '\\'))
						{
							i = j + 2;
							for (int k = i; k < length; k++)
							{
								if (text2[k] == '/' || text2[k] == '\\')
								{
									i = k + 1;
									break;
								}
							}
							break;
						}
					}
				}
				if (text2[i] == '\\' || text2[i] == '/')
				{
					i++;
				}
			}
			while (i < length)
			{
				char c = text2[i];
				if (c == '\\')
				{
					c = '/';
				}
				text += c;
				i++;
			}
			return text;
		}

		// Token: 0x040002C8 RID: 712
		public const int TransferBlockSize = 2048;

		// Token: 0x040002C9 RID: 713
		internal static byte[] _tempBuffer = new byte[2048];
	}
}
