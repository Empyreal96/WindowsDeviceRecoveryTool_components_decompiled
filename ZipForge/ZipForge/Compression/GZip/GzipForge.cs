using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.Tar;
using ComponentAce.Compression.ZipForge;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x0200003C RID: 60
	[ToolboxBitmap(typeof(resfinder), "ComponentAce.Compression.Resources.gzip16.ico")]
	[ToolboxItem(true)]
	public class GzipForge : ArchiverForgeBase
	{
		// Token: 0x06000232 RID: 562 RVA: 0x00016910 File Offset: 0x00015910
		public GzipForge()
		{
			this._currentBlockSize = 0L;
			this._currentSourceBlock = new byte[0];
			this._currentResBlockSize = 0L;
			this._currentResBlock = new byte[0];
			this._gzipArchiverOptions = new GzipArchiveOptions();
			this.CompressionLevel = CompressionLevel.Fastest;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0001695D File Offset: 0x0001595D
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00016965 File Offset: 0x00015965
		[Description("Specifies whether to create separate archivers for each file.")]
		public bool CreateSeparateArchivers
		{
			get
			{
				return this._createSeparateArchivers;
			}
			set
			{
				if (this.Active)
				{
					throw ExceptionBuilder.Exception(ErrorCode.ArchiveIsOpen);
				}
				this._createSeparateArchivers = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0001697E File Offset: 0x0001597E
		[Description("Specifies the gzip options for archiving operations.")]
		public GzipArchiveOptions GzipArchiveOptions
		{
			get
			{
				return this._gzipArchiverOptions;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00016988 File Offset: 0x00015988
		protected override void SaveRenamedItemToArchive(Stream backupFileStream, Stream compressedStream, int itemNo, int itemNoInBackupArray)
		{
			this._itemsHandler.ItemsArray[itemNo].WriteLocalHeaderToStream(compressedStream, 0);
			long num = (this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as GzipItem).CompressedSize;
			num += 8L;
			CompressionUtils.CopyStream(backupFileStream, compressedStream, num);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000169D8 File Offset: 0x000159D8
		protected override long GetEndOfTheDataStreamPosition(int itemNo)
		{
			if (itemNo >= this._itemsHandler.ItemsArray.Count)
			{
				return 0L;
			}
			if ((this._itemsHandler.ItemsArray[itemNo] as GzipItem).DestinationStream == null)
			{
				return 0L;
			}
			return (this._itemsHandler.ItemsArray[itemNo] as GzipItem).DestinationStream.Length;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00016A3C File Offset: 0x00015A3C
		public void ChangeFilesComment(string fileMask, string newComment)
		{
			if (CompressionUtils.IsNullOrEmpty(newComment))
			{
				newComment = string.Empty;
			}
			base.CheckInactive();
			BaseArchiveItem baseArchiveItem = new GzipArchiveItem();
			if (this.FindFirst(fileMask, ref baseArchiveItem))
			{
				this.BeginUpdate();
				try
				{
					do
					{
						(this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo] as GzipItem).Comment = newComment;
						if (!this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified)
						{
							this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified = true;
							this._itemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation = ProcessOperation.ChangeComment;
						}
					}
					while (this.FindNext(ref baseArchiveItem));
					this.EndUpdate();
					return;
				}
				catch
				{
					this.CancelUpdate();
					throw;
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileMask
			});
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00016B44 File Offset: 0x00015B44
		internal override void DoCompress(bool encrypted, IItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long processedBytesCount, ref long compSize, ref uint fcrc32)
		{
			this.DoCompress(encrypted, (GzipItem)item, blockSize, streamCompressFrom, streamCompressTo, ref processedBytesCount, ref compSize, ref fcrc32);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00016B6C File Offset: 0x00015B6C
		internal void DoCompress(bool encrypted, GzipItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long processedBytesCount, ref long compSize, ref uint fcrc32)
		{
			long num = 0L;
			long length = streamCompressFrom.Length;
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
			DeflateCompressor deflateCompressor = new DeflateCompressor();
			deflateCompressor.Init(CompressionDirection.Compress, (byte)this.CompressionLevel);
			streamCompressFrom.Seek(0L, SeekOrigin.Begin);
			while (processedBytesCount < length)
			{
				long num2 = length - processedBytesCount;
				long num3 = (num2 > (long)blockSize) ? ((long)blockSize) : num2;
				this._totalProcessedFilesSize += num3;
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
				if ((long)streamCompressFrom.Read(this._currentSourceBlock, 0, (int)num3) == num3)
				{
					try
					{
						ZipUtil.UpdateCRC32(this._currentSourceBlock, (uint)num3, ref fcrc32);
						if (!deflateCompressor.CompressBlock((uint)blockSize, num3, processedBytesCount + num3 >= length, this._currentSourceBlock, ref num, ref this._currentResBlock))
						{
							throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat);
						}
						processedBytesCount += num3;
					}
					catch
					{
						deflateCompressor.Close();
						return;
					}
					if (!ReadWriteHelper.WriteToStream(this._currentResBlock, 0, (int)num, streamCompressTo, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure)))
					{
						processedBytesCount -= num3;
						break;
					}
					compSize += num;
					continue;
				}
				break;
			}
			item.CompressedSize = compSize;
			item.UncompressedSize = length;
			item.Crc32 = ~fcrc32;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00016E1C File Offset: 0x00015E1C
		protected override DateTime GetLastModificationDateTime(IItem item)
		{
			return item.LastFileModificationTime;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00016E24 File Offset: 0x00015E24
		protected internal override BaseArchiveItem CreateNewArchiveItem(string fileName, string baseDir, StorePathMode storePathMode)
		{
			return new GzipArchiveItem(fileName)
			{
				NeedToStoreHeaderCRC = this.StoreHeaderCRC
			};
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00016E45 File Offset: 0x00015E45
		protected internal override IItemsHandler CreateNewItemsHandler(Stream stream, bool create)
		{
			return new GzipItemsHandler(stream, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00016E68 File Offset: 0x00015E68
		protected internal override void ExtractItem(int itemNo, Stream destStream)
		{
			if (itemNo < 0 || itemNo >= this._itemsHandler.ItemsArray.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds, new ArgumentOutOfRangeException("itemNo"));
			}
			this._compressedStream.Position = this._itemsHandler.ItemsArray[itemNo].RelativeLocalHeaderOffset;
			if (!this.InternalDecompressFile(this._compressedStream, destStream, this._itemsHandler.ItemsArray[itemNo] as GzipItem) && !this._progressCancel && !this._skipFile)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidCheckSum);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00016EFC File Offset: 0x00015EFC
		internal bool InternalDecompressFile(Stream rfs, Stream wfs, GzipItem dirItem)
		{
			long num = 0L;
			long compressedSize = dirItem.CompressedSize;
			int dataOffset = dirItem.GetDataOffset();
			rfs.Seek((long)dataOffset, SeekOrigin.Current);
			this._currentItemOperationStartTime = DateTime.Now;
			long num2 = (long)((ulong)dirItem.CompressionMethod);
			this._skipFile = false;
			if (num2 != 8L)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnknownCompressionMethod);
			}
			uint num3;
			bool flag = this.GzipDecompress(out num, ref rfs, wfs, out num3, dirItem);
			if (this._skipFile)
			{
				return true;
			}
			if (!flag)
			{
				return flag;
			}
			return num % 4294967296L == dirItem.UncompressedSize;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00016F84 File Offset: 0x00015F84
		internal bool GzipDecompress(out long actualDecompressedSize, ref Stream rfs, Stream wfs, out uint crc32, GzipItem dirItem)
		{
			this._currentProcessedItem = dirItem;
			this._decompressedStream = wfs;
			bool result = false;
			this._skipFile = false;
			long num = rfs.Length - (long)dirItem.GetLocalHeaderSize() - 8L;
			byte[] array = new byte[1048578];
			DeflateCompressor deflateCompressor = new DeflateCompressor();
			deflateCompressor.Init(CompressionDirection.Decompress, dirItem.CompressionMode);
			deflateCompressor.OnDecompressedBufferReady += this.decompressor_OnDecompressedBufferReady;
			try
			{
				this._currentItemBytesProcessed = 0L;
				this._currentItemSize = dirItem.UncompressedSize;
				crc32 = 0U;
				this._crc32 = uint.MaxValue;
				actualDecompressedSize = 0L;
				long num3;
				for (long num2 = 0L; num2 < num; num2 += num3)
				{
					if (num - num2 > 1048576L)
					{
						num3 = 1048576L;
					}
					else
					{
						num3 = num - num2;
					}
					int num4;
					if (!ReadWriteHelper.ReadFromStream(array, 0, (int)num3, out num4, rfs, new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure)))
					{
						return result;
					}
					long num5;
					if (!deflateCompressor.DecompressBlock((int)num3, num2 + num3 >= num, array, out num5))
					{
						return result;
					}
					actualDecompressedSize += num5;
				}
			}
			finally
			{
				deflateCompressor.Close();
			}
			crc32 = this._crc32;
			this.ReadCrc32(ref dirItem);
			this.ReadUncompressedSize(ref dirItem);
			return dirItem.Crc32 == ~crc32;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000170CC File Offset: 0x000160CC
		private void ReadCrc32(ref GzipItem item)
		{
			byte[] array = new byte[4];
			int num;
			if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			MemoryStream input = new MemoryStream(array);
			BinaryReader binaryReader = new BinaryReader(input);
			item.Crc32 = binaryReader.ReadUInt32();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00017128 File Offset: 0x00016128
		private void ReadUncompressedSize(ref GzipItem item)
		{
			byte[] array = new byte[4];
			int num;
			if (!ReadWriteHelper.ReadFromStream(array, 0, array.Length, out num, this._compressedStream, new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure)))
			{
				throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
			}
			MemoryStream input = new MemoryStream(array);
			BinaryReader binaryReader = new BinaryReader(input);
			item.UncompressedSize = (long)((ulong)binaryReader.ReadUInt32());
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00017184 File Offset: 0x00016184
		private void decompressor_OnDecompressedBufferReady(byte[] buffer, int outBytes, out bool stopDecompression)
		{
			TimeSpan timeElapsed = new TimeSpan(DateTime.Now.Ticks - this._currentItemOperationStartTime.Ticks);
			double progress = (double)this._currentItemBytesProcessed / (double)this._currentItemSize * 100.0;
			TimeSpan timeLeft;
			if (this._currentItemBytesProcessed > 0L)
			{
				double num = (double)this._currentItemBytesProcessed / (double)timeElapsed.Ticks;
				timeLeft = new TimeSpan((long)((double)(this._currentItemSize - this._currentItemBytesProcessed) / num));
			}
			else
			{
				timeLeft = new TimeSpan(0L);
			}
			TimeSpan timeElapsed2 = new TimeSpan(DateTime.Now.Ticks - this._operationStartTime.Ticks);
			TimeSpan timeLeft2;
			if (this._totalProcessedFilesSize > 0L)
			{
				double num2 = (double)this._totalProcessedFilesSize / (double)timeElapsed2.Ticks;
				timeLeft2 = new TimeSpan((long)((double)(this._toProcessFilesTotalSize - this._totalProcessedFilesSize) / num2));
			}
			else
			{
				timeLeft2 = new TimeSpan(0L);
			}
			double num3 = (double)this._totalProcessedFilesSize / (double)this._toProcessFilesTotalSize * 100.0;
			if (num3 > 100.0)
			{
				num3 = 100.0;
			}
			stopDecompression = false;
			this.DoOnFileProgress(this._currentProcessedItem.Name, progress, timeElapsed, timeLeft, ProcessOperation.Extract, ProgressPhase.Process, ref stopDecompression);
			this.DoOnOverallProgress(num3, timeElapsed2, timeLeft2, ProcessOperation.Extract, ProgressPhase.Process, ref stopDecompression);
			if (!stopDecompression)
			{
				bool flag = ReadWriteHelper.WriteToStream(buffer, 0, outBytes, this._decompressedStream, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure));
				stopDecompression = !flag;
				ZipUtil.UpdateCRC32(buffer, (uint)outBytes, ref this._crc32);
			}
			else
			{
				this._progressCancel = true;
			}
			this._totalProcessedFilesSize += (long)outBytes;
			this._currentItemBytesProcessed += (long)outBytes;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00017324 File Offset: 0x00016324
		protected internal override void FillDirItem(int itemNo, string fileName)
		{
			FileAttributes attr = File.Exists(fileName) ? FileUtils.GetAttributes(fileName) : FileAttributes.Archive;
			string fileName2 = Path.GetFileName(fileName);
			this.FillDirItem(itemNo, fileName, fileName2, true, attr);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00017358 File Offset: 0x00016358
		protected internal void FillDirItem(int itemNo, string fileName, string arcFileName, bool retrieveFileDate, FileAttributes attr)
		{
			if (CompressionUtils.IsNullOrEmpty(arcFileName))
			{
				return;
			}
			string text = FileUtils.StripSlash(fileName);
			DateTime lastFileModificationTime = retrieveFileDate ? File.GetLastWriteTime(text) : DateTime.Now;
			GzipItem gzipItem = new GzipItem(arcFileName, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
			this._itemsHandler.ItemsArray[itemNo] = gzipItem;
			gzipItem.IsModified = true;
			gzipItem.SrcFileName = text;
			gzipItem.CompressionMode = (byte)this.CompressionLevel;
			gzipItem.CompressionMethod = 8;
			gzipItem.LastFileModificationTime = lastFileModificationTime;
			gzipItem.Crc32 = uint.MaxValue;
			gzipItem.CompressedSize = 0L;
			gzipItem.UncompressedSize = 0L;
			gzipItem.ExternalAttributes = attr;
			gzipItem.RelativeLocalHeaderOffset = 0L;
			gzipItem.NeedToStoreHeaderCRC = this.StoreHeaderCRC;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00017414 File Offset: 0x00016414
		public override void OpenArchive()
		{
			if (this.CreateSeparateArchivers && CompressionUtils.IsNullOrEmpty(this._fileName))
			{
				this._isInMemory = true;
			}
			base.OpenArchive();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00017438 File Offset: 0x00016438
		public override void OpenArchive(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
		{
			if (this.CreateSeparateArchivers && CompressionUtils.IsNullOrEmpty(this._fileName))
			{
				this._isInMemory = true;
			}
			base.OpenArchive(fileMode, fileAccess, fileShare);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00017460 File Offset: 0x00016460
		protected internal override void DeleteItem(int itemNo)
		{
			int itemNoInBackupArray = base.GetItemNoInBackupArray(itemNo);
			if (itemNoInBackupArray == -1)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			GzipItem gzipItem = this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as GzipItem;
			gzipItem._backupFileName = base.GetTempFileName();
			FileStream fileStream = new FileStream(gzipItem._backupFileName, FileMode.Create);
			gzipItem.DestinationStream.Seek(0L, SeekOrigin.Begin);
			CompressionUtils.CopyStream(gzipItem.DestinationStream, fileStream, gzipItem.DestinationStream.Length);
			fileStream.Close();
			base.DeleteItem(itemNo);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000174E4 File Offset: 0x000164E4
		protected internal override void BackupFileRest(int itemNo, ref string tempFileName, ref Stream backupFileStream)
		{
			GzipItem gzipItem = this._itemsHandler.ItemsArrayBackup[itemNo] as GzipItem;
			if (!gzipItem.DestinationStream.CanRead && !CompressionUtils.IsNullOrEmpty(gzipItem._backupFileName) && File.Exists(gzipItem._backupFileName))
			{
				tempFileName = gzipItem._backupFileName;
				backupFileStream = new FileStream(tempFileName, FileMode.Open);
				return;
			}
			tempFileName = base.GetTempFileName();
			backupFileStream = new FileStream(tempFileName, FileMode.Create);
			long relativeLocalHeaderOffset = gzipItem.RelativeLocalHeaderOffset;
			if (this._createSeparateArchivers)
			{
				gzipItem.DestinationStream.Seek(relativeLocalHeaderOffset, SeekOrigin.Begin);
				CompressionUtils.CopyStream(gzipItem.DestinationStream, backupFileStream, gzipItem.DestinationStream.Length - relativeLocalHeaderOffset);
				return;
			}
			this._compressedStream.Seek(relativeLocalHeaderOffset, SeekOrigin.Begin);
			CompressionUtils.CopyStream(this._compressedStream, backupFileStream, this._compressedStream.Length - relativeLocalHeaderOffset);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000175B4 File Offset: 0x000165B4
		protected internal override void AddFromFileRest(int itemNo, Stream backupFileStream, long backupOffset, Stream compressedStream)
		{
			GzipItem gzipItem = this._itemsHandler.ItemsArray[itemNo] as GzipItem;
			bool needToDestroyDestinationStream = gzipItem.NeedToDestroyDestinationStream;
			string text = string.Empty;
			Stream destinationStream = gzipItem.DestinationStream;
			if (gzipItem.IsModified)
			{
				if (gzipItem.Operation == ProcessOperation.Rename && gzipItem.NeedToDestroyDestinationStream)
				{
					if (gzipItem.DestinationStream != null && gzipItem.DestinationStream is FileStream)
					{
						text = (gzipItem.DestinationStream as FileStream).Name;
					}
					Stream stream;
					bool flag = this.CreateNewFileInOutputFilesDir(itemNo, out stream);
					if (!flag)
					{
						return;
					}
					gzipItem.DestinationStream = stream;
					gzipItem.NeedToDestroyDestinationStream = true;
					destinationStream.Flush();
					long position = destinationStream.Position;
					destinationStream.Seek(0L, SeekOrigin.Begin);
					CompressionUtils.CopyStream(destinationStream, stream);
				}
				gzipItem.DestinationStream.Seek(backupOffset, SeekOrigin.Begin);
				gzipItem.DestinationStream.SetLength(backupOffset);
				base.AddFromFileRest(itemNo, backupFileStream, backupOffset, gzipItem.DestinationStream);
				if (gzipItem.IsModified && needToDestroyDestinationStream)
				{
					if (destinationStream != null)
					{
						destinationStream.Close();
					}
					if (!CompressionUtils.IsNullOrEmpty(text) && File.Exists(text))
					{
						try
						{
							File.Delete(text);
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000176D8 File Offset: 0x000166D8
		protected internal override bool AddFromNewSource(int itemNo, Stream compressedStream, ref FailureAction action)
		{
			if (!this.CreateSeparateArchivers && this._itemsHandler.ItemsArray.Count > 1)
			{
				throw ExceptionBuilder.Exception(ErrorCode.ShouldCreateSeparateArchivers);
			}
			GzipItem gzipItem = this._itemsHandler.ItemsArray[itemNo] as GzipItem;
			if (gzipItem.DestinationStream == null)
			{
				if (!this.CreateSeparateArchivers)
				{
					gzipItem.DestinationStream = this._compressedStream;
					gzipItem.NeedToDestroyDestinationStream = false;
				}
				else
				{
					Stream destinationStream;
					bool flag = this.CreateNewFileInOutputFilesDir(itemNo, out destinationStream);
					if (flag)
					{
						gzipItem.DestinationStream = destinationStream;
						gzipItem.NeedToDestroyDestinationStream = true;
						return base.AddFromNewSource(itemNo, gzipItem.DestinationStream, ref action);
					}
					return false;
				}
			}
			return base.AddFromNewSource(itemNo, gzipItem.DestinationStream, ref action);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00017780 File Offset: 0x00016780
		public override void CloseArchive()
		{
			if (!this._isOpened)
			{
				if (this._compressedStream != null)
				{
					this._compressedStream.Close();
				}
				this._compressedStream = null;
				if (this._itemsHandler != null)
				{
					for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
					{
						GzipItem gzipItem = this._itemsHandler.ItemsArray[i] as GzipItem;
						if (gzipItem.NeedToDestroyDestinationStream)
						{
							gzipItem.DestinationStream.Close();
						}
					}
				}
				this._itemsHandler = null;
			}
			else
			{
				base.ForceUpdate();
				if (this._compressedStream != null && this._compressedStream.CanWrite && this._compressedStream.Length == 0L)
				{
					this._itemsHandler.SaveItemsArray();
				}
				if (this._itemsHandler != null)
				{
					for (int j = 0; j < this._itemsHandler.ItemsArray.Count; j++)
					{
						GzipItem gzipItem2 = this._itemsHandler.ItemsArray[j] as GzipItem;
						if (gzipItem2.DestinationStream != null && gzipItem2.NeedToDestroyDestinationStream)
						{
							gzipItem2.DestinationStream.Close();
						}
					}
				}
				this._isOpened = false;
			}
			if (!this._isCustomStream && this._compressedStream != null)
			{
				this._compressedStream.Close();
			}
			this._compressedStream = null;
			this._itemsHandler = null;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000178C4 File Offset: 0x000168C4
		private bool CreateNewFileInOutputFilesDir(int itemNo, out Stream destinationFileStream)
		{
			bool flag = true;
			destinationFileStream = null;
			bool flag2 = false;
			GzipArchiveItem gzipArchiveItem = new GzipArchiveItem();
			BaseArchiveItem baseArchiveItem = gzipArchiveItem;
			this._itemsHandler.ItemsArray[itemNo].GetArchiveItem(ref baseArchiveItem);
			GzipArchiveItem gzipArchiveItem2 = gzipArchiveItem;
			gzipArchiveItem2.FileName += ".gz";
			this.DoOnArchiveSingleFile(ref gzipArchiveItem);
			string fileName = gzipArchiveItem.FileName;
			string str = CompressionUtils.GetSlashedDir(this._gzipArchiverOptions.OutputFilesDir);
			if (fileName.IndexOf(":") > 0 || fileName.IndexOf("\\\\") > 0)
			{
				str = string.Empty;
			}
			if (!this._gzipArchiverOptions.CreateDirs)
			{
				fileName = Path.GetFileName(fileName);
			}
			else if (this._gzipArchiverOptions.CreateDirs)
			{
				try
				{
					string path = str + Path.GetDirectoryName(fileName);
					if (!FileUtils.DirectotyExists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
				catch (Exception innerException)
				{
					throw ExceptionBuilder.Exception(ErrorCode.CannotCreateDir, new object[]
					{
						str + Path.GetDirectoryName(fileName)
					}, innerException);
				}
			}
			string text = (str + fileName).Replace("/", "\\");
			if (File.Exists(text))
			{
				if (!this._gzipArchiverOptions.ReplaceReadOnly)
				{
					FileAttributes attributes = FileUtils.GetAttributes(text);
					if ((attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
					{
						return false;
					}
				}
				switch (this._gzipArchiverOptions.Overwrite)
				{
				case OverwriteMode.Prompt:
					this.DoOnConfirmOverwriteOutputFile(fileName, ref text, ref flag, ref flag2);
					if (!flag || flag2)
					{
						return false;
					}
					break;
				case OverwriteMode.Never:
					return false;
				case OverwriteMode.IfNewer:
				case OverwriteMode.IfOlder:
				{
					DateTime lastWriteTime = File.GetLastWriteTime(text);
					DateTime lastModificationDateTime = this.GetLastModificationDateTime(this._itemsHandler.ItemsArray[itemNo]);
					if ((lastWriteTime >= lastModificationDateTime && this._gzipArchiverOptions.Overwrite == OverwriteMode.IfNewer) || (lastWriteTime <= lastModificationDateTime && this._gzipArchiverOptions.Overwrite == OverwriteMode.IfOlder))
					{
						return false;
					}
					break;
				}
				}
			}
			try
			{
				if (File.Exists(text))
				{
					CompressionUtils.FileSetAttr(text, (FileAttributes)0);
					File.Delete(text);
				}
				destinationFileStream = new FileStream(text, FileMode.Create);
			}
			catch (Exception innerException2)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotCreateFile, new object[]
				{
					text
				}, innerException2);
			}
			return true;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600024E RID: 590 RVA: 0x00017B00 File Offset: 0x00016B00
		// (remove) Token: 0x0600024F RID: 591 RVA: 0x00017B19 File Offset: 0x00016B19
		[Description("Occurs before an application overwrited the existing file.")]
		public event ArchiverForgeBase.OnConfirmOverwriteDelegate OnConfirmOverwriteOutputFile;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000250 RID: 592 RVA: 0x00017B32 File Offset: 0x00016B32
		// (remove) Token: 0x06000251 RID: 593 RVA: 0x00017B4B File Offset: 0x00016B4B
		[Description("Occurs if a file in the archive doesn`t contain name.")]
		public event GzipForge.OnSetNameForFilesWithoutNameDelegate OnSetNameForFilesWithoutName;

		// Token: 0x06000252 RID: 594 RVA: 0x00017B64 File Offset: 0x00016B64
		protected internal virtual void DoOnConfirmOverwriteOutputFile(string sourceFileName, ref string destFileName, ref bool confirm, ref bool cancel)
		{
			if (this.OnConfirmOverwriteOutputFile != null)
			{
				this.OnConfirmOverwriteOutputFile(this, sourceFileName, ref destFileName, ref confirm, ref cancel);
				return;
			}
			cancel = false;
			switch (new ConfirmOverwriteDialog
			{
				DialogText = "The folder already contains this file: " + sourceFileName + "\n Would you like to replace the existing file?"
			}.ShowDialog())
			{
			case DialogResult.OK:
				this._gzipArchiverOptions.Overwrite = OverwriteMode.Always;
				confirm = true;
				return;
			case DialogResult.Cancel:
				confirm = false;
				cancel = true;
				break;
			case DialogResult.Abort:
			case DialogResult.Retry:
			case DialogResult.Ignore:
				break;
			case DialogResult.Yes:
				confirm = true;
				return;
			case DialogResult.No:
				confirm = false;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00017BF8 File Offset: 0x00016BF8
		protected internal virtual void DoOnArchiveSingleFile(ref GzipArchiveItem item)
		{
			if (this.OnArchiveSingleFile != null)
			{
				item.FileName = item.FileName.Replace('/', '\\');
				this.OnArchiveSingleFile(this, ref item);
				item.FileName = item.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00017C48 File Offset: 0x00016C48
		protected override void AddNewItemToItemsHandler()
		{
			GzipItem item = new GzipItem(new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
			this._itemsHandler.ItemsArray.AddItem(item);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00017C84 File Offset: 0x00016C84
		protected internal override void InitDirItem(int itemNo, bool isDirectory)
		{
			GzipItem gzipItem = this._itemsHandler.ItemsArray[itemNo] as GzipItem;
			gzipItem.Reset();
			gzipItem.IsModified = true;
			gzipItem.CompressionMethod = 8;
			gzipItem.Crc32 = uint.MaxValue;
			gzipItem.ExtraFlags = 4;
			gzipItem.Id1 = 31;
			gzipItem.Id2 = 139;
			gzipItem.NeedToStoreHeaderCRC = this.StoreHeaderCRC;
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000256 RID: 598 RVA: 0x00017CE9 File Offset: 0x00016CE9
		// (remove) Token: 0x06000257 RID: 599 RVA: 0x00017D02 File Offset: 0x00016D02
		[Description("Occurs when new gzipped file is being created.")]
		public event GzipForge.OnArchiveSingleFileDelegate OnArchiveSingleFile;

		// Token: 0x06000258 RID: 600 RVA: 0x00017D1C File Offset: 0x00016D1C
		protected internal override long GetBlockSize(IItem item)
		{
			switch (this.CompressionLevel)
			{
			case CompressionLevel.Fastest:
				return 524288L;
			case CompressionLevel.Max:
				return 1572864L;
			}
			return 1048576L;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00017D5C File Offset: 0x00016D5C
		protected internal override void InternalOpenArchive()
		{
			base.InternalOpenArchive();
			for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
			{
				if (CompressionUtils.IsNullOrEmpty(this._itemsHandler.ItemsArray[i].Name))
				{
					this.DoOnSetNameForFilesWithoutName(i);
				}
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00017DAE File Offset: 0x00016DAE
		protected internal override void InternalCreateArchive()
		{
			if (!this._createSeparateArchivers)
			{
				base.InternalCreateArchive();
				return;
			}
			this._compressedStream = new MemoryStream();
			this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, true);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00017DE0 File Offset: 0x00016DE0
		private void DoOnSetNameForFilesWithoutName(int itemNo)
		{
			if (this.OnSetNameForFilesWithoutName != null)
			{
				bool flag = false;
				bool flag2 = false;
				while (!flag && !flag2 && CompressionUtils.IsNullOrEmpty(this._itemsHandler.ItemsArray[itemNo].Name))
				{
					string empty = string.Empty;
					this.OnSetNameForFilesWithoutName(this, ref empty, ref flag, ref flag2);
					if (!CompressionUtils.IsNullOrEmpty(empty) && !flag)
					{
						this._itemsHandler.ItemsArray[itemNo].Name = empty;
						(this._itemsHandler.ItemsArray[itemNo] as GzipItem).RemoveFlagBit(3);
					}
				}
				if (flag && !flag2)
				{
					this.BuildDefaultFileName(itemNo);
				}
				if (flag2)
				{
					this._itemsHandler.ItemsArray.DeleteItem(itemNo);
					return;
				}
			}
			else
			{
				this.BuildDefaultFileName(itemNo);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00017EA0 File Offset: 0x00016EA0
		private void BuildDefaultFileName(int itemNo)
		{
			this._itemsHandler.ItemsArray[itemNo].Name = "file";
			(this._itemsHandler.ItemsArray[itemNo] as GzipItem).RemoveFlagBit(3);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00017ED9 File Offset: 0x00016ED9
		protected override BaseArchiveItem CreateNewArchiveItem()
		{
			return new GzipArchiveItem();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00017EE0 File Offset: 0x00016EE0
		public void AddItem(GzipArchiveItem item)
		{
			base.AddItem(item);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00017EEC File Offset: 0x00016EEC
		public bool FindFirst(ref GzipArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(ref baseArchiveItem);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00017F04 File Offset: 0x00016F04
		public bool FindFirst(string fileMask, ref GzipArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00017F20 File Offset: 0x00016F20
		public bool FindFirst(string fileMask, ref GzipArchiveItem archiveItem, FileAttributes searchAttr)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00017F3C File Offset: 0x00016F3C
		public bool FindFirst(string fileMask, ref GzipArchiveItem archiveItem, FileAttributes searchAttr, string exclusionMask)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00017F58 File Offset: 0x00016F58
		public bool FindNext(ref GzipArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindNext(ref baseArchiveItem);
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000264 RID: 612 RVA: 0x00017F70 File Offset: 0x00016F70
		// (remove) Token: 0x06000265 RID: 613 RVA: 0x00017F89 File Offset: 0x00016F89
		[Description("Occurs when file is being extracted from archive.")]
		public event GzipForge.OnExtractFileDelegate OnExtractFile;

		// Token: 0x06000266 RID: 614 RVA: 0x00017FA4 File Offset: 0x00016FA4
		protected internal override void DoOnExtractFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnExtractFile != null)
			{
				GzipArchiveItem gzipArchiveItem = baseItem as GzipArchiveItem;
				gzipArchiveItem.FileName = gzipArchiveItem.FileName.Replace('/', '\\');
				this.OnExtractFile(this, ref gzipArchiveItem);
				gzipArchiveItem.FileName = gzipArchiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000267 RID: 615 RVA: 0x00017FF9 File Offset: 0x00016FF9
		// (remove) Token: 0x06000268 RID: 616 RVA: 0x00018012 File Offset: 0x00017012
		[Description("Occurs when file is being stored into the archive.")]
		public event GzipForge.OnStoreFileDelegate OnStoreFile;

		// Token: 0x06000269 RID: 617 RVA: 0x0001802C File Offset: 0x0001702C
		protected internal override void DoOnStoreFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnStoreFile != null)
			{
				GzipArchiveItem gzipArchiveItem = baseItem as GzipArchiveItem;
				gzipArchiveItem.FileName = gzipArchiveItem.FileName.Replace('/', '\\');
				this.OnStoreFile(this, ref gzipArchiveItem);
				gzipArchiveItem.FileName = gzipArchiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x040001B1 RID: 433
		private readonly GzipArchiveOptions _gzipArchiverOptions;

		// Token: 0x040001B2 RID: 434
		private uint _crc32;

		// Token: 0x040001B3 RID: 435
		private long _currentBlockSize;

		// Token: 0x040001B4 RID: 436
		private long _currentItemBytesProcessed;

		// Token: 0x040001B5 RID: 437
		private long _currentItemSize;

		// Token: 0x040001B6 RID: 438
		private GzipItem _currentProcessedItem;

		// Token: 0x040001B7 RID: 439
		private byte[] _currentResBlock;

		// Token: 0x040001B8 RID: 440
		private long _currentResBlockSize;

		// Token: 0x040001B9 RID: 441
		private byte[] _currentSourceBlock;

		// Token: 0x040001BA RID: 442
		private Stream _decompressedStream;

		// Token: 0x040001BB RID: 443
		private bool _createSeparateArchivers;

		// Token: 0x040001BC RID: 444
		[Description("Specifies the compression level used for archive updating (None, Fastest, Normal, Max).")]
		public CompressionLevel CompressionLevel;

		// Token: 0x040001BD RID: 445
		internal bool StoreHeaderCRC;

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x0600026B RID: 619
		public delegate void OnArchiveSingleFileDelegate(object sender, ref GzipArchiveItem item);

		// Token: 0x0200003E RID: 62
		// (Invoke) Token: 0x0600026F RID: 623
		public delegate void OnSetNameForFilesWithoutNameDelegate(object sender, ref string fileName, ref bool cancel, ref bool removeItem);

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x06000273 RID: 627
		public delegate void OnExtractFileDelegate(object sender, ref GzipArchiveItem item);

		// Token: 0x02000040 RID: 64
		// (Invoke) Token: 0x06000277 RID: 631
		public delegate void OnStoreFileDelegate(object sender, ref GzipArchiveItem item);
	}
}
