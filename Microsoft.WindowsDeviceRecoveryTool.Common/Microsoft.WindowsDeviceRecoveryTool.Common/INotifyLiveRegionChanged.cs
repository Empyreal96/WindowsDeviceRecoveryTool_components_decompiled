using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000007 RID: 7
	public interface INotifyLiveRegionChanged
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000017 RID: 23
		// (remove) Token: 0x06000018 RID: 24
		event EventHandler LiveRegionChanged;
	}
}
