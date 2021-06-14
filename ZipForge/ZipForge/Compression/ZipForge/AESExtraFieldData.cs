using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x0200009A RID: 154
	internal class AESExtraFieldData : ExtraFieldData
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x0002B320 File Offset: 0x0002A320
		public AESExtraFieldData() : this(0, 0, 0)
		{
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002B32C File Offset: 0x0002A32C
		public AESExtraFieldData(ushort versionNumber, byte strength, ushort compressionMethod)
		{
			this.vendorID = new byte[]
			{
				65,
				69
			};
			this.HeaderId = 39169;
			this.strength = strength;
			switch (strength)
			{
			case 1:
				this.keyLengthBits = 128;
				break;
			case 2:
				this.keyLengthBits = 192;
				break;
			case 3:
				this.keyLengthBits = 256;
				break;
			}
			this.versionNumber = versionNumber;
			this.compressionMethod = compressionMethod;
			this.UpdateBaseClassFields();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002B3BC File Offset: 0x0002A3BC
		public static AESExtraFieldData LoadFromStream(Stream source, DirItem item)
		{
			BinaryReader binaryReader = new BinaryReader(source);
			binaryReader.ReadUInt16();
			ushort num = binaryReader.ReadUInt16();
			binaryReader.ReadUInt16();
			byte b = binaryReader.ReadByte();
			ushort num2 = binaryReader.ReadUInt16();
			item.CompressionMethod = 99;
			return new AESExtraFieldData(num, b, num2);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002B404 File Offset: 0x0002A404
		public override object Clone()
		{
			return new AESExtraFieldData(this.versionNumber, this.strength, this.compressionMethod)
			{
				LocalHeaderExtraFieldsLength = this.LocalHeaderExtraFieldsLength
			};
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002B436 File Offset: 0x0002A436
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0002B43E File Offset: 0x0002A43E
		public ushort VersionNumber
		{
			get
			{
				return this.versionNumber;
			}
			set
			{
				this.versionNumber = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002B44D File Offset: 0x0002A44D
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002B458 File Offset: 0x0002A458
		public int KeyLengthBits
		{
			get
			{
				return this.keyLengthBits;
			}
			set
			{
				this.keyLengthBits = value;
				int num = this.keyLengthBits;
				if (num != 128)
				{
					if (num != 192)
					{
						if (num == 256)
						{
							this.strength = 3;
						}
					}
					else
					{
						this.strength = 2;
					}
				}
				else
				{
					this.strength = 1;
				}
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0002B4AC File Offset: 0x0002A4AC
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0002B4B4 File Offset: 0x0002A4B4
		public ushort CompressionMethod
		{
			get
			{
				return this.compressionMethod;
			}
			set
			{
				this.compressionMethod = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002B4C3 File Offset: 0x0002A4C3
		private static ushort GetFieldDataBytesLength()
		{
			return 7;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002B4C8 File Offset: 0x0002A4C8
		private byte[] GetFieldDataBytes()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.versionNumber);
			binaryWriter.Write(this.vendorID);
			binaryWriter.Write(this.strength);
			binaryWriter.Write(this.compressionMethod);
			return memoryStream.ToArray();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002B518 File Offset: 0x0002A518
		private void UpdateBaseClassFields()
		{
			this.DataSize = AESExtraFieldData.GetFieldDataBytesLength();
			this.Data = this.GetFieldDataBytes();
		}

		// Token: 0x040003BE RID: 958
		private ushort versionNumber;

		// Token: 0x040003BF RID: 959
		private int keyLengthBits;

		// Token: 0x040003C0 RID: 960
		private byte strength;

		// Token: 0x040003C1 RID: 961
		private ushort compressionMethod;

		// Token: 0x040003C2 RID: 962
		private readonly byte[] vendorID;
	}
}
