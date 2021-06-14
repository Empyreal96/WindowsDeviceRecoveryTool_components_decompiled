using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.ZipForge.Encryption;

namespace ComponentAce.Compression.ZipForgeRealTime
{
	// Token: 0x02000074 RID: 116
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(resfinder), "ComponentAce.Compression.Resources.rtzip16.ico")]
	public class ZipForgeRealTime : Component
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00021B13 File Offset: 0x00020B13
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00021B1B File Offset: 0x00020B1B
		[Description("Specifies the comment for whole archive.")]
		public string CommentsToArchive
		{
			get
			{
				return this._commentsToArchive;
			}
			set
			{
				this._commentsToArchive = (CompressionUtils.IsNullOrEmpty(value) ? string.Empty : value);
				if (this._dmHandle != null)
				{
					this._dmHandle.ArchiveComment = this._commentsToArchive;
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00021B4C File Offset: 0x00020B4C
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00021B72 File Offset: 0x00020B72
		[Description("Specifies the compression level used for archive")]
		public RealTimeCompressionLevel CompressionLevel
		{
			get
			{
				return (RealTimeCompressionLevel)Enum.Parse(typeof(RealTimeCompressionLevel), this._compressionLevel.ToString());
			}
			set
			{
				this._compressionLevel = (CompressionLevel)Enum.Parse(typeof(CompressionLevel), value.ToString());
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00021B99 File Offset: 0x00020B99
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00021BB5 File Offset: 0x00020BB5
		[Description("Specifies the compression method used for archive.")]
		public RealTimeCompressionMethod CompressionMethod
		{
			get
			{
				return (RealTimeCompressionMethod)Enum.ToObject(typeof(RealTimeCompressionMethod), this._compressionMethod);
			}
			set
			{
				this._compressionMethod = (ushort)value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00021BBE File Offset: 0x00020BBE
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00021BE4 File Offset: 0x00020BE4
		[Description("Specifies the encryption algorithm used to encrypt files in the archive.")]
		public RealTimeEncryptionAlgorithm EncryptionAlgorithm
		{
			get
			{
				return (RealTimeEncryptionAlgorithm)Enum.Parse(typeof(RealTimeEncryptionAlgorithm), this._encryptionAlgorithm.ToString());
			}
			set
			{
				this._encryptionAlgorithm = (EncryptionAlgorithm)Enum.Parse(typeof(EncryptionAlgorithm), value.ToString());
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00021C0B File Offset: 0x00020C0B
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00021C13 File Offset: 0x00020C13
		[Description("Specifies password used to encrypt files in the archive.")]
		public string EncryptionPassword
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
				if (!CompressionUtils.IsNullOrEmpty(this._password))
				{
					this.EncryptionAlgorithm = RealTimeEncryptionAlgorithm.Aes128;
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00021C43 File Offset: 0x00020C43
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00021C4B File Offset: 0x00020C4B
		[Description("Specifies whether to use xCeed unicode extra field to store filename in the unicode format.")]
		public bool UseUnicodeFileNameExtraField
		{
			get
			{
				return this._useUnicodeFileNameExtraField;
			}
			set
			{
				this._useUnicodeFileNameExtraField = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00021C54 File Offset: 0x00020C54
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00021C5F File Offset: 0x00020C5F
		[Description("Specifies whether to use Zip64 format to create archives over 4GB.")]
		public bool UseZip64
		{
			get
			{
				return this._zip64Mode == Zip64Mode.Always;
			}
			set
			{
				this._zip64Mode = (value ? Zip64Mode.Always : Zip64Mode.Disabled);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00021C6E File Offset: 0x00020C6E
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00021C76 File Offset: 0x00020C76
		[Description("Specifies stream used for archiver operations.")]
		public Stream CompressedStream
		{
			get
			{
				return this._compressedStream;
			}
			set
			{
				this._compressedStream = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00021C7F File Offset: 0x00020C7F
		[Browsable(false)]
		public virtual bool Active
		{
			get
			{
				return this._isOpened;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00021C87 File Offset: 0x00020C87
		protected internal uint CentralDirEndSignature
		{
			get
			{
				return 101010256U;
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060004DF RID: 1247 RVA: 0x00021C8E File Offset: 0x00020C8E
		// (remove) Token: 0x060004E0 RID: 1248 RVA: 0x00021CA7 File Offset: 0x00020CA7
		[Description("Occurs when storing file to the archive operation progress indication value.")]
		public event ZipForgeRealTime.OnFileProgressDelegate OnFileProgress;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060004E1 RID: 1249 RVA: 0x00021CC0 File Offset: 0x00020CC0
		// (remove) Token: 0x060004E2 RID: 1250 RVA: 0x00021CD9 File Offset: 0x00020CD9
		[Description("Occurs when specified file name already exists in the archive.")]
		public event ZipForgeRealTime.OnFileRenameDelegate OnFileRename;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060004E3 RID: 1251 RVA: 0x00021CF2 File Offset: 0x00020CF2
		// (remove) Token: 0x060004E4 RID: 1252 RVA: 0x00021D0B File Offset: 0x00020D0B
		[Description("Occurs in case of failure of the current operation.")]
		public event ZipForgeRealTime.OnProcessFileFailureDelegate OnProcessFileFailure;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060004E5 RID: 1253 RVA: 0x00021D24 File Offset: 0x00020D24
		// (remove) Token: 0x060004E6 RID: 1254 RVA: 0x00021D3D File Offset: 0x00020D3D
		[Description("Occurs in case of failure of the write to stream operation.")]
		public event ZipForgeRealTime.OnWriteToStreamFailureDelegate OnWriteToStreamFailure;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060004E7 RID: 1255 RVA: 0x00021D56 File Offset: 0x00020D56
		// (remove) Token: 0x060004E8 RID: 1256 RVA: 0x00021D6F File Offset: 0x00020D6F
		[Description("Occurs when application needs password for the encrypted file.")]
		public event ZipForgeRealTime.OnPasswordDelegate OnPassword;

		// Token: 0x060004E9 RID: 1257 RVA: 0x00021D88 File Offset: 0x00020D88
		public ZipForgeRealTime() : this(null)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00021D94 File Offset: 0x00020D94
		public ZipForgeRealTime(Stream stream)
		{
			this.CommentsToArchive = string.Empty;
			this.CompressionLevel = RealTimeCompressionLevel.Fastest;
			this._compressionMode = ZipUtil.InternalGetCompressionMode(this._compressionLevel);
			this.CompressionMethod = RealTimeCompressionMethod.Deflate;
			this.EncryptionAlgorithm = RealTimeEncryptionAlgorithm.None;
			this._password = string.Empty;
			this.UseUnicodeFileNameExtraField = false;
			this.UseZip64 = false;
			this._oemCodePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._compressedStream = stream;
			this._dmHandle = null;
			this._isOpened = false;
			this._oemCodePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._zip64Mode = Zip64Mode.Disabled;
			this.UseUnicodeFileNameExtraField = false;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00021E40 File Offset: 0x00020E40
		public void OpenArchive(Stream stream, bool createArchive)
		{
			if (this._compressedStream == null)
			{
				ExceptionBuilder.Exception(ErrorCode.CompressedStreamNotSpecified);
			}
			this._dmHandle = new RealTimeDirManager(this);
			this._dmHandle.ArchiveComment = this._commentsToArchive;
			this._dmHandle.OemCodePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._isOpened = true;
			this._createArchive = createArchive;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00021EA2 File Offset: 0x00020EA2
		public void OpenArchive(bool createArchive)
		{
			this.OpenArchive(this._compressedStream, createArchive);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00021EB4 File Offset: 0x00020EB4
		public bool FindFirst(ref ArchiveItem f)
		{
			this.CheckInactive();
			BaseArchiveItem baseArchiveItem = f;
			return CentralDirectorySearcher.FindFirst(ref baseArchiveItem, FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory | FileAttributes.Archive | FileAttributes.Device | FileAttributes.Normal | FileAttributes.Temporary | FileAttributes.SparseFile | FileAttributes.ReparsePoint | FileAttributes.Compressed | FileAttributes.Offline | FileAttributes.NotContentIndexed | FileAttributes.Encrypted, this._dmHandle.CentralDirectory, true, new StringCollection(), new StringCollection());
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00021EEC File Offset: 0x00020EEC
		public bool FindFirst(string fileMask, ref ArchiveItem f)
		{
			this.CheckInactive();
			BaseArchiveItem baseArchiveItem = f;
			return CentralDirectorySearcher.FindFirst(fileMask, ref baseArchiveItem, this._dmHandle.CentralDirectory, true);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00021F18 File Offset: 0x00020F18
		public bool FindNext(ref ArchiveItem f)
		{
			this.CheckInactive();
			BaseArchiveItem baseArchiveItem = f;
			return CentralDirectorySearcher.FindNext(ref baseArchiveItem, this._dmHandle.CentralDirectory, true, new StringCollection(), new StringCollection());
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00021F4B File Offset: 0x00020F4B
		public bool AddFromStream(string fileName, Stream fileStream)
		{
			return this.AddFromStream(fileName, fileStream, string.Empty);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00021F5C File Offset: 0x00020F5C
		public bool AddFromStream(string fileName, Stream fileStream, string fileComment)
		{
			this.CheckInactive();
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}
			if (fileComment == null)
			{
				fileComment = string.Empty;
			}
			if (this._compressedStream == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNullPointer, new object[]
				{
					"compressedStream"
				});
			}
			int num = this.AddItemToCentralDirectory(fileName, fileStream, fileComment);
			this._progressCancel = false;
			bool flag;
			if (num >= 0)
			{
				flag = this.SaveDirectoryItemToStream(num);
				if (!flag && !this._progressCancel)
				{
					throw ExceptionBuilder.Exception(ErrorCode.ErrorOccursDuringSavingFileStream);
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00021FE8 File Offset: 0x00020FE8
		public void CloseArchive()
		{
			if (!this._isOpened)
			{
				this._dmHandle = null;
			}
			else
			{
				this._isOpened = false;
				if (this._compressedStream != null && this._createArchive)
				{
					MemoryStream memoryStream = new MemoryStream();
					this._dmHandle.SaveDir(memoryStream, ref this._sentBytes);
					this.WriteToStream(memoryStream.ToArray(), 0, (int)memoryStream.Length, this._compressedStream);
				}
			}
			this._dmHandle = null;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00022058 File Offset: 0x00021058
		protected internal void DoOnFileProgress(string fileName, long bytesProcessed, TimeSpan timeElapsed, ProgressPhase progressPhase, ref bool cancel)
		{
			if (this.OnFileProgress != null)
			{
				string fileName2 = fileName.Replace('/', '\\');
				this.OnFileProgress(this, fileName2, bytesProcessed, timeElapsed, progressPhase, ref cancel);
				return;
			}
			cancel = false;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00022090 File Offset: 0x00021090
		protected internal void DoOnFileRename(string fileName, ref string newFileName, ref bool cancel)
		{
			if (this.OnFileRename != null)
			{
				string fileName2 = fileName.Replace('/', '\\');
				this.OnFileRename(this, fileName2, ref newFileName, ref cancel);
				return;
			}
			cancel = true;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000220C3 File Offset: 0x000210C3
		protected internal void DoOnProcessFileFailure(string fileName, string errorMessage, Exception innerException)
		{
			if (this.OnProcessFileFailure != null)
			{
				this.OnProcessFileFailure(this, fileName, errorMessage, innerException);
				return;
			}
			throw innerException;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000220DE File Offset: 0x000210DE
		protected internal void DoOnWriteToStreamFailed(Exception innerException, ref bool cancel)
		{
			cancel = false;
			if (this.OnWriteToStreamFailure != null)
			{
				this.OnWriteToStreamFailure(this, innerException, ref cancel);
				return;
			}
			throw innerException;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000220FC File Offset: 0x000210FC
		private int AddItemToCentralDirectory(string fileName, Stream fileStream, string fileComment)
		{
			int num = 0;
			string fileName2 = fileName;
			bool flag = false;
			int num2;
			if (this._dmHandle.CentralDirectory.FileExists(fileName2, ref num))
			{
				do
				{
					this.DoOnFileRename(fileName, ref fileName2, ref flag);
				}
				while (!flag && this._dmHandle.CentralDirectory.FileExists(fileName2, ref num));
				if (flag)
				{
					num2 = -1;
				}
				else
				{
					num2 = this.AddItemToCentralDirectory(fileName2, fileStream, fileComment);
				}
			}
			else
			{
				this._dmHandle.CentralDirectory.AddItem(new DirItem());
				num2 = this._dmHandle.CentralDirectory.Count - 1;
				this.InitDirItemFromArchiveParams(num2);
				this._dmHandle.CentralDirectory[num2].Stream = fileStream;
				this._dmHandle.CentralDirectory[num2].StreamPosition = 0;
				this._dmHandle.CentralDirectory[num2].UncompressedSize = 0L;
				this._dmHandle.CentralDirectory[num2].Name = fileName;
				(this._dmHandle.CentralDirectory[num2] as DirItem).Comment = fileComment;
			}
			return num2;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002220C File Offset: 0x0002120C
		private bool DoCompress(bool isEncrypted, DirItem directoryItem, int blockSize, Stream fileStream, Stream compressedStream, int firstByte, ref long processedBytesCount, ref long compressedBytesCount, ref uint fcrc32)
		{
			if (directoryItem == null)
			{
				throw new ArgumentNullException("directoryItem");
			}
			if (fileStream == null)
			{
				throw new ArgumentNullException("compressedStream");
			}
			if (compressedStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}
			long num = 0L;
			long num2 = 0L;
			byte[] array = new byte[blockSize];
			byte[] array2 = new byte[(long)(1.1 * (double)blockSize)];
			BaseCompressor compressor = CompressorFactory.GetCompressor((CompressionMethod)directoryItem.ActualCompressionMethod, directoryItem.CompressionMode);
			if (compressor != null)
			{
				compressor.Init(CompressionDirection.Compress, directoryItem.CompressionMode);
				BaseZipForgeCryptoTransform baseZipForgeCryptoTransform = null;
				if (isEncrypted)
				{
					if (ZipForgeCryptoTransformFactory.IsAESEncryption(directoryItem.EncryptionAlgorithm))
					{
						directoryItem.CRC32 = 0U;
					}
					baseZipForgeCryptoTransform = ZipForgeCryptoTransformFactory.GetCryptoTransform(directoryItem.EncryptionAlgorithm, directoryItem);
					if (directoryItem.EncryptionAlgorithm == ComponentAce.Compression.Archiver.EncryptionAlgorithm.PkzipClassic)
					{
						throw ExceptionBuilder.Exception(ErrorCode.UnknownEncryptionMethod);
					}
					baseZipForgeCryptoTransform.Initialize(CryptoTransformMode.Encryption, directoryItem);
					baseZipForgeCryptoTransform.GenerateKey(directoryItem.Password);
					compressedBytesCount += (long)baseZipForgeCryptoTransform.GetFileStorageStartBlockSize();
					this.WriteToStream(baseZipForgeCryptoTransform.GetFileStorageStartBlock(), 0, baseZipForgeCryptoTransform.GetFileStorageStartBlockSize(), compressedStream);
					if (this._progressCancel)
					{
						return false;
					}
				}
				long num3;
				do
				{
					if (firstByte >= 0)
					{
						num3 = 1L;
						array[0] = (byte)firstByte;
						num3 += (long)fileStream.Read(array, 1, blockSize - 1);
						firstByte = -1;
					}
					else
					{
						num3 = (long)fileStream.Read(array, 0, blockSize);
					}
					if (num3 == 0L)
					{
						break;
					}
					while ((long)blockSize - num3 > 0L)
					{
						int num4 = fileStream.Read(array, (int)num3, (int)((long)blockSize - num3));
						if (num4 == 0)
						{
							break;
						}
						num3 += (long)num4;
					}
					num2 += num3;
					this.DoOnFileProgress(directoryItem.Name, num2, DateTime.Now - this._currentItemOperationStartTime, ProgressPhase.Process, ref this._progressCancel);
					if (this._progressCancel)
					{
						goto Block_12;
					}
					try
					{
						ZipUtil.UpdateCRC32(array, (uint)num3, ref fcrc32);
						if (!compressor.CompressBlock((uint)blockSize, num3, num3 != (long)blockSize, array, ref num, ref array2))
						{
							throw ExceptionBuilder.Exception(ErrorCode.InvalidFormat);
						}
						processedBytesCount += num3;
						if (isEncrypted)
						{
							baseZipForgeCryptoTransform.EncryptBuffer(array2, 0, (int)num, array2, 0);
						}
					}
					catch
					{
						compressor.Close();
						return false;
					}
					if (!this.WriteToStream(array2, 0, (int)num, compressedStream))
					{
						goto Block_14;
					}
					compressedBytesCount += num;
				}
				while (num3 == (long)blockSize);
				goto IL_21B;
				Block_12:
				num2 -= num3;
				goto IL_21B;
				Block_14:
				processedBytesCount -= num3;
				IL_21B:
				if (isEncrypted)
				{
					this.WriteToStream(baseZipForgeCryptoTransform.GetFileStorageEndBlock(), 0, baseZipForgeCryptoTransform.GetFileStorageEndBlockSize(), compressedStream);
					if (this._progressCancel)
					{
						return false;
					}
					compressedBytesCount += (long)baseZipForgeCryptoTransform.GetFileStorageEndBlockSize();
				}
				directoryItem.CompressedSize = compressedBytesCount;
				directoryItem.UncompressedSize = num2;
				directoryItem.CRC32 = ~fcrc32;
				return processedBytesCount == num2;
			}
			throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNullPointer, new object[]
			{
				"compressor"
			});
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002249C File Offset: 0x0002149C
		private void InitDirItemFromArchiveParams(int dirItemIndex)
		{
			DirItem dirItem = this._dmHandle.CentralDirectory[dirItemIndex] as DirItem;
			dirItem.Reset();
			dirItem.Password = this._password;
			dirItem.CompressionMode = this._compressionMode;
			dirItem.ActualCompressionMethod = this._compressionMethod;
			dirItem.Signature = 33639248U;
			dirItem.ExtractVersion = ((this._dmHandle.CentralDirectoryEnd.Signature == this.CentralDirEndSignature) ? 16660 : 20);
			dirItem.VersionMadeBy = 20;
			if (!CompressionUtils.IsNullOrEmpty(this._password))
			{
				dirItem.SetGeneralPurposeFlagBit(0);
			}
			dirItem.EncryptionAlgorithm = this._encryptionAlgorithm;
			dirItem.CRC32 = uint.MaxValue;
			if (ZipForgeCryptoTransformFactory.IsAESEncryption(this._encryptionAlgorithm))
			{
				dirItem.CRC32 = 0U;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00022560 File Offset: 0x00021560
		private void InternalCompressFile(Stream fileStream, Stream compressedStream, DirItem dirItem, int firstByte)
		{
			if (fileStream == null)
			{
				throw new ArgumentNullException("compressedStream");
			}
			if (compressedStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}
			if (dirItem == null)
			{
				throw new ArgumentNullException("dirItem");
			}
			long num = 0L;
			long num2 = 0L;
			uint maxValue = uint.MaxValue;
			int blockSize = (int)ZipUtil.InternalGetBlockSize(dirItem.CompressionMode);
			bool isEncrypted = !CompressionUtils.IsNullOrEmpty(dirItem.Password);
			if (!this.DoCompress(isEncrypted, dirItem, blockSize, fileStream, compressedStream, firstByte, ref num, ref num2, ref maxValue) && !this._progressCancel)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionFailed);
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000225E4 File Offset: 0x000215E4
		private bool SaveDirectoryItemToStream(int dirItemIndex)
		{
			bool result = false;
			DirItem dirItem = this._dmHandle.CentralDirectory[dirItemIndex] as DirItem;
			Stream stream = dirItem.Stream;
			if (stream == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNullPointer, new object[]
				{
					"fileStream"
				});
			}
			this._currentItemOperationStartTime = DateTime.Now;
			this.DoOnFileProgress(dirItem.Name, 0L, new TimeSpan(0L), ProgressPhase.Start, ref this._progressCancel);
			if (this._progressCancel)
			{
				this._dmHandle.CentralDirectory.DeleteItem(dirItemIndex);
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
				if (this._zip64Mode == Zip64Mode.Always && dirItem.ExtraFields.Zip64ExtraField == null)
				{
					dirItem.ExtraFields.AddExtraField(new Zip64ExtraFieldData(), dirItem);
				}
				else if (this._zip64Mode == Zip64Mode.Disabled)
				{
					dirItem.IsHugeFile = false;
				}
				if (this.UseUnicodeFileNameExtraField)
				{
					dirItem.ExtraFields.AddExtraField(new UnicodeExtraFieldData(name), dirItem);
				}
				dirItem.RelativeLocalHeaderOffset = this._sentBytes;
				dirItem.SetGeneralPurposeFlagBit(3);
				dirItem.ExtractVersion = (dirItem.IsHugeFile ? ((this._dmHandle.CentralDirectoryEnd.Signature == this.CentralDirEndSignature) ? 16685 : 45) : 20);
				uint crc = dirItem.CRC32;
				dirItem.CRC32 = 0U;
				int num = stream.ReadByte();
				if (num < 0)
				{
					dirItem.ActualCompressionMethod = 0;
				}
				MemoryStream memoryStream = new MemoryStream();
				dirItem.WriteLocalHeaderToStream(memoryStream, 0);
				this.WriteToStream(memoryStream.ToArray(), 0, (int)memoryStream.Length, this._compressedStream);
				if (this._progressCancel)
				{
					return false;
				}
				this._sentBytes += memoryStream.Length;
				byte[] array = dirItem.IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetBytes(name) : Encoding.GetEncoding(this._oemCodePage).GetBytes(name);
				this.WriteToStream(array, 0, array.Length, this._compressedStream);
				if (this._progressCancel)
				{
					return false;
				}
				this._sentBytes += (long)((ulong)dirItem.NameLength);
				MemoryStream memoryStream2 = new MemoryStream();
				dirItem.ExtraFields.WriteToStream(memoryStream2, 0L, ExtraFieldsTarget.LocalHeaderExtraFields);
				this.WriteToStream(memoryStream2.ToArray(), 0, (int)memoryStream2.Length, this._compressedStream);
				if (this._progressCancel)
				{
					return false;
				}
				this._sentBytes += memoryStream2.Length;
				dirItem.CRC32 = crc;
				this.InternalCompressFile(stream, this._compressedStream, dirItem, num);
				this._sentBytes += dirItem.CompressedSize;
				if (dirItem.CompressedSize > (long)((ulong)-1))
				{
					dirItem.IsHugeFile = true;
					if (this._zip64Mode == Zip64Mode.Disabled)
					{
						throw ExceptionBuilder.Exception(ErrorCode.HugeFileModeIsNotEnabled, new object[]
						{
							dirItem.Name
						});
					}
				}
				MemoryStream memoryStream3 = new MemoryStream();
				dirItem.DataDescriptor.WriteToStream(memoryStream3, 0L);
				this.WriteToStream(memoryStream3.ToArray(), 0, (int)memoryStream3.Length, this._compressedStream);
				this._sentBytes += memoryStream3.Length;
				if (this._progressCancel)
				{
					return false;
				}
				this.DoOnFileProgress(dirItem.Name, dirItem.UncompressedSize, DateTime.Now - this._currentItemOperationStartTime, ProgressPhase.End, ref this._progressCancel);
				result = true;
			}
			catch (Exception ex)
			{
				this.DoOnProcessFileFailure(dirItem.Name, ex.Message, ex);
			}
			return result;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000229C4 File Offset: 0x000219C4
		private bool WriteToStream(byte[] buffer, int offset, int count, Stream stream)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (buffer.Length < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (buffer.Length < offset + count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag = false;
			bool flag2;
			do
			{
				try
				{
					stream.Write(buffer, offset, count);
					flag2 = false;
				}
				catch (Exception innerException)
				{
					this.DoOnWriteToStreamFailed(innerException, ref flag);
					flag2 = true;
				}
			}
			while (flag2 && !flag);
			if (flag)
			{
				this._progressCancel = true;
			}
			return !flag2;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00022A74 File Offset: 0x00021A74
		public void ExtractFileToStream(Stream streamExtractTo)
		{
			this.CheckInactive();
			if (this._lastExtractedDirItem == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidFileHeaderObject);
			}
			this._currentItemOperationStartTime = DateTime.Now;
			this.DoOnFileProgress(this._lastExtractedDirItem.Name, 0L, DateTime.Now - this._currentItemOperationStartTime, ProgressPhase.Start, ref this._progressCancel);
			if (!this.InternalDecompressFile(this._compressedStream, streamExtractTo, this._lastExtractedDirItem) && !this._progressCancel)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidCheckSum);
			}
			if (!this._progressCancel)
			{
				this.DoOnFileProgress(this._lastExtractedDirItem.Name, this._currentItemBytesProcessed, DateTime.Now - this._currentItemOperationStartTime, ProgressPhase.End, ref this._progressCancel);
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00022B28 File Offset: 0x00021B28
		public ArchiveItem GetFileHeader()
		{
			this.CheckInactive();
			MemoryStream memoryStream = this.ReadCompressedStreamBlockToMemoryStream(DirItem.LocalHeaderSize(), true);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			uint num = binaryReader.ReadUInt32();
			if (num != 67324752U && num != 33639248U)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IncorrectSignatureFound, new object[]
				{
					num
				});
			}
			if (67324752U == num)
			{
				DirItem dirItem = new DirItem();
				byte[] source = memoryStream.ToArray();
				dirItem.LoadLocalHeaderFromByteArray(source, 0U);
				ushort num2 = dirItem.NameLengthToRead;
				if (num2 > 0)
				{
					byte[] array = new byte[(int)num2];
					MemoryStream memoryStream2 = this.ReadCompressedStreamBlockToMemoryStream((int)num2, true);
					memoryStream2.Read(array, 0, (int)num2);
					dirItem.Name = (dirItem.IsGeneralPurposeFlagBitSet(11) ? Encoding.UTF8.GetString(array) : Encoding.GetEncoding(this._oemCodePage).GetString(array));
				}
				else
				{
					dirItem.Name = string.Empty;
				}
				dirItem.Name.Replace('\\', '/');
				num2 = dirItem.ExtraFieldsLenRead;
				ArchiveItem result;
				if (dirItem.IsDirectory())
				{
					this.ReadCompressedStreamBlockToMemoryStream((int)num2, true);
					if (dirItem.IsGeneralPurposeFlagBitSet(3))
					{
						this.ReadDataDescrtiptor(dirItem);
					}
					result = this.GetFileHeader();
				}
				else
				{
					MemoryStream source2 = this.ReadCompressedStreamBlockToMemoryStream((int)num2, true);
					dirItem.LoadExtraFieldsFromStream(source2, num2);
					dirItem.EncryptionAlgorithm = ZipForgeCryptoTransformFactory.GetEncryptionAlgorithm(dirItem);
					this._lastExtractedDirItem = dirItem;
					result = new ArchiveItem(dirItem, this._dirItemIndex);
					this._dirItemIndex++;
				}
				return result;
			}
			return null;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00022C9C File Offset: 0x00021C9C
		internal bool ZipDecompress(bool encrypted, out long actualDecompressedSize, ref Stream compressedStream, Stream streamExtractTo, BaseZipForgeCryptoTransform decrypter, DirItem dirItem)
		{
			BaseZipForgeCryptoTransform baseZipForgeCryptoTransform = null;
			long num = 0L;
			this._currentProcessedItem = dirItem;
			this._decompressedStream = streamExtractTo;
			if (encrypted && !this.DecrypterInitializer(dirItem, out baseZipForgeCryptoTransform, decrypter, out num))
			{
				actualDecompressedSize = 0L;
				return false;
			}
			if (!dirItem.IsGeneralPurposeFlagBitSet(3) || dirItem.ActualCompressionMethod != 0)
			{
				BaseCompressor compressor = CompressorFactory.GetCompressor((CompressionMethod)dirItem.ActualCompressionMethod, dirItem.CompressionMode);
				compressor.Init(CompressionDirection.Decompress, dirItem.CompressionMode, true);
				compressor.OnDecompressedBufferReady += this.OnDecompressedBufferReady;
				try
				{
					this._currentItemBytesProcessed = 0L;
					this._crc32 = uint.MaxValue;
					actualDecompressedSize = 0L;
					bool flag = false;
					int length = 1048576;
					byte[] array = null;
					byte[] array2 = null;
					while (!flag)
					{
						if (!dirItem.IsGeneralPurposeFlagBitSet(3))
						{
							if (!encrypted)
							{
								length = (int)Math.Min(1048576L, dirItem.CompressedSize - num);
							}
							else
							{
								length = (int)Math.Min(1048576L, dirItem.CompressedSize - num - (long)decrypter.GetFileStorageEndBlockSize());
							}
						}
						int num2;
						if (array == null)
						{
							MemoryStream memoryStream = this.ReadCompressedStreamBlockToMemoryStream(length, false);
							array2 = new byte[memoryStream.Length];
							array = new byte[memoryStream.Length];
							memoryStream.Read(array, 0, array.Length);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							num2 = (int)memoryStream.Position;
							memoryStream.Read(array2, 0, array2.Length);
						}
						else
						{
							MemoryStream memoryStream2 = this.ReadCompressedStreamBlockToMemoryStream(length, false);
							byte[] array3 = new byte[memoryStream2.Length];
							array = new byte[memoryStream2.Length];
							memoryStream2.Read(array, 0, array.Length);
							memoryStream2.Seek((long)array2.Length, SeekOrigin.Begin);
							num2 = (int)memoryStream2.Position;
							Buffer.BlockCopy(array2, 0, array3, 0, array2.Length);
							memoryStream2.Read(array3, array2.Length, array3.Length - array2.Length);
							array2 = new byte[memoryStream2.Length];
							Buffer.BlockCopy(array3, 0, array2, 0, array2.Length);
						}
						if (encrypted)
						{
							baseZipForgeCryptoTransform.DecryptBuffer(array2, num2, array2.Length - num2, array2, num2);
						}
						num += (long)array2.Length;
						long num3;
						try
						{
							if (!compressor.DecompressBlock(array2.Length, ref array2, out num3, out flag))
							{
								return false;
							}
						}
						catch
						{
							return false;
						}
						num -= (long)array2.Length;
						actualDecompressedSize += num3;
						if (array2.Length != 0)
						{
							this._buffer = new byte[array2.Length];
							Buffer.BlockCopy(array, array.Length - array2.Length, this._buffer, 0, array2.Length);
						}
						if (encrypted)
						{
							decrypter.DecryptBuffer(array, 0, array.Length - array2.Length, array, 0);
						}
						if (encrypted && dirItem.CompressedSize == num + (long)decrypter.GetFileStorageEndBlockSize())
						{
							flag = true;
						}
						else if (dirItem.CompressedSize == num)
						{
							flag = true;
						}
					}
					goto IL_2BE;
				}
				finally
				{
					compressor.Close();
				}
			}
			actualDecompressedSize = 0L;
			this._crc32 = uint.MaxValue;
			IL_2BE:
			bool result;
			if (encrypted)
			{
				MemoryStream memoryStream3 = this.ReadCompressedStreamBlockToMemoryStream(decrypter.GetFileStorageEndBlockSize(), true);
				decrypter.LoadFileStorageEndBlock(memoryStream3, 0L);
				memoryStream3.Seek(0L, SeekOrigin.Begin);
				baseZipForgeCryptoTransform.LoadFileStorageEndBlock(memoryStream3, 0L);
				if (dirItem.IsGeneralPurposeFlagBitSet(3))
				{
					this.ReadDataDescrtiptor(dirItem);
				}
				result = !dirItem.IsCorrupted(~this._crc32, decrypter);
			}
			else
			{
				if (dirItem.IsGeneralPurposeFlagBitSet(3))
				{
					this.ReadDataDescrtiptor(dirItem);
				}
				result = (dirItem.CRC32 == ~this._crc32);
			}
			return result;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00023020 File Offset: 0x00022020
		private bool DecrypterInitializer(DirItem dirItem, out BaseZipForgeCryptoTransform tmpDecrypter, BaseZipForgeCryptoTransform decrypter, out long totalProcessedFilesSize)
		{
			tmpDecrypter = ZipForgeCryptoTransformFactory.GetCryptoTransform(dirItem.EncryptionAlgorithm, dirItem);
			tmpDecrypter.Initialize(CryptoTransformMode.Decryption, dirItem);
			tmpDecrypter.GenerateKey(this._password);
			MemoryStream memoryStream = this.ReadCompressedStreamBlockToMemoryStream(decrypter.GetFileStorageStartBlockSize(), true);
			totalProcessedFilesSize = memoryStream.Length;
			bool flag = false;
			bool flag2;
			do
			{
				decrypter.LoadFileStorageStartBlock(memoryStream, 0L);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				tmpDecrypter.LoadFileStorageStartBlock(memoryStream, 0L);
				flag2 = !decrypter.CheckPassword(this._password, dirItem);
				tmpDecrypter.CheckPassword(this._password, dirItem);
				if (flag2)
				{
					this.DoOnPassword(dirItem.Name, ref this._password, ref flag);
					if (decrypter.Password != this._password && !flag)
					{
						decrypter.GenerateKey(this._password);
						tmpDecrypter.GenerateKey(this._password);
					}
				}
			}
			while (flag2 && !flag);
			return !flag;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000230FA File Offset: 0x000220FA
		protected internal void DoOnPassword(string fileName, ref string newPassword, ref bool abortProcess)
		{
			if (this.OnPassword != null)
			{
				this.OnPassword(this, fileName, ref newPassword, ref abortProcess);
				return;
			}
			throw ExceptionBuilder.Exception(ErrorCode.IncorrectPassword);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002311C File Offset: 0x0002211C
		private bool InternalDecompressFile(Stream compressedStream, Stream streamExtractTo, DirItem dirItem)
		{
			BaseZipForgeCryptoTransform baseZipForgeCryptoTransform = null;
			bool flag = dirItem.IsGeneralPurposeFlagBitSet(0);
			if (flag)
			{
				baseZipForgeCryptoTransform = ZipForgeCryptoTransformFactory.GetCryptoTransform(dirItem.EncryptionAlgorithm, dirItem);
				baseZipForgeCryptoTransform.Initialize(CryptoTransformMode.Decryption, dirItem);
				baseZipForgeCryptoTransform.GenerateKey(this._password);
			}
			this._currentItemOperationStartTime = DateTime.Now;
			if (dirItem.ActualCompressionMethod < 255)
			{
				long num;
				bool flag2 = this.ZipDecompress(flag, out num, ref compressedStream, streamExtractTo, baseZipForgeCryptoTransform, dirItem);
				return flag2 && num == dirItem.UncompressedSize;
			}
			throw ExceptionBuilder.Exception(ErrorCode.UnknownCompressionMethod);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00023198 File Offset: 0x00022198
		private void OnDecompressedBufferReady(byte[] buffer, int outBytes, out bool stopDecompression)
		{
			TimeSpan timeElapsed = new TimeSpan(DateTime.Now.Ticks - this._currentItemOperationStartTime.Ticks);
			stopDecompression = false;
			this.DoOnFileProgress(this._currentProcessedItem.Name, this._currentItemBytesProcessed, timeElapsed, ProgressPhase.Process, ref stopDecompression);
			if (!stopDecompression)
			{
				stopDecompression = !this.WriteToStream(buffer, 0, outBytes, this._decompressedStream);
				ZipUtil.UpdateCRC32(buffer, (uint)outBytes, ref this._crc32);
			}
			this._currentItemBytesProcessed += (long)outBytes;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00023218 File Offset: 0x00022218
		private MemoryStream ReadCompressedStreamBlockToMemoryStream(int length, bool needToReadExactlyLengthBytes)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (this._compressedStream == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNullPointer, new object[]
				{
					"compressedStream"
				});
			}
			if (!this._compressedStream.CanRead)
			{
				throw ExceptionBuilder.Exception(ErrorCode.StreamDoesNotSupportReading, new object[]
				{
					"compressedStream"
				});
			}
			MemoryStream memoryStream = new MemoryStream();
			if (this._buffer != null)
			{
				if (this._buffer.Length < length)
				{
					memoryStream.Write(this._buffer, 0, this._buffer.Length);
					int length2 = length - this._buffer.Length;
					this._buffer = null;
					MemoryStream memoryStream2 = this.ReadCompressedStreamBlockToMemoryStream(length2, needToReadExactlyLengthBytes);
					memoryStream2.Seek(0L, SeekOrigin.Begin);
					byte[] array = new byte[memoryStream2.Length];
					memoryStream2.Read(array, 0, array.Length);
					memoryStream.Write(array, 0, array.Length);
				}
				else
				{
					memoryStream.Write(this._buffer, 0, length);
					byte[] array2 = new byte[this._buffer.Length - length];
					if (array2.Length > 0)
					{
						Buffer.BlockCopy(this._buffer, length, array2, 0, array2.Length);
						this._buffer = array2;
					}
					else
					{
						this._buffer = null;
					}
				}
			}
			else
			{
				byte[] buffer = new byte[length];
				int num = this._compressedStream.Read(buffer, 0, length);
				if (num > 0)
				{
					memoryStream.Write(buffer, 0, num);
					while (length - num > 0)
					{
						int num2 = this._compressedStream.Read(buffer, 0, length - num);
						if (num2 == 0)
						{
							break;
						}
						memoryStream.Write(buffer, 0, num2);
						num += num2;
					}
				}
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			if (needToReadExactlyLengthBytes && memoryStream.Length != (long)length)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidReadedBytesCount, new object[]
				{
					length,
					memoryStream.Length
				});
			}
			return memoryStream;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000233EC File Offset: 0x000223EC
		private void ReadDataDescrtiptor(DirItem dirItem)
		{
			if (dirItem == null)
			{
				throw new ArgumentNullException("dirItem");
			}
			if (dirItem.DataDescriptor == null)
			{
				throw ExceptionBuilder.Exception(ErrorCode.UnexpectedNullPointer, new object[]
				{
					"DataDescriptor"
				});
			}
			MemoryStream memoryStream = this.ReadCompressedStreamBlockToMemoryStream(dirItem.DataDescriptor.GetSize(), true);
			dirItem.LoadDataDescriptor(memoryStream, 0L);
			if (memoryStream.Position != memoryStream.Length)
			{
				if (this._buffer == null)
				{
					this._buffer = new byte[0];
				}
				byte[] array = new byte[this._buffer.Length];
				Buffer.BlockCopy(this._buffer, 0, array, 0, this._buffer.Length);
				this._buffer = new byte[(long)array.Length + memoryStream.Length - memoryStream.Position];
				byte[] array2 = new byte[memoryStream.Length - memoryStream.Position];
				memoryStream.Read(array2, 0, array2.Length);
				Buffer.BlockCopy(array2, 0, this._buffer, 0, array2.Length);
				Buffer.BlockCopy(array, 0, this._buffer, array2.Length, array.Length);
				return;
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000234F2 File Offset: 0x000224F2
		protected internal void CheckInactive()
		{
			if (!this._isOpened)
			{
				throw ExceptionBuilder.Exception(ErrorCode.ArchiveIsNotOpen);
			}
		}

		// Token: 0x040002E7 RID: 743
		private Stream _compressedStream;

		// Token: 0x040002E8 RID: 744
		private readonly int _oemCodePage;

		// Token: 0x040002E9 RID: 745
		private byte[] _buffer;

		// Token: 0x040002EA RID: 746
		private uint _crc32;

		// Token: 0x040002EB RID: 747
		private long _currentItemBytesProcessed;

		// Token: 0x040002EC RID: 748
		private DateTime _currentItemOperationStartTime;

		// Token: 0x040002ED RID: 749
		private DirItem _currentProcessedItem;

		// Token: 0x040002EE RID: 750
		private Stream _decompressedStream;

		// Token: 0x040002EF RID: 751
		private int _dirItemIndex;

		// Token: 0x040002F0 RID: 752
		private DirItem _lastExtractedDirItem;

		// Token: 0x040002F1 RID: 753
		private bool _progressCancel;

		// Token: 0x040002F2 RID: 754
		private string _commentsToArchive;

		// Token: 0x040002F3 RID: 755
		private readonly byte _compressionMode;

		// Token: 0x040002F4 RID: 756
		private RealTimeDirManager _dmHandle;

		// Token: 0x040002F5 RID: 757
		private bool _isOpened;

		// Token: 0x040002F6 RID: 758
		private long _sentBytes;

		// Token: 0x040002F7 RID: 759
		private Zip64Mode _zip64Mode;

		// Token: 0x040002F8 RID: 760
		private CompressionLevel _compressionLevel;

		// Token: 0x040002F9 RID: 761
		private bool _createArchive;

		// Token: 0x040002FA RID: 762
		private EncryptionAlgorithm _encryptionAlgorithm;

		// Token: 0x040002FB RID: 763
		private ushort _compressionMethod;

		// Token: 0x040002FC RID: 764
		private string _password;

		// Token: 0x040002FD RID: 765
		private bool _useUnicodeFileNameExtraField;

		// Token: 0x02000075 RID: 117
		// (Invoke) Token: 0x06000508 RID: 1288
		public delegate void OnFileProgressDelegate(object sender, string fileName, long bytesProcessed, TimeSpan timeElapsed, ProgressPhase progressPhase, ref bool cancel);

		// Token: 0x02000076 RID: 118
		// (Invoke) Token: 0x0600050C RID: 1292
		public delegate void OnFileRenameDelegate(object sender, string fileName, ref string newFileName, ref bool cancel);

		// Token: 0x02000077 RID: 119
		// (Invoke) Token: 0x06000510 RID: 1296
		public delegate void OnProcessFileFailureDelegate(object sender, string fileName, string errorMessage, Exception exception);

		// Token: 0x02000078 RID: 120
		// (Invoke) Token: 0x06000514 RID: 1300
		public delegate void OnWriteToStreamFailureDelegate(object sender, Exception innerException, ref bool cancel);

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x06000518 RID: 1304
		public delegate void OnPasswordDelegate(object sender, string fileName, ref string newPassword, ref bool abortProcess);
	}
}
