using System;
using System.IO;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.ZipForge;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200007A RID: 122
	public class ArchiveItem : BaseArchiveItem
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00023503 File Offset: 0x00022503
		public long CompressedSize
		{
			get
			{
				return this._compressedSize;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0002350B File Offset: 0x0002250B
		public double CompressionRate
		{
			get
			{
				return this._compressionRate;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00023513 File Offset: 0x00022513
		public bool Encrypted
		{
			get
			{
				return this._encrypted;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0002351B File Offset: 0x0002251B
		public DateTime LastWriteTime
		{
			get
			{
				return this.getLastWriteTime();
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00023523 File Offset: 0x00022523
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0002352B File Offset: 0x0002252B
		public DateTime FileCreationDateTime
		{
			get
			{
				return this.fileCreationDateTime;
			}
			set
			{
				this.fileCreationDateTime = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00023534 File Offset: 0x00022534
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x0002353C File Offset: 0x0002253C
		public DateTime FileLastAccessDateTime
		{
			get
			{
				return this.lastAccessDateTime;
			}
			set
			{
				this.lastAccessDateTime = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00023545 File Offset: 0x00022545
		public uint CRC
		{
			get
			{
				return this._crc;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0002354D File Offset: 0x0002254D
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00023555 File Offset: 0x00022555
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				this._comment = value;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00023570 File Offset: 0x00022570
		internal DateTime getLastWriteTime()
		{
			uint dosDateTime = (uint)((int)this.LastModFileDate << 16 | (int)this.LastModFileTime);
			return CompressionUtils.DosDateTimeToDateTime(dosDateTime);
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00023594 File Offset: 0x00022594
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x0002359C File Offset: 0x0002259C
		public EncryptionAlgorithm EncryptionAlgorithm
		{
			get
			{
				return this.encryptionAlgorithm;
			}
			set
			{
				this.encryptionAlgorithm = value;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000235A8 File Offset: 0x000225A8
		public override void Reset()
		{
			base.Reset();
			this._compressedSize = 0L;
			this._compressionRate = 0.0;
			this._encrypted = false;
			uint num = CompressionUtils.DateTimeToDosDateTime(DateTime.Now);
			this.LastModFileDate = (ushort)(num >> 16);
			this.LastModFileTime = (ushort)(num & 65535U);
			this.FileLastAccessDateTime = DateTime.Now;
			this.FileCreationDateTime = DateTime.Now;
			this._crc = 0U;
			this.Comment = string.Empty;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00023625 File Offset: 0x00022625
		public ArchiveItem()
		{
			this.Reset();
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0002364C File Offset: 0x0002264C
		internal ArchiveItem(string fileName, string baseDir, StorePathMode storePath)
		{
			if (fileName.IndexOf("..\\") > -1 || fileName.IndexOf("\\..") > -1)
			{
				fileName = Path.GetFullPath(fileName);
			}
			base.SrcFileName = fileName;
			string archiveFileName = CompressionUtils.GetArchiveFileName(fileName, baseDir, storePath);
			bool flag = FileUtils.DirectotyExists(fileName);
			if (!flag && !File.Exists(fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
				{
					fileName
				});
			}
			this._compressedSize = 0L;
			if (!flag)
			{
				base.FileName = Path.GetFileName(archiveFileName);
				this.storedPath = Path.GetDirectoryName(archiveFileName).Replace('\\', '/');
				FileInfo fileInfo = new FileInfo(fileName);
				this._uncompressedSize = fileInfo.Length;
				uint num = CompressionUtils.DateTimeToDosDateTime(fileInfo.LastWriteTime);
				this.LastModFileDate = (ushort)(num >> 16);
				this.LastModFileTime = (ushort)(num & 65535U);
				base.FileModificationDateTime = fileInfo.LastWriteTime;
				this.FileLastAccessDateTime = fileInfo.LastAccessTime;
				this.FileCreationDateTime = fileInfo.CreationTime;
				base.ExternalFileAttributes = fileInfo.Attributes;
			}
			else
			{
				base.FileName = "";
				this.storedPath = archiveFileName;
				DirectoryInfo directoryInfo = new DirectoryInfo(fileName);
				this._uncompressedSize = 0L;
				uint num2 = CompressionUtils.DateTimeToDosDateTime(directoryInfo.LastWriteTime);
				this.LastModFileDate = (ushort)(num2 >> 16);
				this.LastModFileTime = (ushort)(num2 & 65535U);
				base.FileModificationDateTime = directoryInfo.LastWriteTime;
				this.FileLastAccessDateTime = directoryInfo.LastAccessTime;
				this.FileCreationDateTime = directoryInfo.CreationTime;
				base.ExternalFileAttributes = directoryInfo.Attributes;
			}
			this._crc = uint.MaxValue;
			this._compressionRate = 0.0;
			this._encrypted = false;
			this.Comment = "";
			this.Handle = new InternalSearchRec();
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0002381E File Offset: 0x0002281E
		public ArchiveItem(string fileName) : this(fileName, "", StorePathMode.RelativePath)
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00023830 File Offset: 0x00022830
		internal ArchiveItem(DirItem item, int itemIndex)
		{
			this.IsCustomPath = false;
			string text = item.Name;
			text = text.Replace('/', '\\');
			if (text[text.Length - 1] == '\\')
			{
				text = text.Substring(0, text.Length - 1);
			}
			base.FileName = Path.GetFileName(item.Name);
			this.storedPath = Path.GetDirectoryName(text);
			this._compressedSize = item.CompressedSize;
			this._uncompressedSize = item.UncompressedSize;
			if (base.UncompressedSize > 0L)
			{
				this._compressionRate = (double)(1f - (float)this.CompressedSize / (float)base.UncompressedSize) * 100.0;
			}
			else
			{
				this._compressionRate = 100.0;
			}
			if (this.CompressionRate < 0.0)
			{
				this._compressionRate = 0.0;
			}
			this._encrypted = item.IsGeneralPurposeFlagBitSet(0);
			this.LastModFileDate = item.LastModificationDate;
			this.LastModFileTime = item.LastModificationTime;
			this._crc = item.CRC32;
			base.ExternalFileAttributes = item.ExternalAttributes;
			this._comment = item.Comment;
			this.Handle = new InternalSearchRec();
			this.Handle.ItemNo = itemIndex;
		}

		// Token: 0x04000303 RID: 771
		internal long _compressedSize;

		// Token: 0x04000304 RID: 772
		internal double _compressionRate;

		// Token: 0x04000305 RID: 773
		internal bool _encrypted;

		// Token: 0x04000306 RID: 774
		private EncryptionAlgorithm encryptionAlgorithm;

		// Token: 0x04000307 RID: 775
		internal ushort LastModFileDate;

		// Token: 0x04000308 RID: 776
		internal ushort LastModFileTime;

		// Token: 0x04000309 RID: 777
		internal DateTime lastAccessDateTime = default(DateTime);

		// Token: 0x0400030A RID: 778
		internal DateTime fileCreationDateTime = default(DateTime);

		// Token: 0x0400030B RID: 779
		internal uint _crc;

		// Token: 0x0400030C RID: 780
		private string _comment;
	}
}
