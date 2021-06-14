using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000004 RID: 4
	[ComVisible(true)]
	[Guid("4EE1152F-246E-4BA3-84D1-2B6C96170E18")]
	public interface IFlashableDevice
	{
		// Token: 0x0600000C RID: 12
		[DispId(1)]
		string GetFriendlyName();

		// Token: 0x0600000D RID: 13
		[DispId(2)]
		string GetUniqueIDStr();

		// Token: 0x0600000E RID: 14
		[DispId(3)]
		string GetSerialNumberStr();

		// Token: 0x0600000F RID: 15
		[DispId(4)]
		bool FlashFFU(string filePath);
	}
}
