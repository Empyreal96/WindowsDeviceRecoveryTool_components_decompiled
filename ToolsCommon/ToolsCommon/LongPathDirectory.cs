using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000059 RID: 89
	public static class LongPathDirectory
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000A5E4 File Offset: 0x000087E4
		public static void CreateDirectory(string path)
		{
			try
			{
				NativeMethods.IU_EnsureDirectoryExists(path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A60C File Offset: 0x0000880C
		public static void Delete(string path)
		{
			string text = LongPathCommon.NormalizeLongPath(path);
			if (!LongPathDirectory.Exists(text))
			{
				return;
			}
			if (!NativeMethods.RemoveDirectory(text))
			{
				throw LongPathCommon.GetExceptionFromLastWin32Error();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A637 File Offset: 0x00008837
		public static void Delete(string path, bool recursive)
		{
			if (recursive)
			{
				NativeMethods.IU_CleanDirectory(path, true);
				return;
			}
			LongPathDirectory.Delete(path);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A64A File Offset: 0x0000884A
		public static bool Exists(string path)
		{
			return NativeMethods.IU_DirectoryExists(path);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000A654 File Offset: 0x00008854
		public static FileAttributes GetAttributes(string path)
		{
			FileAttributes attributes = LongPathCommon.GetAttributes(path);
			if (!attributes.HasFlag(FileAttributes.Directory))
			{
				throw LongPathCommon.GetExceptionFromWin32Error(267);
			}
			return attributes;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A688 File Offset: 0x00008888
		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOptions)
		{
			return LongPathDirectory.GetDirectories(path, searchPattern, searchOptions);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A692 File Offset: 0x00008892
		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
		{
			return LongPathDirectory.EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A69C File Offset: 0x0000889C
		public static IEnumerable<string> EnumerateDirectories(string path)
		{
			return LongPathDirectory.EnumerateDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOptions)
		{
			if (searchOptions != SearchOption.AllDirectories && searchOptions != SearchOption.TopDirectoryOnly)
			{
				throw new NotImplementedException("Unknown search option: " + searchOptions);
			}
			bool fRecursive = searchOptions == SearchOption.AllDirectories;
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			string strFolder = Path.Combine(path, Path.GetDirectoryName(searchPattern));
			string fileName = Path.GetFileName(searchPattern);
			int num2 = NativeMethods.IU_GetAllDirectories(strFolder, fileName, fRecursive, out zero, out num);
			if (num2 != 0)
			{
				throw LongPathCommon.GetExceptionFromWin32Error(num2);
			}
			string[] result;
			try
			{
				result = LongPathCommon.ConvertPtrArrayToStringArray(zero, num);
			}
			finally
			{
				NativeMethods.IU_FreeStringList(zero, num);
			}
			return result;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000A73C File Offset: 0x0000893C
		public static string[] GetDirectories(string path, string searchPattern)
		{
			return LongPathDirectory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A746 File Offset: 0x00008946
		public static string[] GetDirectories(string path)
		{
			return LongPathDirectory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A754 File Offset: 0x00008954
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOptions)
		{
			return LongPathDirectory.GetFiles(path, searchPattern, searchOptions);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A75E File Offset: 0x0000895E
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
		{
			return LongPathDirectory.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A768 File Offset: 0x00008968
		public static IEnumerable<string> EnumerateFiles(string path)
		{
			return LongPathDirectory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A778 File Offset: 0x00008978
		public static string[] GetFiles(string path, string searchPattern, SearchOption searchOptions)
		{
			if (searchOptions != SearchOption.AllDirectories && searchOptions != SearchOption.TopDirectoryOnly)
			{
				throw new NotImplementedException("Unknown search option: " + searchOptions);
			}
			bool fRecursive = searchOptions == SearchOption.AllDirectories;
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			string strFolder = Path.Combine(path, Path.GetDirectoryName(searchPattern));
			string fileName = Path.GetFileName(searchPattern);
			int num2 = NativeMethods.IU_GetAllFiles(strFolder, fileName, fRecursive, out zero, out num);
			if (num2 != 0)
			{
				throw LongPathCommon.GetExceptionFromWin32Error(num2);
			}
			string[] result;
			try
			{
				result = LongPathCommon.ConvertPtrArrayToStringArray(zero, num);
			}
			finally
			{
				NativeMethods.IU_FreeStringList(zero, num);
			}
			return result;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A808 File Offset: 0x00008A08
		public static string[] GetFiles(string path, string searchPattern)
		{
			return LongPathDirectory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A812 File Offset: 0x00008A12
		public static string[] GetFiles(string path)
		{
			return LongPathDirectory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0400015E RID: 350
		public const string ALL_FILE_PATTERN = "*.*";
	}
}
