using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x0200000A RID: 10
	public class SalesNameProvider : BaseSalesNameProvider
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00006388 File Offset: 0x00004588
		public override string NameForVidPid(string vid, string pid)
		{
			string result;
			if (string.Compare(vid, "0421", StringComparison.OrdinalIgnoreCase) == 0 && (string.Compare(pid, "066E", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(pid, "0714", StringComparison.OrdinalIgnoreCase) == 0))
			{
				result = "DeviceInUefiMode";
			}
			else
			{
				result = base.NameForVidPid(vid, pid);
			}
			return result;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000063E8 File Offset: 0x000045E8
		public override string NameForTypeDesignator(string typeDesignator)
		{
			string result;
			if (this.lumia1020List.Contains(typeDesignator.ToUpper()))
			{
				result = "Nokia Lumia 1020";
			}
			else
			{
				result = base.NameForTypeDesignator(typeDesignator);
			}
			return result;
		}

		// Token: 0x0400003A RID: 58
		private readonly List<string> lumia1020List = new List<string>
		{
			"RM-875",
			"RM-876",
			"RM-877"
		};
	}
}
