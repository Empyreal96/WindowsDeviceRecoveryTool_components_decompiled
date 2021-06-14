using System;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000004 RID: 4
	internal interface IHandleThreadException
	{
		// Token: 0x06000007 RID: 7
		bool TryHandleThreadException(Exception exception);
	}
}
