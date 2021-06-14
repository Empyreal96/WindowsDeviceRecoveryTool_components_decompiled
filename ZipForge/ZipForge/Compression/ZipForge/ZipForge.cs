using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.ZipForge.Encryption;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A8 RID: 168
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(resfinder), "ComponentAce.Compression.Resources.zip16.ico")]
	public class ZipForge : BaseArchiver
	{
		// Token: 0x06000777 RID: 1911 RVA: 0x0002C82D File Offset: 0x0002B82D
		protected internal override void SetCompMethod()
		{
			if (this._compressionMode == 0)
			{
				this._compressionMethod = 0;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002C840 File Offset: 0x0002B840
		public ZipForge()
		{
			this.Zip64Mode = Zip64Mode.Auto;
			this.UnicodeFilenames = true;
			this.StoreNTFSTimeStamps = false;
			this._currentBlockSize = 0L;
			this._currentSourceBlock = new byte[0];
			this._currentResBlockSize = 0L;
			this._currentResBlock = new byte[0];
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0002C890 File Offset: 0x0002B890
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0002C898 File Offset: 0x0002B898
		[Description("Specifies whether to use Zip64 format to create archives over 4GB.")]
		public Zip64Mode Zip64Mode
		{
			get
			{
				return this._zip64Mode;
			}
			set
			{
				this._zip64Mode = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0002C8A1 File Offset: 0x0002B8A1
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0002C8A9 File Offset: 0x0002B8A9
		[Description("Specifies whether unicode file names are saved addition to filenames in standard OEM encoding.")]
		public bool UnicodeFilenames
		{
			get
			{
				return this._unicodeFilenames;
			}
			set
			{
				this._unicodeFilenames = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0002C8B2 File Offset: 0x0002B8B2
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0002C8BA File Offset: 0x0002B8BA
		[Description("Specifies whether file modification, last access and creation times are saved.")]
		public bool StoreNTFSTimeStamps
		{
			get
			{
				return this._storeNTFSTimeStamps;
			}
			set
			{
				this._storeNTFSTimeStamps = value;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002C8C3 File Offset: 0x0002B8C3
		internal override BaseArchiver CreateArchiver()
		{
			return new ZipForge();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002C8CC File Offset: 0x0002B8CC
		internal void CalculateStreamCRC32(Stream stream, int blockSize, long count, ref uint fcrc32)
		{
			int num3;
			for (long num = 0L; num < count; num += (long)num3)
			{
				long num2 = count - num;
				num3 = (int)((num2 > (long)blockSize) ? ((long)blockSize) : num2);
				if (stream.Read(this._currentSourceBlock, 0, num3) != num3)
				{
					return;
				}
				ZipUtil.UpdateCRC32(this._currentSourceBlock, (uint)num3, ref fcrc32);
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002C918 File Offset: 0x0002B918
		internal override void DoCompress(bool isEncrypted, IItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long processedBytesCount, ref long compSize, ref uint fcrc32)
		{
			this.DoCompress(isEncrypted, (DirItem)item, blockSize, streamCompressFrom, streamCompressTo, ref processedBytesCount, ref compSize, ref fcrc32);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002C940 File Offset: 0x0002B940
		internal void DoCompress(bool isEncrypted, DirItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long processedBytesCount, ref long compSize, ref uint fcrc32)
		{
			long num = 0L;
			int num2 = 0;
			long length = streamCompressFrom.Length;
			BaseZipForgeCryptoTransform baseZipForgeCryptoTransform = null;
			if (this._currentBlockSize != (long)blockSize || this._currentSourceBlock == null)
			{
				this._currentSourceBlock = new byte[blockSize];
				this._currentBlockSize = (long)blockSize;
			}
			if (this._currentResBlockSize != (long)blockSize || this._currentResBlock == null)
			{
				this._currentResBlock = new byte[(long)(1.1 * (double)blockSize)];
				this._currentResBlockSize = (long)blockSize;
			}
			BaseCompressor compressor = CompressorFactory.GetCompressor((CompressionMethod)item.ActualCompressionMethod, item.CompressionMode);
			compressor.Init(CompressionDirection.Compress, item.CompressionMode);
			if (isEncrypted)
			{
				if (ZipForgeCryptoTransformFactory.IsAESEncryption(item.EncryptionAlgorithm))
				{
					item.CRC32 = 0U;
				}
				baseZipForgeCryptoTransform = ZipForgeCryptoTransformFactory.GetCryptoTransform(item.EncryptionAlgorithm, item);
				uint maxValue = uint.MaxValue;
				this.CalculateStreamCRC32(streamCompressFrom, blockSize, length, ref maxValue);
				item.CRC32 = ~maxValue;
				if (isEncrypted)
				{
					baseZipForgeCryptoTransform.Initialize(CryptoTransformMode.Encryption, item);
				}
				baseZipForgeCryptoTransform.GenerateKey(item.Password);
				compSize += (long)baseZipForgeCryptoTransform.GetFileStorageStartBlockSize();
				streamCompressTo.Write(baseZipForgeCryptoTransform.GetFileStorageStartBlock(), 0, baseZipForgeCryptoTransform.GetFileStorageStartBlockSize());
			}
			streamCompressFrom.Seek(0L, SeekOrigin.Begin);
			while (processedBytesCount < length)
			{
				long num3 = length - processedBytesCount;
				long num4 = (num3 > (long)blockSize) ? ((long)blockSize) : num3;
				this._totalProcessedFilesSize += num4;
				if (this._progressEnabled)
				{
					DateTime now = DateTime.Now;
					TimeSpan timeLeft = new TimeSpan(0L);
					if (processedBytesCount != 0L)
					{
						timeLeft = new TimeSpan(length * (now.Ticks - this._currentItemOperationStartTime.Ticks) / processedBytesCount) - (now - this._currentItemOperationStartTime);
					}
					this.DoOnFileProgress(item.Name, (double)processedBytesCount / (double)length * 100.0, now - this._currentItemOperationStartTime, timeLeft, item.Operation, ProgressPhase.Process, ref this._progressCancel);
					if (this._progressCancel)
					{
						break;
					}
					TimeSpan timeLeft2 = new TimeSpan(0L);
					if (this._totalProcessedFilesSize != 0L)
					{
						timeLeft2 = new TimeSpan(this._toProcessFilesTotalSize * (now.Ticks - this._operationStartTime.Ticks) / this._totalProcessedFilesSize) - (now - this._operationStartTime);
					}
					this.DoOnOverallProgress((double)this._totalProcessedFilesSize / (double)this._toProcessFilesTotalSize * 100.0, now - this._operationStartTime, timeLeft2, item.Operation, ProgressPhase.Process, ref this._progressCancel);
					if (this._progressCancel)
					{
						break;
					}
				}
				if ((long)streamCompressFrom.Read(this._currentSourceBlock, 0, (int)num4) == num4)
				{
					try
					{
						ZipUtil.UpdateCRC32(this._currentSourceBlock, (uint)num4, ref fcrc32);
						if (!compressor.CompressBlock((uint)blockSize, num4, processedBytesCount + num4 >= length, this._currentSourceBlock, ref num, ref this._currentResBlock))
						{
							throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat);
						}
						processedBytesCount += num4;
						if (isEncrypted)
						{
							baseZipForgeCryptoTransform.EncryptBuffer(this._currentResBlock, num2, (int)num, this._currentResBlock, num2);
						}
					}
					catch
					{
						compressor.Close();
						return;
					}
					if (!base.WriteToStreamWithOnDiskFull(this._currentResBlock, num2, (int)num, streamCompressTo))
					{
						processedBytesCount -= num4;
						break;
					}
					compSize += num;
					continue;
				}
				break;
			}
			if (isEncrypted)
			{
				base.WriteToStreamWithOnDiskFull(baseZipForgeCryptoTransform.GetFileStorageEndBlock(), 0, baseZipForgeCryptoTransform.GetFileStorageEndBlockSize(), streamCompressTo);
				compSize += (long)baseZipForgeCryptoTransform.GetFileStorageEndBlockSize();
			}
			item.CompressedSize = compSize;
			item.UncompressedSize = length;
			item.CRC32 = ~fcrc32;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002CCB0 File Offset: 0x0002BCB0
		internal override uint GetFileHeaderSignature()
		{
			return 67324752U;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002CCB7 File Offset: 0x0002BCB7
		internal override uint GetCentralDirSignature()
		{
			return 33639248U;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002CCBE File Offset: 0x0002BCBE
		internal override uint GetCentralDirEndSignature()
		{
			return 101010256U;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002CCC8 File Offset: 0x0002BCC8
		protected override void AddNewItemToItemsHandler()
		{
			DirItem item = new DirItem();
			this._itemsHandler.ItemsArray.AddItem(item);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002CCEC File Offset: 0x0002BCEC
		protected internal override IItemsHandler CreateNewItemsHandler(Stream s, bool create)
		{
			DirManager dirManager = (DirManager)base.CreateNewItemsHandler(s, create);
			dirManager.FZip64 = (this.Zip64Mode == Zip64Mode.Always);
			return dirManager;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002CD17 File Offset: 0x0002BD17
		internal override bool FXCEncryptBuffer(int cryptoAlg, ref byte[] inBuf, int length, string password)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002CD1E File Offset: 0x0002BD1E
		internal override bool FXCDecryptBuffer(int cryptoAlg, ref byte[] inBuf, long length, string password)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002CD25 File Offset: 0x0002BD25
		internal override bool FXCEncryptFilename(ref byte[] inBuf, int length, string Password)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002CD2C File Offset: 0x0002BD2C
		internal override bool FXCDecryptFilename(ref byte[] inBuf, int length, string Password)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002CD33 File Offset: 0x0002BD33
		internal override bool FXCDecompress(bool encrypted, ref long count, ref long decompSize, ref Stream compressedStream, ref Stream destinationStream, ref int volumeNumber, int lastVolumeNumber, ref uint fcrc32, DirItem item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002CD3A File Offset: 0x0002BD3A
		internal override bool IsFlexCompress()
		{
			return false;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002CD3D File Offset: 0x0002BD3D
		protected override BaseArchiveItem CreateNewArchiveItem()
		{
			return new ArchiveItem();
		}

		// Token: 0x04000412 RID: 1042
		private byte[] _currentSourceBlock;

		// Token: 0x04000413 RID: 1043
		private long _currentBlockSize;

		// Token: 0x04000414 RID: 1044
		private byte[] _currentResBlock;

		// Token: 0x04000415 RID: 1045
		private long _currentResBlockSize;
	}
}
