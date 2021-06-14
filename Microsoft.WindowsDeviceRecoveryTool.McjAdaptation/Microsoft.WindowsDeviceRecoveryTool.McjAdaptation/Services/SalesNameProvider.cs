using System;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Services
{
	// Token: 0x02000007 RID: 7
	public class SalesNameProvider : BaseSalesNameProvider
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000031F4 File Offset: 0x000013F4
		public override string NameForVidPid(string vid, string pid)
		{
			return base.NameForVidPid(vid, pid);
		}
	}
}
