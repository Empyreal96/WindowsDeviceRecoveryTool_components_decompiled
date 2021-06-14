using System;
using ClickerUtilityLibrary.DataModel;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x02000021 RID: 33
	public interface IProtocol
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F6 RID: 246
		int StartOfPacketNumBytes { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F7 RID: 247
		int MaxPacketSize { get; }

		// Token: 0x060000F8 RID: 248
		IPacket CreateNewPacket();

		// Token: 0x060000F9 RID: 249
		void ParseHeader(IPacket packet);

		// Token: 0x060000FA RID: 250
		bool IsStartOfPacket(byte[] value);

		// Token: 0x060000FB RID: 251
		bool IsPacketTypeValid(IPacket packet);

		// Token: 0x060000FC RID: 252
		bool IsHeaderValid(IPacket packet);

		// Token: 0x060000FD RID: 253
		bool IsPacketValid(IPacket packet);

		// Token: 0x060000FE RID: 254
		void ParseCommandResponse(IPacket packet);
	}
}
