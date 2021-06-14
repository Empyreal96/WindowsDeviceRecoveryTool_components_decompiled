using System;
using System.Globalization;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;
using ComponentAce.Compression.Tar;
using ComponentAce.Compression.ZipForge;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000043 RID: 67
	internal class GzipItem : IItem, ICloneable
	{
		// Token: 0x0600029F RID: 671 RVA: 0x00018100 File Offset: 0x00017100
		public GzipItem(DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			this.Reset();
			this._codepage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			this._itemName = string.Empty;
			this._oldName = string.Empty;
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00018152 File Offset: 0x00017152
		public GzipItem(string name, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate) : this(writeToStreamFailureDelegate, readFromStreamFailureDelegate)
		{
			this.Name = name;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00018163 File Offset: 0x00017163
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0001816B File Offset: 0x0001716B
		public byte CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00018174 File Offset: 0x00017174
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0001817C File Offset: 0x0001717C
		public ushort ExtraFieldLenToRead
		{
			get
			{
				return this._extraFieldLenToRead;
			}
			set
			{
				this._extraFieldLenToRead = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00018185 File Offset: 0x00017185
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0001818D File Offset: 0x0001718D
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
					this.RemoveFlagBit(4);
					this._comment = string.Empty;
					return;
				}
				this.SetFlagFlagBit(4);
				this._comment = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000181B8 File Offset: 0x000171B8
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x000181C0 File Offset: 0x000171C0
		public byte[] ExtraFieldData
		{
			get
			{
				return this._extraFieldData;
			}
			set
			{
				this._extraFieldData = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x000181C9 File Offset: 0x000171C9
		// (set) Token: 0x060002AA RID: 682 RVA: 0x000181D1 File Offset: 0x000171D1
		public long CompressedSize
		{
			get
			{
				return this._compressedSize;
			}
			set
			{
				this._compressedSize = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002AB RID: 683 RVA: 0x000181DA File Offset: 0x000171DA
		// (set) Token: 0x060002AC RID: 684 RVA: 0x000181E2 File Offset: 0x000171E2
		public uint Crc32
		{
			get
			{
				return this._crc32;
			}
			set
			{
				this._crc32 = value;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000181EC File Offset: 0x000171EC
		public object Clone()
		{
			return new GzipItem(this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate)
			{
				ExtraFlags = this.ExtraFlags,
				Flag = this.Flag,
				CompressionMethod = this.CompressionMethod,
				OperationSystem = this.OperationSystem,
				ExtraFieldLenToRead = this.ExtraFieldLenToRead,
				Comment = this.Comment,
				HeaderCrc = this.HeaderCrc,
				ExtraFieldData = (byte[])this.ExtraFieldData.Clone(),
				CompressedSize = this.CompressedSize,
				Crc32 = this.Crc32,
				Name = this.Name,
				_oldName = this.OldName,
				SrcFileName = this.SrcFileName,
				IsModified = this.IsModified,
				IsTagged = this.IsTagged,
				StreamPosition = this.StreamPosition,
				NeedDestroyStream = this.NeedDestroyStream,
				UncompressedSize = this.UncompressedSize,
				RelativeLocalHeaderOffset = this.RelativeLocalHeaderOffset,
				LastFileModificationTime = this.LastFileModificationTime,
				ExternalAttributes = this.ExternalAttributes,
				DestinationStream = this.DestinationStream,
				NeedToDestroyDestinationStream = this.NeedToDestroyDestinationStream,
				NeedToStoreHeaderCRC = this.NeedToStoreHeaderCRC,
				_backupFileName = this._backupFileName
			};
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00018342 File Offset: 0x00017342
		public string OldName
		{
			get
			{
				return this._oldName;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0001834A File Offset: 0x0001734A
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x00018354 File Offset: 0x00017354
		public string Name
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					this.RemoveFlagBit(3);
				}
				else
				{
					this.SetFlagFlagBit(3);
				}
				if (this._itemName != value)
				{
					this._oldName = this._itemName;
					this._itemName = value;
					if (this.ItemNameChanged != null)
					{
						this.ItemNameChanged(this, null);
					}
				}
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x000183AF File Offset: 0x000173AF
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x000183B7 File Offset: 0x000173B7
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x000183C0 File Offset: 0x000173C0
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x000183C8 File Offset: 0x000173C8
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x000183D1 File Offset: 0x000173D1
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x000183D9 File Offset: 0x000173D9
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x000183E2 File Offset: 0x000173E2
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x000183EA File Offset: 0x000173EA
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x000183F3 File Offset: 0x000173F3
		// (set) Token: 0x060002BA RID: 698 RVA: 0x000183FB File Offset: 0x000173FB
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00018404 File Offset: 0x00017404
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0001840C File Offset: 0x0001740C
		public bool NeedToStoreHeaderCRC
		{
			get
			{
				return this._needToStoreHeaderCrc;
			}
			set
			{
				this._needToStoreHeaderCrc = value;
				if (this._needToStoreHeaderCrc)
				{
					this.SetFlagFlagBit(1);
					return;
				}
				this.RemoveFlagBit(1);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0001842C File Offset: 0x0001742C
		// (set) Token: 0x060002BE RID: 702 RVA: 0x00018434 File Offset: 0x00017434
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0001843D File Offset: 0x0001743D
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x00018445 File Offset: 0x00017445
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0001844E File Offset: 0x0001744E
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x00018456 File Offset: 0x00017456
		public long UncompressedSize
		{
			get
			{
				return this._uncompressedSize;
			}
			set
			{
				this._uncompressedSize = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0001845F File Offset: 0x0001745F
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00018467 File Offset: 0x00017467
		public long RelativeLocalHeaderOffset
		{
			get
			{
				return this._relativeLocalHeaderOffset;
			}
			set
			{
				this._relativeLocalHeaderOffset = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00018470 File Offset: 0x00017470
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00018478 File Offset: 0x00017478
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00018481 File Offset: 0x00017481
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00018489 File Offset: 0x00017489
		public FileAttributes ExternalAttributes
		{
			get
			{
				return this._externalAttributes;
			}
			set
			{
				this._externalAttributes = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00018492 File Offset: 0x00017492
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0001849A File Offset: 0x0001749A
		public byte CompressionMode
		{
			get
			{
				return this.ExtraFlags;
			}
			set
			{
				this.ExtraFlags = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002CB RID: 715 RVA: 0x000184A3 File Offset: 0x000174A3
		// (set) Token: 0x060002CC RID: 716 RVA: 0x000184AB File Offset: 0x000174AB
		public Stream DestinationStream
		{
			get
			{
				return this._destinationStream;
			}
			set
			{
				this._destinationStream = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002CD RID: 717 RVA: 0x000184B4 File Offset: 0x000174B4
		// (set) Token: 0x060002CE RID: 718 RVA: 0x000184BC File Offset: 0x000174BC
		public bool NeedToDestroyDestinationStream
		{
			get
			{
				return this._needToDestroyDestinationStream;
			}
			set
			{
				this._needToDestroyDestinationStream = value;
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000184C8 File Offset: 0x000174C8
		public void WriteLocalHeaderToStream(Stream stream, int offset)
		{
			byte[] array = new byte[this.GetLocalHeaderSize()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.Id1);
			binaryWriter.Write(this.Id2);
			binaryWriter.Write(this.CompressionMethod);
			binaryWriter.Write(this.Flag);
			binaryWriter.Write((int)this.LastFileModificationTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
			binaryWriter.Write(this.ExtraFlags);
			binaryWriter.Write(this.OperationSystem);
			if (this.IsFlagBitSet(2))
			{
				binaryWriter.Write(this.ExtraFieldLenToRead);
				binaryWriter.Write(this.ExtraFieldData);
			}
			if (this.IsFlagBitSet(3))
			{
				binaryWriter.Write(Encoding.GetEncoding(this._codepage).GetBytes(this.Name));
				binaryWriter.Write('\0');
			}
			if (this.IsFlagBitSet(4))
			{
				binaryWriter.Write(Encoding.GetEncoding(this._codepage).GetBytes(this.Comment));
				binaryWriter.Write('\0');
			}
			if (this.IsFlagBitSet(1))
			{
				this.ReCalculateHeaderCRC();
				binaryWriter.Write(this.HeaderCrc);
			}
			if (!ReadWriteHelper.WriteToStream(array, 0, array.Length, stream, this._writeToStreamFailureDelegate))
			{
				throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00018613 File Offset: 0x00017613
		public int GetDataOffset()
		{
			return this.GetLocalHeaderSize();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001861C File Offset: 0x0001761C
		private void ReCalculateHeaderCRC()
		{
			uint num = 0U;
			byte[] array = new byte[this.GetLocalHeaderSize()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.Id1);
			binaryWriter.Write(this.Id2);
			binaryWriter.Write(this.CompressionMethod);
			binaryWriter.Write(this.Flag);
			binaryWriter.Write((int)this.LastFileModificationTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
			binaryWriter.Write(this.ExtraFlags);
			binaryWriter.Write(this.OperationSystem);
			if (this.IsFlagBitSet(2))
			{
				binaryWriter.Write(this.ExtraFieldLenToRead);
				binaryWriter.Write(this.ExtraFieldData);
			}
			if (this.IsFlagBitSet(3))
			{
				binaryWriter.Write(Encoding.GetEncoding(this._codepage).GetBytes(this.Name));
				binaryWriter.Write('\0');
			}
			if (this.IsFlagBitSet(4))
			{
				binaryWriter.Write(Encoding.GetEncoding(this._codepage).GetBytes(this.Comment));
				binaryWriter.Write('\0');
			}
			ZipUtil.UpdateCRC32(array, (uint)array.Length, ref num);
			this.HeaderCrc = (ushort)((byte)num);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00018744 File Offset: 0x00017744
		public void Reset()
		{
			this.ExtraFlags = 0;
			this.Flag = 0;
			this.CompressionMethod = 8;
			this.OperationSystem = 0;
			this.ExtraFieldLenToRead = 0;
			this.Comment = string.Empty;
			this.HeaderCrc = 0;
			this.ExtraFieldData = new byte[0];
			this.CompressedSize = 0L;
			this.Crc32 = 0U;
			this._oldName = string.Empty;
			this.Name = string.Empty;
			this.SrcFileName = string.Empty;
			this.IsModified = false;
			this.IsTagged = false;
			this.StreamPosition = 0;
			this.NeedDestroyStream = false;
			this.UncompressedSize = 0L;
			this.RelativeLocalHeaderOffset = 0L;
			this.LastFileModificationTime = DateTime.Now;
			this.ExternalAttributes = FileAttributes.Normal;
			this._backupFileName = string.Empty;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00018810 File Offset: 0x00017810
		public void GetArchiveItem(ref BaseArchiveItem baseArchiveItem)
		{
			GzipArchiveItem gzipArchiveItem = baseArchiveItem as GzipArchiveItem;
			gzipArchiveItem.FileName = this.Name;
			gzipArchiveItem.SrcFileName = this.SrcFileName;
			gzipArchiveItem._compressedSize = this.CompressedSize;
			gzipArchiveItem._uncompressedSize = this.UncompressedSize;
			if (gzipArchiveItem.UncompressedSize > 0L)
			{
				gzipArchiveItem._compressionRate = (double)(1f - (float)gzipArchiveItem.CompressedSize / (float)gzipArchiveItem.UncompressedSize) * 100.0;
			}
			else
			{
				gzipArchiveItem._compressionRate = 100.0;
			}
			if (gzipArchiveItem._compressionRate < 0.0)
			{
				gzipArchiveItem._compressionRate = 0.0;
			}
			gzipArchiveItem.FileModificationDateTime = this.LastFileModificationTime;
			gzipArchiveItem._crc = this.Crc32;
			gzipArchiveItem.ExternalFileAttributes = this.ExternalAttributes;
			gzipArchiveItem.Comment = this.Comment;
			gzipArchiveItem.NeedToStoreHeaderCRC = this.NeedToStoreHeaderCRC;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000188F4 File Offset: 0x000178F4
		public void CopyFrom(BaseArchiveItem baseArchiveItem)
		{
			GzipArchiveItem gzipArchiveItem = baseArchiveItem as GzipArchiveItem;
			this.Name = gzipArchiveItem.FullName;
			this.SrcFileName = gzipArchiveItem.SrcFileName;
			this.LastFileModificationTime = gzipArchiveItem.FileModificationDateTime;
			this.ExternalAttributes = gzipArchiveItem.ExternalFileAttributes;
			this.UncompressedSize = gzipArchiveItem.UncompressedSize;
			this.Comment = gzipArchiveItem.Comment;
			this.CompressedSize = gzipArchiveItem.CompressedSize;
			this.IsModified = true;
			this.NeedToStoreHeaderCRC = gzipArchiveItem.NeedToStoreHeaderCRC;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0001896F File Offset: 0x0001796F
		public bool IsDirectory()
		{
			return false;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00018974 File Offset: 0x00017974
		public int GetLocalHeaderSize()
		{
			return GzipHeader.SizeOf() + (this.IsFlagBitSet(2) ? ((int)this.ExtraFieldLenToRead + this.ExtraFieldData.Length) : 0) + (this.IsFlagBitSet(3) ? (this.Name.Length + 1) : 0) + (this.IsFlagBitSet(4) ? (this.Comment.Length + 1) : 0) + (this.IsFlagBitSet(1) ? 2 : 0);
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060002D7 RID: 727 RVA: 0x000189E4 File Offset: 0x000179E4
		// (remove) Token: 0x060002D8 RID: 728 RVA: 0x000189FD File Offset: 0x000179FD
		public event EventHandler ItemNameChanged;

		// Token: 0x060002D9 RID: 729 RVA: 0x00018A16 File Offset: 0x00017A16
		internal bool IsFlagBitSet(int bitIndex)
		{
			if (bitIndex < 0 || bitIndex > 7)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			return ((ushort)this.Flag & (ushort)(1 << bitIndex)) != 0;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00018A3B File Offset: 0x00017A3B
		internal void SetFlagFlagBit(int bitIndex)
		{
			if (bitIndex < 0 || bitIndex > 7)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			this.Flag |= (byte)(1 << bitIndex);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00018A61 File Offset: 0x00017A61
		internal void RemoveFlagBit(int bitIndex)
		{
			if (bitIndex < 0 || bitIndex > 7)
			{
				throw ExceptionBuilder.Exception(ErrorCode.IndexOutOfBounds);
			}
			this.Flag &= (byte)(255 ^ 1 << bitIndex);
		}

		// Token: 0x040001CA RID: 458
		private readonly int _codepage;

		// Token: 0x040001CB RID: 459
		private readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x040001CC RID: 460
		private readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;

		// Token: 0x040001CD RID: 461
		private string _itemName;

		// Token: 0x040001CE RID: 462
		public byte ExtraFlags;

		// Token: 0x040001CF RID: 463
		public byte Flag;

		// Token: 0x040001D0 RID: 464
		public byte Id1;

		// Token: 0x040001D1 RID: 465
		public byte Id2;

		// Token: 0x040001D2 RID: 466
		private byte _compressionMethod;

		// Token: 0x040001D3 RID: 467
		public byte OperationSystem;

		// Token: 0x040001D4 RID: 468
		private ushort _extraFieldLenToRead;

		// Token: 0x040001D5 RID: 469
		private string _comment;

		// Token: 0x040001D6 RID: 470
		public ushort HeaderCrc;

		// Token: 0x040001D7 RID: 471
		private byte[] _extraFieldData;

		// Token: 0x040001D8 RID: 472
		private long _compressedSize;

		// Token: 0x040001D9 RID: 473
		private uint _crc32;

		// Token: 0x040001DA RID: 474
		internal string _backupFileName;

		// Token: 0x040001DB RID: 475
		private string _oldName;

		// Token: 0x040001DC RID: 476
		private string _srcFileName;

		// Token: 0x040001DD RID: 477
		private ProcessOperation _operation;

		// Token: 0x040001DE RID: 478
		private Stream _stream;

		// Token: 0x040001DF RID: 479
		private bool _isModified;

		// Token: 0x040001E0 RID: 480
		private bool _isTagged;

		// Token: 0x040001E1 RID: 481
		private bool _needToStoreHeaderCrc;

		// Token: 0x040001E2 RID: 482
		private int _streamPosition;

		// Token: 0x040001E3 RID: 483
		private bool _needDestroyStream;

		// Token: 0x040001E4 RID: 484
		private long _uncompressedSize;

		// Token: 0x040001E5 RID: 485
		private long _relativeLocalHeaderOffset;

		// Token: 0x040001E6 RID: 486
		private DateTime _lastFileModificationTime;

		// Token: 0x040001E7 RID: 487
		private FileAttributes _externalAttributes;

		// Token: 0x040001E8 RID: 488
		private Stream _destinationStream;

		// Token: 0x040001E9 RID: 489
		private bool _needToDestroyDestinationStream;
	}
}
