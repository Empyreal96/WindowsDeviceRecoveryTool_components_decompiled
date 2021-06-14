using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000008 RID: 8
	[ComVisible(true)]
	[Guid("FAD741FC-3AEA-4FAB-9B8D-CBBF5E265D1B")]
	public interface IFlashingManager
	{
		// Token: 0x0600001E RID: 30
		[DispId(1)]
		bool Start();

		// Token: 0x0600001F RID: 31
		[DispId(2)]
		bool Stop();

		// Token: 0x06000020 RID: 32
		[DispId(3)]
		bool GetFlashableDevices(ref IEnumerator result);

		// Token: 0x06000021 RID: 33
		[DispId(4)]
		IFlashableDevice GetFlashableDevice(string instancePath, bool enableFallback);
	}
}
