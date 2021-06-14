using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.ZipForge;

namespace ComponentAce.Compression.ZipForgeRealTime
{
	// Token: 0x02000072 RID: 114
	internal class RealTimeDirManager
	{
		// Token: 0x060004C8 RID: 1224 RVA: 0x00021694 File Offset: 0x00020694
		public RealTimeDirManager(ZipForgeRealTime archiver)
		{
			this.Aborted = false;
			this.CentralDirectory = new CompressionItemsArray();
			this.ArchiveComment = string.Empty;
			this.CentralDirectoryEnd.Signature = archiver.CentralDirEndSignature;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000216CC File Offset: 0x000206CC
		public void SaveDir(Stream stream, ref long sentBytes)
		{
			long num = sentBytes;
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.CentralDirectoryEnd.StartDiskNumber = 0;
			for (int i = 0; i < this.CentralDirectory.Count; i++)
			{
				DirItem dirItem = this.CentralDirectory[i] as DirItem;
				dirItem.RelativeCentralDirectoryOffset = sentBytes;
				stream.Write(dirItem.GetCentralDirBytes(), 0, DirItem.CentralDirSize());
				sentBytes += (long)DirItem.CentralDirSize();
				byte[] buffer = dirItem.IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetBytes(dirItem.Name) : Encoding.GetEncoding(this.OemCodePage).GetBytes(dirItem.Name);
				stream.Write(buffer, 0, (int)dirItem.NameLength);
				sentBytes += (long)((ulong)dirItem.NameLength);
				this.SaveCDirExtraFields(stream, i, ref sentBytes);
				if (!CompressionUtils.IsNullOrEmpty(dirItem.Comment))
				{
					byte[] array = dirItem.IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetBytes(dirItem.Comment) : Encoding.GetEncoding(this.OemCodePage).GetBytes((this.CentralDirectory[i] as DirItem).Comment);
					stream.Write(array, 0, array.Length);
					sentBytes += (long)array.Length;
				}
			}
			long num2 = sentBytes - num;
			if (this.CentralDirectory.Count < 65535)
			{
				this.CentralDirectoryEnd.EntriesOnDisk = (ushort)this.CentralDirectory.Count;
			}
			else
			{
				this.CentralDirectoryEnd.EntriesOnDisk = ushort.MaxValue;
			}
			if (this.CentralDirectory.Count < 65535)
			{
				this.CentralDirectoryEnd.EntriesCentralDir = (ushort)this.CentralDirectory.Count;
			}
			else
			{
				this.CentralDirectoryEnd.EntriesCentralDir = ushort.MaxValue;
			}
			if (num2 < (long)((ulong)-1))
			{
				this.CentralDirectoryEnd.CentralDirSize = (uint)num2;
			}
			else
			{
				this.CentralDirectoryEnd.CentralDirSize = uint.MaxValue;
			}
			if (num < (long)((ulong)-1))
			{
				this.CentralDirectoryEnd.OffsetStartDir = (uint)num;
			}
			else
			{
				this.CentralDirectoryEnd.OffsetStartDir = uint.MaxValue;
			}
			if (this.ArchiveComment != null)
			{
				this.CentralDirectoryEnd.CommentLength = (ushort)this.ArchiveComment.Length;
			}
			else
			{
				this.CentralDirectoryEnd.CommentLength = 0;
			}
			this.CentralDirectoryEnd.DiskNumber = 0;
			if (sentBytes >= (long)((ulong)-1))
			{
				this.SaveZip64CentralDirEnd(stream, num, ref sentBytes);
				this.SaveZip64CentralDirEndLocator(stream, ref sentBytes);
			}
			stream.Write(this.CentralDirectoryEnd.GetBytes(), 0, ZipUtil.ZipCentralDirEndSize);
			sentBytes += (long)ZipUtil.ZipCentralDirEndSize;
			if (!CompressionUtils.IsNullOrEmpty(this.ArchiveComment))
			{
				byte[] array2 = new byte[(int)this.CentralDirectoryEnd.CommentLength];
				Array.Copy(CompressionUtils.StringToByteArray(this.ArchiveComment, this.OemCodePage), 0, array2, 0, this.ArchiveComment.Length);
				stream.Write(array2, 0, (int)this.CentralDirectoryEnd.CommentLength);
				sentBytes += (long)((ulong)this.CentralDirectoryEnd.CommentLength);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000219A0 File Offset: 0x000209A0
		internal void SaveCDirExtraFields(Stream stream, int dirItemNo, ref long sentBytes)
		{
			(this.CentralDirectory[dirItemNo] as DirItem).ExtraFields.WriteToStream(stream, 0L, ExtraFieldsTarget.CentralDirectoryExtraFields);
			sentBytes += (long)((ulong)(this.CentralDirectory[dirItemNo] as DirItem).GetExtraFieldsLength(ExtraFieldsTarget.CentralDirectoryExtraFields));
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000219E0 File Offset: 0x000209E0
		internal void SaveZip64CentralDirEnd(Stream stream, long startCentralDir, ref long sentBytes)
		{
			this.Zip64CentralDirEndLocator.OffsetStartDirEnd = sentBytes;
			this.Zip64CentralDirEnd.Signature = ZipUtil.Zip64CentralDirEndSignature;
			this.Zip64CentralDirEnd.CentralDirEndSize = (long)(ZipUtil.Zip64CentralDirEndSize - 12);
			this.Zip64CentralDirEnd.VersionMadeBy = 45;
			this.Zip64CentralDirEnd.VersionNeededToExtract = 45;
			this.Zip64CentralDirEnd.DiskNumber = 0U;
			this.Zip64CentralDirEnd.StartDiskNumber = 0U;
			this.Zip64CentralDirEnd.EntriesOnDisk = (long)this.CentralDirectory.Count;
			this.Zip64CentralDirEnd.EntriesCentralDir = (long)this.CentralDirectory.Count;
			this.Zip64CentralDirEnd.CentralDirSize = sentBytes - startCentralDir;
			this.Zip64CentralDirEnd.OffsetStartDir = startCentralDir;
			stream.Write(this.Zip64CentralDirEnd.GetBytes(), 0, ZipUtil.Zip64CentralDirEndSize);
			sentBytes += (long)ZipUtil.Zip64CentralDirEndSize;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00021ABC File Offset: 0x00020ABC
		internal void SaveZip64CentralDirEndLocator(Stream stream, ref long sentBytes)
		{
			this.Zip64CentralDirEndLocator.Signature = ZipUtil.Zip64CentralDirEndLocatorSignature;
			this.Zip64CentralDirEndLocator.StartDiskNumber = 0U;
			this.Zip64CentralDirEndLocator.TotalNumberOfDisks = 1U;
			stream.Write(this.Zip64CentralDirEndLocator.GetBytes(), 0, ZipUtil.Zip64CentralDirEndLocatorSize);
			sentBytes += (long)ZipUtil.Zip64CentralDirEndLocatorSize;
		}

		// Token: 0x040002D9 RID: 729
		public const ushort UnicodeExtraFieldHeaderId = 21838;

		// Token: 0x040002DA RID: 730
		public const ushort Zip64ExtraFieldHeaderId = 1;

		// Token: 0x040002DB RID: 731
		public bool Aborted;

		// Token: 0x040002DC RID: 732
		public string ArchiveComment;

		// Token: 0x040002DD RID: 733
		public IItemsArray CentralDirectory;

		// Token: 0x040002DE RID: 734
		public ZipCentralDirEnd CentralDirectoryEnd;

		// Token: 0x040002DF RID: 735
		public int OemCodePage;

		// Token: 0x040002E0 RID: 736
		public Zip64CentralDirEnd Zip64CentralDirEnd;

		// Token: 0x040002E1 RID: 737
		public Zip64CentralDirEndLocator Zip64CentralDirEndLocator;
	}
}
