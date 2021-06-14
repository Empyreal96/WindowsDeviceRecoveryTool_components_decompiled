using System;
using System.Net;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000069 RID: 105
	internal class UnixStandartHeader : OldStyleHeader
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x00020480 File Offset: 0x0001F480
		public UnixStandartHeader(int codepage) : base(codepage)
		{
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00020494 File Offset: 0x0001F494
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x000204C8 File Offset: 0x0001F4C8
		public override string FileName
		{
			get
			{
				return this._namePrefix.Replace("\0", string.Empty) + base.FileName.Replace("\0", string.Empty);
			}
			set
			{
				if (value.Length > 100)
				{
					throw ExceptionBuilder.Exception(ErrorCode.NameTooLong, new object[]
					{
						value,
						100
					});
				}
				base.FileName = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00020504 File Offset: 0x0001F504
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x0002051C File Offset: 0x0001F51C
		public override string GroupName
		{
			get
			{
				return this._groupName.Replace("\0", string.Empty);
			}
			set
			{
				if (value.Length > 32)
				{
					throw ExceptionBuilder.Exception(ErrorCode.NameTooLong, new object[]
					{
						value,
						32
					});
				}
				this._groupName = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00020558 File Offset: 0x0001F558
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00020570 File Offset: 0x0001F570
		public override string UserName
		{
			get
			{
				return this._userName.Replace("\0", string.Empty);
			}
			set
			{
				if (value.Length > 32)
				{
					throw ExceptionBuilder.Exception(ErrorCode.NameTooLong, new object[]
					{
						value,
						32
					});
				}
				this._userName = value;
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000205AC File Offset: 0x0001F5AC
		public override byte[] GetHeaderValue()
		{
			Encoding encoding = Encoding.GetEncoding(this._codepage);
			byte[] headerValue = base.GetHeaderValue();
			encoding.GetBytes(OldStyleHeader.AddChars("ustar", 6, '\0', false)).CopyTo(headerValue, 257);
			encoding.GetBytes("00").CopyTo(headerValue, 263);
			encoding.GetBytes(this.UserName).CopyTo(headerValue, 265);
			encoding.GetBytes(this.GroupName).CopyTo(headerValue, 297);
			encoding.GetBytes(this._namePrefix).CopyTo(headerValue, 345);
			if (base.SizeInBytes >= 8589934591L)
			{
				byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(base.SizeInBytes));
				UnixStandartHeader.SetMarker(UnixStandartHeader.AlignTo12(bytes)).CopyTo(headerValue, 124);
			}
			this.RecalculateChecksum(headerValue);
			encoding.GetBytes(base.HeaderChecksumString).CopyTo(headerValue, 148);
			return headerValue;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0002069C File Offset: 0x0001F69C
		public override bool UpdateHeaderFromBytes()
		{
			Encoding encoding = Encoding.GetEncoding(this._codepage);
			byte[] bytes = base.GetBytes();
			this.UserName = OldStyleHeader.TrimString(encoding.GetString(bytes, 265, 32));
			this.GroupName = OldStyleHeader.TrimString(encoding.GetString(bytes, 297, 32));
			this._namePrefix = OldStyleHeader.TrimString(encoding.GetString(bytes, 347, 157));
			return base.UpdateHeaderFromBytes();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00020710 File Offset: 0x0001F710
		private static byte[] AlignTo12(byte[] bytes)
		{
			byte[] array = new byte[12];
			bytes.CopyTo(array, 12 - bytes.Length);
			return array;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00020733 File Offset: 0x0001F733
		private static byte[] SetMarker(byte[] bytes)
		{
			int num = 0;
			bytes[num] |= 128;
			return bytes;
		}

		// Token: 0x040002BF RID: 703
		private const string Magic = "ustar";

		// Token: 0x040002C0 RID: 704
		private const string Version = "00";

		// Token: 0x040002C1 RID: 705
		private string _groupName;

		// Token: 0x040002C2 RID: 706
		private string _namePrefix = string.Empty;

		// Token: 0x040002C3 RID: 707
		private string _userName;
	}
}
