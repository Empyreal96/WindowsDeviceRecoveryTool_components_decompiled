using System;
using System.Collections.Generic;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000F RID: 15
	public class VidPidPairEqualityComparer : IEqualityComparer<VidPidPair>
	{
		// Token: 0x0600003A RID: 58 RVA: 0x000026A8 File Offset: 0x000008A8
		public bool Equals(VidPidPair x, VidPidPair y)
		{
			return (x == null && y == null) || (!(x == null) && !(y == null) && string.Equals(x.Pid, y.Pid, StringComparison.InvariantCultureIgnoreCase) && string.Equals(x.Vid, y.Vid, StringComparison.CurrentCultureIgnoreCase));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002708 File Offset: 0x00000908
		public int GetHashCode(VidPidPair obj)
		{
			string text = obj.Pid + obj.Vid;
			return text.GetHashCode();
		}
	}
}
