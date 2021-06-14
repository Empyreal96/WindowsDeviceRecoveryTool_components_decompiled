using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200000A RID: 10
	[Guid("71A8CA8E-ED31-4C25-8CFF-689C40E6946E")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class FlashingManager : IFlashingManager
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000245B File Offset: 0x0000065B
		public bool Start()
		{
			FFUManager.Start();
			return true;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002463 File Offset: 0x00000663
		public bool Stop()
		{
			FFUManager.Stop();
			return true;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000246C File Offset: 0x0000066C
		public bool GetFlashableDevices(ref IEnumerator result)
		{
			ICollection<IFFUDevice> collection = new List<IFFUDevice>();
			FFUManager.GetFlashableDevices(ref collection);
			if (collection.Count == 0)
			{
				collection = null;
				return false;
			}
			result = new FlashableDeviceCollection(collection);
			return true;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000249C File Offset: 0x0000069C
		public IFlashableDevice GetFlashableDevice(string instancePath, bool enableFallback)
		{
			IFFUDevice flashableDevice = FFUManager.GetFlashableDevice(instancePath, enableFallback);
			if (flashableDevice == null)
			{
				return null;
			}
			return new FlashableDevice(flashableDevice);
		}
	}
}
