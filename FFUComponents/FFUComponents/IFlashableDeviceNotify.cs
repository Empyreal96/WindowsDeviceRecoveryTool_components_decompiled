using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000005 RID: 5
	[ComVisible(true)]
	[Guid("323459AA-B365-44FE-A763-AEACCBCA8880")]
	public interface IFlashableDeviceNotify
	{
		// Token: 0x06000010 RID: 16
		[DispId(1)]
		void Progress(long position, long length);
	}
}
