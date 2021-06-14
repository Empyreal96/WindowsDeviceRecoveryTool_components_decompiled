using System;
using System.Text;
using ClickerUtilityLibrary.DataModel;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x0200001E RID: 30
	public class AppProtocol : IProtocol
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000619B File Offset: 0x0000439B
		// (set) Token: 0x060000CC RID: 204 RVA: 0x000061A3 File Offset: 0x000043A3
		public int StartOfPacketNumBytes { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000061AC File Offset: 0x000043AC
		// (set) Token: 0x060000CE RID: 206 RVA: 0x000061B4 File Offset: 0x000043B4
		public int MaxPacketSize { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000061C0 File Offset: 0x000043C0
		public static AppProtocol Instance
		{
			get
			{
				bool flag = AppProtocol.instance == null;
				if (flag)
				{
					object obj = AppProtocol.syncRoot;
					lock (obj)
					{
						AppProtocol.instance = new AppProtocol();
					}
				}
				return AppProtocol.instance;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006228 File Offset: 0x00004428
		private AppProtocol()
		{
			this.MaxPacketSize = 4096;
			this.StartOfPacketNumBytes = 2;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006248 File Offset: 0x00004448
		private void FillHeader(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.RawPacket[0] = 80;
				packet.RawPacket[1] = 240;
				packet.RawPacket[2] = packet.Type;
				packet.RawPacket[3] = (byte)packet.BodySize;
				packet.RawPacket[4] = (byte)(packet.BodySize >> 8);
				packet.Length = packet.BodySize + 5;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000062B8 File Offset: 0x000044B8
		public IPacket CreateNewPacket()
		{
			return new FPacket(5, this.MaxPacketSize);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000062D8 File Offset: 0x000044D8
		public void ParseHeader(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = packet.RawPacket[2];
				packet.BodySize = (int)((ushort)(((int)packet.RawPacket[4] << 8) + (int)packet.RawPacket[3]));
				packet.Length = packet.BodySize + 5;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000632C File Offset: 0x0000452C
		public bool IsStartOfPacket(byte[] value)
		{
			bool flag = value == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = value.Length < 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = value[0] == 80 && value[1] == 240;
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006378 File Offset: 0x00004578
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
				bool flag2 = packet.Type > 4;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000063AC File Offset: 0x000045AC
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
					bool flag4 = packet.BodySize > 4096;
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

		// Token: 0x060000D7 RID: 215 RVA: 0x0000641C File Offset: 0x0000461C
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
				packet.ReceiveTimeStamp = DateTime.Now;
				packet.IsValid = this.IsHeaderValid(packet);
				result = packet.IsValid;
			}
			return result;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000645B File Offset: 0x0000465B
		public void ParseCommandResponse(IPacket packet)
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006460 File Offset: 0x00004660
		public void FormResetCommandPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "rst");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006498 File Offset: 0x00004698
		public void FormActivateShipModeCommandPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "clrum");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000064D0 File Offset: 0x000046D0
		public void FormGetFwVersionCommandPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "ver");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006508 File Offset: 0x00004708
		public void FormGetBoardIdCommandPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "hwid");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006540 File Offset: 0x00004740
		public void FormGetVitalProductDataCommandPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "vpd");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006578 File Offset: 0x00004778
		public void FormGetFirmwareInformationPacket(IPacket packet)
		{
			bool flag = packet == null;
			if (!flag)
			{
				packet.Type = 0;
				this.FillString(packet, "fw_cfg");
				this.FillHeader(packet);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000065B0 File Offset: 0x000047B0
		private void FillString(IPacket packet, string str)
		{
			bool flag = packet == null;
			if (!flag)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(str);
				Array.Copy(bytes, 0, packet.RawPacket, 5, bytes.Length);
				packet.BodySize = (int)((ushort)(bytes.Length + 1));
				packet.RawPacket[5 + bytes.Length] = 0;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006604 File Offset: 0x00004804
		public string GetBodyAsString(IPacket packet)
		{
			bool flag = packet == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				byte[] array = new byte[packet.BodySize];
				Array.Copy(packet.RawPacket, 5, array, 0, packet.BodySize);
				string @string = asciiencoding.GetString(array);
				result = @string;
			}
			return result;
		}

		// Token: 0x040000C5 RID: 197
		private const int MAX_PACKET_SIZE = 4096;

		// Token: 0x040000C6 RID: 198
		private const byte StartofPacketByte0 = 80;

		// Token: 0x040000C7 RID: 199
		private const byte StartOfPacketByte1 = 240;

		// Token: 0x040000C8 RID: 200
		private const int HeaderSize = 5;

		// Token: 0x040000C9 RID: 201
		private static volatile AppProtocol instance;

		// Token: 0x040000CA RID: 202
		private static readonly object syncRoot = new object();

		// Token: 0x02000046 RID: 70
		private enum PACKETTYPE : byte
		{
			// Token: 0x04000195 RID: 405
			CLI_COMMAND_PACKET,
			// Token: 0x04000196 RID: 406
			CLI_RESPONSE_PACKET,
			// Token: 0x04000197 RID: 407
			INIT_FLASHING_MODE_PACKET,
			// Token: 0x04000198 RID: 408
			GET_MAC_ADDRESS_REQUEST_PACKET,
			// Token: 0x04000199 RID: 409
			GET_MAC_ADDRESS_RESPONSE_PACKET
		}
	}
}
