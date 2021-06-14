using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000058 RID: 88
	public static class LongPath
	{
		// Token: 0x06000205 RID: 517 RVA: 0x0000A388 File Offset: 0x00008588
		public static string GetDirectoryName(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new ArgumentException("path");
			}
			int num = path.LastIndexOfAny(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.VolumeSeparatorChar
			});
			if (num == -1)
			{
				return null;
			}
			return path.Substring(0, num);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A3EC File Offset: 0x000085EC
		public static string GetFullPath(string path)
		{
			string text = LongPathCommon.NormalizeLongPath(path);
			if (text.StartsWith("\\\\?\\UNC\\"))
			{
				return "\\\\" + text.Substring("\\\\?\\UNC\\".Length);
			}
			if (text.StartsWith("\\\\?\\"))
			{
				return text.Substring("\\\\?\\".Length);
			}
			return text;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A447 File Offset: 0x00008647
		public static string GetFullPathUNC(string path)
		{
			return LongPathCommon.NormalizeLongPath(path);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A450 File Offset: 0x00008650
		public static string GetPathRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			if (path == string.Empty || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new ArgumentException("path");
			}
			if (!Path.IsPathRooted(path))
			{
				return string.Empty;
			}
			if (path.StartsWith("\\\\"))
			{
				int num = path.IndexOf(Path.DirectorySeparatorChar, "\\\\".Length);
				if (num == -1)
				{
					return path;
				}
				int num2 = path.IndexOf(Path.DirectorySeparatorChar, num + 1);
				if (num2 == -1)
				{
					return path;
				}
				return path.Substring(0, num2);
			}
			else
			{
				int num3 = path.IndexOf(Path.VolumeSeparatorChar);
				if (num3 != 1)
				{
					return string.Empty;
				}
				if (path.Length <= 2 || path[2] != Path.DirectorySeparatorChar)
				{
					return path.Substring(0, 2);
				}
				return path.Substring(0, 3);
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000A51C File Offset: 0x0000871C
		public static string Combine(string path, string file)
		{
			return string.Format("{0}\\{1}", path.TrimEnd(new char[]
			{
				'\\'
			}), file.Trim(new char[]
			{
				'\\'
			}));
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A55C File Offset: 0x0000875C
		public static string GetFileName(string path)
		{
			return Regex.Match(path, "\\\\[^\\\\]+$").Value.TrimStart(new char[]
			{
				'\\'
			});
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A590 File Offset: 0x00008790
		public static string GetExtension(string path)
		{
			if (path == null)
			{
				return null;
			}
			string text = Regex.Match(path.ToLowerInvariant(), "\\.[^\\.]+$").Value.TrimStart(new char[]
			{
				'.'
			});
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			return "." + text;
		}

		// Token: 0x0400015B RID: 347
		private const string UNC_PREFIX = "\\\\";

		// Token: 0x0400015C RID: 348
		private const string LONGPATH_PREFIX = "\\\\?\\";

		// Token: 0x0400015D RID: 349
		private const string LONGPATH_UNC_PREFIX = "\\\\?\\UNC\\";
	}
}
