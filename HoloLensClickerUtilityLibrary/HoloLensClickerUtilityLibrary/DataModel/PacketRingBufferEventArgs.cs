using System;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x0200001B RID: 27
	internal class PacketRingBufferEventArgs : EventArgs
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005B14 File Offset: 0x00003D14
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00005B1C File Offset: 0x00003D1C
		internal PacketRingBufferEventType Type { get; set; }
	}
}
