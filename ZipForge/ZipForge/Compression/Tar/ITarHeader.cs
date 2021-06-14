using System;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x0200005E RID: 94
	internal interface ITarHeader
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003E4 RID: 996
		// (set) Token: 0x060003E5 RID: 997
		string FileName { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003E6 RID: 998
		// (set) Token: 0x060003E7 RID: 999
		int GroupId { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003E8 RID: 1000
		// (set) Token: 0x060003E9 RID: 1001
		string GroupName { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003EA RID: 1002
		int HeaderSize { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003EB RID: 1003
		// (set) Token: 0x060003EC RID: 1004
		DateTime LastModification { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003ED RID: 1005
		// (set) Token: 0x060003EE RID: 1006
		int Mode { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003EF RID: 1007
		// (set) Token: 0x060003F0 RID: 1008
		long SizeInBytes { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003F1 RID: 1009
		// (set) Token: 0x060003F2 RID: 1010
		int UserId { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003F3 RID: 1011
		// (set) Token: 0x060003F4 RID: 1012
		string UserName { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003F5 RID: 1013
		// (set) Token: 0x060003F6 RID: 1014
		char TypeFlag { get; set; }
	}
}
