using System;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation.Services
{
	// Token: 0x0200000C RID: 12
	public class FawkesSalesNameProvider : BaseSalesNameProvider
	{
		// Token: 0x06000068 RID: 104 RVA: 0x000037B6 File Offset: 0x000019B6
		public override string NameForString(string text)
		{
			return base.NameForString(text);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000037BF File Offset: 0x000019BF
		public override string NameForTypeDesignator(string typeDesignator)
		{
			return base.NameForTypeDesignator(typeDesignator);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000037C8 File Offset: 0x000019C8
		public override string NameForVidPid(string vid, string pid)
		{
			return base.NameForVidPid(vid, pid);
		}
	}
}
