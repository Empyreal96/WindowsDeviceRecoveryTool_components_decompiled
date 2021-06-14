using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000058 RID: 88
	[ToolboxItem(false)]
	public abstract class TarBaseForge : ArchiverForgeBase
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x0001DB18 File Offset: 0x0001CB18
		protected override void SaveRenamedItemToArchive(Stream backupFileStream, Stream compressedStream, int itemNo, int itemNoInBackupArray)
		{
			this._itemsHandler.ItemsArray[itemNoInBackupArray].WriteLocalHeaderToStream(compressedStream, 0);
			long uncompressedSize = this._itemsHandler.ItemsArrayBackup[itemNoInBackupArray].UncompressedSize;
			long count = uncompressedSize / 512L * 512L + ((uncompressedSize % 512L == 0L) ? 0L : 512L);
			CompressionUtils.CopyStream(backupFileStream, this._compressedStream, count);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001DB8C File Offset: 0x0001CB8C
		protected override long GetEndOfTheDataStreamPosition(int itemNo)
		{
			int index = this._itemsHandler.ItemsArrayBackup.Count - 1;
			long uncompressedSize = this._itemsHandler.ItemsArrayBackup[index].UncompressedSize;
			long num = uncompressedSize / 512L * 512L + ((uncompressedSize % 512L == 0L) ? 0L : 512L);
			return this._itemsHandler.ItemsArrayBackup[index].RelativeLocalHeaderOffset + (long)this._itemsHandler.ItemsArrayBackup[index].GetLocalHeaderSize() + num;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001DC1C File Offset: 0x0001CC1C
		internal override void DoCompress(bool encrypted, IItem item, int blockSize, Stream streamCompressFrom, Stream streamCompressTo, ref long processedBytesCount, ref long compSize, ref uint fcrc32)
		{
			TarWriter tarWriter = new TarWriter(streamCompressTo, this._oemCodePage, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
			long uncompressedSize = item.UncompressedSize;
			while (processedBytesCount < uncompressedSize)
			{
				long num = uncompressedSize - processedBytesCount;
				long num2 = (num > (long)blockSize) ? ((long)blockSize) : num;
				this._totalProcessedFilesSize += num2;
				if (this._progressEnabled)
				{
					DateTime now = DateTime.Now;
					TimeSpan timeLeft = new TimeSpan(0L);
					if (processedBytesCount != 0L)
					{
						timeLeft = new TimeSpan(uncompressedSize * (now.Ticks - this._currentItemOperationStartTime.Ticks) / processedBytesCount) - (now - this._currentItemOperationStartTime);
					}
					this.DoOnFileProgress(item.Name, (double)processedBytesCount / (double)uncompressedSize * 100.0, now - this._currentItemOperationStartTime, timeLeft, item.Operation, ProgressPhase.Process, ref this._progressCancel);
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
				byte[] buffer = new byte[num2];
				if ((long)streamCompressFrom.Read(buffer, 0, (int)num2) != num2)
				{
					break;
				}
				MemoryStream data = new MemoryStream(buffer);
				tarWriter.WriteContent(num2, data);
				processedBytesCount += num2;
				compSize += num2;
			}
			tarWriter.AlignTo512(item.UncompressedSize, false);
			fcrc32 = 0U;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001DDF5 File Offset: 0x0001CDF5
		protected override DateTime GetLastModificationDateTime(IItem item)
		{
			return (item as TarItem).LastFileModificationTime;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001DE02 File Offset: 0x0001CE02
		protected internal override BaseArchiveItem CreateNewArchiveItem(string fileName, string baseDir, StorePathMode storePathMode)
		{
			return new TarArchiveItem(fileName, baseDir, storePathMode);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001DE0C File Offset: 0x0001CE0C
		protected override BaseArchiveItem CreateNewArchiveItem()
		{
			return new TarArchiveItem();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001DE13 File Offset: 0x0001CE13
		protected internal override IItemsHandler CreateNewItemsHandler(Stream stream, bool create)
		{
			return new TarItemsHandler(stream, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001DE34 File Offset: 0x0001CE34
		protected internal override void ExtractItem(int itemNo, Stream destStream)
		{
			long num = this._itemsHandler.ItemsArray[itemNo].UncompressedSize;
			this._compressedStream.Seek(this._itemsHandler.ItemsArray[itemNo].RelativeLocalHeaderOffset + (long)this._itemsHandler.ItemsArray[itemNo].GetLocalHeaderSize(), SeekOrigin.Begin);
			byte[] array = new byte[512];
			long num2 = Math.Min((long)array.Length, num);
			long uncompressedSize = this._itemsHandler.ItemsArray[itemNo].UncompressedSize;
			long num3 = 0L;
			bool flag = false;
			while (num2 > 0L && !flag)
			{
				int num4;
				if (!ReadWriteHelper.ReadFromStream(array, 0, (int)num2, out num4, this._compressedStream, new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure)))
				{
					throw ExceptionBuilder.Exception(ErrorCode.ReadFromStreamFailed);
				}
				num -= (long)num4;
				num3 += (long)num4;
				TimeSpan timeElapsed = new TimeSpan(DateTime.Now.Ticks - this._currentItemOperationStartTime.Ticks);
				double progress = (double)num3 / (double)uncompressedSize * 100.0;
				TimeSpan timeLeft;
				if (num3 > 0L)
				{
					double num5 = (double)num3 / (double)timeElapsed.Ticks;
					timeLeft = new TimeSpan((long)((double)(uncompressedSize - num3) / num5));
				}
				else
				{
					timeLeft = new TimeSpan(0L);
				}
				TimeSpan timeElapsed2 = new TimeSpan(DateTime.Now.Ticks - this._operationStartTime.Ticks);
				TimeSpan timeLeft2;
				if (this._totalProcessedFilesSize > 0L)
				{
					double num6 = (double)this._totalProcessedFilesSize / (double)timeElapsed2.Ticks;
					timeLeft2 = new TimeSpan((long)((double)(this._toProcessFilesTotalSize - this._totalProcessedFilesSize) / num6));
				}
				else
				{
					timeLeft2 = new TimeSpan(0L);
				}
				double num7 = (double)this._totalProcessedFilesSize / (double)this._toProcessFilesTotalSize * 100.0;
				if (num7 > 100.0)
				{
					num7 = 100.0;
				}
				this.DoOnFileProgress(this._itemsHandler.ItemsArray[itemNo].Name, progress, timeElapsed, timeLeft, ProcessOperation.Extract, ProgressPhase.Process, ref flag);
				this.DoOnOverallProgress(num7, timeElapsed2, timeLeft2, ProcessOperation.Extract, ProgressPhase.Process, ref flag);
				this._totalProcessedFilesSize += num2;
				num3 += num2;
				if (!flag)
				{
					if (!ReadWriteHelper.WriteToStream(array, 0, (int)num2, destStream, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure)))
					{
						throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
					}
					num2 = Math.Min((long)array.Length, num);
				}
				else
				{
					this._progressCancel = true;
				}
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001E090 File Offset: 0x0001D090
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

		// Token: 0x060003B2 RID: 946 RVA: 0x0001E0EC File Offset: 0x0001D0EC
		protected internal void FillDirItem(int itemNo, string fileName, string arcFileName, bool isDirectory, bool retrieveFileDate, FileAttributes attr)
		{
			if (CompressionUtils.IsNullOrEmpty(arcFileName))
			{
				return;
			}
			string text = FileUtils.StripSlash(fileName);
			DateTime lastFileModificationTime = retrieveFileDate ? File.GetLastWriteTime(text) : DateTime.Now;
			TarItem tarItem = new TarItem(arcFileName, this._oemCodePage, new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
			this._itemsHandler.ItemsArray[itemNo] = tarItem;
			tarItem.IsModified = true;
			tarItem.SrcFileName = text;
			tarItem.LastFileModificationTime = lastFileModificationTime;
			tarItem.Crc32 = uint.MaxValue;
			tarItem.UncompressedSize = 0L;
			tarItem.ExternalAttributes = attr;
			tarItem.RelativeLocalHeaderOffset = 0L;
			if (File.Exists(text))
			{
				tarItem.GroupId = FileUtils.GetFileOwnerGroupId(text);
				tarItem.GroupName = FileUtils.GetFileOwnerGroupAccount(text);
				tarItem.UserId = FileUtils.GetFileOwnerId(text);
				tarItem.UserName = FileUtils.GetFileOwnerAccount(text);
			}
			if (attr == FileAttributes.Directory)
			{
				tarItem.TypeFlag = '5';
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001E1CC File Offset: 0x0001D1CC
		protected internal override long GetBlockSize(IItem item)
		{
			return 2097152L;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001E1D4 File Offset: 0x0001D1D4
		protected internal virtual void DoOnIncorrectFileNameDelegate(string sourceFileName, ref string destFileName, ref bool cancel)
		{
			if (this.OnIncorrectFileName != null)
			{
				this.OnIncorrectFileName(this, sourceFileName, ref destFileName, ref cancel);
				return;
			}
			Encoding encoding = Encoding.GetEncoding(this._oemCodePage);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sourceFileName.Length; i++)
			{
				string text = sourceFileName.Substring(i, 1);
				if (text.Equals(encoding.GetString(encoding.GetBytes(text))))
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append('_');
				}
			}
			destFileName = stringBuilder.ToString();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001E258 File Offset: 0x0001D258
		protected internal virtual void DoOnIncorrectStoredPathDelegate(string sourceStoredPath, ref string destStoredPath, ref bool cancel)
		{
			if (this.OnIncorrectStoredPath != null)
			{
				this.OnIncorrectStoredPath(this, sourceStoredPath, ref destStoredPath, ref cancel);
				return;
			}
			Encoding encoding = Encoding.GetEncoding(this._oemCodePage);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sourceStoredPath.Length; i++)
			{
				string text = sourceStoredPath.Substring(i, 1);
				if (text.Equals(encoding.GetString(encoding.GetBytes(text))))
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append('_');
				}
			}
			destStoredPath = stringBuilder.ToString();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001E2DC File Offset: 0x0001D2DC
		protected override void AddNewItemToItemsHandler()
		{
			TarItem item = new TarItem(new DoOnStreamOperationFailureDelegate(base.DoOnWriteToStreamFailure), new DoOnStreamOperationFailureDelegate(base.DoOnReadFromStreamFailure));
			this._itemsHandler.ItemsArray.AddItem(item);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001E318 File Offset: 0x0001D318
		protected internal override int InitializeNewItem(BaseArchiveItem baseItem)
		{
			TarArchiveItem tarArchiveItem = baseItem as TarArchiveItem;
			string fileName = tarArchiveItem.FileName;
			string storedPath = tarArchiveItem.StoredPath;
			Encoding encoding = Encoding.GetEncoding(this._oemCodePage);
			bool flag = false;
			while (!fileName.Equals(encoding.GetString(encoding.GetBytes(fileName))))
			{
				this.DoOnIncorrectFileNameDelegate(tarArchiveItem.FileName, ref fileName, ref flag);
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				return -1;
			}
			tarArchiveItem.FileName = fileName;
			while (!storedPath.Equals(encoding.GetString(encoding.GetBytes(storedPath))))
			{
				this.DoOnIncorrectStoredPathDelegate(tarArchiveItem.StoredPath, ref storedPath, ref flag);
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				return -1;
			}
			tarArchiveItem.StoredPath = storedPath;
			return base.InitializeNewItem(tarArchiveItem);
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060003B8 RID: 952 RVA: 0x0001E3C0 File Offset: 0x0001D3C0
		// (remove) Token: 0x060003B9 RID: 953 RVA: 0x0001E3D9 File Offset: 0x0001D3D9
		[Description("Occurs when the file name contains Unicode characters.")]
		public event OnIncorrectFileNameDelegate OnIncorrectFileName;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060003BA RID: 954 RVA: 0x0001E3F2 File Offset: 0x0001D3F2
		// (remove) Token: 0x060003BB RID: 955 RVA: 0x0001E40B File Offset: 0x0001D40B
		[Description("Occurs when the stored path contains Unicode characters.")]
		public event OnIncorrectStorePathDelegate OnIncorrectStoredPath;
	}
}
