using System;
using System.IO;

namespace FFUComponents
{
	// Token: 0x02000049 RID: 73
	internal class PacketConstructor : IDisposable
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x000045D9 File Offset: 0x000027D9
		public PacketConstructor()
		{
			this.packetNumber = 0;
			this.PacketDataSize = (long)PacketConstructor.cbDefaultData;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000045F4 File Offset: 0x000027F4
		public static long DefaultPacketDataSize
		{
			get
			{
				return (long)PacketConstructor.cbDefaultData;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000045FC File Offset: 0x000027FC
		public static long MaxPacketDataSize
		{
			get
			{
				return (long)PacketConstructor.cbMaxData;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004604 File Offset: 0x00002804
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000460C File Offset: 0x0000280C
		public Stream DataStream { internal get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004615 File Offset: 0x00002815
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x0000461D File Offset: 0x0000281D
		public long PacketDataSize { internal get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004626 File Offset: 0x00002826
		public long Position
		{
			get
			{
				return this.DataStream.Position;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004633 File Offset: 0x00002833
		public long Length
		{
			get
			{
				return this.DataStream.Length;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004640 File Offset: 0x00002840
		public long RemainingData
		{
			get
			{
				return this.DataStream.Length - this.DataStream.Position;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004659 File Offset: 0x00002859
		public void Reset()
		{
			this.DataStream.Seek(0L, SeekOrigin.Begin);
			this.packetNumber = 0;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004674 File Offset: 0x00002874
		public unsafe byte[] GetNextPacket(bool optimize)
		{
			byte[] array = new byte[this.PacketDataSize + 12L];
			Array.Clear(array, 0, array.Length);
			int value = this.DataStream.Read(array, 0, (int)this.PacketDataSize);
			int num = (int)this.PacketDataSize;
			byte[] bytes = BitConverter.GetBytes(value);
			bytes.CopyTo(array, num);
			num += bytes.Length;
			bytes = BitConverter.GetBytes(this.packetNumber++);
			bytes.CopyTo(array, num);
			num += bytes.Length;
			uint value2 = 0U;
			if (!optimize)
			{
				fixed (byte* ptr = array)
				{
					value2 = Crc32.GetChecksum(0U, ptr, (uint)(array.Length - 4));
				}
			}
			bytes = BitConverter.GetBytes(value2);
			bytes.CopyTo(array, num);
			return array;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000473C File Offset: 0x0000293C
		public byte[] GetZeroLengthPacket()
		{
			this.DataStream.Seek(0L, SeekOrigin.End);
			return this.GetNextPacket(false);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004754 File Offset: 0x00002954
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000475D File Offset: 0x0000295D
		private void Dispose(bool fDisposing)
		{
			if (fDisposing && this.DataStream != null)
			{
				this.DataStream.Dispose();
				this.DataStream = null;
			}
		}

		// Token: 0x0400010D RID: 269
		private static readonly int cbDefaultData = 262144;

		// Token: 0x0400010E RID: 270
		private static readonly int cbMaxData = 8388608;

		// Token: 0x0400010F RID: 271
		private int packetNumber;
	}
}
