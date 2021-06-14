using System;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000019 RID: 25
	public interface IPacket
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A3 RID: 163
		// (set) Token: 0x060000A4 RID: 164
		byte Type { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A5 RID: 165
		int HeaderSize { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A6 RID: 166
		// (set) Token: 0x060000A7 RID: 167
		int BodySize { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A8 RID: 168
		// (set) Token: 0x060000A9 RID: 169
		byte Checksum { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AA RID: 170
		// (set) Token: 0x060000AB RID: 171
		byte Command { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AC RID: 172
		// (set) Token: 0x060000AD RID: 173
		bool IsValid { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AE RID: 174
		// (set) Token: 0x060000AF RID: 175
		FStatus Status { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B0 RID: 176
		// (set) Token: 0x060000B1 RID: 177
		int Length { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B2 RID: 178
		// (set) Token: 0x060000B3 RID: 179
		byte[] RawPacket { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B4 RID: 180
		string RawPacketAsString { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B5 RID: 181
		// (set) Token: 0x060000B6 RID: 182
		DateTime ReceiveTimeStamp { get; set; }

		// Token: 0x060000B7 RID: 183
		void Reset();

		// Token: 0x060000B8 RID: 184
		void Copy(IPacket packet);
	}
}
