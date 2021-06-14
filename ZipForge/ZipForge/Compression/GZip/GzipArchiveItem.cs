using System;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000038 RID: 56
	public class GzipArchiveItem : BaseArchiveItem
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00016722 File Offset: 0x00015722
		public GzipArchiveItem()
		{
			this.ExternalFileAttributes = (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00016735 File Offset: 0x00015735
		public long CompressedSize
		{
			get
			{
				return this._compressedSize;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0001673D File Offset: 0x0001573D
		public double CompressionRate
		{
			get
			{
				return this._compressionRate;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00016745 File Offset: 0x00015745
		public uint Crc
		{
			get
			{
				return this._crc;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00016750 File Offset: 0x00015750
		public GzipArchiveItem(string fileName)
		{
			if (fileName.IndexOf("..\\") > -1 || fileName.IndexOf("\\..") > -1)
			{
				fileName = Path.GetFullPath(fileName);
			}
			base.SrcFileName = fileName;
			if (!File.Exists(fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
				{
					fileName
				});
			}
			base.FileName = Path.GetFileName(fileName);
			FileInfo fileInfo = new FileInfo(fileName);
			this._uncompressedSize = fileInfo.Length;
			base.FileModificationDateTime = fileInfo.LastWriteTime;
			this.ExternalFileAttributes = fileInfo.Attributes;
			this.Handle = new InternalSearchRec();
		}

		// Token: 0x0400019A RID: 410
		public string Comment;

		// Token: 0x0400019B RID: 411
		internal long _compressedSize;

		// Token: 0x0400019C RID: 412
		internal double _compressionRate;

		// Token: 0x0400019D RID: 413
		protected new FileAttributes ExternalFileAttributes;

		// Token: 0x0400019E RID: 414
		internal uint _crc;

		// Token: 0x0400019F RID: 415
		internal bool NeedToStoreHeaderCRC;
	}
}
