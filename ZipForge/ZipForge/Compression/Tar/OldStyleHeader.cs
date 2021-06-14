using System;
using System.Net;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x0200005F RID: 95
	internal class OldStyleHeader : ITarHeader
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x0001E994 File Offset: 0x0001D994
		public OldStyleHeader(int codepage)
		{
			this._codepage = codepage;
			this.Mode = 511;
			this.UserId = 61;
			this.GroupId = 61;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001E9EE File Offset: 0x0001D9EE
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0001EA18 File Offset: 0x0001DA18
		public virtual string FileName
		{
			get
			{
				if (this._fileName != null)
				{
					return this._fileName.Replace("\0", string.Empty);
				}
				throw ExceptionBuilder.Exception(ErrorCode.FileNameWasNotSpecified);
			}
			set
			{
				if (CompressionUtils.IsNullOrEmpty(value))
				{
					throw ExceptionBuilder.Exception(ErrorCode.SpecifiedFileNameIsNullOrEmpty);
				}
				if (value.Length > 100)
				{
					throw ExceptionBuilder.Exception(ErrorCode.NameTooLong, new object[]
					{
						value,
						100
					});
				}
				this._fileName = value.Substring(0, Math.Min(100, value.Length));
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001EA77 File Offset: 0x0001DA77
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0001EA7F File Offset: 0x0001DA7F
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001EA88 File Offset: 0x0001DA88
		public string GroupIdString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString(this.GroupId, 8), 7, '0', true);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001EAA0 File Offset: 0x0001DAA0
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0001EABB File Offset: 0x0001DABB
		public virtual string GroupName
		{
			get
			{
				return this.GroupId.ToString();
			}
			set
			{
				this.GroupId = int.Parse(value);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001EAC9 File Offset: 0x0001DAC9
		public string HeaderChecksumString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString(this._headerChecksum, 8), 6, '0', true);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0001EAE0 File Offset: 0x0001DAE0
		public virtual int HeaderSize
		{
			get
			{
				return 512;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0001EAE7 File Offset: 0x0001DAE7
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0001EAEF File Offset: 0x0001DAEF
		public DateTime LastModification
		{
			get
			{
				return this._lastModification;
			}
			set
			{
				this._lastModification = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0001EAF8 File Offset: 0x0001DAF8
		public string LastModificationString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString((long)(this.LastModification - this._theEpoch).TotalSeconds, 8), 11, '0', true);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001EB2F File Offset: 0x0001DB2F
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0001EB37 File Offset: 0x0001DB37
		public int Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				this._mode = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001EB40 File Offset: 0x0001DB40
		public string ModeString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString(this.Mode, 8), 7, '0', true);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0001EB57 File Offset: 0x0001DB57
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0001EB5F File Offset: 0x0001DB5F
		public long SizeInBytes
		{
			get
			{
				return this._sizeInBytes;
			}
			set
			{
				this._sizeInBytes = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001EB68 File Offset: 0x0001DB68
		public string SizeString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString(this.SizeInBytes, 8), 11, '0', true);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001EB80 File Offset: 0x0001DB80
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0001EB88 File Offset: 0x0001DB88
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001EB91 File Offset: 0x0001DB91
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0001EB99 File Offset: 0x0001DB99
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001EBA2 File Offset: 0x0001DBA2
		public string UserIdString
		{
			get
			{
				return OldStyleHeader.AddChars(Convert.ToString(this.UserId, 8), 7, '0', true);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001EBBC File Offset: 0x0001DBBC
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0001EBD7 File Offset: 0x0001DBD7
		public virtual string UserName
		{
			get
			{
				return this.UserId.ToString();
			}
			set
			{
				this.UserId = int.Parse(value);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001EBE5 File Offset: 0x0001DBE5
		public byte[] GetBytes()
		{
			return this._buffer;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001EBF0 File Offset: 0x0001DBF0
		public virtual byte[] GetHeaderValue()
		{
			Encoding encoding = Encoding.GetEncoding(this._codepage);
			for (int i = 0; i < 512; i++)
			{
				this._buffer[i] = 0;
			}
			if (CompressionUtils.IsNullOrEmpty(this._fileName))
			{
				throw ExceptionBuilder.Exception(ErrorCode.FileNameEmpty);
			}
			if (this._fileName.Length > 100)
			{
				throw ExceptionBuilder.Exception(ErrorCode.NameTooLong, new object[]
				{
					this._fileName,
					100
				});
			}
			if (this._fileName.Length == 100)
			{
				encoding.GetBytes(this._fileName).CopyTo(this._buffer, 0);
			}
			else
			{
				encoding.GetBytes(OldStyleHeader.AddChars(this._fileName, 100, '\0', false)).CopyTo(this._buffer, 0);
			}
			encoding.GetBytes(this.ModeString).CopyTo(this._buffer, 100);
			encoding.GetBytes(this.UserIdString).CopyTo(this._buffer, 108);
			encoding.GetBytes(this.GroupIdString).CopyTo(this._buffer, 116);
			encoding.GetBytes(this.SizeString).CopyTo(this._buffer, 124);
			encoding.GetBytes(this.LastModificationString).CopyTo(this._buffer, 136);
			encoding.GetBytes(new char[]
			{
				this.TypeFlag
			}).CopyTo(this._buffer, 156);
			this.RecalculateChecksum(this._buffer);
			encoding.GetBytes(this.HeaderChecksumString).CopyTo(this._buffer, 148);
			return this._buffer;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001ED88 File Offset: 0x0001DD88
		public virtual bool UpdateHeaderFromBytes()
		{
			Encoding encoding = Encoding.GetEncoding(this._codepage);
			this.FileName = OldStyleHeader.TrimString(encoding.GetString(this._buffer, 0, 100));
			try
			{
				this.Mode = Convert.ToInt32(int.Parse(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 100, 7))).ToString(), 8);
			}
			catch
			{
				this.Mode = 32759;
			}
			try
			{
				this.UserId = Convert.ToInt32(int.Parse(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 108, 7))).ToString(), 8);
			}
			catch
			{
				this.UserId = 61;
			}
			try
			{
				this.GroupId = Convert.ToInt32(int.Parse(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 116, 7))).ToString(), 8);
			}
			catch
			{
				this.GroupId = 61;
			}
			this.TypeFlag = ((OldStyleHeader.TrimString(encoding.GetString(this._buffer, 156, 1)).Length > 0) ? Convert.ToChar(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 156, 1))) : '0');
			if ((this._buffer[124] & 128) == 128)
			{
				long network = BitConverter.ToInt64(this._buffer, 128);
				this.SizeInBytes = IPAddress.NetworkToHostOrder(network);
			}
			else
			{
				try
				{
					this.SizeInBytes = Convert.ToInt64(long.Parse(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 124, 11))).ToString(), 8);
				}
				catch
				{
					this.SizeInBytes = 0L;
				}
			}
			long num;
			try
			{
				num = long.Parse(OldStyleHeader.TrimString(encoding.GetString(this._buffer, 136, 11)));
			}
			catch
			{
				num = 0L;
			}
			num = Convert.ToInt64(num.ToString(), 8);
			this.LastModification = this._theEpoch.AddSeconds((double)num);
			int num2;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(encoding.GetString(this._buffer, 148, 8));
				for (int i = 0; i < stringBuilder.Length; i++)
				{
					if (!char.IsDigit(stringBuilder.ToString(), i))
					{
						stringBuilder.Remove(i, 1);
						i--;
					}
				}
				num2 = int.Parse(OldStyleHeader.TrimString(stringBuilder.ToString()));
			}
			catch
			{
				num2 = 0;
			}
			num2 = Convert.ToInt32(num2.ToString(), 8);
			this.RecalculateChecksum(this._buffer);
			if ((long)num2 == this._headerChecksum)
			{
				return true;
			}
			this.RecalculateAltChecksum(this._buffer);
			return (long)num2 == this._headerChecksum;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001F06C File Offset: 0x0001E06C
		internal void UnsafeSetFileName(string filename)
		{
			this._fileName = filename;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001F078 File Offset: 0x0001E078
		protected virtual void RecalculateChecksum(byte[] buf)
		{
			Encoding.GetEncoding(this._codepage).GetBytes("        ").CopyTo(buf, 148);
			this._headerChecksum = 0L;
			foreach (byte b in buf)
			{
				this._headerChecksum += (long)((ulong)b);
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001F0D0 File Offset: 0x0001E0D0
		protected static string AddChars(string str, int num, char ch, bool isLeading)
		{
			for (int i = num - str.Length; i > 0; i--)
			{
				if (isLeading)
				{
					str = ch + str;
				}
				else
				{
					str += ch;
				}
			}
			return str;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001F114 File Offset: 0x0001E114
		protected static string TrimString(string str)
		{
			char[] array = new char[2];
			array[0] = ' ';
			return str.Trim(array);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001F134 File Offset: 0x0001E134
		private void RecalculateAltChecksum(byte[] buf)
		{
			Encoding.GetEncoding(this._codepage).GetBytes("        ").CopyTo(buf, 148);
			this._headerChecksum = 0L;
			foreach (byte b in buf)
			{
				if ((b & 128) == 128)
				{
					this._headerChecksum -= (long)(b ^ 128);
				}
				else
				{
					this._headerChecksum += (long)((ulong)b);
				}
			}
		}

		// Token: 0x0400028C RID: 652
		protected readonly int _codepage;

		// Token: 0x0400028D RID: 653
		private readonly byte[] _buffer = new byte[512];

		// Token: 0x0400028E RID: 654
		protected readonly DateTime _theEpoch = new DateTime(1970, 1, 1, 0, 0, 0);

		// Token: 0x0400028F RID: 655
		private string _fileName;

		// Token: 0x04000290 RID: 656
		private long _headerChecksum;

		// Token: 0x04000291 RID: 657
		internal int _groupId;

		// Token: 0x04000292 RID: 658
		internal DateTime _lastModification;

		// Token: 0x04000293 RID: 659
		internal int _mode;

		// Token: 0x04000294 RID: 660
		internal long _sizeInBytes;

		// Token: 0x04000295 RID: 661
		internal int _userId;

		// Token: 0x04000296 RID: 662
		internal char _typeFlag;
	}
}
