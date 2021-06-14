using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.ZipForge.Encryption;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000086 RID: 134
	internal class DirItem : IItem, ICloneable
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x00026E29 File Offset: 0x00025E29
		public static int CentralDirSize()
		{
			return 46;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00026E2D File Offset: 0x00025E2D
		public static int LocalHeaderSize()
		{
			return 30;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00026E34 File Offset: 0x00025E34
		public DirItem()
		{
			this._itemName = string.Empty;
			this._oldName = string.Empty;
			this.Comment = string.Empty;
			this.Password = string.Empty;
			this.SrcFileName = string.Empty;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00026E89 File Offset: 0x00025E89
		public DirItem(string name) : this()
		{
			this._itemName = name;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00026E98 File Offset: 0x00025E98
		public string OldName
		{
			get
			{
				return this._oldName;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00026EA0 File Offset: 0x00025EA0
		public void Reset()
		{
			this._centralDirStreamPos = 0L;
			this._comment = string.Empty;
			this._commentLengthToRead = 0;
			this._compMethod = 0;
			this._compressionMode = 0;
			this._compSize = 0U;
			this._crc32 = 0U;
			this._dataDescriptor = null;
			this._diskNumberStart = 0;
			this._encryptionAlgorithm = EncryptionAlgorithm.None;
			this._externalAttr = FileAttributes.Normal;
			this._extractVersion = 0;
			this._extraFields.Reset();
			this._extraFieldsLenToRead = 0;
			this._genPurposeFlag = 0;
			this._internalAttr = 0;
			this._isHugeFile = false;
			this._isModified = false;
			this._isTagged = false;
			this.Name = string.Empty;
			this._lastModDate = 0;
			this._lastModTime = 0;
			this._nameLengthToRead = 0;
			this._needDestroyStream = false;
			this._password = string.Empty;
			this._oldName = string.Empty;
			this._relOffsetLh = 0U;
			this._srcFileName = string.Empty;
			this._stream = null;
			this._streamPosition = 0;
			this._uncompSize = 0U;
			this._versionMadeBy = 0;
			this._localHeaderExtraFieldsNeeded = false;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00026FB4 File Offset: 0x00025FB4
		public object Clone()
		{
			DirItem dirItem = new DirItem();
			dirItem._signature = this._signature;
			dirItem._versionMadeBy = this._versionMadeBy;
			dirItem._extractVersion = this._extractVersion;
			dirItem._genPurposeFlag = this._genPurposeFlag;
			dirItem._compMethod = this._compMethod;
			dirItem._lastModTime = this._lastModTime;
			dirItem._lastModDate = this._lastModDate;
			dirItem._crc32 = this._crc32;
			dirItem._compSize = this._compSize;
			dirItem._uncompSize = this._uncompSize;
			dirItem._diskNumberStart = this._diskNumberStart;
			dirItem._internalAttr = this._internalAttr;
			dirItem._externalAttr = this._externalAttr;
			dirItem._relOffsetLh = this._relOffsetLh;
			dirItem._itemName = this._itemName;
			dirItem._oldName = this._oldName;
			dirItem.Comment = this.Comment;
			dirItem._extraFields = (ExtraFieldsDataBlock)this._extraFields.Clone();
			dirItem._isHugeFile = this._isHugeFile;
			dirItem._isModified = this._isModified;
			dirItem._password = this._password;
			dirItem._encryptionAlgorithm = this._encryptionAlgorithm;
			dirItem._stream = this._stream;
			dirItem._needDestroyStream = this._needDestroyStream;
			dirItem._streamPosition = this._streamPosition;
			dirItem._centralDirStreamPos = this._centralDirStreamPos;
			dirItem._isTagged = this._isTagged;
			dirItem.SrcFileName = this.SrcFileName;
			dirItem.CompressionMode = this.CompressionMode;
			dirItem.Operation = this.Operation;
			if (this._dataDescriptor != null)
			{
				dirItem._dataDescriptor = (DirItemDataDescriptor)this._dataDescriptor.Clone();
			}
			dirItem._localHeaderExtraFieldsNeeded = this._localHeaderExtraFieldsNeeded;
			return dirItem;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00027164 File Offset: 0x00026164
		public bool IsCorrupted(uint crc32, BaseZipForgeCryptoTransform crypter)
		{
			if (crypter == null)
			{
				return this.CRC32 != crc32;
			}
			return crypter.IsDirItemCorrupted(this, crc32);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002717E File Offset: 0x0002617E
		public void CopyFrom(BaseArchiveItem archiveItem)
		{
			this.CopyFrom(archiveItem, true);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00027188 File Offset: 0x00026188
		public bool IsDirectory()
		{
			return (this.ExternalAttributes & FileAttributes.Directory) != (FileAttributes)0 || (this.UncompressedSize == 0L && this.Name.EndsWith("/"));
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000271B4 File Offset: 0x000261B4
		public void GetArchiveItem(ref BaseArchiveItem baseArchiveItem)
		{
			ArchiveItem archiveItem = baseArchiveItem as ArchiveItem;
			this.FireLocalHeaderExtraFieldsNeededEvent();
			string text = this.Name;
			if (text[text.Length - 1] == '/')
			{
				text = text.Substring(0, text.Length - 1);
				archiveItem.FileName = "";
				archiveItem.storedPath = text;
			}
			else
			{
				archiveItem.FileName = Path.GetFileName(text);
				archiveItem.storedPath = Path.GetDirectoryName(text);
			}
			archiveItem.SrcFileName = this._srcFileName;
			archiveItem._compressedSize = this.CompressedSize;
			archiveItem._uncompressedSize = this.UncompressedSize;
			if (archiveItem.UncompressedSize > 0L)
			{
				archiveItem._compressionRate = (double)(1f - (float)archiveItem.CompressedSize / (float)archiveItem.UncompressedSize) * 100.0;
			}
			else
			{
				archiveItem._compressionRate = 100.0;
			}
			if (archiveItem._compressionRate < 0.0)
			{
				archiveItem._compressionRate = 0.0;
			}
			archiveItem._encrypted = this.IsGeneralPurposeFlagBitSet(0);
			archiveItem.LastModFileDate = this.LastModificationDate;
			archiveItem.LastModFileTime = this.LastModificationTime;
			uint dosDateTime = (uint)((int)this.LastModificationDate << 16 | (int)this.LastModificationTime);
			archiveItem.FileModificationDateTime = CompressionUtils.DosDateTimeToDateTime(dosDateTime);
			NTFSExtraFieldData ntfsextraFieldData = this.ExtraFields.GetExtraFieldById(10) as NTFSExtraFieldData;
			if (ntfsextraFieldData != null)
			{
				archiveItem.FileCreationDateTime = DateTime.FromFileTime(ntfsextraFieldData.FileCreationTime);
				archiveItem.FileLastAccessDateTime = DateTime.FromFileTime(ntfsextraFieldData.FileLastAccessTime);
				archiveItem.FileModificationDateTime = DateTime.FromFileTime(ntfsextraFieldData.FileModificationTime);
			}
			else
			{
				archiveItem.FileLastAccessDateTime = archiveItem.FileModificationDateTime;
				archiveItem.FileCreationDateTime = archiveItem.FileModificationDateTime;
			}
			archiveItem._crc = this.CRC32;
			archiveItem.ExternalFileAttributes = this.ExternalAttributes;
			archiveItem.Comment = this.Comment;
			archiveItem.EncryptionAlgorithm = this.EncryptionAlgorithm;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00027380 File Offset: 0x00026380
		public void CopyFrom(BaseArchiveItem baseArchiveItem, bool storeNTFSdates)
		{
			ArchiveItem archiveItem = baseArchiveItem as ArchiveItem;
			this.Name = archiveItem.FullName;
			this.SrcFileName = archiveItem.SrcFileName;
			this.LastModificationDate = archiveItem.LastModFileDate;
			this.LastModificationTime = archiveItem.LastModFileTime;
			if (storeNTFSdates)
			{
				NTFSExtraFieldData ntfsextraFieldData = this.ExtraFields.GetExtraFieldById(10) as NTFSExtraFieldData;
				if (ntfsextraFieldData != null)
				{
					ntfsextraFieldData.FileCreationTime = archiveItem.FileCreationDateTime.ToFileTime();
					ntfsextraFieldData.FileLastAccessTime = archiveItem.FileLastAccessDateTime.ToFileTime();
					ntfsextraFieldData.FileModificationTime = archiveItem.FileModificationDateTime.ToFileTime();
				}
				else
				{
					this.ExtraFields.AddExtraField(new NTFSExtraFieldData(archiveItem.FileModificationDateTime.ToFileTime(), archiveItem.FileLastAccessDateTime.ToFileTime(), archiveItem.FileCreationDateTime.ToFileTime()), this);
				}
			}
			this.ExternalAttributes = archiveItem.ExternalFileAttributes;
			this.Comment = archiveItem.Comment;
			this.IsModified = true;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002747C File Offset: 0x0002647C
		public ZipFileHeader GetLocalHeader()
		{
			this.FireLocalHeaderExtraFieldsNeededEvent();
			return new ZipFileHeader
			{
				signature = 67324752U,
				extractVersion = this.ExtractVersion,
				genPurposeFlag = this.GeneralPurposeFlag,
				compMethod = this.CompressionMethod,
				lastModTime = this.LastModificationTime,
				lastModDate = this.LastModificationDate,
				crc32 = this.CRC32,
				compSize = this._compSize,
				uncompSize = this._uncompSize,
				nameLength = this.NameLength,
				extraLength = this.ExtraFields.GetBytesLength(ExtraFieldsTarget.LocalHeaderExtraFields)
			};
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002752C File Offset: 0x0002652C
		public int GetLocalHeaderSize()
		{
			return ZipFileHeader.SizeOf();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00027534 File Offset: 0x00026534
		public void WriteLocalHeaderToStream(Stream stream, int offset)
		{
			stream.Write(this.GetLocalHeader().GetBytes(), offset, ZipFileHeader.SizeOf());
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002755C File Offset: 0x0002655C
		public bool CheckLocalHeaderItem(ZipFileHeader header, string name)
		{
			this.FireLocalHeaderExtraFieldsNeededEvent();
			return name.Length == (int)header.nameLength && header.signature == 67324752U && header.crc32 == this.CRC32 && header.extractVersion == this.ExtractVersion && header.genPurposeFlag == this.GeneralPurposeFlag && header.compMethod == this.CompressionMethod && header.lastModTime == this.LastModificationTime && header.lastModDate == this.LastModificationDate && header.compSize == this._compSize && header.uncompSize == this._uncompSize;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002760A File Offset: 0x0002660A
		public void LoadDataDescriptor(Stream stream, long offset)
		{
			this._dataDescriptor = DirItemDataDescriptor.LoadFromStream(stream, offset, this.ExtraFields.Zip64ExtraField != null);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002762A File Offset: 0x0002662A
		public void SaveDataDescriptor(Stream stream, long offset)
		{
			if (this._dataDescriptor != null)
			{
				this._dataDescriptor.WriteToStream(stream, offset);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00027644 File Offset: 0x00026644
		public ExtraFieldsDataBlock ExtraFields
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				if (this._extraFields.Zip64ExtraField != null && this._dataDescriptor != null)
				{
					this._dataDescriptor.IsZip64 = true;
					this._dataDescriptor.CompressedSize = this.CompressedSize;
					this._dataDescriptor.UncompressedSize = this.UncompressedSize;
				}
				return this._extraFields;
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000276A0 File Offset: 0x000266A0
		public void LoadExtraFieldsFromStream(Stream source, ushort extraLength)
		{
			this._extraFields = ExtraFieldsDataBlock.LoadFromStream(source, extraLength, this);
			if (this.ExtraFields.Zip64ExtraField != null && this._dataDescriptor != null)
			{
				this._dataDescriptor.IsZip64 = true;
				this._dataDescriptor.CompressedSize = this.CompressedSize;
				this._dataDescriptor.UncompressedSize = this.UncompressedSize;
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000276FE File Offset: 0x000266FE
		internal void FireLocalHeaderExtraFieldsNeededEvent()
		{
			if (this._localHeaderExtraFieldsNeeded && this.LocalHeaderExtraFieldsNeeded != null)
			{
				this.LocalHeaderExtraFieldsNeeded(this, null);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002771D File Offset: 0x0002671D
		public void LoadLocalHeaderExtraFieldsFromStream(Stream source, ushort extraLength)
		{
			this._extraFields.LoadLocalHeaderExtraFieldsFromStream(source, extraLength, this);
			this._localHeaderExtraFieldsNeeded = false;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00027734 File Offset: 0x00026734
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x00027770 File Offset: 0x00026770
		public string Name
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				UnicodeExtraFieldData unicodeExtraFieldData = (UnicodeExtraFieldData)this.ExtraFields.GetExtraFieldById(21838);
				if (unicodeExtraFieldData != null)
				{
					return unicodeExtraFieldData.FileName;
				}
				return this._itemName;
			}
			set
			{
				if (this._itemName != value)
				{
					UnicodeExtraFieldData unicodeExtraFieldData = (UnicodeExtraFieldData)this.ExtraFields.GetExtraFieldById(21838);
					if (unicodeExtraFieldData != null)
					{
						unicodeExtraFieldData.FileName = value;
					}
					this._oldName = this._itemName;
					this._itemName = value;
					if (this.ItemNameChanged != null)
					{
						this.ItemNameChanged(this, null);
					}
				}
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000277D3 File Offset: 0x000267D3
		public void InformOnLocalHeaderExtraFieldCorruption(int realLocalHeaderExtraFieldSize)
		{
			this._realLocalHeaderExtraFieldSize = realLocalHeaderExtraFieldSize;
			this._localHeaderExtraFieldIsCorrupted = true;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x000277E3 File Offset: 0x000267E3
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x000277F1 File Offset: 0x000267F1
		public long RelativeCentralDirectoryOffset
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				return this._centralDirStreamPos;
			}
			set
			{
				this._centralDirStreamPos = value;
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000277FA File Offset: 0x000267FA
		public int GetDataOffset()
		{
			this.FireLocalHeaderExtraFieldsNeededEvent();
			if (!this._localHeaderExtraFieldIsCorrupted)
			{
				return ZipFileHeader.SizeOf() + (int)this.NameLength + (int)this.ExtraFields.GetBytesLength(ExtraFieldsTarget.LocalHeaderExtraFields);
			}
			return ZipFileHeader.SizeOf() + (int)this.NameLength + this._realLocalHeaderExtraFieldSize;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00027837 File Offset: 0x00026837
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x0002783F File Offset: 0x0002683F
		public bool NeedDestroyStream
		{
			get
			{
				return this._needDestroyStream;
			}
			set
			{
				this._needDestroyStream = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00027848 File Offset: 0x00026848
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x00027850 File Offset: 0x00026850
		public int StreamPosition
		{
			get
			{
				return this._streamPosition;
			}
			set
			{
				this._streamPosition = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00027859 File Offset: 0x00026859
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x00027861 File Offset: 0x00026861
		public string SrcFileName
		{
			get
			{
				return this._srcFileName;
			}
			set
			{
				this._srcFileName = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0002786A File Offset: 0x0002686A
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x00027872 File Offset: 0x00026872
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0002787B File Offset: 0x0002687B
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00027883 File Offset: 0x00026883
		public byte CompressionMode
		{
			get
			{
				return this._compressionMode;
			}
			set
			{
				this._compressionMode = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0002788C File Offset: 0x0002688C
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x00027894 File Offset: 0x00026894
		public bool IsTagged
		{
			get
			{
				return this._isTagged;
			}
			set
			{
				this._isTagged = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0002789D File Offset: 0x0002689D
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x000278A5 File Offset: 0x000268A5
		public ProcessOperation Operation
		{
			get
			{
				return this._operation;
			}
			set
			{
				this._operation = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000278AE File Offset: 0x000268AE
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x000278B6 File Offset: 0x000268B6
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
			set
			{
				this._stream = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x000278BF File Offset: 0x000268BF
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x000278C7 File Offset: 0x000268C7
		public bool IsModified
		{
			get
			{
				return this._isModified;
			}
			set
			{
				this._isModified = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x000278D0 File Offset: 0x000268D0
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x000278D8 File Offset: 0x000268D8
		public bool IsHugeFile
		{
			get
			{
				return this._isHugeFile;
			}
			set
			{
				this._isHugeFile = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x000278E1 File Offset: 0x000268E1
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x000278E9 File Offset: 0x000268E9
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				this._comment = value;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x000278F2 File Offset: 0x000268F2
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00027900 File Offset: 0x00026900
		internal EncryptionAlgorithm EncryptionAlgorithm
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				return this._encryptionAlgorithm;
			}
			set
			{
				if (ZipForgeCryptoTransformFactory.IsAESEncryption(this._encryptionAlgorithm) && !ZipForgeCryptoTransformFactory.IsAESEncryption(value) && this._extraFields.GetExtraFieldById(39169) != null)
				{
					AESExtraFieldData aesextraFieldData = (AESExtraFieldData)this.ExtraFields.GetExtraFieldById(39169);
					this._compMethod = aesextraFieldData.CompressionMethod;
					this._extraFields.RemoveExtraField(aesextraFieldData);
				}
				if (!ZipForgeCryptoTransformFactory.IsAESEncryption(this._encryptionAlgorithm) && this._extraFields.GetExtraFieldById(39169) == null && ZipForgeCryptoTransformFactory.IsAESEncryption(value))
				{
					byte strength = 0;
					switch (value)
					{
					case EncryptionAlgorithm.Aes128:
						strength = 1;
						break;
					case EncryptionAlgorithm.Aes192:
						strength = 2;
						break;
					case EncryptionAlgorithm.Aes256:
						strength = 3;
						break;
					}
					this.ExtraFields.AddExtraField(new AESExtraFieldData(2, strength, this.CompressionMethod), this);
					this.CompressionMethod = 99;
				}
				this._encryptionAlgorithm = value;
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000279DC File Offset: 0x000269DC
		public void LoadCentralDirFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this._signature = binaryReader.ReadUInt32();
			this._versionMadeBy = binaryReader.ReadUInt16();
			this._extractVersion = binaryReader.ReadUInt16();
			this._genPurposeFlag = binaryReader.ReadUInt16();
			this._compMethod = binaryReader.ReadUInt16();
			this._lastModTime = binaryReader.ReadUInt16();
			this._lastModDate = binaryReader.ReadUInt16();
			this._crc32 = binaryReader.ReadUInt32();
			this._compSize = binaryReader.ReadUInt32();
			this._uncompSize = binaryReader.ReadUInt32();
			this._nameLengthToRead = binaryReader.ReadUInt16();
			this._extraFieldsLenToRead = binaryReader.ReadUInt16();
			this._commentLengthToRead = binaryReader.ReadUInt16();
			this._diskNumberStart = binaryReader.ReadUInt16();
			this._internalAttr = binaryReader.ReadUInt16();
			this._externalAttr = (FileAttributes)binaryReader.ReadUInt32();
			this._relOffsetLh = binaryReader.ReadUInt32();
			if (this.IsGeneralPurposeFlagBitSet(3))
			{
				this._dataDescriptor = new DirItemDataDescriptor(this.CRC32, this.CompressedSize, this.UncompressedSize, false, true);
			}
			this._localHeaderExtraFieldsNeeded = true;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00027AFC File Offset: 0x00026AFC
		public void LoadLocalHeaderFromByteArray(byte[] source, uint offset)
		{
			ZipFileHeader zipFileHeader = default(ZipFileHeader);
			zipFileHeader.LoadFromByteArray(source, offset);
			this.Signature = zipFileHeader.signature;
			this.VersionMadeBy = zipFileHeader.extractVersion;
			this.ExtractVersion = zipFileHeader.extractVersion;
			this.GeneralPurposeFlag = zipFileHeader.genPurposeFlag;
			this.CompressionMethod = zipFileHeader.compMethod;
			this.LastModificationTime = zipFileHeader.lastModTime;
			this.LastModificationDate = zipFileHeader.lastModDate;
			this.CRC32 = zipFileHeader.crc32;
			this.CompressedSize = (long)((ulong)zipFileHeader.compSize);
			this.UncompressedSize = (long)((ulong)zipFileHeader.uncompSize);
			this._nameLengthToRead = zipFileHeader.nameLength;
			this._extraFieldsLenToRead = zipFileHeader.extraLength;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00027BB8 File Offset: 0x00026BB8
		public void LoadLocalHeaderFromStream(Stream stream, int offset)
		{
			byte[] array = new byte[ZipFileHeader.SizeOf()];
			stream.Read(array, offset, ZipFileHeader.SizeOf());
			this.LoadLocalHeaderFromByteArray(array, 0U);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00027BE8 File Offset: 0x00026BE8
		public byte[] GetCentralDirBytes()
		{
			this.FireLocalHeaderExtraFieldsNeededEvent();
			byte[] array = new byte[DirItem.CentralDirSize()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this._signature);
			binaryWriter.Write(this._versionMadeBy);
			binaryWriter.Write(this._extractVersion);
			binaryWriter.Write(this._genPurposeFlag);
			binaryWriter.Write(this._compMethod);
			binaryWriter.Write(this._lastModTime);
			binaryWriter.Write(this._lastModDate);
			binaryWriter.Write(this._crc32);
			binaryWriter.Write(this._compSize);
			binaryWriter.Write(this._uncompSize);
			binaryWriter.Write(this.NameLength);
			binaryWriter.Write(this.ExtraFields.GetBytesLength(ExtraFieldsTarget.CentralDirectoryExtraFields));
			binaryWriter.Write(this.CommentLength);
			binaryWriter.Write(this._diskNumberStart);
			binaryWriter.Write(this._internalAttr);
			binaryWriter.Write((uint)this._externalAttr);
			binaryWriter.Write(this._relOffsetLh);
			return array;
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00027CE5 File Offset: 0x00026CE5
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00027CED File Offset: 0x00026CED
		public uint Signature
		{
			get
			{
				return this._signature;
			}
			set
			{
				this._signature = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00027CF6 File Offset: 0x00026CF6
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00027CFE File Offset: 0x00026CFE
		public ushort VersionMadeBy
		{
			get
			{
				return this._versionMadeBy;
			}
			set
			{
				this._versionMadeBy = value;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00027D07 File Offset: 0x00026D07
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00027D0F File Offset: 0x00026D0F
		public ushort ExtractVersion
		{
			get
			{
				return this._extractVersion;
			}
			set
			{
				this._extractVersion = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00027D18 File Offset: 0x00026D18
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00027D20 File Offset: 0x00026D20
		public ushort GeneralPurposeFlag
		{
			get
			{
				return this._genPurposeFlag;
			}
			set
			{
				this._genPurposeFlag = value;
				if (this.IsGeneralPurposeFlagBitSet(3))
				{
					this._dataDescriptor = ((this.ExtraFields.Zip64ExtraField != null) ? new DirItemDataDescriptor(this.CRC32, this.CompressedSize, this.UncompressedSize) : new DirItemDataDescriptor(this.CRC32, (int)this.CompressedSize, (int)this.UncompressedSize));
				}
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00027D82 File Offset: 0x00026D82
		public bool IsGeneralPurposeFlagBitSet(int bitIndex)
		{
			if (bitIndex < 0 || bitIndex > 15)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			return (this.GeneralPurposeFlag & (ushort)(1 << bitIndex)) != 0;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00027DA8 File Offset: 0x00026DA8
		public void SetGeneralPurposeFlagBit(int bitIndex)
		{
			if (bitIndex < 0 || bitIndex > 15)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			this.GeneralPurposeFlag |= (ushort)(1 << bitIndex);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x00027DCF File Offset: 0x00026DCF
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x00027E08 File Offset: 0x00026E08
		public ushort ActualCompressionMethod
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				if (ZipForgeCryptoTransformFactory.IsAESEncryption(this.EncryptionAlgorithm))
				{
					return ((AESExtraFieldData)this.ExtraFields.GetExtraFieldById(39169)).CompressionMethod;
				}
				return this.CompressionMethod;
			}
			set
			{
				if (!ZipForgeCryptoTransformFactory.IsAESEncryption(this.EncryptionAlgorithm))
				{
					this.CompressionMethod = value;
					return;
				}
				AESExtraFieldData aesextraFieldData = this.ExtraFields.GetExtraFieldById(39169) as AESExtraFieldData;
				if (aesextraFieldData != null)
				{
					aesextraFieldData.CompressionMethod = value;
					return;
				}
				this.CompressionMethod = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00027E52 File Offset: 0x00026E52
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x00027E5A File Offset: 0x00026E5A
		public ushort CompressionMethod
		{
			get
			{
				return this._compMethod;
			}
			set
			{
				this._compMethod = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00027E63 File Offset: 0x00026E63
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00027E6B File Offset: 0x00026E6B
		public ushort LastModificationTime
		{
			get
			{
				return this._lastModTime;
			}
			set
			{
				this._lastModTime = value;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00027E74 File Offset: 0x00026E74
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00027E7C File Offset: 0x00026E7C
		public ushort LastModificationDate
		{
			get
			{
				return this._lastModDate;
			}
			set
			{
				this._lastModDate = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00027E85 File Offset: 0x00026E85
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x00027EA1 File Offset: 0x00026EA1
		public uint CRC32
		{
			get
			{
				if (this._dataDescriptor == null)
				{
					return this._crc32;
				}
				return this._dataDescriptor.CRC32;
			}
			set
			{
				this._crc32 = value;
				if (this._dataDescriptor != null)
				{
					this._dataDescriptor.CRC32 = value;
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00027EC0 File Offset: 0x00026EC0
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x00027F4C File Offset: 0x00026F4C
		public long CompressedSize
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				long result;
				if (this._extraFields.Zip64ExtraField == null)
				{
					result = (long)((ulong)this._compSize);
				}
				else if (this._extraFields.Zip64ExtraField.CompressedFileSize >= 0L)
				{
					result = this._extraFields.Zip64ExtraField.CompressedFileSize;
				}
				else
				{
					result = (long)((ulong)this._compSize);
				}
				if (this._dataDescriptor != null && this._dataDescriptor.CompressedSize > 0L && this._dataDescriptor.CompressedSize != (long)((ulong)-1))
				{
					result = this._dataDescriptor.CompressedSize;
				}
				return result;
			}
			set
			{
				if (value < (long)((ulong)-1) && this._extraFields.Zip64ExtraField == null)
				{
					this._compSize = (uint)value;
					if (this._dataDescriptor != null)
					{
						this._dataDescriptor.CompressedSize = (long)((int)value);
						return;
					}
				}
				else
				{
					if (this._extraFields.Zip64ExtraField == null)
					{
						this._extraFields.AddExtraField(new Zip64ExtraFieldData(), this);
					}
					this._compSize = uint.MaxValue;
					this._uncompSize = uint.MaxValue;
					this._extraFields.Zip64ExtraField.CompressedFileSize = value;
					if (this._dataDescriptor != null)
					{
						this._dataDescriptor.CompressedSize = value;
					}
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00027FDC File Offset: 0x00026FDC
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x00028068 File Offset: 0x00027068
		public long UncompressedSize
		{
			get
			{
				this.FireLocalHeaderExtraFieldsNeededEvent();
				long result;
				if (this._extraFields.Zip64ExtraField == null)
				{
					result = (long)((ulong)this._uncompSize);
				}
				else if (this._extraFields.Zip64ExtraField.UncompressedFileSize >= 0L)
				{
					result = this._extraFields.Zip64ExtraField.UncompressedFileSize;
				}
				else
				{
					result = (long)((ulong)this._uncompSize);
				}
				if (this._dataDescriptor != null && this._dataDescriptor.UncompressedSize > 0L && this._dataDescriptor.UncompressedSize != (long)((ulong)-1))
				{
					result = this._dataDescriptor.UncompressedSize;
				}
				return result;
			}
			set
			{
				if (value < (long)((ulong)-1) && this._extraFields.Zip64ExtraField == null)
				{
					this._uncompSize = (uint)value;
					if (this._dataDescriptor != null)
					{
						this._dataDescriptor.UncompressedSize = (long)((int)value);
						return;
					}
				}
				else
				{
					this._isHugeFile = true;
					if (this._extraFields.Zip64ExtraField == null)
					{
						this._extraFields.AddExtraField(new Zip64ExtraFieldData(), this);
					}
					this._uncompSize = uint.MaxValue;
					this._compSize = uint.MaxValue;
					this._extraFields.Zip64ExtraField.UncompressedFileSize = value;
					if (this._dataDescriptor != null)
					{
						this._dataDescriptor.UncompressedSize = value;
					}
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000280FE File Offset: 0x000270FE
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00028106 File Offset: 0x00027106
		public DateTime LastFileModificationTime
		{
			get
			{
				return this._lastFileModificationTime;
			}
			set
			{
				this._lastFileModificationTime = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002810F File Offset: 0x0002710F
		public ushort NameLength
		{
			get
			{
				if (this.IsGeneralPurposeFlagBitSet(11))
				{
					return (ushort)Encoding.UTF8.GetByteCount(this.Name);
				}
				return (ushort)this.Name.Length;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00028139 File Offset: 0x00027139
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x00028141 File Offset: 0x00027141
		public DirItemDataDescriptor DataDescriptor
		{
			get
			{
				return this._dataDescriptor;
			}
			set
			{
				this._dataDescriptor = value;
				this.SetGeneralPurposeFlagBit(3);
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00028151 File Offset: 0x00027151
		public ushort GetExtraFieldsLength(ExtraFieldsTarget target)
		{
			this.FireLocalHeaderExtraFieldsNeededEvent();
			return this.ExtraFields.GetBytesLength(target);
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00028165 File Offset: 0x00027165
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x0002816D File Offset: 0x0002716D
		public ushort ExtraFieldsLenRead
		{
			get
			{
				return this._extraFieldsLenToRead;
			}
			set
			{
				this._extraFieldsLenToRead = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x00028176 File Offset: 0x00027176
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x0002817E File Offset: 0x0002717E
		public ushort CommentLengthToRead
		{
			get
			{
				return this._commentLengthToRead;
			}
			set
			{
				this._commentLengthToRead = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00028187 File Offset: 0x00027187
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x0002818F File Offset: 0x0002718F
		public ushort NameLengthToRead
		{
			get
			{
				return this._nameLengthToRead;
			}
			set
			{
				this._nameLengthToRead = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00028198 File Offset: 0x00027198
		public ushort CommentLength
		{
			get
			{
				if (this.IsGeneralPurposeFlagBitSet(11))
				{
					return (ushort)Encoding.UTF8.GetByteCount(this._comment);
				}
				return (ushort)this._comment.Length;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x000281C2 File Offset: 0x000271C2
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00028204 File Offset: 0x00027204
		public uint DiskStartNumber
		{
			get
			{
				if (this._extraFields.Zip64ExtraField == null)
				{
					return (uint)this._diskNumberStart;
				}
				if (this._extraFields.Zip64ExtraField.StartDiskNumber >= 4294967295U)
				{
					return (uint)this._diskNumberStart;
				}
				return this._extraFields.Zip64ExtraField.StartDiskNumber;
			}
			set
			{
				if (value < 65535U && this._extraFields.Zip64ExtraField == null)
				{
					this._diskNumberStart = (ushort)value;
					return;
				}
				if (this._extraFields.Zip64ExtraField == null)
				{
					this._extraFields.AddExtraField(new Zip64ExtraFieldData(), this);
				}
				this._diskNumberStart = ushort.MaxValue;
				this._uncompSize = uint.MaxValue;
				this._compSize = uint.MaxValue;
				this._relOffsetLh = uint.MaxValue;
				this._extraFields.Zip64ExtraField.StartDiskNumber = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0002827E File Offset: 0x0002727E
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00028286 File Offset: 0x00027286
		public ushort InternalAttributes
		{
			get
			{
				return this._internalAttr;
			}
			set
			{
				this._internalAttr = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0002828F File Offset: 0x0002728F
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00028297 File Offset: 0x00027297
		public FileAttributes ExternalAttributes
		{
			get
			{
				return this._externalAttr;
			}
			set
			{
				this._externalAttr = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x000282A0 File Offset: 0x000272A0
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x000282F0 File Offset: 0x000272F0
		public long RelativeLocalHeaderOffset
		{
			get
			{
				if (this._extraFields.Zip64ExtraField == null)
				{
					return (long)((ulong)this._relOffsetLh);
				}
				if (this._extraFields.Zip64ExtraField.LocalHeaderOffset < 0L)
				{
					return (long)((ulong)this._relOffsetLh);
				}
				return this._extraFields.Zip64ExtraField.LocalHeaderOffset;
			}
			set
			{
				if (value < (long)((ulong)-1) && this._extraFields.Zip64ExtraField == null)
				{
					this._relOffsetLh = (uint)value;
					return;
				}
				if (this._extraFields.Zip64ExtraField == null)
				{
					this._extraFields.AddExtraField(new Zip64ExtraFieldData(), this);
				}
				this._extraFields.Zip64ExtraField.LocalHeaderOffset = value;
				this._relOffsetLh = uint.MaxValue;
				this._uncompSize = uint.MaxValue;
				this._compSize = uint.MaxValue;
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000647 RID: 1607 RVA: 0x0002835C File Offset: 0x0002735C
		// (remove) Token: 0x06000648 RID: 1608 RVA: 0x00028375 File Offset: 0x00027375
		public event EventHandler ItemNameChanged;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06000649 RID: 1609 RVA: 0x0002838E File Offset: 0x0002738E
		// (remove) Token: 0x0600064A RID: 1610 RVA: 0x000283A7 File Offset: 0x000273A7
		public event EventHandler LocalHeaderExtraFieldsNeeded;

		// Token: 0x0400032E RID: 814
		private uint _signature;

		// Token: 0x0400032F RID: 815
		private ushort _versionMadeBy;

		// Token: 0x04000330 RID: 816
		private ushort _extractVersion;

		// Token: 0x04000331 RID: 817
		private ushort _genPurposeFlag;

		// Token: 0x04000332 RID: 818
		private ushort _compMethod;

		// Token: 0x04000333 RID: 819
		private ushort _lastModTime;

		// Token: 0x04000334 RID: 820
		private ushort _lastModDate;

		// Token: 0x04000335 RID: 821
		private uint _crc32;

		// Token: 0x04000336 RID: 822
		private uint _compSize;

		// Token: 0x04000337 RID: 823
		private uint _uncompSize;

		// Token: 0x04000338 RID: 824
		private ushort _diskNumberStart;

		// Token: 0x04000339 RID: 825
		private ushort _internalAttr;

		// Token: 0x0400033A RID: 826
		private FileAttributes _externalAttr;

		// Token: 0x0400033B RID: 827
		private uint _relOffsetLh;

		// Token: 0x0400033C RID: 828
		private bool _localHeaderExtraFieldsNeeded;

		// Token: 0x0400033D RID: 829
		private ushort _extraFieldsLenToRead;

		// Token: 0x0400033E RID: 830
		private ushort _nameLengthToRead;

		// Token: 0x0400033F RID: 831
		private ushort _commentLengthToRead;

		// Token: 0x04000340 RID: 832
		private bool _localHeaderExtraFieldIsCorrupted;

		// Token: 0x04000341 RID: 833
		private int _realLocalHeaderExtraFieldSize;

		// Token: 0x04000342 RID: 834
		private string _itemName;

		// Token: 0x04000343 RID: 835
		private string _oldName;

		// Token: 0x04000344 RID: 836
		private string _comment;

		// Token: 0x04000345 RID: 837
		private bool _isHugeFile;

		// Token: 0x04000346 RID: 838
		private bool _isModified;

		// Token: 0x04000347 RID: 839
		private EncryptionAlgorithm _encryptionAlgorithm;

		// Token: 0x04000348 RID: 840
		private string _password;

		// Token: 0x04000349 RID: 841
		private Stream _stream;

		// Token: 0x0400034A RID: 842
		private bool _needDestroyStream;

		// Token: 0x0400034B RID: 843
		private int _streamPosition;

		// Token: 0x0400034C RID: 844
		private long _centralDirStreamPos;

		// Token: 0x0400034D RID: 845
		private bool _isTagged;

		// Token: 0x0400034E RID: 846
		private string _srcFileName;

		// Token: 0x0400034F RID: 847
		private byte _compressionMode;

		// Token: 0x04000350 RID: 848
		private ExtraFieldsDataBlock _extraFields = new ExtraFieldsDataBlock();

		// Token: 0x04000351 RID: 849
		private DirItemDataDescriptor _dataDescriptor;

		// Token: 0x04000352 RID: 850
		private ProcessOperation _operation;

		// Token: 0x04000353 RID: 851
		private DateTime _lastFileModificationTime;
	}
}
