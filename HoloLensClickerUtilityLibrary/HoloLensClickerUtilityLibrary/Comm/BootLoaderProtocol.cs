using System;
using System.Collections.Generic;
using ClickerUtilityLibrary.DataModel;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x02000022 RID: 34
	public class BootLoaderProtocol : IProtocol
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006976 File Offset: 0x00004B76
		// (set) Token: 0x06000100 RID: 256 RVA: 0x0000697E File Offset: 0x00004B7E
		public int StartOfPacketNumBytes { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006987 File Offset: 0x00004B87
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000698F File Offset: 0x00004B8F
		public int MaxPacketSize { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006998 File Offset: 0x00004B98
		public static BootLoaderProtocol Instance
		{
			get
			{
				bool flag = BootLoaderProtocol.instance == null;
				if (flag)
				{
					object obj = BootLoaderProtocol.syncRoot;
					lock (obj)
					{
						BootLoaderProtocol.instance = new BootLoaderProtocol();
					}
				}
				return BootLoaderProtocol.instance;
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00006A00 File Offset: 0x00004C00
		private BootLoaderProtocol()
		{
			this.MaxPacketSize = 40000;
			this.StartOfPacketNumBytes = 1;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006A20 File Offset: 0x00004C20
		public byte CalculateChecksum(IPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			return this.CalculateChecksum(packet.RawPacket, packet.HeaderSize, packet.BodySize);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006A60 File Offset: 0x00004C60
		public byte CalculateChecksum(byte[] buffer, int start, int length)
		{
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = start >= buffer.Length || start + length > buffer.Length;
			if (flag2)
			{
				throw new ArgumentException("The start and length parameters are invalid for the size of the buffer.");
			}
			byte b = buffer[start];
			int num;
			for (int i = start + 1; i < length + start; i = num + 1)
			{
				b ^= buffer[i];
				num = i;
			}
			return b;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006AD4 File Offset: 0x00004CD4
		public void FormDataPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			FCommand fcommand = CommandDictionary.Instance[4];
			int num = 8;
			bool flag2 = fcommand.Args != null;
			if (flag2)
			{
				foreach (DataElement dataElement in fcommand.Args)
				{
					packet.BodySize += dataElement.Length + 2;
				}
			}
			bool flag3 = packet.BodySize == 0;
			if (!flag3)
			{
				packet.Length = packet.BodySize + 8;
				packet.RawPacket = new byte[packet.Length];
				packet.Type = 4;
				packet.RawPacket[0] = 85;
				packet.RawPacket[1] = 4;
				packet.RawPacket[3] = 4;
				bool flag4 = fcommand.Args != null;
				if (flag4)
				{
					foreach (DataElement dataElement2 in fcommand.Args)
					{
						Array.Copy(BitConverter.GetBytes((int)dataElement2.Type), 0, packet.RawPacket, num, 2);
						num += 2;
						Array.Copy(dataElement2.GetRawData(), 0, packet.RawPacket, num, dataElement2.Length);
						num += dataElement2.Length;
					}
				}
				packet.Length = num;
				packet.BodySize = num - 8;
				packet.Checksum = this.CalculateChecksum(packet.RawPacket, 8, packet.BodySize);
				packet.RawPacket[2] = packet.Checksum;
				Array.Copy(BitConverter.GetBytes(packet.BodySize), 0, packet.RawPacket, 4, 4);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006CBC File Offset: 0x00004EBC
		public void FormCommandPacket(IPacket packet, byte commandcode)
		{
			FCommand fcommand = CommandDictionary.Instance[(int)commandcode];
			int num = 8;
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			packet.Type = 0;
			packet.RawPacket[0] = 85;
			packet.RawPacket[1] = packet.Type;
			packet.RawPacket[3] = commandcode;
			bool flag2 = fcommand.Args != null;
			if (flag2)
			{
				foreach (DataElement dataElement in fcommand.Args)
				{
					Array.Copy(BitConverter.GetBytes((int)dataElement.Type), 0, packet.RawPacket, num, 2);
					num += 2;
					Array.Copy(dataElement.GetRawData(), 0, packet.RawPacket, num, dataElement.Length);
					num += dataElement.Length;
				}
			}
			packet.Length = num;
			packet.BodySize = num - 8;
			packet.Checksum = this.CalculateChecksum(packet.RawPacket, 8, packet.BodySize);
			packet.RawPacket[2] = packet.Checksum;
			Array.Copy(BitConverter.GetBytes(packet.BodySize), 0, packet.RawPacket, 4, 4);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006E04 File Offset: 0x00005004
		public void ParseCommandResponse(IPacket packet)
		{
			int command = (int)packet.Command;
			int num = 8;
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			bool flag2 = !CommandDictionary.Instance.ContainsKey(command);
			if (flag2)
			{
				packet.Status = FStatus.COMM_H_BAD_COMMANDCODE;
			}
			else
			{
				bool flag3 = packet.BodySize < 6;
				if (flag3)
				{
					packet.Status = FStatus.COMM_H_RESP_PACKET_MISSING_STATUS;
				}
				else
				{
					bool flag4 = packet.Type != 1;
					if (flag4)
					{
						packet.Status = FStatus.COMM_H_BAD_CMD_ACK_TYPE;
					}
					else
					{
						int cDEIndex;
						Predicate<DataElement> <>9__0;
						for (;;)
						{
							cDEIndex = (int)BitConverter.ToUInt16(packet.RawPacket, num);
							List<DataElement> responseArgs = CommandDictionary.Instance[command].ResponseArgs;
							Predicate<DataElement> match;
							if ((match = <>9__0) == null)
							{
								match = (<>9__0 = ((DataElement i) => i.Type == (DataElementType)cDEIndex));
							}
							DataElement dataElement = responseArgs.Find(match);
							bool flag5 = dataElement == null;
							if (flag5)
							{
								break;
							}
							num += 2;
							bool flag6 = dataElement.DataType == DataType.DATA_TYPE_BINARYSTREAM;
							if (flag6)
							{
								ushort length = BitConverter.ToUInt16(packet.RawPacket, num);
								dataElement.Length = (int)length;
								num += 2;
							}
							Array.Copy(packet.RawPacket, num, dataElement.GetRawData(), 0, dataElement.Length);
							num += dataElement.Length;
							if (num >= packet.Length)
							{
								goto Block_8;
							}
						}
						packet.Status = FStatus.COMM_H_CMD_RESP_PARSE_DE_ERROR;
						return;
						Block_8:
						packet.Status = FStatus.FSTATUS_OK;
					}
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006F7C File Offset: 0x0000517C
		public IPacket CreateNewPacket()
		{
			return new FPacket(8, this.MaxPacketSize);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006F9C File Offset: 0x0000519C
		public void ParseHeader(IPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			packet.Type = packet.RawPacket[1];
			packet.Checksum = packet.RawPacket[2];
			packet.BodySize = BitConverter.ToInt32(packet.RawPacket, 4);
			packet.Length = packet.BodySize + 8;
			packet.Command = packet.RawPacket[3];
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007010 File Offset: 0x00005210
		public bool IsStartOfPacket(byte[] value)
		{
			bool flag = value == null;
			if (flag)
			{
				throw new ArgumentNullException("value");
			}
			bool flag2 = value.Length < 1;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = value[0] == 85;
				result = flag3;
			}
			return result;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007058 File Offset: 0x00005258
		public bool IsPacketTypeValid(IPacket packet)
		{
			bool flag = packet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = packet.Type > 3;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000708C File Offset: 0x0000528C
		public bool IsHeaderValid(IPacket packet)
		{
			bool flag = packet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.ParseHeader(packet);
				bool flag2 = !this.IsPacketTypeValid(packet);
				bool flag3;
				if (flag2)
				{
					packet.Status = FStatus.COMM_H_BAD_PACKET_TYPE;
					flag3 = false;
				}
				else
				{
					bool flag4 = packet.BodySize > this.MaxPacketSize;
					if (flag4)
					{
						packet.Status = FStatus.COMM_H_BAD_BODY_SIZE;
						flag3 = false;
					}
					else
					{
						flag3 = true;
					}
				}
				result = flag3;
			}
			return result;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000070FC File Offset: 0x000052FC
		public bool IsPacketValid(IPacket packet)
		{
			bool flag = packet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.IsHeaderValid(packet);
				packet.ReceiveTimeStamp = DateTime.Now;
				bool flag2 = this.CalculateChecksum(packet) != packet.Checksum;
				if (flag2)
				{
					packet.Status = FStatus.COMM_H_BAD_CHECKSUM;
					result = false;
				}
				else
				{
					packet.IsValid = true;
					result = packet.IsValid;
				}
			}
			return result;
		}

		// Token: 0x040000D7 RID: 215
		private static volatile BootLoaderProtocol instance;

		// Token: 0x040000D8 RID: 216
		private static readonly object syncRoot = new object();

		// Token: 0x02000047 RID: 71
		private static class PACKETDEF
		{
			// Token: 0x0400019A RID: 410
			internal const byte SOP = 85;

			// Token: 0x0400019B RID: 411
			internal const int HeaderSize = 8;

			// Token: 0x0400019C RID: 412
			internal const int DEIndexSize = 2;

			// Token: 0x0400019D RID: 413
			internal const int MAX_PACKET_SIZE = 40000;
		}

		// Token: 0x02000048 RID: 72
		private static class PACKETINDEX
		{
			// Token: 0x0400019E RID: 414
			public const int INDEX_SOP = 0;

			// Token: 0x0400019F RID: 415
			public const int INDEX_TYPE = 1;

			// Token: 0x040001A0 RID: 416
			public const int INDEX_CHECKSUM = 2;

			// Token: 0x040001A1 RID: 417
			public const int INDEX_CMD = 3;

			// Token: 0x040001A2 RID: 418
			public const int INDEX_BODYSIZE = 4;

			// Token: 0x040001A3 RID: 419
			public const int INDEX_PAYLOAD = 8;

			// Token: 0x040001A4 RID: 420
			public const int INDEX_ACK_STATUS = 8;

			// Token: 0x040001A5 RID: 421
			public const int INDEX_ACK_ARG = 14;
		}

		// Token: 0x02000049 RID: 73
		public enum PACKETTYPE : byte
		{
			// Token: 0x040001A7 RID: 423
			PT_CMD,
			// Token: 0x040001A8 RID: 424
			PT_CMD_ACK,
			// Token: 0x040001A9 RID: 425
			PT_DATA,
			// Token: 0x040001AA RID: 426
			PT_DATA_ACK,
			// Token: 0x040001AB RID: 427
			PT_BINARY_DOWNLOAD,
			// Token: 0x040001AC RID: 428
			PT_BINARY_UPLOAD,
			// Token: 0x040001AD RID: 429
			MAX_PACKET_TYPE
		}
	}
}
