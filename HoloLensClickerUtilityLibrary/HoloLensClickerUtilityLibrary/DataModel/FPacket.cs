using System;
using System.Globalization;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000013 RID: 19
	public class FPacket : IPacket
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00005158 File Offset: 0x00003358
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00005160 File Offset: 0x00003360
		public byte Type { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00005169 File Offset: 0x00003369
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00005171 File Offset: 0x00003371
		public int HeaderSize { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000517A File Offset: 0x0000337A
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00005182 File Offset: 0x00003382
		private int MaxPacketSize { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000518B File Offset: 0x0000338B
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00005193 File Offset: 0x00003393
		public int BodySize { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000519C File Offset: 0x0000339C
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000051A4 File Offset: 0x000033A4
		public byte Checksum { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000051AD File Offset: 0x000033AD
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000051B5 File Offset: 0x000033B5
		public byte Command { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000051BE File Offset: 0x000033BE
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000051C6 File Offset: 0x000033C6
		public bool IsValid { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000051CF File Offset: 0x000033CF
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000051D7 File Offset: 0x000033D7
		public FStatus Status { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000051E0 File Offset: 0x000033E0
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000051E8 File Offset: 0x000033E8
		public int Length { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000051F1 File Offset: 0x000033F1
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000051F9 File Offset: 0x000033F9
		public byte[] RawPacket { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005204 File Offset: 0x00003404
		public string RawPacketAsString
		{
			get
			{
				string text = "";
				int num;
				for (int i = 0; i < this.Length; i = num + 1)
				{
					text = text + "0x" + this.RawPacket[i].ToString("X2", CultureInfo.InvariantCulture) + " ";
					num = i;
				}
				return text;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00005264 File Offset: 0x00003464
		// (set) Token: 0x06000086 RID: 134 RVA: 0x0000526C File Offset: 0x0000346C
		public DateTime ReceiveTimeStamp { get; set; }

		// Token: 0x06000087 RID: 135 RVA: 0x00005275 File Offset: 0x00003475
		public FPacket(int headerSize, int maxPacketSize)
		{
			this.HeaderSize = headerSize;
			this.MaxPacketSize = maxPacketSize;
			this.Reset();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005298 File Offset: 0x00003498
		public void Reset()
		{
			bool flag = this.RawPacket == null;
			if (flag)
			{
				this.RawPacket = new byte[this.MaxPacketSize];
			}
			else
			{
				Array.Clear(this.RawPacket, 0, this.MaxPacketSize);
			}
			this.Length = 0;
			this.BodySize = 0;
			this.Checksum = 0;
			this.Command = 0;
			this.Status = FStatus.FSTATUS_OK;
			this.IsValid = false;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005310 File Offset: 0x00003510
		public void Copy(IPacket packet)
		{
			bool flag = packet == null;
			if (flag)
			{
				throw new ArgumentNullException("packet");
			}
			this.Length = packet.Length;
			this.BodySize = packet.BodySize;
			this.Type = packet.Type;
			this.Status = packet.Status;
			Array.Copy(packet.RawPacket, this.RawPacket, this.Length);
			this.IsValid = packet.IsValid;
			this.Command = packet.Command;
			this.ReceiveTimeStamp = packet.ReceiveTimeStamp;
		}
	}
}
