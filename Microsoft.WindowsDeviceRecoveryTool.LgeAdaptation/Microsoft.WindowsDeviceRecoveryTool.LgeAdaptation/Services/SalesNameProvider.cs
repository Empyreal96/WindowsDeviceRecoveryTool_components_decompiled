using System;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;

namespace Microsoft.WindowsDeviceRecoveryTool.LgeAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	public class SalesNameProvider : BaseSalesNameProvider
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002ABC File Offset: 0x00000CBC
		public override string NameForVidPid(string vid, string pid)
		{
			string result;
			if (string.Compare(vid, "1004", StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(pid, "627E", StringComparison.OrdinalIgnoreCase) == 0)
			{
				result = "LG Lancet";
			}
			else
			{
				result = base.NameForVidPid(vid, pid);
			}
			return result;
		}
	}
}
