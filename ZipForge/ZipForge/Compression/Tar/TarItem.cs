using System;
using System.Globalization;
using System.IO;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Interfaces;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000065 RID: 101
	internal class TarItem : IItem, ICloneable
	{
		// Token: 0x06000436 RID: 1078 RVA: 0x0001F739 File Offset: 0x0001E739
		public TarItem(DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate) : this(CultureInfo.CurrentCulture.TextInfo.OEMCodePage, writeToStreamFailureDelegate, readFromStreamFailureDelegate)
		{
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001F752 File Offset: 0x0001E752
		public TarItem(string name, int codepage, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate) : this(codepage, writeToStreamFailureDelegate, readFromStreamFailureDelegate)
		{
			this._itemName = name;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001F768 File Offset: 0x0001E768
		public TarItem(int codepage, DoOnStreamOperationFailureDelegate writeToStreamFailureDelegate, DoOnStreamOperationFailureDelegate readFromStreamFailureDelegate)
		{
			this._codePage = codepage;
			this._itemName = string.Empty;
			this._oldName = string.Empty;
			this._header = new UnixStandartHeader(this._codePage);
			this._writeToStreamFailureDelegate = writeToStreamFailureDelegate;
			this._readFromStreamFailureDelegate = readFromStreamFailureDelegate;
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000439 RID: 1081 RVA: 0x0001F7B7 File Offset: 0x0001E7B7
		// (remove) Token: 0x0600043A RID: 1082 RVA: 0x0001F7D0 File Offset: 0x0001E7D0
		public event EventHandler ItemNameChanged;

		// Token: 0x0600043B RID: 1083 RVA: 0x0001F7EC File Offset: 0x0001E7EC
		public object Clone()
		{
			TarItem tarItem = new TarItem(this._codePage, this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate);
			tarItem.LastFileModificationTime = this.LastFileModificationTime;
			tarItem.Crc32 = this.Crc32;
			tarItem.UncompressedSize = this.UncompressedSize;
			tarItem.ExternalAttributes = this.ExternalAttributes;
			tarItem.Name = this.Name;
			tarItem._oldName = this.OldName;
			tarItem.IsModified = this.IsModified;
			tarItem.Stream = this.Stream;
			tarItem.NeedDestroyStream = this.NeedDestroyStream;
			tarItem.StreamPosition = this.StreamPosition;
			tarItem.IsTagged = this.IsTagged;
			tarItem.SrcFileName = this.SrcFileName;
			tarItem.Operation = this.Operation;
			tarItem.GroupId = this.GroupId;
			tarItem.GroupName = this.GroupName;
			tarItem.RelativeLocalHeaderOffset = this.RelativeLocalHeaderOffset;
			tarItem.UserId = this.UserId;
			tarItem.UserName = this.UserName;
			tarItem.TypeFlag = this.TypeFlag;
			tarItem.SetLongPathExtraHeader(this._longPathExtraHeader);
			return tarItem;
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001F902 File Offset: 0x0001E902
		public string OldName
		{
			get
			{
				return this._oldName;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0001F90A File Offset: 0x0001E90A
		public string ShortName
		{
			get
			{
				return this.Name.Substring(0, Math.Min(100, this.Name.Length));
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0001F92A File Offset: 0x0001E92A
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0001F934 File Offset: 0x0001E934
		public string Name
		{
			get
			{
				return this._itemName;
			}
			set
			{
				if (value.Length > 100)
				{
					this._longPathExtraHeader = new LongPathExtraHeader(this._codePage, this._writeToStreamFailureDelegate, this._readFromStreamFailureDelegate)
					{
						FileName = value
					};
				}
				else
				{
					this._longPathExtraHeader = null;
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0001F9B0 File Offset: 0x0001E9B0
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0001F9B8 File Offset: 0x0001E9B8
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0001F9C1 File Offset: 0x0001E9C1
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0001F9C9 File Offset: 0x0001E9C9
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0001F9D2 File Offset: 0x0001E9D2
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0001F9DA File Offset: 0x0001E9DA
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0001F9E3 File Offset: 0x0001E9E3
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0001F9EB File Offset: 0x0001E9EB
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001F9F4 File Offset: 0x0001E9F4
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001F9FC File Offset: 0x0001E9FC
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0001FA05 File Offset: 0x0001EA05
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0001FA0D File Offset: 0x0001EA0D
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001FA16 File Offset: 0x0001EA16
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0001FA1E File Offset: 0x0001EA1E
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0001FA27 File Offset: 0x0001EA27
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x0001FA2F File Offset: 0x0001EA2F
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001FA38 File Offset: 0x0001EA38
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0001FA40 File Offset: 0x0001EA40
		public int GroupId
		{
			get
			{
				return this._groupId;
			}
			set
			{
				this._groupId = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001FA49 File Offset: 0x0001EA49
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x0001FA51 File Offset: 0x0001EA51
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
			set
			{
				this._groupName = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0001FA5A File Offset: 0x0001EA5A
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x0001FA62 File Offset: 0x0001EA62
		public int UserId
		{
			get
			{
				return this._userId;
			}
			set
			{
				this._userId = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001FA6B File Offset: 0x0001EA6B
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0001FA73 File Offset: 0x0001EA73
		public string UserName
		{
			get
			{
				return this._userName;
			}
			set
			{
				this._userName = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001FA7C File Offset: 0x0001EA7C
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0001FA84 File Offset: 0x0001EA84
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001FA8D File Offset: 0x0001EA8D
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0001FA95 File Offset: 0x0001EA95
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001FA9E File Offset: 0x0001EA9E
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x0001FAA6 File Offset: 0x0001EAA6
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001FAAF File Offset: 0x0001EAAF
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x0001FAB7 File Offset: 0x0001EAB7
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0001FAC0 File Offset: 0x0001EAC0
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x0001FAC8 File Offset: 0x0001EAC8
		public char TypeFlag
		{
			get
			{
				return this._typeFlag;
			}
			set
			{
				this._typeFlag = value;
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001FAD4 File Offset: 0x0001EAD4
		public void WriteLocalHeaderToStream(Stream stream, int offset)
		{
			if (this._longPathExtraHeader != null)
			{
				this._longPathExtraHeader.Write(stream);
			}
			UnixStandartHeader unixStandartHeader = new UnixStandartHeader(this._codePage);
			unixStandartHeader.FileName = this.ShortName;
			unixStandartHeader.LastModification = this.LastFileModificationTime;
			unixStandartHeader.SizeInBytes = this.UncompressedSize;
			unixStandartHeader.UserId = this.UserId;
			unixStandartHeader.UserName = Convert.ToString(this.UserId, 8);
			unixStandartHeader.GroupId = this.GroupId;
			unixStandartHeader.GroupName = Convert.ToString(this.GroupId, 8);
			unixStandartHeader.Mode = (int)this.ExternalAttributes;
			unixStandartHeader.TypeFlag = this.TypeFlag;
			if (!ReadWriteHelper.WriteToStream(unixStandartHeader.GetHeaderValue(), offset, unixStandartHeader.HeaderSize, stream, this._writeToStreamFailureDelegate))
			{
				throw ExceptionBuilder.Exception(ErrorCode.WriteToTheStreamFailed);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001FB9E File Offset: 0x0001EB9E
		public int GetDataOffset()
		{
			return this.GetLocalHeaderSize();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001FBA8 File Offset: 0x0001EBA8
		public void Reset()
		{
			this.ExternalAttributes = FileAttributes.Normal;
			this.IsModified = false;
			this.IsTagged = false;
			this.Name = string.Empty;
			this.LastFileModificationTime = DateTime.Now;
			this.NeedDestroyStream = false;
			this._oldName = string.Empty;
			this.SrcFileName = string.Empty;
			this.Stream = null;
			this.StreamPosition = 0;
			this.UncompressedSize = 0L;
			this.Crc32 = 0U;
			this.GroupId = 61;
			this.GroupName = string.Empty;
			this.RelativeLocalHeaderOffset = 0L;
			this.UserId = 61;
			this.UserName = string.Empty;
			this.TypeFlag = '0';
			this._longPathExtraHeader = null;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001FC5C File Offset: 0x0001EC5C
		public void GetArchiveItem(ref BaseArchiveItem baseArchiveItem)
		{
			TarArchiveItem tarArchiveItem = baseArchiveItem as TarArchiveItem;
			string text = (this._longPathExtraHeader == null) ? this.Name : this._longPathExtraHeader.FileName;
			if (text[text.Length - 1] == '/')
			{
				text = text.Substring(0, text.Length - 1);
				tarArchiveItem.FileName = string.Empty;
				tarArchiveItem.StoredPath = text;
			}
			else
			{
				tarArchiveItem.FileName = Path.GetFileName(text);
				tarArchiveItem.StoredPath = Path.GetDirectoryName(text);
			}
			tarArchiveItem.SrcFileName = this.SrcFileName;
			tarArchiveItem._uncompressedSize = this.UncompressedSize;
			tarArchiveItem.FileModificationDateTime = this.LastFileModificationTime;
			tarArchiveItem.ExternalFileAttributes = this.ExternalAttributes;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001FD0C File Offset: 0x0001ED0C
		public void CopyFrom(BaseArchiveItem baseArchiveItem)
		{
			TarArchiveItem tarArchiveItem = baseArchiveItem as TarArchiveItem;
			this.Name = tarArchiveItem.FullName;
			this.SrcFileName = tarArchiveItem.SrcFileName;
			this.LastFileModificationTime = tarArchiveItem.FileModificationDateTime;
			this.ExternalAttributes = tarArchiveItem.ExternalFileAttributes;
			this.UncompressedSize = tarArchiveItem.UncompressedSize;
			if (File.Exists(tarArchiveItem.SrcFileName))
			{
				this.GroupId = FileUtils.GetFileOwnerGroupId(tarArchiveItem.SrcFileName);
				this.GroupName = FileUtils.GetFileOwnerGroupAccount(tarArchiveItem.SrcFileName);
				this.UserId = FileUtils.GetFileOwnerId(tarArchiveItem.SrcFileName);
				this.UserName = FileUtils.GetFileOwnerAccount(tarArchiveItem.SrcFileName);
			}
			if (Directory.Exists(tarArchiveItem.SrcFileName))
			{
				this.TypeFlag = '5';
			}
			this.IsModified = true;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001FDCC File Offset: 0x0001EDCC
		public bool IsDirectory()
		{
			string text = (this._longPathExtraHeader == null) ? this.Name : this._longPathExtraHeader.FileName;
			return this.TypeFlag == '5' || this.TypeFlag == 'D' || (this.UncompressedSize == 0L && text.EndsWith("/"));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001FE22 File Offset: 0x0001EE22
		public int GetLocalHeaderSize()
		{
			return this._header.HeaderSize + ((this._longPathExtraHeader == null) ? 0 : this._longPathExtraHeader.SizeOf());
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001FE46 File Offset: 0x0001EE46
		internal void SetLongPathExtraHeader(LongPathExtraHeader extraHeader)
		{
			this._longPathExtraHeader = extraHeader;
		}

		// Token: 0x0400029A RID: 666
		private string _itemName;

		// Token: 0x0400029B RID: 667
		private readonly ITarHeader _header;

		// Token: 0x0400029C RID: 668
		private LongPathExtraHeader _longPathExtraHeader;

		// Token: 0x0400029D RID: 669
		private readonly int _codePage;

		// Token: 0x0400029E RID: 670
		private readonly DoOnStreamOperationFailureDelegate _writeToStreamFailureDelegate;

		// Token: 0x0400029F RID: 671
		private readonly DoOnStreamOperationFailureDelegate _readFromStreamFailureDelegate;

		// Token: 0x040002A1 RID: 673
		internal string _oldName;

		// Token: 0x040002A2 RID: 674
		internal string _srcFileName;

		// Token: 0x040002A3 RID: 675
		internal ProcessOperation _operation;

		// Token: 0x040002A4 RID: 676
		internal Stream _stream;

		// Token: 0x040002A5 RID: 677
		internal bool _isModified;

		// Token: 0x040002A6 RID: 678
		internal bool _isTagged;

		// Token: 0x040002A7 RID: 679
		internal uint _crc32;

		// Token: 0x040002A8 RID: 680
		internal int _streamPosition;

		// Token: 0x040002A9 RID: 681
		internal bool _needDestroyStream;

		// Token: 0x040002AA RID: 682
		internal int _groupId;

		// Token: 0x040002AB RID: 683
		internal string _groupName;

		// Token: 0x040002AC RID: 684
		internal int _userId;

		// Token: 0x040002AD RID: 685
		internal string _userName;

		// Token: 0x040002AE RID: 686
		internal long _uncompressedSize;

		// Token: 0x040002AF RID: 687
		internal long _relativeLocalHeaderOffset;

		// Token: 0x040002B0 RID: 688
		internal DateTime _lastFileModificationTime;

		// Token: 0x040002B1 RID: 689
		internal FileAttributes _externalAttributes;

		// Token: 0x040002B2 RID: 690
		internal char _typeFlag;
	}
}
