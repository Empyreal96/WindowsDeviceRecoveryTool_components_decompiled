using System;
using System.Collections.Specialized;
using System.IO;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200001F RID: 31
	internal class CentralDirectorySearcher
	{
		// Token: 0x06000143 RID: 323 RVA: 0x0000FDEB File Offset: 0x0000EDEB
		public static bool FindFirst(ref BaseArchiveItem f, FileAttributes fileAttributes, IItemsArray cDir, bool recurse, StringCollection fileMasks, StringCollection exclusionMasks)
		{
			f.Handle.UseProperties = true;
			f.Handle.FFindAttr = (int)fileAttributes;
			f.Handle.ItemNo = 0;
			return CentralDirectorySearcher.InternalFind(ref f, cDir, recurse, fileMasks, exclusionMasks);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000FE20 File Offset: 0x0000EE20
		public static bool FindFirst(string fileMask, ref BaseArchiveItem f, IItemsArray cDir, bool recurse)
		{
			return CentralDirectorySearcher.FindFirst(fileMask, ref f, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, string.Empty, cDir, recurse);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000FE35 File Offset: 0x0000EE35
		public static bool FindFirst(string fileMask, ref BaseArchiveItem f, FileAttributes searchAttr, IItemsArray cDir, bool recurse)
		{
			return CentralDirectorySearcher.FindFirst(fileMask, ref f, searchAttr, string.Empty, cDir, recurse);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000FE48 File Offset: 0x0000EE48
		public static bool FindFirst(string fileMask, ref BaseArchiveItem f, FileAttributes searchAttr, string exclusionMask, IItemsArray cDir, bool recurse)
		{
			int length = CompressionUtils.ExtractFileDrive(fileMask).Length;
			string text = (length > 0) ? fileMask.Substring(length, fileMask.Length - length) : fileMask;
			text = text.Replace('\\', '/');
			f.Handle.CFindMask = text;
			f.Handle.ExclusionMask = exclusionMask;
			f.Handle.UseProperties = false;
			f.Handle.FWildCards = (text.IndexOf('*') >= 0 || text.IndexOf('?') >= 0);
			f.Handle.FFindAttr = (int)searchAttr;
			f.Handle.ItemNo = 0;
			return CentralDirectorySearcher.InternalFind(ref f, cDir, recurse, null, null);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000FEF8 File Offset: 0x0000EEF8
		public static bool FindNext(ref BaseArchiveItem f, IItemsArray cDir, bool recurse, StringCollection fileMasks, StringCollection exclusionMasks)
		{
			f.Handle.ItemNo++;
			return CentralDirectorySearcher.InternalFind(ref f, cDir, recurse, fileMasks, exclusionMasks);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000FF1C File Offset: 0x0000EF1C
		protected internal static bool InternalFind(ref BaseArchiveItem f, IItemsArray cDir, bool recurse, StringCollection fileMasks, StringCollection exclusionMasks)
		{
			bool flag = false;
			int i = 0;
			bool flag2 = !CompressionUtils.IsNullOrEmpty(f.Handle.CFindMask) && !f.Handle.FWildCards;
			if (flag2 && !recurse && fileMasks == null && exclusionMasks == null)
			{
				cDir.FileExists(f.Handle.CFindMask, ref i);
				if (i >= f.Handle.ItemNo)
				{
					flag = CentralDirectorySearcher.IsItemMatches(i, f, cDir, fileMasks, exclusionMasks);
					if (flag)
					{
						flag = CentralDirectorySearcher.CheckAttributesMatch((uint)cDir[i].ExternalAttributes, f.Handle.FFindAttr);
					}
				}
			}
			else
			{
				i = f.Handle.ItemNo;
				while (i < cDir.Count)
				{
					if (recurse)
					{
						goto IL_116;
					}
					string text = cDir[i].Name;
					if (text.EndsWith("/"))
					{
						text = text.Substring(0, text.Length - 1);
					}
					if (!CompressionUtils.IsNullOrEmpty(f.Handle.CFindMask) && !(Path.GetDirectoryName(text.Replace('/', '\\')) != Path.GetDirectoryName(f.Handle.CFindMask.Replace('/', '\\'))))
					{
						goto IL_116;
					}
					IL_147:
					i++;
					continue;
					IL_116:
					flag = CentralDirectorySearcher.IsItemMatches(i, f, cDir, fileMasks, exclusionMasks);
					if (!flag)
					{
						goto IL_147;
					}
					if (!CentralDirectorySearcher.CheckAttributesMatch((uint)cDir[i].ExternalAttributes, f.Handle.FFindAttr))
					{
						flag = false;
						goto IL_147;
					}
					break;
				}
			}
			if (flag)
			{
				cDir[i].GetArchiveItem(ref f);
				f.Handle.ItemNo = i;
			}
			return flag;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001009E File Offset: 0x0000F09E
		protected internal static bool IsInternalFileMatchMask(string fileName, string fileMask)
		{
			return !(fileMask == string.Empty) && (CompressionUtils.MatchesMask(fileName, fileMask) || CompressionUtils.MatchesMask(fileName.Replace('/', '\\'), fileMask.Replace('/', '\\')));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000100D4 File Offset: 0x0000F0D4
		protected internal static bool IsItemMatches(int itemNo, BaseArchiveItem f, IItemsArray cDir, StringCollection fileMasks, StringCollection exclusionMasks)
		{
			bool flag = false;
			if (f.Handle.UseProperties)
			{
				string text = cDir[itemNo].Name;
				int length = CompressionUtils.ExtractFileDrive(text).Length;
				if (length > 0)
				{
					text = text.Substring(length, text.Length - length);
				}
				text = text.Replace('/', '\\');
				for (int i = 0; i < fileMasks.Count; i++)
				{
					string fileMask = fileMasks[i];
					if (CentralDirectorySearcher.IsInternalFileMatchMask(text, fileMask))
					{
						flag = true;
						for (int j = 0; j < exclusionMasks.Count; j++)
						{
							string fileMask2 = exclusionMasks[j];
							if (CentralDirectorySearcher.IsInternalFileMatchMask(text, fileMask2))
							{
								return false;
							}
						}
					}
				}
			}
			else
			{
				string text = cDir[itemNo].Name.Replace('/', '\\');
				int length2 = CompressionUtils.ExtractFileDrive(text).Length;
				if (length2 > 0)
				{
					text = text.Substring(length2, text.Length - length2);
				}
				if (text != string.Empty && (text[text.Length - 1] == '\\' || text[text.Length - 1] == '/'))
				{
					text = text.Substring(0, text.Length - 1);
				}
				string fileMask = f.Handle.CFindMask;
				flag = (CentralDirectorySearcher.IsInternalFileMatchMask(text, fileMask) || CentralDirectorySearcher.IsInternalFileMatchMask(Path.GetFileName(text.Replace('/', '\\')), fileMask));
				if (flag && CentralDirectorySearcher.IsInternalFileMatchMask(text, f.Handle.ExclusionMask))
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00010248 File Offset: 0x0000F248
		protected internal static bool CheckAttributesMatch(uint fileAttr, int searchAttr)
		{
			uint num = (uint)((long)(~(long)searchAttr) & 22L);
			return (fileAttr & num) == 0U;
		}
	}
}
