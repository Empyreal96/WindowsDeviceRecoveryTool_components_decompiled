using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Text;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.ZipForge.Encryption;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200007B RID: 123
	public abstract class BaseArchiver : ArchiverForgeBase
	{
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600052E RID: 1326 RVA: 0x0002398C File Offset: 0x0002298C
		// (remove) Token: 0x0600052F RID: 1327 RVA: 0x000239A5 File Offset: 0x000229A5
		[Description("Occurs when application needs password for the encrypted file.")]
		public event BaseArchiver.OnPasswordDelegate OnPassword;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000530 RID: 1328 RVA: 0x000239BE File Offset: 0x000229BE
		// (remove) Token: 0x06000531 RID: 1329 RVA: 0x000239D7 File Offset: 0x000229D7
		[Description("Occurs when next volume should be inserted while writing a multi-volume archive.")]
		public event BaseArchiver.OnRequestBlankVolumeDelegate OnRequestBlankVolume;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000532 RID: 1330 RVA: 0x000239F0 File Offset: 0x000229F0
		// (remove) Token: 0x06000533 RID: 1331 RVA: 0x00023A09 File Offset: 0x00022A09
		[Description("Occurs when first volume should be inserted while extracting or testing a multi-volume archive.")]
		public event BaseArchiver.OnRequestFirstVolumeDelegate OnRequestFirstVolume;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000534 RID: 1332 RVA: 0x00023A22 File Offset: 0x00022A22
		// (remove) Token: 0x06000535 RID: 1333 RVA: 0x00023A3B File Offset: 0x00022A3B
		[Description("Occurs when last volume should be inserted while extracting or testing a multi-volume archive.")]
		public event BaseArchiver.OnRequestLastVolumeDelegate OnRequestLastVolume;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000536 RID: 1334 RVA: 0x00023A54 File Offset: 0x00022A54
		// (remove) Token: 0x06000537 RID: 1335 RVA: 0x00023A6D File Offset: 0x00022A6D
		[Description("Occurs when middle volume should be inserted while extracting or testing a multi-volume archive.")]
		public event BaseArchiver.OnRequestMiddleVolumeDelegate OnRequestMiddleVolume;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06000538 RID: 1336 RVA: 0x00023A86 File Offset: 0x00022A86
		// (remove) Token: 0x06000539 RID: 1337 RVA: 0x00023A9F File Offset: 0x00022A9F
		[Description("Occurs when current volume for multi-disk spanned archive is full.")]
		public event BaseArchiver.OnDiskFullDelegate OnDiskFull;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600053A RID: 1338 RVA: 0x00023AB8 File Offset: 0x00022AB8
		// (remove) Token: 0x0600053B RID: 1339 RVA: 0x00023AD1 File Offset: 0x00022AD1
		[Description("Occurs in case of failure of the current operation.")]
		public event BaseArchiver.OnProcessVolumeFailureDelegate OnProcessVolumeFailure;

		// Token: 0x0600053C RID: 1340
		internal abstract uint GetFileHeaderSignature();

		// Token: 0x0600053D RID: 1341
		internal abstract uint GetCentralDirSignature();

		// Token: 0x0600053E RID: 1342
		internal abstract uint GetCentralDirEndSignature();

		// Token: 0x0600053F RID: 1343
		internal abstract bool IsFlexCompress();

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00023AEA File Offset: 0x00022AEA
		private DirManager _currentItemsHandler
		{
			get
			{
				return this._itemsHandler as DirManager;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00023AF8 File Offset: 0x00022AF8
		private void DirItemLocalHeaderExtraFieldsNeededHandler(object sender, EventArgs e)
		{
			DirItem item = (DirItem)sender;
			this.ReadLocalHeaderExtraFields(item);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00023B14 File Offset: 0x00022B14
		private void ReadLocalHeaderExtraFields(DirItem item)
		{
			long position = this._compressedStream.Position;
			int volumeNumber = this._volumeNumber;
			uint lastVolumeNumber = 0U;
			DirItem dirItem = new DirItem();
			if (this.SpanningMode != SpanningMode.None)
			{
				if (this._currentItemsHandler.FZip64)
				{
					lastVolumeNumber = this._currentItemsHandler.Zip64CentralDirEndLocator.TotalNumberOfDisks - 1U;
				}
				else
				{
					lastVolumeNumber = (uint)this._currentItemsHandler.CentralDirEnd.DiskNumber;
				}
				uint diskStartNumber = item.DiskStartNumber;
				this.OpenVolume(false, false, (int)diskStartNumber, (int)lastVolumeNumber);
			}
			this._compressedStream.Seek(item.RelativeLocalHeaderOffset, SeekOrigin.Begin);
			byte[] array = new byte[DirItem.LocalHeaderSize()];
			this._compressedStream.Read(array, 0, DirItem.LocalHeaderSize());
			dirItem.LoadLocalHeaderFromByteArray(array, 0U);
			this._compressedStream.Seek((long)((ulong)dirItem.NameLengthToRead), SeekOrigin.Current);
			item.LoadLocalHeaderExtraFieldsFromStream(this._compressedStream, dirItem.ExtraFieldsLenRead);
			if (this.SpanningMode != SpanningMode.None)
			{
				this.OpenVolume(false, false, volumeNumber, (int)lastVolumeNumber);
			}
			this._compressedStream.Seek(position, SeekOrigin.Begin);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00023C0E File Offset: 0x00022C0E
		protected internal string GetFileComment()
		{
			if (!this._isOpened)
			{
				return string.Empty;
			}
			return this._currentItemsHandler.ArchiveComment;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00023C2C File Offset: 0x00022C2C
		protected internal void SetFileComment(string value)
		{
			if (CompressionUtils.IsNullOrEmpty(value))
			{
				value = string.Empty;
			}
			base.CheckInactive();
			this.CheckModifySpanning();
			this._currentItemsHandler.ArchiveComment = value;
			if (!base.InUpdate && this.SpanningMode == SpanningMode.None)
			{
				this._currentItemsHandler.SaveDir(false);
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00023C7C File Offset: 0x00022C7C
		protected internal void CheckModifySpanning()
		{
			if (!base.InUpdate && this._currentItemsHandler.ItemsArray.Count > 0 && this.SpanningMode != SpanningMode.None)
			{
				throw ExceptionBuilder.Exception(ErrorCode.SpanningModificationIsNotAllowed);
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00023CA9 File Offset: 0x00022CA9
		protected internal bool GetEncrypted()
		{
			if (this._isOpened && this._currentItemsHandler.ItemsArray.Count > 0)
			{
				return this._currentItemsHandler.IsEncrypted();
			}
			return !CompressionUtils.IsNullOrEmpty(this._password);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00023CE0 File Offset: 0x00022CE0
		protected internal void SetCompressionLevel(CompressionLevel newLevel)
		{
			this._compressionLevel = newLevel;
			this._compressionMode = ZipUtil.InternalGetCompressionMode(newLevel);
			this.SetCompMethod();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00023CFB File Offset: 0x00022CFB
		protected internal void SetCompressionMode(byte newMode)
		{
			if (newMode > 9)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidCompressionMode);
			}
			this._compressionMode = newMode;
			this._compressionLevel = ZipUtil.InternalGetCompressionLevel(newMode);
			this.SetCompMethod();
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00023D22 File Offset: 0x00022D22
		protected internal void SetSpanningMode(SpanningMode value)
		{
			if (this.Active)
			{
				throw ExceptionBuilder.Exception(ErrorCode.ArchiveIsOpen);
			}
			this._spanningMode = value;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00023D3B File Offset: 0x00022D3B
		protected internal override long GetBlockSize(IItem item)
		{
			return (long)((int)ZipUtil.InternalGetBlockSize((item as DirItem).CompressionMode));
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00023D50 File Offset: 0x00022D50
		protected internal override bool InternalCompressFile(Stream currentItemStream, Stream compressedStream, IItem baseItem)
		{
			DirItem dirItem = baseItem as DirItem;
			long length = currentItemStream.Length;
			long num = 0L;
			long num2 = 0L;
			uint maxValue = uint.MaxValue;
			int blockSize = (int)this.GetBlockSize(dirItem);
			currentItemStream.Position = 0L;
			bool encrypted = !CompressionUtils.IsNullOrEmpty(dirItem.Password);
			this.DoCompress(encrypted, dirItem, blockSize, currentItemStream, compressedStream, ref num, ref num2, ref maxValue);
			bool result;
			if (num == length)
			{
				result = true;
				if (this._zip64Mode == Zip64Mode.Always && dirItem.ExtraFields.Zip64ExtraField == null)
				{
					dirItem.ExtraFields.AddExtraField(new Zip64ExtraFieldData(), dirItem);
				}
				if (dirItem.IsHugeFile && this._zip64Mode == Zip64Mode.Disabled)
				{
					throw ExceptionBuilder.Exception(ErrorCode.HugeFileModeIsNotEnabled, new object[]
					{
						dirItem.SrcFileName
					});
				}
				if (length == 0L)
				{
					dirItem.ActualCompressionMethod = 0;
				}
			}
			else
			{
				if (!this._progressCancel)
				{
					throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat, new object[]
					{
						currentItemStream.Length,
						num
					});
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00023E48 File Offset: 0x00022E48
		internal bool ReadFromStream(ref byte[] buffer, long count, ref Stream rfs, ref int volumeNumber, int lastVolumeNumber)
		{
			long num;
			for (num = (long)rfs.Read(buffer, 0, (int)count); num < count; num += (long)rfs.Read(buffer, (int)num, (int)(count - num)))
			{
				volumeNumber++;
				this.OpenVolume(false, false, volumeNumber, lastVolumeNumber);
				rfs = this._compressedStream;
				rfs.Position = 0L;
			}
			return num == count;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00023EA8 File Offset: 0x00022EA8
		internal long EstimatedBytesOut(DirItem d, long size)
		{
			long compressedSize = d.CompressedSize;
			long uncompressedSize = d.UncompressedSize;
			double num = 1.0 / Math.Log(2.0);
			return (long)Math.Pow(2.0, Math.Round(Math.Log((double)((1L + uncompressedSize / compressedSize) * (size + size / 2L + 12L + 255L) & -256L)) * num));
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00023F1C File Offset: 0x00022F1C
		internal bool ZipDecompress(bool encrypted, ref int volumeNumber, int lastVolumeNumber, out long actualDecompressedSize, ref Stream rfs, Stream wfs, out uint crc32, BaseZipForgeCryptoTransform decrypter, DirItem dirItem)
		{
			this._currentProcessedItem = dirItem;
			this._decompressedStream = wfs;
			byte[] array = null;
			bool result = false;
			this._skipFile = false;
			long num = dirItem.CompressedSize;
			if (encrypted)
			{
				num -= (long)(decrypter.GetFileStorageStartBlockSize() + decrypter.GetFileStorageEndBlockSize());
				int num2 = (int)rfs.Position;
				bool flag;
				do
				{
					rfs.Position = (long)num2;
					decrypter.LoadFileStorageStartBlock(rfs, rfs.Position);
					flag = !decrypter.CheckPassword(this._password, dirItem);
					if (flag)
					{
						this.DoOnPassword(dirItem.Name, ref this._password, ref this._skipFile);
						if (decrypter.Password != this._password && !this._skipFile)
						{
							decrypter.GenerateKey(this._password);
						}
					}
				}
				while (flag && !this._skipFile);
				if (this._skipFile)
				{
					actualDecompressedSize = dirItem.UncompressedSize;
					crc32 = 0U;
					return result;
				}
			}
			array = new byte[1048578];
			ushort actualCompressionMethod = dirItem.ActualCompressionMethod;
			BaseCompressor compressor = CompressorFactory.GetCompressor((CompressionMethod)dirItem.ActualCompressionMethod, dirItem.CompressionMode);
			compressor.Init(CompressionDirection.Decompress, dirItem.CompressionMode);
			compressor.OnDecompressedBufferReady += this.decompressor_OnDecompressedBufferReady;
			try
			{
				this._currentItemBytesProcessed = 0L;
				this._currentItemSize = dirItem.UncompressedSize;
				crc32 = 0U;
				this._crc32 = uint.MaxValue;
				actualDecompressedSize = 0L;
				long num4;
				for (long num3 = 0L; num3 < num; num3 += num4)
				{
					if (num - num3 > 1048576L)
					{
						num4 = 1048576L;
					}
					else
					{
						num4 = num - num3;
					}
					if (!this.ReadFromStream(ref array, num4, ref rfs, ref volumeNumber, lastVolumeNumber))
					{
						return result;
					}
					if (encrypted)
					{
						decrypter.DecryptBuffer(array, 0, (int)num4, array, 0);
					}
					long num5;
					if (!compressor.DecompressBlock((int)num4, num3 + num4 >= num, array, out num5))
					{
						return result;
					}
					actualDecompressedSize += num5;
				}
			}
			finally
			{
				compressor.Close();
			}
			crc32 = this._crc32;
			if (encrypted)
			{
				decrypter.LoadFileStorageEndBlock(rfs, rfs.Position);
				return !dirItem.IsCorrupted(~crc32, decrypter);
			}
			return dirItem.CRC32 == ~crc32;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00024158 File Offset: 0x00023158
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
				stopDecompression = !this.WriteToStreamWithOnDiskFull(buffer, 0, outBytes, this._decompressedStream);
				ZipUtil.UpdateCRC32(buffer, (uint)outBytes, ref this._crc32);
			}
			else
			{
				this._progressCancel = true;
			}
			this._totalProcessedFilesSize += (long)outBytes;
			this._currentItemBytesProcessed += (long)outBytes;
		}

		// Token: 0x06000550 RID: 1360
		internal abstract bool FXCDecompress(bool encrypted, ref long count, ref long decompSize, ref Stream compressedStream, ref Stream destinationStream, ref int volumeNumber, int lastVolumeNumber, ref uint fcrc32, DirItem item);

		// Token: 0x06000551 RID: 1361 RVA: 0x000242E8 File Offset: 0x000232E8
		internal bool InternalDecompressFile(Stream compressedStream, Stream destinationStream, DirItem item)
		{
			BaseZipForgeCryptoTransform baseZipForgeCryptoTransform = null;
			bool flag = item.IsGeneralPurposeFlagBitSet(0);
			if (flag)
			{
				baseZipForgeCryptoTransform = ZipForgeCryptoTransformFactory.GetCryptoTransform(item.EncryptionAlgorithm, item);
				baseZipForgeCryptoTransform.Initialize(CryptoTransformMode.Decryption, item);
				baseZipForgeCryptoTransform.GenerateKey(this._password);
			}
			long num = 0L;
			long compressedSize = item.CompressedSize;
			int lastVolumeNumber = (int)((this._itemsHandler as DirManager).FZip64 ? (this._currentItemsHandler.Zip64CentralDirEndLocator.TotalNumberOfDisks - 1U) : ((uint)this._currentItemsHandler.CentralDirEnd.DiskNumber));
			int diskStartNumber = (int)item.DiskStartNumber;
			int dataOffset = item.GetDataOffset();
			compressedStream.Seek((long)dataOffset, SeekOrigin.Current);
			this._currentItemOperationStartTime = DateTime.Now;
			long num2 = (long)((ulong)item.ActualCompressionMethod);
			this._skipFile = false;
			bool flag2;
			if (num2 < 255L)
			{
				uint maxValue;
				flag2 = this.ZipDecompress(flag, ref diskStartNumber, lastVolumeNumber, out num, ref compressedStream, destinationStream, out maxValue, baseZipForgeCryptoTransform, item);
			}
			else
			{
				long num3 = 0L;
				uint maxValue = uint.MaxValue;
				flag2 = this.FXCDecompress(flag, ref num3, ref num, ref compressedStream, ref destinationStream, ref diskStartNumber, lastVolumeNumber, ref maxValue, item);
			}
			if (this._skipFile)
			{
				return true;
			}
			if (!flag2)
			{
				return flag2;
			}
			return num == item.UncompressedSize;
		}

		// Token: 0x06000552 RID: 1362
		protected internal abstract void SetCompMethod();

		// Token: 0x06000553 RID: 1363 RVA: 0x000243F4 File Offset: 0x000233F4
		protected override void SaveRenamedItemToArchive(Stream backupFileStream, Stream compressedStream, int itemNo, int itemNoInBackupArray)
		{
			string text = (this._itemsHandler.ItemsArray[itemNo] as DirItem).Name;
			text = (CompressionUtils.IsNullOrEmpty(text) ? string.Empty : text);
			if (!CompressionUtils.IsNullOrEmpty(text) && Encoding.GetEncoding(this._oemCodePage).GetString(Encoding.GetEncoding(this._oemCodePage).GetBytes(text)) != text)
			{
				(this._itemsHandler.ItemsArray[itemNo] as DirItem).SetGeneralPurposeFlagBit(11);
			}
			string text2 = (this._itemsHandler.ItemsArray[itemNo] as DirItem).Comment;
			text2 = (CompressionUtils.IsNullOrEmpty(text2) ? string.Empty : text2);
			if (!CompressionUtils.IsNullOrEmpty(text2) && Encoding.GetEncoding(this._oemCodePage).GetString(Encoding.GetEncoding(this._oemCodePage).GetBytes(text2)) != text2)
			{
				(this._itemsHandler.ItemsArray[itemNo] as DirItem).SetGeneralPurposeFlagBit(11);
			}
			byte[] bytes = (this._itemsHandler.ItemsArray[itemNo] as DirItem).ExtraFields.GetBytes(ExtraFieldsTarget.LocalHeaderExtraFields);
			(this._itemsHandler.ItemsArray[itemNo] as DirItem).WriteLocalHeaderToStream(compressedStream, 0);
			byte[] buffer = (this._itemsHandler.ItemsArray[itemNo] as DirItem).IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetBytes(text) : Encoding.GetEncoding(this._oemCodePage).GetBytes(text);
			compressedStream.Write(buffer, 0, (int)(this._itemsHandler.ItemsArray[itemNo] as DirItem).NameLength);
			compressedStream.Write(bytes, 0, bytes.Length);
			long num = (long)((ulong)(this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as DirItem).CommentLength);
			num += (this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as DirItem).CompressedSize;
			if ((this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as DirItem).DataDescriptor != null)
			{
				num += (long)(this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray] as DirItem).DataDescriptor.GetSize();
			}
			CompressionUtils.CopyStream(backupFileStream, compressedStream, num);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00024630 File Offset: 0x00023630
		protected internal override bool AddFromNewSource(int itemNo, Stream compressedStream, ref FailureAction action)
		{
			Stream stream = null;
			long oldPosition = 0L;
			string text = string.Empty;
			bool result = false;
			this.SetCompMethod();
			DirItem dirItem = this._itemsHandler.ItemsArray[itemNo] as DirItem;
			this._currentItemOperationStartTime = DateTime.Now;
			if (dirItem.Operation == ProcessOperation.Add || dirItem.Operation == ProcessOperation.Move)
			{
				this.DoOnFileProgress(dirItem.SrcFileName, 0.0, new TimeSpan(0L), new TimeSpan(0L), dirItem.Operation, ProgressPhase.Start, ref this._progressCancel);
			}
			else
			{
				this.DoOnFileProgress(dirItem.Name, 0.0, new TimeSpan(0L), new TimeSpan(0L), dirItem.Operation, ProgressPhase.Start, ref this._progressCancel);
			}
			if (this._progressCancel)
			{
				return false;
			}
			try
			{
				string name = dirItem.Name;
				if (Encoding.GetEncoding(this._oemCodePage).GetString(Encoding.GetEncoding(this._oemCodePage).GetBytes(name)) != name)
				{
					dirItem.SetGeneralPurposeFlagBit(11);
				}
				string comment = dirItem.Comment;
				if (Encoding.GetEncoding(this._oemCodePage).GetString(Encoding.GetEncoding(this._oemCodePage).GetBytes(comment)) != comment)
				{
					dirItem.SetGeneralPurposeFlagBit(11);
				}
				dirItem.IsHugeFile = false;
				oldPosition = 0L;
				if (dirItem.Stream == null)
				{
					if ((dirItem.ExternalAttributes & FileAttributes.Directory) == (FileAttributes)0)
					{
						try
						{
							stream = new FileStream(dirItem.SrcFileName, FileMode.Open, FileAccess.Read, this._archiverOptions.ShareMode);
							goto IL_1C4;
						}
						catch (Exception innerException)
						{
							throw ExceptionBuilder.Exception(ErrorCode.CannotOpenFile, new object[]
							{
								this._itemsHandler.ItemsArray[itemNo].SrcFileName
							}, innerException);
						}
					}
					stream = null;
				}
				else
				{
					stream = dirItem.Stream;
					oldPosition = stream.Position;
					stream.Position = (long)dirItem.StreamPosition;
				}
				IL_1C4:
				if (this._zip64Mode == Zip64Mode.Always && dirItem.ExtraFields.Zip64ExtraField == null)
				{
					dirItem.ExtraFields.AddExtraField(new Zip64ExtraFieldData(), dirItem);
				}
				dirItem.RelativeLocalHeaderOffset = this._compressedStream.Position;
				dirItem.ExtractVersion = ((dirItem.IsHugeFile || (stream != null && stream.Length >= (long)((ulong)-1))) ? ((this._currentItemsHandler.CentralDirEnd.Signature == this.GetCentralDirEndSignature()) ? 16685 : 45) : (this.IsFlexCompress() ? 16660 : 20));
				dirItem.UncompressedSize = (long)((ulong)((stream != null) ? ((uint)stream.Length) : 0U));
				if (this._zip64Mode == Zip64Mode.Disabled && dirItem.IsHugeFile)
				{
					base.CloseStream(itemNo, ref stream, oldPosition);
					throw ExceptionBuilder.Exception(ErrorCode.HugeFileModeIsNotEnabled);
				}
				if (this._spanningMode != SpanningMode.None)
				{
					text = base.GetTempFileName();
					compressedStream = new FileStream(text, FileMode.Create);
				}
				else
				{
					compressedStream = this._compressedStream;
				}
				try
				{
					if (this._unicodeFilenames)
					{
						dirItem.ExtraFields.AddExtraField(new UnicodeExtraFieldData(name), dirItem);
					}
					if (this._storeNTFSTimeStamps && dirItem.ExtraFields.GetExtraFieldById(10) == null)
					{
						dirItem.ExtraFields.AddExtraField(new NTFSExtraFieldData(dirItem.SrcFileName), dirItem);
					}
					compressedStream.Seek((long)dirItem.GetDataOffset(), SeekOrigin.Current);
					if (stream != null)
					{
						this.InternalCompressFile(stream, compressedStream, dirItem);
					}
					compressedStream.Position = ((this._spanningMode != SpanningMode.None) ? 0L : dirItem.RelativeLocalHeaderOffset);
					dirItem.WriteLocalHeaderToStream(compressedStream, 0);
					this._fileNameBuf = (dirItem.IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetBytes(name) : Encoding.GetEncoding(this._oemCodePage).GetBytes(name));
					if (this.IsFlexCompress() && ((this._itemsHandler as DirManager).CentralDirEnd.Signature == this.GetCentralDirEndSignature() || dirItem.ExtractVersion == 16660) && dirItem.IsGeneralPurposeFlagBitSet(0))
					{
						this.FXCEncryptFilename(ref this._fileNameBuf, (int)dirItem.NameLength, this.GetCentralDirEndSignature().ToString());
					}
					compressedStream.Write(this._fileNameBuf, 0, (int)dirItem.NameLength);
					dirItem.ExtraFields.WriteToStream(compressedStream, 0L, ExtraFieldsTarget.LocalHeaderExtraFields);
					if (this._progressCancel)
					{
						base.CloseStream(itemNo, ref stream, oldPosition);
						return false;
					}
					compressedStream.Seek(0L, SeekOrigin.End);
					if (this._spanningMode != SpanningMode.None)
					{
						compressedStream.Position = 0L;
						this.WriteToStream(compressedStream, ref this._compressedStream, ref this._volumeNumber, (long)dirItem.GetDataOffset(), 0);
						dirItem.DiskStartNumber = (uint)((ushort)this._volumeNumber);
						dirItem.RelativeLocalHeaderOffset = this._compressedStream.Position - (long)dirItem.GetDataOffset();
						this.WriteToStream(compressedStream, ref this._compressedStream, ref this._volumeNumber);
					}
				}
				finally
				{
					if (this._spanningMode != SpanningMode.None)
					{
						compressedStream.Close();
					}
					if (!text.Equals(""))
					{
						File.Delete(text);
					}
				}
				base.CloseStream(itemNo, ref stream, oldPosition);
				if (dirItem.Operation == ProcessOperation.Add || dirItem.Operation == ProcessOperation.Move)
				{
					this.DoOnFileProgress(dirItem.SrcFileName, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), dirItem.Operation, ProgressPhase.End, ref this._progressCancel);
				}
				else
				{
					this.DoOnFileProgress(dirItem.Name, 100.0, DateTime.Now - this._currentItemOperationStartTime, new TimeSpan(0L), dirItem.Operation, ProgressPhase.End, ref this._progressCancel);
				}
				result = true;
			}
			catch (ArchiverException ex)
			{
				base.CloseStream(itemNo, ref stream, oldPosition);
				if (this._compressedStream == null)
				{
					action = FailureAction.Abort;
				}
				else if (dirItem.Operation == ProcessOperation.Update)
				{
					this.DoOnProcessFileFailure(dirItem.Name, dirItem.Operation, ex.ErrorCode, ex.Args, ex.Message, ex.InnerException, ref action);
				}
				else
				{
					this.DoOnProcessFileFailure(dirItem.SrcFileName, dirItem.Operation, ex.ErrorCode, ex.Args, ex.Message, ex, ref action);
				}
			}
			catch (Exception)
			{
				base.CloseStream(itemNo, ref stream, oldPosition);
				throw;
			}
			return result;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00024C78 File Offset: 0x00023C78
		protected internal override void CalculateTotalProcessFilesSize(ref int firstItemNo)
		{
			for (int i = 0; i < this._itemsHandler.ItemsArray.Count; i++)
			{
				DirItem dirItem = this._itemsHandler.ItemsArray[i] as DirItem;
				dirItem.FireLocalHeaderExtraFieldsNeededEvent();
				if (dirItem.IsModified)
				{
					this._processedFileCount++;
					if (dirItem.Operation != ProcessOperation.Rename && dirItem.Operation != ProcessOperation.ChangeComment && dirItem.Operation != ProcessOperation.ChangeAttr)
					{
						this._toProcessFilesTotalSize += dirItem.UncompressedSize;
					}
					if (firstItemNo == -1)
					{
						firstItemNo = i;
					}
				}
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00024D0C File Offset: 0x00023D0C
		protected internal override void CalculateBackupOffset(int chgItemNo, out long backupOffset)
		{
			if (chgItemNo < this._itemsHandler.ItemsArrayBackup.Count)
			{
				backupOffset = this._itemsHandler.ItemsArrayBackup[chgItemNo].RelativeLocalHeaderOffset;
				return;
			}
			if (this._itemsHandler.ItemsArrayBackup.Count > 0)
			{
				backupOffset = this.GetEndOfTheDataStreamPosition(chgItemNo);
				return;
			}
			backupOffset = (long)(this._itemsHandler as DirManager).StubSize;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00024D78 File Offset: 0x00023D78
		protected override long GetEndOfTheDataStreamPosition(int itemNo)
		{
			if ((this._itemsHandler as DirManager).CentralDirEnd.OffsetStartDir != 4294967295U)
			{
				return (long)((ulong)(this._itemsHandler as DirManager).CentralDirEnd.OffsetStartDir);
			}
			return (this._itemsHandler as DirManager).Zip64CentralDirEnd.OffsetStartDir;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00024DCC File Offset: 0x00023DCC
		public override void CloseArchive()
		{
			base.CloseArchive();
			if (this.SpanningMode != SpanningMode.None && !CompressionUtils.IsNullOrEmpty(this.FileName) && !CompressionUtils.IsNullOrEmpty(this._volumeFileName) && this._volumeFileName != this.FileName)
			{
				string fileName = this.FileName;
				this.MakeDefaultVolumeName(ref fileName, -1);
				File.Delete(fileName);
				File.Move(this._volumeFileName, fileName);
				this._volumeFileName = fileName;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00024E40 File Offset: 0x00023E40
		protected internal override void FillDirItem(int itemNo, string fileName)
		{
			bool flag = FileUtils.DirectotyExists(fileName);
			string fileName2 = FileUtils.StripSlash(fileName);
			FileAttributes attr;
			if (File.Exists(fileName))
			{
				attr = FileUtils.GetAttributes(fileName2);
			}
			else if (flag)
			{
				attr = FileAttributes.Directory;
			}
			else
			{
				attr = FileAttributes.Archive;
			}
			string archiveFileName = CompressionUtils.GetArchiveFileName(fileName, FileUtils.GetCurrentDirectory(), this._archiverOptions.StorePath);
			this.FillDirItem(itemNo, fileName, archiveFileName, flag, true, attr);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00024E9C File Offset: 0x00023E9C
		protected internal void FillDirItem(int itemNo, string fileName, string arcFileName, bool isDirectory, bool retrieveFileDate, FileAttributes attr)
		{
			if (CompressionUtils.IsNullOrEmpty(arcFileName))
			{
				return;
			}
			string text = FileUtils.StripSlash(fileName);
			uint num;
			if (retrieveFileDate)
			{
				num = CompressionUtils.DateTimeToDosDateTime(File.GetLastWriteTime(text));
				if (num == 4294967295U)
				{
					num = CompressionUtils.DateTimeToDosDateTime(DateTime.Now);
				}
			}
			else
			{
				num = CompressionUtils.DateTimeToDosDateTime(DateTime.Now);
			}
			int num2 = (int)(num >> 16);
			int num3 = (int)(num & 65535U);
			DirItem dirItem = new DirItem(arcFileName);
			this._currentItemsHandler.ItemsArray[itemNo] = dirItem;
			dirItem.IsModified = true;
			dirItem.Password = this._password;
			dirItem.SrcFileName = text;
			dirItem.CompressionMode = ((!isDirectory) ? this._compressionMode : 0);
			dirItem.ActualCompressionMethod = this._compressionMethod;
			dirItem.Signature = this.GetCentralDirSignature();
			dirItem.ExtractVersion = ((this._currentItemsHandler.CentralDirEnd.Signature == this.GetCentralDirEndSignature()) ? 16660 : 20);
			dirItem.VersionMadeBy = 20;
			if (this._password != "")
			{
				dirItem.SetGeneralPurposeFlagBit(0);
			}
			dirItem.LastModificationTime = (ushort)num3;
			dirItem.LastModificationDate = (ushort)num2;
			dirItem.CRC32 = uint.MaxValue;
			dirItem.CompressedSize = 0L;
			dirItem.UncompressedSize = 0L;
			dirItem.ExternalAttributes = attr;
			dirItem.RelativeLocalHeaderOffset = 0L;
			dirItem.EncryptionAlgorithm = this.EncryptionAlgorithm;
			if (ZipForgeCryptoTransformFactory.IsAESEncryption(this.EncryptionAlgorithm))
			{
				dirItem.CRC32 = 0U;
			}
			if (isDirectory)
			{
				dirItem.ActualCompressionMethod = 0;
				dirItem.CRC32 = 0U;
				dirItem.SetGeneralPurposeFlagBit(1);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00025020 File Offset: 0x00024020
		protected internal override void InitDirItem(int itemNo, bool isDirectory)
		{
			DirItem dirItem = this._currentItemsHandler.ItemsArray[itemNo] as DirItem;
			dirItem.Reset();
			dirItem.IsModified = true;
			dirItem.Password = this._password;
			if (!isDirectory)
			{
				dirItem.CompressionMode = this._compressionMode;
			}
			else
			{
				dirItem.CompressionMode = 0;
			}
			dirItem.ActualCompressionMethod = this._compressionMethod;
			dirItem.Signature = this.GetCentralDirSignature();
			dirItem.ExtractVersion = ((this._currentItemsHandler.CentralDirEnd.Signature == this.GetCentralDirEndSignature()) ? 16660 : 20);
			dirItem.VersionMadeBy = 20;
			if (this._password != "")
			{
				dirItem.SetGeneralPurposeFlagBit(0);
			}
			dirItem.EncryptionAlgorithm = this.EncryptionAlgorithm;
			dirItem.CRC32 = uint.MaxValue;
			if (ZipForgeCryptoTransformFactory.IsAESEncryption(this.EncryptionAlgorithm))
			{
				dirItem.CRC32 = 0U;
			}
			if (isDirectory)
			{
				dirItem.ActualCompressionMethod = 0;
				dirItem.CRC32 = 0U;
				dirItem.SetGeneralPurposeFlagBit(1);
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00025114 File Offset: 0x00024114
		protected internal override void ExtractItem(int itemNo, Stream destStream)
		{
			if (itemNo < 0 || itemNo >= this._currentItemsHandler.ItemsArray.Count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds, new ArgumentOutOfRangeException("itemNo"));
			}
			uint lastVolumeNumber;
			if (this._currentItemsHandler.FZip64)
			{
				lastVolumeNumber = this._currentItemsHandler.Zip64CentralDirEndLocator.TotalNumberOfDisks - 1U;
			}
			else
			{
				lastVolumeNumber = (uint)this._currentItemsHandler.CentralDirEnd.DiskNumber;
			}
			uint diskStartNumber = (this._currentItemsHandler.ItemsArray[itemNo] as DirItem).DiskStartNumber;
			this.OpenVolume(false, false, (int)diskStartNumber, (int)lastVolumeNumber);
			this._compressedStream.Position = (this._currentItemsHandler.ItemsArray[itemNo] as DirItem).RelativeLocalHeaderOffset;
			if (!this.InternalDecompressFile(this._compressedStream, destStream, this._currentItemsHandler.ItemsArray[itemNo] as DirItem) && !this._progressCancel && !this._isExtractCorruptedFiles && !this._skipFile)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidCheckSum);
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0002520C File Offset: 0x0002420C
		protected override DateTime GetLastModificationDateTime(IItem baseItem)
		{
			DirItem dirItem = baseItem as DirItem;
			uint dosDateTime = (uint)((int)dirItem.LastModificationDate << 16 | (int)dirItem.LastModificationTime);
			return CompressionUtils.DosDateTimeToDateTime(dosDateTime);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00025238 File Offset: 0x00024238
		protected override void SetFileLastWriteTime(int itemNo, FileStream fileStream)
		{
			NTFSExtraFieldData ntfsextraFieldData = (this._currentItemsHandler.ItemsArray[itemNo] as DirItem).ExtraFields.GetExtraFieldById(10) as NTFSExtraFieldData;
			if (ntfsextraFieldData != null)
			{
				if (ntfsextraFieldData.FileModificationTime != 0L)
				{
					FileInfo fileInfo = new FileInfo(fileStream.Name);
					fileInfo.LastWriteTime = DateTime.FromFileTime(ntfsextraFieldData.FileModificationTime);
					fileInfo.LastAccessTime = DateTime.FromFileTime(ntfsextraFieldData.FileLastAccessTime);
					fileInfo.CreationTime = DateTime.FromFileTime(ntfsextraFieldData.FileCreationTime);
					return;
				}
			}
			else
			{
				uint dosDateTime = (uint)((int)(this._currentItemsHandler.ItemsArray[itemNo] as DirItem).LastModificationDate << 16 | (int)(this._currentItemsHandler.ItemsArray[itemNo] as DirItem).LastModificationTime);
				FileUtils.SetFileLastWriteTime(fileStream.Name, CompressionUtils.DosDateTimeToDateTime(dosDateTime));
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0002530C File Offset: 0x0002430C
		protected internal override int AddNewItemToArchive(BaseArchiveItem item, bool move)
		{
			int num = base.AddNewItemToArchive(item, move);
			bool flag = base.CheckNameMatchInMaskList(item.FullName, this.NoCompressionMasks, (item.ExternalFileAttributes & FileAttributes.Directory) != (FileAttributes)0);
			if (flag)
			{
				(this._currentItemsHandler.ItemsArray[num] as DirItem).CompressionMode = 0;
				(this._currentItemsHandler.ItemsArray[num] as DirItem).ActualCompressionMethod = ((this._compressionMethod < 255) ? 0 : 255);
			}
			return num;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00025394 File Offset: 0x00024394
		protected internal override BaseArchiveItem CreateNewArchiveItem(string fileName, string baseDir, StorePathMode storePathMode)
		{
			return new ArchiveItem(fileName, baseDir, storePathMode);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0002539E File Offset: 0x0002439E
		protected override void CopyItem(BaseArchiveItem source, IItem destination)
		{
			(destination as DirItem).CopyFrom(source, this._storeNTFSTimeStamps);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000253B2 File Offset: 0x000243B2
		protected internal void ReopenFileStream(string newFileName)
		{
			if (this._compressedStream != null)
			{
				this._compressedStream.Close();
			}
			this._compressedStream = new FileStream(newFileName, this._fileOpenMode, this._fileOpenAccess, this._fileOpenShare);
			this._volumeFileName = newFileName;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000253EC File Offset: 0x000243EC
		protected internal void MakeDefaultVolumeName(ref string volumeFileName, int volumeNumber)
		{
			string text = Path.GetDirectoryName(volumeFileName);
			if (text != "")
			{
				text += "\\";
			}
			string text2 = Path.GetExtension(volumeFileName);
			string text3 = Path.GetFileName(volumeFileName);
			text3 = text3.Substring(0, text3.Length - text2.Length);
			bool flag = (this._itemsHandler != null && this._currentItemsHandler.StubSize != 0) || this.SFXStub != "" || this.IsSFXArchive(volumeFileName);
			if (volumeNumber == -1)
			{
				if (flag && this._spanningMode == SpanningMode.Splitting)
				{
					if (this._compressionMethod < 252)
					{
						text2 = ".ZIP";
					}
					else
					{
						text2 = ".ZF";
					}
					volumeFileName = text + text3 + text2;
				}
				return;
			}
			switch (this._spanningMode)
			{
			case SpanningMode.Spanning:
				if (flag)
				{
					text2 = ".exe";
				}
				if (this._spanningOptions.AdvancedNaming)
				{
					volumeFileName = string.Concat(new object[]
					{
						text,
						text3,
						'_',
						string.Format("{0:000}", volumeNumber + 1),
						text2
					});
					return;
				}
				volumeFileName = text + text3 + text2;
				return;
			case SpanningMode.Splitting:
				if (volumeNumber == 0 && flag)
				{
					volumeFileName = text + text3 + ".exe";
					return;
				}
				if (this._spanningOptions.AdvancedNaming)
				{
					volumeFileName = string.Concat(new object[]
					{
						text,
						text3,
						'_',
						string.Format("{0:000}", volumeNumber + 1),
						text2
					});
					return;
				}
				if (this._compressionMethod < 252)
				{
					volumeFileName = text + text3 + ".Z" + string.Format("{0:00}", volumeNumber + 1);
					return;
				}
				volumeFileName = text + text3 + ".F" + string.Format("{0:00}", volumeNumber + 1);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000255DC File Offset: 0x000245DC
		protected internal long GetFreeDriveSpace(string volumeFileName)
		{
			if (this.SpanningMode == SpanningMode.Spanning)
			{
				return 1000000000L;
			}
			string fullPath = Path.GetFullPath(volumeFileName);
			int num = fullPath.IndexOf(':');
			if (num == -1)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidVolumeName);
			}
			string str = fullPath.Substring(0, num + 1);
			ManagementObject managementObject = new ManagementObject("Win32_LogicalDisk.DeviceID='" + str + "'");
			return long.Parse(managementObject.Properties["FreeSpace"].Value.ToString());
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00025655 File Offset: 0x00024655
		protected internal long WriteToStream(byte[] buffer, long count, ref Stream stream, ref int volumeNumber)
		{
			return this.WriteToStream(buffer, count, ref stream, ref volumeNumber, -1);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00025664 File Offset: 0x00024664
		protected internal virtual long WriteToStream(byte[] buffer, long count, ref Stream stream, ref int volumeNumber, int requiredFreeSpace)
		{
			long num;
			if (this._spanningMode != SpanningMode.None)
			{
				num = 0L;
				while (num < count)
				{
					long num2 = this.GetFreeDriveSpace(this._volumeFileName);
					long val = this._spanningOptions.GetVolumeSize(volumeNumber) - stream.Length;
					if (this._spanningOptions.GetVolumeSize(volumeNumber) != -1L)
					{
						num2 = Math.Min(num2, val);
					}
					if (num2 < 0L)
					{
						break;
					}
					if ((requiredFreeSpace == -1 || count - num + (long)requiredFreeSpace <= num2) && Math.Min(count - num, num2) > 0L)
					{
						long num3 = Math.Min(count - num, num2);
						stream.Write(buffer, (int)num, (int)num3);
						num += num3;
					}
					if (stream is FileStream && this.SpanningMode == SpanningMode.Spanning)
					{
						stream.Flush();
					}
					if (num < count)
					{
						volumeNumber++;
						this.OpenVolume(false, false, volumeNumber, -1);
						stream = this._compressedStream;
					}
				}
			}
			else
			{
				stream.Write(buffer, 0, (int)count);
				num = count;
			}
			return num;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0002574F File Offset: 0x0002474F
		protected internal long WriteToStream(Stream srcStream, ref Stream destStream, ref int volumeNumber)
		{
			return this.WriteToStream(srcStream, ref destStream, ref volumeNumber, -1L, -1);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0002575D File Offset: 0x0002475D
		protected internal long WriteToStream(Stream srcStream, ref Stream destStream, ref int volumeNumber, long size)
		{
			return this.WriteToStream(srcStream, ref destStream, ref volumeNumber, size, -1);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0002576C File Offset: 0x0002476C
		protected internal virtual long WriteToStream(Stream srcStream, ref Stream destStream, ref int volumeNumber, long size, int requiredFreeSpace)
		{
			long num;
			long num2;
			if (size == -1L)
			{
				num = srcStream.Length - srcStream.Position;
				num2 = 1000000L;
			}
			else
			{
				num = size;
				num2 = Math.Min(size, 1000000L);
			}
			long num3;
			long count;
			for (num3 = 0L; num3 < num; num3 += this.WriteToStream(this._tempBuffer, count, ref destStream, ref volumeNumber, requiredFreeSpace))
			{
				count = (long)srcStream.Read(this._tempBuffer, 0, (int)num2);
			}
			return num3;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000257D6 File Offset: 0x000247D6
		protected internal void WriteBufferToStream(byte[] buffer, long count, ref Stream stream, ref int volumeNumber)
		{
			this.WriteBufferToStream(buffer, count, ref stream, ref volumeNumber, -1);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000257E4 File Offset: 0x000247E4
		internal virtual void WriteBufferToStream(byte[] buffer, long count, ref Stream stream, ref int volumeNumber, int requiredFreeSpace)
		{
			if (this.WriteToStream(buffer, count, ref stream, ref volumeNumber, requiredFreeSpace) != count)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotWriteToStream);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00025800 File Offset: 0x00024800
		protected internal bool WriteToStreamWithOnDiskFull(byte[] buffer, int offset, int count, Stream stream)
		{
			long position = stream.Position;
			bool flag = false;
			bool flag2 = true;
			string fileName = this._fileName;
			do
			{
				try
				{
					flag2 = false;
					stream.Write(buffer, offset, count);
					return true;
				}
				catch
				{
					flag2 = true;
				}
				if (flag2)
				{
					this.DoOnDiskFull(-1, ref fileName, ref flag);
				}
				if (flag2 && !flag)
				{
					stream.Position = position;
				}
			}
			while (flag2 && !flag);
			if (flag)
			{
				this._progressCancel = true;
			}
			return !flag2;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0002587C File Offset: 0x0002487C
		protected internal void OpenVolume(bool firstVolume, bool lastVolume, int volumeNumber, int lastVolumeNumber)
		{
			bool flag = false;
			string text = "";
			if (this.SpanningMode != SpanningMode.None)
			{
				if (this._fileOpenMode == FileMode.Create || this._fileOpenMode == FileMode.CreateNew || (this._fileOpenMode == FileMode.OpenOrCreate && !File.Exists(this.FileName)))
				{
					if (this._compressedStream != null)
					{
						this._compressedStream.Close();
						this._compressedStream = null;
					}
					long volumeSize = this._spanningOptions.GetVolumeSize(volumeNumber);
					do
					{
						try
						{
							text = this._fileName;
							this.DoOnRequestBlankVolume(volumeNumber, ref text, ref flag);
							long freeDriveSpace = this.GetFreeDriveSpace(text);
							if (freeDriveSpace > 65536L && (volumeSize <= 0L || freeDriveSpace >= volumeSize))
							{
								break;
							}
							this.DoOnDiskFull(volumeNumber, ref text, ref flag);
						}
						catch
						{
							flag = true;
						}
					}
					while (!flag);
					if (!flag)
					{
						this.ReopenFileStream(text);
						if (this.SpanningMode == SpanningMode.Spanning)
						{
							string lpVolumeName;
							if (volumeNumber < 0)
							{
								lpVolumeName = "pkback# " + string.Format("##0", 1);
							}
							else
							{
								lpVolumeName = "pkback# " + string.Format("##0", volumeNumber + 1);
							}
							Compression.SetVolumeLabel(CompressionUtils.ExtractFileDrive(text), lpVolumeName);
						}
					}
				}
				else
				{
					string b = text;
					flag = false;
					if (this._volumeNumber != volumeNumber)
					{
						for (;;)
						{
							text = this.FileName;
							flag = true;
							if (firstVolume || volumeNumber == 0)
							{
								this.DoOnRequestFirstVolume(ref text, ref flag);
							}
							else if (lastVolume || volumeNumber == lastVolumeNumber)
							{
								this.DoOnRequestLastVolume(ref text, ref flag);
							}
							else
							{
								this.DoOnRequestMiddleVolume(volumeNumber, ref text, ref flag);
							}
							if (!flag)
							{
								if (!(text != b) && this._spanningMode != SpanningMode.Spanning)
								{
									goto IL_1B7;
								}
								if (File.Exists(text) || this._fileOpenMode == FileMode.Create)
								{
									break;
								}
							}
							if (flag)
							{
								goto Block_15;
							}
						}
						this.ReopenFileStream(text);
						this._volumeNumber = volumeNumber;
						Block_15:;
					}
				}
			}
			else
			{
				if (this._compressedStream == null)
				{
					this.ReopenFileStream(this.FileName);
				}
				flag = false;
			}
			IL_1B7:
			if (flag)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotOpenArchiveFile);
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00025A5C File Offset: 0x00024A5C
		protected internal bool IsSFXArchive(string ArcFileName)
		{
			return Path.GetExtension(ArcFileName).ToLower().Equals(".exe");
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00025A74 File Offset: 0x00024A74
		protected internal override void InternalCreateArchive()
		{
			if (this._isInMemory)
			{
				this._compressedStream = new MemoryStream();
				this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, true);
				return;
			}
			this.OpenVolume(this._spanningOptions.FSaveDirToFirstVolume, !this._spanningOptions.FSaveDirToFirstVolume, -1, -1);
			this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, true);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00025ADC File Offset: 0x00024ADC
		protected internal void InternalOpenNonSFXArchive()
		{
			this._itemsHandler = null;
			do
			{
				this._volumeNumber = 0;
				this.OpenVolume(this._spanningOptions.FSaveDirToFirstVolume, !this._spanningOptions.FSaveDirToFirstVolume, -1, -1);
				this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, false);
				this._currentItemsHandler.HasCentralDirEnd();
				this.DetectSpanning();
			}
			while (!this._currentItemsHandler.HasCentralDirEnd() && this._spanningMode != SpanningMode.None);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00025B54 File Offset: 0x00024B54
		protected internal void InternalOpenSFXArchive()
		{
			this.OpenVolume(true, false, 0, -1);
			this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, false);
			if (!this._currentItemsHandler.HasCentralDirEnd())
			{
				do
				{
					this._volumeNumber = 0;
					this.OpenVolume(false, true, -1, -1);
					this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, false);
				}
				while (!this._currentItemsHandler.HasCentralDirEnd() && this._spanningMode != SpanningMode.None);
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00025BC4 File Offset: 0x00024BC4
		protected internal void DetectSpanning()
		{
			if (this._currentItemsHandler.HasCentralDirEnd() && this._currentItemsHandler.CentralDirEnd.DiskNumber == 0)
			{
				this._spanningMode = SpanningMode.None;
				return;
			}
			this._spanningMode = SpanningMode.Splitting;
			string fileName = this._fileName;
			this.MakeDefaultVolumeName(ref fileName, (int)(this._currentItemsHandler.CentralDirEnd.DiskNumber - 1));
			if (!File.Exists(fileName))
			{
				this._spanningOptions.AdvancedNaming = !this._spanningOptions.AdvancedNaming;
				fileName = this._fileName;
				this.MakeDefaultVolumeName(ref fileName, (int)(this._currentItemsHandler.CentralDirEnd.DiskNumber - 1));
				if (!File.Exists(fileName))
				{
					this._spanningOptions.AdvancedNaming = !this._spanningOptions.AdvancedNaming;
					this._spanningMode = SpanningMode.Spanning;
				}
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00025C8C File Offset: 0x00024C8C
		protected internal override void InternalOpenArchive()
		{
			if (this._fileOpenMode == FileMode.Create || (this._fileOpenMode == FileMode.OpenOrCreate && !File.Exists(this.FileName)) || this._isInMemory)
			{
				this.InternalCreateArchive();
			}
			else
			{
				this._volumeNumber = -2;
				if (this.IsSFXArchive(this.FileName))
				{
					this.InternalOpenSFXArchive();
				}
				else
				{
					this.InternalOpenNonSFXArchive();
				}
				if (!this._currentItemsHandler.HasCentralDirEnd() && !this._isOpenCorruptedArchives)
				{
					if (!this._isCustomStream)
					{
						this._compressedStream.Close();
						this._compressedStream = null;
					}
					throw ExceptionBuilder.Exception(ErrorCode.DamagedArchive);
				}
				this.DetectSpanning();
				this._currentItemsHandler.LoadItemsArray();
				for (int i = 0; i < this._currentItemsHandler.ItemsArray.Count; i++)
				{
					(this._currentItemsHandler.ItemsArray[i] as DirItem).LocalHeaderExtraFieldsNeeded += this.DirItemLocalHeaderExtraFieldsNeededHandler;
				}
			}
			this._isOpened = true;
			base.DoAfterOpen();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00025D86 File Offset: 0x00024D86
		protected internal virtual void DoOnPassword(string fileName, ref string newPassword, ref bool skipFile)
		{
			if (this.OnPassword != null)
			{
				this.OnPassword(this, fileName, ref newPassword, ref skipFile);
				return;
			}
			throw ExceptionBuilder.Exception(ErrorCode.IncorrectPassword);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00025DA8 File Offset: 0x00024DA8
		protected internal virtual void DoOnRequestBlankVolume(int volumeNumber, ref string volumeFileName, ref bool cancel)
		{
			if (volumeNumber < 0)
			{
				volumeNumber = 0;
			}
			this.MakeDefaultVolumeName(ref volumeFileName, volumeNumber);
			cancel = false;
			if (this.OnRequestBlankVolume != null)
			{
				this.OnRequestBlankVolume(this, volumeNumber + 1, ref volumeFileName, ref cancel);
				return;
			}
			if (this.SpanningMode == SpanningMode.Spanning)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NoOnRequestBlankVolumeHandler);
			}
			cancel = false;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00025DF8 File Offset: 0x00024DF8
		protected internal virtual void DoOnRequestFirstVolume(ref string volumeFileName, ref bool cancel)
		{
			this.MakeDefaultVolumeName(ref volumeFileName, 0);
			cancel = false;
			if (this.OnRequestFirstVolume != null)
			{
				this.OnRequestFirstVolume(this, ref volumeFileName, ref cancel);
				return;
			}
			if (this.SpanningMode == SpanningMode.Spanning)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NoOnRequestFirstVolumeHandler);
			}
			cancel = (!File.Exists(volumeFileName) && this._fileOpenMode != FileMode.Create);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00025E54 File Offset: 0x00024E54
		protected internal virtual void DoOnRequestLastVolume(ref string volumeFileName, ref bool cancel)
		{
			this.MakeDefaultVolumeName(ref volumeFileName, -1);
			cancel = false;
			if (this.OnRequestLastVolume != null)
			{
				this.OnRequestLastVolume(this, ref volumeFileName, ref cancel);
				return;
			}
			if (this.SpanningMode == SpanningMode.Spanning)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NoOnRequestLastVolumeHandler);
			}
			cancel = !File.Exists(volumeFileName);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00025EA0 File Offset: 0x00024EA0
		protected internal virtual void DoOnRequestMiddleVolume(int volumeNumber, ref string volumeFileName, ref bool cancel)
		{
			this.MakeDefaultVolumeName(ref volumeFileName, volumeNumber);
			cancel = false;
			if (this.OnRequestMiddleVolume != null)
			{
				this.OnRequestMiddleVolume(this, volumeNumber + 1, ref volumeFileName, ref cancel);
				return;
			}
			if (this.SpanningMode == SpanningMode.Spanning)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NoOnRequestMiddleVolumeHandler);
			}
			cancel = !File.Exists(volumeFileName);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00025EEF File Offset: 0x00024EEF
		protected internal virtual void DoOnDiskFull(int volumeNumber, ref string volumeFileName, ref bool cancel)
		{
			cancel = false;
			if (this.OnDiskFull != null)
			{
				this.OnDiskFull(this, volumeNumber + 1, volumeFileName, ref cancel);
				return;
			}
			throw ExceptionBuilder.Exception(ErrorCode.DiskIsFull);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00025F16 File Offset: 0x00024F16
		protected internal virtual void DoOnProcessVolumeFailure(ProcessOperation operation, int volumeNumber, ref string volumeFileName, ref bool cancel)
		{
			if (this.OnProcessVolumeFailure != null)
			{
				this.OnProcessVolumeFailure(this, operation, volumeNumber, volumeFileName, ref cancel);
			}
		}

		// Token: 0x0600057B RID: 1403
		internal abstract BaseArchiver CreateArchiver();

		// Token: 0x0600057C RID: 1404 RVA: 0x00025F34 File Offset: 0x00024F34
		protected internal override IItemsHandler CreateNewItemsHandler(Stream s, bool create)
		{
			return new DirManager(s, create, this)
			{
				OemCodePage = this._oemCodePage
			};
		}

		// Token: 0x0600057D RID: 1405
		internal abstract bool FXCEncryptBuffer(int cryptoAlg, ref byte[] inBuf, int length, string password);

		// Token: 0x0600057E RID: 1406
		internal abstract bool FXCDecryptBuffer(int cryptoAlg, ref byte[] inBuf, long length, string password);

		// Token: 0x0600057F RID: 1407
		internal abstract bool FXCEncryptFilename(ref byte[] inBuf, int length, string Password);

		// Token: 0x06000580 RID: 1408
		internal abstract bool FXCDecryptFilename(ref byte[] inBuf, int length, string Password);

		// Token: 0x06000581 RID: 1409 RVA: 0x00025F58 File Offset: 0x00024F58
		protected BaseArchiver()
		{
			this._itemsHandler = null;
			this._compressedStream = null;
			this._archiverOptions = new ArchiverOptions();
			this.CompressionLevel = CompressionLevel.Fastest;
			this._isOpened = false;
			this._noCompressionMasks = new StringCollection();
			this._isExtractCorruptedFiles = false;
			this._isOpenCorruptedArchives = true;
			this._spanningMode = SpanningMode.None;
			this._spanningOptions = new SpanningOptions();
			this._zip64Mode = Zip64Mode.Disabled;
			this._password = string.Empty;
			this.SFXStub = string.Empty;
			this._encryptionAlgorithm = EncryptionAlgorithm.None;
			this._compressionMethod = 8;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00026008 File Offset: 0x00025008
		~BaseArchiver()
		{
			this.CloseArchive();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00026034 File Offset: 0x00025034
		public override void OpenArchive(Stream stream, bool create)
		{
			if (this.SpanningMode != SpanningMode.None)
			{
				throw ExceptionBuilder.Exception(ErrorCode.MultiVolumeArchiveIsNotAllowed);
			}
			this.CloseArchive();
			this._isCustomStream = true;
			if (create)
			{
				stream.SetLength(0L);
			}
			this._itemsHandler = this.CreateNewItemsHandler(stream, create);
			this._compressedStream = stream;
			if (create)
			{
				this._currentItemsHandler.SaveDir(true);
			}
			else
			{
				this._currentItemsHandler.LoadItemsArray();
			}
			for (int i = 0; i < this._currentItemsHandler.ItemsArray.Count; i++)
			{
				(this._currentItemsHandler.ItemsArray[i] as DirItem).LocalHeaderExtraFieldsNeeded += this.DirItemLocalHeaderExtraFieldsNeededHandler;
			}
			this._isOpened = true;
			base.DoAfterOpen();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000260EC File Offset: 0x000250EC
		public void MakeSFX(string SFXFileName)
		{
			if (this.SFXStub == "")
			{
				throw ExceptionBuilder.Exception(ErrorCode.StubNotSpecified);
			}
			if (!File.Exists(this.SFXStub))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
				{
					this.SFXStub
				});
			}
			if (this.SpanningMode != SpanningMode.None)
			{
				throw ExceptionBuilder.Exception(ErrorCode.MakeSFXIsNotAllowed);
			}
			if (base.InUpdate)
			{
				base.ForceUpdate();
			}
			Stream stream = this._compressedStream;
			Stream stream2 = null;
			Stream stream3 = null;
			try
			{
				stream2 = new FileStream(this.SFXStub, FileMode.Open, FileAccess.Read, FileShare.None);
				stream3 = new FileStream(SFXFileName, FileMode.Create);
				bool flag;
				DirManager dirManager;
				if (this._isOpened)
				{
					stream = this._compressedStream;
					flag = this._currentItemsHandler.IsSFXArchive();
				}
				else
				{
					stream = new FileStream(this._fileName, FileMode.Open, FileAccess.Read, FileShare.None);
					dirManager = (this.CreateNewItemsHandler(stream, false) as DirManager);
					try
					{
						dirManager.LoadItemsArray();
						flag = dirManager.IsSFXArchive();
					}
					finally
					{
					}
					stream.Position = 0L;
				}
				if (flag)
				{
					throw ExceptionBuilder.Exception(ErrorCode.ArchiveAlreadyHasSFXStub);
				}
				CompressionUtils.CopyStream(stream2, stream3);
				CompressionUtils.CopyStream(stream, stream3, stream.Length);
				dirManager = (this.CreateNewItemsHandler(stream3, false) as DirManager);
				dirManager.LoadItemsArray();
				dirManager.SaveDir(false);
			}
			finally
			{
				if (!this._isOpened)
				{
					stream.Close();
				}
				stream2.Close();
				stream3.Close();
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00026248 File Offset: 0x00025248
		public override void ChangeFilesAttr(string fileMask, FileAttributes newAttributes)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.ChangeFilesAttr(fileMask, newAttributes);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00026260 File Offset: 0x00025260
		internal void ChangeFilesIntAttr(string fileMask, ushort newAttr)
		{
			base.CheckInactive();
			BaseArchiveItem baseArchiveItem = new ArchiveItem();
			if (this.FindFirst(fileMask, ref baseArchiveItem))
			{
				this.BeginUpdate();
				try
				{
					do
					{
						(this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo] as DirItem).InternalAttributes = newAttr;
						if (!this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified)
						{
							this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified = true;
							this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation = ProcessOperation.ChangeAttr;
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

		// Token: 0x06000587 RID: 1415 RVA: 0x00026358 File Offset: 0x00025358
		public void ChangeFilesComment(string fileMask, string newComment)
		{
			if (CompressionUtils.IsNullOrEmpty(newComment))
			{
				newComment = string.Empty;
			}
			base.CheckInactive();
			BaseArchiveItem baseArchiveItem = new ArchiveItem();
			if (this.FindFirst(fileMask, ref baseArchiveItem))
			{
				this.BeginUpdate();
				try
				{
					do
					{
						(this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo] as DirItem).Comment = newComment;
						if (!this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified)
						{
							this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].IsModified = true;
							this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].Operation = ProcessOperation.ChangeComment;
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

		// Token: 0x06000588 RID: 1416 RVA: 0x00026460 File Offset: 0x00025460
		public bool IsFilePasswordValid(string fileName, string password)
		{
			base.CheckInactive();
			bool result = false;
			BaseArchiveItem baseArchiveItem = new ArchiveItem();
			if (this.FindFirst(fileName, ref baseArchiveItem))
			{
				if ((baseArchiveItem as ArchiveItem).Encrypted)
				{
					BaseZipForgeCryptoTransform cryptoTransform = ZipForgeCryptoTransformFactory.GetCryptoTransform((baseArchiveItem as ArchiveItem).EncryptionAlgorithm, this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo] as DirItem);
					cryptoTransform.Initialize(CryptoTransformMode.Decryption, this._currentItemsHandler.ItemsArray[(baseArchiveItem as ArchiveItem).Handle.ItemNo] as DirItem);
					this._compressedStream.Position = this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].RelativeLocalHeaderOffset;
					this._compressedStream.Seek((long)this._currentItemsHandler.ItemsArray[baseArchiveItem.Handle.ItemNo].GetDataOffset(), SeekOrigin.Current);
					if ((this._currentItemsHandler.ItemsArray[(baseArchiveItem as ArchiveItem).Handle.ItemNo] as DirItem).ActualCompressionMethod < 255)
					{
						cryptoTransform.GenerateKey(password);
						cryptoTransform.LoadFileStorageStartBlock(this._compressedStream, this._compressedStream.Position);
						result = cryptoTransform.CheckPassword(password, this._currentItemsHandler.ItemsArray[(baseArchiveItem as ArchiveItem).Handle.ItemNo] as DirItem);
					}
					else
					{
						byte[] array = new byte[50];
						if (this._compressedStream.Read(array, 0, 50) != 50)
						{
							return result;
						}
						FXCFileHeader fxcfileHeader = new FXCFileHeader();
						fxcfileHeader.LoadFromByteArray(array, 0U);
						FXCFileHeader fxcfileHeader2 = new FXCFileHeader();
						fxcfileHeader2.LoadFromByteArray(fxcfileHeader.GetBytes(), 0U);
						this.FXCDecryptBuffer((int)fxcfileHeader2.CryptoAlgorithm, ref fxcfileHeader2.ControlBlock, 16L, password);
						uint maxValue = uint.MaxValue;
						ZipUtil.UpdateCRC32(fxcfileHeader2.ControlBlock, 16U, ref maxValue);
						result = (~maxValue == fxcfileHeader2.ControlBlockCrc32);
					}
				}
				else
				{
					result = true;
				}
				return result;
			}
			throw ExceptionBuilder.Exception(ErrorCode.FileNotFound, new object[]
			{
				fileName
			});
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00026672 File Offset: 0x00025672
		public override void AddFiles()
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddFiles();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00026686 File Offset: 0x00025686
		public override void AddFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddFiles(fileMask, searchAttr, exclusionMask);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0002669D File Offset: 0x0002569D
		public override void AddFromBuffer(string fileName, byte[] buffer, int count)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddFromBuffer(fileName, buffer, count);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000266B4 File Offset: 0x000256B4
		public override void AddFromStream(string fileName, Stream stream, long position, long count)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddFromStream(fileName, stream, position, count);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000266CD File Offset: 0x000256CD
		public override void AddFromString(string fileName, string text)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddFromString(fileName, text);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000266E3 File Offset: 0x000256E3
		public void AddItem(ArchiveItem item)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.AddItem(item);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000266F8 File Offset: 0x000256F8
		public override void MoveFiles()
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.MoveFiles();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0002670C File Offset: 0x0002570C
		public override void MoveFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.MoveFiles(fileMask, searchAttr, exclusionMask);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00026723 File Offset: 0x00025723
		public override void RenameFile(string oldName, string newName)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.RenameFile(oldName, newName);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00026739 File Offset: 0x00025739
		public override void DeleteFiles()
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.DeleteFiles();
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0002674D File Offset: 0x0002574D
		public override void DeleteFiles(string fileMask, FileAttributes searchAttributes, string exclusionMask)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.DeleteFiles(fileMask, searchAttributes, exclusionMask);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00026764 File Offset: 0x00025764
		public override void UpdateFiles()
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.UpdateFiles();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00026778 File Offset: 0x00025778
		public override void UpdateFiles(string fileMask, FileAttributes searchAttr, string exclusionMask)
		{
			base.CheckInactive();
			this.CheckModifySpanning();
			base.UpdateFiles(fileMask, searchAttr, exclusionMask);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0002678F File Offset: 0x0002578F
		public void RepairArchive()
		{
			this.RepairArchive("");
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0002679C File Offset: 0x0002579C
		public void RepairArchive(string outputFileName)
		{
			bool extractCorruptedFiles = false;
			string text = "";
			if (this._isOpened)
			{
				throw ExceptionBuilder.Exception(ErrorCode.ArchiveIsOpen);
			}
			string text2 = outputFileName;
			if (outputFileName == "")
			{
				text2 = base.GetTempFileName();
			}
			FileStream fileStream;
			try
			{
				if (outputFileName == "")
				{
					text = text2;
					fileStream = new FileStream(text, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
					fileStream.Close();
					File.Delete(text2);
					fileStream = new FileStream(text2, FileMode.Create);
					fileStream.Close();
					File.Delete(text2);
				}
				else
				{
					text = outputFileName;
					File.Delete(text);
					fileStream = new FileStream(text, FileMode.Create);
					fileStream.Close();
					File.Delete(text);
				}
			}
			catch (Exception innerException)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CannotCreateOutputFile, new object[]
				{
					text
				}, innerException);
			}
			try
			{
				bool isOpenCorruptedArchives = this._isOpenCorruptedArchives;
				this._isOpenCorruptedArchives = true;
				this.OpenArchive(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				extractCorruptedFiles = this.ExtractCorruptedFiles;
				this.ExtractCorruptedFiles = true;
				base.GetType();
				BaseArchiver baseArchiver = this.CreateArchiver();
				baseArchiver.FileName = text2;
				baseArchiver.OpenArchive(FileMode.Create);
				string tempFileName = base.GetTempFileName();
				fileStream = new FileStream(tempFileName, FileMode.Open);
				try
				{
					for (int i = 0; i < this._currentItemsHandler.ItemsArray.Count; i++)
					{
						if ((this._currentItemsHandler.ItemsArray[i].ExternalAttributes & FileAttributes.Directory) != (FileAttributes)0)
						{
							baseArchiver._currentItemsHandler.ItemsArray.AddItem(this._currentItemsHandler.ItemsArray[i]);
						}
						else
						{
							fileStream.SetLength(0L);
							try
							{
								this.ExtractToStream(this._currentItemsHandler.ItemsArray[i].Name, fileStream);
								baseArchiver.Password = this.Password;
								baseArchiver._compressionMode = this._compressionMode;
								baseArchiver._compressionMethod = this._compressionMethod;
								baseArchiver.AddFromStream(this._currentItemsHandler.ItemsArray[i].Name, fileStream);
								baseArchiver.ChangeFilesAttr(this._currentItemsHandler.ItemsArray[i].Name, this._currentItemsHandler.ItemsArray[i].ExternalAttributes);
								baseArchiver.ChangeFilesIntAttr(this._currentItemsHandler.ItemsArray[i].Name, (this._currentItemsHandler.ItemsArray[i] as DirItem).InternalAttributes);
							}
							catch
							{
							}
						}
					}
				}
				finally
				{
					this._isOpenCorruptedArchives = isOpenCorruptedArchives;
				}
			}
			finally
			{
				fileStream.Close();
				this.CloseArchive();
				BaseArchiver baseArchiver = null;
				this.ExtractCorruptedFiles = extractCorruptedFiles;
			}
			if (outputFileName == "")
			{
				File.Delete(this.FileName);
				File.Copy(text2, this.FileName, false);
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00026AA0 File Offset: 0x00025AA0
		public bool IsValidArchiveFile()
		{
			bool result;
			if (!this._isOpened)
			{
				if (File.Exists(this._fileName))
				{
					this._compressedStream = new FileStream(this._fileName, FileMode.Open, FileAccess.Read, FileShare.None);
					try
					{
						this._itemsHandler = this.CreateNewItemsHandler(this._compressedStream, false);
						bool isOpenCorruptedArchives = this._isOpenCorruptedArchives;
						this._isOpenCorruptedArchives = false;
						try
						{
							if (this._currentItemsHandler.HasCentralDirEnd())
							{
								result = true;
							}
							else
							{
								try
								{
									this._currentItemsHandler.LoadItemsArray();
									result = true;
								}
								catch
								{
									result = false;
								}
							}
						}
						finally
						{
							this._isOpenCorruptedArchives = isOpenCorruptedArchives;
							this._itemsHandler = null;
						}
						return result;
					}
					finally
					{
						this._compressedStream.Close();
						this._compressedStream = null;
					}
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00026B78 File Offset: 0x00025B78
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00026B80 File Offset: 0x00025B80
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Comment
		{
			get
			{
				return this.GetFileComment();
			}
			set
			{
				this.SetFileComment(value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00026B89 File Offset: 0x00025B89
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00026B91 File Offset: 0x00025B91
		[Description("Specifies the archive encryption algorithm")]
		public EncryptionAlgorithm EncryptionAlgorithm
		{
			get
			{
				return this._encryptionAlgorithm;
			}
			set
			{
				this._encryptionAlgorithm = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00026B9A File Offset: 0x00025B9A
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00026BA2 File Offset: 0x00025BA2
		[Description("Specifies whether the corrupted archives can be opened.")]
		public bool OpenCorruptedArchives
		{
			get
			{
				return this._isOpenCorruptedArchives;
			}
			set
			{
				this._isOpenCorruptedArchives = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00026BAB File Offset: 0x00025BAB
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00026BB3 File Offset: 0x00025BB3
		[Description("Specifies whether the partially corrupted files can be extracted from damaged archive.")]
		public bool ExtractCorruptedFiles
		{
			get
			{
				return this._isExtractCorruptedFiles;
			}
			set
			{
				this._isExtractCorruptedFiles = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00026BBC File Offset: 0x00025BBC
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00026BC4 File Offset: 0x00025BC4
		[Description("Specifies the compression algorithm to use to compress archive items")]
		public CompressionMethod CompressionMethod
		{
			get
			{
				return (CompressionMethod)this._compressionMethod;
			}
			set
			{
				this._compressionMethod = (ushort)value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00026BCD File Offset: 0x00025BCD
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00026BD5 File Offset: 0x00025BD5
		[Description("Specifies the compression level used for archive updating (None, Fastest, Normal, Max).")]
		public CompressionLevel CompressionLevel
		{
			get
			{
				return this._compressionLevel;
			}
			set
			{
				this.SetCompressionLevel(value);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00026BDE File Offset: 0x00025BDE
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00026BE6 File Offset: 0x00025BE6
		[Description("Specifies the compression level used for archive updating (from 0 to 9).")]
		public byte CompressionMode
		{
			get
			{
				return this._compressionMode;
			}
			set
			{
				this.SetCompressionMode(value);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00026BEF File Offset: 0x00025BEF
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00026BF7 File Offset: 0x00025BF7
		[Description("Specifies the password for files stored within the archive.")]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				this._password = value;
				if (value != "" && this._encryptionAlgorithm == EncryptionAlgorithm.None)
				{
					this._encryptionAlgorithm = EncryptionAlgorithm.PkzipClassic;
				}
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00026C2B File Offset: 0x00025C2B
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00026C33 File Offset: 0x00025C33
		[Obsolete("This property no longer used, the system's temporary directory always used.")]
		[Description("Specifies a temporary directory to use during archiving operations.")]
		public string TempDir
		{
			get
			{
				return this._tempDir;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				this._tempDir = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00026C4B File Offset: 0x00025C4B
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00026C53 File Offset: 0x00025C53
		[Description("Specifies how the archive file will be stored on disk: as a single file, as multiple files or it will be spanned across removable disks.")]
		public SpanningMode SpanningMode
		{
			get
			{
				return this._spanningMode;
			}
			set
			{
				this.SetSpanningMode(value);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00026C5C File Offset: 0x00025C5C
		[Description("Specifies how the archive file will be splitted or spanned.")]
		public SpanningOptions SpanningOptions
		{
			get
			{
				return this._spanningOptions;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00026C64 File Offset: 0x00025C64
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00026C6C File Offset: 0x00025C6C
		[Description("Specifies the file name of the executable stub.")]
		public string SFXStub
		{
			get
			{
				return this._sfxStub;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					value = string.Empty;
				}
				this._sfxStub = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00026C84 File Offset: 0x00025C84
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Specifies files or wildcards for files that will be stored without compression.")]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
		public StringCollection NoCompressionMasks
		{
			get
			{
				return this._noCompressionMasks;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00026C8C File Offset: 0x00025C8C
		public bool FindFirst(ref ArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(ref baseArchiveItem);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00026CA4 File Offset: 0x00025CA4
		public bool FindFirst(string fileMask, ref ArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00026CC0 File Offset: 0x00025CC0
		public bool FindFirst(string fileMask, ref ArchiveItem archiveItem, FileAttributes searchAttr)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00026CDC File Offset: 0x00025CDC
		public bool FindFirst(string fileMask, ref ArchiveItem archiveItem, FileAttributes searchAttr, string exclusionMask)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindFirst(fileMask, ref baseArchiveItem, searchAttr, exclusionMask);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00026CF8 File Offset: 0x00025CF8
		public bool FindNext(ref ArchiveItem archiveItem)
		{
			BaseArchiveItem baseArchiveItem = archiveItem;
			return base.FindNext(ref baseArchiveItem);
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060005B6 RID: 1462 RVA: 0x00026D10 File Offset: 0x00025D10
		// (remove) Token: 0x060005B7 RID: 1463 RVA: 0x00026D29 File Offset: 0x00025D29
		[Description("Occurs when file is being extracted from archive.")]
		public event BaseArchiver.OnExtractFileDelegate OnExtractFile;

		// Token: 0x060005B8 RID: 1464 RVA: 0x00026D44 File Offset: 0x00025D44
		protected internal override void DoOnExtractFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnExtractFile != null)
			{
				ArchiveItem archiveItem = baseItem as ArchiveItem;
				archiveItem.FileName = archiveItem.FileName.Replace('/', '\\');
				this.OnExtractFile(this, ref archiveItem);
				archiveItem.FileName = archiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060005B9 RID: 1465 RVA: 0x00026D99 File Offset: 0x00025D99
		// (remove) Token: 0x060005BA RID: 1466 RVA: 0x00026DB2 File Offset: 0x00025DB2
		[Description("Occurs when file is being stored into the archive.")]
		public event BaseArchiver.OnStoreFileDelegate OnStoreFile;

		// Token: 0x060005BB RID: 1467 RVA: 0x00026DCC File Offset: 0x00025DCC
		protected internal override void DoOnStoreFile(ref BaseArchiveItem baseItem)
		{
			if (this.OnStoreFile != null)
			{
				ArchiveItem archiveItem = baseItem as ArchiveItem;
				archiveItem.FileName = archiveItem.FileName.Replace('/', '\\');
				this.OnStoreFile(this, ref archiveItem);
				archiveItem.FileName = archiveItem.FileName.Replace('\\', '/');
			}
		}

		// Token: 0x0400030D RID: 781
		private const int _tempBufferSize = 1000000;

		// Token: 0x04000315 RID: 789
		internal string _volumeFileName;

		// Token: 0x04000316 RID: 790
		internal int _volumeNumber;

		// Token: 0x04000317 RID: 791
		internal string _tempDir;

		// Token: 0x04000318 RID: 792
		private string _password;

		// Token: 0x04000319 RID: 793
		private EncryptionAlgorithm _encryptionAlgorithm;

		// Token: 0x0400031A RID: 794
		internal string _sfxStub;

		// Token: 0x0400031B RID: 795
		internal SpanningMode _spanningMode;

		// Token: 0x0400031C RID: 796
		internal SpanningOptions _spanningOptions;

		// Token: 0x0400031D RID: 797
		internal bool _isExtractCorruptedFiles;

		// Token: 0x0400031E RID: 798
		private long _currentItemSize;

		// Token: 0x0400031F RID: 799
		private long _currentItemBytesProcessed;

		// Token: 0x04000320 RID: 800
		private DirItem _currentProcessedItem;

		// Token: 0x04000321 RID: 801
		protected internal ushort _compressionMethod;

		// Token: 0x04000322 RID: 802
		protected internal byte _compressionMode;

		// Token: 0x04000323 RID: 803
		protected internal CompressionLevel _compressionLevel;

		// Token: 0x04000324 RID: 804
		private Stream _decompressedStream;

		// Token: 0x04000325 RID: 805
		private uint _crc32;

		// Token: 0x04000326 RID: 806
		protected internal Zip64Mode _zip64Mode;

		// Token: 0x04000327 RID: 807
		protected internal bool _unicodeFilenames;

		// Token: 0x04000328 RID: 808
		protected internal bool _storeNTFSTimeStamps;

		// Token: 0x04000329 RID: 809
		private readonly byte[] _tempBuffer = new byte[1000000];

		// Token: 0x0400032A RID: 810
		private byte[] _fileNameBuf = new byte[257];

		// Token: 0x0400032B RID: 811
		private StringCollection _noCompressionMasks;

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x060005BD RID: 1469
		public delegate void OnPasswordDelegate(object sender, string fileName, ref string newPassword, ref bool skipFile);

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x060005C1 RID: 1473
		public delegate void OnRequestBlankVolumeDelegate(object sender, int volumeNumber, ref string volumeFileName, ref bool cancel);

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x060005C5 RID: 1477
		public delegate void OnRequestFirstVolumeDelegate(object sender, ref string volumeFileName, ref bool cancel);

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x060005C9 RID: 1481
		public delegate void OnRequestLastVolumeDelegate(object sender, ref string volumeFileName, ref bool cancel);

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x060005CD RID: 1485
		public delegate void OnRequestMiddleVolumeDelegate(object sender, int volumeNumber, ref string volumeFileName, ref bool cancel);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x060005D1 RID: 1489
		public delegate void OnDiskFullDelegate(object sender, int volumeNumber, string volumeFileName, ref bool cancel);

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x060005D5 RID: 1493
		public delegate void OnProcessVolumeFailureDelegate(object sender, ProcessOperation operation, int volumeNumber, string volumeFileName, ref bool cancel);

		// Token: 0x02000083 RID: 131
		// (Invoke) Token: 0x060005D9 RID: 1497
		public delegate void OnExtractFileDelegate(object sender, ref ArchiveItem item);

		// Token: 0x02000084 RID: 132
		// (Invoke) Token: 0x060005DD RID: 1501
		public delegate void OnStoreFileDelegate(object sender, ref ArchiveItem item);
	}
}
